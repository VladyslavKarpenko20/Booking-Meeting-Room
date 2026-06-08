using Booking_Meeting_Rooms.Interface;
using Booking_Meeting_Rooms.AuthDto;
using Booking_Meeting_Rooms.Exceptions;
using Booking_Meeting_Rooms.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace Booking_Meeting_Rooms.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;


        public AuthServices(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }


        public async Task Registr(RegistrDto registrDto)
        {
            if (string.IsNullOrWhiteSpace(registrDto.Email) || string.IsNullOrWhiteSpace(registrDto.Password))
                throw new BadRequestExceptions("Invalid Data");

            var user = await _userRepository.GetUserByEmail(registrDto.Email);

            if (user != null)
                throw new ConflictExceptions("A user with this email already exists");

            var createUser = new User
            {
                Name = registrDto.Name,
                Email = registrDto.Email,
            };

            createUser.Password = _passwordHasher.HashPassword(createUser, registrDto.Password);

            await _userRepository.AddUser(createUser);
        }

        public async Task<string> Login(string Email , string Password)
        {

            var login = new LoginDto
            {
                Email = Email,
                Password = Password
            };

            var user = await _userRepository.GetUserByEmail(login.Email);


            if (user == null)
                throw new NotFoundExceptions("User Not Found");

            if (string.IsNullOrWhiteSpace(user.Password))
                throw new BadRequestExceptions("Invalid Data");

            var password = _passwordHasher.VerifyHashedPassword(user, user.Password, login.Password);

            if (password == PasswordVerificationResult.Failed)
                throw new BadRequestExceptions("Invalid Password Data");

            return GenerateAcsesToken(user);

        }

        public string GenerateAcsesToken(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Email))
                throw new BadRequestExceptions("Invalid Data");

            var secretKey = _configuration["JWTSetting:SecretKey"] ?? "1234";

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.Name, user.Name)
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));


            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken
            (
               issuer: "MyApp",
               audience: "User",
               claims: claims,
               expires: DateTime.UtcNow.AddHours(1),
               signingCredentials: credential

            );

            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();


            return tokenHandler.WriteToken(token);


        }

    }
}
