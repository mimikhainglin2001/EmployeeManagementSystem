using AutoMapper;
using EmployeesManagement.Data;
using EmployeesManagement.Models;
using EmployeesManagement.Services;
using EmployeesManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmployeesManagement.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IExtensionService _extensionService;

        public EmployeesController(ApplicationDbContext context, IConfiguration configuration, IExtensionService extensionService, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _extensionService = extensionService;
            _mapper = mapper;
        }

        // GET: Employees
        public async Task<IActionResult> Index(EmployeeViewModel employees)
        {
            var query = _context.Employees
                .Include(x => x.Status)
                .AsQueryable();

            // Full Name filter
            if (!string.IsNullOrWhiteSpace(employees.FullName))
            {
                query = query.Where(x => x.FullName.Contains(employees.FullName));
            }

            // Phone Number filter
            if (!string.IsNullOrWhiteSpace(employees.PhoneNumber))
            {
                query = query.Where(x => x.PhoneNumber == employees.PhoneNumber);
            }

            // Email filter
            if (!string.IsNullOrWhiteSpace(employees.EmailAddress))
            {
                query = query.Where(x => x.EmailAddress == employees.EmailAddress);
            }

            // Employee Number filter
            if (!string.IsNullOrWhiteSpace(employees.EmpNo))
            {
                query = query.Where(x => x.EmpNo == employees.EmpNo);
            }

            employees.Employees = await query.ToListAsync();

            return View(employees);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees

                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["BankId"] = new SelectList(_context.Banks, "Id", "Name");
            ViewData["EmploymentTermsId"] = new SelectList(
                _context.SystemCodeDetails
                    .Include(x => x.SystemCode)
                    .Where(x => x.SystemCode.Code == "EmploymentTerms"),
                "Id", "Description"
            );
            ViewData["DisabilityId"] = new SelectList(
               _context.SystemCodeDetails
                   .Include(x => x.SystemCode)
                   .Where(x => x.SystemCode.Code == "DisabilityTypes"),
               "Id", "Description"
           );
           
            ViewData["GenderId"] = new SelectList(
                _context.SystemCodeDetails
                    .Include(x => x.SystemCode)
                    .Where(x => x.SystemCode.Code == "Gender"),
                "Id", "Description"
            );

            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name");
            ViewData["DesignationId"] = new SelectList(_context.Designations, "Id", "Name");
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");

            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel newemployee, IFormFile employeephoto)
        {
            try
            {
            var employee = new Employee();
            _mapper.Map(newemployee, employee); // auto mapper
            employee.EmpNo = await _extensionService.GenerateEmployeeNumber(); //autogenerate
            
            // Image
            if (employeephoto.Length > 0)
            {
                var fileName = "EmployeePhoto_" + DateTime.Now.ToString("yyymmddhhmmss")+"_"+employeephoto.FileName;
                var path = _configuration["FileSettings:UploadFolder"]!; // appsetting.json
                var filePath = Path.Combine(path, fileName);
                var stream = new FileStream(filePath, FileMode.Create);
                await employeephoto.CopyToAsync(stream);
                employee.Photo = fileName;
            }

            var statusId = await _context.SystemCodeDetails
                    .Include(x => x.SystemCode)
                    .Where(x => x.SystemCode.Code == "EmployeeStatus"
                    && x.Code == "Active").FirstOrDefaultAsync();

                var userid = User.GetUserId();
            employee.CreatedById = userid;
            employee.CreatedOn = DateTime.Now;
            employee.StatusId = statusId.Id;

            _context.Add(employee);
                await _context.SaveChangesAsync(userid);
                TempData["Message"] = "Employee created successfully";
                return RedirectToAction(nameof(Index));

            ViewData["DisabilityId"] = new SelectList(
              _context.SystemCodeDetails
                  .Include(x => x.SystemCode)
                  .Where(x => x.SystemCode.Code == "DisabilityTypes"),
              "Id", "Description", employee.DisabilityId
          );

            ViewData["BankId"] = new SelectList(_context.Banks, "Id", "Name", employee.BankId);
            ViewData["EmploymentTermsId"] = new SelectList(
                _context.SystemCodeDetails
                    .Include(x => x.SystemCode)
                    .Where(x => x.SystemCode.Code == "EmploymentTerms"),
                "Id", "Description"
            , employee.EmploymentTermsId);
            ViewData["GenderId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "Gender"), "Id", "Description", employee.GenderId);
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", employee.CountryId);
            ViewData["DesignationId"] = new SelectList(_context.Designations, "Id", "Name", employee.DesignationId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", employee.DepartmentId);
            return View(employee);
        }
            catch(Exception ex)
            {
                TempData["Error"] = "Employee could not be created successfully"+ ex.Message;

                return View(newemployee);
            }
            }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["GenderId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "Gender"), "Id", "Description", employee.GenderId);
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", employee.CountryId);
            ViewData["DesignationId"] = new SelectList(_context.Designations, "Id", "Name", employee.DesignationId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", employee.DepartmentId);
            ViewData["BankId"] = new SelectList(_context.Banks, "Id", "Name", employee.BankId);
            ViewData["EmploymentTermsId"] = new SelectList(
               _context.SystemCodeDetails
                   .Include(x => x.SystemCode)
                   .Where(x => x.SystemCode.Code == "EmploymentTerms"),
               "Id", "Description", employee.EmploymentTermsId
           );
            ViewData["DisabilityId"] = new SelectList(
               _context.SystemCodeDetails
                   .Include(x => x.SystemCode)
                   .Where(x => x.SystemCode.Code == "DisabilityTypes"),
               "Id", "Description", employee.DisabilityId
           );
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Bank");
            ModelState.Remove("Country");
            ModelState.Remove("Department");
            ModelState.Remove("Designation");
            ModelState.Remove("Employee");
            ModelState.Remove("ApplicationUser");
            ModelState.Remove("CreatedBy");
            ModelState.Remove("ModifiedBy");
            ModelState.Remove("Gender");
            ModelState.Remove("Disability");
            ModelState.Remove("CauseofInactivity");
            ModelState.Remove("Reasonfortermination");
            ModelState.Remove("EmploymentTerms");
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //employee.ModifiedById = userId;
            //employee.ModifiedOn = DateTime.Now;

            try
            {
                var userid = User.GetUserId();
                employee.ModifiedById = userid;
                employee.ModifiedOn = DateTime.Now;
                _context.Update(employee);
                await _context.SaveChangesAsync(userid);
                TempData["Message"] = "Employee updated successfully";

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(employee.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
                TempData["Error"] = "Employee could not be updated successfully";

            }

            ViewData["GenderId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "Gender"), "Id", "Description", employee.GenderId);
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", employee.CountryId);
            ViewData["DesignationId"] = new SelectList(_context.Designations, "Id", "Name", employee.DesignationId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", employee.DepartmentId);
            ViewData["BankId"] = new SelectList(_context.Banks, "Id", "Name", employee.BankId);
            ViewData["EmploymentTermsId"] = new SelectList(
                _context.SystemCodeDetails
                    .Include(x => x.SystemCode)
                    .Where(x => x.SystemCode.Code == "EmploymentTerms"),
                "Id", "Description", employee.EmploymentTermsId
            );
            ViewData["DisabilityId"] = new SelectList(
               _context.SystemCodeDetails
                   .Include(x => x.SystemCode)
                   .Where(x => x.SystemCode.Code == "DisabilityTypes"),
               "Id", "Description",employee.DisabilityId
           );

            return RedirectToAction(nameof(Index));
        }
        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
