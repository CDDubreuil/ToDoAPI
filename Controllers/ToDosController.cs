using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDoAPIProject.Models;
using Microsoft.AspNetCore.Cors;

namespace ToDoAPIProject.Controllers
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class ToDosController : Controller
    {
        private readonly ToDoContext _context;

        public ToDosController(ToDoContext context)
        {
            _context = context;
        }

        // GET: ToDos
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ToDo>>> GetToDo()
        {
            if (_context.ToDos == null)
            {
                return NotFound();
            }
            //Step 07) Modify the GET functionality to include the Category
            var resources = await _context.ToDos.Include(x => x.Category).Select(x => new ToDo
            {
                //Assign each resource in our data set to a new Resource object for this application.
                CategoryId = x.CategoryId,
                ToDoId = x.ToDoId,
                Name = x.Name,
                Done = x.Done,
                Category = x.Category != null ? new Category()


                {

                    CategoryId = x.Category.CategoryId,
                    CatName = x.Category.CatName,
                    CatDesc = x.Category.CatDesc

                } : null
            }).ToListAsync();


            return Ok(resources);
        }

      //  GET: ToDos/Details/5
        [HttpGet]
        public async Task<ActionResult<ToDo>> GetToDo(int id)
        {
            if (_context.ToDos == null)
            {
                return NotFound();
            }

            var toDo = await _context.ToDos
               .Where(x => x.ToDoId == id).Select(x => new ToDo()
               {

                   CategoryId = x.CategoryId,
                   ToDoId = x.ToDoId,
                   Name = x.Name,
                   Done = x.Done,
                   Category = x.Category != null ? new Category()
                   {
                       CategoryId = x.Category.CategoryId,
                       CatName = x.Category.CatName,
                       CatDesc = x.Category.CatDesc

                   } : null
               })
                .FirstOrDefaultAsync();
            if (toDo == null)
            {
                return NotFound();
            }
            return toDo;
        }

        // GET: ToDos/Create
        [HttpPost("{id}")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId");
            return View();
        }

        // POST: ToDos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
   
        public async Task<IActionResult> Create([Bind("ToDoId,Name,Done,CategoryId")] ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(toDo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", toDo.CategoryId);
            return View(toDo);
        }

        // GET: ToDos/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ToDos == null)
            {
                return NotFound();
            }

            var toDo = await _context.ToDos.FindAsync(id);
            if (toDo == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", toDo.CategoryId);
            return View(toDo);
        }

        // POST: ToDos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
    
        public async Task<IActionResult> Edit(int id, [Bind("ToDoId,Name,Done,CategoryId")] ToDo toDo)
        {
            if (id != toDo.ToDoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(toDo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToDoExists(toDo.ToDoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", toDo.CategoryId);
            return View(toDo);
        }

        // GET: ToDos/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ToDos == null)
            {
                return NotFound();
            }

            var toDo = await _context.ToDos
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.ToDoId == id);
            if (toDo == null)
            {
                return NotFound();
            }

            return View(toDo);
        }

        // POST: ToDos/Delete/5
        [HttpDelete]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ToDos == null)
            {
                return NotFound();
            }
            var toDo = await _context.ToDos.FindAsync(id);
            if (toDo == null)
            {
                return NotFound();
            }

            _context.ToDos.Remove(toDo);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ToDoExists(int id)
        {
          return (_context.ToDos?.Any(e => e.ToDoId == id)).GetValueOrDefault();
        }
    }
}
