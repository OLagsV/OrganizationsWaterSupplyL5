using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OrganizationsWaterSupplyL4.Data;
using OrganizationsWaterSupplyL4.Models;
using OrganizationsWaterSupplyL4.Services;
using OrganizationsWaterSupplyL4.ViewModels.CountersModels;

namespace OrganizationsWaterSupplyL4.Controllers
{
    public class CounterModelsController : Controller
    {
        private readonly OrganizationsWaterSupplyContext _context;
        CachedModelsService _modelsService;
        List<CounterModel> _models;
        public CounterModelsController(OrganizationsWaterSupplyContext context)
        {
            _context = context;
            _modelsService = (CachedModelsService)context.GetService<ICachedService<CounterModel>>();
            _models = _modelsService.GetData("models").ToList();
        }

        // GET: CounterModels
        public async Task<IActionResult> Index(string modelName, string manufacturer, int serviceTime,  int page = 1, SortState sortOrder = SortState.ModelNameAsc)
        {
            int pageSize = 20;
            if (!string.IsNullOrEmpty(modelName))
            {
                _models = _models.Where(p => p.ModelName == modelName).ToList();
            }
            if (!string.IsNullOrEmpty(manufacturer))
            {
                _models = _models.Where(p => p.Manufacturer == manufacturer).ToList();
            }
            if (serviceTime != 0)
            {
                _models = _models.Where(p => p.ServiceTime == serviceTime).ToList();
            }
            switch (sortOrder)
            {
                case SortState.ModelNameDesc:
                    _models = _models.OrderByDescending(s => s.ModelName).ToList();
                    break;
                case SortState.ManufacturerAsc:
                    _models = _models.OrderBy(s => s.Manufacturer).ToList();
                    break;
                case SortState.ManufacturerDesc:
                    _models = _models.OrderByDescending(s => s.Manufacturer).ToList();
                    break;
                case SortState.ServiceTimeAsc:
                    _models = _models.OrderBy(s => s.ServiceTime).ToList();
                    break;
                case SortState.ServiceTimeDesc:
                    _models = _models.OrderByDescending(s => s.ServiceTime).ToList();
                    break;
                default:
                    _models = _models.OrderBy(s => s.ModelName).ToList();
                    break;
            }
            var count = _models.Count();
            var items = _models.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            CountersModelsFilterViewModel filterModel = new CountersModelsFilterViewModel(modelName, manufacturer, serviceTime);
            CountersModelsSortViewModel sortModel = new CountersModelsSortViewModel(sortOrder);
            CountersModelsViewModel viewModel = new CountersModelsViewModel(items, pageViewModel,filterModel,sortModel);
            return View(viewModel);

        }

        // GET: CounterModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CounterModels == null)
            {
                return NotFound();
            }

            var counterModel = await _context.CounterModels
                .FirstOrDefaultAsync(m => m.ModelId == id);
            if (counterModel == null)
            {
                return NotFound();
            }

            return View(counterModel);
        }

        // GET: CounterModels/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: CounterModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModelId,ModelName,Manufacturer,ServiceTime")] CounterModel counterModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(counterModel);
                await _context.SaveChangesAsync();
                _modelsService.UpdateData("models");
                _models = _modelsService.GetData("models").ToList();
                return RedirectToAction(nameof(Index));
            }
            return View(counterModel);
        }

        // GET: CounterModels/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CounterModels == null)
            {
                return NotFound();
            }

            var counterModel = await _context.CounterModels.FindAsync(id);
            if (counterModel == null)
            {
                return NotFound();
            }
            return View(counterModel);
        }

        // POST: CounterModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ModelId,ModelName,Manufacturer,ServiceTime")] CounterModel counterModel)
        {
            if (id != counterModel.ModelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(counterModel);
                    await _context.SaveChangesAsync();
                    _modelsService.UpdateData("models");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CounterModelExists(counterModel.ModelId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _modelsService.UpdateData("models");
                _models = _modelsService.GetData("models").ToList();
                return RedirectToAction(nameof(Index));
            }
            return View(counterModel);
        }

        // GET: CounterModels/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CounterModels == null)
            {
                return NotFound();
            }

            var counterModel = await _context.CounterModels
                .FirstOrDefaultAsync(m => m.ModelId == id);
            if (counterModel == null)
            {
                return NotFound();
            }

            return View(counterModel);
        }

        // POST: CounterModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CounterModels == null)
            {
                return Problem("Entity set 'OrganizationsWaterSupplyContext.CounterModels'  is null.");
            }
            var counterModel = await _context.CounterModels.FindAsync(id);
            if (counterModel != null)
            {
                _context.CounterModels.Remove(counterModel);
            }
            
            await _context.SaveChangesAsync();
            _modelsService.UpdateData("models");
            _models = _modelsService.GetData("models").ToList();
            return RedirectToAction(nameof(Index));
        }

        private bool CounterModelExists(int id)
        {
          return (_context.CounterModels?.Any(e => e.ModelId == id)).GetValueOrDefault();
        }
    }
}
