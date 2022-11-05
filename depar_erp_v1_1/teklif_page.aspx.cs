using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace depar_erp_v1_1
{
    public partial class teklif_page : System.Web.UI.Page
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

            if (usr_name.username_role.Trim()!="ADMIN")
            {
                Response.Redirect("dashboard.aspx");
            }

            if (!Page.IsPostBack)
            {
                cari_bindign();
                malzeme_kartlari_bindign();
                banka_binding();
                liste_binding_2();
                usr_name.siparis_durum=drp_teklif_durum.SelectedValue.Trim();
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

        private void malzeme_kartlari_bindign()
        {
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(" SELECT ID,TRIM(KOD) AS KOD,CONCAT(TRIM(DEPAR_KOD),'--',TRIM(KOD),'--',TRIM(KOD_AD)) AS MALZEME FROM STOK_KART WHERE TRIM(KOD) LIKE '152%' and ESKI_YENI='YENI' ORDER BY KOD ASC ", con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                drp_siparis_malz_kart.Items.Clear();
                drp_siparis_malz_kart.Items.Add("Malzeme Seçiniz");
                drp_siparis_malz_kart.Items.Add("Yeni Ürün");
                drp_siparis_malz_kart.DataTextField = "MALZEME";
                drp_siparis_malz_kart.DataValueField = "KOD";
                drp_siparis_malz_kart.DataSource = ds;
                drp_siparis_malz_kart.DataBind();

                drp_malzeme_adi.Items.Clear();
                drp_malzeme_adi.Items.Add("Malzeme Seçiniz");
                drp_malzeme_adi.Items.Add("Yeni Ürün");
                drp_malzeme_adi.DataTextField = "MALZEME";
                drp_malzeme_adi.DataValueField = "KOD";
                drp_malzeme_adi.DataSource = ds;
                drp_malzeme_adi.DataBind();
                con.Close();


            }
        }

        private void cari_bindign()
        {
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(" select ID, TRIM(CARI_AD) AS CARI_AD,TRIM(CARI_KOD) AS CARI_KOD from CARI  order by LEFT(CARI_KOD,6) ASC,CARI_AD ASC  ", con);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                drp_carikodu_kart.Items.Clear();
                drp_carikodu_kart.Items.Add("Cari Seçiniz");
                drp_carikodu_kart.DataTextField = "CARI_AD";
                drp_carikodu_kart.DataValueField = "CARI_KOD";
                drp_carikodu_kart.DataSource = ds;
                drp_carikodu_kart.DataBind();

                drp_cari_teklif_liste.Items.Clear();
                drp_cari_teklif_liste.Items.Add("Cari Seçiniz");
                drp_cari_teklif_liste.DataTextField = "CARI_AD";
                drp_cari_teklif_liste.DataValueField = "CARI_KOD";
                drp_cari_teklif_liste.DataSource = ds;
                drp_cari_teklif_liste.DataBind();
                con.Close();
            }
        }

        protected void save_av(object sender, EventArgs e)
        {
            siparis_ekranini_temilze2();
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlCommand cmd2 = new SqlCommand(" select TOP 1 EVRAKNO ,substring(EVRAKNO,10,5)+1  AS NUMBER  from TEKLIFE ORDER BY ID DESC ", con);
                SqlDataReader dr2 = cmd2.ExecuteReader();
                dr2.Read();
                if (dr2.HasRows)
                {
                    string next_number2 = dr2["NUMBER"].ToString();
                    txt_teklifno_kart.Text = "TKL" + DateTime.Now.ToString("yyyyMM").ToString() + next_number2;
                    dr2.Close();
                }
                else
                {
                    txt_teklifno_kart.Text = "TKL" + DateTime.Now.ToString("yyyyMM").ToString() + "1";
                }
                dr2.Close();
                con.Close();
            }
            txt_teklifveren_kart.Text=usr_name.username_full.Trim();
            datepicker4.Value = Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd");

            txt_vade.Text="";
            txt_ödeme.Text="%50 Peşin %50 Mal teslimi";
            string metin = "Makinaya ait yatağına ilişkin imalat birim fiyat teklifimiz aşağıda bilgilerinize sunulmuştur. Teklifimizi uygun bulacağınızı umut eder, çalışmalarınızda sağlık ve başarılar dileriz.";
            txt_teklifnotlar_kart.Text=HttpUtility.HtmlDecode(metin.Trim());
            txt_teklifteslimsekli.Text="Fabrika Teslimidir.";
            cari_bindign();
        }

        private void siparis_ekranini_temilze2()
        {
            txt_teklifno_kart.Text="";
            datepicker4.Value="";
            drp_carikodu_kart.ClearSelection();
            drp_carikodu_kart.Items.FindByText("Cari Seçiniz").Selected = true;
            txt_teklifnotlar_kart.Text="";
            txt_teklifveren_kart.Text="";
            txt_teklifteslimsekli.Text="";
            txt_ödeme.Text="";
            txt_vade.Text="";
            drp_teklif_durum.ClearSelection();
            drp_teklif_durum.Items.FindByValue("1").Selected = true;
            lbl_onay_info.Text="";
            drp_siparis_malz_kart.ClearSelection();
            drp_siparis_malz_kart.Items.FindByText("Malzeme Seçiniz").Selected = true;
            drp_malzeme_revizyon.ClearSelection();
            drp_malzeme_revizyon.Items.FindByText("Revizyon seçiniz").Selected = true;
            txt_siparismalzemeadi_kart.Text="";
            txt_siparismiktar_kart.Text="";
            drp_siparis_birim_kart.ClearSelection();
            drp_siparis_birim_kart.Items.FindByValue("5").Selected = true;
            txt_birimfiyat_kart.Text="";
            drp_parabirimi_kart.ClearSelection();
            drp_parabirimi_kart.Items.FindByText("USD").Selected = true;
            txt_siparis_satırnot_kart.Text="";
            grd_siparis_detay.DataSource = null;
            grd_siparis_detay.DataBind();
        }

        protected void delete_av(object sender, EventArgs e)
        {
            if (txt_teklifno_kart.Text.Trim()=="")
            {
                Response.Write("<script lang='JavaScript'>alert('Teklif evrakı silmek için öncelikle teklif seçiniz. Kayıt yapılmadı.');</script>");
                return;
            }

            string constr3 = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con3 = new SqlConnection(constr3))
            {
                con3.Open();
                SqlCommand cmd = new SqlCommand("Delete From TEKLIFE where TRIM(EVRAKNO)='" + txt_teklifno_kart.Text.Trim() + "'", con3);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log_at_av(ex.ToString().Trim());
                    Response.Write("<script lang='JavaScript'>alert('Database baglantısında bir sorun yaşandı lütfen sistem yöneticiniz ile iletişime geçiniz.');</script>");
                }
                cmd.Dispose();

                SqlCommand cmd2 = new SqlCommand("Delete From TEKLIFT where TRIM(EVRAKNO)='" + txt_teklifno_kart.Text.Trim() + "'", con3);
                try
                {
                    cmd2.ExecuteNonQuery();
                    txt_teklifno_kart.Text="";
                    datepicker4.Value="";
                    drp_carikodu_kart.ClearSelection();
                    drp_carikodu_kart.Items.FindByText("Cari Seçiniz").Selected = true;
                    txt_vade.Text="";
                    txt_ödeme.Text="";
                    lstbanka.ClearSelection();
                    lstbanka.Items.FindByText("Banka Seçiniz").Selected = true;
                    txt_teklifnotlar_kart.Text="";
                    txt_teklifveren_kart.Text="";
                    drp_must_yetkili.ClearSelection();
                    drp_must_yetkili.Items.FindByText("Yetkili seçiniz").Selected = true;
                    txt_teklifteslimsekli.Text="";
                    drp_siparis_malz_kart.ClearSelection();
                    drp_siparis_malz_kart.Items.FindByText("Malzeme Seçiniz").Selected = true;
                    drp_malzeme_revizyon.ClearSelection();
                    drp_malzeme_revizyon.Items.FindByText("Revizyon seçiniz").Selected = true;
                    txt_siparismalzemeadi_kart.Text="";
                    txt_olcu.Text="";
                    txt_siparismiktar_kart.Text="";
                    drp_siparis_birim_kart.ClearSelection();
                    drp_siparis_birim_kart.Items.FindByText("Birim Seçiniz").Selected = true;
                    txt_birimfiyat_kart.Text="";
                    drp_parabirimi_kart.ClearSelection();
                    drp_parabirimi_kart.Items.FindByText("Para Birim Seçiniz").Selected = true;
                    drp_urunno.ClearSelection();
                    drp_urunno.Items.FindByText("Üretici Parça No").Selected = true;
                    txt_siparis_satırnot_kart.Text="";
                    grd_siparis_detay.DataSource = null;
                    grd_siparis_detay.DataBind();

                }
                catch (Exception ex)
                {
                    log_at_av(ex.ToString().Trim());
                    Response.Write("<script lang='JavaScript'>alert('Database baglantısında bir sorun yaşandı lütfen sistem yöneticiniz ile iletişime geçiniz.');</script>");
                }
                cmd2.Dispose();

                con3.Close();
            }

        }

        protected void print_av(object sender, EventArgs e)
        {
            if (txt_teklifno_kart.Text.Trim()=="")
            {
                Response.Write("<script lang='JavaScript'>alert('Teklif no seçilmeden teklif formu alınamaz.');</script>");
                return;
            }

            string firma_adi = "";
            string firma_kod = "";
            string teklif_tarihi = "";
            string teklif_veren = "";
            string firma_adres = "";
            string firma_tel = "";
            string firma_mail = "";
            string teklif_notlar = "";
            string odeme = "";
            string teslim_sekli = "";
            string musteri_tems = "";


            string banka = "";
            string iban = "";
            string hesap_ad = "";

            string banka2 = "";
            string iban2 = "";
            string hesap_ad2 = "";

            string banka3 = "";
            string iban3 = "";
            string hesap_ad3 = "";
            string bank1 = "";
            string bank2 = "";
            string bank3 = "";
            decimal KDV = 0;


            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string query = " SELECT B.BANKA,B.HESAP_AD,B.IBAN,C.BANKA AS BANKA2,C.HESAP_AD AS HESAP_AD2,C.IBAN AS IBAN2 ,D.BANKA AS BANKA3,D.HESAP_AD AS HESAP_AD3 ,D.IBAN AS IBAN3," +
                               " A.MUSTERI_TEMS AS MUSTERI_TEMS_KOD,(SELECT MAX(YETKILI) AS YETKILI FROM CARI_YETKILI X  WHERE X.ID=RTRIM(A.MUSTERI_TEMS)  ) AS MUSTERI_TEMS,A.NOTLAR,A.ODEME,A.CHKODU AS CHKODU,A.CHKODU_AD AS CHKODU_AD,A.TARIH AS TARIH,A.TEKLIF_VEREN AS TEKLIF_VEREN,A.TESLIM_SEKLI FROM TEKLIFE A " +
                               " LEFT OUTER JOIN BANKA B ON ( TRIM(A.BANKA)=B.ID )  " +
                               " LEFT OUTER JOIN BANKA C ON ( TRIM(A.BANKA2)=C.ID )  " +
                               " LEFT OUTER JOIN BANKA D ON ( TRIM(A.BANKA3)=D.ID )  " +
                               " WHERE A.EVRAKNO='"+txt_teklifno_kart.Text.Trim()+"' ";

                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    firma_adi = dr["CHKODU_AD"].ToString().Trim();
                    firma_kod = dr["CHKODU"].ToString().Trim();
                    teklif_tarihi = Convert.ToDateTime(dr["TARIH"].ToString().Trim()).ToString("yyyy-MM-dd");
                    teklif_veren=dr["TEKLIF_VEREN"].ToString().Trim();
                    teklif_notlar = dr["NOTLAR"].ToString().Trim();
                    odeme = dr["ODEME"].ToString().Trim();
                    teslim_sekli = dr["TESLIM_SEKLI"].ToString().Trim();
                    if (dr["MUSTERI_TEMS_KOD"].ToString().Trim()=="0" || dr["MUSTERI_TEMS_KOD"].ToString().Trim()=="")
                    {
                        musteri_tems = "Yetkili";
                    }
                    else
                    {
                        musteri_tems = dr["MUSTERI_TEMS"].ToString().Trim();
                    }


                    if (dr["BANKA"].ToString().Trim()!="")
                    {
                        banka = dr["BANKA"].ToString().Trim();
                        iban = dr["IBAN"].ToString().Trim();
                        hesap_ad = dr["HESAP_AD"].ToString().Trim();
                        bank1= " Banka: "+ banka.Trim()+" / IBAN:" + iban.Trim() + " / Hesap Adı: "+hesap_ad.Trim();
                    }

                    if (dr["BANKA2"].ToString().Trim()!="")
                    {
                        banka2 = dr["BANKA2"].ToString().Trim();
                        iban2 = dr["IBAN2"].ToString().Trim();
                        hesap_ad2 = dr["HESAP_AD2"].ToString().Trim();
                        bank2= " Banka: "+ banka2.Trim()+" / IBAN:" + iban2.Trim() + " / Hesap Adı: "+hesap_ad2.Trim();
                    }

                    if (dr["BANKA3"].ToString().Trim()!="")
                    {
                        banka3 = dr["BANKA3"].ToString().Trim();
                        iban3 = dr["IBAN3"].ToString().Trim();
                        hesap_ad3 = dr["HESAP_AD3"].ToString().Trim();
                        bank3= " Banka: "+ banka3.Trim()+" / IBAN:" + iban3.Trim() + " / Hesap Adı: "+hesap_ad3.Trim();
                    }

                }
                dr.Close();

                string query2 = " SELECT * FROM CARI WHERE TRIM(CARI_KOD)='"+firma_kod.Trim()+"' ";
                SqlCommand cmd2 = new SqlCommand(query2, con);
                SqlDataReader dr2 = cmd2.ExecuteReader();
                if (dr2.HasRows)
                {
                    dr2.Read();
                    firma_adres = dr2["FAT_ADRES"].ToString().Trim();
                    firma_tel = "Tel: " + dr2["TEL"].ToString().Trim();
                    firma_mail = "Email: "+ dr2["EMAIL"].ToString().Trim();

                }
                dr.Close();
                con.Close();
            }




            using (MemoryStream ms = new MemoryStream())
            {
                //*************************** HEADER BİLGİLERİ YAZDIRILIYOR
                //Document document = new Document(iTextSharp.text.PageSize.A4.Rotate(), 25, 25, 30, 30);
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/depar_logo.PNG"));
                img.ScaleToFit(150, 150);
                img.SetAbsolutePosition(50, 760);
                img.Border = 0;
                img.BorderColor = iTextSharp.text.BaseColor.BLACK;
                img.BorderWidth = 5f;
                document.Add(img);

                iTextSharp.text.Image IMGISO9001 = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/ISO9001.png"));
                IMGISO9001.ScaleToFit(100, 100);
                IMGISO9001.SetAbsolutePosition(420, 750);
                //IMGISO9001.Border = iTextSharp.text.Rectangle.BOX;
                IMGISO9001.Border = 0;
                IMGISO9001.BorderColor = iTextSharp.text.BaseColor.BLACK;
                IMGISO9001.BorderWidth = 1f;
                document.Add(IMGISO9001);


                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));

                BaseFont bF = BaseFont.CreateFont("C:\\Windows\\Fonts\\Arial.ttf", "windows-1254", true);

                PdfPTable table = new PdfPTable(3);
                table.WidthPercentage = 100;
                PdfPCell cell = new PdfPCell(new Phrase("FİYAT TEKLİFİ", new iTextSharp.text.Font(bF, 12f, iTextSharp.text.Font.BOLD)));
                cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                table.AddCell(cell);
                document.Add(table);


                table = new PdfPTable(9);
                table.WidthPercentage = 100;
                table.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;

                cell = new PdfPCell();
                PdfPCell cell211 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 12f, iTextSharp.text.Font.NORMAL)));
                cell211.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell211.Border = 0;
                cell211.Colspan = 7;
                table.AddCell(cell211);

                cell = new PdfPCell();
                PdfPCell cell21 = new PdfPCell(new Phrase("Teklif No:", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell21.HorizontalAlignment = Element.ALIGN_LEFT;
                cell21.Border = 0;
                table.AddCell(cell21);

                PdfPCell cell22 = new PdfPCell(new Phrase(txt_teklifno_kart.Text.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell22.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell22.Border = 0;
                table.AddCell(cell22);

                PdfPCell cell322 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 12f, iTextSharp.text.Font.NORMAL)));
                cell322.HorizontalAlignment = Element.ALIGN_LEFT;
                cell322.Border = 0;
                cell322.Colspan = 7;
                table.AddCell(cell322);

                PdfPCell cell32 = new PdfPCell(new Phrase("Teklif Tarihi", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell32.HorizontalAlignment = Element.ALIGN_LEFT;
                cell32.Border = 0;
                table.AddCell(cell32);

                PdfPCell cell33 = new PdfPCell(new Phrase(teklif_tarihi, new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell33.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell33.Border = 0;
                table.AddCell(cell33);
                document.Add(table);
                document.Add(new Paragraph(" "));


                //***************************************** CARİLER BİLGİELRİ

                table = new PdfPTable(5);
                table.WidthPercentage = 100;

                PdfPCell cell70 = new PdfPCell(new Phrase("DEPAR MOTORLU ARAÇLAR YATAKLARI BURÇLARI VE İNŞ. SAN.TİC.LTD.ŞTİ", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell70.HorizontalAlignment = Element.ALIGN_LEFT;
                cell70.Colspan = 2;
                cell70.Border = 0;
                table.AddCell(cell70);

                PdfPCell cell71 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 15f, iTextSharp.text.Font.NORMAL)));
                cell71.HorizontalAlignment = Element.ALIGN_CENTER;
                cell71.Border = 0;
                table.AddCell(cell71);


                PdfPCell cell72 = new PdfPCell(new Phrase(firma_adi.ToString().ToUpper(), new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell72.HorizontalAlignment = Element.ALIGN_LEFT;
                cell72.Colspan = 2;
                cell72.Border = 0;
                table.AddCell(cell72);




                PdfPCell cell74 = new PdfPCell(new Phrase(teklif_veren.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell74.HorizontalAlignment = Element.ALIGN_LEFT;
                cell74.Colspan = 2;
                cell74.Border = 0;
                table.AddCell(cell74);

                PdfPCell cell75 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell75.HorizontalAlignment = Element.ALIGN_CENTER;
                cell75.Border = 0;
                table.AddCell(cell75);


                PdfPCell cell76 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell76.HorizontalAlignment = Element.ALIGN_LEFT;
                cell76.Colspan = 2;
                cell76.Border = 0;
                table.AddCell(cell76);


                PdfPCell cell77 = new PdfPCell(new Phrase("Makine Mühendisi", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell77.HorizontalAlignment = Element.ALIGN_LEFT;
                cell77.Colspan = 2;
                cell77.Border = 0;
                table.AddCell(cell77);

                PdfPCell cell78 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell78.HorizontalAlignment = Element.ALIGN_CENTER;
                cell78.Border = 0;
                table.AddCell(cell78);


                PdfPCell cell79 = new PdfPCell(new Phrase(firma_adres.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL))); ;
                cell79.HorizontalAlignment = Element.ALIGN_LEFT;
                cell79.Colspan = 2;
                cell79.Border = 0;
                table.AddCell(cell79);

                PdfPCell cell77_1 = new PdfPCell(new Phrase("sedat@deparbearings.com", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell77_1.HorizontalAlignment = Element.ALIGN_LEFT;
                cell77_1.Colspan = 2;
                cell77_1.Border = 0;
                table.AddCell(cell77_1);

                PdfPCell cell78_1 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell78_1.HorizontalAlignment = Element.ALIGN_CENTER;
                cell78_1.Border = 0;
                table.AddCell(cell78_1);


                PdfPCell cell79_1 = new PdfPCell(new Phrase(firma_tel.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL))); ;
                cell79_1.HorizontalAlignment = Element.ALIGN_LEFT;
                cell79_1.Colspan = 2;
                cell79_1.Border = 0;
                table.AddCell(cell79_1);


                PdfPCell cell80 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell80.HorizontalAlignment = Element.ALIGN_LEFT;
                cell80.Colspan = 2;
                cell80.Border = 0;
                table.AddCell(cell80);

                PdfPCell cell81 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell81.HorizontalAlignment = Element.ALIGN_CENTER;
                cell81.Border = 0;
                table.AddCell(cell81);


                PdfPCell cell82 = new PdfPCell(new Phrase(firma_mail.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL))); ;
                cell82.HorizontalAlignment = Element.ALIGN_LEFT;
                cell82.Colspan = 2;
                cell82.Border = 0;
                table.AddCell(cell82);
                document.Add(table);


                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));

                table = new PdfPTable(1);
                table.WidthPercentage = 100;


                PdfPCell cell891 = new PdfPCell(new Phrase("Sayın "+musteri_tems.Trim()+ " dikkatine,", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell891.HorizontalAlignment = Element.ALIGN_LEFT;
                cell891.Border = 0;
                table.AddCell(cell891);

                PdfPCell cell89 = new PdfPCell(new Phrase(HttpUtility.HtmlDecode(teklif_notlar.Trim()), new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell89.HorizontalAlignment = Element.ALIGN_LEFT;
                cell89.Border = 0;
                table.AddCell(cell89);
                document.Add(table);


                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));


                table = new PdfPTable(13);
                table.WidthPercentage = 100;

                PdfPCell cell90 = new PdfPCell(new Phrase("D.No", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell90.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell90);

                PdfPCell cell190 = new PdfPCell(new Phrase("Par./Marka/Model", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell190.HorizontalAlignment = Element.ALIGN_LEFT;
                cell190.Colspan = 4;
                table.AddCell(cell190);

                PdfPCell cell96 = new PdfPCell(new Phrase("Ölçü", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell96.HorizontalAlignment = Element.ALIGN_CENTER;

                table.AddCell(cell96);

                PdfPCell cell91 = new PdfPCell(new Phrase("Miktar", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell91.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell91);

                PdfPCell cell92 = new PdfPCell(new Phrase("Birim", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell92.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell92);

                PdfPCell cell93 = new PdfPCell(new Phrase("B.Fiyat", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell93.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell93);

                PdfPCell cell94 = new PdfPCell(new Phrase("P.Birimi", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell94.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell94);

                PdfPCell cell95 = new PdfPCell(new Phrase("Toplam Tutar", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell95.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell95);

                PdfPCell cell97 = new PdfPCell(new Phrase("Açıklama", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell97.HorizontalAlignment = Element.ALIGN_CENTER;
                cell97.Colspan = 2;

                table.AddCell(cell97);
                document.Add(table);


                string kod = "";
                string kod_ad = "";
                string kod_ad2 = "";
                string kod_ad3 = "";
                decimal miktar = 0;
                string birim = "";
                decimal fiyat = 0;
                string p_birim = "";
                decimal t_tutar = 0;
                string olcu = "";
                string note = "";
                string sembol = "";

                decimal alt_toplam = 0;
                decimal alt_toplam2 = 0;
                decimal alt_toplam_kdv = 0;
                decimal genel_toplam = 0;


                foreach (GridViewRow gvrow in grd_siparis_detay.Rows)
                {

                    string TEKLIFT_ID = gvrow.Cells[2].Text.Trim();
                    string TEKLIFT_KOD = HttpUtility.HtmlDecode(gvrow.Cells[3].Text.Trim());

                    if (TEKLIFT_KOD.Trim()!="Yeni Ürün")
                    {
                        using (SqlConnection con2 = new SqlConnection(constr))
                        {
                            con2.Open();
                            //string query = " SELECT A.*,CONCAT(TRIM(B.DEPAR_KOD),'-',TRIM(D.GRUPKODU_AD),'-',TRIM(C.GRUPKODU_AD) ) AS DEPAR_KOD FROM TEKLIFT A " + //07-09-2022 REVİZ EDİLDİ
                            string query = " SELECT A.*,TRIM(B.DEPAR_KOD)  AS DEPAR_KOD FROM TEKLIFT A " +
                                          " LEFT OUTER JOIN STOK_KART B ON ( TRIM(A.KOD)=TRIM(B.KOD))   " +
                                          " LEFT OUTER JOIN GRUPKODU C ON ( B.GK_4=C.GRUPKODU AND  C.KOD='GK_4' ) " +
                                          " LEFT OUTER JOIN GRUPKODU D ON ( B.GK_5=D.GRUPKODU  AND  D.KOD='GK_5' ) " +
                                          " WHERE A.ID='"+TEKLIFT_ID.Trim()+"'";
                            SqlCommand cmd2 = new SqlCommand(query, con2);
                            SqlDataReader dr2 = cmd2.ExecuteReader();
                            if (dr2.HasRows)
                            {
                                dr2.Read();
                                kod = dr2["DEPAR_KOD"].ToString().Trim();
                                kod_ad2 = dr2["KOD_AD"].ToString().Trim();
                                birim = dr2["BIRIM"].ToString().Trim();
                                decimal miktar2 = Convert.ToDecimal(dr2["MIKTAR"].ToString().Trim());
                                miktar = Decimal.Round(miktar2, 2);
                                decimal fiyat2 = Convert.ToDecimal(dr2["FIYAT"].ToString().Trim());
                                fiyat = Decimal.Round(fiyat2, 2);
                                p_birim = dr2["PARA_BIRIMI"].ToString().Trim();
                                decimal t_tutar2 = Convert.ToDecimal(dr2["TOPLAM_TUTAR"].ToString().Trim());
                                t_tutar = Decimal.Round(t_tutar2, 2);
                                olcu = dr2["OLCU"].ToString().Trim();
                                note = dr2["ACIKLAMA"].ToString().Trim();
                                KDV = Convert.ToDecimal(dr2["KDV"].ToString().Trim());
                                if (dr2["PARA_BIRIMI"].ToString().Trim()=="USD")
                                {
                                    sembol="$";
                                }
                                else if (dr2["PARA_BIRIMI"].ToString().Trim()=="EUR")
                                {
                                    sembol="€";
                                }
                                else if (dr2["PARA_BIRIMI"].ToString().Trim()=="TL")
                                {
                                    sembol="₺";
                                }
                                else if (dr2["PARA_BIRIMI"].ToString().Trim()=="GBP")
                                {
                                    sembol="£";
                                }
                                else
                                {
                                    sembol="";

                                }
                                dr2.Close();
                                con2.Close();
                            }
                        }
                    }
                    else
                    {
                        using (SqlConnection con_22 = new SqlConnection(constr))
                        {
                            con_22.Open();
                            string query = " SELECT A.*,A.KOD_AD AS DEPAR_KOD FROM TEKLIFT A " +
                                           " WHERE A.ID='"+TEKLIFT_ID.Trim()+"'";
                            SqlCommand cmd_22 = new SqlCommand(query, con_22);
                            SqlDataReader dr_22 = cmd_22.ExecuteReader();
                            if (dr_22.HasRows)
                            {
                                dr_22.Read();
                                kod = " ";
                                kod_ad3 = dr_22["KOD_AD"].ToString().Trim();
                                birim = dr_22["BIRIM"].ToString().Trim();
                                decimal miktar2 = Convert.ToDecimal(dr_22["MIKTAR"].ToString().Trim());
                                miktar = Decimal.Round(miktar2, 2);
                                decimal fiyat2 = Convert.ToDecimal(dr_22["FIYAT"].ToString().Trim());
                                fiyat = Decimal.Round(fiyat2, 2);
                                p_birim = dr_22["PARA_BIRIMI"].ToString().Trim();
                                decimal t_tutar2 = Convert.ToDecimal(dr_22["TOPLAM_TUTAR"].ToString().Trim());
                                t_tutar = Decimal.Round(t_tutar2, 2);
                                olcu = dr_22["OLCU"].ToString().Trim();
                                note = dr_22["ACIKLAMA"].ToString().Trim();
                                KDV = Convert.ToDecimal(dr_22["KDV"].ToString().Trim());

                                if (dr_22["PARA_BIRIMI"].ToString().Trim()=="USD")
                                {
                                    sembol="$";
                                }
                                else if (dr_22["PARA_BIRIMI"].ToString().Trim()=="EUR")
                                {
                                    sembol="€";
                                }
                                else if (dr_22["PARA_BIRIMI"].ToString().Trim()=="TL")
                                {
                                    sembol="₺";
                                }
                                else if (dr_22["PARA_BIRIMI"].ToString().Trim()=="GBP")
                                {
                                    sembol="£";
                                }
                                else
                                {
                                    sembol="";
                                }
                            }
                            dr_22.Close();
                            con_22.Close();
                        }
                    }


                    if (kod_ad2.Trim()!="")
                    {
                        kod_ad= kod_ad2.Trim();
                    }
                    if (kod_ad3.Trim()!="")
                    {
                        kod_ad= kod_ad3.Trim();
                    }


                    table = new PdfPTable(13);

                    table.WidthPercentage = 100;

                    PdfPCell cell100 = new PdfPCell(new Phrase(kod.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cell100.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell100);

                    PdfPCell cell1100 = new PdfPCell(new Phrase(kod_ad.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cell1100.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell1100.Colspan = 4;
                    table.AddCell(cell1100);

                    PdfPCell cell106 = new PdfPCell(new Phrase(olcu.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cell106.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell106);

                    PdfPCell cell101 = new PdfPCell(new Phrase(miktar.ToString().Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cell101.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell101);

                    PdfPCell cell102 = new PdfPCell(new Phrase(birim.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cell102.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell102);

                    PdfPCell cell103 = new PdfPCell(new Phrase(fiyat.ToString().Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cell103.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell103);

                    PdfPCell cell104 = new PdfPCell(new Phrase(p_birim.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL))); ;
                    cell104.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell104);

                    PdfPCell cell105 = new PdfPCell(new Phrase(t_tutar.ToString().Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cell105.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell105);

                    PdfPCell cell107 = new PdfPCell(new Phrase(note.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cell107.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell107.Colspan = 2;

                    table.AddCell(cell107);
                    document.Add(table);
                    alt_toplam2 = alt_toplam2 + t_tutar;
                }
                alt_toplam = Decimal.Round(alt_toplam2, 2);
                alt_toplam_kdv=0;

                if (KDV!=0)
                {
                    alt_toplam_kdv= decimal.Round(alt_toplam*KDV/100, 2);
                    decimal genel_toplam2 = alt_toplam_kdv + alt_toplam;
                    genel_toplam = Decimal.Round(genel_toplam2, 2);


                    table = new PdfPTable(3);
                    table.WidthPercentage = 30;
                    table.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.SpacingBefore = 5f;
                    table.SpacingAfter = 15f;

                    cell = new PdfPCell();
                    PdfPCell cella = new PdfPCell(new Phrase("TOPLAM TUTAR:", new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cella.HorizontalAlignment = Element.ALIGN_LEFT;
                    cella.Border = 0;
                    cella.Colspan = 2;
                    table.AddCell(cella);

                    PdfPCell cella1 = new PdfPCell(new Phrase(alt_toplam.ToString().Trim() + " " + sembol.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cella1.HorizontalAlignment = Element.ALIGN_LEFT;
                    cella1.Border = 0;
                    table.AddCell(cella1);

                    PdfPCell cella2 = new PdfPCell(new Phrase("KDV" + KDV.ToString().Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cella2.HorizontalAlignment = Element.ALIGN_LEFT;
                    cella2.Border = 0;
                    cella2.Colspan = 2;
                    table.AddCell(cella2);

                    PdfPCell cella3 = new PdfPCell(new Phrase(alt_toplam_kdv.ToString().Trim()+ " " + sembol.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cella3.HorizontalAlignment = Element.ALIGN_LEFT;
                    cella3.Border = 0;
                    table.AddCell(cella3);

                    PdfPCell cella4 = new PdfPCell(new Phrase("GENEL TOPLAM TUTAR:", new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cella4.HorizontalAlignment = Element.ALIGN_LEFT;
                    cella4.Border = 0;
                    cella4.Colspan = 2;
                    table.AddCell(cella4);

                    PdfPCell cella5 = new PdfPCell(new Phrase(genel_toplam.ToString().Trim() + " "+ sembol.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cella5.HorizontalAlignment = Element.ALIGN_LEFT;
                    cella5.Border = 0;
                    table.AddCell(cella5);
                }
                else
                {
                    table = new PdfPTable(3);
                    table.WidthPercentage = 30;
                    table.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.SpacingBefore = 5f;
                    table.SpacingAfter = 15f;

                    cell = new PdfPCell();
                    PdfPCell cella = new PdfPCell(new Phrase("TOPLAM TUTAR:", new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cella.HorizontalAlignment = Element.ALIGN_LEFT;
                    cella.Border = 0;
                    cella.Colspan = 2;
                    table.AddCell(cella);

                    PdfPCell cella1 = new PdfPCell(new Phrase(alt_toplam.ToString().Trim() +" "+sembol.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cella1.HorizontalAlignment = Element.ALIGN_LEFT;
                    cella1.Border = 0;
                    table.AddCell(cella1);

                }
                document.Add(table);




                //iTextSharp.text.Image img2 = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/depar_bos.jpg"));
                //img2.ScaleToFit(200, 50);
                //img2.SetAbsolutePosition(40, 155);
                //img2.Border = 0;
                //img2.BorderColor = iTextSharp.text.BaseColor.BLACK;
                //img2.BorderWidth = 1f;
                //document.Add(img2);


                //iTextSharp.text.Image img3 = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/depar_bos.jpg"));
                //img3.ScaleToFit(200, 50);
                //img3.SetAbsolutePosition(350, 155);
                //img3.Border = 0;
                //img3.BorderColor = iTextSharp.text.BaseColor.BLACK;
                //img3.BorderWidth = 1f;
                //document.Add(img3);




                PdfContentByte pcb = writer.DirectContent;
                table = new PdfPTable(2);
                table.TotalWidth = 520f;
                PdfPCell cell120 = new PdfPCell(new Phrase("DEPAR ONAY", new iTextSharp.text.Font(bF, 11f, iTextSharp.text.Font.NORMAL)));
                cell120.HorizontalAlignment = Element.ALIGN_CENTER;
                cell120.Border = 0;
                table.AddCell(cell120);

                PdfPCell cell121 = new PdfPCell(new Phrase("MUSTERI ONAY", new iTextSharp.text.Font(bF, 11f, iTextSharp.text.Font.NORMAL)));
                cell121.HorizontalAlignment = Element.ALIGN_CENTER;
                cell121.Border = 0;
                table.AddCell(cell121);
                table.WriteSelectedRows(0, -1, 25, 270, pcb);

                table = new PdfPTable(1);
                table.TotalWidth = 520f;

                //PdfPCell cell129 = new PdfPCell(new Phrase(" TEKLIF AÇIKLAMALARI", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.BOLD)));
                //cell129.HorizontalAlignment = Element.ALIGN_CENTER;
                //cell129.Border = 0;
                //table.AddCell(cell129);

                PdfPCell cell130 = new PdfPCell(new Phrase(" ODEME ŞEKLİ:" + odeme.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.ITALIC))); ;
                cell130.HorizontalAlignment = Element.ALIGN_LEFT;
                cell130.Border = 0;
                table.AddCell(cell130);

                //PdfPCell cell131 = new PdfPCell(new Phrase(" SİPARİŞ ŞEKLİ: 1000 adetlik partiler halinde", new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.ITALIC)));
                //cell131.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell131.Border = 0;
                //table.AddCell(cell131);

                //PdfPCell cell132 = new PdfPCell(new Phrase(" TESLİM SÜRESİ:" + , new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.ITALIC)));
                //cell132.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell132.Border = 0;
                //table.AddCell(cell132);

                PdfPCell cell133 = new PdfPCell(new Phrase(" TESLİM YERI VE TARİHİ:" + teslim_sekli.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.ITALIC)));
                cell133.HorizontalAlignment = Element.ALIGN_LEFT;
                cell133.Border = 0;
                table.AddCell(cell133);

                PdfPCell cell134 = new PdfPCell(new Phrase(" Teklifimizin geçerlilik süresi 30 gündür, bu süreyi geçen teklifler tarafımızdan yeniden değerlendirilecektir.", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.ITALIC)));
                cell134.HorizontalAlignment = Element.ALIGN_LEFT;
                cell134.Border = 0;
                table.AddCell(cell134);

                PdfPCell cell1341 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.ITALIC)));
                cell1341.HorizontalAlignment = Element.ALIGN_CENTER;
                cell1341.Border = 0;
                table.AddCell(cell1341);

                document.Add(new Paragraph(" "));

                PdfPCell cell135 = new PdfPCell(new Phrase(bank1.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.ITALIC)));
                cell135.HorizontalAlignment = Element.ALIGN_LEFT;
                cell135.Border = 0;
                table.AddCell(cell135);

                PdfPCell cell136 = new PdfPCell(new Phrase(bank2.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.ITALIC)));
                cell136.HorizontalAlignment = Element.ALIGN_LEFT;
                cell136.Border = 0;
                table.AddCell(cell136);

                PdfPCell cell1361 = new PdfPCell(new Phrase(bank3.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.ITALIC)));
                cell1361.HorizontalAlignment = Element.ALIGN_LEFT;
                cell1361.Border = 0;
                table.AddCell(cell1361);

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



                table.WriteSelectedRows(0, -1, 25, 240, pcb);


                iTextSharp.text.Image IMGISO90012 = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/ISO9001.png"));
                IMGISO90012.ScaleToFit(100, 70);
                IMGISO90012.SetAbsolutePosition(40, 10);
                //IMGISO9001.Border = iTextSharp.text.Rectangle.BOX;
                IMGISO90012.Border = 0;
                IMGISO90012.BorderColor = iTextSharp.text.BaseColor.BLACK;
                IMGISO90012.BorderWidth = 1f;
                document.Add(IMGISO90012);

                //iTextSharp.text.Image IMGISO140001 = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/ISO14001.png"));
                //IMGISO140001.ScaleToFit(100, 100);
                //IMGISO140001.SetAbsolutePosition(225, 10);
                ////IMGISO140001.Border = iTextSharp.text.Rectangle.BOX;
                //IMGISO140001.Border = 0;
                //IMGISO140001.BorderColor = iTextSharp.text.BaseColor.BLACK;
                //IMGISO140001.BorderWidth = 1f;
                //document.Add(IMGISO140001);

                //iTextSharp.text.Image IMGISO450001 = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/ISO45001.png"));
                //IMGISO450001.ScaleToFit(100, 100);
                //IMGISO450001.SetAbsolutePosition(410, 10);
                //IMGISO450001.Border = 0;
                //IMGISO450001.BorderColor = iTextSharp.text.BaseColor.BLACK;
                //IMGISO450001.BorderWidth = 1f;
                //document.Add(IMGISO450001);


                document.Close();
                writer.Close();


                Response.ContentType = "pdf/application";
                Response.AddHeader("content-disposition", "attachment;filename=" + txt_teklifno_kart.Text.Trim() +".pdf");
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            }

        }

        protected void print_av2(object sender, EventArgs e)
        {
            if (txt_teklifno_kart.Text.Trim()=="")
            {
                Response.Write("<script lang='JavaScript'>alert('Teklif no seçilmeden teklif formu alınamaz.');</script>");
                return;
            }

            string firma_adi = "";
            string firma_kod = "";
            string teklif_tarihi = "";
            string teklif_veren = "";
            string firma_adres = "";
            string firma_mail = "";
            string teklif_notlar = "";
            string odeme = "";
            string teslim_sekli = "";
            string musteri_tems = "";


            string banka = "";
            string iban = "";
            string hesap_ad = "";

            string banka2 = "";
            string iban2 = "";
            string hesap_ad2 = "";

            string banka3 = "";
            string iban3 = "";
            string hesap_ad3 = "";
            string bank1 = "";
            string bank2 = "";
            string bank3 = "";


            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string query = " SELECT A.TESLIM_SEKLI,B.BANKA,B.HESAP_AD,B.IBAN,C.BANKA AS BANKA2,C.HESAP_AD AS HESAP_AD2,C.IBAN AS IBAN2 ,D.BANKA AS BANKA3,D.HESAP_AD AS HESAP_AD3 ,D.IBAN AS IBAN3," +
                               " A.MUSTERI_TEMS AS MUSTERI_TEMS_KOD,(SELECT MAX(YETKILI) AS YETKILI FROM CARI_YETKILI X  WHERE X.ID=RTRIM(A.MUSTERI_TEMS)  ) AS MUSTERI_TEMS,A.NOTLAR,A.ODEME,A.CHKODU AS CHKODU,A.CHKODU_AD AS CHKODU_AD,A.TARIH AS TARIH,A.TEKLIF_VEREN AS TEKLIF_VEREN FROM TEKLIFE A " +
                               " LEFT OUTER JOIN BANKA B ON ( TRIM(A.BANKA)=B.ID )  " +
                               " LEFT OUTER JOIN BANKA C ON ( TRIM(A.BANKA2)=C.ID )  " +
                               " LEFT OUTER JOIN BANKA D ON ( TRIM(A.BANKA3)=D.ID )  " +
                               " WHERE A.EVRAKNO='"+txt_teklifno_kart.Text.Trim()+"' ";

                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    firma_adi = dr["CHKODU_AD"].ToString().Trim();
                    firma_kod = dr["CHKODU"].ToString().Trim();
                    teklif_tarihi = Convert.ToDateTime(dr["TARIH"].ToString().Trim()).ToString("yyyy-MM-dd");
                    teklif_veren=dr["TEKLIF_VEREN"].ToString().Trim();
                    teklif_notlar = dr["NOTLAR"].ToString().Trim();
                    odeme = dr["ODEME"].ToString().Trim();
                    teslim_sekli = dr["TESLIM_SEKLI"].ToString().Trim();
                    musteri_tems = dr["MUSTERI_TEMS"].ToString().Trim();
                    if (dr["BANKA"].ToString().Trim()!="")
                    {
                        banka = dr["BANKA"].ToString().Trim();
                        iban = dr["IBAN"].ToString().Trim();
                        hesap_ad = dr["HESAP_AD"].ToString().Trim();
                        bank1= " Bank: "+ banka.Trim()+" / IBAN:" + iban.Trim() + " / SWIF No: ";
                    }

                    if (dr["BANKA2"].ToString().Trim()!="")
                    {
                        banka2 = dr["BANKA2"].ToString().Trim();
                        iban2 = dr["IBAN2"].ToString().Trim();
                        hesap_ad2 = dr["HESAP_AD2"].ToString().Trim();
                        bank2= " Bank: "+ banka2.Trim()+" / IBAN:" + iban2.Trim() + " / SWIF No: ";
                    }

                    if (dr["BANKA3"].ToString().Trim()!="")
                    {
                        banka3 = dr["BANKA3"].ToString().Trim();
                        iban3 = dr["IBAN3"].ToString().Trim();
                        hesap_ad3 = dr["HESAP_AD3"].ToString().Trim();
                        bank3= " Bank: "+ banka3.Trim()+" / IBAN:" + iban3.Trim() + " / SWIF No: ";
                    }

                }
                dr.Close();

                string query2 = " SELECT * FROM CARI WHERE TRIM(CARI_KOD)='"+firma_kod.Trim()+"' ";
                SqlCommand cmd2 = new SqlCommand(query2, con);
                SqlDataReader dr2 = cmd2.ExecuteReader();
                if (dr2.HasRows)
                {
                    dr2.Read();
                    firma_adres = dr2["FAT_ADRES"].ToString().Trim() + " " + " Tel:" + dr2["TEL"].ToString().Trim();
                    firma_mail = dr2["EMAIL"].ToString().Trim();

                }
                dr.Close();
                con.Close();
            }




            using (MemoryStream ms = new MemoryStream())
            {
                //*************************** HEADER BİLGİLERİ YAZDIRILIYOR
                //Document document = new Document(iTextSharp.text.PageSize.A4.Rotate(), 25, 25, 30, 30);
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/depar_logo.PNG"));
                img.ScaleToFit(150, 150);
                img.SetAbsolutePosition(50, 760);
                img.Border = 0;
                img.BorderColor = iTextSharp.text.BaseColor.BLACK;
                img.BorderWidth = 5f;
                document.Add(img);

                iTextSharp.text.Image IMGISO9001 = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/ISO9001.png"));
                IMGISO9001.ScaleToFit(100, 100);
                IMGISO9001.SetAbsolutePosition(420, 750);
                //IMGISO9001.Border = iTextSharp.text.Rectangle.BOX;
                IMGISO9001.Border = 0;
                IMGISO9001.BorderColor = iTextSharp.text.BaseColor.BLACK;
                IMGISO9001.BorderWidth = 1f;
                document.Add(IMGISO9001);


                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));

                BaseFont bF = BaseFont.CreateFont("C:\\Windows\\Fonts\\Arial.ttf", "windows-1254", true);

                PdfPTable table = new PdfPTable(3);
                table.WidthPercentage = 100;
                PdfPCell cell = new PdfPCell(new Phrase("PROFORMA INVOICE", new iTextSharp.text.Font(bF, 12f, iTextSharp.text.Font.BOLD)));
                cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                table.AddCell(cell);
                document.Add(table);


                table = new PdfPTable(9);
                table.WidthPercentage = 100;
                table.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;

                cell = new PdfPCell();
                PdfPCell cell211 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 12f, iTextSharp.text.Font.NORMAL)));
                cell211.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell211.Border = 0;
                cell211.Colspan = 7;
                table.AddCell(cell211);

                cell = new PdfPCell();
                PdfPCell cell21 = new PdfPCell(new Phrase("Offer No:", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell21.HorizontalAlignment = Element.ALIGN_LEFT;
                cell21.Border = 0;
                table.AddCell(cell21);

                PdfPCell cell22 = new PdfPCell(new Phrase(txt_teklifno_kart.Text.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell22.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell22.Border = 0;
                table.AddCell(cell22);

                PdfPCell cell322 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 12f, iTextSharp.text.Font.NORMAL)));
                cell322.HorizontalAlignment = Element.ALIGN_LEFT;
                cell322.Border = 0;
                cell322.Colspan = 7;
                table.AddCell(cell322);

                PdfPCell cell32 = new PdfPCell(new Phrase("Offer Date", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell32.HorizontalAlignment = Element.ALIGN_LEFT;
                cell32.Border = 0;
                table.AddCell(cell32);

                PdfPCell cell33 = new PdfPCell(new Phrase(teklif_tarihi, new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell33.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell33.Border = 0;
                table.AddCell(cell33);
                document.Add(table);
                document.Add(new Paragraph(" "));


                //***************************************** CARİLER BİLGİELRİ

                table = new PdfPTable(5);
                table.WidthPercentage = 100;

                PdfPCell cell70 = new PdfPCell(new Phrase("DEPAR MOTORLU ARAÇLAR YATAKLARI BURÇLARI VE İNŞ. SAN.TİC.LTD.ŞTİ", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell70.HorizontalAlignment = Element.ALIGN_LEFT;
                cell70.Colspan = 2;
                cell70.Border = 0;
                table.AddCell(cell70);

                PdfPCell cell71 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 15f, iTextSharp.text.Font.NORMAL)));
                cell71.HorizontalAlignment = Element.ALIGN_CENTER;
                cell71.Border = 0;
                table.AddCell(cell71);


                PdfPCell cell72 = new PdfPCell(new Phrase(firma_adi.ToString().ToUpper(), new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell72.HorizontalAlignment = Element.ALIGN_LEFT;
                cell72.Colspan = 2;
                cell72.Border = 0;
                table.AddCell(cell72);




                PdfPCell cell74 = new PdfPCell(new Phrase(teklif_veren.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell74.HorizontalAlignment = Element.ALIGN_LEFT;
                cell74.Colspan = 2;
                cell74.Border = 0;
                table.AddCell(cell74);

                PdfPCell cell75 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell75.HorizontalAlignment = Element.ALIGN_CENTER;
                cell75.Border = 0;
                table.AddCell(cell75);


                PdfPCell cell76 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell76.HorizontalAlignment = Element.ALIGN_LEFT;
                cell76.Colspan = 2;
                cell76.Border = 0;
                table.AddCell(cell76);


                PdfPCell cell77 = new PdfPCell(new Phrase("Mechanical Engineer", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell77.HorizontalAlignment = Element.ALIGN_LEFT;
                cell77.Colspan = 2;
                cell77.Border = 0;
                table.AddCell(cell77);

                PdfPCell cell78 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell78.HorizontalAlignment = Element.ALIGN_CENTER;
                cell78.Border = 0;
                table.AddCell(cell78);


                PdfPCell cell79 = new PdfPCell(new Phrase(firma_adres.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL))); ;
                cell79.HorizontalAlignment = Element.ALIGN_LEFT;
                cell79.Colspan = 2;
                cell79.Border = 0;
                table.AddCell(cell79);


                PdfPCell cell80 = new PdfPCell(new Phrase("sedat@deparbearings.com", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell80.HorizontalAlignment = Element.ALIGN_LEFT;
                cell80.Colspan = 2;
                cell80.Border = 0;
                table.AddCell(cell80);

                PdfPCell cell81 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell81.HorizontalAlignment = Element.ALIGN_CENTER;
                cell81.Border = 0;
                table.AddCell(cell81);


                PdfPCell cell82 = new PdfPCell(new Phrase(firma_mail.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL))); ;
                cell82.HorizontalAlignment = Element.ALIGN_LEFT;
                cell82.Colspan = 2;
                cell82.Border = 0;
                table.AddCell(cell82);
                document.Add(table);


                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));

                table = new PdfPTable(1);
                table.WidthPercentage = 100;

                PdfPCell cell891 = new PdfPCell(new Phrase("Dear "+musteri_tems.Trim(), new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell891.HorizontalAlignment = Element.ALIGN_LEFT;
                cell891.Border = 0;
                table.AddCell(cell891);

                PdfPCell cell89 = new PdfPCell(new Phrase(teklif_notlar.Trim(), new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell89.HorizontalAlignment = Element.ALIGN_LEFT;
                cell89.Border = 0;
                table.AddCell(cell89);

                PdfPCell cell89_1 = new PdfPCell(new Phrase("Best Regards.", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell89_1.HorizontalAlignment = Element.ALIGN_LEFT;
                cell89_1.Border = 0;
                table.AddCell(cell89_1);

                document.Add(table);


                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));


                table = new PdfPTable(13);
                table.WidthPercentage = 100;

                PdfPCell cell90 = new PdfPCell(new Phrase("Depar No", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell90.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell90);

                PdfPCell cell190 = new PdfPCell(new Phrase("Piece./Brand/Model", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell190.HorizontalAlignment = Element.ALIGN_LEFT;
                cell190.Colspan = 4;
                table.AddCell(cell190);

                PdfPCell cell96 = new PdfPCell(new Phrase("Measure", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell96.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell96);

                PdfPCell cell91 = new PdfPCell(new Phrase("Quantity", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell91.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell91);

                PdfPCell cell92 = new PdfPCell(new Phrase("Unit", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell92.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell92);

                PdfPCell cell93 = new PdfPCell(new Phrase("Price", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell93.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell93);

                PdfPCell cell94 = new PdfPCell(new Phrase("P.Unit", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell94.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell94);

                PdfPCell cell95 = new PdfPCell(new Phrase("Total Price", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell95.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell95);

                PdfPCell cell97 = new PdfPCell(new Phrase("Note", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell97.HorizontalAlignment = Element.ALIGN_CENTER;
                cell97.Colspan = 2;

                table.AddCell(cell97);
                document.Add(table);


                string kod = "";
                string kod_ad = "";
                string kod_ad2 = "";
                string kod_ad3 = "";
                decimal miktar = 0;
                string birim = "";
                decimal fiyat = 0;
                string p_birim = "";
                decimal t_tutar = 0;
                string olcu = "";
                string note = "";

                decimal alt_toplam = 0;
                decimal alt_toplam2 = 0;
                decimal alt_toplam_kdv = 0;
                decimal genel_toplam = 0;

                foreach (GridViewRow gvrow in grd_siparis_detay.Rows)
                {
                    string TEKLIFT_ID = gvrow.Cells[2].Text.Trim();
                    string TEKLIFT_KOD = HttpUtility.HtmlDecode(gvrow.Cells[3].Text.Trim());

                    if (TEKLIFT_KOD.Trim()!="Yeni Ürün")
                    {
                        using (SqlConnection con2 = new SqlConnection(constr))
                        {
                            con2.Open();
                            string query = " SELECT A.*,CONCAT(TRIM(B.DEPAR_KOD),'-',TRIM(D.GRUPKODU_AD),'-',TRIM(C.GRUPKODU_AD) ) AS DEPAR_KOD FROM TEKLIFT A " +
                                           " LEFT OUTER JOIN STOK_KART B ON ( TRIM(A.KOD)=TRIM(B.KOD) )   " +
                                           " LEFT OUTER JOIN GRUPKODU C ON B.GK_4=C.GRUPKODU AND  C.KOD='GK_4' " +
                                           " LEFT OUTER JOIN GRUPKODU D ON B.GK_5=D.GRUPKODU  AND  D.KOD='GK_5' " +
                                           " WHERE A.ID='"+TEKLIFT_ID.Trim()+"'";

                            SqlCommand cmd2 = new SqlCommand(query, con2);
                            SqlDataReader dr2 = cmd2.ExecuteReader();
                            if (dr2.HasRows)
                            {
                                dr2.Read();
                                kod = dr2["DEPAR_KOD"].ToString().Trim();
                                kod_ad2 = dr2["KOD_AD"].ToString().Trim();
                                birim = dr2["BIRIM"].ToString().Trim();
                                decimal miktar2 = Convert.ToDecimal(dr2["MIKTAR"].ToString().Trim());
                                miktar = Decimal.Round(miktar2, 2);
                                decimal fiyat2 = Convert.ToDecimal(dr2["FIYAT"].ToString().Trim());
                                fiyat = Decimal.Round(fiyat2, 2);
                                p_birim = dr2["PARA_BIRIMI"].ToString().Trim();
                                decimal t_tutar2 = Convert.ToDecimal(dr2["TOPLAM_TUTAR"].ToString().Trim());
                                t_tutar = Decimal.Round(t_tutar2, 2);
                                olcu = dr2["OLCU"].ToString().Trim();
                                note = dr2["ACIKLAMA"].ToString().Trim();
                            }
                            dr2.Close();
                            con2.Close();
                        }
                    }
                    else
                    {
                        using (SqlConnection con_22 = new SqlConnection(constr))
                        {
                            con_22.Open();
                            string query = " SELECT A.*,A.KOD_AD AS DEPAR_KOD FROM TEKLIFT A " +
                                           " WHERE A.ID='"+TEKLIFT_ID.Trim()+"'";
                            SqlCommand cmd_22 = new SqlCommand(query, con_22);
                            SqlDataReader dr_22 = cmd_22.ExecuteReader();
                            if (dr_22.HasRows)
                            {
                                dr_22.Read();
                                kod = " ";
                                kod_ad3 = dr_22["KOD_AD"].ToString().Trim();
                                birim = dr_22["BIRIM"].ToString().Trim();
                                decimal miktar2 = Convert.ToDecimal(dr_22["MIKTAR"].ToString().Trim());
                                miktar = Decimal.Round(miktar2, 2);
                                decimal fiyat2 = Convert.ToDecimal(dr_22["FIYAT"].ToString().Trim());
                                fiyat = Decimal.Round(fiyat2, 2);
                                p_birim = dr_22["PARA_BIRIMI"].ToString().Trim();
                                decimal t_tutar2 = Convert.ToDecimal(dr_22["TOPLAM_TUTAR"].ToString().Trim());
                                t_tutar = Decimal.Round(t_tutar2, 2);
                                olcu = dr_22["OLCU"].ToString().Trim();
                                note = dr_22["ACIKLAMA"].ToString().Trim();
                            }
                            dr_22.Close();
                            con_22.Close();
                        }
                    }


                    if (kod_ad2.Trim()!="")
                    {
                        kod_ad= kod_ad2.Trim();
                    }
                    if (kod_ad3.Trim()!="")
                    {
                        kod_ad= kod_ad3.Trim();
                    }


                    table = new PdfPTable(13);
                    table.WidthPercentage = 100;

                    PdfPCell cell100 = new PdfPCell(new Phrase(kod.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cell100.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell100);

                    PdfPCell cell1100 = new PdfPCell(new Phrase(kod_ad.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cell1100.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell1100.Colspan = 4;
                    table.AddCell(cell1100);

                    PdfPCell cell106 = new PdfPCell(new Phrase(olcu.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cell106.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell106);

                    PdfPCell cell101 = new PdfPCell(new Phrase(miktar.ToString().Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cell101.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell101);

                    PdfPCell cell102 = new PdfPCell(new Phrase(birim.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cell102.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell102);

                    PdfPCell cell103 = new PdfPCell(new Phrase(fiyat.ToString().Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cell103.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell103);

                    PdfPCell cell104 = new PdfPCell(new Phrase(p_birim.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL))); ;
                    cell104.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell104);

                    PdfPCell cell105 = new PdfPCell(new Phrase(t_tutar.ToString().Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cell105.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell105);

                    PdfPCell cell107 = new PdfPCell(new Phrase(note.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cell107.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell107.Colspan = 2;

                    table.AddCell(cell107);
                    document.Add(table);
                    alt_toplam2 = alt_toplam2 + t_tutar;
                }
                alt_toplam = Decimal.Round(alt_toplam2, 2);
                decimal alt_toplam_kdv2 = alt_toplam * 18 / 100;
                alt_toplam_kdv = Decimal.Round(alt_toplam_kdv2, 2);
                decimal genel_toplam2 = alt_toplam_kdv + alt_toplam;
                genel_toplam = Decimal.Round(genel_toplam2, 2);

                table = new PdfPTable(3);
                table.WidthPercentage = 30;
                table.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.SpacingBefore = 5f;
                table.SpacingAfter = 15f;

                cell = new PdfPCell();
                PdfPCell cella = new PdfPCell(new Phrase("Total Price:", new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                cella.HorizontalAlignment = Element.ALIGN_LEFT;
                cella.Border = 0;
                cella.Colspan = 2;
                table.AddCell(cella);

                PdfPCell cella1 = new PdfPCell(new Phrase(alt_toplam.ToString().Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                cella1.HorizontalAlignment = Element.ALIGN_LEFT;
                cella1.Border = 0;
                table.AddCell(cella1);

                document.Add(table);



                PdfContentByte pcb = writer.DirectContent;
                table = new PdfPTable(2);
                table.TotalWidth = 520f;
                PdfPCell cell120 = new PdfPCell(new Phrase("DEPAR Confirm/Signature", new iTextSharp.text.Font(bF, 11f, iTextSharp.text.Font.NORMAL)));
                cell120.HorizontalAlignment = Element.ALIGN_CENTER;
                cell120.Border = 0;
                table.AddCell(cell120);

                PdfPCell cell121 = new PdfPCell(new Phrase("Customer Confirm/Signature", new iTextSharp.text.Font(bF, 11f, iTextSharp.text.Font.NORMAL)));
                cell121.HorizontalAlignment = Element.ALIGN_CENTER;
                cell121.Border = 0;
                table.AddCell(cell121);
                table.WriteSelectedRows(0, -1, 25, 270, pcb);

                table = new PdfPTable(1);
                table.TotalWidth = 520f;

                //PdfPCell cell129 = new PdfPCell(new Phrase(" TEKLIF AÇIKLAMALARI", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.BOLD)));
                //cell129.HorizontalAlignment = Element.ALIGN_CENTER;
                //cell129.Border = 0;
                //table.AddCell(cell129);

                PdfPCell cell130 = new PdfPCell(new Phrase("Paymnet Type:" + odeme.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.ITALIC))); ;
                cell130.HorizontalAlignment = Element.ALIGN_LEFT;
                cell130.Border = 0;
                table.AddCell(cell130);

                //PdfPCell cell131 = new PdfPCell(new Phrase(" SİPARİŞ ŞEKLİ: 1000 adetlik partiler halinde", new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.ITALIC)));
                //cell131.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell131.Border = 0;
                //table.AddCell(cell131);

                //PdfPCell cell132 = new PdfPCell(new Phrase(" TESLİM SÜRESİ:" + , new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.ITALIC)));
                //cell132.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell132.Border = 0;
                //table.AddCell(cell132);

                PdfPCell cell133 = new PdfPCell(new Phrase(" Delivery Place and Date:" + teslim_sekli.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.ITALIC)));
                cell133.HorizontalAlignment = Element.ALIGN_LEFT;
                cell133.Border = 0;
                table.AddCell(cell133);

                PdfPCell cell134 = new PdfPCell(new Phrase(" The validity period of our offer is 30 days, offers exceeding this period will be recalculate", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.ITALIC)));
                cell134.HorizontalAlignment = Element.ALIGN_LEFT;
                cell134.Border = 0;
                table.AddCell(cell134);

                PdfPCell cell1341 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.ITALIC)));
                cell1341.HorizontalAlignment = Element.ALIGN_CENTER;
                cell1341.Border = 0;
                table.AddCell(cell1341);

                document.Add(new Paragraph(" "));

                PdfPCell cell135 = new PdfPCell(new Phrase(bank1.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.ITALIC)));
                cell135.HorizontalAlignment = Element.ALIGN_LEFT;
                cell135.Border = 0;
                table.AddCell(cell135);

                PdfPCell cell136 = new PdfPCell(new Phrase(bank2.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.ITALIC)));
                cell136.HorizontalAlignment = Element.ALIGN_LEFT;
                cell136.Border = 0;
                table.AddCell(cell136);

                PdfPCell cell1361 = new PdfPCell(new Phrase(bank3.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.ITALIC)));
                cell1361.HorizontalAlignment = Element.ALIGN_LEFT;
                cell1361.Border = 0;
                table.AddCell(cell1361);

                document.Add(new Paragraph(" "));


                PdfPCell cell1362 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.ITALIC)));
                cell1362.HorizontalAlignment = Element.ALIGN_CENTER;
                cell1362.Border = 0;
                table.AddCell(cell1362);

                PdfPCell cell1363 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.ITALIC)));
                cell1363.HorizontalAlignment = Element.ALIGN_CENTER;
                cell1363.Border = 0;
                table.AddCell(cell1363);

                PdfPCell cell137 = new PdfPCell(new Phrase("Esenler mahallesi sude sokak no:2 Pendik / İSTANBUL / TURKEY :", new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.ITALIC)));
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



                table.WriteSelectedRows(0, -1, 25, 240, pcb);


                iTextSharp.text.Image IMGISO90012 = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/ISO9001.png"));
                IMGISO90012.ScaleToFit(100, 70);
                IMGISO90012.SetAbsolutePosition(40, 10);
                //IMGISO9001.Border = iTextSharp.text.Rectangle.BOX;
                IMGISO90012.Border = 0;
                IMGISO90012.BorderColor = iTextSharp.text.BaseColor.BLACK;
                IMGISO90012.BorderWidth = 1f;
                document.Add(IMGISO90012);

                //iTextSharp.text.Image IMGISO140001 = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/ISO14001.png"));
                //IMGISO140001.ScaleToFit(100, 100);
                //IMGISO140001.SetAbsolutePosition(225, 10);
                ////IMGISO140001.Border = iTextSharp.text.Rectangle.BOX;
                //IMGISO140001.Border = 0;
                //IMGISO140001.BorderColor = iTextSharp.text.BaseColor.BLACK;
                //IMGISO140001.BorderWidth = 1f;
                //document.Add(IMGISO140001);

                //iTextSharp.text.Image IMGISO450001 = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/ISO45001.png"));
                //IMGISO450001.ScaleToFit(100, 100);
                //IMGISO450001.SetAbsolutePosition(410, 10);
                //IMGISO450001.Border = 0;
                //IMGISO450001.BorderColor = iTextSharp.text.BaseColor.BLACK;
                //IMGISO450001.BorderWidth = 1f;
                //document.Add(IMGISO450001);


                document.Close();
                writer.Close();


                Response.ContentType = "pdf/application";
                Response.AddHeader("content-disposition", "attachment;filename=" + txt_teklifno_kart.Text.Trim() +".pdf");
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            }

        }

        protected void onay1_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void siparis_av(object sender, EventArgs e)
        {
            if (txt_teklifno_kart.Text.Trim()=="")
            {
                Response.Write("<script lang='JavaScript'>alert('Teklifi siparişe dönüştürmek için öncelikle teklif seçiniz. Kayıt yapılmadı.');</script>");
                return;
            }
            if (drp_teklif_durum.SelectedValue.Trim()=="1" || drp_teklif_durum.SelectedValue.Trim()=="2" || drp_teklif_durum.SelectedValue.Trim()=="4" || drp_teklif_durum.SelectedValue.Trim()=="5")
            {
                Response.Write("<script lang='JavaScript'>alert('Sadece müşteri onayı alınmış tekliflerin siparişe dönüşümü yapılabilir. Kayıt yapılmadı.');</script>");
                return;
            }

            //sipariş kaytıt işlemi yapılıyor.
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                //yeni sipariş numarası alınıyor
                string siparis_no = "";
                SqlCommand cmd2 = new SqlCommand(" select TOP 1 SIPNO ,substring(SIPNO,10,5)+1  AS NUMBER  from SIPARISE ORDER BY ID DESC ", con);
                SqlDataReader dr2 = cmd2.ExecuteReader();
                dr2.Read();
                if (dr2.HasRows)
                {
                    string next_number2 = dr2["NUMBER"].ToString();
                    siparis_no = "DPR" + DateTime.Now.ToString("yyyyMM").ToString() + next_number2;
                    dr2.Close();
                }
                else
                {
                    siparis_no = "DPR" + DateTime.Now.ToString("yyyyMM").ToString() + "1";
                }
                dr2.Close();
                if (siparis_no.Trim()=="")
                {
                    Response.Write("<script lang='JavaScript'>alert('Boş bir sipariş numarası alınamadıgı için siparişiniz açılamadı. Lütfen sistem yöneticiniz ile iletişime geçiniz.');</script>");
                    return;
                }


                SqlCommand cmd2_1 = new SqlCommand(" SELECT * FROM TEKLIFT  WHERE RTRIM(EVRAKNO)='"+txt_teklifno_kart.Text.Trim()+"' and RTRIM(SIPARISNO)<>'' ", con);
                SqlDataReader dr2_1 = cmd2_1.ExecuteReader();
                if (dr2_1.HasRows)
                {
                    dr2_1.Read();
                    string acilan_sip = dr2_1["SIPARISNO"].ToString().Trim();
                    Response.Write("<script lang='JavaScript'>alert('Bu teklif daha önce "+acilan_sip.Trim()+" nolu siparişe dönüştürülmüştür.Tekrar sipariş açılamaz');</script>");
                    return;
                }
                dr2_1.Close();

                SqlCommand cmd3 = new SqlCommand(" INSERT INTO SIPARISE (SIPNO,TARIH,CH_KODU,CH_ADI,NOTLAR,MUSTERI_TEMS,CREATE_USER,CREATE_DATE,ODEME,VADE,DURUM,TESLIM_SEKLI ) " +
                                                     " values(@sipno,@tarih,@ch_kodu,@ch_adi,@notlar,@must_tem,@user,@date,@ODEME,@VADE,@DURUM,@TESLIM_SEKLI) ", con);
                cmd3.Parameters.AddWithValue("@sipno", siparis_no.Trim());
                cmd3.Parameters.AddWithValue("@tarih", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd3.Parameters.AddWithValue("@ch_kodu", drp_carikodu_kart.SelectedValue.Trim());
                cmd3.Parameters.AddWithValue("@ODEME", txt_ödeme.Text.Trim());
                cmd3.Parameters.AddWithValue("@VADE", txt_vade.Text.Trim());
                cmd3.Parameters.AddWithValue("@ch_adi", drp_carikodu_kart.SelectedItem.Text.Trim());
                cmd3.Parameters.AddWithValue("@notlar", txt_teklifnotlar_kart.Text.Trim());
                cmd3.Parameters.AddWithValue("@must_tem", drp_must_yetkili.SelectedValue.Trim());
                cmd3.Parameters.AddWithValue("@user", usr_name.username.Trim());
                cmd3.Parameters.AddWithValue("@date", DateTime.Now);
                cmd3.Parameters.AddWithValue("@DURUM", "0");
                cmd3.Parameters.AddWithValue("@TESLIM_SEKLI", txt_teklifteslimsekli.Text.Trim());
                try
                {
                    cmd3.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    erp_log.log_at_av(usr_name.username, ex.ToString());
                    Response.Write("<script lang='JavaScript'>alert('Database baglantısında bir sorun yaşandı lütfen sistem yöneticiniz ile iletişime geçiniz.');</script>");
                    return;
                }


                foreach (GridViewRow gvrow in grd_siparis_detay.Rows)
                {
                    string rev_1 = "R";
                    string K_MIN = "0";
                    string K_MAX = "0";
                    string YU_MIN = "0";
                    string YU_MAX = "0";
                    string YAG_MIN = "0";
                    string YAG_MAX = "0";
                    string CI_MIN = "0";
                    string CI_MAX = "0";
                    if (gvrow.Cells[5].Text.Trim()=="S")
                    {
                        rev_1="R";
                    }
                    else
                    {
                        rev_1=gvrow.Cells[5].Text.Trim();
                    }
                    SqlCommand cmd4_1 = new SqlCommand(" SELECT * FROM STOK_KART_REV WHERE RTRIM(KOD)='"+HttpUtility.HtmlDecode(gvrow.Cells[3].Text.Trim())+"' AND RTRIM(REVIZYON)='"+rev_1.Trim()+"'", con);
                    SqlDataReader dr4_1 = cmd4_1.ExecuteReader();
                    if (dr4_1.HasRows)
                    {
                        dr4_1.Read();
                        K_MIN = dr4_1["MIL_MIN"].ToString().Trim();
                        K_MAX = dr4_1["MIL_MAX"].ToString().Trim();
                        YU_MIN = dr4_1["YUVA_MIN"].ToString().Trim();
                        YU_MAX = dr4_1["YUVA_MAX"].ToString().Trim();
                        YAG_MIN = dr4_1["YAG_MIN"].ToString().Trim();
                        YAG_MAX = dr4_1["YAG_MAX"].ToString().Trim();
                        YAG_MIN = dr4_1["CIDAR_MIN"].ToString().Trim();
                        YAG_MAX = dr4_1["CIDAR_MAX"].ToString().Trim();
                    }
                    dr4_1.Close();


                    SqlCommand cmd4 = new SqlCommand(" INSERT INTO SIPARIST (SIPNO,STOK_KOD,STOK_AD,MIKTAR,KALAN_MIKTAR,BIRIM,SATIR_NOT,BIRIM_FIYAT,TOPLAM_TUTAR,PARA_BIRIMI,KDV,REVIZYON,K_MIN,K_MAX,YU_MIN,YU_MAX,YAG_MIN,YAG_MAX,CI_MIN,CI_MAX)  " +
                                                         " values(@sipno,@stok_kod,@stok_ad,@miktar,@KALAN_MIKTAR,@birim,@satir_not,@BIRIM_FIYAT,@TOPLAM_TUTAR,@PARA_BIRIMI,@KDV,@REVIZYON,@K_MIN,@K_MAX,@YU_MIN,@YU_MAX,@YAG_MIN,@YAG_MAX,@CI_MIN,@CI_MAX) ", con);
                    cmd4.Parameters.AddWithValue("@sipno", siparis_no.Trim());
                    cmd4.Parameters.AddWithValue("@stok_kod", HttpUtility.HtmlDecode(gvrow.Cells[3].Text.Trim()));
                    cmd4.Parameters.AddWithValue("@stok_ad", HttpUtility.HtmlDecode(gvrow.Cells[4].Text.Trim()));
                    cmd4.Parameters.AddWithValue("@miktar", gvrow.Cells[8].Text.Trim().Replace(',', '.'));
                    cmd4.Parameters.AddWithValue("@KALAN_MIKTAR", gvrow.Cells[8].Text.Trim().Replace(',', '.'));
                    cmd4.Parameters.AddWithValue("@BIRIM_FIYAT", gvrow.Cells[6].Text.Trim().Replace(',', '.'));
                    decimal toplam_tutar = Convert.ToDecimal(gvrow.Cells[6].Text.Trim()) * Convert.ToDecimal(gvrow.Cells[8].Text.Trim());
                    cmd4.Parameters.AddWithValue("@TOPLAM_TUTAR", toplam_tutar.ToString().Replace(',', '.'));
                    cmd4.Parameters.AddWithValue("@REVIZYON", rev_1.Trim());
                    cmd4.Parameters.AddWithValue("@birim", HttpUtility.HtmlDecode(gvrow.Cells[9].Text.Trim()));
                    cmd4.Parameters.AddWithValue("@PARA_BIRIMI", gvrow.Cells[7].Text.Trim());
                    cmd4.Parameters.AddWithValue("@KDV", drp_kdv.SelectedValue.Trim());
                    cmd4.Parameters.AddWithValue("@satir_not", HttpUtility.HtmlDecode(gvrow.Cells[10].Text.Trim())+"  "+HttpUtility.HtmlDecode(gvrow.Cells[14].Text.Trim()));

                    cmd4.Parameters.AddWithValue("@K_MIN", K_MIN.Trim().Replace(',', '.'));
                    cmd4.Parameters.AddWithValue("@K_MAX", K_MAX.Trim().Replace(',', '.'));
                    cmd4.Parameters.AddWithValue("@YU_MIN", YU_MIN.Trim().Replace(',', '.'));
                    cmd4.Parameters.AddWithValue("@YU_MAX", YU_MIN.Trim().Replace(',', '.'));
                    cmd4.Parameters.AddWithValue("@YAG_MIN", YAG_MIN.Trim().Replace(',', '.'));
                    cmd4.Parameters.AddWithValue("@YAG_MAX", YAG_MAX.Trim().Replace(',', '.'));
                    cmd4.Parameters.AddWithValue("@CI_MIN", CI_MIN.Trim().Replace(',', '.'));
                    cmd4.Parameters.AddWithValue("@CI_MAX", CI_MAX.Trim().Replace(',', '.'));
                    try
                    {
                        cmd4.ExecuteNonQuery();
                        SqlCommand cmd5 = new SqlCommand(" UPDATE TEKLIFT  SET  SIPARISNO='"+siparis_no+"' WHERE ID='"+gvrow.Cells[2].Text.Trim()+"' ", con);
                        cmd5.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {
                        erp_log.log_at_av(usr_name.username, ex.ToString());
                        Response.Write("<script lang='JavaScript'>alert('Database baglantısında bir sorun yaşandı lütfen sistem yöneticiniz ile iletişime geçiniz.');</script>");
                        return;
                    }
                }

                SqlCommand cmd5_1 = new SqlCommand(" UPDATE TEKLIFE  SET  DURUM='5' WHERE RTRIM(EVRAKNO)='"+txt_teklifno_kart.Text.Trim()+"' ", con);
                try
                {
                    cmd5_1.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    erp_log.log_at_av(usr_name.username, ex.ToString());
                    Response.Write("<script lang='JavaScript'>alert('Database baglantısında bir sorun yaşandı lütfen sistem yöneticiniz ile iletişime geçiniz.');</script>");
                    return;
                }
                Response.Write("<script>window.open ('siparis_page.aspx?&evrakno=0');</script>");
            }


        }

        private void lbl_onay_info_guncelle()
        {
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string query = " SELECT CONCAT(ONAY_USER,'-', FORMAT (ONAY_DATE,'yyyy-MM-dd HH:mm:ss')) AS DURUM  FROM TEKLIFE    WHERE TRIM(EVRAKNO)='"+txt_teklifno_kart.Text.Trim()+"' ";
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

        protected void revizyon_bul_av(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            if (drp_siparis_malz_kart.SelectedValue.Trim()!="")
            {
                if (drp_siparis_malz_kart.SelectedItem.Text.Trim()!="Yeni Ürün" && drp_siparis_malz_kart.SelectedValue.Trim()!="152 00 01 0001" &&  drp_siparis_malz_kart.SelectedValue.Trim()!="152 00 02 0001")
                {
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        con.Open();
                        string query = " SELECT A.REVIZYON,A.KOD, "+
                                      " CASE WHEN A.REVIZYON='R' THEN 'S' ELSE CONCAT(TRIM(A.REVIZYON),'-',TRIM(A.ACIKLAMA)) END REV_1 FROM STOK_KART_REV A "+
                                      " LEFT OUTER JOIN STOK_KART B ON(TRIM(A.KOD)= TRIM(B.KOD)) "+
                                      " WHERE TRIM(B.KOD)='"+drp_siparis_malz_kart.SelectedValue.Trim()+"' ";
                        SqlCommand cmd = new SqlCommand(query, con);
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        adp.Fill(ds);
                        drp_malzeme_revizyon.Items.Clear();
                        drp_malzeme_revizyon.DataTextField = "REV_1";
                        drp_malzeme_revizyon.DataValueField = "REV_1";
                        drp_malzeme_revizyon.DataSource = ds;
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
        }

        protected void listele_av(object sender, EventArgs e)
        {
            liste_binding_2();
        }

        private void liste_binding_2()
        {
            string query = "";
            string constr2 = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con2 = new SqlConnection(constr2))
            {
                con2.Open();
                query = " SELECT A.*,B.KOD,B.KOD_AD,C.DEPAR_KOD,B.MIKTAR,B.BIRIM,B.TOPLAM_TUTAR,B.PARA_BIRIMI,B.SIPARISNO, " +
                        " CASE WHEN A.DURUM='1' THEN 'Cevap Bekliyor' WHEN A.DURUM='2' THEN 'Müşteri Red Etti' WHEN A.DURUM='3' THEN 'Müşteri Onayladı'  WHEN A.DURUM='4' THEN 'Müşteri Kismi Onaylandi'   WHEN A.DURUM='5' THEN 'Siparişe Dönüştü' END DURUM_AD     "+
                        " FROM TEKLIFE A  " +
                        " LEFT OUTER JOIN TEKLIFT B ON ( TRIM(A.EVRAKNO)=TRIM(B.EVRAKNO) ) " +
                        " LEFT OUTER JOIN STOK_KART C ON ( TRIM(B.KOD)=TRIM(C.KOD) ) "+
                        " WHERE 1=1 ";
                if (txt_siparino_liste.Text.Trim() != "")
                {
                    query = query + "  and  A.EVRAKNO LIKE '%" + txt_siparino_liste.Text.Trim() + "%'";
                }
                if (drp_cari_teklif_liste.SelectedItem.Text.Trim()!="Cari Seçiniz")
                {
                    query = query + "  and  A.CHKODU = '" + drp_cari_teklif_liste.SelectedValue.Trim() + "'";
                }
                if (txt_siparisnot_liste.Text.Trim() != "")
                {
                    query = query + "  and  A.ACIKLAMA LIKE '%" + txt_siparisnot_liste.Text.Trim() + "%'";
                }
                if (chc1.Checked)
                {
                    query = query + "  and  TRIM(A.ONAY)='E' ";
                }
                if (txt_depar_kodu_liste.Text.Trim()!="")
                {
                    query = query + "  and TRIM(C.DEPAR_KOD)='"+txt_depar_kodu_liste.Text.Trim()+"' ";
                }
                if (drp_malzeme_adi.SelectedItem.Text.Trim()!="Malzeme Seçiniz")
                {
                    query = query + "  and TRIM(C.KOD)='"+drp_malzeme_adi.SelectedValue.Trim()+"' ";
                }
                if (datepicker5.Value.Trim()!="Başlangıç Tarihi")
                {
                    if (datepicker5.Value!="")
                    {
                        query = query + "  and  CAST(A.TARIH  as datetime)>=cast('"+datepicker5.Value.Trim()+"' as datetime) ";
                    }
                }
                if (datepicker6.Value.Trim()!="Bitiş Tarihi")
                {
                    if (datepicker6.Value!="")
                    {
                        query = query + "  and  CAST(A.TARIH  as datetime)<=cast('"+datepicker6.Value.Trim()+"' as datetime) ";
                    }
                }

                if (drp_durum_liste.SelectedItem.Text.Trim()!="Durum Seçiniz")
                {
                    query = query + "  and  RTRIM(A.DURUM)='"+drp_durum_liste.SelectedValue.Trim()+"' ";
                }
                query = query + " ORDER BY A.ID DESC ";
                SqlCommand cmd2 = new SqlCommand(query, con2);
                SqlDataAdapter adp2 = new SqlDataAdapter(cmd2);
                DataSet ds2 = new DataSet();
                adp2.Fill(ds2);
                grd_teklif_listesi.DataSource = ds2;
                grd_teklif_listesi.DataBind();

                DataTable dtTemp = new DataTable();
                ViewState["dtbl"] = dtTemp;
                adp2.Fill(dtTemp);

                con2.Close();
            }
            //sipariş liste tabında arama yapınca diger tab'a geçmemesi için çalışasn scripttir.
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ChangeForm(8)", true);
        }

        protected void liste_sec_av(object sender, EventArgs e)
        {
            GridViewRow row = grd_teklif_listesi.SelectedRow;
            String ID = row.Cells[1].Text.Trim();
            String EVRAKNO = row.Cells[2].Text.Trim();

            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string query = " SELECT A.*,CONCAT(ONAY_USER,'-', FORMAT (ONAY_DATE,'yyyy-MM-dd HH:mm:ss')) AS DURUM2 FROM TEKLIFE A " +
                                " WHERE A.ID="+ID+" ";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    txt_teklifno_kart.Text = dr["EVRAKNO"].ToString().Trim();
                    lstbanka.SelectionMode = ListSelectionMode.Multiple;
                    foreach (System.Web.UI.WebControls.ListItem item in lstbanka.Items)
                    {
                        if (item.Value == dr["BANKA2"].ToString().Trim() || item.Value == dr["BANKA"].ToString().Trim() || item.Value == dr["BANKA3"].ToString().Trim())
                        {
                            item.Selected = true;
                        }
                    }
                    datepicker4.Value = Convert.ToDateTime(dr["TARIH"].ToString().Trim()).ToString("yyyy-MM-dd");
                    drp_carikodu_kart.ClearSelection();
                    drp_carikodu_kart.Items.FindByValue(dr["CHKODU"].ToString().Trim()).Selected = true;
                    yetki_bul_av_2();
                    txt_teklifveren_kart.Text=dr["TEKLIF_VEREN"].ToString().Trim();
                    txt_teklifteslimsekli.Text=dr["TESLIM_SEKLI"].ToString().Trim();
                    txt_vade.Text=dr["VADE"].ToString().Trim();
                    txt_ödeme.Text=dr["ODEME"].ToString().Trim();
                    txt_teklifnotlar_kart.Text=dr["NOTLAR"].ToString().Trim();

                    if (dr["MUSTERI_TEMS"].ToString().Trim()=="" || dr["MUSTERI_TEMS"].ToString().Trim()=="0")
                    {
                        drp_must_yetkili.ClearSelection();
                        drp_must_yetkili.Items.FindByValue("Yetkili seçiniz").Selected = true;
                    }
                    else
                    {
                        drp_must_yetkili.ClearSelection();
                        drp_must_yetkili.Items.FindByValue(dr["MUSTERI_TEMS"].ToString().Trim()).Selected = true;
                    }

                    string bank1 = "";
                    string bank2 = "";
                    string bank3 = "";

                    if (dr["BANKA"].ToString().Trim()!="Banka Seçiniz")
                    {
                        bank1=dr["BANKA"].ToString().Trim();
                    }
                    else
                    {
                        bank1="0";
                    }
                    if (dr["BANKA2"].ToString().Trim()!="Banka Seçiniz" || dr["BANKA2"].ToString().Trim()!="")
                    {
                        bank2=dr["BANKA2"].ToString().Trim();
                    }
                    else
                    {
                        bank2="0";
                    }
                    if (dr["BANKA3"].ToString().Trim()!="Banka Seçiniz" || dr["BANKA3"].ToString().Trim()!="")
                    {
                        bank3=dr["BANKA3"].ToString().Trim();
                    }
                    else
                    {
                        bank3="0";
                    }

                    if (bank2=="")
                    {
                        bank2="Banka Seçiniz";
                    }
                    if (bank1=="")
                    {
                        bank1="Banka Seçiniz";
                    }
                    if (bank3=="")
                    {
                        bank3="Banka Seçiniz";
                    }

                    if (dr["DURUM"].ToString().Trim()!="")
                    {
                        string erhan = dr["DURUM"].ToString().Trim();
                        drp_teklif_durum.ClearSelection();
                        drp_teklif_durum.Items.FindByValue(dr["DURUM"].ToString().Trim()).Selected = true;
                        lbl_onay_info.Text=dr["DURUM2"].ToString().Trim();
                    }
                    else
                    {
                        drp_teklif_durum.ClearSelection();
                        drp_teklif_durum.Items.FindByValue("1").Selected = true;
                        lbl_onay_info.Text="";
                    }
                }
                dr.Close();
                con.Close();
            }

            using (SqlConnection con2 = new SqlConnection(constr))
            {
                con2.Open();
                string query2 = " SELECT B.ID,B.KOD," +
                                "  CASE  WHEN B.URUNNO='0' THEN '0' WHEN B.URUNNO='1' THEN  ( SELECT MAX(TRIM(C.URUNNO1)) AS KOD FROM STOK_KART C WHERE B.KOD=C.KOD )"    +
                                " WHEN B.URUNNO='2' THEN(SELECT MAX(TRIM(C.URUNNO2)) AS KOD FROM STOK_KART C WHERE B.KOD=C.KOD) "+
                                " WHEN B.URUNNO='3' THEN(SELECT MAX(TRIM(C.URUNNO3)) AS KOD FROM STOK_KART C WHERE B.KOD=C.KOD) "+
                                " WHEN B.URUNNO='4' THEN(SELECT MAX(TRIM(C.URUNNO4)) AS KOD FROM STOK_KART C WHERE B.KOD=C.KOD) "+
                                " WHEN B.URUNNO='5' THEN(SELECT MAX(TRIM(C.URUNNO5)) AS KOD FROM STOK_KART C WHERE B.KOD=C.KOD) "+
                                " WHEN B.URUNNO='6' THEN(SELECT MAX(TRIM(C.URUNNO6)) AS KOD FROM STOK_KART C WHERE B.KOD=C.KOD) END URUNNO, "+
                                " B.KOD_AD,B.BIRIM,B.REVIZYON,B.FIYAT,B.MIKTAR,B.OLCU,B.TOPLAM_TUTAR,B.TESLIM_TARIHI,B.ACIKLAMA,B.KDV,B.PARA_BIRIMI  FROM TEKLIFT B " +
                                " WHERE TRIM(B.EVRAKNO)='"+EVRAKNO.Trim()+"' ";

                SqlCommand cmd2 = new SqlCommand(query2, con2);
                SqlDataAdapter adp2 = new SqlDataAdapter(cmd2);
                DataSet ds2 = new DataSet();
                adp2.Fill(ds2);
                grd_siparis_detay.DataSource = ds2;
                grd_siparis_detay.DataBind();

                string query3 = " SELECT * FROM  TEKLIFT WHERE TRIM(EVRAKNO)='"+EVRAKNO.Trim()+"'";
                SqlCommand cmd3 = new SqlCommand(query3, con2);
                SqlDataReader dr3 = cmd3.ExecuteReader();
                if (dr3.HasRows)
                {
                    dr3.Read();
                    drp_kdv.ClearSelection();
                    drp_kdv.Items.FindByValue(dr3["KDV"].ToString().Trim()).Selected = true;
                }
                con2.Close();
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ChangeForm(7)", true);
        }

        protected void save_av_2(object sender, EventArgs e)
        {
            //sipariş kayıt öncesi yapılan kontrol işlemleri
            if (txt_teklifno_kart.Text.Trim()=="")
            {
                Response.Write("<script lang='JavaScript'>alert('Teklif no olmadan kayıt yapılamaz. Kayıt yapılmadı.');</script>");
                return;
            }
            if (drp_carikodu_kart.SelectedItem.Text.Trim()=="Cari Seçiniz")
            {
                Response.Write("<script lang='JavaScript'>alert('Teklif müşterisi girilmeden kayıt yapılamaz. Kayıt yapılmadı.');</script>");
                return;
            }
            if (datepicker4.Value.Trim()=="")
            {
                Response.Write("<script lang='JavaScript'>alert('Teklif tarihi girilmeden kayıt yapılamaz. Kayıt yapılmadı.');</script>");
                return;
            }
            if (txt_teklifnotlar_kart.Text.Length >= 400)
            {
                Response.Write("<script lang='JavaScript'>alert('Teklif not kısmında yazılan açıklama 400 karakterden fazla olamaz.Lütfen açıklama alanınızı kısaltınız.Kayıt yapılmadı. ');</script>");
                return;
            }
            if (drp_siparis_birim_kart.SelectedItem.Text.Trim()=="Birim Seçiniz")
            {
                Response.Write("<script lang='JavaScript'>alert('Malzeme birimi girilmeden kayıt yapılamaz.Kayıt yapılmadı. ');</script>");
                return;
            }
            if (drp_siparis_malz_kart.SelectedItem.Text.Trim()!="Yeni Ürün")
            {
                if (drp_malzeme_revizyon.SelectedItem.Text.Trim()=="Revizyon seçiniz")
                {
                    Response.Write("<script lang='JavaScript'>alert('Malzeme revziyonu girilmeden kayıt yapılamaz.Kayıt yapılmadı. ');</script>");
                    return;
                }

            }
            if (txt_ödeme.Text.Length>100)
            {
                Response.Write("<script lang='JavaScript'>alert('Ödeme alanına 100 karakterden uzun bilgi yazamazsınız. Kayıt yapılmadı.');</script>");
                return;
            }
            if (txt_vade.Text.Length>30)
            {
                Response.Write("<script lang='JavaScript'>alert('Vade alanına 30 karakterden uzun bilgi yazamazsınız. Kayıt yapılmadı.');</script>");
                return;
            }

            //3 ADET BANKA KODU KONTOLÜ YAPILIYOR ve seçili olan bankalar alınıyor.
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


            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlCommand cmd2 = new SqlCommand("  SELECT * FROM TEKLIFE A WHERE A.EVRAKNO ='" + txt_teklifno_kart.Text.Trim() + "'", con);
                SqlDataReader dr2 = cmd2.ExecuteReader();
                if (dr2.HasRows)
                {
                    dr2.Close();
                    if (HDN_SATIR_ID.Value.Trim()=="")
                    {
                        SqlCommand cmd4 = new SqlCommand("INSERT INTO TEKLIFT (EVRAKNO,KOD,KOD_AD,TARIH,MIKTAR,BIRIM,FIYAT,PARA_BIRIMI,ACIKLAMA,TOPLAM_TUTAR,REVIZYON,OLCU,URUNNO,KDV) " +
                                                " values(@EVRAKNO,@KOD,@KOD_AD,@TARIH,@MIKTAR,@BIRIM,@FIYAT,@PARA_BIRIMI,@ACIKLAMA,@TOPLAM_TUTAR,@REVIZYON,@OLCU,@URUNNO,@KDV) ", con);
                        cmd4.Parameters.AddWithValue("@EVRAKNO", txt_teklifno_kart.Text.Trim());
                        cmd4.Parameters.AddWithValue("@KOD", drp_siparis_malz_kart.SelectedValue.Trim());
                        cmd4.Parameters.AddWithValue("@KOD_AD", txt_siparismalzemeadi_kart.Text.Trim());

                        string iDate = Request.Form["ctl00$ContentPlaceHolder1$datepicker4"];
                        DateTime oDate = DateTime.Parse(iDate);
                        cmd4.Parameters.AddWithValue("@TARIH", iDate);
                        decimal sip_miktar = Convert.ToDecimal(txt_siparismiktar_kart.Text.ToString().Trim());
                        cmd4.Parameters.AddWithValue("@MIKTAR", sip_miktar);
                        decimal birim_fiyat = Convert.ToDecimal(txt_birimfiyat_kart.Text.ToString().Trim());
                        cmd4.Parameters.AddWithValue("@FIYAT", birim_fiyat);
                        cmd4.Parameters.AddWithValue("@BIRIM", drp_siparis_birim_kart.SelectedItem.Text.Trim());
                        cmd4.Parameters.AddWithValue("@TOPLAM_TUTAR", birim_fiyat*sip_miktar);
                        string revizyon_info = "";
                        if (drp_malzeme_revizyon.SelectedValue.ToString().Trim()!="Revizyon seçiniz")
                        {
                            revizyon_info = drp_malzeme_revizyon.SelectedValue.ToString().Trim();
                        }
                        cmd4.Parameters.AddWithValue("@REVIZYON", revizyon_info);
                        cmd4.Parameters.AddWithValue("@PARA_BIRIMI", drp_parabirimi_kart.SelectedItem.Text.Trim());
                        cmd4.Parameters.AddWithValue("@OLCU", txt_olcu.Text.Trim());
                        cmd4.Parameters.AddWithValue("@ACIKLAMA", txt_siparis_satırnot_kart.Text.Trim());
                        cmd4.Parameters.AddWithValue("@KDV", drp_kdv.SelectedValue.Trim());
                        if (drp_siparis_malz_kart.SelectedItem.Text.Trim()!="Yeni Ürün")
                        {
                            cmd4.Parameters.AddWithValue("@URUNNO", drp_urunno.SelectedValue.Trim());
                        }

                        else
                        {
                            cmd4.Parameters.AddWithValue("@URUNNO", "0");
                        }
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

                    else
                    {
                        //decimal sip_miktar2 = Convert.ToDecimal(txt_siparismiktar_kart.Text.ToString().Trim().Replace(',','.'));
                        decimal sip_miktar2 = decimal.Parse(txt_siparismiktar_kart.Text.ToString(), new CultureInfo("tr-TR"));
                        decimal birim_fiyat2 = decimal.Parse(txt_birimfiyat_kart.Text.ToString(), new CultureInfo("tr-TR"));

                        //decimal birim_fiyat2 = Convert.ToDecimal(txt_birimfiyat_kart.Text.ToString().Trim().Replace(',', '.'));
                        SqlCommand cmd4 = new SqlCommand(" UPDATE TEKLIFT SET KOD='"+drp_siparis_malz_kart.SelectedValue.Trim()+"',KOD_AD='"+txt_siparismalzemeadi_kart.Text.Trim()+"'," +
                                                        " MIKTAR='"+sip_miktar2.ToString().Replace(',', '.')+"',OLCU='"+txt_olcu.Text.ToString().Trim()+"', " +
                                                        " FIYAT='"+birim_fiyat2.ToString().Replace(',', '.')+"',BIRIM='"+drp_siparis_birim_kart.SelectedItem.Text.Trim()+"'," +
                                                        " ACIKLAMA='"+txt_siparis_satırnot_kart.Text.Trim()+"',PARA_BIRIMI='"+drp_parabirimi_kart.SelectedItem.Text.Trim()+ "', "+
                                                        " TOPLAM_TUTAR='"+(sip_miktar2*birim_fiyat2).ToString().Replace(',', '.')+"',KDV='"+drp_kdv.SelectedValue.Trim()+"'"+
                                                        " WHERE ID='"+HDN_SATIR_ID.Value.Trim()+"'  ", con);
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
                    //decimal birim_fiyat = 0;
                    //decimal sip_miktar = 0;
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

                    dr2.Close();

                    SqlCommand cmd3 = new SqlCommand("INSERT INTO TEKLIFE (EVRAKNO,BANKA,BANKA2,BANKA3,TARIH,CHKODU,CHKODU_AD,TEKLIF_VEREN,MUSTERI_TEMS,TESLIM_SEKLI,CREATE_USER,CREATE_DATE,ODEME,VADE,NOTLAR )  " +
                                            " values(@EVRAKNO,@BANKA,@BANKA2,@BANKA3,@TARIH,@CHKODU,@CHKODU_AD,@TEKLIF_VEREN,@MUSTERI_TEMS,@TESLIM_SEKLI,@CREATE_USER,@CREATE_DATE,@ODEME,@VADE,@NOTLAR) ", con);
                    cmd3.Parameters.AddWithValue("@EVRAKNO", txt_teklifno_kart.Text.Trim());
                    string iDate = Request.Form["ctl00$ContentPlaceHolder1$datepicker4"];
                    DateTime oDate = DateTime.Parse(iDate);
                    cmd3.Parameters.AddWithValue("@TARIH", iDate);
                    cmd3.Parameters.AddWithValue("@CHKODU", drp_carikodu_kart.SelectedValue.Trim());
                    cmd3.Parameters.AddWithValue("@CHKODU_AD", drp_carikodu_kart.SelectedItem.Text.Trim());
                    cmd3.Parameters.AddWithValue("@TEKLIF_VEREN", txt_teklifveren_kart.Text.Trim());
                    cmd3.Parameters.AddWithValue("@TESLIM_SEKLI", txt_teklifteslimsekli.Text.Trim());
                    cmd3.Parameters.AddWithValue("@CREATE_USER", usr_name.username_full.Trim());
                    cmd3.Parameters.AddWithValue("@CREATE_DATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd3.Parameters.AddWithValue("@VADE", txt_vade.Text.Trim());
                    cmd3.Parameters.AddWithValue("@ODEME", txt_ödeme.Text.Trim());
                    cmd3.Parameters.AddWithValue("@NOTLAR", txt_teklifnotlar_kart.Text.Trim());
                    cmd3.Parameters.AddWithValue("@BANKA", banka1.Trim());
                    cmd3.Parameters.AddWithValue("@BANKA2", banka2.Trim());
                    cmd3.Parameters.AddWithValue("@BANKA3", banka3.Trim());
                    if (drp_must_yetkili.SelectedItem.Text.Trim()!="Yetkili seçiniz")
                    {
                        cmd3.Parameters.AddWithValue("@MUSTERI_TEMS", drp_must_yetkili.SelectedValue.Trim());
                    }
                    else
                    {
                        cmd3.Parameters.AddWithValue("@MUSTERI_TEMS", "0");
                    }


                    try
                    {
                        cmd3.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        log_at_av(ex.ToString().Trim());
                        Response.Write("<script lang='JavaScript'>alert('Database baglantısında bir sorun yaşandı lütfen sistem yöneticiniz ile iletişime geçiniz.');</script>");
                        return;
                    }
                    //ilk sipariş kaydını yaparken hem e tablosunu hemde t tablosunu birlikte dolduracaktır.
                    dr2.Close();

                    SqlCommand cmd4 = new SqlCommand(" INSERT INTO TEKLIFT (EVRAKNO,KOD,KOD_AD,TARIH,MIKTAR,BIRIM,FIYAT,PARA_BIRIMI,ACIKLAMA,TOPLAM_TUTAR,REVIZYON,OLCU,URUNNO,KDV)  " +
                                                     " values(@EVRAKNO,@KOD,@KOD_AD,@TARIH,@MIKTAR,@BIRIM,@FIYAT,@PARA_BIRIMI,@ACIKLAMA,@TOPLAM_TUTAR,@REVIZYON,@OLCU,@URUNNO,@KDV) ", con);
                    cmd4.Parameters.AddWithValue("@EVRAKNO", txt_teklifno_kart.Text.Trim());
                    cmd4.Parameters.AddWithValue("@KOD", drp_siparis_malz_kart.SelectedValue.Trim());
                    cmd4.Parameters.AddWithValue("@KOD_AD", txt_siparismalzemeadi_kart.Text.Trim());
                    string iDate2 = Request.Form["ctl00$ContentPlaceHolder1$datepicker4"];
                    DateTime oDat2e = DateTime.Parse(iDate2);
                    cmd4.Parameters.AddWithValue("@TARIH", iDate2);
                    decimal sip_miktar2 = Convert.ToDecimal(txt_siparismiktar_kart.Text.ToString().Trim());
                    cmd4.Parameters.AddWithValue("@MIKTAR", sip_miktar2);
                    decimal birim_fiyat2 = Convert.ToDecimal(txt_birimfiyat_kart.Text.ToString().Trim());
                    cmd4.Parameters.AddWithValue("@FIYAT", birim_fiyat2);
                    cmd4.Parameters.AddWithValue("@BIRIM", drp_siparis_birim_kart.SelectedItem.Text.Trim());
                    cmd4.Parameters.AddWithValue("@TOPLAM_TUTAR", birim_fiyat2*sip_miktar2);
                    cmd4.Parameters.AddWithValue("@KDV", drp_kdv.SelectedValue.Trim());
                    string revizyon_info = "";
                    if (drp_malzeme_revizyon.SelectedValue.ToString().Trim()!="Revizyon seçiniz")
                    {
                        revizyon_info = drp_malzeme_revizyon.SelectedValue.ToString().Trim();
                    }
                    cmd4.Parameters.AddWithValue("@REVIZYON", revizyon_info);
                    cmd4.Parameters.AddWithValue("@PARA_BIRIMI", drp_parabirimi_kart.SelectedItem.Text.Trim());
                    cmd4.Parameters.AddWithValue("@OLCU", txt_olcu.Text.Trim());
                    cmd4.Parameters.AddWithValue("@ACIKLAMA", txt_siparis_satırnot_kart.Text.Trim());
                    if (drp_siparis_malz_kart.SelectedItem.Text.Trim()!="Yeni Ürün")
                    {
                        cmd4.Parameters.AddWithValue("@URUNNO", drp_urunno.SelectedValue.Trim());
                    }
                    else
                    {
                        cmd4.Parameters.AddWithValue("@URUNNO", "0");
                    }

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
                con.Close();
            }
            sip_detay_liste_cari_bindign();
            alanlari_temizle();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ChangeForm(7)", true);
        }

        private void alanlari_temizle()
        {
            drp_siparis_malz_kart.ClearSelection();
            drp_siparis_malz_kart.Items.FindByText("Malzeme Seçiniz").Selected = true;
            drp_malzeme_revizyon.ClearSelection();
            HDN_SATIR_ID.Value="";
            txt_siparismalzemeadi_kart.Text="";
            txt_olcu.Text="";
            txt_siparismiktar_kart.Text="";
            drp_siparis_birim_kart.ClearSelection();
            drp_siparis_birim_kart.Items.FindByValue("5").Selected = true;
            txt_birimfiyat_kart.Text="";
            drp_parabirimi_kart.ClearSelection();
            drp_parabirimi_kart.Items.FindByText("USD").Selected = true;
            drp_urunno.ClearSelection();
            drp_urunno.Items.FindByText("Bilgi seçiniz").Selected = true;
        }

        private void sip_detay_liste_cari_bindign()
        {
            string query = "";
            string constr2 = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con2 = new SqlConnection(constr2))
            {
                if (drp_siparis_malz_kart.SelectedItem.Text.Trim()!="Yeni Ürün")
                {
                    query = " SELECT A.ID,A.KOD,CASE  WHEN A.KDV=1 THEN '18' WHEN A.KDV='2' THEN '8' WHEN A.KDV='3' THEN '1' WHEN A.KDV='4' THEN 'YOK' END KDV," +
                        " CASE WHEN A.URUNNO='1' THEN  ( SELECT MAX(TRIM(B.URUNNO1)) AS KOD FROM STOK_KART B WHERE A.KOD=B.KOD )"    +
                        " WHEN A.URUNNO='2' THEN (SELECT MAX(TRIM(B.URUNNO2)) AS KOD FROM STOK_KART B WHERE A.KOD=B.KOD) "+
                        " WHEN A.URUNNO='3' THEN (SELECT MAX(TRIM(B.URUNNO3)) AS KOD FROM STOK_KART B WHERE A.KOD=B.KOD) "+
                        " WHEN A.URUNNO='4' THEN (SELECT MAX(TRIM(B.URUNNO4)) AS KOD FROM STOK_KART B WHERE A.KOD=B.KOD) "+
                        " WHEN A.URUNNO='5' THEN (SELECT MAX(TRIM(B.URUNNO5)) AS KOD FROM STOK_KART B WHERE A.KOD=B.KOD) "+
                        " WHEN A.URUNNO='6' THEN (SELECT MAX(TRIM(B.URUNNO6)) AS KOD FROM STOK_KART B WHERE A.KOD=B.KOD) END URUNNO, "+
                        " A.KOD_AD,A.REVIZYON,A.BIRIM,A.FIYAT,A.MIKTAR,A.OLCU,A.TOPLAM_TUTAR,A.TESLIM_TARIHI,A.ACIKLAMA,A.PARA_BIRIMI " +
                        " FROM TEKLIFT A  WHERE TRIM(A.EVRAKNO)='"+txt_teklifno_kart.Text.Trim()+"'";
                }
                else
                {
                    query = " SELECT A.ID,A.KOD,A.KDV, "+
                        " A.KOD_AD,A.REVIZYON,A.BIRIM,A.FIYAT,A.MIKTAR,A.OLCU,A.TOPLAM_TUTAR,A.TESLIM_TARIHI,A.ACIKLAMA,'0' AS URUNNO,A.PARA_BIRIMI " +
                        " FROM TEKLIFT A  WHERE TRIM(A.EVRAKNO)='"+txt_teklifno_kart.Text.Trim()+"'";
                }

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

        protected void delete_satir_av(object sender, GridViewDeleteEventArgs e)
        {
            int SID = Convert.ToInt32(grd_siparis_detay.DataKeys[e.RowIndex].Value);
            string constr3 = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con3 = new SqlConnection(constr3))
            {
                con3.Open();
                SqlCommand cmd = new SqlCommand("Delete From TEKLIFT where ID='" + SID + "'", con3);
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
                string query = " SELECT * FROM TEKLIFT A WHERE A.ID="+ID+" ";

                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    HDN_SATIR_ID.Value=dr["ID"].ToString().Trim();
                    if (dr["KOD"].ToString().Trim()=="")
                    {
                        drp_siparis_malz_kart.ClearSelection();
                        drp_siparis_malz_kart.Items.FindByValue("Malzeme Seçiniz").Selected = true;
                    }
                    else
                    {
                        drp_siparis_malz_kart.ClearSelection();
                        drp_siparis_malz_kart.Items.FindByValue(dr["KOD"].ToString().Trim()).Selected = true;
                    }

                    HDN_SATIR_ID.Value=dr["ID"].ToString().Trim();
                    txt_siparismalzemeadi_kart.Text=dr["KOD_AD"].ToString().Trim();
                    txt_olcu.Text=dr["OLCU"].ToString().Trim();
                    txt_siparismiktar_kart.Text=dr["MIKTAR"].ToString().Trim();
                    txt_siparis_satırnot_kart.Text=dr["ACIKLAMA"].ToString().Trim();

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
                    txt_birimfiyat_kart.Text=dr["FIYAT"].ToString().Trim();

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

                    if (dr["URUNNO"].ToString().Trim()=="" || dr["URUNNO"].ToString().Trim()=="0")
                    {
                        drp_urunno.ClearSelection();
                        drp_urunno.Items.FindByText("Üretici Parça No").Selected = true;
                    }
                    else
                    {
                        drp_urunno.ClearSelection();
                        drp_urunno.Items.FindByValue(dr["URUNNO"].ToString().Trim()).Selected = true;
                    }


                }
                dr.Close();
                con.Close();
            }

        }

        protected void update_av(object sender, EventArgs e)
        {
            if (txt_teklifno_kart.Text.Trim()=="")
            {
                Response.Write("<script lang='JavaScript'>alert('Teklif no olmadan güncelleme yapılamaz. Kayıt yapılmadı.');</script>");
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
                string TEMSILCI = "0";
                if (drp_must_yetkili.SelectedItem.Text.Trim()!="Yetkili seçiniz")
                {
                    TEMSILCI= drp_must_yetkili.SelectedValue.Trim();
                }
                con2.Open();
                string query = " UPDATE TEKLIFE  SET TARIH='"+datepicker4.Value+"',VADE='"+txt_vade.Text.Trim()+"'," +
                               " ODEME='"+txt_ödeme.Text.Trim()+"',NOTLAR='"+txt_teklifnotlar_kart.Text.Trim()+"', "+
                               " TEKLIF_VEREN='"+txt_teklifveren_kart.Text.Trim()+"', " +
                               " MUSTERI_TEMS='"+TEMSILCI.Trim()+"', TESLIM_SEKLI='"+txt_teklifteslimsekli.Text.Trim()+"'," +
                               " BANKA='"+banka1.Trim()+"',BANKA2='"+banka2.Trim()+"',BANKA3='"+banka3.Trim()+"', " +
                               " REV_USER='"+usr_name.username.Trim()+"',REV_DATE='"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' " +
                               " WHERE EVRAKNO='"+txt_teklifno_kart.Text.Trim()+"' ";
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

                string query2 = " UPDATE TEKLIFT SET KDV='"+drp_kdv.SelectedValue.Trim()+"'  WHERE EVRAKNO='"+txt_teklifno_kart.Text.Trim()+"' ";
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
                con2.Close();
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ChangeForm(7)", true);
        }

        protected void durum_change_av(object sender, EventArgs e)
        {
            if (txt_teklifno_kart.Text.Trim()=="")
            {
                drp_teklif_durum.ClearSelection();
                drp_teklif_durum.Items.FindByText(usr_name.siparis_durum.Trim()).Selected = true;
                return;
            }
            if (usr_name.username_role!="ADMIN")
            {
                drp_teklif_durum.ClearSelection();
                drp_teklif_durum.Items.FindByText(usr_name.siparis_durum.Trim()).Selected = true;
                Response.Write("<script lang='JavaScript'>alert('Müşteri teklif  durumunu güncellemek için yetkili değilsiniz. Kayıt yapılmadı.');</script>");
                return;
            }
            else
            {
                string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    if (drp_teklif_durum.SelectedValue.Trim()!="")
                    {
                        string query = " UPDATE TEKLIFE SET DURUM='"+drp_teklif_durum.SelectedValue.Trim()+"',ONAY_USER='"+usr_name.username.Trim()+"',ONAY_DATE='"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"'  WHERE EVRAKNO='"+txt_teklifno_kart.Text.Trim()+"'  ";
                        SqlCommand cmd = new SqlCommand(query, con);
                        try
                        {
                            cmd.ExecuteNonQuery();
                            lbl_onay_info_guncelle();
                        }
                        catch (Exception)
                        {
                            drp_teklif_durum.ClearSelection();
                            drp_teklif_durum.Items.FindByText(usr_name.siparis_durum.Trim()).Selected = true;
                            Response.Write("<script lang='JavaScript'>alert('Müşteri onay durumunu güncellenemedi lütfen sistem yöneticiniz ile iletişime geçiniz. Kayıt yapılmadı !!!.');</script>");
                            throw;
                        }
                    }
                    con.Close();
                }

            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ChangeForm(7)", true);
        }

        protected void btn_Click(object sender, EventArgs e)
        {
            int toplam = lstbanka.Items.Count;
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
                Response.Write("<script lang='JavaScript'>alert('3 Adetden daha fazla banka seçimi yapılmaz.');</script>");
                return;
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
                    txt_ödeme.Text="Payment:30 % in advance, the rest before shipping";
                    txt_teklifnotlar_kart.Text=HttpUtility.HtmlDecode("Con Rod and Main Bearings manufacturing unit price offer for the **** machine is presented below for your information");
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

        protected void grd_siparis_detay_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string item = e.Row.Cells[0].Text;
                foreach (Button button in e.Row.Cells[1].Controls.OfType<Button>())
                {
                    if (button.CommandName == "Delete")
                    {
                        button.Attributes["onclick"] = "if(!confirm('Satırı silmek istiyor musunuz? ')){ return false; };";
                    }
                }
            }

        }

        protected void SORT_AV(object sender, GridViewSortEventArgs e)
        {
            DataTable dataTable = ViewState["dtbl"] as DataTable;
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                dataView.Sort = e.SortExpression + " " + ConvertSortDirection(e.SortDirection);
                grd_teklif_listesi.DataSource = dataView;
                grd_teklif_listesi.DataBind();
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ChangeForm(8)", true);
        }


        private string ConvertSortDirection(SortDirection sortDirection)
        {
            string newSortDirection = String.Empty;
            switch (sortDirection)
            {
                case SortDirection.Ascending:
                    newSortDirection = "DESC";
                    break;
                case SortDirection.Descending:
                    newSortDirection = "ASC";
                    break;
            }
            return newSortDirection;
        }

        protected void PAGE_AV(object sender, GridViewPageEventArgs e)
        {
            liste_binding_2();
            grd_teklif_listesi.PageIndex = e.NewPageIndex;
            grd_teklif_listesi.DataBind();
        }

        protected void print_av3(object sender, EventArgs e)
        {
            if (txt_teklifno_kart.Text.Trim()=="")
            {
                Response.Write("<script lang='JavaScript'>alert('Teklif no seçilmeden teklif formu alınamaz.');</script>");
                return;
            }

            string firma_adi = "";
            string firma_kod = "";
            string teklif_tarihi = "";
            string teklif_veren = "";
            string firma_adres = "";
            string firma_tel = "";
            string firma_mail = "";
            string teklif_notlar = "";
            string odeme = "";
            string teslim_sekli = "";
            string musteri_tems = "";


            string banka = "";
            string iban = "";
            string hesap_ad = "";

            string banka2 = "";
            string iban2 = "";
            string hesap_ad2 = "";

            string banka3 = "";
            string iban3 = "";
            string hesap_ad3 = "";
            string bank1 = "";
            string bank2 = "";
            string bank3 = "";
            decimal KDV = 0;


            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string query = " SELECT B.BANKA,B.HESAP_AD,B.IBAN,C.BANKA AS BANKA2,C.HESAP_AD AS HESAP_AD2,C.IBAN AS IBAN2 ,D.BANKA AS BANKA3,D.HESAP_AD AS HESAP_AD3 ,D.IBAN AS IBAN3," +
                               " A.MUSTERI_TEMS AS MUSTERI_TEMS_KOD,(SELECT MAX(YETKILI) AS YETKILI FROM CARI_YETKILI X  WHERE X.ID=RTRIM(A.MUSTERI_TEMS)  ) AS MUSTERI_TEMS,A.NOTLAR,A.ODEME,A.CHKODU AS CHKODU,A.CHKODU_AD AS CHKODU_AD,A.TARIH AS TARIH,A.TEKLIF_VEREN AS TEKLIF_VEREN,A.TESLIM_SEKLI FROM TEKLIFE A " +
                               " LEFT OUTER JOIN BANKA B ON ( TRIM(A.BANKA)=B.ID )  " +
                               " LEFT OUTER JOIN BANKA C ON ( TRIM(A.BANKA2)=C.ID )  " +
                               " LEFT OUTER JOIN BANKA D ON ( TRIM(A.BANKA3)=D.ID )  " +
                               " WHERE A.EVRAKNO='"+txt_teklifno_kart.Text.Trim()+"' ";

                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    firma_adi = dr["CHKODU_AD"].ToString().Trim();
                    firma_kod = dr["CHKODU"].ToString().Trim();
                    teklif_tarihi = Convert.ToDateTime(dr["TARIH"].ToString().Trim()).ToString("yyyy-MM-dd");
                    teklif_veren=dr["TEKLIF_VEREN"].ToString().Trim();
                    teklif_notlar = dr["NOTLAR"].ToString().Trim();
                    odeme = dr["ODEME"].ToString().Trim();
                    teslim_sekli = dr["TESLIM_SEKLI"].ToString().Trim();
                    if (dr["MUSTERI_TEMS_KOD"].ToString().Trim()=="0" || dr["MUSTERI_TEMS_KOD"].ToString().Trim()=="")
                    {
                        musteri_tems = "Yetkili";
                    }
                    else
                    {
                        musteri_tems = dr["MUSTERI_TEMS"].ToString().Trim();
                    }


                    if (dr["BANKA"].ToString().Trim()!="")
                    {
                        banka = dr["BANKA"].ToString().Trim();
                        iban = dr["IBAN"].ToString().Trim();
                        hesap_ad = dr["HESAP_AD"].ToString().Trim();
                        bank1= " Banka: "+ banka.Trim()+" / IBAN:" + iban.Trim() + " / Hesap Adı: "+hesap_ad.Trim();
                    }

                    if (dr["BANKA2"].ToString().Trim()!="")
                    {
                        banka2 = dr["BANKA2"].ToString().Trim();
                        iban2 = dr["IBAN2"].ToString().Trim();
                        hesap_ad2 = dr["HESAP_AD2"].ToString().Trim();
                        bank2= " Banka: "+ banka2.Trim()+" / IBAN:" + iban2.Trim() + " / Hesap Adı: "+hesap_ad2.Trim();
                    }

                    if (dr["BANKA3"].ToString().Trim()!="")
                    {
                        banka3 = dr["BANKA3"].ToString().Trim();
                        iban3 = dr["IBAN3"].ToString().Trim();
                        hesap_ad3 = dr["HESAP_AD3"].ToString().Trim();
                        bank3= " Banka: "+ banka3.Trim()+" / IBAN:" + iban3.Trim() + " / Hesap Adı: "+hesap_ad3.Trim();
                    }

                }
                dr.Close();

                string query2 = " SELECT * FROM CARI WHERE TRIM(CARI_KOD)='"+firma_kod.Trim()+"' ";
                SqlCommand cmd2 = new SqlCommand(query2, con);
                SqlDataReader dr2 = cmd2.ExecuteReader();
                if (dr2.HasRows)
                {
                    dr2.Read();
                    firma_adres = dr2["FAT_ADRES"].ToString().Trim();
                    firma_tel = "Tel: " + dr2["TEL"].ToString().Trim();
                    firma_mail = "Email: "+ dr2["EMAIL"].ToString().Trim();

                }
                dr.Close();
                con.Close();
            }




            using (MemoryStream ms = new MemoryStream())
            {
                //*************************** HEADER BİLGİLERİ YAZDIRILIYOR
                //Document document = new Document(iTextSharp.text.PageSize.A4.Rotate(), 25, 25, 30, 30);
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/depar_logo.PNG"));
                img.ScaleToFit(150, 150);
                img.SetAbsolutePosition(50, 760);
                img.Border = 0;
                img.BorderColor = iTextSharp.text.BaseColor.BLACK;
                img.BorderWidth = 5f;
                document.Add(img);

                iTextSharp.text.Image IMGISO9001 = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/ISO9001.png"));
                IMGISO9001.ScaleToFit(100, 100);
                IMGISO9001.SetAbsolutePosition(420, 750);
                //IMGISO9001.Border = iTextSharp.text.Rectangle.BOX;
                IMGISO9001.Border = 0;
                IMGISO9001.BorderColor = iTextSharp.text.BaseColor.BLACK;
                IMGISO9001.BorderWidth = 1f;
                document.Add(IMGISO9001);


                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));

                BaseFont bF = BaseFont.CreateFont("C:\\Windows\\Fonts\\Arial.ttf", "windows-1254", true);

                PdfPTable table = new PdfPTable(3);
                table.WidthPercentage = 100;
                PdfPCell cell = new PdfPCell(new Phrase("PROFORMA FATURA", new iTextSharp.text.Font(bF, 12f, iTextSharp.text.Font.BOLD)));
                cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                table.AddCell(cell);
                document.Add(table);


                table = new PdfPTable(9);
                table.WidthPercentage = 100;
                table.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;

                cell = new PdfPCell();
                PdfPCell cell211 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 12f, iTextSharp.text.Font.NORMAL)));
                cell211.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell211.Border = 0;
                cell211.Colspan = 7;
                table.AddCell(cell211);

                cell = new PdfPCell();
                PdfPCell cell21 = new PdfPCell(new Phrase("Fatura No:", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell21.HorizontalAlignment = Element.ALIGN_LEFT;
                cell21.Border = 0;
                table.AddCell(cell21);

                PdfPCell cell22 = new PdfPCell(new Phrase(txt_teklifno_kart.Text.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell22.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell22.Border = 0;
                table.AddCell(cell22);

                PdfPCell cell322 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 12f, iTextSharp.text.Font.NORMAL)));
                cell322.HorizontalAlignment = Element.ALIGN_LEFT;
                cell322.Border = 0;
                cell322.Colspan = 7;
                table.AddCell(cell322);

                PdfPCell cell32 = new PdfPCell(new Phrase("Fatura Tarihi", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell32.HorizontalAlignment = Element.ALIGN_LEFT;
                cell32.Border = 0;
                table.AddCell(cell32);

                PdfPCell cell33 = new PdfPCell(new Phrase(teklif_tarihi, new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell33.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell33.Border = 0;
                table.AddCell(cell33);
                document.Add(table);
                document.Add(new Paragraph(" "));


                //***************************************** CARİLER BİLGİELRİ

                table = new PdfPTable(5);
                table.WidthPercentage = 100;

                PdfPCell cell70 = new PdfPCell(new Phrase("DEPAR MOTORLU ARAÇLAR YATAKLARI BURÇLARI VE İNŞ. SAN.TİC.LTD.ŞTİ", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell70.HorizontalAlignment = Element.ALIGN_LEFT;
                cell70.Colspan = 2;
                cell70.Border = 0;
                table.AddCell(cell70);

                PdfPCell cell71 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 15f, iTextSharp.text.Font.NORMAL)));
                cell71.HorizontalAlignment = Element.ALIGN_CENTER;
                cell71.Border = 0;
                table.AddCell(cell71);


                PdfPCell cell72 = new PdfPCell(new Phrase(firma_adi.ToString().ToUpper(), new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell72.HorizontalAlignment = Element.ALIGN_LEFT;
                cell72.Colspan = 2;
                cell72.Border = 0;
                table.AddCell(cell72);




                PdfPCell cell74 = new PdfPCell(new Phrase(teklif_veren.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell74.HorizontalAlignment = Element.ALIGN_LEFT;
                cell74.Colspan = 2;
                cell74.Border = 0;
                table.AddCell(cell74);

                PdfPCell cell75 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell75.HorizontalAlignment = Element.ALIGN_CENTER;
                cell75.Border = 0;
                table.AddCell(cell75);


                PdfPCell cell76 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell76.HorizontalAlignment = Element.ALIGN_LEFT;
                cell76.Colspan = 2;
                cell76.Border = 0;
                table.AddCell(cell76);


                PdfPCell cell77 = new PdfPCell(new Phrase("Makine Mühendisi", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell77.HorizontalAlignment = Element.ALIGN_LEFT;
                cell77.Colspan = 2;
                cell77.Border = 0;
                table.AddCell(cell77);

                PdfPCell cell78 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell78.HorizontalAlignment = Element.ALIGN_CENTER;
                cell78.Border = 0;
                table.AddCell(cell78);


                PdfPCell cell79 = new PdfPCell(new Phrase(firma_adres.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL))); ;
                cell79.HorizontalAlignment = Element.ALIGN_LEFT;
                cell79.Colspan = 2;
                cell79.Border = 0;
                table.AddCell(cell79);

                PdfPCell cell77_1 = new PdfPCell(new Phrase("sedat@deparbearings.com", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell77_1.HorizontalAlignment = Element.ALIGN_LEFT;
                cell77_1.Colspan = 2;
                cell77_1.Border = 0;
                table.AddCell(cell77_1);

                PdfPCell cell78_1 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell78_1.HorizontalAlignment = Element.ALIGN_CENTER;
                cell78_1.Border = 0;
                table.AddCell(cell78_1);


                PdfPCell cell79_1 = new PdfPCell(new Phrase(firma_tel.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL))); ;
                cell79_1.HorizontalAlignment = Element.ALIGN_LEFT;
                cell79_1.Colspan = 2;
                cell79_1.Border = 0;
                table.AddCell(cell79_1);


                PdfPCell cell80 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell80.HorizontalAlignment = Element.ALIGN_LEFT;
                cell80.Colspan = 2;
                cell80.Border = 0;
                table.AddCell(cell80);

                PdfPCell cell81 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                cell81.HorizontalAlignment = Element.ALIGN_CENTER;
                cell81.Border = 0;
                table.AddCell(cell81);


                PdfPCell cell82 = new PdfPCell(new Phrase(firma_mail.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL))); ;
                cell82.HorizontalAlignment = Element.ALIGN_LEFT;
                cell82.Colspan = 2;
                cell82.Border = 0;
                table.AddCell(cell82);
                document.Add(table);


                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));

                table = new PdfPTable(1);
                table.WidthPercentage = 100;


                PdfPCell cell891 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell891.HorizontalAlignment = Element.ALIGN_LEFT;
                cell891.Border = 0;
                table.AddCell(cell891);

                PdfPCell cell89 = new PdfPCell(new Phrase(HttpUtility.HtmlDecode(""), new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell89.HorizontalAlignment = Element.ALIGN_LEFT;
                cell89.Border = 0;
                table.AddCell(cell89);
                document.Add(table);


                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));


                table = new PdfPTable(13);
                table.WidthPercentage = 100;

                PdfPCell cell90 = new PdfPCell(new Phrase("D.No", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell90.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell90);

                PdfPCell cell190 = new PdfPCell(new Phrase("Par./Marka/Model", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell190.HorizontalAlignment = Element.ALIGN_LEFT;
                cell190.Colspan = 4;
                table.AddCell(cell190);

                PdfPCell cell96 = new PdfPCell(new Phrase("Ölçü", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell96.HorizontalAlignment = Element.ALIGN_CENTER;

                table.AddCell(cell96);

                PdfPCell cell91 = new PdfPCell(new Phrase("Miktar", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell91.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell91);

                PdfPCell cell92 = new PdfPCell(new Phrase("Birim", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell92.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell92);

                PdfPCell cell93 = new PdfPCell(new Phrase("B.Fiyat", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell93.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell93);

                PdfPCell cell94 = new PdfPCell(new Phrase("P.Birimi", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell94.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell94);

                PdfPCell cell95 = new PdfPCell(new Phrase("Toplam Tutar", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell95.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell95);

                PdfPCell cell97 = new PdfPCell(new Phrase("Açıklama", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                cell97.HorizontalAlignment = Element.ALIGN_CENTER;
                cell97.Colspan = 2;

                table.AddCell(cell97);
                document.Add(table);


                string kod = "";
                string kod_ad = "";
                string kod_ad2 = "";
                string kod_ad3 = "";
                decimal miktar = 0;
                string birim = "";
                decimal fiyat = 0;
                string p_birim = "";
                decimal t_tutar = 0;
                string olcu = "";
                string note = "";
                string sembol = "";

                decimal alt_toplam = 0;
                decimal alt_toplam2 = 0;
                decimal alt_toplam_kdv = 0;
                decimal genel_toplam = 0;


                foreach (GridViewRow gvrow in grd_siparis_detay.Rows)
                {

                    string TEKLIFT_ID = gvrow.Cells[2].Text.Trim();
                    string TEKLIFT_KOD = HttpUtility.HtmlDecode(gvrow.Cells[3].Text.Trim());

                    if (TEKLIFT_KOD.Trim()!="Yeni Ürün")
                    {
                        using (SqlConnection con2 = new SqlConnection(constr))
                        {
                            con2.Open();
                            //string query = " SELECT A.*,CONCAT(TRIM(B.DEPAR_KOD),'-',TRIM(D.GRUPKODU_AD),'-',TRIM(C.GRUPKODU_AD) ) AS DEPAR_KOD FROM TEKLIFT A " + //07-09-2022 REVİZ EDİLDİ
                            string query = " SELECT A.*,TRIM(B.DEPAR_KOD)  AS DEPAR_KOD FROM TEKLIFT A " +
                                          " LEFT OUTER JOIN STOK_KART B ON ( TRIM(A.KOD)=TRIM(B.KOD))   " +
                                          " LEFT OUTER JOIN GRUPKODU C ON ( B.GK_4=C.GRUPKODU AND  C.KOD='GK_4' ) " +
                                          " LEFT OUTER JOIN GRUPKODU D ON ( B.GK_5=D.GRUPKODU  AND  D.KOD='GK_5' ) " +
                                          " WHERE A.ID='"+TEKLIFT_ID.Trim()+"'";
                            SqlCommand cmd2 = new SqlCommand(query, con2);
                            SqlDataReader dr2 = cmd2.ExecuteReader();
                            if (dr2.HasRows)
                            {
                                dr2.Read();
                                kod = dr2["DEPAR_KOD"].ToString().Trim();
                                kod_ad2 = dr2["KOD_AD"].ToString().Trim();
                                birim = dr2["BIRIM"].ToString().Trim();
                                decimal miktar2 = Convert.ToDecimal(dr2["MIKTAR"].ToString().Trim());
                                miktar = Decimal.Round(miktar2, 2);
                                decimal fiyat2 = Convert.ToDecimal(dr2["FIYAT"].ToString().Trim());
                                fiyat = Decimal.Round(fiyat2, 2);
                                p_birim = dr2["PARA_BIRIMI"].ToString().Trim();
                                decimal t_tutar2 = Convert.ToDecimal(dr2["TOPLAM_TUTAR"].ToString().Trim());
                                t_tutar = Decimal.Round(t_tutar2, 2);
                                olcu = dr2["OLCU"].ToString().Trim();
                                note = dr2["ACIKLAMA"].ToString().Trim();
                                KDV = Convert.ToDecimal(dr2["KDV"].ToString().Trim());
                                if (dr2["PARA_BIRIMI"].ToString().Trim()=="USD")
                                {
                                    sembol="$";
                                }
                                else if (dr2["PARA_BIRIMI"].ToString().Trim()=="EUR")
                                {
                                    sembol="€";
                                }
                                else if (dr2["PARA_BIRIMI"].ToString().Trim()=="TL")
                                {
                                    sembol="₺";
                                }
                                else if (dr2["PARA_BIRIMI"].ToString().Trim()=="GBP")
                                {
                                    sembol="£";
                                }
                                else
                                {
                                    sembol="";

                                }
                                dr2.Close();
                                con2.Close();
                            }
                        }
                    }
                    else
                    {
                        using (SqlConnection con_22 = new SqlConnection(constr))
                        {
                            con_22.Open();
                            string query = " SELECT A.*,A.KOD_AD AS DEPAR_KOD FROM TEKLIFT A " +
                                           " WHERE A.ID='"+TEKLIFT_ID.Trim()+"'";
                            SqlCommand cmd_22 = new SqlCommand(query, con_22);
                            SqlDataReader dr_22 = cmd_22.ExecuteReader();
                            if (dr_22.HasRows)
                            {
                                dr_22.Read();
                                kod = " ";
                                kod_ad3 = dr_22["KOD_AD"].ToString().Trim();
                                birim = dr_22["BIRIM"].ToString().Trim();
                                decimal miktar2 = Convert.ToDecimal(dr_22["MIKTAR"].ToString().Trim());
                                miktar = Decimal.Round(miktar2, 2);
                                decimal fiyat2 = Convert.ToDecimal(dr_22["FIYAT"].ToString().Trim());
                                fiyat = Decimal.Round(fiyat2, 2);
                                p_birim = dr_22["PARA_BIRIMI"].ToString().Trim();
                                decimal t_tutar2 = Convert.ToDecimal(dr_22["TOPLAM_TUTAR"].ToString().Trim());
                                t_tutar = Decimal.Round(t_tutar2, 2);
                                olcu = dr_22["OLCU"].ToString().Trim();
                                note = dr_22["ACIKLAMA"].ToString().Trim();
                                KDV = Convert.ToDecimal(dr_22["KDV"].ToString().Trim());

                                if (dr_22["PARA_BIRIMI"].ToString().Trim()=="USD")
                                {
                                    sembol="$";
                                }
                                else if (dr_22["PARA_BIRIMI"].ToString().Trim()=="EUR")
                                {
                                    sembol="€";
                                }
                                else if (dr_22["PARA_BIRIMI"].ToString().Trim()=="TL")
                                {
                                    sembol="₺";
                                }
                                else if (dr_22["PARA_BIRIMI"].ToString().Trim()=="GBP")
                                {
                                    sembol="£";
                                }
                                else
                                {
                                    sembol="";
                                }
                            }
                            dr_22.Close();
                            con_22.Close();
                        }
                    }


                    if (kod_ad2.Trim()!="")
                    {
                        kod_ad= kod_ad2.Trim();
                    }
                    if (kod_ad3.Trim()!="")
                    {
                        kod_ad= kod_ad3.Trim();
                    }


                    table = new PdfPTable(13);

                    table.WidthPercentage = 100;

                    PdfPCell cell100 = new PdfPCell(new Phrase(kod.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cell100.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell100);

                    PdfPCell cell1100 = new PdfPCell(new Phrase(kod_ad.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cell1100.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell1100.Colspan = 4;
                    table.AddCell(cell1100);

                    PdfPCell cell106 = new PdfPCell(new Phrase(olcu.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cell106.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell106);

                    PdfPCell cell101 = new PdfPCell(new Phrase(miktar.ToString().Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cell101.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell101);

                    PdfPCell cell102 = new PdfPCell(new Phrase(birim.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cell102.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell102);

                    PdfPCell cell103 = new PdfPCell(new Phrase(fiyat.ToString().Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cell103.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell103);

                    PdfPCell cell104 = new PdfPCell(new Phrase(p_birim.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL))); ;
                    cell104.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell104);

                    PdfPCell cell105 = new PdfPCell(new Phrase(t_tutar.ToString().Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cell105.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell105);

                    PdfPCell cell107 = new PdfPCell(new Phrase(note.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cell107.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell107.Colspan = 2;

                    table.AddCell(cell107);
                    document.Add(table);
                    alt_toplam2 = alt_toplam2 + t_tutar;
                }
                alt_toplam = Decimal.Round(alt_toplam2, 2);
                alt_toplam_kdv=0;

                if (KDV!=0)
                {
                    alt_toplam_kdv= decimal.Round(alt_toplam*KDV/100, 2);
                    decimal genel_toplam2 = alt_toplam_kdv + alt_toplam;
                    genel_toplam = Decimal.Round(genel_toplam2, 2);


                    table = new PdfPTable(3);
                    table.WidthPercentage = 30;
                    table.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.SpacingBefore = 5f;
                    table.SpacingAfter = 15f;

                    cell = new PdfPCell();
                    PdfPCell cella = new PdfPCell(new Phrase("TOPLAM TUTAR:", new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cella.HorizontalAlignment = Element.ALIGN_LEFT;
                    cella.Border = 0;
                    cella.Colspan = 2;
                    table.AddCell(cella);

                    PdfPCell cella1 = new PdfPCell(new Phrase(alt_toplam.ToString().Trim() + " " + sembol.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cella1.HorizontalAlignment = Element.ALIGN_LEFT;
                    cella1.Border = 0;
                    table.AddCell(cella1);

                    PdfPCell cella2 = new PdfPCell(new Phrase("KDV" + KDV.ToString().Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cella2.HorizontalAlignment = Element.ALIGN_LEFT;
                    cella2.Border = 0;
                    cella2.Colspan = 2;
                    table.AddCell(cella2);

                    PdfPCell cella3 = new PdfPCell(new Phrase(alt_toplam_kdv.ToString().Trim()+ " " + sembol.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cella3.HorizontalAlignment = Element.ALIGN_LEFT;
                    cella3.Border = 0;
                    table.AddCell(cella3);

                    PdfPCell cella4 = new PdfPCell(new Phrase("GENEL TOPLAM TUTAR:", new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cella4.HorizontalAlignment = Element.ALIGN_LEFT;
                    cella4.Border = 0;
                    cella4.Colspan = 2;
                    table.AddCell(cella4);

                    PdfPCell cella5 = new PdfPCell(new Phrase(genel_toplam.ToString().Trim() + " "+ sembol.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cella5.HorizontalAlignment = Element.ALIGN_LEFT;
                    cella5.Border = 0;
                    table.AddCell(cella5);
                }
                else
                {
                    table = new PdfPTable(3);
                    table.WidthPercentage = 30;
                    table.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.SpacingBefore = 5f;
                    table.SpacingAfter = 15f;

                    cell = new PdfPCell();
                    PdfPCell cella = new PdfPCell(new Phrase("TOPLAM TUTAR:", new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cella.HorizontalAlignment = Element.ALIGN_LEFT;
                    cella.Border = 0;
                    cella.Colspan = 2;
                    table.AddCell(cella);

                    PdfPCell cella1 = new PdfPCell(new Phrase(alt_toplam.ToString().Trim() +" "+sembol.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                    cella1.HorizontalAlignment = Element.ALIGN_LEFT;
                    cella1.Border = 0;
                    table.AddCell(cella1);

                }
                document.Add(table);




                //iTextSharp.text.Image img2 = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/depar_bos.jpg"));
                //img2.ScaleToFit(200, 50);
                //img2.SetAbsolutePosition(40, 155);
                //img2.Border = 0;
                //img2.BorderColor = iTextSharp.text.BaseColor.BLACK;
                //img2.BorderWidth = 1f;
                //document.Add(img2);


                //iTextSharp.text.Image img3 = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/depar_bos.jpg"));
                //img3.ScaleToFit(200, 50);
                //img3.SetAbsolutePosition(350, 155);
                //img3.Border = 0;
                //img3.BorderColor = iTextSharp.text.BaseColor.BLACK;
                //img3.BorderWidth = 1f;
                //document.Add(img3);




                PdfContentByte pcb = writer.DirectContent;
                table = new PdfPTable(2);
                table.TotalWidth = 520f;
                PdfPCell cell120 = new PdfPCell(new Phrase("DEPAR ONAY", new iTextSharp.text.Font(bF, 11f, iTextSharp.text.Font.NORMAL)));
                cell120.HorizontalAlignment = Element.ALIGN_CENTER;
                cell120.Border = 0;
                table.AddCell(cell120);

                PdfPCell cell121 = new PdfPCell(new Phrase("MUSTERI ONAY", new iTextSharp.text.Font(bF, 11f, iTextSharp.text.Font.NORMAL)));
                cell121.HorizontalAlignment = Element.ALIGN_CENTER;
                cell121.Border = 0;
                table.AddCell(cell121);
                table.WriteSelectedRows(0, -1, 25, 270, pcb);

                table = new PdfPTable(1);
                table.TotalWidth = 520f;

                //PdfPCell cell129 = new PdfPCell(new Phrase(" TEKLIF AÇIKLAMALARI", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.BOLD)));
                //cell129.HorizontalAlignment = Element.ALIGN_CENTER;
                //cell129.Border = 0;
                //table.AddCell(cell129);

                PdfPCell cell130 = new PdfPCell(new Phrase(" ODEME ŞEKLİ:" + odeme.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.ITALIC))); ;
                cell130.HorizontalAlignment = Element.ALIGN_LEFT;
                cell130.Border = 0;
                table.AddCell(cell130);

                //PdfPCell cell131 = new PdfPCell(new Phrase(" SİPARİŞ ŞEKLİ: 1000 adetlik partiler halinde", new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.ITALIC)));
                //cell131.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell131.Border = 0;
                //table.AddCell(cell131);

                //PdfPCell cell132 = new PdfPCell(new Phrase(" TESLİM SÜRESİ:" + , new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.ITALIC)));
                //cell132.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell132.Border = 0;
                //table.AddCell(cell132);

                PdfPCell cell133 = new PdfPCell(new Phrase(" TESLİM YERI VE TARİHİ:" + teslim_sekli.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.ITALIC)));
                cell133.HorizontalAlignment = Element.ALIGN_LEFT;
                cell133.Border = 0;
                table.AddCell(cell133);

                PdfPCell cell134 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.ITALIC)));
                cell134.HorizontalAlignment = Element.ALIGN_LEFT;
                cell134.Border = 0;
                table.AddCell(cell134);

                PdfPCell cell1341 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.ITALIC)));
                cell1341.HorizontalAlignment = Element.ALIGN_CENTER;
                cell1341.Border = 0;
                table.AddCell(cell1341);

                document.Add(new Paragraph(" "));

                PdfPCell cell135 = new PdfPCell(new Phrase(bank1.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.ITALIC)));
                cell135.HorizontalAlignment = Element.ALIGN_LEFT;
                cell135.Border = 0;
                table.AddCell(cell135);

                PdfPCell cell136 = new PdfPCell(new Phrase(bank2.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.ITALIC)));
                cell136.HorizontalAlignment = Element.ALIGN_LEFT;
                cell136.Border = 0;
                table.AddCell(cell136);

                PdfPCell cell1361 = new PdfPCell(new Phrase(bank3.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.ITALIC)));
                cell1361.HorizontalAlignment = Element.ALIGN_LEFT;
                cell1361.Border = 0;
                table.AddCell(cell1361);

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



                table.WriteSelectedRows(0, -1, 25, 240, pcb);


                iTextSharp.text.Image IMGISO90012 = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/ISO9001.png"));
                IMGISO90012.ScaleToFit(100, 70);
                IMGISO90012.SetAbsolutePosition(40, 10);
                //IMGISO9001.Border = iTextSharp.text.Rectangle.BOX;
                IMGISO90012.Border = 0;
                IMGISO90012.BorderColor = iTextSharp.text.BaseColor.BLACK;
                IMGISO90012.BorderWidth = 1f;
                document.Add(IMGISO90012);

                //iTextSharp.text.Image IMGISO140001 = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/ISO14001.png"));
                //IMGISO140001.ScaleToFit(100, 100);
                //IMGISO140001.SetAbsolutePosition(225, 10);
                ////IMGISO140001.Border = iTextSharp.text.Rectangle.BOX;
                //IMGISO140001.Border = 0;
                //IMGISO140001.BorderColor = iTextSharp.text.BaseColor.BLACK;
                //IMGISO140001.BorderWidth = 1f;
                //document.Add(IMGISO140001);

                //iTextSharp.text.Image IMGISO450001 = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/ISO45001.png"));
                //IMGISO450001.ScaleToFit(100, 100);
                //IMGISO450001.SetAbsolutePosition(410, 10);
                //IMGISO450001.Border = 0;
                //IMGISO450001.BorderColor = iTextSharp.text.BaseColor.BLACK;
                //IMGISO450001.BorderWidth = 1f;
                //document.Add(IMGISO450001);


                document.Close();
                writer.Close();


                Response.ContentType = "pdf/application";
                Response.AddHeader("content-disposition", "attachment;filename=" + txt_teklifno_kart.Text.Trim() +".pdf");
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            }


        }
    }
}