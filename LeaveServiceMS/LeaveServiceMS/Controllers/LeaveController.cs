using LeaveServiceMS.Context;
using LeaveServiceMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeaveServiceMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController : ControllerBase
    {
        private readonly LeaveDbContext _context;

        public LeaveController(LeaveDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Employee")]
        [HttpPost("apply")]
        public async Task<IActionResult> ApplyLeave(ApplyLeaveRequest request)
        {
            var username = User.Identity?.Name;
            var daysRequested = (request.EndDate - request.StartDate).Days + 1;

            var balance = await _context.LeaveBalances.FirstOrDefaultAsync(b => b.Username == username);
            if (balance == null)
            {
                balance = new LeaveBalance { Username = username, AvailableDays = 20 };
                _context.LeaveBalances.Add(balance);
            }

            if (daysRequested > balance.AvailableDays)
                return BadRequest("Insufficient leave balance.");

            var leave = new LeaveRequest
            {
                EmployeeUsername = username,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };

            _context.LeaveRequests.Add(leave);
            await _context.SaveChangesAsync();

            return Ok("Leave request submitted.");
        }

        [Authorize(Roles = "Manager")]
        [HttpPost("decision")]
        public async Task<IActionResult> ApproveOrReject(DecisionRequest decision)
        {
            var leave = await _context.LeaveRequests.FirstOrDefaultAsync(r => r.Id == decision.RequestId);

            if (leave == null) return NotFound("Leave request not found.");
            if (leave.Status != "Pending") return BadRequest("Request already decided.");

            leave.Status = decision.IsApproved ? "Approved" : "Rejected";
            leave.ManagerComment = decision.ManagerComment;

            if (decision.IsApproved)
            {
                var days = (leave.EndDate - leave.StartDate).Days + 1;
                var balance = await _context.LeaveBalances.FirstOrDefaultAsync(b => b.Username == leave.EmployeeUsername);
                if (balance != null)
                    balance.AvailableDays -= days;
            }

            await _context.SaveChangesAsync();

            // Notification placeholder
            Console.WriteLine($"[Notification] {leave.EmployeeUsername}, your leave request was {leave.Status}");

            return Ok("Decision updated.");
        }

        [Authorize]
        [HttpGet("my-leaves")]
        public async Task<IActionResult> GetMyLeaves()
        {
            var username = User.Identity?.Name;
            var myLeaves = await _context.LeaveRequests
                .Where(r => r.EmployeeUsername == username)
                .ToListAsync();

            return Ok(myLeaves);
        }

        [Authorize(Roles = "HR/Admin")]
        [HttpGet("balances")]
        public async Task<IActionResult> GetAllBalances()
        {
            var balances = await _context.LeaveBalances.ToListAsync();
            return Ok(balances);
        }

        [Authorize(Roles = "HR/Admin")]
        [HttpGet("report")]
        public async Task<IActionResult> LeaveReport()
        {
            var report = await _context.LeaveRequests.ToListAsync();
            return Ok(report);
        }
    }
}
