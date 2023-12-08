using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OrganizationsWaterSupplyL4.Data;
using OrganizationsWaterSupplyL4.Models;
using OrganizationsWaterSupplyL4.Services;
using OrganizationsWaterSupplyL4.ViewModels.Rates;

namespace OrganizationsWaterSupplyL4.Controllers
{
    public class RatesController : Controller
    {
        private readonly OrganizationsWaterSupplyContext _context;
        CachedRatesService _ratesService;
        List<Rate> _rates;

        public RatesController(OrganizationsWaterSupplyContext context)
        {
            _context = context;
            _ratesService = (CachedRatesService)context.GetService<ICachedService<Rate>>();
            _rates = _ratesService.GetData("rates").ToList();
        }

        // GET: Rates
        public async Task<IActionResult> Index(string rateName, decimal price, int page = 1, SortState sortOrder = SortState.RateNameAsc)
        {
            int pageSize = 20;
            if (!string.IsNullOrEmpty(rateName))
            {
                _rates = _rates.Where(p => p.RateName == rateName).ToList();
            }
            if (price != 0)
            {
                _rates = _rates.Where(p => p.Price == price).ToList();
            }
            switch (sortOrder)
            {
                case SortState.RateNameDesc:
                    _rates = _rates.OrderByDescending(s => s.RateName).ToList();
                    break;
                case SortState.PriceAsc:
                    _rates = _rates.OrderBy(s => s.Price).ToList();
                    break;
                case SortState.PriceDesc:
                    _rates = _rates.OrderByDescending(s => s.Price).ToList();
                    break;
                default:
                    _rates = _rates.OrderBy(s => s.RateName).ToList();
                    break;
            }
            var count = _rates.Count();
            var items = _rates.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            RatesFilterViewModel ratesFilterViewModel = new RatesFilterViewModel(rateName, price);
            RatesSortViewModel ratesSortViewModel = new RatesSortViewModel(sortOrder);
            RatesViewModel viewModel = new RatesViewModel(items, pageViewModel, ratesFilterViewModel,ratesSortViewModel);
            return View(viewModel);
        }

        // GET: Rates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Rates == null)
            {
                return NotFound();
            }

            var rate = await _context.Rates
                .FirstOrDefaultAsync(m => m.RateId == id);
            if (rate == null)
            {
                return NotFound();
            }

            return View(rate);
        }

        // GET: Rates/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RateId,RateName,Price")] Rate rate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rate);
                await _context.SaveChangesAsync();
                _ratesService.UpdateData("rates");
                _rates = _ratesService.GetData("rates").ToList();
                return RedirectToAction(nameof(Index));
            }
            return View(rate);
        }

        // GET: Rates/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Rates == null)
            {
                return NotFound();
            }

            var rate = await _context.Rates.FindAsync(id);
            if (rate == null)
            {
                return NotFound();
            }
            return View(rate);
        }

        // POST: Rates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RateId,RateName,Price")] Rate rate)
        {
            if (id != rate.RateId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RateExists(rate.RateId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _ratesService.UpdateData("rates");
                _rates = _ratesService.GetData("rates").ToList();
                return RedirectToAction(nameof(Index));
            }
            return View(rate);
        }

        // GET: Rates/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Rates == null)
            {
                return NotFound();
            }

            var rate = await _context.Rates
                .FirstOrDefaultAsync(m => m.RateId == id);
            if (rate == null)
            {
                return NotFound();
            }

            return View(rate);
        }

        // POST: Rates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Rates == null)
            {
                return Problem("Entity set 'OrganizationsWaterSupplyContext.Rates'  is null.");
            }
            var rate = await _context.Rates.FindAsync(id);
            if (rate != null)
            {
                _context.Rates.Remove(rate);
            }
            
            await _context.SaveChangesAsync();
            _ratesService.UpdateData("rates");
            _rates = _ratesService.GetData("rates").ToList();
            return RedirectToAction(nameof(Index));
        }

        private bool RateExists(int id)
        {
          return (_context.Rates?.Any(e => e.RateId == id)).GetValueOrDefault();
        }
    }
}
