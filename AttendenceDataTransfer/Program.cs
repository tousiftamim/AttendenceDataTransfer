using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AttendenceDataTransfer.Models;
using AttendenceDataTransfer.Service;
using Oracle.ManagedDataAccess.Client;

namespace AttendenceDataTransfer
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Date Transfer Started.......");

                string serverName = "RC-COLA-ATTENDA\\BSSERVER";

/*
                string serverName = "DESKTOP-J4N60QI\\SQLEXPRESS";
*/

                //string serverName = "DESKTOP-4LARUBE\\SQLEXPRESS";DESKTOP-J4N60QI\SQLEXPRESS
                string dbName = "BioStar";
                List<EventLog> eventLogs = new List<EventLog>();
                using (SqlConnection conn = new SqlConnection())
                {
                    Console.WriteLine("Retreiving Data from SQL Server.......");
                    conn.ConnectionString = "Server=" + serverName + ";Database=" + dbName + ";Trusted_Connection=true";
                    SqlCommand cmd = new SqlCommand("SELECT nDateTime,nUserID  FROM TB_EVENT_LOG", conn);
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            var indexOfColumn2 = reader.GetOrdinal("nDateTime");
                            var indexOfColumn3 = reader.GetOrdinal("nUserID");

                            while (reader.Read())
                            {
                                eventLogs.Add(new EventLog
                                {
                                    nDateTime = reader.GetInt32(indexOfColumn2),
                                    nUserID = reader.GetInt32(indexOfColumn3)

                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    conn.Close();
                    
                    Console.WriteLine("Data Retreive Successful.......");
                    TransferDataService dataService = new TransferDataService();
                    var convertedEventLog = dataService.Transfer(eventLogs);
                    foreach (var oracleDB in convertedEventLog)
                    {
                        Console.WriteLine("XINSTALNO = " + oracleDB.XINSTALNO);
                        Console.WriteLine("XDATE = " + oracleDB.XDATE);
                        Console.WriteLine("XDESC = " + oracleDB.XDESC);
                        Console.WriteLine("____________________________________________");
                    }

                    Console.WriteLine("Data Conversion Successful.......");
                    Console.WriteLine("Saving Data in Oracle.......");
                    string oradb = "Data Source=localhost:1521/orcl;User Id=pblfhr;Password=pblfhr;";
                    //string oradb = "Data Source=localhost:1521/xe;User Id=icebreakers;Password=icebreakers;";

                    OracleConnection connOracle = new OracleConnection(oradb); // C#

                    connOracle.Open();

                    OracleCommand cmdOracle = new OracleCommand { Connection = connOracle };
                    if (convertedEventLog.Count > 0)
                    {
                        var str = "INSERT ALL ";
                        foreach (var oracleDB in convertedEventLog)
                        {
                            str = str +
                                  "INTO pratdummy (ZID, XINSTALNO, XDATE, XDESC, XPFLAG)" +
                                  " VALUES (" + oracleDB.ZID + ","
                                  + oracleDB.XINSTALNO + ", "
                                  + "TO_DATE('" + oracleDB.XDATE.ToString("dd/MM/yyyy") + "','DD/MM/YY'), "
                                  + "'" + oracleDB.XDESC + "', "
                                  + "'" + oracleDB.XPFLAG + "') ";
                        }
                        str = str + "SELECT 1 FROM DUAL " +
                              "WHERE NOT EXISTS (SELECT ZID, XINSTALNO, XDATE, XDESC, XPFLAG FROM pratdummy)";

                        cmdOracle.CommandText = str;
                        cmdOracle.CommandType = CommandType.Text;
                        OracleDataReader dr = cmdOracle.ExecuteReader();

                    }

                    connOracle.Close();

                    Console.WriteLine("Data Successfully Saved in Oracle.......");

                    Thread.Sleep(TimeSpan.FromSeconds(30));

                }
            }

        }
    }
}
