<%@ Page Language="C#" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="spec_yazdir.aspx.cs" Inherits="depar_erp_v1_1.spec_yazdir" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function hesapla_av() {
            var getValue = document.getElementById('drp_sablon').selectedOptions[0].value;
            //ince cidar ve kalın cidarli şablon hesaplaması aynı oldugu için aynu sub içinde hesaplamalar yapılmaktadır.
            if (getValue === "1" || getValue === "2") {
                var txt_nihai_bronz = document.getElementById('txt_nihai_bronz').value.replace(',', '.');
                var txt_discap = document.getElementById('txt_discap').value.replace(',', '.');
                var txt_dokum_adet = document.getElementById('txt_dokum_adet').value.replace(',', '.');
                var txt_wideman = document.getElementById('txt_wideman').value.replace(',', '.');
                var txt_nihainoyislempayi = document.getElementById('txt_nihainoyislempayi').value.replace(',', '.');
                var txt_nahaiictorna = document.getElementById('txt_nahaiictorna').value.replace(',', '.');
                var txt_dokummuayne = document.getElementById('txt_dokummuayne').value.replace(',', '.');
                var txt_taslama = document.getElementById('txt_taslama').value.replace(',', '.');
                var txt_kaliptan_gecirme = "" document.getElementById('txt_kaliptan_gecirme').value.replace(',', '.');
                var txt_alistirma = document.getElementById('txt_alistirma').value.replace(',', '.');
                var txt_mil_ort = document.getElementById('txt_mil_ort').value.replace(',', '.');
                var txt_alistirma = document.getElementById('txt_alistirma').value.replace(',', '.');
                var txt_kep_ort = document.getElementById('txt_kep_ort').value.replace(',', '.');
                var txt_boy_ort = document.getElementById('txt_boy_ort').value.replace(',', '.');
                var txt_cidar_ort = document.getElementById('txt_cidar_ort').value.replace(',', '.');
                var txt_ayna_tutma = document.getElementById('txt_ayna_tutma').value.replace(',', '.');
                var txt_herikitaraftemizlik = document.getElementById('txt_herikitaraftemizlik').value.replace(',', '.');
                var txt_kesmekalem = document.getElementById('txt_kesmekalem').value.replace(',', '.');
                var txt_talas_partm = document.getElementById('txt_talas_partm').value.replace(',', '.');
                var txt_ikiyebolme = document.getElementById('txt_ikiyebolme').value.replace(',', '.');
                var txt_sackalinlik_paramt = document.getElementById('txt_sackalinlik_paramt').value.replace(',', '.');
                var txt_sacgenislik_parmt = document.getElementById('txt_sacgenislik_parmt').value.replace(',', '.');
                var txt_sac_acilim_partm = document.getElementById('txt_sac_acilim_partm').value.replace(',', '.');
                var txt_kaplama = document.getElementById('txt_kaplama').value.replace(',', '.');


                var ic_kab_olcusu = parseFloat(txt_nihai_bronz) + parseFloat(txt_kaliptan_gecirme) + parseFloat(txt_alistirma) + parseFloat(txt_mil_ort);
                var dis_kab_olcusu = parseFloat(txt_discap) + parseFloat(txt_taslama) + parseFloat(txt_kaliptan_gecirme) + parseFloat(txt_alistirma) + parseFloat(txt_kep_ort);
                var dokum_capi = parseFloat(ic_kab_olcusu) - parseFloat(txt_nihai_bronz) - parseFloat(txt_wideman) - parseFloat(txt_nahaiictorna) - parseFloat(txt_dokummuayne);
                var dokum_muayne_torna = parseFloat(ic_kab_olcusu) - parseFloat(txt_nihai_bronz) - parseFloat(txt_wideman) - parseFloat(txt_nahaiictorna);
                var boy_kesme = parseFloat(txt_nihainoyislempayi) + parseFloat(txt_boy_ort);
                var celik_kilif_capi = parseFloat(ic_kab_olcusu) - parseFloat(txt_kaliptan_gecirme);
                var nihai_ic_torna = parseFloat(celik_kilif_capi) - parseFloat(txt_nihai_bronz) - parseFloat(txt_wideman);
                var nihai_dis_torna = parseFloat(txt_alistirma) + parseFloat(txt_taslama) + parseFloat(txt_kep_ort);
                var taslamak = parseFloat(txt_alistirma) + parseFloat(txt_kep_ort);
                var alistirma = parseFloat(txt_kep_ort) + parseFloat("0.02");
                var nihai_boy_torna = parseFloat(txt_boy_ort);
                var kaplanmis_cidar = parseFloat(txt_cidar_ort);
                var boru_kesme = (parseFloat(txt_dokum_adet) * parseFloat(txt_boy_ort)) + parseFloat(txt_ayna_tutma) + (parseFloat(txt_dokum_adet) * parseFloat(txt_nihainoyislempayi)) + (parseFloat(txt_herikitaraftemizlik) * parseFloat("2")) + ((parseFloat(txt_dokum_adet) - parseFloat("1")) * parseFloat(txt_kesmekalem));
                var talas_miktari = (((parseFloat("0.785") * ((parseFloat(ic_kab_olcusu) * parseFloat(ic_kab_olcusu)) - (parseFloat(dokum_capi) * parseFloat(dokum_capi)))) * parseFloat(boru_kesme)) * parseFloat("8.88")) / parseFloat("1000") * parseFloat(txt_talas_partm);
                var ikiye_bolmme = parseFloat(txt_ikiyebolme);
                var malzeme_1 = ((parseFloat(dis_kab_olcusu) - parseFloat(ic_kab_olcusu)) / parseFloat("2")) + parseFloat(txt_sackalinlik_paramt);
                var malzeme_2 = parseFloat(boru_kesme) + parseFloat(txt_sacgenislik_parmt);
                var malzeme_3 = ((parseFloat(dis_kab_olcusu) + parseFloat(ic_kab_olcusu)) / parseFloat("2") * parseFloat("3.14")) + parseFloat(txt_sac_acilim_partm);
                var videman = parseFloat(kaplanmis_cidar) - parseFloat(txt_kaplama);
                var grid = document.getElementById("<%= grd_spek_detay.ClientID%>");
            }
            //ince cidarliz düz şablon hesaplaması yapılıyor
            else if (getValue === "3") {

            }

            for (var i = 0; i < grid.rows.length - 1; i++) {
                var SATIR_DEGER = grid.rows[i + 1].cells[2].innerText;

                if (SATIR_DEGER == "65" || SATIR_DEGER == "94") {
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = malzeme_1.toFixed(0) + "X" + malzeme_2.toFixed(0) + "X" + malzeme_3.toFixed(0);
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = malzeme_1.toFixed(0) + "X" + malzeme_2.toFixed(0) + "X" + malzeme_3.toFixed(0);
                }
                if (SATIR_DEGER == "66" || SATIR_DEGER == "95") {
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = talas_miktari.toFixed(0);
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = talas_miktari.toFixed(0);
                }
                if (SATIR_DEGER == "67" || SATIR_DEGER == "97") {
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = ic_kab_olcusu.toFixed(2);
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = ic_kab_olcusu.toFixed(2);
                }
                if (SATIR_DEGER == "68" || SATIR_DEGER == "98") {
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = dis_kab_olcusu.toFixed(2);
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = dis_kab_olcusu.toFixed(2);
                }
                if (SATIR_DEGER == "69" || SATIR_DEGER == "99") {
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = dokum_capi.toFixed(2);
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = dokum_capi.toFixed(2);
                }
                if (SATIR_DEGER == "70" || SATIR_DEGER == "100") {
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = dokum_muayne_torna.toFixed(2);
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = dokum_muayne_torna.toFixed(2);
                }
                if (SATIR_DEGER == "71" || SATIR_DEGER == "101") {
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = boy_kesme.toFixed(2);
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = boy_kesme.toFixed(2);
                }
                if (SATIR_DEGER == "72" || SATIR_DEGER == "102") {
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = parseFloat(txt_kaliptan_gecirme).toFixed(2);
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = parseFloat(txt_kaliptan_gecirme).toFixed(2);
                }
                if (SATIR_DEGER == "73" || SATIR_DEGER == "104") {
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = celik_kilif_capi.toFixed(2);
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = celik_kilif_capi.toFixed(2);
                }
                if (SATIR_DEGER == "74" || SATIR_DEGER == "106") {
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = nihai_ic_torna.toFixed(2);
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = nihai_ic_torna.toFixed(2);
                }
                if (SATIR_DEGER == "75" || SATIR_DEGER == "108") {
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = nihai_dis_torna.toFixed(2);
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = nihai_dis_torna.toFixed(2);
                }
                if (SATIR_DEGER == "78") {
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = taslamak.toFixed(2);
                }
                if (SATIR_DEGER == "79" || SATIR_DEGER == "103") {
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = ikiye_bolmme.toFixed(2);
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = ikiye_bolmme.toFixed(2);
                }
                if (SATIR_DEGER == "81" || SATIR_DEGER == "111") {
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = alistirma.toFixed(2);
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = alistirma.toFixed(2);
                }
                if (SATIR_DEGER == "87" || SATIR_DEGER == "116") {
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = videman.toFixed(2);
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = videman.toFixed(2);
                }
                if (SATIR_DEGER == "89" || SATIR_DEGER == "117") {
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = kaplanmis_cidar.toFixed(2);
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = kaplanmis_cidar.toFixed(2);
                }
                if (SATIR_DEGER == "124" || SATIR_DEGER == "96") {
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = boru_kesme.toFixed(2);
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = boru_kesme.toFixed(2);
                }
                if (SATIR_DEGER == "123" || SATIR_DEGER == "105") {
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = nihai_boy_torna.toFixed(2);
                    grid.rows[i + 1].getElementsByTagName("input")[0].value = nihai_boy_torna.toFixed(2);
                }
            }
        };
    </script>
    <link href="bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="main_style_sheet.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="sablon_varmi" runat="server" />
        <div class="row">
            <div class="col-md-2 col-xs-2 col-sm-2">
                <div class="from-group">
                    <asp:Label ID="Label9" runat="server" Text="Şabon Seçiniz"></asp:Label>
                    <asp:DropDownList ID="drp_sablon" runat="server" CssClass="form-control" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="CHANGE_AV">
                        <Items>
                            <asp:ListItem Text="Şablon Seçiniz" />
                        </Items>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2 col-xs-2 col-sm-2">
                <div class="from-group">
                    <asp:Label ID="Label22" runat="server" Text="Rota Seçiniz"></asp:Label>
                    <asp:DropDownList ID="drp_rota" runat="server" CssClass="form-control" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ROTA_AV">
                        <Items>
                            <asp:ListItem Text="Rota Seçiniz" />
                            <asp:ListItem Text="Döküm Rotası" Value="1" />
                            <asp:ListItem Text="Bant Rotası" Value="2" />
                        </Items>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2 col-xs-2 col-sm-2">
                <div class="from-group">
                    <asp:Label ID="Label10" runat="server" Text=".."></asp:Label>
                    <asp:Button ID="btn_update" runat="server" CssClass="form-control btn btn-success" Width="100%" Text="Güncelle" OnClick="SPEK_UPDATE_AV" OnClientClick="return confirm('Spek kartınız güncellenecektir emin misiniz?');" />
                </div>
            </div>

            <div class="col-md-2 col-xs-2 col-sm-2">
                <div class="from-group">
                    <asp:Label ID="Label12" runat="server" Text=".."></asp:Label>
                    <asp:Button ID="btn_print" runat="server" CssClass="form-control btn btn-success" Width="100%" Text="Spek Yazdır" OnClick="SPEK_PRINT_AV" />
                </div>
            </div>
            <div class="col-md-2 col-xs-2 col-sm-2">
                <div class="from-group">
                    <asp:Label ID="Label74" runat="server" Text=".."></asp:Label>
                    <asp:Button ID="Button1" runat="server" CssClass="form-control btn btn-danger" Width="100%" Text="Spek Sil" OnClick="SPEK_DELETE_AV" OnClientClick="return confirm('Spek kartını silmek istediginize emin misiniz?');" />
                </div>
            </div>
        </div>

        <%-- URUN BİLGİLERİNİN GİRİLDİGİ ALAN --%>
        <fieldset>
            <legend>Ürün Bilgileri</legend>
            <div class="row">
                <div class="col-md-1 col-xs-1 col-sm-1">
                    <div class="from-group">
                        <asp:Label ID="Label20" runat="server" Text="Depar Kodu"></asp:Label>
                        <asp:TextBox ID="txt_depar_kod" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-1 col-xs-1 col-sm-1">
                    <div class="from-group">
                        <asp:Label ID="Label1" runat="server" Text="Ürün Kodu"></asp:Label>
                        <asp:TextBox ID="txt_stokkod" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-2 col-xs-2 col-sm-2">
                    <div class="from-group">
                        <asp:Label ID="Label3" runat="server" Text="Ürün Grubu"></asp:Label>
                        <asp:TextBox ID="txt_urungrubu" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-2 col-xs-2 col-sm-2">
                    <div class="from-group">
                        <asp:Label ID="Label6" runat="server" Text="Ürün Cinsi"></asp:Label>
                        <asp:TextBox ID="txt_uruncinsi" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-2 col-xs-2 col-sm-2">
                    <div class="from-group">
                        <asp:Label ID="Label2" runat="server" Text="Marka"></asp:Label>
                        <asp:TextBox ID="txt_marka" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-2 col-xs-2 col-sm-2">
                    <div class="from-group">
                        <asp:Label ID="Label5" runat="server" Text="Model"></asp:Label>
                        <asp:TextBox ID="txt_model" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-2 col-xs-2 col-sm-2">
                    <div class="from-group">
                        <asp:Label ID="Label21" runat="server" Text="R.Açıkl."></asp:Label>
                        <asp:TextBox ID="txt_rev_aciklama" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
            </div>
            <br />


            <div class="row">
                <div class="col-md-3 col-xs-3 col-sm-3">
                    <div class="row row-cols-2">
                        <div class="col">
                            <asp:Label ID="Label4" runat="server" Text="Ölçü"></asp:Label>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_olcu" runat="server" Width="150px"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:Label ID="Label25" runat="server" Text="Halita Kodu"></asp:Label>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_halita" runat="server" Width="150px"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:Label ID="Label7" runat="server" Text="Boru kodu"></asp:Label>
                        </div>
                        <div class="col">
                            <asp:DropDownList ID="drp_borukodu" runat="server" CssClass="form-control" Width="150px" AppendDataBoundItems="true" AutoPostBack="true" OnTextChanged="drp_borukodu_TextChanged">
                                <Items>
                                    <asp:ListItem Text="Boru Seçiniz" />
                                </Items>
                            </asp:DropDownList>
                        </div>
                        <div class="col">
                            <asp:Label ID="Label23" runat="server" Text="Sac kodu"></asp:Label>
                        </div>
                        <div class="col">
                            <asp:DropDownList ID="drp_sackodu" runat="server" CssClass="form-control" Width="150px" AppendDataBoundItems="true" AutoPostBack="true" OnTextChanged="sac_bul_av">
                                <Items>
                                    <asp:ListItem Text="Sac Seçiniz" />
                                </Items>
                            </asp:DropDownList>
                        </div>
                        <div class="col">
                            <asp:Label ID="Label75" runat="server" Text="Sac Kalınlık"></asp:Label>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_sac_kalinlik_deger" runat="server" Width="150px"></asp:TextBox>
                        </div>
                    </div>

                </div>
                <div class="col-md-4 col-xs-4 col-sm-4">
                    <div class="row row-cols-4">
                        <div class="col">
                            <asp:Label ID="Label13" runat="server" Text="Ölçüsü(mm)" Font-Bold="true"></asp:Label>
                        </div>
                        <div class="col">
                            <asp:Label ID="Label14_1" runat="server" Text="Min" Font-Bold="true"></asp:Label>
                        </div>
                        <div class="col">
                            <asp:Label ID="Label15_1" runat="server" Text="Max" Font-Bold="true"></asp:Label>
                        </div>
                        <div class="col">
                            <asp:Label ID="Label16_1" runat="server" Text="Ort" Font-Bold="true"></asp:Label>
                        </div>
                        <%-- mil bilgileri  --%>
                        <div class="col">
                            <asp:Label ID="Label14_3" runat="server" Text="Mil Çapı"></asp:Label>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_mil_min" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_mil_max" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_mil_ort" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                        <%-- kep bilgileri  --%>
                        <div class="col">
                            <asp:Label ID="Label15_2" runat="server" Text="Kep Çapı"></asp:Label>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_kep_min" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_kep_max" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_kep_ort" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                        <%-- yag boşlugu bilgileri  --%>
                        <div class="col">
                            <asp:Label ID="Label8" runat="server" Text="Yağ Boşlugu"></asp:Label>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_yag_min" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_yag_max" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_yag_ort" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="col-md-4 col-xs-4 col-sm-4">
                    <div class="row row-cols-4">
                        <div class="col">
                            <asp:Label ID="Label11" runat="server" Text="Ölçüsü(mm)" Font-Bold="true"></asp:Label>
                        </div>
                        <div class="col">
                            <asp:Label ID="Label24" runat="server" Text="Min" Font-Bold="true"></asp:Label>
                        </div>
                        <div class="col">
                            <asp:Label ID="Label26" runat="server" Text="Max" Font-Bold="true"></asp:Label>
                        </div>
                        <div class="col">
                            <asp:Label ID="Label27" runat="server" Text="Ort" Font-Bold="true"></asp:Label>
                        </div>

                        <%-- CİDAR BİLGİLERİ GİRİLİYOR --%>
                        <div class="col">
                            <asp:Label ID="Label28" runat="server" Text="Cidar"></asp:Label>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_cidar_min" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_cidar_max" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_cidar_ort" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                        <%-- İÇÇAP BİLGİLERİ GİRİLİYOR --%>
                        <div class="col">
                            <asp:Label ID="Label29" runat="server" Text="İççap"></asp:Label>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_iccap_min" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_iccap_max" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_iccap_ort" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                        <%-- DIŞÇAP BİLGİLERİ GİRİLİYOR --%>
                        <div class="col">
                            <asp:Label ID="Label30" runat="server" Text="Dışçap"></asp:Label>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_discap_min" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_discap_max" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_discap_ort" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                        <%-- BOY BİLGİLERİ GİRİLİYOR --%>
                        <div class="col">
                            <asp:Label ID="Label31" runat="server" Text="Boy"></asp:Label>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_boy_min" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_boy_max" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_boy_ort" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                        <%-- BOY 2 BİLGİLERİ GİRİLİYOR --%>
                        <div class="col">
                            <asp:Label ID="Label73" runat="server" Text="Boy 2"></asp:Label>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_boy2_min" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_boy2_max" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_boy2_ort" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                        <%-- CENEARASI BİLGİLERİ GİRİLİYOR --%>
                        <div class="col">
                            <asp:Label ID="Label32" runat="server" Text="Çenearası"></asp:Label>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_ceneara_min" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_ceneara_max" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_ceneara_ort" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                        <%-- CENECAP BİLGİLERİ GİRİLİYOR --%>
                        <div class="col">
                            <asp:Label ID="Label33" runat="server" Text="Çeneçap"></asp:Label>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_cenecap_min" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_cenecap_max" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txt_cenecap_ort" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                    </div>
                </div>
            </div>
            <br />
        </fieldset>
        <br />

        <%-- PARAMETRE GİRİŞ ALANI --%>
        <fieldset>
            <legend>Parametri Girişleri</legend>
            <div class="col-md-12 col-xs-12 col-sm-12">
                <div class="row row-cols-5" style="border: 1px; text-align: center">
                    <div class="col">
                        <asp:Label ID="Label14" runat="server" Text="İççap için" Font-Bold="true" Width="100%" BorderStyle="Solid" BorderWidth="1px"></asp:Label>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label35" runat="server" Text="Dışçap için" Font-Bold="true" Width="100%" BorderStyle="Solid" BorderWidth="1px"></asp:Label>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label36" runat="server" Text="İç ve Dış için" Font-Bold="true" Width="100%" BorderStyle="Solid" BorderWidth="1px"></asp:Label>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label37" runat="server" Text="Boy için" Font-Bold="true" Width="100%" BorderStyle="Solid" BorderWidth="1px"></asp:Label>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label38" runat="server" Text="Malzeme " Font-Bold="true" Width=" 100%" BorderStyle="Solid" BorderWidth="1px"></asp:Label>
                    </div>
                </div>

                <div class="row row-cols-10" style="font-size: small">
                    <%-- 1. satır için  --%>
                    <div class="col">
                        <asp:Label ID="Label15" runat="server" Text="Nihai ürün bronz"></asp:Label>
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txt_nihai_bronz" CssClass="form-control" runat="server" onchange="hesapla_av()"></asp:TextBox>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label16" runat="server" Text="Dış çap torna"></asp:Label>
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txt_discap" CssClass="form-control" runat="server" onchange="hesapla_av()"></asp:TextBox>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label17" runat="server" Text="Kalıptan Geçme"></asp:Label>
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txt_kaliptan_gecirme" CssClass="form-control" runat="server" onchange="hesapla_av()"></asp:TextBox>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label18" runat="server" Text="Ayna tutma yeri"></asp:Label>
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txt_ayna_tutma" CssClass="form-control" runat="server" onchange="hesapla_av()"></asp:TextBox>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label19" runat="server" Text="Saç açılım param."></asp:Label>
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txt_sac_acilim_partm" CssClass="form-control" runat="server" onchange="hesapla_av()"></asp:TextBox>
                    </div>
                </div>
                <div class="row row-cols-10" style="font-size: small">
                    <%-- 2. satır için  --%>
                    <div class="col">
                        <asp:Label ID="Label34" runat="server" Text="Wideman"></asp:Label>
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txt_wideman" CssClass="form-control" runat="server" onchange="hesapla_av()"></asp:TextBox>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label39" runat="server" Text="Taşlama "></asp:Label>
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txt_taslama" CssClass="form-control" runat="server" onchange="hesapla_av()"></asp:TextBox>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label40" runat="server" Text="İkiye bölme Testere"></asp:Label>
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txt_ikiyebolme" CssClass="form-control" runat="server" onchange="hesapla_av()"></asp:TextBox>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label41" runat="server" Text="Tam boy nihai işlem payı"></asp:Label>
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txt_nihainoyislempayi" CssClass="form-control" runat="server" onchange="hesapla_av()"></asp:TextBox>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label42" runat="server" Text="Saç genişlik param."></asp:Label>
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txt_sacgenislik_parmt" CssClass="form-control" runat="server" onchange="hesapla_av()"></asp:TextBox>
                    </div>
                </div>
                <div class="row row-cols-10" style="font-size: small">
                    <%-- 3. satır için  --%>
                    <div class="col">
                        <asp:Label ID="Label43" runat="server" Text="Nihai iç torna"></asp:Label>
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txt_nahaiictorna" CssClass="form-control" runat="server" onchange="hesapla_av()"></asp:TextBox>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label44" runat="server" Text="Kaba Ç.üstü kurşun temz payı"></asp:Label>
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txt_kabaustukursunpay" CssClass="form-control" runat="server" onchange="hesapla_av()"></asp:TextBox>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label45" runat="server" Text="Alıştırma"></asp:Label>
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txt_alistirma" CssClass="form-control" runat="server" onchange="hesapla_av()"></asp:TextBox>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label46" runat="server" Text="Her iki baş temizlik tek taraf için"></asp:Label>
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txt_herikitaraftemizlik" CssClass="form-control" runat="server" onchange="hesapla_av()"></asp:TextBox>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label47" runat="server" Text="Saç kalınlık param."></asp:Label>
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txt_sackalinlik_paramt" CssClass="form-control" runat="server" onchange="hesapla_av()"></asp:TextBox>
                    </div>
                </div>
                <div class="row row-cols-10" style="font-size: small">
                    <%-- 4. satır için  --%>
                    <div class="col">
                        <asp:Label ID="Label48" runat="server" Text="Döküm Muayene "></asp:Label>
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txt_dokummuayne" CssClass="form-control" runat="server" onchange="hesapla_av()"></asp:TextBox>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label49" runat="server" Text="Kba çene üstü çelik kalınlığı"></asp:Label>
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txt_kabaustucelik" CssClass="form-control" runat="server" onchange="hesapla_av()"></asp:TextBox>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label50" runat="server" Text="ÇAKMA PAYI"></asp:Label>
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txt_cakmapay" CssClass="form-control" runat="server" onchange="hesapla_av()"></asp:TextBox>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label51" runat="server" Text="Kesme kalem genişlik"></asp:Label>
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txt_kesmekalem" CssClass="form-control" runat="server" onchange="hesapla_av()"></asp:TextBox>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label52" runat="server" Text="Talaş miktar paramt."></asp:Label>
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txt_talas_partm" CssClass="form-control" runat="server" onchange="hesapla_av()"></asp:TextBox>
                    </div>
                </div>
                <div class="row row-cols-10" style="font-size: small">
                    <%-- 4. satır için  --%>
                    <div class="col">
                        <asp:Label ID="Label53" runat="server" Text="Kaplama"></asp:Label>
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txt_kaplama" CssClass="form-control" runat="server" onchange="hesapla_av()"></asp:TextBox>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label54" runat="server" Text="Kanal Kalem Boyu"></asp:Label>
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txt_kanakalemboy" CssClass="form-control" runat="server" onchange="hesapla_av()"></asp:TextBox>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label55" runat="server" Text="Alıştırma Böl den sonra"></asp:Label>
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txt_alsitirmabol" CssClass="form-control" runat="server" onchange="hesapla_av()"></asp:TextBox>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label56" runat="server" Text="Döküm adet"></asp:Label>
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txt_dokum_adet" CssClass="form-control" runat="server" onchange="hesapla_av()"></asp:TextBox>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label57" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="col">
                        <%--<asp:TextBox ID="TextBox19" CssClass="form-control" runat="server" ></asp:TextBox>--%>
                    </div>
                </div>
                <div class="row row-cols-10" style="font-size: small">
                    <%-- 5. satır için  --%>
                    <div class="col">
                        <asp:Label ID="Label58" runat="server" Text="  "></asp:Label>
                    </div>
                    <div class="col">
                        <%-- <asp:TextBox ID="TextBox9" CssClass="form-control" runat="server" ></asp:TextBox>--%>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label59" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="col">
                        <%--<asp:TextBox ID="TextBox14" CssClass="form-control" runat="server"></asp:TextBox>--%>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label60" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="col">
                        <%--<asp:TextBox ID="TextBox17" CssClass="form-control" runat="server" ></asp:TextBox>--%>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label62" runat="server" Text="Kesme Kalem Genişliği"></asp:Label>
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txt_kesmekalemgenislik" CssClass="form-control" runat="server" onchange="hesapla_av()"></asp:TextBox>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label61" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="col">
                        <%--<asp:TextBox ID="TextBox19" CssClass="form-control" runat="server" ></asp:TextBox>--%>
                    </div>

                </div>
                <div class="row row-cols-10" style="font-size: small">
                    <%-- 6. satır için  --%>
                    <div class="col">
                        <asp:Label ID="Label63" runat="server" Text="  "></asp:Label>
                    </div>
                    <div class="col">
                        <%-- <asp:TextBox ID="TextBox9" CssClass="form-control" runat="server" ></asp:TextBox>--%>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label64" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="col">
                        <%--<asp:TextBox ID="TextBox14" CssClass="form-control" runat="server"></asp:TextBox>--%>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label65" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="col">
                        <%--<asp:TextBox ID="TextBox17" CssClass="form-control" runat="server" ></asp:TextBox>--%>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label67" runat="server" Text="Boy Nihai Bronz"></asp:Label>
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txt_boybronz" CssClass="form-control" runat="server" onchange="hesapla_av()"></asp:TextBox>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label66" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="col">
                        <%--<asp:TextBox ID="TextBox19" CssClass="form-control" runat="server" ></asp:TextBox>--%>
                    </div>

                </div>
                <div class="row row-cols-10" style="font-size: small">
                    <%-- 6. satır için  --%>
                    <div class="col">
                        <asp:Label ID="Label68" runat="server" Text="  "></asp:Label>
                    </div>
                    <div class="col">
                        <%-- <asp:TextBox ID="TextBox9" CssClass="form-control" runat="server" ></asp:TextBox>--%>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label69" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="col">
                        <%--<asp:TextBox ID="TextBox14" CssClass="form-control" runat="server"></asp:TextBox>--%>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label70" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="col">
                        <%--<asp:TextBox ID="TextBox17" CssClass="form-control" runat="server" ></asp:TextBox>--%>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label72" runat="server" Text="Merkezden Kesme"></asp:Label>
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txt_merkezdenkesme" CssClass="form-control" runat="server" onchange="hesapla_av()"></asp:TextBox>
                    </div>
                    <div class="col">
                        <asp:Label ID="Label71" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="col">
                        <%--<asp:TextBox ID="TextBox19" CssClass="form-control" runat="server" ></asp:TextBox>--%>
                    </div>

                </div>



            </div>
        </fieldset>
        <br />
        <div class="row">
            <div class="col-md-6 col-xs-6 col-sm-6">
                <fieldset id="sablon_1" runat="server" style="background: url('../images/ceneli_format_1.png') no-repeat left bottom; height: 250px; width:100%">
                    <legend>Şablon1</legend>
                    <input id="txt_3" type="text" style="margin-left: 250px; width: 50px; margin-top: 15px" />
                    <input id="txt_1" type="text" style="margin-left: 500px; width: 50px; margin-top: 85px; height: 30px" />
                    <input id="txt_2" type="text" style="margin-left: 10px; width: 50px; margin-top: 100px" />
                </fieldset>
            </div>
            <div class="col-md-6 col-xs-6 col-sm-6">
                <fieldset id="sablon_2" runat="server" style="background: url('../images/ceneli_format_2.png') no-repeat left bottom; height: 250px; width:100%">
                    <legend>Şablon2</legend>
                    <input id="txt_11" type="text" style="margin-left: 500px; width: 50px; margin-top: 100px; height: 30px" />
                    <input id="txt_41"type="text" style="margin-left: 10px; width: 50px; margin-top: 100px" />
                </fieldset>
            </div>
        </div>


        <%-- SABLON VERİLERİ OLDUGU ALAN --%>
        <fieldset>
            <legend>Proses Bilgilerri</legend>
            <div class="row">
                <asp:GridView ID="grd_spek_detay"
                    AutoGenerateColumns="False"
                    GridLines="None"
                    PageSize="50"
                    AllowPaging="true"
                    CssClass="mGrid_teklif"
                    PagerStyle-CssClass="pgr" DataKeyNames="ID"
                    AlternatingRowStyle-CssClass="alt" runat="server" OnRowDataBound="DRID_DETAY_DOLDUR_AV">
                    <Columns>
                        <asp:TemplateField HeaderText="" ItemStyle-Width="40px">
                            <ItemTemplate>
                                <asp:LinkButton ID="LNK_BTN_UP" runat="server" Font-Size="X-Small" Text="Yukarı" CssClass="btn btn-info" OnClick="MOVE_UP"></asp:LinkButton>
                            </ItemTemplate>
                            <ControlStyle Width="40px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" ItemStyle-Width="40px">
                            <ItemTemplate>
                                <asp:LinkButton ID="LNK_BTN_DOWN" runat="server" Font-Size="X-Small" Text="Aşagı" CssClass="btn btn-info" OnClick="MOVE_DOWN"></asp:LinkButton>
                            </ItemTemplate>
                            <ControlStyle Width="40px" />
                        </asp:TemplateField>

                        <asp:BoundField DataField="ID" HeaderText="ID" ItemStyle-Width="1px" />
                        <asp:BoundField DataField="ISLEM_SIRA_NO" HeaderText="Sıra no" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="ISLEM_ADI" HeaderText="İşlem Açıklama" />

                        <asp:TemplateField HeaderText="Ölçü" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:TextBox ID="OLCU" runat="server" Text='<%# Eval("OLCU") %>' Width="100%"></asp:TextBox>
                            </ItemTemplate>
                            <ControlStyle Width="100px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Tolerans" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="TOLERANS" runat="server" Text='<%# Eval("TOLERANS") %>' Width="100%"></asp:TextBox>
                            </ItemTemplate>
                            <ControlStyle Width="80px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Bölüm" HeaderStyle-Width="120px">
                            <ItemTemplate>
                                <asp:Label ID="BOLUM" runat="server" Text='<%# Eval("BOLUM") %>' Visible="false" />
                                <asp:DropDownList ID="DRP_BOLUM" runat="server" AppendDataBoundItems="true" Width="100%">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <ControlStyle Width="120px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Makine" HeaderStyle-Width="120px">
                            <ItemTemplate>
                                <asp:Label ID="MAKINA" runat="server" Text='<%# Eval("MAKINA") %>' Visible="false" />
                                <asp:DropDownList ID="DRP_MAKINE" runat="server" AppendDataBoundItems="true" Width="100%">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <ControlStyle Width="120px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Program" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="PROGRAM" runat="server" Text='<%# Eval("PROGRAM") %>' Width="100%"></asp:TextBox>
                            </ItemTemplate>
                            <ControlStyle Width="80px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Kalıp No" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:TextBox ID="KALIPNO" runat="server" Text='<%# Eval("KALIPNO") %>' Width="100%"></asp:TextBox>
                            </ItemTemplate>
                            <ControlStyle Width="80px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Açıklama" HeaderStyle-Width="250px">
                            <ItemTemplate>
                                <asp:TextBox ID="ACIKLAMA" runat="server" Text='<%# Eval("ACIKLAMA") %>'></asp:TextBox>
                            </ItemTemplate>
                            <ControlStyle Width="250px" />
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </div>
        </fieldset>

    </form>
</body>
</html>
