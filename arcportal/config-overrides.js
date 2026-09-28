const CopyPlugin = require('copy-webpack-plugin');

module.exports = function override(config, env) {
    // Ensure development mode when running 'npm start'
    if (env === 'development') {
        config.mode = 'development';
        // Disable minification in development
        if (config.optimization) {
            config.optimization.minimize = false;
        }
    }
    
    // Add CopyPlugin without replacing existing plugins
    config.plugins = [
        ...config.plugins,
        new CopyPlugin({
            patterns: [
                {
                    from: 'node_modules/@fluentui/font-icons-mdl2/fonts',
                    to: 'static/fonts',
                },
            ],
        }),
    ];
    return config;
};
