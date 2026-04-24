using EmployeesManagement.Data;
using EmployeesManagement.Migrations;
using EmployeesManagement.Models;
using EmployeesManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesManagement.Controllers
{
    public class FixedAssetsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;


        public FixedAssetsController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        // GET: FixedAssets
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.FixedAssets.Include(f => f.CreatedBy).Include(f => f.Location).Include(f => f.ModifiedBy).Include(f => f.ResponsibleEmployee).Include(f => f.Status).Include(f => f.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: FixedAssets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fixedAsset = await _context.FixedAssets
                .Include(f => f.CreatedBy)
                .Include(f => f.Location)
                .Include(f => f.ModifiedBy)
                .Include(f => f.ResponsibleEmployee)
                .Include(f => f.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fixedAsset == null)
            {
                return NotFound();
            }

            return View(fixedAsset);
        }

        // GET: FixedAssets/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "AssetCategory"),

 "Id", "Description");
            ViewData["ResponsibleEmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            return View();
        }

        // POST: FixedAssets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FixedAsset fixedAsset , IFormFile assetphoto)
        {
            // Image
            if (assetphoto != null && assetphoto.Length > 0)
            {
                var fileName = "AssetPhoto" + DateTime.Now.ToString("yyymmddhhmmss") + "_" + assetphoto.FileName;
                var path = _configuration["FileSettings:UploadFolder"]!; // appsetting.json
                var filePath = Path.Combine(path, fileName);
                var stream = new FileStream(filePath, FileMode.Create);
                await assetphoto.CopyToAsync(stream);
                fixedAsset.Photo = fileName;
            }
            var fixedassetstatus = await _context.SystemCodeDetails
                    .Include(x => x.SystemCode)
                    .Where(x => x.SystemCode.Code == "AssetStatus"
                    && x.Code == "Active").FirstOrDefaultAsync();

            var Userid = User.GetUserId();
            fixedAsset.CreatedById = Userid;
            fixedAsset.CreatedOn = DateTime.Now;
            fixedAsset.StatusId = fixedassetstatus.Id;
            _context.Add(fixedAsset);
                await _context.SaveChangesAsync(Userid);
            TempData["Message"] = "Fixed Asset Details created successfully";
            return RedirectToAction(nameof(Index));
            
            ViewData["CategoryId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "AssetCategory"), "Id", "Description", fixedAsset.CategoryId);
            ViewData["ResponsibleEmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", fixedAsset.ResponsibleEmployeeId);
            return View(fixedAsset);
        }

        // GET: FixedAssets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fixedAsset = await _context.FixedAssets.FindAsync(id);
            if (fixedAsset == null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "AssetCategory"), "Id", "Description", fixedAsset.CategoryId);
            ViewData["ResponsibleEmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", fixedAsset.ResponsibleEmployeeId);
            return View(fixedAsset);
        }

        // POST: FixedAssets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FixedAsset fixedAsset, IFormFile assetphoto)
        {
            if (id != fixedAsset.Id)
            {
                return NotFound();
            }

            // Image
            if (assetphoto != null && assetphoto.Length > 0)
            {
                var fileName = "AssetPhoto" + DateTime.Now.ToString("yyymmddhhmmss") + "_" + assetphoto.FileName;
                var path = _configuration["FileSettings:UploadFolder"]!; // appsetting.json
                var filePath = Path.Combine(path, fileName);
                var stream = new FileStream(filePath, FileMode.Create);
                await assetphoto.CopyToAsync(stream);
                fixedAsset.Photo = fileName;
            }

            var fixedassetstatus = await _context.SystemCodeDetails
                    .Include(x => x.SystemCode)
                    .Where(x => x.SystemCode.Code == "AssetStatus"
                    && x.Code == "Active").FirstOrDefaultAsync();

            var Userid = User.GetUserId();
            fixedAsset.ModifiedById = Userid;
            fixedAsset.ModifiedOn = DateTime.Now;
            fixedAsset.StatusId = fixedassetstatus.Id;

            try
                {
                    _context.Update(fixedAsset);
                    await _context.SaveChangesAsync(Userid);
                TempData["Message"] = "Fixed Asset Details updated successfully";

            }
            catch (DbUpdateConcurrencyException)
                {
                    if (!FixedAssetExists(fixedAsset.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            TempData["Message"] = "Fixed Asset Details updated successfully";

            return RedirectToAction(nameof(Index));
            
            ViewData["CategoryId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "AssetCategory"), "Id", "Description", fixedAsset.CategoryId);
            ViewData["ResponsibleEmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", fixedAsset.ResponsibleEmployeeId);
            return View(fixedAsset);
        }

        // GET: FixedAssets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fixedAsset = await _context.FixedAssets
                .Include(f => f.CreatedBy)
                .Include(f => f.Location)
                .Include(f => f.ModifiedBy)
                .Include(f => f.ResponsibleEmployee)
                .Include(f => f.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fixedAsset == null)
            {
                return NotFound();
            }

            return View(fixedAsset);
        }

        // POST: FixedAssets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fixedAsset = await _context.FixedAssets.FindAsync(id);
            if (fixedAsset != null)
            {
                _context.FixedAssets.Remove(fixedAsset);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FixedAssetExists(int id)
        {
            return _context.FixedAssets.Any(e => e.Id == id);
        }
    }
}
