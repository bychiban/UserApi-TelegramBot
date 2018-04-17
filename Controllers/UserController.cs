using System.Collections.Generic; 
using Microsoft.AspNetCore.Mvc; 
using UserApi.Models; 
using System.Linq; 

namespace UserApi.Controllers {
    [Route("api/[controller]")]
    public class UserController:Controller {
        private readonly UserContext _context; 

        public UserController(UserContext context) {
            _context = context; 

            if (_context.UserItems.Count() == 0) {
                _context.UserItems.Add(new UserItem {FirstName = "Bob", LastName = "Panda", FathersName = "Pandian"}); 
                _context.SaveChanges(); 
            }
        }

        [HttpGet]
        public IEnumerable < UserItem > GetAll() {
            return _context.UserItems.ToList(); 
        }

        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult GetById(long id) {
            var item = _context.UserItems.FirstOrDefault(t => t.Id == id); 
            if (item == null) {
                return NotFound(); 
            }
            return new ObjectResult(item); 
        }
        [HttpPost]
        public IActionResult Create([FromBody] UserItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _context.UserItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetUser", new { id = item.Id }, item);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(long id) {
            var user = _context.UserItems.FirstOrDefault(t => t.Id == id); 
            if (user == null) {
                return NotFound(); 
            }

            _context.UserItems.Remove(user); 
            _context.SaveChanges(); 
            return new NoContentResult(); 
        }
    }
}