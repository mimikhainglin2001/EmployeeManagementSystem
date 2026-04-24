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
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmployeesManagement.Controllers
{
    public class LeaveApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;


        public LeaveApplicationsController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {


            var leaveapplications = await _context.LeaveApplications
                .AsNoTracking()
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
                .Include(l => l.CreatedBy)
            

                .OrderByDescending(l => l.CreatedOn)
                .ToListAsync();

            return View(leaveapplications);
        }
        // GET: LeaveApplications
        public async Task<IActionResult> ApprovedLeaveApplications()
        {
            var approvedstatus = _context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveApprovalStatus" && y.Code == "Approved").FirstOrDefault();

            var applicationDbContext = _context.LeaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
                .Where(l => l.StatusId == approvedstatus.Id);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: LeaveApplications
        public async Task<IActionResult> RejectedLeaveApplications()
        {
            var rejectedstatus = _context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveApprovalStatus" && y.Code == "Rejected").FirstOrDefault();

            var applicationDbContext = _context.LeaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
                .Where(l => l.StatusId == rejectedstatus.Id);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: LeaveApplications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications
                .AsNoTracking()
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
                .Include(l => l.CreatedBy)
                .Include(l => l.ApprovedBy)
                .Include(l => l.ModifiedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveApplication == null)
            {
                return NotFound();
            }

            return View(leaveApplication);
        }

        // GET: LeaveApplications/Create
        public IActionResult Create()
        {
            ViewData["DurationId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveDuration"), "Id", "Description");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Name");
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ApproveLeave(int? id)
        {
            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
                .Include(l => l.CreatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveApplication == null)
            {
                return NotFound();
            }
            ViewData["DurationId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveDuration"), "Id", "Description");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Name");
            return View(leaveApplication);
        }
        [HttpPost]
        public async Task<IActionResult> ApproveLeave(LeaveApplication leave)
        {
            try
            {
                var approvedstatus = await _context.SystemCodeDetails
                    .Include(x => x.SystemCode)
                    .FirstOrDefaultAsync(y =>
                        y.SystemCode.Code == "LeaveApprovalStatus" &&
                        y.Code == "Approved");

                var adjustmenttype = await _context.SystemCodeDetails
                    .Include(x => x.SystemCode)
                    .FirstOrDefaultAsync(y =>
                        y.SystemCode.Code == "LeaveAdjustment" &&
                        y.Code == "Negative");

                if (approvedstatus == null)
                    throw new Exception("Approved status not found");

                if (adjustmenttype == null)
                    throw new Exception("Adjustment type not found");

                var leaveApplication = await _context.LeaveApplications
                    .Include(l => l.Duration)
                    .Include(l => l.Employee)
                    .Include(l => l.LeaveType)
                    .Include(l => l.Status)
                    .FirstOrDefaultAsync(m => m.Id == leave.Id);

                if (leaveApplication == null)
                    return NotFound();

                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);

                leaveApplication.ApprovedOn = DateTime.Now;
                leaveApplication.ApprovedById = userid;
                leaveApplication.StatusId = approvedstatus.Id;
                leaveApplication.ApprovalNotes = leave.ApprovalNotes;

                var adjustment = new LeaveAdjustmentEntry
                {
                    EmployeeId = leaveApplication.EmployeeId,
                    NoOfDays = leaveApplication.NoOfDays,
                    LeaveStartDate = leaveApplication.StartDate,
                    LeaveEndDate = leaveApplication.EndDate,
                    AdjustmentDescription = "Leave Taken - Negative Adjustment",
                    LeavePeriodId = 1,
                    LeaveAdjustmentDate = DateTime.Now,
                    AdjustmentTypeId = adjustmenttype.Id
                };

                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Id == leaveApplication.EmployeeId);

                if (employee == null)
                    throw new Exception("Employee not found");

                employee.LeaveOustandingBalance =
                    employee.AllocatedLeaveDays - leaveApplication.NoOfDays;

                _context.Update(leaveApplication);
                _context.Add(adjustment);
                _context.Update(employee);

                await _context.SaveChangesAsync(userid);

                TempData["Message"] = "Leave Application approved successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                ViewData["DurationId"] = new SelectList(
                    _context.SystemCodeDetails.Include(x => x.SystemCode)
                    .Where(y => y.SystemCode.Code == "LeaveDuration"),
                    "Id", "Description", leave.DurationId);

                ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", leave.EmployeeId);

                ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Name", leave.LeaveTypeId);

                return View(leave);
            }
        }

        [HttpGet]
        public async Task<IActionResult> RejectLeave(int? id)
        {
            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveApplication == null)
            {
                return NotFound();
            }
            ViewData["DurationId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveDuration"), "Id", "Description");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Name");
            return View(leaveApplication);
        }
        [HttpPost]
        public async Task<IActionResult> RejectLeave(LeaveApplication leave)
        {
            try
            {


                var rejectedstatus = _context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveApprovalStatus" && y.Code == "Rejected").FirstOrDefault();
                var leaveApplication = await _context.LeaveApplications
                    .Include(l => l.Duration)
                    .Include(l => l.Employee)
                    .Include(l => l.LeaveType)
                    .Include(l => l.Status)
                    .FirstOrDefaultAsync(m => m.Id == leave.Id);
                if (leaveApplication == null)
                {
                    return NotFound();
                }
                leaveApplication.ApprovedOn = DateTime.Now;
                leaveApplication.ApprovedById = User.GetUserId();
                leaveApplication.StatusId = rejectedstatus!.Id;
                leaveApplication.ApprovalNotes = leave.ApprovalNotes;

                _context.Update(leaveApplication);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Leave Application rejected successfully";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                TempData["Error"] = "Leave Application could not be rejected successfully" + ex.Message;

            }
            ViewData["DurationId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveDuration"), "Id", "Description", leave.DurationId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", leave.EmployeeId);
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Name", leave.LeaveTypeId);
            return View(leave);
        }

        // POST: LeaveApplications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveApplication leaveApplication, IFormFile leaveattachment)
        {
            try
            {


                if (leaveattachment != null && leaveattachment.Length > 0)
                {
                    var fileName = "LeaveAttachment" + DateTime.Now.ToString("yyymmddhhmmss") + "_" + leaveattachment.FileName;
                    var path = _configuration["FileSettings:UploadFolder"]!; // appsetting.json
                    var filePath = Path.Combine(path, fileName);
                    var stream = new FileStream(filePath, FileMode.Create);
                    await leaveattachment.CopyToAsync(stream);
                    leaveApplication.Attachment = fileName;
                }
                leaveApplication.EndDate = leaveApplication.StartDate.AddDays(leaveApplication.NoOfDays);
                // Get Pending status correctly with await
                var pendingStatus = await _context.SystemCodeDetails
                    .Include(x => x.SystemCode)
                    .Where(y => y.Code == "AwaitingApproval" && y.SystemCode.Code == "LeaveApprovalStatus")
                    .FirstOrDefaultAsync();

                var userid = User.GetUserId();
                leaveApplication.CreatedOn = DateTime.Now;
                leaveApplication.CreatedById = userid;
                leaveApplication.StatusId = pendingStatus.Id;
                leaveApplication.NoOfDays = (leaveApplication.EndDate - leaveApplication.StartDate).Days + 1;

                _context.Add(leaveApplication);
                await _context.SaveChangesAsync(userid);

                // Leave Type
                var documenttype = await _context.SystemCodeDetails.Include(x => x.SystemCode).Where(x => x.SystemCode.Code == "DocumentTypes" && x.Code == "LeaveApplication").FirstOrDefaultAsync();

                // Wrokflow UserGroup
                var usergroup = await _context.ApprovalsUserMetrixs.Where(x => x.UserId == userid && x.DocumentTypeId == documenttype.Id && x.Active == true).FirstOrDefaultAsync();
                var awaitingapproval = await _context.SystemCodeDetails
                    .Include(x => x.SystemCode)
                    .Where(y => y.Code == "AwaitingApproval" && y.SystemCode.Code == "LeaveApprovalStatus")
                    .FirstOrDefaultAsync();
                var approvers = await _context.WorkFlowUserGroupMembers.Where(x => x.WorkFlowUserGroupId == usergroup.WorkflowUserGroupId && x.SenderId == userid).ToListAsync();
                foreach (var approver in approvers)
                {
                    // Generate AN Approval Entries

                    var approvalentries = new ApprovalEntry()
                    {
                        ApproverId = approver.ApproverId,
                        DateSentForApproval = DateTime.Now,
                        LastModifiedOn = DateTime.Now,
                        LastModifiedById = approver.SenderId,
                        RecordId = leaveApplication.Id,
                        ControllerName = "LeaveApplications",
                        DocumentTypeId = documenttype.Id,
                        SequenceNo = approver.SequenceNo,
                        StatusId = awaitingapproval.Id,
                        Comments = "Sent for Approval"
                    };
                    _context.Add(approvalentries);
                }
                await _context.SaveChangesAsync(userid);
                TempData["Message"] = "Leave Application created successfully";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occured while creating Leave application" + ex.Message;
                return View(leaveApplication);

            }


            // Repopulate dropdowns
            ViewData["DurationId"] = new SelectList(
            _context.SystemCodeDetails.Include(x => x.SystemCode)
                    .Where(y => y.SystemCode.Code == "LeaveDuration"),
                "Id", "Description", leaveApplication.DurationId);

            ViewData["EmployeeId"] = new SelectList(
                _context.Employees, "Id", "FullName", leaveApplication.EmployeeId);

            ViewData["LeaveTypeId"] = new SelectList(
                _context.LeaveTypes, "Id", "Name", leaveApplication.LeaveTypeId);

        }

        // GET: LeaveApplications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var pendingStatus = _context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.Code == "Pending" && y.SystemCode.Code == "LeaveApprovalStatus").FirstOrDefaultAsync(); ;

            var leaveApplication = await _context.LeaveApplications.FindAsync(id);
            if (leaveApplication == null)
            {
                return NotFound();
            }
            ViewData["DurationId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveDuration"), "Id", "Description", leaveApplication.DurationId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", leaveApplication.EmployeeId);
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Name", leaveApplication.LeaveTypeId);
            return View(leaveApplication);
        }

        // POST: LeaveApplications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveApplication leaveApplication)
        {
            if (id != leaveApplication.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var pendingStatus = await _context.SystemCodeDetails
                    .Include(x => x.SystemCode)
                    .Where(y => y.Code == "Pending" && y.SystemCode.Code == "LeaveApprovalStatus")
                    .FirstOrDefaultAsync();

                try
                {
                    leaveApplication.ModifiedOn = DateTime.Now;
                    leaveApplication.ModifiedById = "Macro Code";
                    leaveApplication.StatusId = pendingStatus.Id;

                    _context.Update(leaveApplication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveApplicationExists(leaveApplication.Id))
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
            ViewData["DurationId"] = new SelectList(_context.SystemCodeDetails, "Id", "Description", leaveApplication.DurationId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", leaveApplication.EmployeeId);
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Name", leaveApplication.LeaveTypeId);
            return View(leaveApplication);
        }

        // GET: LeaveApplications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveApplication == null)
            {
                return NotFound();
            }

            return View(leaveApplication);
        }

        // POST: LeaveApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveApplication = await _context.LeaveApplications.FindAsync(id);
            if (leaveApplication != null)
            {
                _context.LeaveApplications.Remove(leaveApplication);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveApplicationExists(int id)
        {
            return _context.LeaveApplications.Any(e => e.Id == id);
        }
    }
}
