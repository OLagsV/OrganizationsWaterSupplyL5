using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OrganizationsWaterSupplyL4.Data;
using OrganizationsWaterSupplyL4.Models;
using OrganizationsWaterSupplyL4.Services;
using OrganizationsWaterSupplyL4.ViewModels.CountersChecks;

namespace OrganizationsWaterSupplyL4.Controllers
{
    public class CountersChecksController : Controller
    {
        private readonly OrganizationsWaterSupplyContext _context;
        CachedCountersChecksService _countersChecksService;
        List<CountersCheck> _checks;
        public CountersChecksController(OrganizationsWaterSupplyContext context)
        {
            _context = context;
            _countersChecksService = (CachedCountersChecksService)context.GetService<ICachedService<CountersCheck>>();
            _checks = _countersChecksService.GetData("checks").ToList();
        }

        // GET: CountersChecks
        public async Task<IActionResult> Index(string result, DateTime checkDate, int registartionNumber, int page = 1, SortState sortOrder = SortState.CheckDateAsc)
        {
            int pageSize = 20;
            if (!string.IsNullOrEmpty(result))
            {
                _checks = _checks.Where(c => c.CheckResult == result).ToList();
            }
            if (checkDate.Date != (new DateTime()).Date)
            {
                _checks = _checks.Where(c => c.CheckDate == checkDate).ToList();
            }
            if (registartionNumber != 0)
            {
                _checks = _checks.Where(c => c.CounterRegistrationNumber == registartionNumber).ToList();
            }
            switch (sortOrder)
            {
                case SortState.CheckDateDesc:
                    _checks = _checks.OrderByDescending(s => s.CheckDate).ToList();
                    break;
                case SortState.ResultAsc:
                    _checks = _checks.OrderBy(s => s.CheckResult).ToList();
                    break;
                case SortState.ResultDesc:
                    _checks = _checks.OrderByDescending(s => s.CheckResult).ToList();
                    break;
                default:
                    _checks = _checks.OrderBy(s => s.CheckDate).ToList();
                    break;
            }
            var count = _checks.Count();
            var items = _checks.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            CountersChecksFilterViewModel countersChecksFilterViewModel = new CountersChecksFilterViewModel(_context.Counters.ToList(), checkDate, result, registartionNumber);
            CountersChecksSortViewModel countersSortViewModel = new CountersChecksSortViewModel(sortOrder);
            CountersChecksViewModel viewModel = new CountersChecksViewModel(items, pageViewModel, countersChecksFilterViewModel,countersSortViewModel);
            return View(viewModel);
        }

        // GET: CountersChecks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CountersChecks == null)
            {
                return NotFound();
            }

            var countersCheck = await _context.CountersChecks
                .Include(c => c.RegistrationNumber)
                .FirstOrDefaultAsync(m => m.CountersCheckId == id);
            if (countersCheck == null)
            {
                return NotFound();
            }

            return View(countersCheck);
        }

        // GET: CountersChecks/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewData["CounterRegistrationNumber"] = new SelectList(_context.Counters, "RegistrationNumber", "RegistrationNumber");
            return View();
        }

        // POST: CountersChecks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CountersCheckId,CounterRegistrationNumber,CheckDate,CheckResult")] CountersCheck countersCheck)
        {
            if (ModelState.IsValid)
            {
                _context.Add(countersCheck);
                await _context.SaveChangesAsync();
                _countersChecksService.UpdateData("checks");
                _checks = _countersChecksService.GetData("checks").ToList();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CounterRegistrationNumber"] = new SelectList(_context.Counters, "RegistrationNumber", "RegistrationNumber", countersCheck.CounterRegistrationNumber);
            return View(countersCheck);
        }

        // GET: CountersChecks/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CountersChecks == null)
            {
                return NotFound();
            }

            var countersCheck = await _context.CountersChecks.FindAsync(id);
            if (countersCheck == null)
            {
                return NotFound();
            }
            ViewData["CounterRegistrationNumber"] = new SelectList(_context.Counters, "RegistrationNumber", "RegistrationNumber", countersCheck.CounterRegistrationNumber);
            return View(countersCheck);
        }

        // POST: CountersChecks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CountersCheckId,CounterRegistrationNumber,CheckDate,CheckResult")] CountersCheck countersCheck)
        {
            if (id != countersCheck.CountersCheckId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(countersCheck);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountersCheckExists(countersCheck.CountersCheckId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _countersChecksService.UpdateData("checks");
                _checks = _countersChecksService.GetData("checks").ToList();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CounterRegistrationNumber"] = new SelectList(_context.Counters, "RegistrationNumber", "RegistrationNumber", countersCheck.CounterRegistrationNumber);
            return View(countersCheck);
        }

        // GET: CountersChecks/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CountersChecks == null)
            {
                return NotFound();
            }

            var countersCheck = await _context.CountersChecks
                .Include(c => c.RegistrationNumber)
                .FirstOrDefaultAsync(m => m.CountersCheckId == id);
            if (countersCheck == null)
            {
                return NotFound();
            }

            return View(countersCheck);
        }

        // POST: CountersChecks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CountersChecks == null)
            {
                return Problem("Entity set 'OrganizationsWaterSupplyContext.CountersChecks'  is null.");
            }
            var countersCheck = await _context.CountersChecks.FindAsync(id);
            if (countersCheck != null)
            {
                _context.CountersChecks.Remove(countersCheck);
            }
            
            await _context.SaveChangesAsync();
            _countersChecksService.UpdateData("checks");
            _checks = _countersChecksService.GetData("checks").ToList();
            return RedirectToAction(nameof(Index));
        }

        private bool CountersCheckExists(int id)
        {
          return (_context.CountersChecks?.Any(e => e.CountersCheckId == id)).GetValueOrDefault();
        }
    }
}
