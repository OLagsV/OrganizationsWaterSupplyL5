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
using OrganizationsWaterSupplyL4.ViewModels.RateOrgNotes;

namespace OrganizationsWaterSupplyL4.Controllers
{
    public class RateOrgNotesController : Controller
    {
        private readonly OrganizationsWaterSupplyContext _context;
        CachedRateOrgNotesService _notesService;
        List<RateOrgNote> _notes;

        public RateOrgNotesController(OrganizationsWaterSupplyContext context)
        {
            _context = context;
            _notesService = (CachedRateOrgNotesService)context.GetService<ICachedService<RateOrgNote>>();
            _notes = _notesService.GetData("notes").ToList();
        }

        // GET: RateOrgNotes
        public async Task<IActionResult> Index(int rateId, int organizationId, int page = 1, SortState sortOrder = SortState.RateAsc)
        {
            int pageSize = 20;
            if (rateId != 0)
            {
                _notes = _notes.Where(c => c.RateId == rateId).ToList();
            }
            if (organizationId != 0)
            {
                _notes = _notes.Where(c => c.OrganizationId == organizationId).ToList();
            }
            switch (sortOrder)
            {
                case SortState.RateDesc:
                    _notes = _notes.OrderByDescending(s => s.Rate.RateName).ToList();
                    break;
                case SortState.OrganizationAsc:
                    _notes = _notes.OrderBy(s => s.Organization.OrgName).ToList();
                    break;
                case SortState.OrganizationDesc:
                    _notes = _notes.OrderByDescending(s => s.Organization.OrgName).ToList();
                    break;
                default:
                    _notes = _notes.OrderBy(s => s.Rate.RateName).ToList();
                    break;
            }
            var count = _notes.Count();
            var items = _notes.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            RateOrgNotesFilterViewModel rateOrgNotesFilterViewModel = new RateOrgNotesFilterViewModel(_context.Rates.ToList(), _context.Organizations.ToList(), rateId, organizationId);
            RateOrgNotesSortViewModel rateOrgNotesSortViewModel = new RateOrgNotesSortViewModel(sortOrder);
            RateOrgNotesViewModel viewModel = new RateOrgNotesViewModel(items, pageViewModel, rateOrgNotesFilterViewModel,rateOrgNotesSortViewModel);
            return View(viewModel);
        }

        // GET: RateOrgNotes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RateOrgNotes == null)
            {
                return NotFound();
            }

            var rateOrgNote = await _context.RateOrgNotes
                .Include(r => r.Organization)
                .Include(r => r.Rate)
                .FirstOrDefaultAsync(m => m.RateOrgNoteId == id);
            if (rateOrgNote == null)
            {
                return NotFound();
            }

            return View(rateOrgNote);
        }

        // GET: RateOrgNotes/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewData["OrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "OrgName");
            ViewData["RateId"] = new SelectList(_context.Rates, "RateId", "RateName");
            return View();
        }

        // POST: RateOrgNotes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RateOrgNoteId,RateId,OrganizationId")] RateOrgNote rateOrgNote)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rateOrgNote);
                await _context.SaveChangesAsync();
                _notesService.UpdateData("notes");
                _notes = _notesService.GetData("notes").ToList();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "OrgName", rateOrgNote.OrganizationId);
            ViewData["RateId"] = new SelectList(_context.Rates, "RateId", "RateName", rateOrgNote.RateId);
            return View(rateOrgNote);
        }

        // GET: RateOrgNotes/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RateOrgNotes == null)
            {
                return NotFound();
            }

            var rateOrgNote = await _context.RateOrgNotes.FindAsync(id);
            if (rateOrgNote == null)
            {
                return NotFound();
            }
            ViewData["OrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "OrgName", rateOrgNote.OrganizationId);
            ViewData["RateId"] = new SelectList(_context.Rates, "RateId", "RateName", rateOrgNote.RateId);
            return View(rateOrgNote);
        }

        // POST: RateOrgNotes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RateOrgNoteId,RateId,OrganizationId")] RateOrgNote rateOrgNote)
        {
            if (id != rateOrgNote.RateOrgNoteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rateOrgNote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RateOrgNoteExists(rateOrgNote.RateOrgNoteId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _notesService.UpdateData("notes");
                _notes = _notesService.GetData("notes").ToList();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "OrgName", rateOrgNote.OrganizationId);
            ViewData["RateId"] = new SelectList(_context.Rates, "RateId", "RateName", rateOrgNote.RateId);
            return View(rateOrgNote);
        }

        // GET: RateOrgNotes/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RateOrgNotes == null)
            {
                return NotFound();
            }

            var rateOrgNote = await _context.RateOrgNotes
                .Include(r => r.Organization)
                .Include(r => r.Rate)
                .FirstOrDefaultAsync(m => m.RateOrgNoteId == id);
            if (rateOrgNote == null)
            {
                return NotFound();
            }

            return View(rateOrgNote);
        }

        // POST: RateOrgNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RateOrgNotes == null)
            {
                return Problem("Entity set 'OrganizationsWaterSupplyContext.RateOrgNotes'  is null.");
            }
            var rateOrgNote = await _context.RateOrgNotes.FindAsync(id);
            if (rateOrgNote != null)
            {
                _context.RateOrgNotes.Remove(rateOrgNote);
            }
            
            await _context.SaveChangesAsync();
            _notesService.UpdateData("notes");
            _notes = _notesService.GetData("notes").ToList();
            return RedirectToAction(nameof(Index));
        }

        private bool RateOrgNoteExists(int id)
        {
          return (_context.RateOrgNotes?.Any(e => e.RateOrgNoteId == id)).GetValueOrDefault();
        }
    }
}
