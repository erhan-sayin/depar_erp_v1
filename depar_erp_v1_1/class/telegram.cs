using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace depar_erp_v1_1
{
    public class telegram
    {
        public static bool send_mesaj(string telegram_id, string telegram_mesaj)
        {
            try
            {
                string urlString = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}";
                //DEPAR TELEGRAM TOKEN
                string apiToken = "5634875013:AAEkH5Oa8uekqyIKnawnpJ1o5xtIghxwQm0";
                string chatId = telegram_id;
                string text = telegram_mesaj.Trim();
                urlString = String.Format(urlString, apiToken, chatId, text);

                WebRequest request = WebRequest.Create(urlString);
                Stream rs = request.GetResponse().GetResponseStream();
                StreamReader reader = new StreamReader(rs);
                string line = "";
                StringBuilder sb = new StringBuilder();
                while (line != null)
                {
                    line = reader.ReadLine();
                    if (line != null)
                        sb.Append(line);
                }
                string response = sb.ToString();
            }
            catch (Exception ex)
            {
                erp_log.log_at_av(telegram_id, ex.ToString());
                return false;
            }
            return true;
        }

        public static string telegram_userid(string username)
        {
            string deger = "";
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string query = " SELECT USERNAME,TELEGRAM_ID FROM USERS  WHERE TRIM(USERNAME)='"+username.Trim()+"' ";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    if (dr["TELEGRAM_ID"].ToString().Trim()=="" || dr["TELEGRAM_ID"].ToString().Trim()== null)
                    {
                        deger = "BOS";
                    }
                    else
                    {
                        deger = dr["TELEGRAM_ID"].ToString().Trim();
                    }
                }
                else
                {
                    string mesaj = username.Trim() +" ilgili kullanıcı için telegram id bulunamadı";
                    erp_log.log_at_av(username, mesaj);
                    return deger="BOS";
                }
                dr.Close();
                con.Close();
            }
            return deger;
        }

        public static void telegram_liste_yap(string mesaj, string[] telegram_liste)
        {
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                foreach (string i in telegram_liste)
                {
                    if (i.Trim()!="")
                    {
                        if (telegram_userid(i.Trim())!="BOS")
                        {
                            send_mesaj(telegram_userid(i.Trim()),mesaj);
                        }
                    }
                }      
                con.Close();
            }
        }
    }
}