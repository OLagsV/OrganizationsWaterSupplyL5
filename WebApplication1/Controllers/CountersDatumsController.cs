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
using OrganizationsWaterSupplyL4.ViewModels.CountersData;

namespace OrganizationsWaterSupplyL4.Controllers
{
    public class CountersDatumsController : Controller
    {
        private readonly OrganizationsWaterSupplyContext _context;
        CachedCountersDataService _countersDataService;
        List<CountersDatum> _data;

        public CountersDatumsController(OrganizationsWaterSupplyContext context)
        {
            _context = context;
            _countersDataService = (CachedCountersDataService)context.GetService<ICachedService<CountersDatum>>();
            _data = _countersDataService.GetData("countersData").ToList();
        }

        // GET: CountersDatums
        public async Task<IActionResult> Index(int volume, DateTime dataCheckDate, int registartionNumber,  int page = 1, SortState sortOrder = SortState.DataCheckDateAsc)
        {
            int pageSize = 20;
            if (volume != 0)
            {
                _data = _data.Where(c => c.Volume == volume).ToList();
            }
            if (dataCheckDate.Date != (new DateTime()).Date)
            {
                _data = _data.Where(c => c.DataCheckDate == dataCheckDate).ToList();
            }
            if (registartionNumber != 0)
            {
                _data = _data.Where(c => c.CounterRegistrationNumber == registartionNumber).ToList();
            }
            switch (sortOrder)
            {
                case SortState.DataCheckDateDesc:
                    _data = _data.OrderByDescending(s => s.DataCheckDate).ToList();
                    break;
                case SortState.VolumeAsc:
                    _data = _data.OrderBy(s => s.Volume).ToList();
                    break;
                case SortState.VolumeDesc:
                    _data = _data.OrderByDescending(s => s.Volume).ToList();
                    break;
                default:
                    _data = _data.OrderBy(s => s.DataCheckDate).ToList();
                    break;
            }
            var count = _data.Count();
            var items = _data.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            CountersDataFilterViewModel countersDataFilterView = new CountersDataFilterViewModel(_context.Counters.ToList(), dataCheckDate, volume, registartionNumber);
            CountersDataSortViewModel countersDataSortViewModel = new CountersDataSortViewModel(sortOrder);
            CountersDataViewModel viewModel = new CountersDataViewModel(items, pageViewModel, countersDataFilterView, countersDataSortViewModel);
            return View(viewModel);
        }

        // GET: CountersDatums/Details/5

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CountersData == null)
            {
                return NotFound();
            }

            var countersDatum = await _context.CountersData
                .Include(c => c.RegistrationNumber)
                .FirstOrDefaultAsync(m => m.CountersDataId == id);
            if (countersDatum == null)
            {
                return NotFound();
            }

            return View(countersDatum);
        }

        // GET: CountersDatums/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewData["CounterRegistrationNumber"] = new SelectList(_context.Counters, "RegistrationNumber", "RegistrationNumber");
            return View();
        }

        // POST: CountersDatums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CountersDataId,CounterRegistrationNumber,DataCheckDate,Volume")] CountersDatum countersDatum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(countersDatum);
                await _context.SaveChangesAsync();
                _countersDataService.UpdateData("countersData");
                _data = _countersDataService.GetData("countersData").ToList();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CounterRegistrationNumber"] = new SelectList(_context.Counters, "RegistrationNumber", "RegistrationNumber", countersDatum.CounterRegistrationNumber);
            return View(countersDatum);
        }

        // GET: CountersDatums/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CountersData == null)
            {
                return NotFound();
            }

            var countersDatum = await _context.CountersData.FindAsync(id);
            if (countersDatum == null)
            {
                return NotFound();
            }
            ViewData["CounterRegistrationNumber"] = new SelectList(_context.Counters, "RegistrationNumber", "RegistrationNumber", countersDatum.CounterRegistrationNumber);
            return View(countersDatum);
        }

        // POST: CountersDatums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CountersDataId,CounterRegistrationNumber,DataCheckDate,Volume")] CountersDatum countersDatum)
        {
            if (id != countersDatum.CountersDataId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(countersDatum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountersDatumExists(countersDatum.CountersDataId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _countersDataService.UpdateData("countersData");
                _data = _countersDataService.GetData("countersData").ToList();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CounterRegistrationNumber"] = new SelectList(_context.Counters, "RegistrationNumber", "RegistrationNumber", countersDatum.CounterRegistrationNumber);
            return View(countersDatum);
        }

        // GET: CountersDatums/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CountersData == null)
            {
                return NotFound();
            }

            var countersDatum = await _context.CountersData
                .Include(c => c.RegistrationNumber)
                .FirstOrDefaultAsync(m => m.CountersDataId == id);
            if (countersDatum == null)
            {
                return NotFound();
            }

            return View(countersDatum);
        }

        // POST: CountersDatums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CountersData == null)
            {
                return Problem("Entity set 'OrganizationsWaterSupplyContext.CountersData'  is null.");
            }
            var countersDatum = await _context.CountersData.FindAsync(id);
            if (countersDatum != null)
            {
                _context.CountersData.Remove(countersDatum);
            }
            
            await _context.SaveChangesAsync();
            _countersDataService.UpdateData("countersData");
            _data = _countersDataService.GetData("countersData").ToList();
            return RedirectToAction(nameof(Index));
        }

        private bool CountersDatumExists(int id)
        {
          return (_context.CountersData?.Any(e => e.CountersDataId == id)).GetValueOrDefault();
        }
    }
}
