using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using AttendenceDataTransfer.Models;

namespace AttendenceDataTransfer.Service
{
    public class TransferDataService
    {
        public List<OracleDB> Transfer(List<EventLog> eventLogs)
        {
            List<OracleDB> oracleDb = new List<OracleDB>();
            foreach (var eventLog in eventLogs)
            {
                oracleDb.Add(new OracleDB
                {
                    ZID = 100002,
                    XPFLAG = "012",
                    XINSTALNO = eventLog.nUserID,
                    XDATE = Misc.Misc.UnixTimeStampToDateTime(eventLog.nDateTime).Date,
                    XDESC = Misc.Misc.UnixTimeStampToDateTime(eventLog.nDateTime).ToString("HH:mm:ss")
                });
            }
            return oracleDb;
        }
    }
}