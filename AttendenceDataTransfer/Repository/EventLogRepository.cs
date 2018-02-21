using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Web;
using AttendenceDataTransfer.Models;

namespace AttendenceDataTransfer.Repository
{
    public class EventLogRepository
    {
        private readonly MssqlContext _mssqlContext = new MssqlContext();
        public List<EventLog> GetEventLogs()
        {
            return _mssqlContext.EventLogs.ToList();
        }

        public List<EventLog> GetEventLogsByDate(DateTime dateTime)
        {
            return _mssqlContext.EventLogs.Where(_ =>
                EntityFunctions.TruncateTime(new DateTime(_.nDateTime)) == dateTime).ToList();
        }
        public List<EventLog> GetEventLogsByFromToDate(DateTime from, DateTime to)
        {
            return _mssqlContext.EventLogs.Where(_=>
                EntityFunctions.TruncateTime(new DateTime(_.nDateTime)) >= from &&
                EntityFunctions.TruncateTime(new DateTime(_.nDateTime)) <= to).ToList();
        }
    }
}