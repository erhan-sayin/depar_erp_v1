using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace depar_erp_v1_1
{

    public class evrak
    {
        public static bool teklif_sipno_update(string sipno, string USERNAME)
        {
            string constr3 = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con3 = new SqlConnection(constr3))
            {
                con3.Open();
                SqlCommand cmd = new SqlCommand("select * from TEKLIFT where SIPARISNO='" + sipno.Trim() + "'", con3);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Close();
                    SqlCommand cmd_2 = new SqlCommand("UPDATE TEKLIFE  SET DURUM= '3' WHERE RTRIM(EVRAKNO) IN (select EVRAKNO from TEKLIFT where SIPARISNO='" + sipno.Trim() + "' GROUP BY EVRAKNO) ", con3);
                    SqlCommand cmd_1 = new SqlCommand("UPDATE TEKLIFT  SET SIPARISNO= NULL WHERE RTRIM(SIPARISNO)='"+sipno.Trim()+"'", con3);

                    try
                    {
                        cmd_2.ExecuteNonQuery();
                        cmd_1.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        erp_log.log_at_av(USERNAME.Trim(), ex.ToString());
                    }

                    cmd_1.Dispose();
                }
                cmd.Dispose();
                con3.Close();
            }

            return true;
        }

        public static bool siparis_kapanan_miktar_update(string username, string tur, string SID, string miktar, string IRS_NO)
        {
            if (tur.Trim()=="AZ")
            {
                Decimal sip_miktar = 0;
                Decimal kalan_miktar = 0;
                Decimal bakiye_miktar = 0;
                decimal kapanan_miktar = 0;
                string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    string query = " SELECT * FROM SIPARIST WHERE ID='"+SID.Trim()+"' ";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        string sip_miktar_2 = dr["MIKTAR"].ToString();
                        string kalan_miktr_2 = dr["KALAN_MIKTAR"].ToString();
                        string kapanan_miktar_2 = dr["KAPANAN_MIKTAR"].ToString();

                        sip_miktar=decimal.Parse(sip_miktar_2);
                        decimal sevk_miktar = decimal.Parse(miktar);

                        if (dr["KALAN_MIKTAR"].ToString().Trim()!="")
                        {
                            kalan_miktar=decimal.Parse(kalan_miktr_2);
                        }
                        if (dr["KAPANAN_MIKTAR"].ToString().Trim()!="")
                        {
                            kapanan_miktar=decimal.Parse(kapanan_miktar_2);
                        }
                        kapanan_miktar= kapanan_miktar+ sevk_miktar;
                        bakiye_miktar= sip_miktar-kapanan_miktar;
                        if (bakiye_miktar<0)
                        {
                            bakiye_miktar=0;
                        }
                    }
                    else
                    {
                        string mesaj = "sipariş numarası bulunamadı";
                        erp_log.log_at_av(username.Trim(), mesaj.Trim());
                        return false;
                    }
                    dr.Close();

                    if (kapanan_miktar>=sip_miktar)
                    {
                        SqlCommand cmd1_1 = new SqlCommand(" UPDATE SIPARIST  SET  DURUM='6', KALAN_MIKTAR='"+bakiye_miktar.ToString().Replace(',', '.')+"',KAPANAN_MIKTAR='"+kapanan_miktar.ToString().Replace(',', '.')+"' WHERE ID='"+SID.Trim()+"' ", con);
                        try
                        {
                            cmd1_1.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            erp_log.log_at_av(username.Trim(), ex.ToString().Trim());
                            return false;
                        }

                    }
                    else
                    {
                        SqlCommand cmd1_1 = new SqlCommand(" UPDATE SIPARIST  SET  DURUM='5', KALAN_MIKTAR='"+bakiye_miktar.ToString().Replace(',', '.')+"',KAPANAN_MIKTAR='"+kapanan_miktar.ToString().Replace(',', '.')+"' WHERE ID='"+SID.Trim()+"' ", con);
                        try
                        {
                            cmd1_1.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            erp_log.log_at_av(username.Trim(), ex.ToString().Trim());
                            return false;
                        }
                    }

                    con.Close();
                }
            }
            else if (tur.Trim()=="AR")
            {
                decimal silinen_miktar = Convert.ToDecimal(miktar.Trim());
                decimal sip_miktar = 0;
                decimal eski_kap_miktar = 0;
                string SIP_ORDERNO = "";
                string query = " SELECT * FROM SIPARIST  WHERE ID='"+SID.Trim()+"' ";
                string constr3 = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr3))
                {
                    con.Open();
                    SqlCommand cmd_2 = new SqlCommand(query, con);
                    SqlDataReader dr_2 = cmd_2.ExecuteReader();
                    if (dr_2.HasRows)
                    {
                        dr_2.Read();
                        SIP_ORDERNO=dr_2["SIPNO"].ToString().Trim();
                        sip_miktar = Convert.ToDecimal(dr_2["MIKTAR"].ToString().Trim());
                        eski_kap_miktar = Convert.ToDecimal(dr_2["KAPANAN_MIKTAR"].ToString().Trim());
                    }
                    dr_2.Close();

                    SqlCommand cmd3_1 = new SqlCommand(" UPDATE SIPARIST  SET DURUM='5',KAPANAN_MIKTAR='"+(eski_kap_miktar-silinen_miktar).ToString().Replace(',', '.')+"',KALAN_MIKTAR='"+(sip_miktar-(eski_kap_miktar-silinen_miktar)).ToString().Replace(',', '.')+"' WHERE ID='"+SID+"' ", con);
                    try
                    {
                        cmd3_1.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        erp_log.log_at_av(username.Trim(), ex.ToString());
                        return false;
                    }

                    SqlCommand cmd = new SqlCommand("Delete From IRSALIYEE where RTRIM(EVRAKNO)='" +IRS_NO.Trim()+ "'", con);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        erp_log.log_at_av(username.Trim(), ex.ToString());
                        return false;
                    }
                    cmd.Dispose();

                    SqlCommand cmd_1 = new SqlCommand("Delete From IRSALIYET where RTRIM(EVRAKNO)='" + IRS_NO.Trim() + "'", con);
                    try
                    {
                        cmd_1.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        erp_log.log_at_av(username.Trim(), ex.ToString());
                        return false;
                    }

                    cmd_1.Dispose();
                    con.Close();
                }

                return true;
            }

            return true;
        }
    }
}