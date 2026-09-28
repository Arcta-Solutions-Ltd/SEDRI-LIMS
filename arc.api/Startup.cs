using arc.api.Controllers;
using arc.api.Services;
using arc.api.StartUpOptions;
using arc.app.Files;
using arc.common.Options;
using arc.data.Configuration;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace arc.api;

public class Startup
{
    private readonly IWebHostEnvironment _env;

    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        Configuration = configuration;
        _env = env;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSwagger();

        services.Configure<DataOptions>(Configuration.GetSection("ConnectionStrings"));
        services.Configure<QueueHashOptions>(Configuration.GetSection("QueueHash"));
        services.Configure<AuthenticationOptions>(Configuration.GetSection("Authentication"));

        services.AddControllers().AddApplicationPart(typeof(ConfigController).Assembly);

        services.AddIdentity(Configuration);

        services.AddArcLogging();
        services.AddMemoryCache();
        services.AddSingleton<ILoginThrottleService, LoginThrottleService>();

        services.AddSingleton<IFileService>(_ =>
        {
            var useCloudStorage = Configuration.GetValue<bool>("Files:UseCloudStorage");

            if (useCloudStorage)
            {
                var blobServiceUri = Configuration["Files:BlobServiceUri"];
                var containerName = Configuration["Files:ContainerName"];
                var connectionString = Configuration["Files:ConnectionString"]; // Optional

                if (string.IsNullOrWhiteSpace(blobServiceUri))
                    throw new InvalidOperationException("Files:BlobServiceUri is required when UseCloudStorage is true");
                if (string.IsNullOrWhiteSpace(containerName))
                    throw new InvalidOperationException("Files:ContainerName is required when UseCloudStorage is true");

                return new AzureFileService(blobServiceUri, containerName, connectionString);
            }
            else
            {
                var storageRoot = Configuration["Files:StorageRoot"];
                if (string.IsNullOrWhiteSpace(storageRoot))
                    throw new InvalidOperationException("Files:StorageRoot missing");
                return new FileService(storageRoot);
            }
        });

        services.AddCommon();
        services.AddFactories();
        services.AddSqlRepositories();
        services.AddHostedService<ExportScheduleBackgroundService>();
        services.AddMappers();
        services.AddAdapters();

        services.AddHsts(options =>
        {
            options.MaxAge = TimeSpan.FromDays(180);
            options.IncludeSubDomains = true;
            options.Preload = true;
        });

        // WAPT-007: Add CSRF protection for login endpoint
        services.AddAntiforgery(options =>
        {
            options.HeaderName = "X-CSRF-TOKEN";
            options.Cookie.Name = "X-CSRF-TOKEN-COOKIE";
            options.Cookie.HttpOnly = true;

            // For cross-origin requests (frontend on different port/protocol), cookies must use SameSite=None
            // Modern browsers require Secure=true with SameSite=None, so we use Secure in both dev and prod
            // The cookie will be set by HTTPS backend and sent back to HTTPS backend, even if frontend page is HTTP
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.None;
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
    {
        logger.LogInformation("ARC API started - application log file initialised");
        app.UseSwaggerView();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }

        app.UseMiddleware<ExceptionHandler>();

        // Security headers (values from appsettings.json SecurityHeaders section)
        var xFrameOptions = Configuration["SecurityHeaders:XFrameOptions"] ?? "SAMEORIGIN";
        var xContentTypeOptions = Configuration["SecurityHeaders:XContentTypeOptions"] ?? "nosniff";
        var referrerPolicy = Configuration["SecurityHeaders:ReferrerPolicy"] ?? "strict-origin-when-cross-origin";
        var permissionsPolicy = Configuration["SecurityHeaders:PermissionsPolicy"] ??
            "camera=(), microphone=(), geolocation=(), payment=(), usb=(), magnetometer=(), gyroscope=(), accelerometer=()";
        var csp = Configuration["SecurityHeaders:ContentSecurityPolicy"] ??
            "default-src 'none'; frame-ancestors 'none'; base-uri 'none'";

        app.Use(async (context, next) =>
        {
            // WAPT-005: Remove information disclosure headers
            context.Response.Headers.Remove("X-Powered-By");
            context.Response.Headers.Remove("Server");

            // Prevent clickjacking by controlling iframe embedding
            context.Response.Headers["X-Frame-Options"] = xFrameOptions;

            // Prevent MIME-type sniffing attacks
            context.Response.Headers["X-Content-Type-Options"] = xContentTypeOptions;

            // Control referrer information sent to external sites
            context.Response.Headers["Referrer-Policy"] = referrerPolicy;

            // Restrict access to browser features not used by the application
            context.Response.Headers["Permissions-Policy"] = permissionsPolicy;

            // Content Security Policy for API responses
            context.Response.Headers["Content-Security-Policy"] = csp;

            await next();
        });

        app.UseHttpsRedirection();

        app.UseRouting();

        var webclient = Configuration["Urls:webclient"];
        // WAPT-007: Allow credentials for CSRF token cookies
        app.UseCors(builder =>
           builder.WithOrigins(webclient)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials());

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
