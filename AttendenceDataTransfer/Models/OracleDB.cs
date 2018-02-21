using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendenceDataTransfer.Models
{
    public class OracleDB
    {
        public int ZID { get; set; }
        public int XINSTALNO { get; set; }
        public DateTime XDATE { get; set; }
        public string XDESC { get; set; }
        public string XPFLAG { get; set; }
    }
}