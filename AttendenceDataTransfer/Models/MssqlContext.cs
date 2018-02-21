using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
namespace AttendenceDataTransfer.Models
{
    public class MssqlContext: DbContext
    {
        public MssqlContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<EventLog> EventLogs { get; set; }
    }
}