
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace depar_erp_v1_1
{

    public class send_mail
    {
        public static bool mail_gonder(string username, string kime, string konu, string mesaj, string dosya)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress("deparerp@yandex.com", "Depar Motor Yatakları");
                message.To.Add(kime.Trim());
                message.Subject = konu.Trim();
                message.IsBodyHtml = true;
                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(dosya);
                message.Attachments.Add(attachment);
                message.Body = mesaj.Trim();
                System.Net.Mail.SmtpClient SMTP = new System.Net.Mail.SmtpClient();
                SMTP.Host = "smtp.yandex.com"; ;
                SMTP.Port = 587;
                SMTP.EnableSsl = true;
                SMTP.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                SMTP.UseDefaultCredentials = false;
                SMTP.Credentials = new System.Net.NetworkCredential("deparerp@yandex.com", "uyfugvgydrimtohj");
                SMTP.Send(message);
                SMTP.Dispose();

            }
            catch (Exception ex)
            {
                erp_log.log_at_av(username.Trim(), ex.ToString());
                return false;
            }
            return true;
        }

        public static bool irsaliye_gonder(string username, string evrakno, string kime, string konu)
        {
            try
            {
                string firma_adi = "";
                string firma_kod = "";
                string teklif_tarihi = "";
                string firma_adres = "";
                string firma_tel = "";
                string firma_mail = "";
                string teslim_eden = "";
                string teslim_alan = "";
                string arac_plaka = "";
                string sevk_notu = "";
                string sevk_tarih = "";
                string duz_tarih = "";

                string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    string query = " SELECT * FROM IRSALIYEE A " +
                                   " LEFT OUTER JOIN IRSALIYET B ON (RTRIM(A.EVRAKNO)=RTRIM(B.EVRAKNO) ) " +
                                   "  WHERE RTRIM(A.EVRAKNO)='"+evrakno.Trim()+"' ";

                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        firma_adi = dr["CH_ADI"].ToString().Trim();
                        firma_kod = dr["CH_KODU"].ToString().Trim();
                        teklif_tarihi = Convert.ToDateTime(dr["SEVK_TARIH"].ToString().Trim()).ToString("yyyy-MM-dd");
                        teslim_eden=dr["TESLIM_EDEN"].ToString().Trim();
                        teslim_alan=dr["TESLIM_ALAN"].ToString().Trim();
                        arac_plaka = dr["ARAC_PLAKA"].ToString().Trim();
                        sevk_notu = dr["NOTLAR"].ToString().Trim();
                        sevk_tarih = Convert.ToDateTime(dr["SEVK_TARIH"].ToString().Trim()).ToString("yyyy-MM-dd");
                        duz_tarih = Convert.ToDateTime(dr["DUZ_TARIH"].ToString().Trim()).ToString("yyyy-MM-dd");
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
                    string file_name_1 = evrakno.Trim()+"--"+DateTime.Now.ToString("HHmm")+".pdf";
                    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(HttpContext.Current.Server.MapPath("~/IRSALIYE/") +file_name_1.Trim(), FileMode.Create));

                    string mail_dosya_yolu = HttpContext.Current.Server.MapPath("~/IRSALIYE/") + file_name_1.Trim();
                    document.Open();

                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/images/depar_logo.PNG"));
                    img.ScaleToFit(150, 150);
                    img.SetAbsolutePosition(50, 760);
                    img.Border = 0;
                    img.BorderColor = iTextSharp.text.BaseColor.BLACK;
                    img.BorderWidth = 5f;
                    document.Add(img);

                    iTextSharp.text.Image IMGISO9001 = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/images/ISO9001.png"));
                    IMGISO9001.ScaleToFit(150, 150);
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
                    PdfPCell cell = new PdfPCell(new Phrase("Sevk İrsaliyesi", new iTextSharp.text.Font(bF, 12f, iTextSharp.text.Font.BOLD)));
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
                    PdfPCell cell21 = new PdfPCell(new Phrase("İrsaliye No:", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell21.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell21.Border = 0;
                    table.AddCell(cell21);

                    PdfPCell cell22 = new PdfPCell(new Phrase(evrakno.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell22.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell22.Border = 0;
                    table.AddCell(cell22);
                    //------------------------------------------

                    PdfPCell cell322 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 12f, iTextSharp.text.Font.NORMAL)));
                    cell322.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell322.Border = 0;
                    cell322.Colspan = 7;
                    table.AddCell(cell322);

                    PdfPCell cell32 = new PdfPCell(new Phrase("İrsaliye Tarihi", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell32.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell32.Border = 0;
                    table.AddCell(cell32);

                    PdfPCell cell33 = new PdfPCell(new Phrase(sevk_tarih.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell33.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell33.Border = 0;
                    table.AddCell(cell33);

                    //----------------------------------------------

                    PdfPCell cell323 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 12f, iTextSharp.text.Font.NORMAL)));
                    cell323.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell323.Border = 0;
                    cell323.Colspan = 7;
                    table.AddCell(cell323);

                    PdfPCell cell324 = new PdfPCell(new Phrase("Düzenlenme Tarihi", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell324.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell324.Border = 0;
                    table.AddCell(cell324);

                    PdfPCell cell325 = new PdfPCell(new Phrase(duz_tarih.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell325.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell325.Border = 0;
                    table.AddCell(cell325);
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

                    //-----------------------------


                    PdfPCell cell77 = new PdfPCell(new Phrase("Esenler mahallesi sude sokak no:2 Pendik / İSTANBUL / TURKİYE", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
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

                    //---------------------------------

                    PdfPCell cell77_1 = new PdfPCell(new Phrase("www.deparbearings.com  email:info@deparbearings.com", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
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

                    //---------------------------------

                    PdfPCell cell80 = new PdfPCell(new Phrase("Tel:(0216) 395 95 63 Fax:(0216) 395 95 69", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
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

                    //----------------------------

                    document.Add(new Paragraph(" "));
                    document.Add(new Paragraph(" "));

                    table = new PdfPTable(1);
                    table.WidthPercentage = 100;


                    document.Add(new Paragraph(" "));
                    document.Add(new Paragraph(" "));


                    table = new PdfPTable(11);
                    table.WidthPercentage = 100;

                    PdfPCell cell902 = new PdfPCell(new Phrase("No", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                    cell902.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell902);

                    PdfPCell cell901 = new PdfPCell(new Phrase("D.Kodu", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                    cell901.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell901);

                    PdfPCell cell90 = new PdfPCell(new Phrase("Stok kodu", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                    cell90.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell90.Colspan = 2;
                    table.AddCell(cell90);

                    PdfPCell cell190 = new PdfPCell(new Phrase("Malzeme açıklama", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                    cell190.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell190.Colspan = 5;
                    table.AddCell(cell190);

                    PdfPCell cell91 = new PdfPCell(new Phrase("Miktar", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                    cell91.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell91);

                    PdfPCell cell92 = new PdfPCell(new Phrase("Birim", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                    cell92.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell92);

                    document.Add(table);

                    int satir = 0;
                    decimal alt_toplam = 0;
                    decimal alt_toplam2 = 0;



                    using (SqlConnection con_3 = new SqlConnection(constr))
                    {
                        con_3.Open();
                        string query = " SELECT * FROM IRSALIYET B WHERE RTRIM(B.EVRAKNO)='"+evrakno.Trim()+"' ";
                        SqlCommand cmd = new SqlCommand(query, con_3);
                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            satir= satir +1;
                            string depar_kod = dr["DEPAR_KOD"].ToString();
                            string stok_kod = dr["KOD"].ToString();
                            string stok_ad = dr["KOD_AD"].ToString();
                            string miktar = dr["MIKTAR"].ToString();
                            string birim = dr["BIRIM"].ToString();

                            table = new PdfPTable(11);
                            table.WidthPercentage = 100;

                            PdfPCell cell100 = new PdfPCell(new Phrase(satir.ToString().Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                            cell100.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.AddCell(cell100);

                            PdfPCell cell1100 = new PdfPCell(new Phrase(depar_kod.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                            cell1100.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.AddCell(cell1100);

                            PdfPCell cell106 = new PdfPCell(new Phrase(stok_kod.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                            cell106.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell106.Colspan = 2;
                            table.AddCell(cell106);

                            PdfPCell cell101 = new PdfPCell(new Phrase(stok_ad.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                            cell101.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell101.Colspan = 5;
                            table.AddCell(cell101);

                            PdfPCell cell102 = new PdfPCell(new Phrase(miktar.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                            cell102.HorizontalAlignment = Element.ALIGN_CENTER;
                            table.AddCell(cell102);

                            PdfPCell cell103 = new PdfPCell(new Phrase(HttpUtility.HtmlDecode(birim.Trim()), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                            cell103.HorizontalAlignment = Element.ALIGN_CENTER;
                            table.AddCell(cell103);
                            document.Add(table);
                            alt_toplam2 = alt_toplam2 + Convert.ToDecimal(miktar.Trim());
                        }
                        dr.Close();
                        con_3.Close();
                    }

                    alt_toplam = Decimal.Round(alt_toplam2, 0);

                    table = new PdfPTable(3);
                    table.WidthPercentage = 30;
                    table.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.SpacingBefore = 5f;
                    table.SpacingAfter = 15f;

                    cell = new PdfPCell();
                    PdfPCell cella = new PdfPCell(new Phrase("TOPLAM MİKTAR:", new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
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
                    PdfPCell cell120 = new PdfPCell(new Phrase("Teslim Eden", new iTextSharp.text.Font(bF, 11f, iTextSharp.text.Font.NORMAL)));
                    cell120.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell120.Border = 0;
                    table.AddCell(cell120);

                    PdfPCell cell121 = new PdfPCell(new Phrase("Teslim Alan", new iTextSharp.text.Font(bF, 11f, iTextSharp.text.Font.NORMAL)));
                    cell121.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell121.Border = 0;
                    table.AddCell(cell121);

                    table.TotalWidth = 520f;
                    PdfPCell cell120_1 = new PdfPCell(new Phrase(teslim_eden.Trim(), new iTextSharp.text.Font(bF, 11f, iTextSharp.text.Font.BOLD)));
                    cell120_1.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell120_1.Border = 0;
                    table.AddCell(cell120_1);

                    PdfPCell cell121_1 = new PdfPCell(new Phrase(teslim_alan.Trim(), new iTextSharp.text.Font(bF, 11f, iTextSharp.text.Font.BOLD)));
                    cell121_1.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell121_1.Border = 0;
                    table.AddCell(cell121_1);
                    table.WriteSelectedRows(0, -1, 25, 270, pcb);

                    table = new PdfPTable(1);
                    table.TotalWidth = 520f;

                    PdfPCell cell133 = new PdfPCell(new Phrase(" Notlar:" + HttpUtility.HtmlDecode(sevk_notu.Trim()), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.ITALIC)));
                    cell133.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell133.Border = 0;
                    table.AddCell(cell133);


                    PdfPCell cell1341 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.ITALIC)));
                    cell1341.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell1341.Border = 0;
                    table.AddCell(cell1341);

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


                    iTextSharp.text.Image IMGISO90012 = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/images/ISO9001.png"));
                    IMGISO90012.ScaleToFit(100, 70);
                    IMGISO90012.SetAbsolutePosition(40, 10);
                    //IMGISO9001.Border = iTextSharp.text.Rectangle.BOX;
                    IMGISO90012.Border = 0;
                    IMGISO90012.BorderColor = iTextSharp.text.BaseColor.BLACK;
                    IMGISO90012.BorderWidth = 1f;
                    document.Add(IMGISO90012);

                    iTextSharp.text.Image IMGISO140001 = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/images/ISO14001.png"));
                    IMGISO140001.ScaleToFit(100, 100);
                    IMGISO140001.SetAbsolutePosition(225, 10);
                    //IMGISO140001.Border = iTextSharp.text.Rectangle.BOX;
                    IMGISO140001.Border = 0;
                    IMGISO140001.BorderColor = iTextSharp.text.BaseColor.BLACK;
                    IMGISO140001.BorderWidth = 1f;
                    document.Add(IMGISO140001);

                    iTextSharp.text.Image IMGISO450001 = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/images/ISO45001.png"));
                    IMGISO450001.ScaleToFit(100, 100);
                    IMGISO450001.SetAbsolutePosition(410, 10);
                    IMGISO450001.Border = 0;
                    IMGISO450001.BorderColor = iTextSharp.text.BaseColor.BLACK;
                    IMGISO450001.BorderWidth = 1f;
                    document.Add(IMGISO450001);


                    document.Close();
                    writer.Close();

                    //Byte[] FileBuffer = File.ReadAllBytes(HttpContext.Current.Server.MapPath(path) +evrakno.Trim()+"--"+DateTime.Now.ToString("HHmmss")+".pdf");
                    //Byte[] FileBuffer = File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/IRSALIYE/") +file_name_1.Trim() );
                    //if (FileBuffer !=null)
                    //{
                    //    HttpContext.Current.Response.ContentType ="application/pdf";
                    //    HttpContext.Current.Response.AddHeader("content-length", FileBuffer.Length.ToString());
                    //    HttpContext.Current.Response.BinaryWrite(FileBuffer);
                    //}

                    //string path = @"\\192.168.1.4\DOKUMANLAR\IRSALIYELER\"+evrakno.Trim()+"--"+DateTime.Now.ToString("HHmmss") +".pdf";

                    //HttpContext.Current.Response.ContentType = "pdf/application";
                    //HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + evrakno.Trim()+"--"+DateTime.Now.ToString("HHmmss") +".pdf");
                    //HttpContext.Current.Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                    //FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write);
                    //ms.WriteTo(file);
                    //file.Close();
                    string mail = "erhan.sayin@ersaybilisim.com";
                    string konu_1 = evrakno.Trim() +" nolu sipariş için sevk irsaliyesi";
                    string mesaj_1 = "İrsaliye";
                    mail_gonder(username, mail, konu_1, mesaj_1, mail_dosya_yolu);
                }

            }
            catch (Exception ex)
            {
                erp_log.log_at_av(username, ex.ToString().Trim());
                return false;
            }
            return true;
        }

        public static bool irsaliye_gonder_2(string username, string evrakno)
        {
            //KULLANICI İÇİN MAİL ADRESİ TESPİT EDİLİYOR
            string mail_adres = "";
            string body_html = "";
            string firma_adi = "";
            string firma_kod = "";
            string teklif_tarihi = "";
            string firma_adres = "";
            string firma_tel = "";
            string firma_mail = "";
            string teslim_eden = "";
            string teslim_alan = "";
            string arac_plaka = "";
            string sevk_notu = "";
            string sevk_tarih = "";
            string duz_tarih = "";

            try
            {
                string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    string query = " SELECT * FROM USERS WHERE RTRIM(USERNAME)='"+username.Trim()+"'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        mail_adres = dr["EMAIL"].ToString().Trim();
                    }
                    dr.Close();
                    con.Close();
                    if (mail_adres.Trim()=="")
                    {
                        string metin_ack = "ilgili kullanıcı için mail adresi bulunamadı";
                        erp_log.log_at_av(username, metin_ack.Trim());
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                erp_log.log_at_av(username, ex.ToString().Trim());
                return false;
            }

            try
            {
                string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    string query = " SELECT * FROM IRSALIYEE A " +
                                   " LEFT OUTER JOIN IRSALIYET B ON (RTRIM(A.EVRAKNO)=RTRIM(B.EVRAKNO) ) " +
                                   "  WHERE RTRIM(A.EVRAKNO)='"+evrakno.Trim()+"' ";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        firma_adi = dr["CH_ADI"].ToString().Trim();
                        firma_kod = dr["CH_KODU"].ToString().Trim();
                        teklif_tarihi = Convert.ToDateTime(dr["SEVK_TARIH"].ToString().Trim()).ToString("yyyy-MM-dd");
                        teslim_eden=dr["TESLIM_EDEN"].ToString().Trim();
                        teslim_alan=dr["TESLIM_ALAN"].ToString().Trim();
                        arac_plaka = dr["ARAC_PLAKA"].ToString().Trim();
                        sevk_notu = dr["NOTLAR"].ToString().Trim();
                        sevk_tarih = Convert.ToDateTime(dr["SEVK_TARIH"].ToString().Trim()).ToString("yyyy-MM-dd");
                        duz_tarih = Convert.ToDateTime(dr["DUZ_TARIH"].ToString().Trim()).ToString("yyyy-MM-dd");
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

                body_html= "<!DOCTYPE html> " +
                   "<html xmlns=\"http://www.w3.org/1999/xhtml\">" +
                   "<head>" +
                     "<title>Email</title>" +
                   "</head>" +
                   "<body style=\"font-family:'Century Gothic'\">" +
                   "<h5>Aşagıda bilgileri yer alan cari için sevk işlemi yapılmıştır. Detaylar ektkei gibidir.</h5>" +
                    "<table border =" + 1 + "> " +
                   "<tr>" +
                   "<td> <strong> Cari : </strong> " + firma_adi.Trim() + " </td>" +
                   "</tr>" +
                    "<tr>" +
                   "<td> <strong> Teslim Eden: </strong>  " + teslim_eden.Trim() + " </td>" +
                   "</tr>" +
                     "<tr>" +
                   "<td> <strong> İrsaliye No: </strong> " + evrakno.Trim() + " </td>" +
                   "</tr>" +
                   "</table>" +
                    "</body>" +
                   "</html>";

                using (MemoryStream ms = new MemoryStream())
                {
                    //*************************** HEADER BİLGİLERİ YAZDIRILIYOR
                    //Document document = new Document(iTextSharp.text.PageSize.A4.Rotate(), 25, 25, 30, 30);
                    Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                    string file_name_1 = evrakno.Trim()+"--"+DateTime.Now.ToString("HHmm")+".pdf";
                    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(HttpContext.Current.Server.MapPath("~/IRSALIYE/") +file_name_1.Trim(), FileMode.Create));

                    string mail_dosya_yolu = HttpContext.Current.Server.MapPath("~/IRSALIYE/") + file_name_1.Trim();
                    document.Open();

                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/images/depar_logo.PNG"));
                    img.ScaleToFit(150, 150);
                    img.SetAbsolutePosition(50, 760);
                    img.Border = 0;
                    img.BorderColor = iTextSharp.text.BaseColor.BLACK;
                    img.BorderWidth = 5f;
                    document.Add(img);

                    iTextSharp.text.Image IMGISO9001 = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/images/ISO9001.png"));
                    IMGISO9001.ScaleToFit(150, 150);
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
                    PdfPCell cell = new PdfPCell(new Phrase("Sevk İrsaliyesi", new iTextSharp.text.Font(bF, 12f, iTextSharp.text.Font.BOLD)));
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
                    PdfPCell cell21 = new PdfPCell(new Phrase("İrsaliye No:", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell21.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell21.Border = 0;
                    table.AddCell(cell21);

                    PdfPCell cell22 = new PdfPCell(new Phrase(evrakno.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell22.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell22.Border = 0;
                    table.AddCell(cell22);
                    //------------------------------------------

                    PdfPCell cell322 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 12f, iTextSharp.text.Font.NORMAL)));
                    cell322.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell322.Border = 0;
                    cell322.Colspan = 7;
                    table.AddCell(cell322);

                    PdfPCell cell32 = new PdfPCell(new Phrase("İrsaliye Tarihi", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell32.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell32.Border = 0;
                    table.AddCell(cell32);

                    PdfPCell cell33 = new PdfPCell(new Phrase(sevk_tarih.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell33.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell33.Border = 0;
                    table.AddCell(cell33);

                    //----------------------------------------------

                    PdfPCell cell323 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 12f, iTextSharp.text.Font.NORMAL)));
                    cell323.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell323.Border = 0;
                    cell323.Colspan = 7;
                    table.AddCell(cell323);

                    PdfPCell cell324 = new PdfPCell(new Phrase("Düzenlenme Tarihi", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell324.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell324.Border = 0;
                    table.AddCell(cell324);

                    PdfPCell cell325 = new PdfPCell(new Phrase(duz_tarih.Trim(), new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
                    cell325.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell325.Border = 0;
                    table.AddCell(cell325);
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

                    //-----------------------------


                    PdfPCell cell77 = new PdfPCell(new Phrase("Esenler mahallesi sude sokak no:2 Pendik / İSTANBUL / TURKİYE", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
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

                    //---------------------------------

                    PdfPCell cell77_1 = new PdfPCell(new Phrase("www.deparbearings.com  email:info@deparbearings.com", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
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

                    //---------------------------------

                    PdfPCell cell80 = new PdfPCell(new Phrase("Tel:(0216) 395 95 63 Fax:(0216) 395 95 69", new iTextSharp.text.Font(bF, 9f, iTextSharp.text.Font.NORMAL)));
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

                    //----------------------------

                    document.Add(new Paragraph(" "));
                    document.Add(new Paragraph(" "));

                    table = new PdfPTable(1);
                    table.WidthPercentage = 100;


                    document.Add(new Paragraph(" "));
                    document.Add(new Paragraph(" "));


                    table = new PdfPTable(11);
                    table.WidthPercentage = 100;

                    PdfPCell cell902 = new PdfPCell(new Phrase("No", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                    cell902.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell902);

                    PdfPCell cell901 = new PdfPCell(new Phrase("D.Kodu", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                    cell901.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell901);

                    PdfPCell cell90 = new PdfPCell(new Phrase("Stok kodu", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                    cell90.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell90.Colspan = 2;
                    table.AddCell(cell90);

                    PdfPCell cell190 = new PdfPCell(new Phrase("Malzeme açıklama", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                    cell190.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell190.Colspan = 5;
                    table.AddCell(cell190);

                    PdfPCell cell91 = new PdfPCell(new Phrase("Miktar", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                    cell91.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell91);

                    PdfPCell cell92 = new PdfPCell(new Phrase("Birim", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.NORMAL)));
                    cell92.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell92);

                    document.Add(table);

                    int satir = 0;
                    decimal alt_toplam = 0;
                    decimal alt_toplam2 = 0;



                    using (SqlConnection con_3 = new SqlConnection(constr))
                    {
                        con_3.Open();
                        string query = " SELECT * FROM IRSALIYET B WHERE RTRIM(B.EVRAKNO)='"+evrakno.Trim()+"' ";
                        SqlCommand cmd = new SqlCommand(query, con_3);
                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            satir= satir +1;
                            string depar_kod = dr["DEPAR_KOD"].ToString();
                            string stok_kod = dr["KOD"].ToString();
                            string stok_ad = dr["KOD_AD"].ToString();
                            string miktar = dr["MIKTAR"].ToString();
                            string birim = dr["BIRIM"].ToString();

                            table = new PdfPTable(11);
                            table.WidthPercentage = 100;

                            PdfPCell cell100 = new PdfPCell(new Phrase(satir.ToString().Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                            cell100.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.AddCell(cell100);

                            PdfPCell cell1100 = new PdfPCell(new Phrase(depar_kod.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                            cell1100.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.AddCell(cell1100);

                            PdfPCell cell106 = new PdfPCell(new Phrase(stok_kod.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                            cell106.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell106.Colspan = 2;
                            table.AddCell(cell106);

                            PdfPCell cell101 = new PdfPCell(new Phrase(stok_ad.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                            cell101.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell101.Colspan = 5;
                            table.AddCell(cell101);

                            PdfPCell cell102 = new PdfPCell(new Phrase(miktar.Trim(), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                            cell102.HorizontalAlignment = Element.ALIGN_CENTER;
                            table.AddCell(cell102);

                            PdfPCell cell103 = new PdfPCell(new Phrase(HttpUtility.HtmlDecode(birim.Trim()), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
                            cell103.HorizontalAlignment = Element.ALIGN_CENTER;
                            table.AddCell(cell103);
                            document.Add(table);
                            alt_toplam2 = alt_toplam2 + Convert.ToDecimal(miktar.Trim());
                        }
                        dr.Close();
                        con_3.Close();
                    }

                    alt_toplam = Decimal.Round(alt_toplam2, 0);

                    table = new PdfPTable(3);
                    table.WidthPercentage = 30;
                    table.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.SpacingBefore = 5f;
                    table.SpacingAfter = 15f;

                    cell = new PdfPCell();
                    PdfPCell cella = new PdfPCell(new Phrase("TOPLAM MİKTAR:", new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.NORMAL)));
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
                    PdfPCell cell120 = new PdfPCell(new Phrase("Teslim Eden", new iTextSharp.text.Font(bF, 11f, iTextSharp.text.Font.NORMAL)));
                    cell120.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell120.Border = 0;
                    table.AddCell(cell120);

                    PdfPCell cell121 = new PdfPCell(new Phrase("Teslim Alan", new iTextSharp.text.Font(bF, 11f, iTextSharp.text.Font.NORMAL)));
                    cell121.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell121.Border = 0;
                    table.AddCell(cell121);

                    table.TotalWidth = 520f;
                    PdfPCell cell120_1 = new PdfPCell(new Phrase(teslim_eden.Trim(), new iTextSharp.text.Font(bF, 11f, iTextSharp.text.Font.BOLD)));
                    cell120_1.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell120_1.Border = 0;
                    table.AddCell(cell120_1);

                    PdfPCell cell121_1 = new PdfPCell(new Phrase(teslim_alan.Trim(), new iTextSharp.text.Font(bF, 11f, iTextSharp.text.Font.BOLD)));
                    cell121_1.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell121_1.Border = 0;
                    table.AddCell(cell121_1);
                    table.WriteSelectedRows(0, -1, 25, 270, pcb);

                    table = new PdfPTable(1);
                    table.TotalWidth = 520f;

                    PdfPCell cell133 = new PdfPCell(new Phrase(" Notlar:" + HttpUtility.HtmlDecode(sevk_notu.Trim()), new iTextSharp.text.Font(bF, 8f, iTextSharp.text.Font.ITALIC)));
                    cell133.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell133.Border = 0;
                    table.AddCell(cell133);


                    PdfPCell cell1341 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bF, 10f, iTextSharp.text.Font.ITALIC)));
                    cell1341.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell1341.Border = 0;
                    table.AddCell(cell1341);

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


                    iTextSharp.text.Image IMGISO90012 = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/images/ISO9001.png"));
                    IMGISO90012.ScaleToFit(100, 70);
                    IMGISO90012.SetAbsolutePosition(40, 10);
                    //IMGISO9001.Border = iTextSharp.text.Rectangle.BOX;
                    IMGISO90012.Border = 0;
                    IMGISO90012.BorderColor = iTextSharp.text.BaseColor.BLACK;
                    IMGISO90012.BorderWidth = 1f;
                    document.Add(IMGISO90012);

                    iTextSharp.text.Image IMGISO140001 = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/images/ISO14001.png"));
                    IMGISO140001.ScaleToFit(100, 100);
                    IMGISO140001.SetAbsolutePosition(225, 10);
                    //IMGISO140001.Border = iTextSharp.text.Rectangle.BOX;
                    IMGISO140001.Border = 0;
                    IMGISO140001.BorderColor = iTextSharp.text.BaseColor.BLACK;
                    IMGISO140001.BorderWidth = 1f;
                    document.Add(IMGISO140001);

                    iTextSharp.text.Image IMGISO450001 = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/images/ISO45001.png"));
                    IMGISO450001.ScaleToFit(100, 100);
                    IMGISO450001.SetAbsolutePosition(410, 10);
                    IMGISO450001.Border = 0;
                    IMGISO450001.BorderColor = iTextSharp.text.BaseColor.BLACK;
                    IMGISO450001.BorderWidth = 1f;
                    document.Add(IMGISO450001);


                    document.Close();
                    writer.Close();

                    //Byte[] FileBuffer = File.ReadAllBytes(HttpContext.Current.Server.MapPath(path) +evrakno.Trim()+"--"+DateTime.Now.ToString("HHmmss")+".pdf");
                    //Byte[] FileBuffer = File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/IRSALIYE/") +file_name_1.Trim() );
                    //if (FileBuffer !=null)
                    //{
                    //    HttpContext.Current.Response.ContentType ="application/pdf";
                    //    HttpContext.Current.Response.AddHeader("content-length", FileBuffer.Length.ToString());
                    //    HttpContext.Current.Response.BinaryWrite(FileBuffer);
                    //}

                    //string path = @"\\192.168.1.4\DOKUMANLAR\IRSALIYELER\"+evrakno.Trim()+"--"+DateTime.Now.ToString("HHmmss") +".pdf";

                    //HttpContext.Current.Response.ContentType = "pdf/application";
                    //HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + evrakno.Trim()+"--"+DateTime.Now.ToString("HHmmss") +".pdf");
                    //HttpContext.Current.Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                    //FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write);
                    //ms.WriteTo(file);
                    //file.Close();
                    string konu_1 = evrakno.Trim() +" nolu sipariş için sevk irsaliyesi";
                    string mesaj_1 = "İrsaliye";
                    mail_gonder(username, mail_adres, konu_1, body_html, mail_dosya_yolu);
                }

            }
            catch (Exception ex)
            {
                erp_log.log_at_av(username, ex.ToString().Trim());
                return false;
            }
            return true;
        }

    }
}