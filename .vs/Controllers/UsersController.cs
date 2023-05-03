using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication5.Data;
using WebApplication5.Models;
using WebApplication5.Services;

namespace WebApplication5.Controllers
{
    public class UsersController : Controller
    {
        private readonly RestaurantDbContext _context;
        private readonly IUserService _service;
        public UsersController(RestaurantDbContext context, IUserService service)
        {
            _context = context;
            _service = service;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _service.GetUserAccount((int)id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("username,password,name,role,Id")] User user)
        {
            if (ModelState.IsValid)
            {
               await _service.SignInUserAccount(user);

                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("username,password,name,role,Id")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.UpdateUserAccount(user);


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _service.GetUserAccount((int)id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'WebApplication5Context.User'  is null.");
            }
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                await _service.DeleteUserAccount(user);
            }

           
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<IActionResult> Angajat(int? id)
        {
            User user = null;
            if (id != null)
            {
                user = await _service.GetUserAccount((int)id);



            }
            return  View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Admin(int? id)
            {
            User user = null;
            if (id!= null)
            {
                user = await _service.GetUserAccount((int)id);
            }
           
           // Console.WriteLine("Entered here");
          return   View(user);
    }


        [HttpGet]
        public async Task<IActionResult> Login()
        {
            Debug.WriteLine("Here is login");

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            if (await _service.logout())
                HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));

        }


        [HttpPost]
        public async Task<IActionResult> Login(IFormCollection form)
        {
            Debug.WriteLine("enterd in login");
            User user = await _service.login(form["username"], form["password"]);
            
            if(user != null)
            {
               
                HttpContext.Session.SetString("id", user.Id.ToString());

                HttpContext.Session.SetString("username", user.username);
                HttpContext.Session.SetString("role", user.role);
                if (user.role == "admin") 
                return RedirectToAction(nameof(Admin), new { id = user.Id });

                if(user.role == "angajat") 
                return RedirectToAction(nameof(Angajat), new { id = user.Id });

            }
            await Response.WriteAsync("<script>alert('Data inserted successfully')</script>");
            return Redirect("https://localhost:7080/Home");
        }


        [HttpGet]
        public async Task<IActionResult> ChangePasswordFirst()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePasswordFirst(IFormCollection form)
        {

            await _service.changePassword(form["username"], form["email"]);

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> ChangePasswordSecond(int? id)
        {
            User user = null;
            if(id!=null)
            user=await _service.GetUserAccount((int)id);
            return View(id);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePasswordSecond([Bind("username","password")]User user)
        {
            await _service.resetPassword(user.username, user.password);
            return Redirect("http://localhost:7080/Home");
        }


    }
}
