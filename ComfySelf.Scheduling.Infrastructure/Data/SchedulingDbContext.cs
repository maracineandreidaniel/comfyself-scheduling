using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComfySelf.Scheduling.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ComfySelf.Scheduling.Infrastructure.Data
{
    public class SchedulingDbContext(DbContextOptions<SchedulingDbContext> options) : DbContext(options)
    {
       public DbSet<Appointment> Appointments {  get; set; }
    }
}
