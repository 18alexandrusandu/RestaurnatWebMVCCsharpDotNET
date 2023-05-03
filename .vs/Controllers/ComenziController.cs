using System;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using NuGet.Protocol;
using WebApplication5.Data;
using WebApplication5.Models;
using WebApplication5.Services;

namespace WebApplication5.Controllers
{
    public class ComenziController : Controller
    {
        private readonly RestaurantDbContext _context;
        public readonly IComenziService _service;
        public readonly IMenuService _menuservice;
        public readonly IExportService  _exportService;



        public ComenziController(RestaurantDbContext context, IComenziService service, IMenuService menuservice)
        {
            _context = context;
            _service = service;
            _menuservice = menuservice;
        }

        // GET: Comandas
        public async Task<IActionResult> Index()
        {
            return View(await _service.List());
        }

        // GET: Comandas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Comanda == null)
            {
                return NotFound();
            }

            var comanda = await _service.GetComanda((int)id);


               
            if (comanda == null)
            {
                return NotFound();
            }

            return View(comanda);
        }

        // GET: Comandas/Create
        public IActionResult Create()
        {
            return View();
        }



        [HttpGet]
        public async Task<IActionResult> AddToComanda(int product_id)
         {
         
            return View();
           

        }
        [HttpPost]
        public async Task<IActionResult> AddToComanda(int id,[Bind("Id,Quantity")] ComandaItem comandaItem)
        {
            Debug.Write("id should be:"+id,"comanda id is:"+ comandaItem.Id);
            var comandaId=comandaItem.Id;
            var comanda= await _service.GetComanda(comandaId);
            await _service.AddProduct(comandaId, id,comandaItem.Quantity);
            return RedirectToAction(nameof(Index));


        }




        // POST: Comandas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("pretTotal,status,Id,dateAndHour")] Comanda comanda)
        {
            if (ModelState.IsValid)
            {
                await _service.Add(comanda);
               
               // await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(comanda);
        }

        // GET: Comandas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Comanda == null)
            {
                return NotFound();
            }

            var comanda = await _service.GetComanda((int)id);
            if (comanda == null)
            {
                return NotFound();
            }
            return View(comanda);
        }

        // POST: Comandas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("pretTotal,status,Id,dateAndHour")] Comanda comanda)
        {
            if (id != comanda.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                     await _service.Update(comanda);
                   
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await ComandaExists(comanda.Id))
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
            return View(comanda);
        }

        // GET: Comandas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comanda = await _service.GetComanda((int)id);
            if (comanda == null)
            {
                return NotFound();
            }

            return View(comanda);
        }

        // POST: Comandas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Comanda == null)
            {
                return Problem("Entity set 'WebApplication5Context.Comanda'  is null.");
            }
            var comanda = await _service.GetComanda(id);
            if (comanda != null)
            {
               await _service.Delete(comanda);
            }
            
          
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> FinishComanda(int id)
        {
            await _service.FinalizeComanda(id);
           
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Report(Reports? report)
        {
            if(report==null)
            return View(null);
            

            return View(report);

        }


        [HttpPost]
        public async Task<IActionResult> See_Report(IFormCollection form)
        {
            Debug.WriteLine("Entered here");
            Reports report = new Reports();
            report.Type = "Report";
            report.Data =await _service.RaportComenzi(DateTime.Parse((String)form["start"]),
                                                 DateTime.Parse((String)form["end"]));
            return RedirectToAction(nameof(Report),report);

           

        }
        [HttpPost]
        public async Task<IActionResult> See_Stat()
        {
            Debug.WriteLine("Entered in see_stat");
            Reports report = new Reports();
            report.Type = "Stat";
            report.Data =await _service.RaportStatisiticiProduseComanda();

            
            return RedirectToAction(nameof(Report),report);

        }
        private async  Task<bool> ComandaExists(int id)
        {
            return await _service.Exists(id);
        }
    }
}
