using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UserManagementAPI.Data;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        // GET: api/users?page=1&pageSize=10
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var users = UserStore.Users
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(users);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public ActionResult<User> GetById(int id)
        {
            var user = FindUser(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public ActionResult<User> Create([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (UserStore.Users.Any(u => u.Email == user.Email))
                return Conflict("Ya existe un usuario con ese correo.");

            user.Id = UserStore.GetNextId();
            UserStore.Users.Add(user);

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] User updatedUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = FindUser(id);
            if (user == null) return NotFound();

            if (UserStore.Users.Any(u => u.Email == updatedUser.Email && u.Id != id))
                return Conflict("Otro usuario ya tiene ese correo.");

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;

            return NoContent();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = FindUser(id);
            if (user == null) return NotFound();

            UserStore.Users.Remove(user);
            return NoContent();
        }

        // 🔍 Método privado para buscar usuario
        private User? FindUser(int id) =>
            UserStore.Users.FirstOrDefault(u => u.Id == id);
    }
}
