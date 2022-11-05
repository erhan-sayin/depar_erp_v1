using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace depar_erp_v1_1
{
    public class erp_log
    {
        public static void log_at_av( string username, string mesaj)
        {
            string path = @"\\192.168.1.4\DEPAR_ERP_LOG\log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (!File.Exists(path))
            {
                File.CreateText(path).Dispose();
            }
            using (TextWriter txt = File.AppendText(path))
            {
                txt.WriteLine(DateTime.Now + "----" + username.Trim() + "----" + mesaj.Trim());
                txt.WriteLine("———————————————————————————");
                txt.Dispose();
            }
        }

    }
}