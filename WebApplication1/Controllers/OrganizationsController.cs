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
using OrganizationsWaterSupplyL4.Data;
using OrganizationsWaterSupplyL4.Models;
using OrganizationsWaterSupplyL4.Services;
using OrganizationsWaterSupplyL4.ViewModels.Organizations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OrganizationsWaterSupplyL4.Controllers
{
    public class OrganizationsController : Controller
    {
        private readonly OrganizationsWaterSupplyContext _context;
        CachedOrganizationsService _organizationsService;
        List<Organization> _organizations;
        public OrganizationsController(OrganizationsWaterSupplyContext context)
        {
            _context = context;
            _organizationsService = (CachedOrganizationsService)context.GetService<ICachedService<Organization>>();
            _organizations = _organizationsService.GetData("organizations").ToList();
        }

        // GET: Organizations
        public async Task<IActionResult> Index(string orgName, string ownershipType, int page = 1, SortState sortOrder = SortState.OrgNameAsc)
        {

            int pageSize = 20;
            if (!string.IsNullOrEmpty(orgName))
            {
                _organizations = _organizations.Where(p => p.OrgName == orgName).ToList();
            }
            if (!string.IsNullOrEmpty(ownershipType))
            {
                _organizations = _organizations.Where(p => p.OwnershipType == ownershipType).ToList();
            }
            switch (sortOrder)
            {
                case SortState.OrgNameDesc:
                    _organizations = _organizations.OrderByDescending(s => s.OrgName).ToList();
                    break;
                case SortState.OwnerTypeAsc:
                    _organizations = _organizations.OrderBy(s => s.OwnershipType).ToList();
                    break;
                case SortState.OwnerTypeDesc:
                    _organizations = _organizations.OrderByDescending(s => s.OwnershipType).ToList();
                    break;
                case SortState.AdressAsc:
                    _organizations = _organizations.OrderBy(s => s.Adress).ToList();
                    break;
                case SortState.AdressDesc:
                    _organizations = _organizations.OrderByDescending(s => s.Adress).ToList();
                    break;
                case SortState.DirectorAsc:
                    _organizations = _organizations.OrderBy(s => s.DirectorFullname).ToList();
                    break;
                case SortState.DirectorDesc:
                    _organizations = _organizations.OrderByDescending(s => s.DirectorFullname).ToList();
                    break;
                case SortState.ResponsibleAsc:
                    _organizations = _organizations.OrderBy(s => s.ResponsibleFullname).ToList();
                    break;
                case SortState.ResponsibleDesc:
                    _organizations = _organizations.OrderByDescending(s => s.ResponsibleFullname).ToList();
                    break;
                default:
                    _organizations = _organizations.OrderBy(s => s.OrgName).ToList();
                    break;
            }
            var count = _organizations.Count();
            var items = _organizations.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            OrganizationsFilterViewModel filterModel = new OrganizationsFilterViewModel(orgName,ownershipType);
            OrganizationsSortViewModel sortModel = new OrganizationsSortViewModel(sortOrder);
            OrganizationsViewModel viewModel = new OrganizationsViewModel(items, pageViewModel, filterModel, sortModel);
            return View(viewModel);
        }

        // GET: Organizations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Organizations == null)
            {
                return NotFound();
            }

            var organization = await _context.Organizations
                .FirstOrDefaultAsync(m => m.OrganizationId == id);
            if (organization == null)
            {
                return NotFound();
            }

            return View(organization);
        }

        // GET: Organizations/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Organizations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrganizationId,OrgName,OwnershipType,Adress,DirectorFullname,DirectorPhone,ResponsibleFullname,ResponsiblePhone")] Organization organization)
        {
            if (ModelState.IsValid)
            {
                _context.Add(organization);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(organization);
        }

        // GET: Organizations/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Organizations == null)
            {
                return NotFound();
            }

            var organization = await _context.Organizations.FindAsync(id);
            if (organization == null)
            {
                return NotFound();
            }
            return View(organization);
        }

        // POST: Organizations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrganizationId,OrgName,OwnershipType,Adress,DirectorFullname,DirectorPhone,ResponsibleFullname,ResponsiblePhone")] Organization organization)
        {
            if (id != organization.OrganizationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(organization);
                    await _context.SaveChangesAsync();
                    _organizationsService.UpdateData("organizations");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationExists(organization.OrganizationId))
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
            return View(organization);
        }

        // GET: Organizations/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Organizations == null)
            {
                return NotFound();
            }

            var organization = await _context.Organizations
                .FirstOrDefaultAsync(m => m.OrganizationId == id);
            if (organization == null)
            {
                return NotFound();
            }

            return View(organization);
        }

        // POST: Organizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Organizations == null)
            {
                return Problem("Entity set 'OrganizationsWaterSupplyContext.Organizations'  is null.");
            }
            var organization = await _context.Organizations.FindAsync(id);
            if (organization != null)
            {
                _context.Organizations.Remove(organization);
            }
            
            await _context.SaveChangesAsync();
            _organizationsService.UpdateData("organizations");
            return RedirectToAction(nameof(Index));
        }

        private bool OrganizationExists(int id)
        {
          return (_context.Organizations?.Any(e => e.OrganizationId == id)).GetValueOrDefault();
        }
    }
}
