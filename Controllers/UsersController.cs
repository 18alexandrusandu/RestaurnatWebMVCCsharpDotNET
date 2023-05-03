using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class UsersController : Controller
    {
        public IUserService _service;
        private readonly RestaurantDbContext _context;

        public UsersController(RestaurantDbContext context, IUserService _service)
        {   this._service = _service;
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
              return View(await _service.List());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
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
            if (id == null)
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
                    if (!await UserExists(user.Id))
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
                return Problem("Entity set 'RestaurantDbContext.User'  is null.");
            }
            var user = await _service.GetUserAccount((int)id);

             await _service.DeleteUserAccount(user);
          
            return RedirectToAction(nameof(Index));
        }

        private Task<bool> UserExists(int id)
        {
            return  _service.Exist((int)id);

        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("username,password")] User user)
        {
            User user2 = null;
            try
            { 
               user2 = await _service.login(user.username, user.password);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }
                
                if (user2 != null)
                {
                    HttpContext.Session.SetString("id", user2.Id.ToString());
                    HttpContext.Session.SetString("username", user2.username.ToString());
                    HttpContext.Session.SetString("role", user2.role.ToString());

                    if (user2.role == "angajat")
                        return RedirectToAction(nameof(Angajat), new { id = user2.Id });
                    else
                      if (user2.role == "admin")
                        return RedirectToAction(nameof(Admin), new { id = user2.Id });
                }
            
            {
                

              

            }
            ErrorViewModel model2 = new ErrorViewModel();
            model2.RequestId = "The login failed you either input a bad bad username or pasword,please try again ";
            return RedirectToAction("Error", "Home", model2);



        }
        public async Task<IActionResult> Logout()
        {
            await _service.logout();
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }
        public async Task<IActionResult> Angajat(int? id)
        {
            User user = null;
            if (id.HasValue)
            user = await _service.GetUserAccount((int)id);
            return View(user);
        }

        public async Task<IActionResult> Admin(int? id)
        {
            User user = null;
            if (id.HasValue)
             user = await _service.GetUserAccount((int)id);
            return View(user);
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

            return RedirectToAction(nameof(Login));

        }
        public async Task<IActionResult> ChangePasswordSecond(int? id)
        {

           User user= await _service.GetUserAccount((int)id);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePasswordSecond([Bind("username,password")] User user)
        {
           await _service.resetPassword(user.username, user.password);
            return RedirectToAction(nameof(Login));
        }


    }
}
