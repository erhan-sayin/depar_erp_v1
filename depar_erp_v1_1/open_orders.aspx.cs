using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace depar_erp_v1_1
{
    public partial class open_orders : System.Web.UI.Page
    {
        public class usr_name
        {
            public static string username;
            public static string username_full;
            public static string username_role;
            public static bool onay;
            public static string siparis_durum;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }
            usr_name.username = HttpContext.Current.User.Identity.Name.ToString().Trim();
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string query = " SELECT USER_ADI,ROLE FROM USERS  WHERE TRIM(USERNAME)='"+usr_name.username.Trim()+"' ";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    usr_name.username_full = dr["USER_ADI"].ToString().Trim();
                    usr_name.username_role=dr["ROLE"].ToString().Trim();
                }
                dr.Close();
                con.Close();
            }
            if (!Page.IsPostBack)
            {
                order_bindign();
            }
        }

        private void order_bindign()
        {
            grid_binding();
        }

        private void grid_binding()
        {
            string query = "";
            string constr2 = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con2 = new SqlConnection(constr2))
            {
                con2.Open();
                query = " SELECT B.ID,B.DURUM,A.SIPNO,B.SATIR_NOT,A.CH_ADI,B.STOK_KOD,B.STOK_AD,B.MIKTAR,B.KALAN_MIKTAR,B.BIRIM,A.TARIH,FORMAT(B.TESLIM_TARIH,'yyy-MM-dd') as TESLIM_TARIH, " +
                        " DATEDIFF(DAY ,GETDATE(),FORMAT(B.TESLIM_TARIH,'yyy-MM-dd')) AS FARK," +
                        " A.URETIM_NOTLAR, " +
                        " case when C.DEPAR_KOD<>'' THEN C.DEPAR_KOD ELSE CONCAT(TRIM(C.KOD), '-', TRIM(C.KOD_AD)) END DEPAR_KOD, " +
                        " concat(B.REVIZYON,'-',C1.ACIKLAMA) AS REVIZYON,B.SATIR_NOT,B.K_MIN,B.K_MAX,B.YU_MAX,B.YU_MIN,B.YAG_MAX,B.YAG_MIN,B.CI_MAX,B.CI_MIN, "+
                        " TRIM(D.GRUPKODU_AD) AS GK_5_AD,TRIM(D1.GRUPKODU_AD) AS GK_6_AD,TRIM(D2.GRUPKODU_AD) AS GK_4_AD,TRIM(D3.GRUPKODU_AD) AS GK_3_AD," +
                        " CASE  "+
                        " WHEN B.DURUM='0'  THEN 'Onay Bekliyor' " +
                        " WHEN B.DURUM='1'  THEN 'Planlamada' " +
                        " WHEN B.DURUM='2'  THEN 'Üretimde' " +
                        " WHEN B.DURUM='3'  THEN 'Kalite Kontrolde' " +
                        " WHEN B.DURUM='4'  THEN 'Depoda' " +
                        " WHEN B.DURUM='5'  THEN 'Kısmı Sevkiyat'" +
                        " WHEN B.DURUM='6'  THEN 'Sevkiyat'" +
                        " END DURUM_AD  " +
                        " FROM SIPARISE A "+
                        " LEFT OUTER JOIN SIPARIST B ON(TRIM(A.SIPNO)= TRIM(B.SIPNO)) "+
                        " LEFT OUTER JOIN STOK_KART C ON(TRIM(B.STOK_KOD)= TRIM(C.KOD)) "+
                        " LEFT OUTER JOIN STOK_KART_REV C1 ON( TRIM(B.STOK_KOD)= TRIM(C1.KOD) AND  TRIM(B.REVIZYON)=TRIM(C1.REVIZYON) ) "+
                        " LEFT OUTER JOIN GRUPKODU D ON(TRIM(C.GK_5)= TRIM(D.GRUPKODU) AND TRIM(D.KOD)= 'GK_5') " +
                        " LEFT OUTER JOIN GRUPKODU D1 ON(TRIM(C.GK_6)= TRIM(D1.GRUPKODU) AND TRIM(D1.KOD)= 'GK_6') " +
                        " LEFT OUTER JOIN GRUPKODU D2 ON(TRIM(C.GK_4)= TRIM(D2.GRUPKODU) AND TRIM(D2.KOD)= 'GK_4') " +
                        " LEFT OUTER JOIN GRUPKODU D3 ON(TRIM(C.GK_3)= TRIM(D3.GRUPKODU) AND TRIM(D3.KOD)= 'GK_3') " +
                        //SİPARİŞ ONAYI TAMAMLANMAMIŞ OLANLAR VE DURUM KODU ONAY VERİLMEMİŞ OLANLARIN HEPSİ GETİRİLİYOR
                        " WHERE 1=1 AND TRIM(B.DURUM) NOT IN ('0','6') " +
                        " ORDER  BY ID DESC ";
                //" ORDER  BY DATEDIFF(DAY ,GETDATE(),FORMAT(B.TESLIM_TARIH,'yyy-MM-dd')) ASC ";

                SqlCommand cmd2 = new SqlCommand(query, con2);
                SqlDataAdapter adp2 = new SqlDataAdapter(cmd2);
                DataSet ds2 = new DataSet();
                adp2.Fill(ds2);
                grd_siparis_uretim.DataSource = ds2;
                grd_siparis_uretim.DataBind();

                DataTable dtTemp = new DataTable();
                ViewState["dtbl"] = dtTemp;
                adp2.Fill(dtTemp);

                con2.Close();
            }
        }

        protected void new_stok_kart_Click(object sender, EventArgs e)
        {
            Response.Write("<script lang='JavaScript'>alert('iş emri süreç kurgusu devam etmektedir.');</script>");
            return;
        }

        protected void statu_update(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            int a = 0;
            string dep_kod = "";
            foreach (GridViewRow grow in grd_siparis_uretim.Rows)
            {
                CheckBox chk = (CheckBox)grow.FindControl("chc_sec");
                if (chk!= null & chk.Checked)
                {
                    a=a+1;
                    dep_kod=grow.Cells[5].Text.Trim();
                    if (dep_kod.Trim()=="-" || dep_kod.Trim()=="")
                    {
                        Response.Write("<script lang='JavaScript'>alert('Siparş statusü güncellemek  öncelikle "+grow.Cells[2].Text.Trim()+" nolu siparişteki depar kodunu güncelleyiniz..Kayıt yapılmadı');</script>");
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ChangeForm(12)", true);
                        return;
                    }
                }
            }
            if (a==0)
            {
                Response.Write("<script lang='JavaScript'>alert('Siparş statusü güncellemek için en az 1 satır seçmelisiniz.Kayıt yapılmadı');</script>");
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ChangeForm(12)", true);
                return;
            }

            if (drp_siparis_durum.SelectedItem.Text.Trim()=="Statu Seçiniz")
            {
                Response.Write("<script lang='JavaScript'>alert('İlgili siparişler için statu güncellemek için öncelikle stau seçiniz.');</script>");
                return;
            }

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                foreach (GridViewRow grow in grd_siparis_uretim.Rows)
                {
                    CheckBox chk = (CheckBox)grow.FindControl("chc_sec");
                    if (chk!= null & chk.Checked)
                    {
                        SqlCommand cmd4 = new SqlCommand(" UPDATE SIPARIST SET DURUM='"+drp_siparis_durum.SelectedValue.Trim()+"' WHERE ID='"+grow.Cells[1].Text.Trim()+"'  ", con);
                        try
                        {
                            cmd4.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            log_at_av(ex.ToString().Trim());
                            Response.Write("<script lang='JavaScript'>alert('Database baglantısında bir sorun yaşandı lütfen sistem yöneticiniz ile iletişime geçiniz.');</script>");
                            return;
                        }

                    }
                }
            }
            grid_binding();
        }

        private void log_at_av(string v)
        {
            string path = @"\\192.168.1.4\DEPAR_ERP_LOG\log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (!File.Exists(path))
            {
                File.CreateText(path).Dispose();
            }
            using (TextWriter txt = File.AppendText(path))
            {
                txt.WriteLine(DateTime.Now + "----" + usr_name.username.Trim() + "----" + v.ToString().Trim());
                txt.WriteLine("———————————————————————————");
                txt.Dispose();
            }
        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            grid_binding();
        }

        protected void PAGE_AV(object sender, GridViewPageEventArgs e)
        {
            grid_binding();
            grd_siparis_uretim.PageIndex = e.NewPageIndex;
            grd_siparis_uretim.DataBind();

        }

        protected void SORT_AV(object sender, GridViewSortEventArgs e)
        {
            DataTable dataTable = ViewState["dtbl"] as DataTable;
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                dataView.Sort = e.SortExpression + " " + ConvertSortDirection(e.SortDirection);
                grd_siparis_uretim.DataSource = dataView;
                grd_siparis_uretim.DataBind();
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ChangeForm(6)", true);

        }

        
        private string ConvertSortDirection(SortDirection sortDirection)
        {
            string newSortDirection = String.Empty;
            switch (sortDirection)
            {
                case SortDirection.Ascending:
                    newSortDirection = "ASC";
                    break;
                case SortDirection.Descending:
                    newSortDirection = "DESC";
                    break;
            }
            return newSortDirection;
        }
    }
}