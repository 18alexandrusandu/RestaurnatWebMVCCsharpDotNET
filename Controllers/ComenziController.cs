using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using ThirdParty.Json.LitJson;
using WebApplication5.Data;
using WebApplication5.Models;
using WebApplication5.Services;
using WebApplication5.Services.implementations;
using static ServiceStack.LicenseUtils;

namespace WebApplication5.Controllers
{
    public class ComenziController : Controller
    {
        private readonly RestaurantDbContext _context;

        public object RaportResult;
        public ComenziController(RestaurantDbContext context, IComenziService comenziService)
        {
            _service = comenziService;
            _context = context;
            RaportResult = null;
        }

        public IComenziService _service;
        public IExportService ExportService { get; set; }


        // GET: Comandas
        public async Task<IActionResult> Index()
        {
              return View(await _service.List());
        }

        // GET: Comandas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null )
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
                    if (!await ComandaExists(comanda.Id))
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
         
            var comanda = await _service.GetComanda(id);
            if (comanda != null)
            {
               await _service.Delete(comanda);
            }
            
          
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ComandaExists(int id)
        {
            return await _service.Exists(id);
        }

    

        public async Task<IActionResult> FinishComanda(int? id)
        {
           await _service.FinalizeComanda((int)id);
            return RedirectToAction(nameof(Index)); 
        }
        [HttpGet]
        public async Task<IActionResult> AddToComanda(int? id)
        {
            if (await _service.CheckMenuForComanda((int)id, null))
            {
                ComandaItem comandaItem = new ComandaItem();
                comandaItem.Id = (int)id;
                return View(comandaItem);
            }
            else
            {
                ErrorViewModel model = new ErrorViewModel();
                model.RequestId = "The stock is empty no more commands can be made with this product";

                return RedirectToAction("Error", "Home", model);
            }



        }
        [HttpPost]
        public async Task<IActionResult> AddToComanda([Bind("Id,ComandaId", "Quantity")]ComandaItem item)
        {
            if (await _service.CheckMenuForComanda(item.Id, item.Quantity))
            {


               if(! await _service.AddProduct(item.ComandaId, item.Id, item.Quantity))
                {
                    ErrorViewModel model2 = new ErrorViewModel();
                    model2.RequestId = "The vommand is either finalized or some database error appeared ";

                    return RedirectToAction("Error", "Home", model2);
                }

                return RedirectToAction(nameof(Details), new { id = item.ComandaId });
            }
            ErrorViewModel model = new ErrorViewModel();
            model.RequestId = "The stock is too small for the requested command";

            return RedirectToAction("Error","Home", model);
            
        }

        [HttpGet]
        public async Task<IActionResult> Report(Reports? rep)
        {

            return View(rep);
        }
        [HttpPost]
        public async Task<IActionResult> See_Report(IFormCollection form)
        {
              RaportResult=await _service.RaportComenzi(DateTime.Parse(form["start"]), DateTime.Parse(form["end"]));

            Reports raport = new Reports();
            raport.Type = "Report";
            raport.RawData = RaportResult;
            raport.Data = RaportResult.ToJson();
            return RedirectToAction(nameof(Report), raport);

        }
        [HttpPost]
        public async Task<IActionResult> See_Stat()
        {
            RaportResult =await _service.RaportStatisiticiProduseComanda();
            
            
            
            Debug.WriteLine(RaportResult);
            Reports raport = new Reports();
            raport.RawData = RaportResult;
            raport.Type = "Stat";
            raport.Data = RaportResult.ToJson();
            return RedirectToAction(nameof(Report),raport);
        }


        [HttpGet]

        public async Task<IActionResult> Export(String DataAsJson)
        {
          
            Debug.WriteLine("Data", DataAsJson);

            JsonData jsonData = new JsonData(DataAsJson);
            Reports report = new Reports();
            


            return View(report);


        }



        [HttpPost]

        public void Export(IFormCollection form)
        {

            Reports report = new Reports();

            String filename = form["filename"];
            string type = form["type"];
            string content=null;

            if (type == "Csv")
            {
                ExportService = new ExportCsvService();
            }

            else
            if (type == "Xml")
            {
                ExportService = new ExportXmlServicecs();

            }

               if (report.Type=="Report")
                {
                    content = ExportService.serialize<List<Comanda>>(report.RawData);

                }else
                {
                var Map = RaportResult;
                content = ExportService.serialize<List<String>>(report.RawData);

                }
              
            ExportService.export(content, filename);


            }





        }


    }

