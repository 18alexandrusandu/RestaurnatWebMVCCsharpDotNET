using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication5.Data;
using WebApplication5.Models;
using WebApplication5.Services;

namespace WebApplication5.Controllers
{
    public class MenuController : Controller
    {
        private readonly RestaurantDbContext _context;
        private readonly IMenuService _service;

        public MenuController(RestaurantDbContext context, IMenuService service)
        {
            _context = context;
            _service = service;
        }

        // GET: MenuItems
        public async Task<IActionResult> Index()
        {
              return View(await _service.List());
        }

        // GET: MenuItems/Details/5
        public async Task<IActionResult> Details(int? Id)
        {
            if (Id == null || _context.MenuItem == null)
            {
                return NotFound();
            }

            var menuItem = await _service.GetMenuItem((int)Id);
            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        // GET: MenuItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MenuItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,numePreparat,pret,stoc")] MenuItem menuItem)
        {
            if (ModelState.IsValid)
            {
                await _service.Add(menuItem);
         
                
                return RedirectToAction(nameof(Index));
            }
            return View(menuItem);
        }

        // GET: MenuItems/Edit/5
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var menuItem = await _service.GetMenuItem((int)Id);
            if (menuItem == null)
            {
                return NotFound();
            }
            return View(menuItem);
        }

        // POST: MenuItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id,[Bind("Id,numePreparat,pret,stoc")] MenuItem menuItem)
        {
            if (Id != menuItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _service.Update(menuItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                 
                }
                return RedirectToAction(nameof(Index));
            }
            return View(menuItem);
        }

        // GET: MenuItems/Delete/5
        public async Task<IActionResult> Delete(String name)
        {
            if (name == null )
            {
                return NotFound();
            }

            var menuItem = await _service.GetMenuItem(name);
            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        // POST: MenuItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(String name)
        {
            if (_context.MenuItem == null)
            {
                return Problem("Entity set 'WebApplication5Context.MenuItem'  is null.");
            }
            var menuItem = await _service.GetMenuItem(name);
            if (menuItem != null)
            {
                _service.Delete(menuItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        
    }
}
