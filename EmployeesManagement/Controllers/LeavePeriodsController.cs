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
    public class LeavePeriodsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeavePeriodsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LeavePeriods
        public async Task<IActionResult> Index()
        {
            return View(await _context.LeavePeriods.ToListAsync());
        }

        // GET: LeavePeriods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leavePeriod = await _context.LeavePeriods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leavePeriod == null)
            {
                return NotFound();
            }

            return View(leavePeriod);
        }

        // GET: LeavePeriods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LeavePeriods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeavePeriod leavePeriod)
        {
            try
            {


                var userid = User.GetUserId();
                leavePeriod.CreatedById = userid;
                leavePeriod.CreatedOn = DateTime.Now;
                _context.Add(leavePeriod);
                await _context.SaveChangesAsync(userid);
                TempData["Message"] = "Leave Period create successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error creating Leave Period" + ex.Message;
                return View(leavePeriod);
            }
        }

        // GET: LeavePeriods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leavePeriod = await _context.LeavePeriods.FindAsync(id);
            if (leavePeriod == null)
            {
                return NotFound();
            }
            return View(leavePeriod);
        }

        // POST: LeavePeriods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDate,EndDate,Closed,Locked,CreatedById,CreatedOn,ModifiedById,ModifiedOn")] LeavePeriod leavePeriod)
        {
            if (id != leavePeriod.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leavePeriod);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeavePeriodExists(leavePeriod.Id))
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
            return View(leavePeriod);
        }

        // GET: LeavePeriods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leavePeriod = await _context.LeavePeriods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leavePeriod == null)
            {
                return NotFound();
            }

            return View(leavePeriod);
        }

        // POST: LeavePeriods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leavePeriod = await _context.LeavePeriods.FindAsync(id);
            if (leavePeriod != null)
            {
                _context.LeavePeriods.Remove(leavePeriod);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeavePeriodExists(int id)
        {
            return _context.LeavePeriods.Any(e => e.Id == id);
        }
    }
}
