using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")] // POST: api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExits(registerDto.Username))  return BadRequest("Username is taken");

            using var hmac = new HMACSHA512(); // The 'using' statement will release the resources of the HMACSHA512 object after it is used
            var user = new AppUser
            {
                UserName = registerDto.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)), // The ComputeHash method of the hmac object is used to calculate the hash value of a byte array representing the password in UTF-8 format.
                PasswordSalt = hmac.Key // Key is a property of the hmac object containing the key.
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == loginDto.Username); // SingleOrDefaultAsync is used to retrieve a single user based on a specified condition

            if (user == null) return Unauthorized("invalid username"); // response failed with status code 401 (Unauthorized)

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++) // compare password
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("invalid password"); 
            }
            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain).Url
            };
        }

        private async Task<bool> UserExits(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}
