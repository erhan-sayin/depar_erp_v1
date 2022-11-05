using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace depar_erp_v1_1
{
    public partial class dashboard : System.Web.UI.Page
    {
        public class usr_name
        {
            public static string username;
            public static string username_full;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }
            usr_name.username = HttpContext.Current.User.Identity.Name.ToString().Trim();
            if (!Page.IsPostBack)
            {
                doviz_al();
                get_LME_info();

                //BindGvData();
                //BindChart();
            }
        }

        //private void BindChart()
        //{
        //    DataTable dsChartData = new DataTable();
        //    StringBuilder strScript = new StringBuilder();
        //    try
        //    {
        //        dsChartData = GetChartData();

        //        strScript.Append(@"<script type='text/javascript'>  
        //            google.load('visualization', '1', {packages: ['corechart']});</script>  
  
        //            <script type='text/javascript'>  
        //            function drawVisualization() {         
        //            var data = google.visualization.arrayToDataTable([  
        //            ['Tarih', 'Bakır', 'Kalay', 'Kursun', 'Nikel','Alüminyum'],");

        //        foreach (DataRow row in dsChartData.Rows)
        //        {
        //            strScript.Append("['" + row["TRH"] + "'," + row["BAKIR"] + "," +
        //                row["KALAY"] + "," + row["KURSUN"] + "," + row["NIKEL"] + ","+ row["ALUMINYUM"] +"],");
        //        }
        //        strScript.Remove(strScript.Length - 1, 1);
        //        strScript.Append("]);");

        //        strScript.Append("var options = { title : 'lME Trendi', vAxis: {title: 'LME'},  hAxis: {title: 'Tarih'}, seriesType: 'bars', series: {3: {type: 'area'}} };");
        //        strScript.Append(" var chart = new google.visualization.ComboChart(document.getElementById('chart_div_2'));  chart.draw(data, options); } google.setOnLoadCallback(drawVisualization);");
        //        strScript.Append(" </script>");

        //        ltScripts.Text = strScript.ToString();
        //    }
        //    catch
        //    {
        //    }
        //    finally
        //    {
        //        dsChartData.Dispose();
        //        strScript.Clear();
        //    }
        //}

        //private void BindGvData()
        //{
        //    gvData.DataSource = GetChartData();
        //    gvData.DataBind();
        //}

        private DataTable GetChartData()
        {
            DataSet ds = new DataSet();

            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = " Select FORMAT(TARIH,'yyyy-MM-dd') as TRH,BAKIR,KALAY,KURSUN,NIKEL,ALUMINYUM from LME where TARIH>dateadd(day,-8,getdate()) ";
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);

                adp.Fill(ds);
                return ds.Tables[0];
                con.Close();
            }

        }

        private void get_LME_info()
        {
            string adres = "http://www.metalsmarket.net/w_lmeCashSett.html";
            WebClient client = new WebClient();
            string htmlString = client.DownloadString(adres);
            HtmlAgilityPack.HtmlDocument htmlBelgesi = new HtmlAgilityPack.HtmlDocument();
            htmlBelgesi.OptionFixNestedTags = true;
            htmlBelgesi.LoadHtml(htmlString);

            //tarih
            HtmlAgilityPack.HtmlNodeCollection tarih = htmlBelgesi.DocumentNode.SelectNodes("//td[@id='_21224']");
            //alüminyum
            HtmlAgilityPack.HtmlNodeCollection al_offer = htmlBelgesi.DocumentNode.SelectNodes("//td[@id='_21256']");
            //bakır
            HtmlAgilityPack.HtmlNodeCollection cu_offer = htmlBelgesi.DocumentNode.SelectNodes("//td[@id='_21192']");
            //çinko
            HtmlAgilityPack.HtmlNodeCollection zn_offer = htmlBelgesi.DocumentNode.SelectNodes("//td[@id='_21384']");
            //nikel 
            HtmlAgilityPack.HtmlNodeCollection ni_offer = htmlBelgesi.DocumentNode.SelectNodes("//td[@id='_21448']");
            //Kurşun
            HtmlAgilityPack.HtmlNodeCollection pb_offer = htmlBelgesi.DocumentNode.SelectNodes("//td[@id='_21512']");


            if (al_offer != null) // Etiket bulabildiyse ... (Şarta uyan etiket bulunamadıysa null döndürüyor.)
            {
                // Aranan secilenler'in ilk elemanıdır. Bunun InnerText özellik değerini label1'in
                // Text özelliğine ata.
                lme_tarih.Text = tarih[0].InnerText.Replace('/', '.');
                lbl_lme_al.Text = al_offer[0].InnerText;
                lbl_lme_cu.Text = cu_offer[0].InnerText;
                lbl_lme_zn.Text = zn_offer[0].InnerText;
                lbl_lme_ni.Text = ni_offer[0].InnerText;
                lbl_lme_pb.Text = pb_offer[0].InnerText;
            }
            else
            {
                lme_tarih.Text = "";
                lbl_lme_al.Text = "";
                lbl_lme_cu.Text = "";
                lbl_lme_zn.Text = "";
                lbl_lme_ni.Text = "";
                lbl_lme_pb.Text = "";

            }
        }

        private void doviz_al()
        {
            try
            {
                XmlDocument xmlVerisi = new XmlDocument();
                xmlVerisi.Load("http://www.tcmb.gov.tr/kurlar/today.xml");

                decimal dolar = Convert.ToDecimal(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "USD")).InnerText.Replace('.', ','));
                decimal euro = Convert.ToDecimal(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "EUR")).InnerText.Replace('.', ','));
                decimal sterlin = Convert.ToDecimal(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "GBP")).InnerText.Replace('.', ','));

                lbl_tarih_today.Text=DateTime.Now.ToString("dd/MM/yyyy");
                lbl_usd.Text = dolar.ToString();
                lbl_eur.Text = euro.ToString();
                lbl_gbp.Text = sterlin.ToString();
            }
            catch (XmlException)
            {
                throw;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<object> table_Client2()
        {

            string query =  " select A.SIPNO,A.CH_ADI, B.STOK_AD,B.STOK_KOD," +
                            " FORMAT(B.TESLIM_TARIH, 'yyyy-MM-dd')  AS TESLIM_TARIH, FORMAT (A.ONAY_DATE, 'yyyy-MM-dd') AS ONAY_TARIH," +
                            " (DATEDIFF(dd, GETDATE(), FORMAT(A.ONAY_DATE, 'yyyy-MM-dd'))+1)-(DATEDIFF(wk, GETDATE(), FORMAT(A.ONAY_DATE, 'yyyy-MM-dd'))*2)-(case when DATENAME(dw, GETDATE())='Sunday' then 1 else 0 end )-(case when DATENAME(dw, FORMAT (A.ONAY_DATE,'yyyy-MM-dd'))='Saturday' then 1 else 0 end ) as FARK "+
                            " from SIPARISE A "+
                            " LEFT OUTER JOIN SIPARIST B ON(TRIM(A.SIPNO)=TRIM(B.SIPNO)) " +
                            " LEFT OUTER JOIN STOK_KART C ON(RTRIM(B.STOK_KOD)= RTRIM(C.KOD)) "+
                            " WHERE A.DURUM NOT IN('5', '6') ";

            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            List<object> chartData = new List<object>();
            chartData.Add(new object[]
            {
        "Sipariş No", "Müşteri Adı","Stok Ad","Stok Kodu","Teslim Tarihi","Sipariş Onay Tarihi","Kalan Zaman"
            });
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            chartData.Add(new object[]
                            {
                        sdr["SIPNO"], sdr["CH_ADI"],sdr["STOK_AD"],sdr["STOK_KOD"],sdr["TESLIM_TARIH"],sdr["ONAY_TARIH"],sdr["FARK"]
                            });
                        }
                    }
                    con.Close();
                    return chartData;
                }
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<object> GetPieChartData_Client()
        {
            string query = "  select A.CH_ADI, COUNT(DISTINCT(A.SIPNO)) AS SIP_SAYISI "+
                            " from SIPARISE A  LEFT OUTER JOIN SIPARIST B ON(TRIM(A.SIPNO)=TRIM(B.SIPNO)) "+
                             " WHERE CAST(FORMAT (GETDATE(),'yyyy-MM-dd') AS DATE) >= CAST(FORMAT(B.TESLIM_TARIH, 'yyyy-MM-dd') AS DATE) "+
                              " GROUP BY A.CH_ADI ";

            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            List<object> chartData = new List<object>();
            chartData.Add(new object[]
            {
        "Müşteri Adı", "Sipariş Sayısı"
            });
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            chartData.Add(new object[]
                            {
                        sdr["CH_ADI"], sdr["SIP_SAYISI"]
                            });
                        }
                    }
                    con.Close();
                    return chartData;
                }
            }
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<object> GetlineChartData_Client()
        {
            string query = " Select FORMAT(TARIH,'yyyy-MM-dd') as TRH,BAKIR,KALAY,KURSUN,NIKEL,ALUMINYUM from LME where TARIH>dateadd(day,-8,getdate()) ";

            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            List<object> chartData = new List<object>();
            chartData.Add(new object[]
            {
        "Tarih(gün)", "Bakır","Kalay","Kursun","Nikel","Alüminyum"
            });
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            chartData.Add(new object[] { Convert.ToDateTime(sdr["TRH"]), sdr["BAKIR"], sdr["KALAY"], sdr["KURSUN"], sdr["NIKEL"], sdr["ALUMINYUM"] });
                        }
                    }
                    con.Close();
                    return chartData;
                }
            }
        }

    }
}