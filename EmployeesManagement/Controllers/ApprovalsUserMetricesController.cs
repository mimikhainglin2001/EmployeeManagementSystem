using EmployeesManagement.Data;
using EmployeesManagement.Models;
using EmployeesManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmployeesManagement.Controllers
{
    public class ApprovalsUserMetricesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApprovalsUserMetricesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ApprovalsUserMetrices
        public async Task<IActionResult> Index()
        {
            var matrix = await _context.ApprovalsUserMetrixs
                .Include(a => a.DocumentType)
                .Include(a => a.User)
                .Include(a => a.CreatedBy)
                .Include(a => a.WorkflowUserGroup)
                .ToListAsync();
            return View(matrix);
        }

        // GET: ApprovalsUserMetrices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approvalsUserMetrix = await _context.ApprovalsUserMetrixs
                .Include(a => a.DocumentType)
                .Include(a => a.User)
                .Include(a => a.WorkflowUserGroup)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (approvalsUserMetrix == null)
            {
                return NotFound();
            }

            return View(approvalsUserMetrix);
        }

        // GET: ApprovalsUserMetrices/Create
        public IActionResult Create()
        {
            ViewData["DocumentTypeId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "DocumentTypes"), "Id", "Description");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["WorkflowUserGroupId"] = new SelectList(_context.WorkFlowUserGroups, "Id", "Description");
            return View();
        }

        // POST: ApprovalsUserMetrices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApprovalsUserMetrix approvalsUserMetrix)
        {
            var userid = User.GetUserId();
            approvalsUserMetrix.CreatedById = userid;
            approvalsUserMetrix.CreatedOn = DateTime.Now;

            _context.Add(approvalsUserMetrix);
            await _context.SaveChangesAsync(userid);
            TempData["Message"] = "Approval User Matrix created successfully";
            return RedirectToAction(nameof(Index));

            ViewData["DocumentTypeId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "DocumentTypes"), "Id", "Description", approvalsUserMetrix.DocumentTypeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", approvalsUserMetrix.UserId);
            ViewData["WorkflowUserGroupId"] = new SelectList(_context.WorkFlowUserGroups, "Id", "Description", approvalsUserMetrix.WorkflowUserGroupId);
            return View(approvalsUserMetrix);
        }

        // GET: ApprovalsUserMetrices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approvalsUserMetrix = await _context.ApprovalsUserMetrixs.FindAsync(id);
            if (approvalsUserMetrix == null)
            {
                return NotFound();
            }
            ViewData["DocumentTypeId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "DocumentTypes"), "Id", "Description", approvalsUserMetrix.DocumentTypeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", approvalsUserMetrix.UserId);
            ViewData["WorkflowUserGroupId"] = new SelectList(_context.WorkFlowUserGroups, "Id", "Description", approvalsUserMetrix.WorkflowUserGroupId);
            return View(approvalsUserMetrix);
        }

        // POST: ApprovalsUserMetrices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ApprovalsUserMetrix approvalsUserMetrix)
        {
            if (id != approvalsUserMetrix.Id)
            {
                return NotFound();
            }

            var userid = User.GetUserId();
            approvalsUserMetrix.ModifiedById = userid;
            approvalsUserMetrix.ModifiedOn = DateTime.Now;

            ModelState.Remove("CreatedBy");
            ModelState.Remove("ModifiedBy");
            ModelState.Remove("DocumentType"); 
            ModelState.Remove("WorkflowUserGroup");
            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(approvalsUserMetrix);
                    await _context.SaveChangesAsync(userid);
                    TempData["Message"] = "Approval User Matrix updated successfully";

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApprovalsUserMetrixExists(approvalsUserMetrix.Id))
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
            ViewData["DocumentTypeId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "DocumentTypes"), "Id", "Description", approvalsUserMetrix.DocumentTypeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", approvalsUserMetrix.UserId);
            ViewData["WorkflowUserGroupId"] = new SelectList(_context.WorkFlowUserGroups, "Id", "Description", approvalsUserMetrix.WorkflowUserGroupId);
            return View(approvalsUserMetrix);
        }

        // GET: ApprovalsUserMetrices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approvalsUserMetrix = await _context.ApprovalsUserMetrixs
                .Include(a => a.DocumentType)
                .Include(a => a.User)
                .Include(a => a.WorkflowUserGroup)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (approvalsUserMetrix == null)
            {
                return NotFound();
            }

            return View(approvalsUserMetrix);
        }

        // POST: ApprovalsUserMetrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var approvalsUserMetrix = await _context.ApprovalsUserMetrixs.FindAsync(id);
            if (approvalsUserMetrix != null)
            {
                _context.ApprovalsUserMetrixs.Remove(approvalsUserMetrix);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApprovalsUserMetrixExists(int id)
        {
            return _context.ApprovalsUserMetrixs.Any(e => e.Id == id);
        }
    }
}
