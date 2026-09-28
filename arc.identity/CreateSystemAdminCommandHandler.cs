//using arc.app;
//using arc.domain.Security.User;
//using Microsoft.AspNetCore.Identity;
//using Newtonsoft.Json;
//using System.Threading.Tasks;

//namespace arc.identity
//{
//    public class CreateSystemAdminCommandHandler : ICreateSystemAdminCommandHandler
//    {
//        private readonly IGenericRepository _genericRepository;

//        public CreateSystemAdminCommandHandler(IGenericRepository genericRepository)
//        {
//            _genericRepository = genericRepository;
//        }

//        public async Task Create(CreateSystemAdminCommand command)
//        {
//            var hasher = new PasswordHasher<User>();
//            var newUser = new User("SystemAdmin", "System", "", "Yes");
//            newUser.SetPassword(hasher.HashPassword(newUser, command.Password));

//            var message = JsonConvert.SerializeObject(newUser);
//            _genericRepository.AddConfiguration("user");
//            await _genericRepository.AddAsync(message);
//        }
//    }
//}

