using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace depar_erp_v1_1
{
    public partial class siparis_page : System.Web.UI.Page

    {
        public class usr_name
        {
            public static string username;
            public static string username_full;
            public static string username_role;
            public static string evrakno;
            public static bool onay;
            public static string siparis_durum;
            public static string siparis_satirno;

        }
        bool SayiMi(string text)
        {
            foreach (char chr in text)
            {
                if (chr.ToString() != ",")
                {
                    if (!Char.IsNumber(chr)) return false;
                }
            }
            return true;
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
            if (usr_name.username_role.Trim()!="ADMIN")
            {
                Response.Redirect("dashboard.aspx");
            }

            if (!Page.IsPostBack)
            {
                usr_name.onay = onay1.Checked;
                cari_bindign();
                malzeme_kartlari_bindign();
                banka_binding();
                liste_binding();
                listele2_bindign();
                usr_name.siparis_durum=drp_siparis_durum.SelectedItem.Text.Trim();
                if (Request.QueryString["evrakno"].ToString().Trim()!="0")
                {
                    if (HDN_ODEME.Value=="E")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ChangeForm(9)", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ChangeForm(5)", true);
                    }

                }
            }

            if (Request.QueryString["evrakno"].ToString().Trim()!="0")
            {
                usr_name.evrakno = Request.QueryString["evrakno"].ToString().Trim();
                yeni_kayit();
                cari_bindign();
                sip_doldur();
                banka_binding();
                liste_binding();
                listele2_bindign();
                usr_name.siparis_durum=drp_siparis_durum.SelectedItem.Text.Trim();
                if (HDN_ODEME.Value=="E")
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ChangeForm(9)", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ChangeForm(5)", true);
                }
            }
        }

        private void banka_binding()
        {
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(" SELECT ID,CONCAT(TRIM(HESAP_AD),'-',TRIM(IBAN),'-',TRIM(BANKA)) AS BANK_ID FROM  BANKA ", con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                lstbanka.Items.Clear();
                lstbanka.Items.Add("Banka Seçiniz");
                lstbanka.DataTextField = "BANK_ID";
                lstbanka.DataValueField = "ID";
                lstbanka.DataSource = ds;
                lstbanka.DataBind();
                con.Close();
            }
        }

        private void sip_doldur()
        {
            usr_name.username = HttpContext.Current.User.Identity.Name.ToString().Trim();
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string query = " SELECT * FROM TEKLIFE WHERE TRIM(EVRAKNO)='"+usr_name.evrakno.Trim()+"' ";
                SqlCommand cmd_00 = new SqlCommand(query, con);
                SqlDataReader dr_00 = cmd_00.ExecuteReader();
                if (dr_00.HasRows)
                {
                    dr_00.Read();
                    drp_carikodu_kart.ClearSelection();
                    drp_carikodu_kart.Items.FindByText(HttpUtility.HtmlDecode(dr_00["CHKODU_AD"].ToString().Trim())).Selected = true;
                    txt_siparisnotlar_kart.Text=dr_00["NOTLAR"].ToString().Trim();
                    yetkili_bul_binding();
                    if (dr_00["MUSTERI_TEMS"].ToString().Trim()!="")
                    {
                        drp_must_yetkili.ClearSelection();
                        drp_must_yetkili.Items.FindByValue(dr_00["MUSTERI_TEMS"].ToString().Trim()).Selected = true;
                    }
                    else
                    {
                        drp_must_yetkili.ClearSelection();
                        drp_must_yetkili.Items.FindByValue("Yetkili seçiniz").Selected = true;
                    }

                }
                dr_00.Close();


                string query2 = " SELECT A.ID,A.KOD AS STOK_KOD,A.KOD_AD AS STOK_AD,'' AS REVIZYON,A.FIYAT AS BIRIM_FIYAT,A.MIKTAR,A.BIRIM,A.MIKTAR*A.FIYAT TOPLAM_TUTAR," +
                                    " FORMAT(DATEADD(DAY,7,getdate()),'yyyy-MM-dd') AS TESLIM_TARIH," +
                                    " A.ACIKLAMA AS SATIR_NOT, " +
                                    " B.MIL_MIN AS K_MIN,B.MIL_MAX AS K_MAX,B.YUVA_MIN AS YU_MIN,B.YUVA_MAX AS YU_MAX,B.YAG_MAX AS YAG_MIN,B.YAG_MAX AS YAG_MAX,B.CIDAR_MIN AS CI_MIN,B.CIDAR_MAX AS CI_MAX "+
                                    " FROM TEKLIFT A " +
                                    " LEFT OUTER JOIN  STOK_KART_REV B ON ( TRIM(A.KOD)=TRIM(B.KOD) AND TRIM(B.REVIZYON)='R' ) "+
                                    " WHERE TRIM(A.EVRAKNO)='"+usr_name.evrakno.Trim()+"' ";
                SqlCommand cmd_01 = new SqlCommand(query2, con);
                SqlDataReader dr_01 = cmd_01.ExecuteReader();
                if (dr_01.HasRows)
                {
                    dr_01.Close();
                    SqlCommand cmd_02 = new SqlCommand(query2, con);
                    SqlDataAdapter adp_02 = new SqlDataAdapter(cmd_02);
                    DataSet ds_02 = new DataSet();
                    adp_02.Fill(ds_02);
                    grd_siparis_detay.DataSource = ds_02;
                    grd_siparis_detay.DataBind();
                }
                dr_01.Close();



                con.Close();
            }


        }

        private void malzeme_kartlari_bindign()
        {
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(" SELECT ID,TRIM(KOD) AS KOD,CONCAT(TRIM(KOD),'--',TRIM(KOD_AD),'--',TRIM(KOD_AD2)) AS MALZEME FROM STOK_KART WHERE TRIM(KOD) LIKE '152%' and ESKI_YENI='YENI' ORDER BY KOD ASC ", con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                drp_siparis_malz_kart.Items.Clear();
                drp_siparis_malz_kart.Items.Add("Malzeme Seçiniz");
                drp_siparis_malz_kart.DataTextField = "MALZEME";
                drp_siparis_malz_kart.DataValueField = "KOD";
                drp_siparis_malz_kart.DataSource = ds;
                drp_siparis_malz_kart.DataBind();

                drp_malzemekodu_liste.Items.Clear();
                drp_malzemekodu_liste.Items.Add("Malzeme Seçiniz");
                drp_malzemekodu_liste.DataTextField = "MALZEME";
                drp_malzemekodu_liste.DataValueField = "KOD";
                drp_malzemekodu_liste.DataSource = ds;
                drp_malzemekodu_liste.DataBind();
                con.Close();
            }
        }

        private void cari_bindign()
        {
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(" select ID,TRIM(CARI_AD) AS CARI_AD,TRIM(CARI_KOD) AS CARI_KOD from CARI order by LEFT(CARI_KOD,6) ASC,CARI_AD ASC  ", con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                drp_carikodu_kart.Items.Clear();
                drp_carikodu_kart.Items.Add("Cari Seçiniz");
                drp_carikodu_kart.DataTextField = "CARI_AD";
                drp_carikodu_kart.DataValueField = "CARI_KOD";
                drp_carikodu_kart.DataSource = ds;
                drp_carikodu_kart.DataBind();

                drp_cari_liste.Items.Clear();
                drp_cari_liste.Items.Add("Cari Seçiniz");
                drp_cari_liste.DataTextField = "CARI_AD";
                drp_cari_liste.DataValueField = "CARI_KOD";
                drp_cari_liste.DataSource = ds;
                drp_cari_liste.DataBind();

                drp_cari_liste_2.Items.Clear();
                drp_cari_liste_2.Items.Add("Cari Seçiniz");
                drp_cari_liste_2.DataTextField = "CARI_AD";
                drp_cari_liste_2.DataValueField = "CARI_KOD";
                drp_cari_liste_2.DataSource = ds;
                drp_cari_liste_2.DataBind();

                con.Close();
            }
        }

        protected void save_av(object sender, EventArgs e)
        {
            yeni_kayit();
        }

        private void yeni_kayit()
        {
            siparis_ekranini_temilze2();
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlCommand cmd2 = new SqlCommand(" select TOP 1 SIPNO ,substring(SIPNO,10,5)+1  AS NUMBER  from SIPARISE ORDER BY ID DESC ", con);
                SqlDataReader dr2 = cmd2.ExecuteReader();
                dr2.Read();
                if (dr2.HasRows)
                {
                    string next_number2 = dr2["NUMBER"].ToString();
                    txt_siparisno_kart.Text = "DPR" + DateTime.Now.ToString("yyyyMM").ToString() + next_number2;
                    dr2.Close();
                }
                else
                {
                    txt_siparisno_kart.Text = "DPR" + DateTime.Now.ToString("yyyyMM").ToString() + "1";
                }
                dr2.Close();
                con.Close();

                txt_odeme.Text="%50 Peşin %50 Mal teslimi";
                txt_siparisnotlar_kart.Text=HttpUtility.HtmlDecode("Makinaya ait yatağına ilişkin imalat birim fiyat teklifimiz aşağıda bilgilerinize sunulmuştur. Teklifimizi uygun bulacağınızı umut eder, çalışmalarınızda sağlık ve başarılar dileriz.");
            }
            datepicker1.Value = Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd");
            cari_bindign();
        }

        protected void delete_av(object sender, EventArgs e)
        {
            if (txt_siparisno_kart.Text.Trim()=="")
            {
                Response.Write("<script lang='JavaScript'>alert('Sipariş no seçilmeden kayıt silinemez.');</script>");
                return;
            }

            //SİPARİŞ SATIR VE BELGE BİLGİLERİ SİLİNİYOR.
            string constr3 = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con3 = new SqlConnection(constr3))
            {
                con3.Open();
                SqlCommand cmd = new SqlCommand("Delete From SIPARISE where SIPNO='" + txt_siparisno_kart.Text.Trim() + "'", con3);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con3.Close();
                //grd_siparis_detay.EditIndex = -1;
            }

            using (SqlConnection con4 = new SqlConnection(constr3))
            {
                con4.Open();
                SqlCommand cmd2 = new SqlCommand("Delete From SIPARIST where SIPNO='" + txt_siparisno_kart.Text.Trim() + "'", con4);
                cmd2.ExecuteNonQuery();
                cmd2.Dispose();
                con4.Close();
                //grd_siparis_detay.EditIndex = -1;
            }
            evrak.teklif_sipno_update(txt_siparisno_kart.Text.Trim(), usr_name.username.Trim());
            sip_detay_liste_cari_bindign();
            siparis_ekranini_temilze();
        }

        private void siparis_ekranini_temilze()
        {
            txt_siparisno_kart.Text="";
            datepicker1.Value="";
            datepicker2.Value="";
            txt_siparisnotlar_kart.Text="";
            drp_must_yetkili.ClearSelection();
            drp_must_yetkili.Items.FindByText("Yetkili seçiniz").Selected = true;
            malzeme_kartlari_bindign();
            cari_bindign();
            drp_carikodu_kart.ClearSelection();
            drp_carikodu_kart.Items.FindByText("Cari Seçiniz").Selected = true;
            drp_siparis_malz_kart.ClearSelection();
            drp_siparis_malz_kart.Items.FindByText("Malzeme Seçiniz").Selected = true;

            txt_siparismalzemeadi_kart.Text="";
            txt_siparismiktar_kart.Text="";
            drp_siparis_birim_kart.ClearSelection();
            drp_siparis_birim_kart.Items.FindByValue("5").Selected = true;
            txt_birimfiyat_kart.Text="";
            drp_parabirimi_kart.ClearSelection();
            drp_parabirimi_kart.Items.FindByText("USD").Selected = true;
            grd_siparis_detay.DataSource = null;
            grd_siparis_detay.DataBind();

        }

        private void siparis_ekranini_temilze2()
        {
            datepicker2.Value="";
            onay1.Checked=false;
            lbl_onay_info.Text="";
            txt_uretim_notlar.Text="";
            lbl_durum_onay.Text="";
            txt_teklifteslimsekli.Text="";
            malzeme_kartlari_bindign();
            cari_bindign();
            txt_siparismalzemeadi_kart.Text="";
            txt_siparismiktar_kart.Text="";
            txt_birimfiyat_kart.Text="";
            stok_kodu_hdn.Value="";
            txt_vade.Text="";


            drp_must_yetkili.ClearSelection();
            drp_must_yetkili.Items.FindByText("Yetkili seçiniz").Selected = true;
            drp_siparis_durum.ClearSelection();
            drp_siparis_durum.Items.FindByText("Onay Bekliyor").Selected = true;
            drp_carikodu_kart.ClearSelection();
            drp_carikodu_kart.Items.FindByText("Cari Seçiniz").Selected = true;
            drp_siparis_malz_kart.ClearSelection();
            drp_siparis_malz_kart.Items.FindByText("Malzeme Seçiniz").Selected = true;
            drp_siparis_birim_kart.ClearSelection();
            drp_siparis_birim_kart.Items.FindByValue("5").Selected = true;
            drp_parabirimi_kart.ClearSelection();
            drp_parabirimi_kart.Items.FindByText("USD").Selected = true;
            drp_kdv.ClearSelection();
            drp_kdv.Items.FindByValue("18").Selected = true;


            grd_siparis_detay.DataSource = null;
            grd_siparis_detay.DataBind();
            grd_siparis_dokumanlar.DataSource = null;
            grd_siparis_dokumanlar.DataBind();

        }

        protected void delete_satir_av(object sender, GridViewDeleteEventArgs e)
        {
            int SID = Convert.ToInt32(grd_siparis_detay.DataKeys[e.RowIndex].Value);
            string constr3 = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con3 = new SqlConnection(constr3))
            {
                con3.Open();
                SqlCommand cmd = new SqlCommand("Delete From SIPARIST where ID='" + SID + "'", con3);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con3.Close();
                grd_siparis_detay.EditIndex = -1;
                sip_detay_liste_cari_bindign();
            }

        }

        protected void sec_av(object sender, EventArgs e)
        {

            GridViewRow row = grd_siparis_detay.SelectedRow;
            String ID = row.Cells[2].Text.Trim();
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string query = " SELECT A.*,'0' AS K_MIN,'0' AS K_MAX,'0' AS YU_MIN,'0' AS YU_MAX,'0' AS  YAG_MIN,'0' AS YAG_MAX,'0' AS CI_MIN,'0' AS CI_MAX FROM SIPARIST A WHERE A.ID="+ID+" ";

                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    hdn_sip_satir.Value=dr["ID"].ToString().Trim();
                    if (dr["STOK_KOD"].ToString().Trim()=="" || dr["STOK_KOD"].ToString().Trim()=="Malzeme Seçiniz" || dr["STOK_KOD"].ToString().Trim()=="Yeni Ürün")
                    {
                        drp_siparis_malz_kart.ClearSelection();
                        drp_siparis_malz_kart.Items.FindByValue("Malzeme Seçiniz").Selected = true;
                    }
                    else
                    {
                        drp_siparis_malz_kart.ClearSelection();
                        drp_siparis_malz_kart.Items.FindByValue(dr["STOK_KOD"].ToString().Trim()).Selected = true;
                    }

                    txt_siparismalzemeadi_kart.Text=HttpUtility.HtmlDecode(dr["STOK_AD"].ToString().Trim());
                    txt_siparismiktar_kart.Text=dr["MIKTAR"].ToString().Trim();
                    if (dr["BIRIM"].ToString().Trim()=="")
                    {
                        drp_siparis_birim_kart.ClearSelection();
                        drp_siparis_birim_kart.Items.FindByValue("Birim Seçiniz").Selected = true;
                    }
                    else
                    {
                        drp_siparis_birim_kart.ClearSelection();
                        drp_siparis_birim_kart.Items.FindByText(dr["BIRIM"].ToString().Trim()).Selected = true;
                    }
                    txt_birimfiyat_kart.Text=dr["BIRIM_FIYAT"].ToString().Trim();
                    if (dr["TESLIM_TARIH"].ToString().Trim()!="")
                    {
                        datepicker2.Value=Convert.ToString(Convert.ToDateTime(dr["TESLIM_TARIH"].ToString()).ToString("yyyy-MM-dd"));
                    }
                    if (dr["PARA_BIRIMI"].ToString().Trim()=="")
                    {
                        drp_parabirimi_kart.ClearSelection();
                        drp_parabirimi_kart.Items.FindByValue("Para Birim Seçiniz").Selected = true;
                    }
                    else
                    {
                        drp_parabirimi_kart.ClearSelection();
                        drp_parabirimi_kart.Items.FindByValue(dr["PARA_BIRIMI"].ToString().Trim()).Selected = true;
                    }
                    txt_siparis_satırnot_kart.Text=HttpUtility.HtmlDecode(dr["SATIR_NOT"].ToString().Trim());
                }
                dr.Close();
                con.Close();
            }

        }

        protected void listele_av(object sender, EventArgs e)
        {
            liste_binding();
        }

        private void liste_binding()
        {
            string query = "";
            string constr2 = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con2 = new SqlConnection(constr2))
            {
                con2.Open();
                query = " SELECT A.*,B.STOK_KOD,B.STOK_AD,B.MIKTAR,B.BIRIM,B.BIRIM_FIYAT,B.TESLIM_TARIH,ISNULL(B.TOPLAM_TUTAR,0) AS TOPLAM_TUTAR,B.PARA_BIRIMI," +
                        " case when C.DEPAR_KOD<>'' THEN C.DEPAR_KOD  ELSE CONCAT(TRIM(C.KOD),'-',TRIM(C.KOD_AD) ) END DEPAR_KOD," +
                        " CASE  "+
                        " WHEN B.DURUM='0'  THEN 'Onay Bekliyor' " +
                        " WHEN B.DURUM='1'  THEN 'Planlamada' " +
                        " WHEN B.DURUM='2'  THEN 'Üretimde' " +
                        " WHEN B.DURUM='3'  THEN 'Kalite Kontrolde' " +
                        " WHEN B.DURUM='4'  THEN 'Depoda' " +
                        " WHEN B.DURUM='5'  THEN 'Kısmı Sevkiyat'" +
                        " WHEN B.DURUM='6'  THEN 'Sevkiyat'" +
                        " END DURUM_AD " +
                        " FROM SIPARISE A " +
                        " LEFT OUTER JOIN SIPARIST B ON ( TRIM(A.SIPNO)=TRIM(B.SIPNO) ) "+
                        " LEFT OUTER JOIN STOK_KART C ON ( TRIM(B.STOK_KOD)=TRIM(C.KOD) )"+
                        "  WHERE 1=1 ";
                if (txt_siparino_liste.Text.Trim() != "")
                {
                    query = query + "  and  A.SIPNO LIKE '%" + txt_siparino_liste.Text.Trim() + "%'";
                }
                if (drp_cari_liste.SelectedItem.Text.Trim()!="Cari Seçiniz")
                {
                    query = query + "  and  A.CH_KODU='" + drp_cari_liste.SelectedValue.Trim() + "'";
                }
                if (txt_siparisnot_liste.Text.Trim() != "")
                {
                    query = query + "  and  A.NOTLAR LIKE '%" + txt_siparisnot_liste.Text.Trim() + "%'";
                }
                if (datepicker3.Value!= "")
                {
                    string iDate = Request.Form["ctl00$ContentPlaceHolder1$datepicker3"];
                    DateTime oDate = DateTime.Parse(iDate);
                    query = query + "  and  A.TARIH='" + oDate+ "'";
                }

                if (drp_sip_durum_liste.SelectedItem.Text.Trim() != "Hepsi")
                {
                    query = query + "  and  B.DURUM = '" + drp_sip_durum_liste.SelectedValue.Trim() + "'";
                }
                if (txt_deparkod_liste.Text.Trim() != "")
                {
                    query = query + "  and  C.DEPAR_KOD  LIKE '%" + txt_deparkod_liste.Text.Trim() + "%'";
                }
                if (drp_malzemekodu_liste.SelectedItem.Text.Trim() != "Malzeme Seçiniz")
                {
                    query = query + "  and  C.KOD  = '" + drp_malzemekodu_liste.SelectedValue.Trim() + "'";
                }
                if (txt_stokadi_liste.Text.Trim() != "")
                {
                    query = query + "  and  B.STOK_AD  LIKE '%" + txt_stokadi_liste.Text.Trim() + "%'";
                }
                if (datepicker7.Value!="Başlangıç Tarihi")
                {
                    if (datepicker7.Value!="")
                    {
                        query = query + "  and  CAST(A.TARIH  as datetime)>=cast('"+datepicker7.Value.Trim()+"' as datetime) ";
                    }
                }
                if (datepicker8.Value!="Bitiş Tarihi")
                {
                    if (datepicker8.Value!="")
                    {
                        query = query + "  and  CAST(A.TARIH  as datetime)<=cast('"+datepicker8.Value.Trim()+"' as datetime) ";
                    }
                }

                if (datepicker9.Value!="Başlangıç Tarihi")
                {
                    if (datepicker9.Value!="")
                    {
                        query = query + "  and  CAST(B.TESLIM_TARIH  as datetime)>=cast('"+datepicker9.Value.Trim()+"' as datetime) ";
                    }
                }
                if (datepicker10.Value!="Bitiş Tarihi")
                {
                    if (datepicker10.Value!="")
                    {
                        query = query + "  and  CAST(B.TESLIM_TARIH  as datetime)<=cast('"+datepicker10.Value.Trim()+"' as datetime) ";
                    }
                }

                query = query + " ORDER BY A.ID DESC ";
                SqlCommand cmd2 = new SqlCommand(query, con2);
                SqlDataAdapter adp2 = new SqlDataAdapter(cmd2);
                DataSet ds2 = new DataSet();
                adp2.Fill(ds2);
                grd_siparis_listesi.DataSource = ds2;
                grd_siparis_listesi.DataBind();

                DataTable dtTemp = new DataTable();
                ViewState["dtbl"] = dtTemp;
                adp2.Fill(dtTemp);

                con2.Close();


                decimal total_usd_tutar = 0;
                decimal total_eur_tutar = 0;
                decimal total_tl_tutar = 0;
                decimal total_gbp_tutar = 0;
                foreach (GridViewRow gvrow in grd_siparis_listesi.Rows)
                {
                    if (gvrow.Cells[12].Text.Trim()=="USD")
                    {
                        total_usd_tutar =total_usd_tutar + Convert.ToDecimal(gvrow.Cells[11].Text.Trim());
                    }
                    else if (gvrow.Cells[12].Text.Trim()=="EUR")
                    {
                        total_eur_tutar =total_eur_tutar + Convert.ToDecimal(gvrow.Cells[11].Text.Trim());
                    }
                    else if (gvrow.Cells[12].Text.Trim()=="GBP")
                    {
                        total_gbp_tutar =total_gbp_tutar + Convert.ToDecimal(gvrow.Cells[11].Text.Trim());
                    }
                    else if (gvrow.Cells[12].Text.Trim()=="TL")
                    {
                        total_tl_tutar =total_tl_tutar + Convert.ToDecimal(gvrow.Cells[11].Text.Trim());
                    }
                }
                if (total_usd_tutar>0)
                {
                    this.AddTotalRow("Toplam Tutar USD", total_usd_tutar.ToString("N2"));
                }
                if (total_eur_tutar>0)
                {
                    this.AddTotalRow("Toplam Tutar EUR", total_eur_tutar.ToString("N2"));
                }
                if (total_tl_tutar>0)
                {
                    this.AddTotalRow("Toplam Tutar TL", total_tl_tutar.ToString("N2"));
                }
                if (total_gbp_tutar>0)
                {
                    this.AddTotalRow("Toplam Tutar GBP", total_gbp_tutar.ToString("N2"));
                }

            }
            //sipariş liste tabında arama yapınca diger tab'a geçmemesi için çalışasn scripttir.
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ChangeForm(6)", true);
        }

        private void AddTotalRow(string labelText, string value)
        {
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal);
            row.BackColor = ColorTranslator.FromHtml("#F9F9F9");
            row.Cells.AddRange(new TableCell[14] { new TableCell(), new TableCell(), new TableCell(), new TableCell(), new TableCell(), new TableCell(), new TableCell(), new TableCell(), new TableCell(), new TableCell(), new TableCell { Text = labelText, HorizontalAlign = HorizontalAlign.Right }, new TableCell { Text = value, HorizontalAlign = HorizontalAlign.Right }, new TableCell(), new TableCell() });
            grd_siparis_listesi.Controls[0].Controls.Add(row);
        }

        protected void save_av_2(object sender, EventArgs e)
        {
            //sipariş kayıt öncesi yapılan kontrol işlemleri
            if (txt_siparisno_kart.Text.Trim()=="")
            {
                Response.Write("<script lang='JavaScript'>alert('Sipariş no olmadan kayıt yapılamaz. Kayıt yapılmadı.');</script>");
                return;
            }
            if (drp_carikodu_kart.SelectedItem.Text.Trim()=="Cari Seçiniz")
            {
                Response.Write("<script lang='JavaScript'>alert('Sipariş müşterisi girilmeden kayıt yapılamaz. Kayıt yapılmadı.');</script>");
                return;
            }
            if (datepicker1.Value.Trim()=="")
            {
                Response.Write("<script lang='JavaScript'>alert('Sipariş tarihi girilmeden kayıt yapılamaz. Kayıt yapılmadı.');</script>");
                return;
            }
            if (txt_siparisnotlar_kart.Text.Length >= 1000)
            {
                Response.Write("<script lang='JavaScript'>alert('Sipariş not kısmında yazılan açıklama 1000 karakterden fazla olamaz.Lütfen açıklama alanınızı kısaltınız.Kayıt yapılmadı. ');</script>");
                return;
            }
            if (txt_uretim_notlar.Text.Length >= 400)
            {
                Response.Write("<script lang='JavaScript'>alert('Üretim için girilen not kısmında yazılan açıklama max. 400 karakter olmalıdır.Lütfen açıklama alanınızı kısaltınız.Kayıt yapılmadı. ');</script>");
                return;
            }
            if (drp_siparis_birim_kart.SelectedItem.Text.Trim()=="Birim Seçiniz")
            {
                Response.Write("<script lang='JavaScript'>alert('Malzeme birimi girilmeden kayıt yapılamaz.Kayıt yapılmadı. ');</script>");
                return;
            }
            if (drp_malzeme_revizyon.SelectedItem.Text.Trim()=="Revizyon seçiniz")
            {
                Response.Write("<script lang='JavaScript'>alert('Malzeme revziyonu girilmeden kayıt yapılamaz.Kayıt yapılmadı. ');</script>");
                return;
            }


            decimal k_min = 0;
            decimal k_max = 0;
            decimal yu_min = 0;
            decimal yu_max = 0;
            decimal yag_min = 0;
            decimal yag_max = 0;
            decimal ci_min = 0;
            decimal ci_max = 0;

            int erhan = 0;
            string banka1 = "";
            string banka2 = "";
            string banka3 = "";
            foreach (System.Web.UI.WebControls.ListItem item in lstbanka.Items)
            {
                if (item.Selected)
                {
                    erhan += 1;
                    if (erhan==1)
                    {
                        banka1= item.Value.Trim();
                    }
                    else if (erhan==2)
                    {
                        banka2= item.Value.Trim();
                    }
                    else if (erhan==2)
                    {
                        banka3= item.Value.Trim();
                    }
                }
            }
            if (erhan>3)
            {
                Response.Write("<script lang='JavaScript'>alert('3 Adetden daha fazla banka seçimi yapılamaz.');</script>");
                return;
            }

            //MALZEME İLE İLGİLİ OLARAK REVİZYON OLCU BİLGİLERİ KAYIT ALTINA ALINIYOR
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con2 = new SqlConnection(constr))
            {
                con2.Open();
                if (drp_siparis_malz_kart.SelectedItem.Text.Trim()!="Yeni Ürün" && drp_siparis_malz_kart.SelectedValue.Trim()!="152 00 01 0001" &&  drp_siparis_malz_kart.SelectedValue.Trim()!="152 00 02 0001")
                {
                    string query = " SELECT A.REVIZYON,A.KOD, CASE WHEN A.REVIZYON='R' THEN 'S' ELSE A.REVIZYON END REV_1,A.MIL_MIN,A.MIL_MAX,A.YUVA_MIN,A.YUVA_MAX,A.YAG_MIN,A.YAG_MAX,A.CIDAR_MIN,A.CIDAR_MAX " +
                               " FROM STOK_KART_REV A "+
                               " LEFT OUTER JOIN STOK_KART B ON(TRIM(A.KOD)= TRIM(B.KOD)) "+
                               " WHERE TRIM(B.KOD)='"+drp_siparis_malz_kart.SelectedValue.Trim()+"' AND RTRIM(A.REVIZYON)='"+drp_malzeme_revizyon.SelectedValue.Trim()+"' ";
                    SqlCommand cmd2 = new SqlCommand(query, con2);
                    SqlDataReader dr2 = cmd2.ExecuteReader();
                    if (dr2.HasRows)
                    {
                        dr2.Read();
                        stok_kodu_hdn.Value=dr2["KOD"].ToString().Trim();
                        k_min=Convert.ToDecimal(dr2["MIL_MIN"].ToString().Trim());
                        k_max=Convert.ToDecimal(dr2["MIL_MAX"].ToString().Trim());
                        yu_min=Convert.ToDecimal(dr2["YUVA_MIN"].ToString().Trim());
                        yu_max=Convert.ToDecimal(dr2["YUVA_MAX"].ToString().Trim());
                        yag_min=Convert.ToDecimal(dr2["YAG_MIN"].ToString().Trim());
                        yag_max=Convert.ToDecimal(dr2["YAG_MAX"].ToString().Trim());
                        ci_max=Convert.ToDecimal(dr2["CIDAR_MAX"].ToString().Trim());
                        ci_max=Convert.ToDecimal(dr2["CIDAR_MAX"].ToString().Trim());
                    }
                }
                else
                {
                    drp_malzeme_revizyon.ClearSelection();
                    drp_malzeme_revizyon.Items.FindByText("R").Selected = true;
                }
                con2.Close();
            }

            //MALZEME İLE İLGİLİ BİLGİLERİ SATIRLARA YAZILIYOR  
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlCommand cmd2 = new SqlCommand("  SELECT * FROM SIPARISE A WHERE A.SIPNO ='" + txt_siparisno_kart.Text.Trim() + "'", con);
                SqlDataReader dr2 = cmd2.ExecuteReader();
                if (dr2.HasRows)
                {
                    if (hdn_sip_satir.Value=="")
                    {
                        if (datepicker2.Value.Trim()=="")
                        {
                            Response.Write("<script lang='JavaScript'>alert('Sipariş tesli tarihi olmadan kayıt yapılamaz.');</script>");
                            return;
                        }
                        dr2.Close();
                        SqlCommand cmd4 = new SqlCommand(" INSERT INTO SIPARIST (SIPNO,STOK_KOD,STOK_AD,MIKTAR,BIRIM,TESLIM_TARIH,SATIR_NOT,BIRIM_FIYAT,TOPLAM_TUTAR,PARA_BIRIMI,REVIZYON,K_MIN,K_MAX,YU_MIN,YU_MAX,YAG_MIN,YAG_MAX,CI_MIN,CI_MAX,KDV,KALAN_MIKTAR,DURUM)  " +
                                                         " values(@sipno,@stok_kod,@stok_ad,@miktar,@birim,@teslim_tarihi,@satir_not,@BIRIM_FIYAT,@TOPLAM_TUTAR,@PARA_BIRIMI,@REVIZYON,@K_MIN,@K_MAX,@YU_MIN,@YU_MAX,@YAG_MIN,@YAG_MAX,@CI_MIN,@CI_MAX,@KDV,@KALAN_MIKTAR,@DURUM) ", con);
                        cmd4.Parameters.AddWithValue("@sipno", txt_siparisno_kart.Text.Trim());
                        cmd4.Parameters.AddWithValue("@stok_kod", HttpUtility.HtmlDecode(drp_siparis_malz_kart.SelectedValue.Trim()));
                        cmd4.Parameters.AddWithValue("@stok_ad", HttpUtility.HtmlDecode(txt_siparismalzemeadi_kart.Text.Trim()));

                        decimal sip_miktar = Convert.ToDecimal(txt_siparismiktar_kart.Text.ToString().Trim());
                        cmd4.Parameters.AddWithValue("@miktar", sip_miktar);
                        cmd4.Parameters.AddWithValue("@KALAN_MIKTAR", sip_miktar);
                        decimal birim_fiyat = Convert.ToDecimal(txt_birimfiyat_kart.Text.ToString().Trim());
                        cmd4.Parameters.AddWithValue("@BIRIM_FIYAT", birim_fiyat);
                        cmd4.Parameters.AddWithValue("@TOPLAM_TUTAR", birim_fiyat*sip_miktar);
                        string revizyon_info = drp_malzeme_revizyon.SelectedValue.ToString().Trim();
                        cmd4.Parameters.AddWithValue("@REVIZYON", revizyon_info);
                        cmd4.Parameters.AddWithValue("@birim", drp_siparis_birim_kart.SelectedItem.Text.Trim());
                        cmd4.Parameters.AddWithValue("@PARA_BIRIMI", drp_parabirimi_kart.SelectedItem.Text.Trim());
                        cmd4.Parameters.AddWithValue("@KDV", drp_kdv.SelectedValue.Trim());
                        cmd4.Parameters.AddWithValue("@DURUM", "1");
                        string tes_tarih = "";
                        if (datepicker2.Value.Trim()=="")
                        {
                            tes_tarih = "";
                        }
                        else
                        {
                            tes_tarih = datepicker2.Value.Trim();
                        }
                        cmd4.Parameters.AddWithValue("@teslim_tarihi", tes_tarih);
                        cmd4.Parameters.AddWithValue("@satir_not", txt_siparis_satırnot_kart.Text.Trim());
                        cmd4.Parameters.AddWithValue("@K_MIN", k_min);
                        cmd4.Parameters.AddWithValue("@K_MAX", k_max);
                        cmd4.Parameters.AddWithValue("@YU_MIN", yu_min);
                        cmd4.Parameters.AddWithValue("@YU_MAX", yu_max);
                        cmd4.Parameters.AddWithValue("@YAG_MIN", yag_min);
                        cmd4.Parameters.AddWithValue("@YAG_MAX", yag_max);
                        cmd4.Parameters.AddWithValue("@CI_MIN", ci_min);
                        cmd4.Parameters.AddWithValue("@CI_MAX", ci_max);
                        try
                        {
                            cmd4.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            erp_log.log_at_av(usr_name.username.Trim(), ex.ToString());
                            Response.Write("<script lang='JavaScript'>alert('Database baglantısında bir sorun yaşandı lütfen sistem yöneticiniz ile iletişime geçiniz.');</script>");
                            return;
                        }
                    }
                    else
                    {
                        dr2.Close();
                        //decimal sip_miktar2 = Convert.ToDecimal(txt_siparismiktar_kart.Text.ToString().Trim().Replace(',','.'));
                        decimal sip_miktar2 = decimal.Parse(txt_siparismiktar_kart.Text.ToString(), new CultureInfo("tr-TR"));
                        decimal birim_fiyat2 = decimal.Parse(txt_birimfiyat_kart.Text.ToString(), new CultureInfo("tr-TR"));
                        //string iDate = Request.Form["ctl00$ContentPlaceHolder1$datepicker2"];
                        //string oDate = DateTime.Parse(iDate).ToString("yyyy-MM-dd HH:mm:ss");
                        //string  oDate = iDate.ToString("yyyy-MM-dd HH:mm:ss");
                        string tes_tarih = "";
                        if (datepicker2.Value.Trim()=="")
                        {
                            tes_tarih = "";
                        }
                        else
                        {
                            tes_tarih = datepicker2.Value.Trim();
                        }

                        //decimal birim_fiyat2 = Convert.ToDecimal(txt_birimfiyat_kart.Text.ToString().Trim().Replace(',', '.'));
                        SqlCommand cmd4 = new SqlCommand(" UPDATE SIPARIST SET STOK_KOD='"+drp_siparis_malz_kart.SelectedValue.Trim()+"',STOK_AD='"+HttpUtility.HtmlDecode(txt_siparismalzemeadi_kart.Text.Trim())+"',MIKTAR='"+sip_miktar2.ToString().Replace(',', '.')+"', " +
                                                        " BIRIM_FIYAT='"+birim_fiyat2.ToString().Replace(',', '.')+"',BIRIM='"+drp_siparis_birim_kart.SelectedItem.Text.Trim()+"'," +
                                                        " SATIR_NOT='"+HttpUtility.HtmlDecode(txt_siparis_satırnot_kart.Text.Trim())+"',PARA_BIRIMI='"+drp_parabirimi_kart.SelectedItem.Text.Trim()+ "', "+
                                                        " TOPLAM_TUTAR='"+(sip_miktar2*birim_fiyat2).ToString().Replace(',', '.')+"',KDV='"+drp_kdv.SelectedValue.Trim()+"',TESLIM_TARIH='"+tes_tarih.Trim()+"'"+
                                                        " WHERE ID='"+hdn_sip_satir.Value.Trim()+"'  ", con);
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
                else
                {
                    if (datepicker2.Value.Trim()=="")
                    {
                        Response.Write("<script lang='JavaScript'>alert('Sipariş teslim tarihi olmadan kayıt yapılamaz.');</script>");
                        return;
                    }
                    decimal birim_fiyat = 0;
                    decimal sip_miktar = 0;

                    dr2.Close();
                    SqlCommand cmd3 = new SqlCommand(" INSERT INTO SIPARISE (SIPNO,TARIH,CH_KODU,CH_ADI,NOTLAR,MUSTERI_TEMS,CREATE_USER,CREATE_DATE,ODEME,VADE,BANKA,BANKA2,BANKA3,DURUM,URETIM_NOTLAR,TESLIM_SEKLI ) " +
                                                     " values(@sipno,@tarih,@ch_kodu,@ch_adi,@notlar,@must_tem,@user,@date,@ODEME,@VADE,@BANKA,@BANKA2,@BANKA3,@DURUM,@URETIM_NOTLAR,@TESLIM_SEKLI) ", con);
                    cmd3.Parameters.AddWithValue("@sipno", txt_siparisno_kart.Text.Trim());
                    string iDate = Request.Form["ctl00$ContentPlaceHolder1$datepicker1"];
                    DateTime oDate = DateTime.Parse(iDate);
                    cmd3.Parameters.AddWithValue("@tarih", iDate);
                    cmd3.Parameters.AddWithValue("@ch_kodu", drp_carikodu_kart.SelectedValue.Trim());
                    cmd3.Parameters.AddWithValue("@ODEME", txt_odeme.Text.Trim());
                    cmd3.Parameters.AddWithValue("@VADE", txt_vade.Text.Trim());
                    cmd3.Parameters.AddWithValue("@BANKA", banka1.Trim());
                    cmd3.Parameters.AddWithValue("@BANKA2", banka2.Trim());
                    cmd3.Parameters.AddWithValue("@BANKA3", banka3.Trim());
                    cmd3.Parameters.AddWithValue("@ch_adi", drp_carikodu_kart.SelectedItem.Text.Trim());
                    cmd3.Parameters.AddWithValue("@notlar", txt_siparisnotlar_kart.Text.Trim());
                    cmd3.Parameters.AddWithValue("@must_tem", drp_must_yetkili.SelectedValue.Trim());
                    cmd3.Parameters.AddWithValue("@user", usr_name.username.Trim());
                    cmd3.Parameters.AddWithValue("@date", DateTime.Now);
                    cmd3.Parameters.AddWithValue("@DURUM", drp_siparis_durum.SelectedValue.Trim());
                    cmd3.Parameters.AddWithValue("@URETIM_NOTLAR", txt_uretim_notlar.Text.Trim());
                    cmd3.Parameters.AddWithValue("@TESLIM_SEKLI", txt_teklifteslimsekli.Text.Trim());
                    try
                    {
                        cmd3.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        erp_log.log_at_av(usr_name.username.Trim(), ex.ToString());
                        Response.Write("<script lang='JavaScript'>alert('Database baglantısında bir sorun yaşandı lütfen sistem yöneticiniz ile iletişime geçiniz.');</script>");
                        return;
                    }

                    //ilk sipariş kaydını yaparken hem e tablosunu hemde t tablosunu birlikte dolduracaktır.
                    dr2.Close();
                    SqlCommand cmd4 = new SqlCommand(" INSERT INTO SIPARIST (SIPNO,STOK_KOD,REVIZYON,STOK_AD,MIKTAR,BIRIM,TESLIM_TARIH,SATIR_NOT,BIRIM_FIYAT,TOPLAM_TUTAR,PARA_BIRIMI,K_MIN,K_MAX,YU_MIN,YU_MAX,YAG_MIN,YAG_MAX,CI_MIN,CI_MAX,KDV,KALAN_MIKTAR,DURUM )  " +
                                                     " values(@sipno,@stok_kod,@REVIZYON,@stok_ad,@miktar,@birim,@teslim_tarihi,@satir_not,@BIRIM_FIYAT,@TOPLAM_TUTAR,@PARA_BIRIMI,@K_MIN,@K_MAX,@YU_MIN,@YU_MAX,@YAG_MIN,@YAG_MAX,@CI_MIN,@CI_MAX,@KDV,@KALAN_MIKTAR,@DURUM) ", con);
                    cmd4.Parameters.AddWithValue("@sipno", txt_siparisno_kart.Text.Trim());
                    cmd4.Parameters.AddWithValue("@stok_kod", HttpUtility.HtmlDecode(drp_siparis_malz_kart.SelectedValue.Trim()));
                    cmd4.Parameters.AddWithValue("@stok_ad", HttpUtility.HtmlDecode(txt_siparismalzemeadi_kart.Text.Trim()));
                    if (txt_siparismiktar_kart.Text.Trim()!="")
                    {
                        sip_miktar = Convert.ToDecimal(txt_siparismiktar_kart.Text.ToString().Trim());
                    }
                    cmd4.Parameters.AddWithValue("@miktar", sip_miktar);
                    cmd4.Parameters.AddWithValue("@KALAN_MIKTAR", sip_miktar);
                    if (txt_birimfiyat_kart.Text.Trim()!="")
                    {
                        birim_fiyat = Convert.ToDecimal(txt_birimfiyat_kart.Text.ToString().Trim());
                    }
                    cmd4.Parameters.AddWithValue("@BIRIM_FIYAT", birim_fiyat);
                    string revizyon_info = drp_malzeme_revizyon.SelectedValue.ToString().Trim();
                    cmd4.Parameters.AddWithValue("@REVIZYON", revizyon_info);
                    //birim fiyat veya miktar alanı 0 olursa toplam tutar alanın da boş olmas için gerkeli kontrol.
                    if (txt_birimfiyat_kart.Text.Trim()=="" || txt_siparismiktar_kart.Text.Trim()=="")
                    {
                        cmd4.Parameters.AddWithValue("@TOPLAM_TUTAR", "0");
                    }
                    else
                    {
                        cmd4.Parameters.AddWithValue("@TOPLAM_TUTAR", birim_fiyat*sip_miktar);
                    }
                    cmd4.Parameters.AddWithValue("@birim", drp_siparis_birim_kart.SelectedItem.Text.Trim());
                    cmd4.Parameters.AddWithValue("@PARA_BIRIMI", drp_parabirimi_kart.SelectedItem.Text.Trim());
                    cmd4.Parameters.AddWithValue("@KDV", drp_kdv.SelectedValue.Trim());
                    cmd4.Parameters.AddWithValue("@DURUM", "0");
                    string tes_tarih = "";
                    if (datepicker2.Value.Trim()=="")
                    {
                        tes_tarih = "";
                    }
                    else
                    {
                        tes_tarih = datepicker2.Value.Trim();
                    }
                    cmd4.Parameters.AddWithValue("@teslim_tarihi", tes_tarih);
                    cmd4.Parameters.AddWithValue("@satir_not", txt_siparis_satırnot_kart.Text.Trim());
                    cmd4.Parameters.AddWithValue("@K_MIN", k_min);
                    cmd4.Parameters.AddWithValue("@K_MAX", k_max);
                    cmd4.Parameters.AddWithValue("@YU_MIN", yu_min);
                    cmd4.Parameters.AddWithValue("@YU_MAX", yu_max);
                    cmd4.Parameters.AddWithValue("@YAG_MIN", yag_min);
                    cmd4.Parameters.AddWithValue("@YAG_MAX", yag_max);
                    cmd4.Parameters.AddWithValue("@CI_MIN", ci_min);
                    cmd4.Parameters.AddWithValue("@CI_MAX", ci_max);
                    try
                    {
                        cmd4.ExecuteNonQuery();
                        siparis_detay_satir_temizle();
                    }
                    catch (Exception ex)
                    {
                        erp_log.log_at_av(usr_name.username.Trim(), ex.ToString());
                        Response.Write("<script lang='JavaScript'>alert('Database baglantısında bir sorun yaşandı lütfen sistem yöneticiniz ile iletişime geçiniz.');</script>");
                        return;
                    }
                }
                con.Close();
            }
            sip_detay_liste_cari_bindign();
            siparis_detay_satir_temizle();

        }

        private void siparis_detay_satir_temizle()
        {
            txt_siparismalzemeadi_kart.Text="";
            hdn_sip_satir.Value="";
            malzeme_kartlari_bindign();
            drp_siparis_malz_kart.ClearSelection();
            drp_siparis_malz_kart.Items.FindByText("Malzeme Seçiniz").Selected = true;
            txt_siparismiktar_kart.Text="";
            drp_siparis_birim_kart.ClearSelection();
            drp_siparis_birim_kart.Items.FindByValue("5").Selected = true;
            txt_birimfiyat_kart.Text="";
            drp_parabirimi_kart.ClearSelection();
            drp_parabirimi_kart.Items.FindByText("USD").Selected = true;
            datepicker2.Value="";
            txt_siparis_satırnot_kart.Text="";
        }

        private void sip_detay_liste_cari_bindign()
        {
            string query = "";
            string constr2 = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con2 = new SqlConnection(constr2))
            {
                query = " SELECT A.*,CONCAT(TRIM(B.REVIZYON),'-',TRIM(B.ACIKLAMA)) AS REV_ACIKLAM FROM SIPARIST A " +
                        " LEFT OUTER JOIN STOK_KART_REV B ON ( TRIM(A.STOK_KOD)=TRIM(B.KOD) AND TRIM(A.REVIZYON)=TRIM(B.REVIZYON) )"+
                        " WHERE TRIM(A.SIPNO)='"+txt_siparisno_kart.Text.Trim()+"'";
                SqlCommand cmd2 = new SqlCommand(query, con2);
                SqlDataAdapter adp2 = new SqlDataAdapter(cmd2);
                DataSet ds2 = new DataSet();
                adp2.Fill(ds2);
                grd_siparis_detay.DataSource = ds2;
                grd_siparis_detay.DataBind();
                con2.Close();
            }
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

        protected void sec_av_liste(object sender, EventArgs e)
        {

            GridViewRow row = grd_siparis_listesi.SelectedRow;
            String ID = row.Cells[1].Text.Trim();
            String order_no = row.Cells[2].Text.Trim();
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string query = " SELECT A.*,CONCAT(ONAY_USER,'-', FORMAT (ONAY_DATE,'yyyy-MM-dd HH:mm:ss')) AS DURUM_1," +
                               " CONCAT(D_ONAY_USER,'-', FORMAT (D_ONAY_DATE,'yyyy-MM-dd HH:mm:ss')) AS DURUM_2 FROM SIPARISE A " +
                               " WHERE A.ID="+ID+" ";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    foreach (System.Web.UI.WebControls.ListItem item in lstbanka.Items)
                    {
                        if (item.Value == dr["BANKA2"].ToString().Trim() || item.Value == dr["BANKA"].ToString().Trim() || item.Value == dr["BANKA3"].ToString().Trim())
                        {
                            item.Selected = true;
                        }
                    }
                    txt_siparisno_kart.Text = dr["SIPNO"].ToString().Trim();
                    datepicker1.Value = dr["TARIH"].ToString().Trim();
                    if (dr["CH_KODU"].ToString().Trim()!="")
                    {
                        drp_carikodu_kart.ClearSelection();
                        drp_carikodu_kart.Items.FindByValue(dr["CH_KODU"].ToString().Trim()).Selected = true;
                    }
                    else
                    {
                        drp_carikodu_kart.ClearSelection();
                        drp_carikodu_kart.Items.FindByValue("Cari Seçiniz").Selected = true;
                    }
                    yetkili_bul_av();
                    if (dr["MUSTERI_TEMS"].ToString().Trim()!="" && dr["MUSTERI_TEMS"].ToString().Trim()!="0")
                    {
                        drp_must_yetkili.ClearSelection();
                        drp_must_yetkili.Items.FindByValue(dr["MUSTERI_TEMS"].ToString().Trim()).Selected = true;
                    }
                    else
                    {
                        drp_must_yetkili.ClearSelection();
                        drp_must_yetkili.Items.FindByValue("Yetkili seçiniz").Selected = true;
                    }
                    txt_odeme.Text= dr["ODEME"].ToString().Trim();
                    txt_vade.Text= dr["VADE"].ToString().Trim();
                    txt_siparisnotlar_kart.Text =dr["NOTLAR"].ToString().Trim();
                    txt_uretim_notlar.Text =dr["URETIM_NOTLAR"].ToString().Trim();

                    if (dr["ONAY"].ToString().Trim()=="E")
                    {
                        onay1.Checked=true;
                        lbl_onay_info.Text=dr["DURUM_1"].ToString().Trim();
                    }
                    else
                    {
                        onay1.Checked=false;
                        lbl_onay_info.Text="";
                    }

                    if (dr["DURUM"].ToString().Trim()=="")
                    {
                        drp_siparis_durum.ClearSelection();
                        drp_siparis_durum.Items.FindByValue("0").Selected = true;
                        lbl_durum_onay.Text="";
                    }
                    else
                    {
                        drp_siparis_durum.ClearSelection();
                        drp_siparis_durum.Items.FindByValue(dr["DURUM"].ToString().Trim()).Selected = true;
                        lbl_durum_onay.Text=dr["DURUM_2"].ToString().Trim();
                    }

                }
                dr.Close();
                con.Close();
            }

            using (SqlConnection con2 = new SqlConnection(constr))
            {
                con2.Open();
                string query2 = " SELECT TRIM(B.REVIZYON) AS REVIZYON,B.ID,B.SIPNO, C.KOD AS STOK_KOD,B.STOK_AD,B.MIKTAR,B.BIRIM,B.TESLIM_TARIH,B.SATIR_NOT,B.BIRIM_FIYAT,CONCAT(B.TOPLAM_TUTAR,' ',B.PARA_BIRIMI) AS TOPLAM_TUTAR," +
                                " B.K_MIN,B.K_MAX,B.YU_MIN,B.YU_MAX,B.YAG_MIN,B.YAG_MAX,B.CI_MIN,B.CI_MAX,B.KALAN_MIKTAR FROM SIPARISE A " +
                                " LEFT OUTER JOIN SIPARIST B  ON ( TRIM(A.SIPNO)=TRIM(B.SIPNO) ) " +
                                " LEFT OUTER JOIN STOK_KART C ON ( TRIM(B.STOK_KOD)=TRIM(C.KOD) ) "+
                                " WHERE A.ID="+ID+" ";
                SqlCommand cmd2 = new SqlCommand(query2, con2);
                SqlDataAdapter adp2 = new SqlDataAdapter(cmd2);
                DataSet ds2 = new DataSet();
                adp2.Fill(ds2);
                grd_siparis_detay.DataSource = ds2;
                grd_siparis_detay.DataBind();

                string query3 = " SELECT * FROM  SIPARIST WHERE TRIM(SIPNO)='"+txt_siparisno_kart.Text.Trim()+"'";
                SqlCommand cmd3 = new SqlCommand(query3, con2);
                SqlDataReader dr3 = cmd3.ExecuteReader();
                if (dr3.HasRows)
                {
                    dr3.Read();
                    drp_kdv.ClearSelection();
                    drp_kdv.Items.FindByValue(dr3["KDV"].ToString().Trim()).Selected = true;
                }
                else
                {
                    drp_kdv.ClearSelection();
                    drp_kdv.Items.FindByText("KDV Yok").Selected = true;
                }
                con2.Close();
            }
            usr_name.siparis_durum=drp_siparis_durum.SelectedValue.Trim();
            sip_dokuman_bindign(order_no);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ChangeForm(5)", true);

        }

        private void yetkili_bul_av()
        {
            yetkili_bul_binding();
        }

        private void yetkili_bul_binding()
        {
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(" select ID,TRIM(YETKILI) AS YETKILI,CONCAT(TRIM(YETKILI),'-',TRIM(UNVAN)) AS YETKILI_1 from CARI_YETKILI WHERE TRIM(CARI_KOD)='"+drp_carikodu_kart.SelectedValue.Trim()+"'  ", con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                drp_must_yetkili.Items.Clear();
                drp_must_yetkili.Items.Add("Yetkili seçiniz");
                drp_must_yetkili.DataTextField = "YETKILI_1";
                drp_must_yetkili.DataValueField = "ID";
                drp_must_yetkili.DataSource = ds;
                drp_must_yetkili.DataBind();
                con.Close();
            }
        }

        private void sip_dokuman_bindign(string order_no)
        {
            string path2 = @"\\192.168.1.4\dokumanlar\SIPARISLER\"+order_no.Trim();
            System.IO.FileInfo file2 = new System.IO.FileInfo(path2);
            if (Directory.Exists(path2))
            {
                string[] klasordekiler = Directory.GetFiles(path2);
                DataTable tablo = new DataTable();
                tablo.Columns.Add("ID");
                foreach (string resimdosyasi in klasordekiler)
                {
                    string dosya_1 = System.IO.Path.GetFileName(resimdosyasi);
                    DataRow row1 = tablo.NewRow();
                    row1["ID"] = System.IO.Path.GetFileName(resimdosyasi);
                    tablo.Rows.Add(row1);
                    grd_siparis_dokumanlar.DataSource = tablo;
                    grd_siparis_dokumanlar.DataBind();
                }
            }
            else
            {
                grd_siparis_dokumanlar.DataSource = null;
                grd_siparis_dokumanlar.DataBind();
            }
        }

        protected void bnt_uretim_av(object sender, EventArgs e)
        {
            if (txt_siparisno_kart.Text.Trim()=="")
            {
                Response.Write("<script lang='JavaScript'>alert('Sipariş numaraası seçilmeden imalat bildirim formu alınamaz. Kayıt yapılmadı.');</script>");
                return;
            }
            if (!onay1.Checked)
            {
                Response.Write("<script lang='JavaScript'>alert('Sipariş onayı verilmemiş olan siparişler için imalat bildirim formu çıkartılamaz.. Kayıt yapılmadı.');</script>");
                return;
            }
            imalat_form_print();
        }

        private void imalat_form_print()
        {

            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (MemoryStream ms = new MemoryStream())
            {
                //*************************** HEADER BİLGİLERİ YAZDIRILIYOR
                Document document = new Document(iTextSharp.text.PageSize.A4.Rotate(), 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/depar_logo.png"));
                img.ScaleToFit(100, 200);
                img.Border = 0;
                //img.Border = iTextSharp.text.Rectangle.BOX;
                //img.BorderColor = iTextSharp.text.BaseColor.BLACK;
                //img.BorderWidth = 5f;
                document.Add(img);


                BaseFont bF = BaseFont.CreateFont("C:\\Windows\\Fonts\\Arial.ttf", "windows-1254", true);

                PdfPTable table = new PdfPTable(3);
                table.WidthPercentage = 100;
                PdfPCell cell = new PdfPCell(new Phrase("SİPARİŞ ALMA VE İMALAT BİLDİRİM FORMU", new iTextSharp.text.Font(bF, 18f, iTextSharp.text.Font.BOLD)));
                cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = 0;
                table.AddCell(cell);
                document.Add(table);
                document.Add(new Paragraph(" "));

                PdfContentByte cb = writer.DirectContent;

                table = new PdfPTable(2);
                table.WidthPercentage = 45;

                float[] colWidthsaccing4 = { 10, 35 };
                table.SetWidths(colWidthsaccing4);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;

                PdfPCell cell23 = new PdfPCell(new Phrase("Firma Adı", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell23.HorizontalAlignment = Element.ALIGN_LEFT;
                cell23.Border = 0;
                table.AddCell(cell23);

                PdfPCell cell34 = new PdfPCell(new Phrase(drp_carikodu_kart.SelectedItem.Text.Trim(), new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell34.HorizontalAlignment = Element.ALIGN_LEFT;
                cell34.Border = 0;
                table.AddCell(cell34);

                cell = new PdfPCell();
                PdfPCell cell21 = new PdfPCell(new Phrase("Sipariş No:", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell21.HorizontalAlignment = Element.ALIGN_LEFT;
                cell21.Border = 0;
                table.AddCell(cell21);

                PdfPCell cell22 = new PdfPCell(new Phrase(txt_siparisno_kart.Text.Trim(), new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell22.HorizontalAlignment = Element.ALIGN_LEFT;
                cell22.Border = 0;
                table.AddCell(cell22);


                //PdfPCell cell221 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                //cell221.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell221.Border = 0;
                //table.AddCell(cell221);




                PdfPCell cell31 = new PdfPCell(new Phrase("Sipariş Tarihi:", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell31.HorizontalAlignment = Element.ALIGN_LEFT;
                cell31.Border = 0;
                table.AddCell(cell31);


                PdfPCell cell32 = new PdfPCell(new Phrase(datepicker1.Value.Trim(), new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell32.HorizontalAlignment = Element.ALIGN_LEFT;
                cell32.Border = 0;
                table.AddCell(cell32);





                document.Add(table);
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));


                //********************************** satır başlık bilgileri 
                table = new PdfPTable(19);
                table.WidthPercentage = 100;

                PdfPCell cell40 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell40.HorizontalAlignment = Element.ALIGN_CENTER;
                cell40.Border = 0;
                table.AddCell(cell40);

                PdfPCell cell41 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell41.HorizontalAlignment = Element.ALIGN_CENTER;
                cell41.Colspan = 2;
                cell41.Border = 0;
                table.AddCell(cell41);

                PdfPCell cell42 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell42.HorizontalAlignment = Element.ALIGN_CENTER;
                cell42.Colspan = 2;
                cell42.Border = 0;
                table.AddCell(cell42);

                PdfPCell cell43 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell43.HorizontalAlignment = Element.ALIGN_CENTER;
                cell43.Border = 0;
                cell43.Colspan = 2;
                table.AddCell(cell43);

                PdfPCell cell48 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell48.HorizontalAlignment = Element.ALIGN_CENTER;
                cell48.Border = 0;
                table.AddCell(cell48);

                PdfPCell cell44 = new PdfPCell(new Phrase("Mil Çapı", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell44.HorizontalAlignment = Element.ALIGN_CENTER;
                cell44.Colspan = 2;
                table.AddCell(cell44);

                PdfPCell cell45 = new PdfPCell(new Phrase("Kep Çapı", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell45.HorizontalAlignment = Element.ALIGN_CENTER;
                cell45.Colspan = 2;
                table.AddCell(cell45);

                PdfPCell cell46 = new PdfPCell(new Phrase("Yağ Boşluğu", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell46.HorizontalAlignment = Element.ALIGN_CENTER;
                cell46.Colspan = 2;
                table.AddCell(cell46);

                PdfPCell cell47 = new PdfPCell(new Phrase("Cidar", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell47.HorizontalAlignment = Element.ALIGN_CENTER;
                cell47.Colspan = 2;
                table.AddCell(cell47);



                PdfPCell cell49 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell49.HorizontalAlignment = Element.ALIGN_CENTER;
                cell49.Border = 0;
                table.AddCell(cell49);

                PdfPCell cell491 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell491.HorizontalAlignment = Element.ALIGN_CENTER;
                cell491.Border = 0;
                table.AddCell(cell491);

                PdfPCell cell492 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell492.HorizontalAlignment = Element.ALIGN_CENTER;
                cell492.Border = 0;
                table.AddCell(cell492);

                document.Add(table);



                //*****************

                table = new PdfPTable(19);
                table.WidthPercentage = 100;

                PdfPCell cell50 = new PdfPCell(new Phrase("Depar No", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell50.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell50);

                PdfPCell cell51 = new PdfPCell(new Phrase("Marka", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell51.HorizontalAlignment = Element.ALIGN_CENTER;
                cell51.Colspan = 2;
                table.AddCell(cell51);

                PdfPCell cell52 = new PdfPCell(new Phrase("Model", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell52.HorizontalAlignment = Element.ALIGN_CENTER;
                cell52.Colspan = 2;
                table.AddCell(cell52);

                PdfPCell cell53 = new PdfPCell(new Phrase("Cinsi", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell53.HorizontalAlignment = Element.ALIGN_CENTER;
                cell53.Colspan = 2;
                table.AddCell(cell53);

                PdfPCell cell58 = new PdfPCell(new Phrase("Miktar", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell58.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell58);

                PdfPCell cell54 = new PdfPCell(new Phrase("min", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell54.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell54);

                PdfPCell cell541 = new PdfPCell(new Phrase("max", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell541.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell541);


                PdfPCell cell55 = new PdfPCell(new Phrase("min", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell55.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell55);

                PdfPCell cell551 = new PdfPCell(new Phrase("max", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell551.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell551);


                PdfPCell cell56 = new PdfPCell(new Phrase("min", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell56.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell56);

                PdfPCell cell561 = new PdfPCell(new Phrase("max", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell561.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell561);

                PdfPCell cell57 = new PdfPCell(new Phrase("min", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell57.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell57);

                PdfPCell cell571 = new PdfPCell(new Phrase("max", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell571.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell571);



                PdfPCell cell59 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell59.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell59);

                PdfPCell cell591 = new PdfPCell(new Phrase("Num", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell591.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell591);

                PdfPCell cell592 = new PdfPCell(new Phrase("T.Tarih", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell592.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell592);

                document.Add(table);

                //*************************************************************************



                foreach (GridViewRow gvrow in grd_siparis_detay.Rows)
                {
                    string stok_kod = gvrow.Cells[3].Text.Trim();
                    string SID = gvrow.Cells[2].Text.Trim();
                    string stok_kod_rev = gvrow.Cells[5].Text.Trim();
                    if (stok_kod_rev.Trim()=="S" || stok_kod_rev.Trim()=="")
                    {
                        stok_kod_rev="R";
                    }

                    string miktar = gvrow.Cells[7].Text.Trim();
                    string t_tarih = Convert.ToDateTime(gvrow.Cells[11].Text.Trim()).ToString("dd-MM-yy");
                    string depar_kod = "";
                    string marka = "";
                    string model = "";
                    string cinsi = "";
                    string satir_not = gvrow.Cells[12].Text.Trim();

                    string m_min = gvrow.Cells[13].Text.Trim();
                    string m_max = gvrow.Cells[14].Text.Trim();

                    string k_min = gvrow.Cells[15].Text.Trim();
                    string k_max = gvrow.Cells[16].Text.Trim();

                    string y_min = gvrow.Cells[17].Text.Trim();
                    string y_max = gvrow.Cells[18].Text.Trim();

                    string c_min = gvrow.Cells[19].Text.Trim();
                    string c_max = gvrow.Cells[20].Text.Trim();


                    using (SqlConnection con2 = new SqlConnection(constr))
                    {
                        con2.Open();
                        //string query = " SELECT A.*,B.ID AS DEPARNO,B.GK_5,(SELECT C.GRUPKODU_AD FROM GRUPKODU C WHERE C.KOD='GK_5' AND B.GK_5=C.GRUPKODU) AS MARKA," +
                        //               " B.GK_6,(SELECT D.GRUPKODU_AD FROM C D WHERE D.KOD='GK_6' AND B.GK_5=D.GRUPKODU) AS MODEL, "+
                        //               " B.GK_4,(SELECT E.GRUPKODU_AD FROM GRUPKODU E WHERE E.KOD='GK_4' AND B.GK_4=E.GRUPKODU) AS CINSI, "+
                        //               " B.DEPAR_KOD FROM STOK_KART_REV A  " +
                        //               " LEFT OUTER JOIN STOK_KART B  ON ( TRIM(A.KOD)=TRIM(B.KOD) ) " +
                        //               " WHERE TRIM(A.KOD)='"+stok_kod.Trim()+"'  AND  TRIM(A.REVIZYON)='"+stok_kod_rev.Trim()+"' ";
                        string query = " select A.*,B.DEPAR_KOD AS DEPAR_KOD, D.GRUPKODU_AD AS MODEL,C.GRUPKODU_AD AS MARKA,E.GRUPKODU_AD AS CINSI from SIPARIST A " +
                                       " LEFT OUTER JOIN STOK_KART B ON ( TRIM(A.STOK_KOD)=TRIM(B.KOD)  )  " +
                                       " LEFT OUTER JOIN GRUPKODU C ON ( TRIM(B.GK_5)=TRIM(C.GRUPKODU) AND TRIM(C.KOD)='GK_5' )  " +
                                       " LEFT OUTER JOIN GRUPKODU D ON ( TRIM(B.GK_6)=TRIM(D.GRUPKODU) AND TRIM(D.KOD)='GK_6' )  " +
                                       " LEFT OUTER JOIN GRUPKODU E ON ( TRIM(B.GK_4)=TRIM(E.GRUPKODU) AND TRIM(E.KOD)='GK_4' )  " +
                                       " WHERE A.ID='"+SID+"' ";
                        SqlCommand cmd2 = new SqlCommand(query, con2);
                        SqlDataReader dr2 = cmd2.ExecuteReader();
                        if (dr2.HasRows)
                        {
                            dr2.Read();
                            depar_kod = dr2["DEPAR_KOD"].ToString().Trim();
                            marka = dr2["MARKA"].ToString().Trim();
                            model = dr2["MODEL"].ToString().Trim();
                            cinsi = dr2["CINSI"].ToString().Trim();
                            m_min = dr2["K_MIN"].ToString().Trim();
                            m_max = dr2["K_MAX"].ToString().Trim();
                            k_min = dr2["YU_MIN"].ToString().Trim();
                            k_max = dr2["YU_MAX"].ToString().Trim();
                            y_min = dr2["YAG_MIN"].ToString().Trim();
                            y_max = dr2["YAG_MAX"].ToString().Trim();
                            c_min = dr2["CI_MIN"].ToString().Trim();
                            c_max = dr2["CI_MAX"].ToString().Trim();
                        }
                        dr2.Close();
                        con2.Close();
                    }


                    table = new PdfPTable(19);
                    table.WidthPercentage = 100;

                    PdfPCell cell60 = new PdfPCell(new Phrase(depar_kod.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell60.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell60);

                    PdfPCell cell61 = new PdfPCell(new Phrase(marka.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell61.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell61.Colspan = 2;
                    table.AddCell(cell61);

                    PdfPCell cell62 = new PdfPCell(new Phrase(model.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell62.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell62.Colspan = 2;
                    table.AddCell(cell62);

                    PdfPCell cell63 = new PdfPCell(new Phrase(cinsi.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell63.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell63.Colspan = 2;
                    table.AddCell(cell63);

                    PdfPCell cell693 = new PdfPCell(new Phrase(miktar.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell693.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell693);

                    PdfPCell cell64 = new PdfPCell(new Phrase(m_min.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell64.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell64);

                    PdfPCell cell65 = new PdfPCell(new Phrase(m_max.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell65.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell65);


                    PdfPCell cell66 = new PdfPCell(new Phrase(k_min.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell66.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell66);

                    PdfPCell cell67 = new PdfPCell(new Phrase(k_max.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell67.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell67);


                    PdfPCell cell68 = new PdfPCell(new Phrase(y_min.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell68.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell68);

                    PdfPCell cell69 = new PdfPCell(new Phrase(y_max.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell69.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell69);

                    PdfPCell cell691 = new PdfPCell(new Phrase(c_min.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell691.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell691);

                    PdfPCell cell692 = new PdfPCell(new Phrase(c_max.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell692.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell692);



                    PdfPCell cell694 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell694.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell694.Colspan = 3;
                    table.AddCell(cell694);

                    PdfPCell cell695 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell695.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell695);

                    PdfPCell cell696 = new PdfPCell(new Phrase(t_tarih.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell696.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell696);

                    document.Add(table);


                }

                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));


                table = new PdfPTable(1);
                table.WidthPercentage = 100;

                PdfPCell cell70 = new PdfPCell(new Phrase("Sipariş Notları", new iTextSharp.text.Font(bF, 15f, iTextSharp.text.Font.BOLD)));
                cell70.HorizontalAlignment = Element.ALIGN_LEFT;
                cell70.Border = 0;
                table.AddCell(cell70);

                PdfPCell cell71 = new PdfPCell(new Phrase(txt_uretim_notlar.Text.Trim(), new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell71.HorizontalAlignment = Element.ALIGN_LEFT;
                cell71.Border = 0;
                table.AddCell(cell71);
                document.Add(table);



                document.Close();
                writer.Close();
                Response.ContentType = "pdf/application";
                Response.AddHeader("content-disposition",
                "attachment;filename=" + "imalat_bildirim_formu" + DateTime.Now.ToString("dd/MM/yyyy") + ".pdf");
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);

            }
        }

        protected void print_av(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (MemoryStream ms = new MemoryStream())
            {
                //*************************** HEADER BİLGİLERİ YAZDIRILIYOR
                Document document = new Document(iTextSharp.text.PageSize.A4.Rotate(), 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/depar_logo.png"));
                img.ScaleToFit(150, 150);
                img.SetAbsolutePosition(20, 530);
                img.Border = 0;
                //img.Border = iTextSharp.text.Rectangle.BOX;
                //img.BorderColor = iTextSharp.text.BaseColor.BLACK;
                //img.BorderWidth = 5f;
                document.Add(img);

                iTextSharp.text.Image IMGISO9001 = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/ISO9001.png"));
                IMGISO9001.ScaleToFit(100, 100);
                IMGISO9001.SetAbsolutePosition(620, 510);
                //IMGISO9001.Border = iTextSharp.text.Rectangle.BOX;
                IMGISO9001.Border = 0;
                //IMGISO9001.BorderColor = iTextSharp.text.BaseColor.BLACK;
                //IMGISO9001.BorderWidth = 1f;
                document.Add(IMGISO9001);


                BaseFont bF = BaseFont.CreateFont("C:\\Windows\\Fonts\\Arial.ttf", "windows-1254", true);

                PdfPTable table = new PdfPTable(3);
                table.WidthPercentage = 100;
                PdfPCell cell = new PdfPCell(new Phrase("SİPARİŞ TEYİT FORMU", new iTextSharp.text.Font(bF, 18f, iTextSharp.text.Font.BOLD)));
                cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = 0;
                table.AddCell(cell);
                document.Add(table);
                document.Add(new Paragraph(" "));

                PdfContentByte cb = writer.DirectContent;

                table = new PdfPTable(2);
                table.WidthPercentage = 45;

                float[] colWidthsaccing4 = { 10, 35 };
                table.SetWidths(colWidthsaccing4);
                table.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;

                cell = new PdfPCell();
                PdfPCell cell21 = new PdfPCell(new Phrase("Sipariş No:", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell21.HorizontalAlignment = Element.ALIGN_LEFT;
                cell21.Border = 0;
                table.AddCell(cell21);

                PdfPCell cell22 = new PdfPCell(new Phrase(txt_siparisno_kart.Text.Trim(), new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell22.HorizontalAlignment = Element.ALIGN_LEFT;
                cell22.Border = 0;
                table.AddCell(cell22);


                PdfPCell cell23 = new PdfPCell(new Phrase("Firma Adı", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell23.HorizontalAlignment = Element.ALIGN_LEFT;
                cell23.Border = 0;
                table.AddCell(cell23);

                PdfPCell cell34 = new PdfPCell(new Phrase(drp_carikodu_kart.SelectedItem.Text.Trim(), new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell34.HorizontalAlignment = Element.ALIGN_LEFT;
                cell34.Border = 0;
                table.AddCell(cell34);


                PdfPCell cell31 = new PdfPCell(new Phrase("Sipariş Tarihi:", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell31.HorizontalAlignment = Element.ALIGN_LEFT;
                cell31.Border = 0;
                table.AddCell(cell31);

                PdfPCell cell32 = new PdfPCell(new Phrase(datepicker1.Value.Trim(), new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell32.HorizontalAlignment = Element.ALIGN_LEFT;
                cell32.Border = 0;
                table.AddCell(cell32);


                string t_tarih_2 = "";
                foreach (GridViewRow gvrow in grd_siparis_detay.Rows)
                {
                    if (gvrow.Cells[11].Text.Trim()=="" || gvrow.Cells[11].Text.Trim()=="&nbsp;")
                    {
                        t_tarih_2 = "";
                    }
                    else
                    {
                        t_tarih_2 = Convert.ToDateTime(gvrow.Cells[11].Text.Trim()).ToString("yyyy-MM-dd");
                    }
                }


                PdfPCell cell31_2 = new PdfPCell(new Phrase("Teslim Tarihi:", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell31_2.HorizontalAlignment = Element.ALIGN_LEFT;
                cell31_2.Border = 0;
                table.AddCell(cell31_2);

                PdfPCell cell32_3 = new PdfPCell(new Phrase(t_tarih_2.Trim(), new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell32_3.HorizontalAlignment = Element.ALIGN_LEFT;
                cell32_3.Border = 0;
                table.AddCell(cell32_3);


                document.Add(table);
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));


                //********************************** satır başlık bilgileri 
                table = new PdfPTable(19);
                table.WidthPercentage = 100;

                PdfPCell cell40 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell40.HorizontalAlignment = Element.ALIGN_CENTER;
                cell40.Border = 0;
                table.AddCell(cell40);

                PdfPCell cell41 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell41.HorizontalAlignment = Element.ALIGN_CENTER;
                cell41.Colspan = 2;
                cell41.Border = 0;
                table.AddCell(cell41);

                PdfPCell cell42 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell42.HorizontalAlignment = Element.ALIGN_CENTER;
                cell42.Colspan = 2;
                cell42.Border = 0;
                table.AddCell(cell42);

                PdfPCell cell43 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell43.HorizontalAlignment = Element.ALIGN_CENTER;
                cell43.Border = 0;
                cell43.Colspan = 2;
                table.AddCell(cell43);

                PdfPCell cell48 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell48.HorizontalAlignment = Element.ALIGN_CENTER;
                cell48.Border = 0;
                table.AddCell(cell48);

                PdfPCell cell44 = new PdfPCell(new Phrase("Mil Çapı", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell44.HorizontalAlignment = Element.ALIGN_CENTER;
                cell44.Colspan = 2;
                table.AddCell(cell44);

                PdfPCell cell45 = new PdfPCell(new Phrase("Kep Çapı", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell45.HorizontalAlignment = Element.ALIGN_CENTER;
                cell45.Colspan = 2;
                table.AddCell(cell45);

                PdfPCell cell46 = new PdfPCell(new Phrase("Yağ Boşluğu", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell46.HorizontalAlignment = Element.ALIGN_CENTER;
                cell46.Colspan = 2;
                table.AddCell(cell46);

                PdfPCell cell47 = new PdfPCell(new Phrase("Cidar", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell47.HorizontalAlignment = Element.ALIGN_CENTER;
                cell47.Colspan = 2;
                table.AddCell(cell47);



                PdfPCell cell49 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell49.HorizontalAlignment = Element.ALIGN_CENTER;
                cell49.Border = 0;
                table.AddCell(cell49);

                PdfPCell cell491 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell491.HorizontalAlignment = Element.ALIGN_CENTER;
                cell491.Border = 0;
                table.AddCell(cell491);

                PdfPCell cell492 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell492.HorizontalAlignment = Element.ALIGN_CENTER;
                cell492.Border = 0;
                table.AddCell(cell492);

                document.Add(table);



                //*****************

                table = new PdfPTable(19);
                table.WidthPercentage = 100;

                PdfPCell cell50 = new PdfPCell(new Phrase("Depar No", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell50.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell50);

                PdfPCell cell51 = new PdfPCell(new Phrase("Marka", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell51.HorizontalAlignment = Element.ALIGN_CENTER;
                cell51.Colspan = 2;
                table.AddCell(cell51);

                PdfPCell cell52 = new PdfPCell(new Phrase("Model", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell52.HorizontalAlignment = Element.ALIGN_CENTER;
                cell52.Colspan = 2;
                table.AddCell(cell52);

                PdfPCell cell53 = new PdfPCell(new Phrase("Cinsi", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell53.HorizontalAlignment = Element.ALIGN_CENTER;
                cell53.Colspan = 2;
                table.AddCell(cell53);

                PdfPCell cell58 = new PdfPCell(new Phrase("Miktar", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell58.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell58);

                PdfPCell cell54 = new PdfPCell(new Phrase("min", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell54.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell54);

                PdfPCell cell541 = new PdfPCell(new Phrase("max", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell541.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell541);


                PdfPCell cell55 = new PdfPCell(new Phrase("min", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell55.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell55);

                PdfPCell cell551 = new PdfPCell(new Phrase("max", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell551.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell551);


                PdfPCell cell56 = new PdfPCell(new Phrase("min", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell56.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell56);

                PdfPCell cell561 = new PdfPCell(new Phrase("max", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell561.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell561);

                PdfPCell cell57 = new PdfPCell(new Phrase("min", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell57.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell57);

                PdfPCell cell571 = new PdfPCell(new Phrase("max", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell571.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell571);



                PdfPCell cell59 = new PdfPCell(new Phrase("Açıklama", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell59.HorizontalAlignment = Element.ALIGN_CENTER;
                cell59.Colspan = 3;
                table.AddCell(cell59);

                //PdfPCell cell591 = new PdfPCell(new Phrase("Num", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                //cell591.HorizontalAlignment = Element.ALIGN_CENTER;
                //table.AddCell(cell591);

                //PdfPCell cell592 = new PdfPCell(new Phrase("T.Tarih", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                //cell592.HorizontalAlignment = Element.ALIGN_CENTER;
                //table.AddCell(cell592);

                document.Add(table);

                //*************************************************************************



                foreach (GridViewRow gvrow in grd_siparis_detay.Rows)
                {
                    string stok_kod = gvrow.Cells[3].Text.Trim();
                    string stok_kod_rev = gvrow.Cells[5].Text.Trim();

                    if (stok_kod_rev.Trim()=="S" || stok_kod_rev.Trim()=="" || stok_kod_rev.Trim()== null)
                    {
                        stok_kod_rev="R";
                    }
                    else
                    {
                        stok_kod_rev=gvrow.Cells[5].Text.Trim();
                    }

                    string miktar = gvrow.Cells[7].Text.Trim();
                    string t_tarih = "";
                    if (gvrow.Cells[11].Text.Trim()=="" || gvrow.Cells[11].Text.Trim()=="&nbsp;")
                    {
                        t_tarih = "";
                    }
                    else
                    {
                        t_tarih = Convert.ToDateTime(gvrow.Cells[11].Text.Trim()).ToString("yyyy-MM-dd");
                    }

                    string depar_kod = "";
                    string marka = "";
                    string model = "";
                    string cinsi = "";

                    string aciklama = HttpUtility.HtmlDecode(gvrow.Cells[12].Text.Trim());
                    string m_min = gvrow.Cells[13].Text.Trim();
                    string m_max = gvrow.Cells[14].Text.Trim();
                    string k_min = gvrow.Cells[15].Text.Trim();
                    string k_max = gvrow.Cells[16].Text.Trim();
                    string y_min = gvrow.Cells[17].Text.Trim();
                    string y_max = gvrow.Cells[18].Text.Trim();
                    string c_min = gvrow.Cells[19].Text.Trim();
                    string c_max = gvrow.Cells[20].Text.Trim();


                    using (SqlConnection con2 = new SqlConnection(constr))
                    {
                        con2.Open();
                        string query = " SELECT A.*,B.ID AS DEPARNO,B.GK_5,(SELECT C.GRUPKODU_AD FROM GRUPKODU C WHERE C.KOD='GK_5' AND B.GK_5=C.GRUPKODU) AS MARKA," +
                                       " B.GK_6,(SELECT D.GRUPKODU_AD FROM GRUPKODU D WHERE D.KOD='GK_6' AND B.GK_5=D.GRUPKODU) AS MODEL, "+
                                       " B.GK_4,(SELECT E.GRUPKODU_AD FROM GRUPKODU E WHERE E.KOD='GK_4' AND B.GK_4=E.GRUPKODU) AS CINSI, "+
                                       " B.DEPAR_KOD FROM STOK_KART_REV A  " +
                                       " LEFT OUTER JOIN STOK_KART B  ON ( TRIM(A.KOD)=TRIM(B.KOD) ) " +
                                       " WHERE TRIM(A.KOD)='"+stok_kod.Trim()+"'  AND  TRIM(A.REVIZYON)='"+stok_kod_rev.Trim()+"' ";
                        SqlCommand cmd2 = new SqlCommand(query, con2);
                        SqlDataReader dr2 = cmd2.ExecuteReader();
                        if (dr2.HasRows)
                        {
                            dr2.Read();
                            depar_kod = dr2["DEPAR_KOD"].ToString().Trim();
                            marka = dr2["MARKA"].ToString().Trim();
                            model = dr2["MODEL"].ToString().Trim();
                            cinsi = dr2["CINSI"].ToString().Trim();
                            m_min = dr2["MIL_MIN"].ToString().Trim();
                            m_max = dr2["MIL_MAX"].ToString().Trim();
                            k_min = dr2["KEP_MIN"].ToString().Trim();
                            k_max = dr2["KEP_MAX"].ToString().Trim();
                            y_min = dr2["YAG_MIN"].ToString().Trim();
                            y_max = dr2["YAG_MAX"].ToString().Trim();
                            c_min = dr2["CIDAR_MIN"].ToString().Trim();
                            c_max = dr2["CIDAR_MAX"].ToString().Trim();
                        }
                        dr2.Close();
                        con2.Close();
                    }


                    table = new PdfPTable(19);
                    table.WidthPercentage = 100;

                    PdfPCell cell60 = new PdfPCell(new Phrase(depar_kod.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell60.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell60);
                    PdfPCell cell61 = new PdfPCell(new Phrase(marka.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell61.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell61.Colspan = 2;
                    table.AddCell(cell61);

                    PdfPCell cell62 = new PdfPCell(new Phrase(model.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell62.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell62.Colspan = 2;
                    table.AddCell(cell62);

                    PdfPCell cell63 = new PdfPCell(new Phrase(cinsi.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell63.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell63.Colspan = 2;
                    table.AddCell(cell63);

                    PdfPCell cell693 = new PdfPCell(new Phrase(miktar.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell693.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell693);

                    PdfPCell cell64 = new PdfPCell(new Phrase(m_min.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell64.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell64);

                    PdfPCell cell65 = new PdfPCell(new Phrase(m_max.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell65.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell65);


                    PdfPCell cell66 = new PdfPCell(new Phrase(k_min.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell66.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell66);

                    PdfPCell cell67 = new PdfPCell(new Phrase(k_max.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell67.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell67);


                    PdfPCell cell68 = new PdfPCell(new Phrase(y_min.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell68.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell68);

                    PdfPCell cell69 = new PdfPCell(new Phrase(y_max.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell69.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell69);

                    PdfPCell cell691 = new PdfPCell(new Phrase(c_min.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell691.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell691);

                    PdfPCell cell692 = new PdfPCell(new Phrase(c_max.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell692.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell692);



                    PdfPCell cell694 = new PdfPCell(new Phrase(aciklama.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell694.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell694.Colspan = 3;
                    table.AddCell(cell694);

                    //PdfPCell cell695 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    //cell695.HorizontalAlignment = Element.ALIGN_CENTER;
                    //table.AddCell(cell695);

                    //PdfPCell cell696 = new PdfPCell(new Phrase(t_tarih.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    //cell696.HorizontalAlignment = Element.ALIGN_CENTER;
                    //table.AddCell(cell696);

                    document.Add(table);


                }

                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));

                table = new PdfPTable(1);
                table.WidthPercentage = 100;

                PdfContentByte pcb = writer.DirectContent;
                table = new PdfPTable(2);
                table.TotalWidth = 800f;
                PdfPCell cell120 = new PdfPCell(new Phrase("DEPAR ONAY", new iTextSharp.text.Font(bF, 11f, iTextSharp.text.Font.NORMAL)));
                cell120.HorizontalAlignment = Element.ALIGN_CENTER;
                cell120.Border = 0;
                table.AddCell(cell120);

                PdfPCell cell121 = new PdfPCell(new Phrase("MUSTERI ONAY", new iTextSharp.text.Font(bF, 11f, iTextSharp.text.Font.NORMAL)));
                cell121.HorizontalAlignment = Element.ALIGN_CENTER;
                cell121.Border = 0;
                table.AddCell(cell121);

                table.WriteSelectedRows(0, -1, 25, 150, pcb);


                table = new PdfPTable(1);
                table.TotalWidth = 800f;
                document.Add(new Paragraph(" "));

                PdfPCell cell1362 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.ITALIC)));
                cell1362.HorizontalAlignment = Element.ALIGN_CENTER;
                cell1362.Border = 0;
                table.AddCell(cell1362);

                PdfPCell cell1363 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.ITALIC)));
                cell1363.HorizontalAlignment = Element.ALIGN_CENTER;
                cell1363.Border = 0;
                table.AddCell(cell1363);

                PdfPCell cell137 = new PdfPCell(new Phrase("Esenler mahallesi sude sokak no:2 Pendik / İSTANBUL / TURKİYE :", new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.ITALIC)));
                cell137.HorizontalAlignment = Element.ALIGN_CENTER;
                cell137.Border = 0;
                table.AddCell(cell137);


                PdfPCell cell138 = new PdfPCell(new Phrase("www.deparbearings.com  email:info@deparbearings.com  Tel:(0216) 395 95 63 Fax:(0216) 395 95 69", new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.ITALIC)));
                cell138.HorizontalAlignment = Element.ALIGN_CENTER;
                cell138.Border = 0;
                table.AddCell(cell138);

                PdfPCell cell139 = new PdfPCell(new Phrase("V.D: Pendik Vergi Dairesi   V.NO: 9260842681  ", new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.ITALIC)));
                cell139.HorizontalAlignment = Element.ALIGN_CENTER;
                cell139.Border = 0;
                table.AddCell(cell139);



                table.WriteSelectedRows(0, -1, 25, 120, pcb);


                iTextSharp.text.Image IMGISO90012 = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/ISO9001.png"));
                IMGISO90012.ScaleToFit(100, 70);
                IMGISO90012.SetAbsolutePosition(225, 10);
                //IMGISO9001.Border = iTextSharp.text.Rectangle.BOX;
                IMGISO90012.Border = 0;
                IMGISO90012.BorderColor = iTextSharp.text.BaseColor.BLACK;
                IMGISO90012.BorderWidth = 1f;
                document.Add(IMGISO90012);

                //iTextSharp.text.Image IMGISO140001 = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/ISO14001.png"));
                //IMGISO140001.ScaleToFit(100, 100);
                //IMGISO140001.SetAbsolutePosition(400, 10);
                ////IMGISO140001.Border = iTextSharp.text.Rectangle.BOX;
                //IMGISO140001.Border = 0;
                //IMGISO140001.BorderColor = iTextSharp.text.BaseColor.BLACK;
                //IMGISO140001.BorderWidth = 1f;
                //document.Add(IMGISO140001);

                //iTextSharp.text.Image IMGISO450001 = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/ISO45001.png"));
                //IMGISO450001.ScaleToFit(100, 100);
                //IMGISO450001.SetAbsolutePosition(595, 10);
                //IMGISO450001.Border = 0;
                //IMGISO450001.BorderColor = iTextSharp.text.BaseColor.BLACK;
                //IMGISO450001.BorderWidth = 1f;
                //document.Add(IMGISO450001);




                document.Close();
                writer.Close();
                Response.ContentType = "pdf/application";
                Response.AddHeader("content-disposition",
                "attachment;filename=" + "siparis_teyit_formu" + DateTime.Now.ToString("dd/MM/yyyy") + ".pdf");
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);

            }

        }

        protected void onay1_CheckedChanged(object sender, EventArgs e)
        {
            if (txt_siparisno_kart.Text.Trim()=="")
            {
                onay1.Checked=usr_name.onay;
                Response.Write("<script lang='JavaScript'>alert('Sipariş onayı vermek için sipariş seçiniz. Kayıt yapılmadı.');</script>");
                return;
            }

            if (usr_name.username_role!="ADMIN")
            {
                onay1.Checked=usr_name.onay;
                Response.Write("<script lang='JavaScript'>alert('Müşteri onay durumunu güncellemek için yetkili değilsiniz. Kayıt yapılmadı.');</script>");
                return;
            }
            else
            {
                string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    string durum_1 = "";
                    string durum_2 = drp_siparis_durum.SelectedValue.Trim();
                    if (onay1.Checked)
                    {
                        durum_1="E";
                        if (drp_siparis_durum.SelectedValue.Trim()=="0")
                        {
                            //onay bekliyor konumundaki sipariş onay işlemi ile planlama poziyonuna alınıyor.
                            durum_2="1";
                            drp_siparis_durum.ClearSelection();
                            drp_siparis_durum.Items.FindByValue("1").Selected = true;
                        }
                    }
                    string query = " UPDATE SIPARISE SET ONAY='"+durum_1.Trim()+"',DURUM='"+durum_2.Trim()+"',ONAY_USER='"+usr_name.username.Trim()+"',ONAY_DATE='"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"'  WHERE SIPNO='"+txt_siparisno_kart.Text.Trim()+"'  ";
                    string query_1 = " UPDATE SIPARIST SET DURUM='"+durum_2.Trim()+"'  WHERE SIPNO='"+txt_siparisno_kart.Text.Trim()+"'  ";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlCommand cmd_1 = new SqlCommand(query_1, con);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        cmd_1.ExecuteNonQuery();
                        lbl_onay_info_guncelle();
                    }
                    catch (Exception)
                    {
                        onay1.Checked=usr_name.onay;
                        Response.Write("<script lang='JavaScript'>alert('Müşteri onay durumunu güncellenemedi lütfen sistem yöneticiniz ile iletişime geçiniz. Kayıt yapılmadı !!!.');</script>");
                        throw;
                    }
                    con.Close();
                }

            }

        }

        private void lbl_onay_info_guncelle()
        {
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string query = " SELECT CONCAT(ONAY_USER,'-', FORMAT (ONAY_DATE,'yyyy-MM-dd HH:mm:ss')) AS DURUM  FROM SIPARISE    WHERE TRIM(SIPNO)='"+txt_siparisno_kart.Text.Trim()+"' ";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    lbl_onay_info.Text= dr["DURUM"].ToString().Trim();
                }
                else
                {
                    lbl_onay_info.Text= "";
                }
                dr.Close();
                con.Close();
            }
        }

        private void lbl_onay_info_guncelle2()
        {
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string query = " SELECT CONCAT(D_ONAY_USER,'-', FORMAT (D_ONAY_DATE,'yyyy-MM-dd HH:mm:ss')) AS DURUM  FROM SIPARISE    WHERE TRIM(SIPNO)='"+txt_siparisno_kart.Text.Trim()+"' ";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    lbl_durum_onay.Text= dr["DURUM"].ToString().Trim();
                }
                else
                {
                    lbl_durum_onay.Text= "";
                }
                dr.Close();
                con.Close();
            }
        }

        protected void siparis_dosya_upload_Click(object sender, EventArgs e)
        {
            //sipariş oluşmuş mu durum kontrolü yapıyor.

            if (txt_siparisno_kart.Text.Trim() == "")
            {
                Response.Write("<script lang='JavaScript'>alert('Dosya yükleme yapmak için önce sipariş seçiniz.Kayıt yapılmadı. ');</script>");
                return;
            }

            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string query = " SELECT *  FROM SIPARISE  WHERE TRIM(SIPNO)='"+txt_siparisno_kart.Text.Trim()+"' ";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (!dr.HasRows)
                {
                    Response.Write("<script lang='JavaScript'>alert('Sipariş kayıt işlemi bittikten sonra döküman yükleme işini yapınız.Kayıt yapılmadı.');</script>");
                    return;
                }
                dr.Close();
                con.Close();
            }

            //sipariş no adı ile açılan dosya bilgisi kontrolü yapılıyor.
            string path_1 = @"\\192.168.1.4\dokumanlar\SIPARISLER\" +txt_siparisno_kart.Text.Trim();
            if (!Directory.Exists(path_1))
            {
                Directory.CreateDirectory(path_1);
            }

            //eklene dosya server üzerinde dokumlar klasörü içine kayıt ediliyor.
            string path4 = "";
            if (stok_file_upload.HasFile == true)
            {
                if (stok_file_upload.PostedFile.ContentLength < 17485760)
                {
                    try
                    {
                        path4 = @"\\192.168.1.4\dokumanlar\SIPARISLER\"+txt_siparisno_kart.Text.Trim()+ @"\";
                        string extension = System.IO.Path.GetExtension(stok_file_upload.PostedFile.FileName);
                        //string newname = txt_carikod_kart.Text.Trim() + "---" + DateTime.Now.ToString("HHmmss");
                        string dosya_descr = System.IO.Path.GetFileName(stok_file_upload.PostedFile.FileName);
                        //YENIAD4 = newname + extension;


                        if (System.IO.File.Exists(path4 + dosya_descr + extension))
                        {
                            Response.Write("<script lang='JavaScript'>alert('Arşivde bu dosya adı ile kayıtlı bir dosya mevcut dosyanızın ismini değiştirin lütfen.');</script>");
                        }
                        else
                        {
                            stok_file_upload.SaveAs(path4 + dosya_descr + extension);
                        }
                    }
                    catch (Exception ex)
                    {
                        log_at_av(ex.ToString().Trim());
                        Response.Write("<script lang='JavaScript'>alert('Satış dökümanı yükleme anında hata oluşu. Dosya yüklenemedi !!1');</script>");
                        return;
                    }
                }
                else
                {
                    Response.Write("<script lang='JavaScript'>alert('Eklemek istediginiz dosya boyutu max. 15 mb olmalıdır.');</script>");
                    return;

                }

            }
            else
            {
                Response.Write("<script lang='JavaScript'>alert('Dosya eklemek için öncelikle dosya seçiniz.');</script>");
                return;
            }
            sip_dokuman_bindign(txt_siparisno_kart.Text.Trim());
        }

        protected void dokuman_open_av(object sender, EventArgs e)
        {
            GridViewRow row = grd_siparis_dokumanlar.SelectedRow;
            String filePath = @"\\192.168.1.4\dokumanlar\SIPARISLER\"+txt_siparisno_kart.Text.Trim()+@"\" + row.Cells[1].Text.Trim();
            System.IO.FileInfo file = new System.IO.FileInfo(filePath);
            if (file.Exists)
            {
                string dosyaAdi = filePath.Trim();
                //string dosyaAdi = @"\\192.168.1.4/dokumanlar/CARI_KARTLAR/""/depar_logo.png.png";
                FileInfo dosya = new FileInfo(dosyaAdi);
                System.IO.FileInfo ff = new System.IO.FileInfo(dosyaAdi);
                string DosyaUzantisi = ff.Extension;
                Response.Clear(); // Her ihtimale karşı Buffer' da kalmış herhangibir veri var ise bunu silmek için yapıyoruz.
                Response.AddHeader("Content-Disposition", "attachment; filename=" + dosyaAdi+"."+DosyaUzantisi); // Bu şekilde tarayıcı penceresinden hangi dosyanın indirileceği belirtilir. Eğer belirtilmesse bulunulan sayfanın kendisi indirilir. Okunaklı bir formattada olmaz.
                Response.AddHeader("Content-Length", dosya.Length.ToString()); // İndirilecek dosyanın uzunluğu bildirilir.
                Response.ContentType = "application/octet-stream"; // İçerik tipi belirtilir. Buna göre dosyalar binary formatta indirilirler.
                Response.WriteFile(dosyaAdi); // Dosya indirme işlemi başlar.
                Response.End(); // Süreç bu noktada sonlandırılır. Bir başka deyişle bu satırdan sonraki satırlar işletilmez hatta global.asax dosyasında eğer yazılmışsa Application_EndRequest metodu çağırılır.
            }

        }

        protected void revizyon_bul_av(object sender, EventArgs e)
        {
            if (drp_siparis_malz_kart.SelectedValue.Trim()!="" && drp_siparis_malz_kart.SelectedValue.Trim()!="152 00 01 0001" && drp_siparis_malz_kart.SelectedValue.Trim()!="152 00 02 0001")
            {
                string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    string query2 = " SELECT A.REVIZYON,A.KOD, "+
                                  " CASE WHEN A.REVIZYON='R' THEN 'S' ELSE CONCAT(TRIM(A.REVIZYON),'-',TRIM(A.ACIKLAMA)) END REV_1 FROM STOK_KART_REV A "+
                                  " LEFT OUTER JOIN STOK_KART B ON(TRIM(A.KOD)= TRIM(B.KOD)) "+
                                  " WHERE TRIM(B.KOD)='"+drp_siparis_malz_kart.SelectedValue.Trim()+"' ";
                    SqlCommand cmd2 = new SqlCommand(query2, con);
                    SqlDataAdapter adp2 = new SqlDataAdapter(cmd2);
                    DataSet ds2 = new DataSet();
                    adp2.Fill(ds2);
                    drp_malzeme_revizyon.Items.Clear();
                    drp_malzeme_revizyon.DataTextField = "REV_1";
                    drp_malzeme_revizyon.DataValueField = "REVIZYON";
                    drp_malzeme_revizyon.DataSource = ds2;
                    drp_malzeme_revizyon.DataBind();
                    con.Close();
                }
                using (SqlConnection con2 = new SqlConnection(constr))
                {
                    con2.Open();
                    string query = " SELECT B.GK_4,D.GRUPKODU_AD AS MARKA,E.GRUPKODU_AD AS MODEL, CASE WHEN TRIM(B.KOD_AD)='' THEN  C.GRUPKODU_AD  ELSE B.KOD_AD END  STOK_AD FROM STOK_KART_REV A "+
                                  " LEFT OUTER JOIN STOK_KART B ON(TRIM(A.KOD)= TRIM(B.KOD)) "+
                                  " LEFT OUTER JOIN GRUPKODU C ON(TRIM(B.GK_4)= TRIM(C.GRUPKODU) AND TRIM(C.KOD)='GK_4') "+
                                  " LEFT OUTER JOIN GRUPKODU D ON(TRIM(B.GK_5)= TRIM(D.GRUPKODU) AND TRIM(D.KOD)='GK_5') "+
                                  " LEFT OUTER JOIN GRUPKODU E ON(TRIM(B.GK_6)= TRIM(E.GRUPKODU) AND TRIM(E.KOD)='GK_6') "+
                                  " WHERE TRIM(B.KOD)='"+drp_siparis_malz_kart.SelectedValue.Trim()+"' ";
                    SqlCommand cmd = new SqlCommand(query, con2);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        txt_siparismalzemeadi_kart.Text=dr["MARKA"].ToString().Trim()+" / "+dr["MODEL"].ToString().Trim()+" / "+dr["STOK_AD"].ToString().Trim();
                    }
                    con2.Close();
                }
            }
            else
            {
                drp_malzeme_revizyon.ClearSelection();
                drp_malzeme_revizyon.Items.FindByText("R").Selected = true;
            }

        }

        protected void yetkili_bul_av(object sender, EventArgs e)
        {
            if (drp_carikodu_kart.SelectedItem.Text.Trim()=="Cari Seçiniz")
            {
                return;
            }
            else
            {
                if (drp_carikodu_kart.SelectedValue.Trim().Substring(0, 6)=="120 02")
                {
                    txt_odeme.Text="Payment:30 % in advance, the rest before shipping";
                    txt_siparisnotlar_kart.Text=HttpUtility.HtmlDecode("Con Rod and Main Bearings manufacturing unit price offer for the **** machine is presented below for your information");
                }
            }
            yetki_bul_av_2();

        }

        private void yetki_bul_av_2()
        {
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(" select TRIM(YETKILI) AS YETKILI,CONCAT(TRIM(YETKILI),'-',TRIM(UNVAN)) AS YETKILI_1 from CARI_YETKILI WHERE TRIM(CARI_KOD)='"+drp_carikodu_kart.SelectedValue.Trim()+"'  ", con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                drp_must_yetkili.Items.Clear();
                drp_must_yetkili.Items.Add("Yetkili seçiniz");
                drp_must_yetkili.DataTextField = "YETKILI_1";
                drp_must_yetkili.DataValueField = "YETKILI";
                drp_must_yetkili.DataSource = ds;
                drp_must_yetkili.DataBind();
                con.Close();
            }
        }

        protected void grd_siparis_detay_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string item = e.Row.Cells[0].Text;
                foreach (Button button in e.Row.Cells[1].Controls.OfType<Button>())
                {
                    if (button.CommandName == "Delete")
                    {
                        button.Attributes["onclick"] = "if(!confirm('Sipariş satırını silmek istiyor musunuz? ')){ return false; };";
                    }
                }
            }
        }

        protected void cidar_hesapla_av(object sender, EventArgs e)
        {
            foreach (GridViewRow gvrow in grd_siparis_detay.Rows)
            {
                string KIRANK_MIN = ((TextBox)gvrow.FindControl("TXTK_MIN")).Text.ToString();
                string KIRANK_MAX = ((TextBox)gvrow.FindControl("TXTK_MAX")).Text.ToString();
                string YUVA_MIN = ((TextBox)gvrow.FindControl("TXTYU_MIN")).Text.ToString();
                string YUVA_MAX = ((TextBox)gvrow.FindControl("TXTYU_MAX")).Text.ToString();
                string YAG_MIN = ((TextBox)gvrow.FindControl("TXTYAG_MIN")).Text.ToString();
                string YAG_MAX = ((TextBox)gvrow.FindControl("TXTYAG_MAX")).Text.ToString();

                if (KIRANK_MAX.Trim()=="0"|| YUVA_MIN.Trim()=="0" || YAG_MIN.Trim()=="0")
                {
                    return;
                }
                else
                {
                    decimal CIDAR_MIN = (Convert.ToDecimal(YUVA_MIN.Trim())-Convert.ToDecimal(KIRANK_MAX.Trim())-Convert.ToDecimal(YAG_MAX.Trim())) / 2;
                    decimal CIDAR_MAX = (Convert.ToDecimal(YUVA_MAX.Trim())-Convert.ToDecimal(KIRANK_MIN.Trim())-Convert.ToDecimal(YAG_MIN.Trim())) / 2;
                    gvrow.Cells[18].Text = Convert.ToString(CIDAR_MIN);
                    gvrow.Cells[19].Text= Convert.ToString(CIDAR_MAX);

                }

            }


        }

        protected void grd_siparis_detay_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void update_av(object sender, EventArgs e)
        {

            if (txt_siparisno_kart.Text.Trim()=="")
            {
                Response.Write("<script lang='JavaScript'>alert('Sipariş no seçilmeden güncelleme yapılamaz. Kayıt yapılmadı.');</script>");
                return;
            }
            if (txt_uretim_notlar.Text.Trim().Length>=400)
            {
                Response.Write("<script lang='JavaScript'>alert('Üretim için girilen notlar mak. 400 karakter olmalıdır. KAYIT YAPILMADI.');</script>");
                return;
            }
            if (txt_siparisnotlar_kart.Text.Trim().Length>=1000)
            {
                Response.Write("<script lang='JavaScript'>alert('Sipariş için girilen notlar mak. 1000 karakter olmalıdır. KAYIT YAPILMADI.');</script>");
                return;
            }

            int erhan = 0;
            string banka1 = "";
            string banka2 = "";
            string banka3 = "";
            foreach (System.Web.UI.WebControls.ListItem item in lstbanka.Items)
            {
                if (item.Selected)
                {
                    erhan += 1;
                    if (erhan==1)
                    {
                        banka1= item.Value.Trim();
                    }
                    else if (erhan==2)
                    {
                        banka2= item.Value.Trim();
                    }
                    else if (erhan==2)
                    {
                        banka3= item.Value.Trim();
                    }
                }
            }
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con2 = new SqlConnection(constr))
            {
                con2.Open();
                string query = " UPDATE SIPARISE  SET CH_KODU='"+drp_carikodu_kart.SelectedValue.Trim()+"',CH_ADI='"+drp_carikodu_kart.SelectedItem.Text.Trim()+"', " +
                               " TARIH='"+datepicker1.Value+"',VADE='"+txt_vade.Text.Trim()+"'," +
                               " ODEME='"+txt_odeme.Text.Trim()+"',NOTLAR='"+txt_siparisnotlar_kart.Text.Trim()+"' "+
                               ",MUSTERI_TEMS='"+drp_must_yetkili.SelectedValue.Trim()+"'" +
                               ",BANKA='"+banka1.Trim()+"',BANKA2='"+banka2.Trim()+"',BANKA3='"+banka3.Trim()+"' " +
                               ",REV_USER='"+usr_name.username.Trim()+"',REV_DATE='"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"', " +
                               " URETIM_NOTLAR='"+txt_uretim_notlar.Text.Trim()+"'"+
                               " WHERE TRIM(SIPNO)='"+txt_siparisno_kart.Text.Trim()+"' ";
                SqlCommand cmd2 = new SqlCommand(query, con2);
                try
                {
                    cmd2.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log_at_av(ex.ToString().Trim());
                    Response.Write("<script lang='JavaScript'>alert('Database baglantısında bir sorun yaşandı lütfen sistem yöneticiniz ile iletişime geçiniz.');</script>");
                }

                //SİPARİŞE AİT OLAN KDV ORANI BUTUNS SATILARDA UPDATE EDİLİR.
                string query2 = " UPDATE SIPARIST SET KDV='"+drp_kdv.SelectedValue.Trim()+"'  WHERE TRIM(SIPNO)='"+txt_siparisno_kart.Text.Trim()+"' ";
                SqlCommand cmd2_1 = new SqlCommand(query2, con2);
                try
                {
                    cmd2_1.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log_at_av(ex.ToString().Trim());
                    Response.Write("<script lang='JavaScript'>alert('Database baglantısında bir sorun yaşandı lütfen sistem yöneticiniz ile iletişime geçiniz.');</script>");
                }


                string deger_2 = "0";
                string deger_3 = "0";
                string deger_4 = "0";
                string deger_5 = "0";
                string deger_6 = "0";
                string deger_7 = "0";
                string deger_8 = "0";
                string deger_9 = "0";
                //SİPARİŞ GÜNCELLEME ANINDA EGER SATIR SATIYI 0 ALTINDAYDA GÜNCELELME YAPMAYACAK.
                //if (grd_siparis_detay.Rows.Count==0)
                //{
                //    return;
                //}

                foreach (GridViewRow gvrow in grd_siparis_detay.Rows)
                {
                    string sırano_txt = gvrow.Cells[2].Text.Trim();

                    string miktar_2 = ((TextBox)gvrow.FindControl("TXTK_MIN")).Text.ToString();
                    deger_2 = miktar_2.ToString().Trim();

                    string miktar_3 = ((TextBox)gvrow.FindControl("TXTK_MAX")).Text.ToString();
                    deger_3 = miktar_3.ToString().Trim();

                    string miktar_4 = ((TextBox)gvrow.FindControl("TXTYU_MIN")).Text.ToString();
                    deger_4 = miktar_4.ToString().Trim();

                    string miktar_5 = ((TextBox)gvrow.FindControl("TXTYU_MAX")).Text.ToString();
                    deger_5 = miktar_5.ToString().Trim();

                    string miktar_6 = ((TextBox)gvrow.FindControl("TXTYAG_MIN")).Text.ToString();
                    deger_6 = miktar_6.ToString().Trim();

                    string miktar_7 = ((TextBox)gvrow.FindControl("TXTYAG_MAX")).Text.ToString();
                    deger_7 = miktar_7.ToString().Trim();

                    string miktar_8 = ((TextBox)gvrow.FindControl("TXTCI_MIN")).Text.ToString();
                    //string miktar_8 = ((HtmlInputControl)gvrow.FindControl("TXTCI_MIN")).Value;
                    //string miktar_8 = ((HtmlInputText)gvrow.FindControl("TXTCI_MIN")).Value;
                    //string miktar_8 = gvrow.Cells[17].Text.Trim();
                    //string lblname = ((TextBox)gvrow.FindControl("TXTCI_MIN")).Text.ToString();

                    if (miktar_8.Trim()!="")
                    {
                        deger_8 = miktar_8.ToString().Trim();
                    }
                    string miktar_9 = ((TextBox)gvrow.FindControl("TXTCI_MAX")).Text.ToString();
                    //string miktar_9 = ((HtmlInputControl)gvrow.FindControl("TXTCI_MAX")).Value;
                    //string miktar_9 = ((HtmlInputText)gvrow.FindControl("TXTCI_MAX")).Value;
                    //string miktar_9 = gvrow.Cells[18].Text.Trim();
                    if (miktar_9.Trim()!="")
                    {
                        deger_9 = miktar_9.ToString().Trim();
                    }
                    //if (Convert.ToDecimal(deger_8.ToString().Trim())<=0 || Convert.ToDecimal(deger_9.ToString().Trim())<=0)
                    //{
                    //    Response.Write("<script lang='JavaScript'>alert('Cidar ölçüsü 0(sıfır) veya negatif olamaz. Kayıt yapılmadı.');</script>");
                    //    return;
                    //}


                    SqlCommand cmd4 = new SqlCommand(" UPDATE SIPARIST SET REV_USER='"+usr_name.username.Trim()+"',REV_DATE='"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"'" +
                                                     " ,K_MIN='"+ deger_2.ToString().Replace(',', '.') + "',K_MAX='" + deger_3.ToString().Replace(',', '.') + "' " +
                                                     " ,YU_MIN='"+ deger_4.ToString().Replace(',', '.') + "',YU_MAX='" + deger_5.ToString().Replace(',', '.') + "' " +
                                                     " ,YAG_MIN='"+ deger_6.ToString().Replace(',', '.') + "',YAG_MAX='" + deger_7.ToString().Replace(',', '.') + "' " +
                                                     " ,CI_MIN='"+ deger_8.ToString().Replace(',', '.') + "',CI_MAX='" + deger_9.ToString().Replace(',', '.') + "' " +
                                                     " WHERE ID='" + sırano_txt.Trim() + "' ", con2);

                    try
                    {
                        cmd4.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        log_at_av(ex.ToString().Trim());
                        Response.Write("<script lang='JavaScript'>alert('Database baglantısında bir sorun yaşandı lütfen sistem yöneticiniz ile iletişime geçiniz.');</script>");
                    }
                }

                con2.Close();
            }

        }

        protected void sip_durum_av(object sender, EventArgs e)
        {
            if (txt_siparisno_kart.Text.Trim()=="")
            {
                drp_siparis_durum.ClearSelection();
                drp_siparis_durum.Items.FindByText(usr_name.siparis_durum.Trim()).Selected = true;
                return;
            }
            if (usr_name.username_role!="ADMIN")
            {
                drp_siparis_durum.ClearSelection();
                drp_siparis_durum.Items.FindByText(usr_name.siparis_durum.Trim()).Selected = true;
                Response.Write("<script lang='JavaScript'>alert('Sipariş durumunu güncellemek için yetkili değilsiniz. Kayıt yapılmadı.');</script>");
                return;
            }
            if (Convert.ToInt32(drp_siparis_durum.SelectedValue.Trim())>=2)
            {
                Response.Write("<script lang='JavaScript'>alert('Sipariş durumları arasında onaylandı ve planlamada statuleri haricindeki statüleri lütfen açık siparişler listesi ekranından yapınız. Kayıt yapılmadı.');</script>");
                drp_siparis_durum.ClearSelection();
                drp_siparis_durum.Items.FindByValue(usr_name.siparis_durum.Trim()).Selected = true;
                return;
            }
            else
            {
                string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    string query2 = " SELECT * FROM SIPARISE WHERE TRIM(SIPNO)='"+txt_siparisno_kart.Text.Trim()+"' ";
                    SqlCommand cmd2 = new SqlCommand(query2, con);
                    SqlDataReader dr2 = cmd2.ExecuteReader();
                    if (!dr2.HasRows)
                    {
                        drp_siparis_durum.ClearSelection();
                        drp_siparis_durum.Items.FindByText(usr_name.siparis_durum.Trim()).Selected = true;
                        Response.Write("<script lang='JavaScript'>alert('Sipariş durumu güncellemek için öncelikle siparişinizi kayıt ediniz. Kayıt yapılmadı !!!.');</script>");
                        return;
                    }
                    dr2.Close();

                    if (drp_siparis_durum.SelectedValue.Trim()!="")
                    {
                        string query = " UPDATE SIPARISE SET DURUM='"+drp_siparis_durum.SelectedValue.Trim()+"',D_ONAY_USER='"+usr_name.username.Trim()+"',D_ONAY_DATE='"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"'  WHERE SIPNO='"+txt_siparisno_kart.Text.Trim()+"'  ";
                        string query_1 = " UPDATE SIPARIST SET DURUM='"+drp_siparis_durum.SelectedValue.Trim()+"' WHERE SIPNO='"+txt_siparisno_kart.Text.Trim()+"'  ";
                        SqlCommand cmd = new SqlCommand(query, con);
                        SqlCommand cmd_1 = new SqlCommand(query_1, con);
                        try
                        {
                            cmd.ExecuteNonQuery();
                            cmd_1.ExecuteNonQuery();
                            lbl_onay_info_guncelle2();
                        }
                        catch (Exception)
                        {
                            drp_siparis_durum.ClearSelection();
                            drp_siparis_durum.Items.FindByText(usr_name.siparis_durum.Trim()).Selected = true;
                            Response.Write("<script lang='JavaScript'>alert('Sipariş durumu güncellenemedi lütfen sistem yöneticiniz ile iletişime geçiniz. Kayıt yapılmadı !!!.');</script>");
                            throw;
                        }
                    }
                    con.Close();
                }

            }

        }

        private void AddTotalRow2(string labelText, string value, string labeltext2, string value2, string value3)
        {
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal);
            row.BackColor = ColorTranslator.FromHtml("#F9F9F9");
            row.Cells.AddRange(new TableCell[10] { new TableCell(), new TableCell(), new TableCell(), new TableCell(), new TableCell { Text = labelText, HorizontalAlign = HorizontalAlign.Left }, new TableCell { Text = value, HorizontalAlign = HorizontalAlign.Left }, new TableCell { Text = labeltext2, HorizontalAlign = HorizontalAlign.Left }, new TableCell { Text = value2, HorizontalAlign = HorizontalAlign.Left }, new TableCell { Text = value3, HorizontalAlign = HorizontalAlign.Left }, new TableCell() });
            grd_siparis_listesi_2.Controls[0].Controls.Add(row);
        }

        protected void grd_siparis_listesi_RowDataBound_2(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                decimal total_odeme = Convert.ToDecimal(e.Row.Cells[7].Text.ToString()) +Convert.ToDecimal(e.Row.Cells[9].Text.ToString());
                decimal sip_bakiye = Convert.ToDecimal(e.Row.Cells[5].Text.ToString());

                if (total_odeme>=sip_bakiye)
                {
                    e.Row.Attributes.CssStyle.Value = "background-color: skyblue; color: black";
                }
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ChangeForm(9)", true);
        }

        protected void odeme_av(object sender, EventArgs e)
        {
            if (txt_siparisno_kart.Text.Trim()=="")
            {
                Response.Write("<script lang='JavaScript'>alert('Ödeme girişi yapmak için öncelikle sipariş seçiniz !!!.');</script>");
                return;
            }
            txt_cariadi.Text=drp_carikodu_kart.SelectedItem.Text.Trim();
            txt_carikod.Text=drp_carikodu_kart.SelectedValue.Trim();
            txt_order_no.Text= txt_siparisno_kart.Text.Trim();
            txt_tutar.Text= "";
            datepicker11.Value="";
            drp_para_birimi2.ClearSelection();
            drp_para_birimi2.Items.FindByText("Para Birim Seçiniz").Selected = true;
            ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openModal();", true);
        }

        protected void odeme_av_2(object sender, EventArgs e)
        {

            if (SayiMi(txt_tutar.Text.Trim()) == false)
            {
                Response.Write("<script lang='JavaScript'>alert('Tutar alanına sayısal bir değer yazınız. KAYIT YAPILMADI.');</script>");
                return;
            }
            if (drp_para_birimi2.SelectedItem.Text.Trim()=="Para Birim Seçiniz")
            {
                Response.Write("<script lang='JavaScript'>alert('Ödeme girişi için para birimi seçiniz. KAYIT YAPILMADI.');</script>");
                return;
            }
            if (datepicker11.Value=="")
            {
                Response.Write("<script lang='JavaScript'>alert('Ödeme girişi için tarih seçiniz. KAYIT YAPILMADI.');</script>");
                return;
            }
            if (txt_odeme_not.Text.Trim().Length>=150)
            {
                Response.Write("<script lang='JavaScript'>alert('Ödeme notları alanına 150 karaktereden uzun bilgi yazılamaz. KAYIT YAPILMADI.');</script>");
                return;
            }
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "HidePopup", "$('#myModal').modal('hide')", true);

            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con2 = new SqlConnection(constr))
            {
                con2.Open();
                SqlCommand cmd1 = new SqlCommand(" INSERT INTO CARI_ODEME (CARI_KOD,CARI_AD,TARIH,SIP_NO,TUTAR,PARA_BIRIMI,ODEME_NOT,TUTAR_ISK)  " +
                                                            " values(@CARI_KOD,@CARI_AD,@TARIH,@SIP_NO,@TUTAR,@PARA_BIRIMI,@ODEME_NOT,@TUTAR_ISK) ", con2);
                cmd1.Parameters.AddWithValue("@CARI_KOD", txt_carikod.Text.Trim());
                cmd1.Parameters.AddWithValue("@CARI_AD", txt_cariadi.Text.Trim());
                string iDate_11 = Request.Form["ctl00$ContentPlaceHolder1$datepicker11"];
                DateTime oDate_11 = DateTime.Parse(iDate_11);
                cmd1.Parameters.AddWithValue("@TARIH", oDate_11);
                cmd1.Parameters.AddWithValue("@SIP_NO", txt_order_no.Text.Trim());
                cmd1.Parameters.AddWithValue("@ODEME_NOT", txt_odeme_not.Text.Trim());
                cmd1.Parameters.AddWithValue("@TUTAR", Convert.ToDecimal(txt_tutar.Text.Trim()));
                cmd1.Parameters.AddWithValue("@TUTAR_ISK", Convert.ToDecimal(txt_tutar2.Text.Trim()));
                cmd1.Parameters.AddWithValue("@PARA_BIRIMI", drp_para_birimi2.SelectedItem.Text.Trim());

                try
                {
                    cmd1.ExecuteNonQuery();
                    Response.Write("<script lang='JavaScript'>alert('Ödemeniz başarılı şekilde kayıt edildi.');</script>");
                }
                catch (Exception ex)
                {
                    log_at_av(ex.ToString().Trim());
                    Response.Write("<script lang='JavaScript'>alert('Database baglantısında bir sorun yaşandı lütfen sistem yöneticiniz ile iletişime geçiniz.');</script>");
                }
            }

        }

        protected void listele_av_2(object sender, EventArgs e)
        {
            listele2_bindign();
        }

        private void listele2_bindign()
        {
            string query = "";
            string constr2 = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con2 = new SqlConnection(constr2))
            {
                con2.Open();
                query = " SELECT B.TARIH,A.SIPNO,A.PARA_BIRIMI,B.CH_KODU,B.CH_KODU,B.CH_ADI,SUM(A.TOPLAM_TUTAR) AS ALACAK_TUTAR," +
                        " ISNULL((SELECT SUM(TUTAR) FROM CARI_ODEME C WHERE TRIM(C.CARI_KOD)=TRIM(B.CH_KODU) AND TRIM(A.SIPNO)=TRIM(C.SIP_NO)),0) AS ALINAN_ODEME," +
                        " SUM(A.TOPLAM_TUTAR)-ISNULL((select SUM(TUTAR) from CARI_ODEME C  WHERE TRIM(C.SIP_NO)=TRIM(A.SIPNO)),0)- ISNULL((select SUM(TUTAR_ISK) from CARI_ODEME D    WHERE TRIM(D.SIP_NO)=TRIM(A.SIPNO)),0) AS BAKIYE, " +
                        " ISNULL((select SUM(TUTAR_ISK) from CARI_ODEME D    WHERE TRIM(D.SIP_NO)=TRIM(A.SIPNO)),0) AS ISKONTO,'ekstre' AS EKSTRE "+
                        " FROM SIPARIST A "+
                        " LEFT OUTER JOIN SIPARISE B ON(TRIM(A.SIPNO)=TRIM(B.SIPNO)) "+
                        "  WHERE 1=1 ";
                if (txt_siparino_liste_2.Text.Trim() != "")
                {
                    query = query + "  and  A.SIPNO LIKE '%" + txt_siparino_liste_2.Text.Trim() + "%'";
                }
                if (drp_cari_liste_2.SelectedItem.Text.Trim()!="Cari Seçiniz")
                {
                    query = query + "  and  B.CH_KODU='" + drp_cari_liste_2.SelectedValue.Trim() + "'";
                }
                //if (drp_sip_durum_liste_2.SelectedItem.Text.Trim() != "Hepsi")
                //{
                //    query = query + "  and  A.DURUM = '" + drp_sip_durum_liste_2.SelectedValue.Trim() + "'";
                //}
                if (datepicker12.Value!="Başlangıç Tarihi")
                {
                    if (datepicker12.Value!="")
                    {
                        query = query + "  and  CAST(A.TARIH  as datetime)>=cast('"+datepicker12.Value.Trim()+"' as datetime) ";
                    }
                }
                if (datepicker13.Value!="Bitiş Tarihi")
                {
                    if (datepicker13.Value!="")
                    {
                        query = query + "  and  CAST(A.TARIH  as datetime)<=cast('"+datepicker13.Value.Trim()+"' as datetime) ";
                    }
                }

                query = query + " GROUP BY B.TARIH,A.SIPNO,A.PARA_BIRIMI,B.CH_KODU,B.CH_ADI ";
                SqlCommand cmd_1 = new SqlCommand(query, con2);
                SqlDataAdapter adp2 = new SqlDataAdapter(cmd_1);
                DataSet ds2 = new DataSet();
                adp2.Fill(ds2);
                grd_siparis_listesi_2.DataSource = ds2;
                grd_siparis_listesi_2.DataBind();
                con2.Close();

                decimal total_usd_tutar = 0;
                decimal total_usd_tutar_2 = 0;
                decimal usb_bakiye = 0;
                decimal total_eur_tutar = 0;
                decimal total_eur_tutar_2 = 0;
                decimal eur_bakiye = 0;
                decimal total_tl_tutar = 0;
                decimal total_tl_tutar_2 = 0;
                decimal tl_bakiye = 0;
                decimal total_gbp_tutar = 0;
                decimal total_gbp_tutar_2 = 0;
                decimal gbp_bakiye = 0;

                foreach (GridViewRow gvrow in grd_siparis_listesi_2.Rows)
                {
                    if (gvrow.Cells[6].Text.Trim()=="USD")
                    {
                        total_usd_tutar =total_usd_tutar + Convert.ToDecimal(gvrow.Cells[5].Text.Trim());
                        total_usd_tutar_2 =total_usd_tutar_2 + Convert.ToDecimal(gvrow.Cells[7].Text.Trim());
                        usb_bakiye=total_usd_tutar-total_usd_tutar_2;
                    }
                    else if (gvrow.Cells[6].Text.Trim()=="EUR")
                    {
                        total_eur_tutar =total_eur_tutar + Convert.ToDecimal(gvrow.Cells[5].Text.Trim());
                        total_eur_tutar_2 =total_eur_tutar_2 + Convert.ToDecimal(gvrow.Cells[7].Text.Trim());
                        eur_bakiye=total_eur_tutar-total_eur_tutar_2;
                    }
                    else if (gvrow.Cells[6].Text.Trim()=="GBP")
                    {
                        total_gbp_tutar =total_gbp_tutar + Convert.ToDecimal(gvrow.Cells[5].Text.Trim());
                        total_gbp_tutar_2 =total_gbp_tutar_2 + Convert.ToDecimal(gvrow.Cells[7].Text.Trim());
                        tl_bakiye=total_gbp_tutar-total_gbp_tutar_2;
                    }
                    else if (gvrow.Cells[6].Text.Trim()=="TL")
                    {
                        total_tl_tutar =total_tl_tutar + Convert.ToDecimal(gvrow.Cells[5].Text.Trim());
                        total_tl_tutar_2 =total_tl_tutar_2 + Convert.ToDecimal(gvrow.Cells[7].Text.Trim());
                        gbp_bakiye=total_tl_tutar-total_tl_tutar_2;
                    }
                }
                if (total_usd_tutar>0)
                {
                    this.AddTotalRow2("Toplam Alacak USD", total_usd_tutar.ToString("N2"), "Toplam Ödenen USD", total_usd_tutar_2.ToString("N2"), usb_bakiye.ToString("N2"));
                }
                if (total_eur_tutar>0)
                {
                    this.AddTotalRow2("Toplam Alacak EUR", total_eur_tutar.ToString("N2"), "Toplam Ödenen EUR", total_usd_tutar_2.ToString("N2"), eur_bakiye.ToString("N2"));
                }
                if (total_tl_tutar>0)
                {
                    this.AddTotalRow2("Toplam Alacak TL", total_tl_tutar.ToString("N2"), "Toplam Ödenen TL", total_tl_tutar_2.ToString("N2"), tl_bakiye.ToString("N2"));
                }
                if (total_gbp_tutar>0)
                {
                    this.AddTotalRow2("Toplam Alacak GBP", total_gbp_tutar.ToString("N2"), "Toplam Ödenen GBP", total_gbp_tutar_2.ToString("N2"), gbp_bakiye.ToString("N2"));
                }
            }
            //sipariş liste tabında arama yapınca diger tab'a geçmemesi için çalışasn scripttir.
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ChangeForm(9)", true);
        }

        protected void Display(object sender, EventArgs e)
        {

            int rowIndex = Convert.ToInt32(((sender as LinkButton).NamingContainer as GridViewRow).RowIndex);
            GridViewRow row = grd_siparis_listesi_2.Rows[rowIndex];
            LinkButton btn = (LinkButton)grd_siparis_listesi_2.Rows[rowIndex].FindControl("lnkBtnEdit_2");
            //string SIPNO = btn.Text;
            //TextBox3.Text = row.Cells[1].Text.ToString();
            TextBox3.Text = btn.Text;
            TextBox1.Text=row.Cells[2].Text.ToString();
            TextBox2.Text=row.Cells[3].Text.ToString();
            sip_txt_tutar.Text=row.Cells[5].Text.ToString();
            TextBox5.Text="";
            TextBox6.Text="";
            TextBox4.Text="";
            ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openModal2();", true);
        }

        protected void odeme_av_3(object sender, EventArgs e)
        {
            if (SayiMi(TextBox4.Text.Trim()) == false)
            {
                Response.Write("<script lang='JavaScript'>alert('Tutar alanına sayısal bir değer yazınız. KAYIT YAPILMADI.');</script>");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "text", "ChangeForm(9)", true);
                return;
            }
            if (DropDownList1.SelectedItem.Text.Trim()=="Para Birim Seçiniz")
            {
                Response.Write("<script lang='JavaScript'>alert('Ödeme girişi için para birimi seçiniz. KAYIT YAPILMADI.');</script>");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "text", "ChangeForm(9)", true);
                return;
            }
            if (datepicker22.Value=="")
            {
                Response.Write("<script lang='JavaScript'>alert('Ödeme girişi için tarih seçiniz. KAYIT YAPILMADI.');</script>");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "text", "ChangeForm(9)", true);
                return;
            }
            if (TextBox6.Text.Trim().Length>=150)
            {
                Response.Write("<script lang='JavaScript'>alert('Ödeme notları alanına 150 karaktereden uzun bilgi yazılamaz. KAYIT YAPILMADI.');</script>");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "text", "ChangeForm(9)", true);
                return;
            }
            decimal sip_tutar_1 = Convert.ToDecimal(sip_txt_tutar.Text);
            decimal odeme_toplam = 0;
            decimal iskonto_toplam = 0;
            if (TextBox4.Text.Trim()!="")
            {
                odeme_toplam = Convert.ToDecimal(TextBox4.Text);
            }
            if (TextBox5.Text.Trim()!="")
            {
                iskonto_toplam = Convert.ToDecimal(TextBox5.Text);
            }


            if (sip_tutar_1<(odeme_toplam + iskonto_toplam))
            {
                Response.Write("<script lang='JavaScript'>alert('Ödeme yapılan miktar sipariş tutarından daha fazla olamaz. KAYIT YAPILMADI.');</script>");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "text", "ChangeForm(9)", true);
                return;
            }
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con2 = new SqlConnection(constr))
            {
                con2.Open();
                SqlCommand cmd1 = new SqlCommand(" INSERT INTO CARI_ODEME (CARI_KOD,CARI_AD,TARIH,SIP_NO,TUTAR,PARA_BIRIMI,ODEME_NOT,TUTAR_ISK)  " +
                                                            " values(@CARI_KOD,@CARI_AD,@TARIH,@SIP_NO,@TUTAR,@PARA_BIRIMI,@ODEME_NOT,@TUTAR_ISK) ", con2);
                cmd1.Parameters.AddWithValue("@CARI_KOD", TextBox1.Text.Trim());
                cmd1.Parameters.AddWithValue("@CARI_AD", TextBox2.Text.Trim());
                string iDate_11 = Request.Form["ctl00$ContentPlaceHolder1$datepicker22"];
                DateTime oDate_11 = DateTime.Parse(iDate_11);
                cmd1.Parameters.AddWithValue("@TARIH", oDate_11);
                cmd1.Parameters.AddWithValue("@SIP_NO", TextBox3.Text.Trim());
                cmd1.Parameters.AddWithValue("@ODEME_NOT", TextBox6.Text.Trim());
                cmd1.Parameters.AddWithValue("@TUTAR", Convert.ToDecimal(TextBox4.Text.Trim()));
                if (TextBox5.Text.Trim()!="")
                {
                    cmd1.Parameters.AddWithValue("@TUTAR_ISK", Convert.ToDecimal(TextBox5.Text.Trim()));
                }
                else
                {
                    cmd1.Parameters.AddWithValue("@TUTAR_ISK", "0");
                }
                cmd1.Parameters.AddWithValue("@PARA_BIRIMI", DropDownList1.SelectedItem.Text.Trim());
                try
                {
                    cmd1.ExecuteNonQuery();
                    Response.Write("<script lang='JavaScript'>alert('Ödemeniz başarılı şekilde kayıt edildi.');</script>");
                }
                catch (Exception ex)
                {
                    log_at_av(ex.ToString().Trim());
                    Response.Write("<script lang='JavaScript'>alert('Database baglantısında bir sorun yaşandı lütfen sistem yöneticiniz ile iletişime geçiniz.');</script>");
                }

            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "text", "ChangeForm(9)", true);
            listele2_bindign();
        }

        protected void grd_siparis_listesi_2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "payment_into")
            {
                HDN_ODEME.Value="E";
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = grd_siparis_listesi_2.Rows[rowIndex];
                string SIPNO = row.Cells[1].Text.ToString();
                string CH_KODU = row.Cells[2].Text.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal2()", true);
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "text", "ChangeForm(9)", true);
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Pop", "openModal2();", true);
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "text", "ChangeForm(9)", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal2()", true);

        }

        protected void SORT_AV(object sender, GridViewSortEventArgs e)
        {
            DataTable dataTable = ViewState["dtbl"] as DataTable;
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                dataView.Sort = e.SortExpression + " " + ConvertSortDirection(e.SortDirection);
                grd_siparis_listesi.DataSource = dataView;
                grd_siparis_listesi.DataBind();
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

        protected void PAGE_LISTE_AV(object sender, GridViewPageEventArgs e)
        {
            liste_binding();
            grd_siparis_listesi.PageIndex = e.NewPageIndex;
            grd_siparis_listesi.DataBind();
        }

        protected void Display_2(object sender, EventArgs e)
        {
            int rowIndex = Convert.ToInt32(((sender as LinkButton).NamingContainer as GridViewRow).RowIndex);
            GridViewRow row = grd_siparis_listesi_2.Rows[rowIndex];


            LinkButton btn = (LinkButton)grd_siparis_listesi_2.Rows[rowIndex].FindControl("lnkBtnEdit_2");
            string SIPNO = btn.Text;


            string constr2 = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con2 = new SqlConnection(constr2))
            {
                con2.Open();
                string query = "  SELECT SIPNO,ID,STOK_KOD,STOK_AD,MIKTAR,BIRIM,BIRIM_FIYAT,PARA_BIRIMI,TOPLAM_TUTAR FROM SIPARIST WHERE RTRIM(SIPNO)='"+SIPNO.Trim()+"' ORDER BY  ID DESC ";
                SqlCommand cmd_1 = new SqlCommand(query, con2);
                SqlDataAdapter adp2 = new SqlDataAdapter(cmd_1);
                DataSet ds2 = new DataSet();
                adp2.Fill(ds2);
                grd_popup_siparisler.DataSource = ds2;
                grd_popup_siparisler.DataBind();
                con2.Close();
            }

            ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openModal3();", true);
        }

        protected void EKSTRE_AV(object sender, EventArgs e)
        {
            int rowIndex = Convert.ToInt32(((sender as LinkButton).NamingContainer as GridViewRow).RowIndex);
            LinkButton btn = (LinkButton)grd_siparis_listesi_2.Rows[rowIndex].FindControl("lnkBtnEdit_2");
            string cari_kodu = grd_siparis_listesi_2.Rows[rowIndex].Cells[2].Text.Trim();
            string cari_adi = grd_siparis_listesi_2.Rows[rowIndex].Cells[3].Text.Trim();
            txt_ekstre_carikod.Text=cari_kodu.Trim();
            txt_ekstre_cariad.Text=cari_adi.Trim();
            datepicker24.Value=DateTime.Now.ToString("yyyy")+"-"+"01-01";
            datepicker25.Value=DateTime.Now.ToString("yyyy")+"-"+"12-31";
            ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openModal4();", true);
        }

        protected void ekstreal_av2(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (MemoryStream ms = new MemoryStream())
            {
                //*************************** HEADER BİLGİLERİ YAZDIRILIYOR
                //Document document = new Document(iTextSharp.text.PageSize.A4.Rotate(), 25, 25, 30, 30);
                //PdfWriter writer = PdfWriter.GetInstance(document, ms);
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/depar_logo.png"));
                img.ScaleToFit(100, 100);
                img.SetAbsolutePosition(20, 800);
                img.Border = 0;
                //img.Border = iTextSharp.text.Rectangle.BOX;
                //img.BorderColor = iTextSharp.text.BaseColor.BLACK;
                //img.BorderWidth = 5f;
                document.Add(img);

                //iTextSharp.text.Image IMGISO9001 = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/ISO9001.png"));
                //IMGISO9001.ScaleToFit(100, 100);
                //IMGISO9001.SetAbsolutePosition(620, 510);
                ////IMGISO9001.Border = iTextSharp.text.Rectangle.BOX;
                //IMGISO9001.Border = 0;
                ////IMGISO9001.BorderColor = iTextSharp.text.BaseColor.BLACK;
                ////IMGISO9001.BorderWidth = 1f;
                //document.Add(IMGISO9001);


                BaseFont bF = BaseFont.CreateFont("C:\\Windows\\Fonts\\Arial.ttf", "windows-1254", true);
                BaseFont bF_2 = BaseFont.CreateFont("C:\\Windows\\Fonts\\times.ttf", "windows-1254", true);

                PdfPTable table = new PdfPTable(3);
                table.WidthPercentage = 100;
                PdfPCell cell = new PdfPCell(new Phrase("Cari Hesap Ekstresi", new iTextSharp.text.Font(bF, 18f, iTextSharp.text.Font.BOLD)));
                cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = 0;
                table.AddCell(cell);
                document.Add(table);
                document.Add(new Paragraph(" "));

                PdfContentByte cb = writer.DirectContent;

                {
                    table = new PdfPTable(2);
                    table.WidthPercentage = 75;

                    float[] colWidthsaccing4 = { 80, 150 };
                    table.SetWidths(colWidthsaccing4);
                    table.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;

                    cell = new PdfPCell();
                    PdfPCell cell21 = new PdfPCell(new Phrase("Cari Adı:", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                    cell21.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell21.Border = 0;
                    table.AddCell(cell21);

                    PdfPCell cell22 = new PdfPCell(new Phrase(txt_ekstre_cariad.Text.Trim(), new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                    cell22.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell22.Border = 0;
                    table.AddCell(cell22);


                    PdfPCell cell23 = new PdfPCell(new Phrase("Tarih:", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                    cell23.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell23.Border = 0;
                    table.AddCell(cell23);

                    PdfPCell cell34 = new PdfPCell(new Phrase(datepicker24.Value.Trim()+"--"+datepicker25.Value.Trim(), new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                    cell34.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell34.Border = 0;
                    table.AddCell(cell34);
                    document.Add(table);
                }

                document.Add(new Paragraph(" "));

                {
                    table = new PdfPTable(6);

                    table.WidthPercentage = 95;

                    PdfPCell hucre_1 = new PdfPCell(new Phrase("Tarih:", new iTextSharp.text.Font(bF_2, 11f, iTextSharp.text.Font.NORMAL)));
                    hucre_1.HorizontalAlignment = Element.ALIGN_LEFT;
                    hucre_1.Border = 0;
                    //hucre_1.Colspan = 1;
                    table.AddCell(hucre_1);

                    PdfPCell hucre_2 = new PdfPCell(new Phrase("Evrakno:", new iTextSharp.text.Font(bF_2, 11f, iTextSharp.text.Font.NORMAL)));
                    hucre_2.HorizontalAlignment = Element.ALIGN_LEFT;
                    hucre_2.Border = 0;
                    //hucre_1.Colspan = 1;
                    table.AddCell(hucre_2);

                    PdfPCell hucre_4 = new PdfPCell(new Phrase("Alacak:", new iTextSharp.text.Font(bF_2, 11f, iTextSharp.text.Font.NORMAL)));
                    hucre_4.HorizontalAlignment = Element.ALIGN_LEFT;
                    hucre_4.Border = 0;
                    table.AddCell(hucre_4);

                    PdfPCell hucre_5 = new PdfPCell(new Phrase("Borç:", new iTextSharp.text.Font(bF_2, 11f, iTextSharp.text.Font.NORMAL)));
                    hucre_5.HorizontalAlignment = Element.ALIGN_LEFT;
                    hucre_5.Border = 0;
                    table.AddCell(hucre_5);

                    PdfPCell hucre_6 = new PdfPCell(new Phrase("Para Birimi:", new iTextSharp.text.Font(bF_2, 11f, iTextSharp.text.Font.NORMAL)));
                    hucre_6.HorizontalAlignment = Element.ALIGN_CENTER;
                    hucre_6.Border = 0;
                    table.AddCell(hucre_6);

                    PdfPCell hucre_7 = new PdfPCell(new Phrase("Bakiye:", new iTextSharp.text.Font(bF_2, 11f, iTextSharp.text.Font.NORMAL)));
                    hucre_7.HorizontalAlignment = Element.ALIGN_RIGHT;
                    hucre_7.Border = 0;
                    table.AddCell(hucre_7);
                    document.Add(table);
                }
                document.Add(new Paragraph(" "));
                LineSeparator line = new LineSeparator(1f, 100f, BaseColor.BLACK, Element.ALIGN_LEFT, 1);
                document.Add(line);

                string toplambakiye = "";
                string constr2 = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
                using (SqlConnection con2 = new SqlConnection(constr2))
                {
                    con2.Open();

                    string query = " SELECT TUR,CH_KODU,SIPNO,PARA_BIRIMI,TARIH, TOPLAM_TUTAR," +
                                    " SUM(TOPLAM_TUTAR) OVER(order by TARIH ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) as K_TOPLAM FROM( "+
                                    " SELECT 'ALACAK' AS TUR, B.CH_KODU, A.SIPNO, B.TARIH, A.TOPLAM_TUTAR, A.PARA_BIRIMI FROM SIPARIST  A "+
                                    " LEFT OUTER JOIN SIPARISE B ON(RTRIM(A.SIPNO)= RTRIM(B.SIPNO))"+
                                    " WHERE RTRIM(B.CH_KODU)='"+txt_ekstre_carikod.Text.Trim()+"' AND B.TARIH>='"+datepicker24.Value.Trim()+"' AND  B.TARIH<='"+datepicker25.Value.Trim()+"' "+
                                    " UNION ALL" +
                                    " SELECT 'BORC' AS TUR, C.CARI_KOD, C.SIP_NO, C.TARIH, (ISNULL(C.TUTAR, 0)+ISNULL(C.TUTAR_ISK, 0))*-1 AS TUTAR, C.PARA_BIRIMI " +
                                    " FROM CARI_ODEME C WHERE RTRIM(C.CARI_KOD)='"+txt_ekstre_carikod.Text.Trim()+"' "+
                                    " AND C.TARIH>='"+datepicker24.Value.Trim()+"' AND  C.TARIH<='"+datepicker25.Value.Trim()+"') EKSTRE ";

                    SqlCommand cmd_1 = new SqlCommand(query, con2);
                    SqlDataReader dr = cmd_1.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {

                            table = new PdfPTable(6);

                            table.WidthPercentage = 90;

                            PdfPCell hucre_1 = new PdfPCell(new Phrase(Convert.ToDateTime(dr["TARIH"]).ToString("yyyy-MM-dd"), new iTextSharp.text.Font(bF_2, 10f, iTextSharp.text.Font.NORMAL)));
                            hucre_1.HorizontalAlignment = Element.ALIGN_LEFT;
                            hucre_1.Border = 0;
                            //hucre_1.Colspan = 1;
                            table.AddCell(hucre_1);

                            PdfPCell hucre_2 = new PdfPCell(new Phrase(dr["SIPNO"].ToString().Trim(), new iTextSharp.text.Font(bF_2, 10f, iTextSharp.text.Font.NORMAL)));
                            hucre_2.HorizontalAlignment = Element.ALIGN_LEFT;
                            hucre_2.Border = 0;
                            //hucre_1.Colspan = 1;
                            table.AddCell(hucre_2);

                            decimal tutar_alacak = 0;
                            decimal tutar_borc = 0;

                            if (dr["TUR"].ToString().Trim()=="ALACAK")
                            {
                                tutar_alacak=Math.Round(Convert.ToDecimal(dr["TOPLAM_TUTAR"].ToString().Trim()), 2);
                            }
                            else
                            {
                                tutar_borc=Math.Round(Convert.ToDecimal(dr["TOPLAM_TUTAR"].ToString().Trim()));
                            }
                            PdfPCell hucre_4 = new PdfPCell(new Phrase(tutar_alacak.ToString().Trim(), new iTextSharp.text.Font(bF_2, 10f, iTextSharp.text.Font.NORMAL)));
                            hucre_4.HorizontalAlignment = Element.ALIGN_LEFT;
                            hucre_4.Border = 0;
                            table.AddCell(hucre_4);

                            PdfPCell hucre_5 = new PdfPCell(new Phrase(tutar_borc.ToString().Trim(), new iTextSharp.text.Font(bF_2, 10f, iTextSharp.text.Font.NORMAL)));
                            hucre_5.HorizontalAlignment = Element.ALIGN_LEFT;
                            hucre_5.Border = 0;
                            table.AddCell(hucre_5);

                            PdfPCell hucre_6 = new PdfPCell(new Phrase(dr["PARA_BIRIMI"].ToString().Trim(), new iTextSharp.text.Font(bF_2, 10f, iTextSharp.text.Font.NORMAL)));
                            hucre_6.HorizontalAlignment = Element.ALIGN_CENTER;
                            hucre_6.Border = 0;
                            table.AddCell(hucre_6);

                            decimal k_tutar = Math.Round(Convert.ToDecimal(dr["K_TOPLAM"].ToString().Trim()), 2);

                            PdfPCell hucre_7 = new PdfPCell(new Phrase(k_tutar.ToString().Trim(), new iTextSharp.text.Font(bF_2, 10f, iTextSharp.text.Font.NORMAL)));
                            hucre_7.HorizontalAlignment = Element.ALIGN_RIGHT;
                            hucre_7.Border = 0;
                            table.AddCell(hucre_7);

                            document.Add(table);

                            if (k_tutar>0)
                            {
                                toplambakiye="Toplam alacak bakiye: " +k_tutar.ToString().Trim();
                            }
                            else
                            {
                                toplambakiye="Toplam borç bakiye: "+ k_tutar.ToString().Trim();
                            }
                        }
                    }

                    con2.Close();
                }
                document.Add(new Paragraph(" "));
                document.Add(line);

                {
                    table = new PdfPTable(6);
                    table.WidthPercentage = 90;
                    PdfPCell hucre_1 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bF_2, 10f, iTextSharp.text.Font.NORMAL)));
                    hucre_1.HorizontalAlignment = Element.ALIGN_LEFT;
                    hucre_1.Border = 0;
                    table.AddCell(hucre_1);

                    PdfPCell hucre_2 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bF_2, 10f, iTextSharp.text.Font.NORMAL)));
                    hucre_2.HorizontalAlignment = Element.ALIGN_LEFT;
                    hucre_2.Border = 0;
                    table.AddCell(hucre_2);

                    PdfPCell hucre_4 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bF_2, 10f, iTextSharp.text.Font.NORMAL)));
                    hucre_4.HorizontalAlignment = Element.ALIGN_LEFT;
                    hucre_4.Border = 0;
                    table.AddCell(hucre_4);

                    PdfPCell hucre_5 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bF_2, 10f, iTextSharp.text.Font.NORMAL)));
                    hucre_5.HorizontalAlignment = Element.ALIGN_LEFT;
                    hucre_5.Border = 0;
                    table.AddCell(hucre_5);

                    PdfPCell hucre_7 = new PdfPCell(new Phrase(toplambakiye.Trim(), new iTextSharp.text.Font(bF_2, 10f, iTextSharp.text.Font.NORMAL)));
                    hucre_7.HorizontalAlignment = Element.ALIGN_RIGHT;
                    hucre_7.Border = 0;
                    hucre_7.Colspan = 2;
                    table.AddCell(hucre_7);

                    document.Add(table);
                }

                //*************************************************************************

                document.Add(new Paragraph(" "));

                document.Close();
                writer.Close();
                Response.ContentType = "pdf/application";
                Response.AddHeader("content-disposition",
                "attachment;filename=" + "Cari Hesap Ekstresi: " + txt_ekstre_cariad.Text.Trim() + ".pdf");
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);

            }

        }
    }
}