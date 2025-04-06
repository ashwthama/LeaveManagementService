using LeaveServiceMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LeaveServiceMS.Context
{
    public class LeaveDbContext: DbContext
    {
        public LeaveDbContext(DbContextOptions<LeaveDbContext> options) : base(options) { }

        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveBalance> LeaveBalances { get; set; }
    }
}
