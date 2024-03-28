using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize] // Requires users to authenticate to access
    public class UsersController : BaseApiController
    {
        private readonly DataContext _context; // declare a field, readonly mean value be assigned only once and not changed afterwards
        public UsersController(DataContext context)
        {
            _context = context;
        }

        [AllowAnonymous] // override or bypass access requested by [Authorize] and access without authentication
        [HttpGet] // endpoint /api/users
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        [HttpGet("{id}")] // endpoint /api/users/2
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}
