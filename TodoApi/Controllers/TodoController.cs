using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : Controller
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;

            if(_context.TodoItems.Count() == 0)
            {
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.
                _context.TodoItems.Add(new TodoItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public ActionResult<List<TodoItem>> GetAll()
        {
            return _context.TodoItems.ToList();
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public ActionResult<TodoItem> GetById(long id)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }
        [HttpPost]
        public ActionResult create(TodoItem newItem)
        {
            _context.TodoItems.Add(newItem);
            _context.SaveChanges();

            return CreatedAtRoute("GetTodo", new { id = newItem.Id }, newItem);
        }
        [HttpPut("{id}")]
        public ActionResult Update(long id, TodoItem item)
        {
            var todo = _context.TodoItems.Find(id);

            if (todo == null) NotFound();
            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;

            _context.TodoItems.Update(todo);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public ActionResult Delete(long id)
        {
            var todo = _context.TodoItems.Find(id);
            if (todo == null) NotFound();

            _context.TodoItems.Remove(todo);
            _context.SaveChanges();

            return NoContent();

            
        }
    }
}
