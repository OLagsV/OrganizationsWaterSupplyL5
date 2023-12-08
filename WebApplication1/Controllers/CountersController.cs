using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrganizationsWaterSupplyL4.Data;
using OrganizationsWaterSupplyL4.Services;
using OrganizationsWaterSupplyL4.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.AspNetCore.Authorization;
using OrganizationsWaterSupplyL4.ViewModels.Counters;
using Microsoft.Data.SqlClient;

namespace OrganizationsWaterSupplyL4.Controllers
{
    public class CountersController : Controller
    {
        private readonly OrganizationsWaterSupplyContext _context;
        CachedCountersService _countersService;
        List<Counter> _counters;

        public CountersController(OrganizationsWaterSupplyContext context)
        {
            _context = context;
            _countersService = (CachedCountersService)context.GetService<ICachedService<Counter>>();
            _counters = _countersService.GetData("counters").ToList();
        }

        // GET: Counters
        public async Task<IActionResult> Index(int modelId, int organizationId, DateTime timeOfInstallation, int page = 1, SortState sortOrder = SortState.TimeOfInstallationAsc)
        {
            int pageSize = 20;
            if (modelId != 0)
            {
                _counters = _counters.Where(c => c.ModelId == modelId).ToList();
            }
            if (organizationId != 0)
            {
                _counters = _counters.Where(c => c.OrganizationId == organizationId).ToList();
            }
            if (timeOfInstallation.Date != (new DateTime()).Date)
            {
                _counters = _counters.Where(c => c.TimeOfInstallation == timeOfInstallation).ToList();
            }
            switch (sortOrder)
            {
                case SortState.TimeOfInstallationDesc:
                    _counters = _counters.OrderByDescending(s => s.TimeOfInstallation).ToList();
                    break;
                case SortState.ModelAsc:
                    _counters = _counters.OrderBy(s => s.Model.ModelName).ToList();
                    break;
                case SortState.ModelDesc:
                    _counters = _counters.OrderByDescending(s => s.Model.ModelName).ToList();
                    break;
                case SortState.OrganizationAsc:
                    _counters = _counters.OrderBy(s => s.Organization.OrgName).ToList();
                    break;
                case SortState.OrganizationDesc:
                    _counters = _counters.OrderByDescending(s => s.Organization.OrgName).ToList();
                    break;
                default:
                    _counters = _counters.OrderBy(s => s.TimeOfInstallation).ToList();
                    break;
            }
            var count = _counters.Count();
            var items = _counters.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            CountersFilterViewModel CountersFilterViewModel = new CountersFilterViewModel(_context.CounterModels.ToList(), _context.Organizations.ToList(), modelId, organizationId, timeOfInstallation);
            CountersSortViewModel CountersSortViewModel = new CountersSortViewModel(sortOrder);
            CountersViewModel viewModel = new CountersViewModel(items, pageViewModel, CountersFilterViewModel, CountersSortViewModel);
            return View(viewModel);
        }

        // GET: Counters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Counters == null)
            {
                return NotFound();
            }

            var counter = await _context.Counters
                .Include(c => c.Model)
                .Include(c => c.Organization)
                .FirstOrDefaultAsync(m => m.RegistrationNumber == id);
            if (counter == null)
            {
                return NotFound();
            }

            return View(counter);
        }

        // GET: Counters/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewData["ModelId"] = new SelectList(_context.CounterModels, "ModelId", "ModelName");
            ViewData["OrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "OrgName");
            return View();
        }

        // POST: Counters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RegistrationNumber,ModelId,TimeOfInstallation,OrganizationId")] Counter counter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(counter);
                await _context.SaveChangesAsync();
                _countersService.UpdateData("counters");
                _counters = _countersService.GetData("counters").ToList();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ModelId"] = new SelectList(_context.CounterModels, "ModelId", "ModelId", counter.ModelId);
            ViewData["OrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "OrganizationId", counter.OrganizationId);
            return View(counter);
        }

        // GET: Counters/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Counters == null)
            {
                return NotFound();
            }

            var counter = await _context.Counters.FindAsync(id);
            if (counter == null)
            {
                return NotFound();
            }
            ViewData["ModelId"] = new SelectList(_context.CounterModels, "ModelId", "ModelName", counter.ModelId);
            ViewData["OrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "OrgName", counter.OrganizationId);
            return View(counter);
        }

        // POST: Counters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RegistrationNumber,ModelId,TimeOfInstallation,OrganizationId")] Counter counter)
        {
            if (id != counter.RegistrationNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(counter);
                    await _context.SaveChangesAsync();
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CounterExists(counter.RegistrationNumber))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _countersService.UpdateData("counters");
                _counters = _countersService.GetData("counters").ToList();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ModelId"] = new SelectList(_context.CounterModels, "ModelId", "ModelName", counter.ModelId);
            ViewData["OrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "OrganizationId", counter.OrganizationId);
            return View(counter);
        }

        // GET: Counters/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Counters == null)
            {
                return NotFound();
            }

            var counter = await _context.Counters
                .Include(c => c.Model)
                .Include(c => c.Organization)
                .FirstOrDefaultAsync(m => m.RegistrationNumber == id);
            if (counter == null)
            {
                return NotFound();
            }

            return View(counter);
        }

        // POST: Counters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Counters == null)
            {
                return Problem("Entity set 'OrganizationsWaterSupplyContext.Counters'  is null.");
            }
            var counter = await _context.Counters.FindAsync(id);
            if (counter != null)
            {
                _context.Counters.Remove(counter);
            }
            
            await _context.SaveChangesAsync();
            _countersService.UpdateData("counters");
            return RedirectToAction(nameof(Index));
        }

        private bool CounterExists(int id)
        {
          return (_context.Counters?.Any(e => e.RegistrationNumber == id)).GetValueOrDefault();
        }
    }
}
