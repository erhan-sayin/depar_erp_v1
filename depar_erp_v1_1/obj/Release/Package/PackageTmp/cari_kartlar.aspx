<%@ Page Title="" Language="C#" MasterPageFile="~/master_page.Master" AutoEventWireup="true" CodeBehind="cari_kartlar.aspx.cs" Inherits="depar_erp_v1_1.cari_kartlar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="margin-left: 2%; margin-right: 96%; margin-right: 2%; text-align: left">

        <table style="width: 100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td style="width: 50%"><a href="#2151624#" id="a3" class="tab_selected" onclick="ChangeForm(3)">Cari Kart Giriş</a></td>
                <td style="width: 50%"><a href="#2151625#" id="a4" class="tab" onclick="ChangeForm(4)">Cari Kart liste</a></td>
            </tr>
        </table>


        <%-- Cari kart giriş ekranı --%>
        <div id="div3">
            <br />
            <div class="row">
                <table style="width: 100%">
                    <tr>
                        <th style="width: 5%">
                            <asp:Button ID="new_stok_kart" runat="server" class="btn btn-success" Text="Ekle" OnClick="save_av" />
                        </th>
                        <th style="width: 8%">
                            <asp:Button ID="Button1" runat="server" CssClass=" btn btn-success" Text="Güncelle" OnClick="update_av" />
                        </th>
                        <th style="width: 5%">
                            <asp:Button ID="delete_kart" runat="server" class="btn btn-danger" Text="Sil" OnClick="delete_kart_Click" OnClientClick="return confirm('Cari kartı silmek istediginize emin misiniz?');" />
                        </th>
                        <th style="width: 20%">
                            <div class="col-md-12 col-lg-12 col-sm-12 col-xs-12">
                                <asp:Label ID="Label7" runat="server" Text="Cari Tipi1"></asp:Label>
                                <asp:DropDownList ID="drp_cari_tip" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                    <Items>
                                        <asp:ListItem Text="Cari Tipini Giriniz" Value="0" />
                                        <asp:ListItem Text="Yurt içi" Value="1" />
                                        <asp:ListItem Text="Yurt Dışı" Value="2" />
                                    </Items>
                                </asp:DropDownList>
                            </div>
                        </th>
                        <th style="width: 20%">
                            <asp:Label ID="Label20" runat="server" Text="Cari Tipi2"></asp:Label>
                            <asp:DropDownList ID="drp_cari_tip_2" runat="server" CssClass="form-control" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="cari_tip_change_av">
                                <Items>
                                    <asp:ListItem Text="Cari Tipini Giriniz" Value="0" />
                                    <asp:ListItem Text="Müşteri" Value="1" />
                                    <asp:ListItem Text="Tedarikçi" Value="2" />
                                    <asp:ListItem Text="Müşteri/Tedarikçi" Value="3" />
                                </Items>
                            </asp:DropDownList>
                        </th>
                        <th style="width: 40%; text-align: center">
                            <asp:CheckBox ID="onay_1" runat="server" AutoPostBack="true" OnCheckedChanged="onay1_CheckedChanged" />
                            <asp:Label ID="label_onay" runat="server" Text="Cari Onayı"></asp:Label>
                            <asp:Label ID="label_onay_2" runat="server" Text="" Font-Bold="false"></asp:Label>
                        </th>

                    </tr>
                </table>
            </div>
            <br />
            <fieldset>
                <legend>Cari Genel Bilgiler</legend>
                <div class="row">
                    <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                        <asp:Label ID="Label2" runat="server" Text="Cari Kodu"></asp:Label>
                        <asp:TextBox ID="txt_carikod_kart" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>

                    <div class="col-md-3 col-lg-3 col-sm-3 col-xs-3">
                        <asp:Label ID="Label3" runat="server" Text="Cari Adı"></asp:Label>
                        <asp:TextBox ID="txt_cariadi_kart" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>

                    <div class="col-md-3 col-lg-3 col-sm-3 col-xs-3">
                        <asp:Label ID="Label17" runat="server" Text="Cari Sınıfı"></asp:Label>
                        <asp:DropDownList ID="drp_carisinif_kart" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                            <Items>
                                <asp:ListItem Text="Cari Sınıfı Giriniz" Value="0" />
                            </Items>
                        </asp:DropDownList>
                    </div>

                    <%--<div class="col-md-3 col-lg-3 col-sm-3 col-xs-3">
                        <asp:Label ID="Label18" runat="server" Text="Cari Grubu"></asp:Label>
                        <asp:DropDownList ID="drp_carigrup_kart" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                            <Items>
                                <asp:ListItem Text="Cari Grubunu Giriniz" Value="0" />
                            </Items>
                        </asp:DropDownList>
                    </div>--%>
                    <div class="col-md-1 col-lg-1 col-sm-1 col-xs-1">
                        <asp:Label ID="Label19" runat="server" Text="Durum"></asp:Label>
                        <asp:DropDownList ID="drp_caridurum_kart" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                            <Items>
                                <asp:ListItem Text="Aktif" Value="A" />
                                <asp:ListItem Text="Pasif" Value="P" />
                            </Items>
                        </asp:DropDownList>
                    </div>
                    

                </div>

                <br />
                <div class="row">
                    <div class="col-md-3 col-lg-3 col-sm-3 col-xs-3">
                        <asp:Label ID="Label4" runat="server" Text="Cari Ülkesi"></asp:Label>
                        <asp:DropDownList ID="drp_cariulke_kart" runat="server" CssClass="ddl form-control" AppendDataBoundItems="true">
                            <Items>
                                <asp:ListItem Text="Cari ülkesi Giriniz" />
                                <asp:ListItem Text="Türkiye" Selected="True" /> 
                            </Items>
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-3 col-lg-3 col-sm-3 col-xs-3">
                        <div class="from-group">
                            <asp:Label ID="Label5" runat="server" Text="Cari ili"></asp:Label>
                            <asp:DropDownList ID="drp_cariil_kart" runat="server" CssClass="ddl form-control" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="il_change_av">
                                <Items>
                                    <asp:ListItem Text="Cari ili Giriniz" />
                                </Items>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="col-md-3 col-lg-3 col-sm-3 col-xs-3">
                        <div class="from-group">
                            <asp:Label ID="Label8" runat="server" Text="Cari İlçesi"></asp:Label>
                            <asp:DropDownList ID="drp_cariilce_kart" runat="server" CssClass="ddl form-control" AppendDataBoundItems="true">
                                <Items>
                                    <asp:ListItem Text="Cari ilçesi Giriniz" />
                                </Items>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-3 col-lg-3 col-sm-3 col-xs-3">
                        <asp:Label ID="Label11" runat="server" Text="Email"></asp:Label>
                        <asp:TextBox ID="txt_cariemail_kart" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="row">

                    <div class="col-md-3 col-lg-3 col-sm-3 col-xs-3">
                        <asp:Label ID="Label12" runat="server" Text="Tel"></asp:Label>
                        <asp:TextBox ID="txt_caritel_kart" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-3 col-lg-3 col-sm-3 col-xs-3">
                        <asp:Label ID="Label13" runat="server" Text="Fax"></asp:Label>
                        <asp:TextBox ID="txt_carifax_kart" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-3 col-lg-3 col-sm-3 col-xs-3">
                        <div class="from-group">
                            <asp:Label ID="Label15" runat="server" Text="Vergi Dairesi"></asp:Label>
                            <asp:DropDownList ID="drp_carivdairesi_kart" runat="server" CssClass="ddl form-control" AppendDataBoundItems="true">
                                <Items>
                                    <asp:ListItem Text="Vergi Dairesi Giriniz" />
                                </Items>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-3 col-lg-3 col-sm-3 col-xs-3">
                        <asp:Label ID="Label16" runat="server" Text="Vergi No"></asp:Label>
                        <asp:TextBox ID="txt_carivergino_kart" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div>
                    <div class="col-md-12 col-lg-12 col-sm-12 col-xs-12">
                        <asp:Label ID="Label14" runat="server" Text="Cari Notlar"></asp:Label>
                        <asp:TextBox ID="txt_carinotlar_kart" TextMode="MultiLine" Height="100px" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
            </fieldset>
            <br />
            <fieldset>
                <div class="row">
                    <div class="col-md-6 col-lg-6 col-sm-6 col-xs-6">
                        <legend>Adres Bilgileri</legend>
                        <div class="row">
                            <div class="col-md-6 col-lg-6 col-sm-6 col-xs-6">
                                <asp:Label ID="Label9" runat="server" Text="Fatura Adresi"></asp:Label>
                                <asp:TextBox ID="txt_fatadres_kart" CssClass="form-control" TextMode="MultiLine" Height="100px" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-6 col-lg-6 col-sm-6 col-xs-6">
                                <asp:Label ID="Label10" runat="server" Text="Sevk Adresi"></asp:Label>
                                <asp:TextBox ID="txt_sevkadres_kart" CssClass="form-control" TextMode="MultiLine" Height="100px" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <%-- 17 ekim sedat beyin istegi üzerine cari kartları ödeme ekranı pasif edildi. --%>
                    <%--<div class="col-md-6 col-lg-6 col-sm-6 col-xs-6">
                        <legend>Ödeme bilgileri</legend>
                        <div class="row">
                            <asp:GridView ID="grd_cari_ödeme"
                                AutoGenerateColumns="False"
                                GridLines="None"
                                PageSize="10"
                                AllowPaging="true"
                                CssClass="mGrid_teklif"
                                PagerStyle-CssClass="pgr" DataKeyNames="SIPNO"
                                AlternatingRowStyle-CssClass="alt" runat="server" Width="90%" OnRowDataBound="grd_cari_ödeme_RowDataBound" >
                                <Columns>
                                    <asp:BoundField DataField="SIPNO" HeaderText="Sipariş"  ItemStyle-Width="30%"/>
                                    <asp:BoundField DataField="ALACAK_TUTAR" HeaderText="Tutar"  ItemStyle-Width="10%" />
                                    <asp:BoundField DataField="PARA_BIRIMI" HeaderText="Para Birimi"   ItemStyle-Width="30%"/>
                                    <asp:BoundField DataField="ALINAN_ODEME" HeaderText="Ödeme"   ItemStyle-Width="10%"/>
                                    <asp:BoundField DataField="BAKIYE" HeaderText="Bakiye"   ItemStyle-Width="15%"/>
                                </Columns>
                            </asp:GridView>

                        </div>
                    </div>--%>
                </div>
            </fieldset>

            <br />

            <asp:HiddenField ID="HDN_TEMSILCI" runat="server" />
            <fieldset>
                <legend>Müşteri Temsilcileri</legend>
                <div class="row ">
                    <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                        <asp:Label ID="Label24" runat="server" Text="Yetkili Adı/Soyadı"></asp:Label>
                        <asp:TextBox ID="txt_yet_adsoyad" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                        <asp:Label ID="Label25" runat="server" Text="Yetkili Ünvanı/Görevi"></asp:Label>
                        <asp:TextBox ID="txt_yet_unvan" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                        <asp:Label ID="Label26" runat="server" Text="Mail"></asp:Label>
                        <asp:TextBox ID="txt_yet_mail" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                        <asp:Label ID="Label27" runat="server" Text="Tel"></asp:Label>
                        <asp:TextBox ID="txt_yet_tel" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                        <asp:Label ID="Label22" runat="server" Text="Mobil"></asp:Label>
                        <asp:TextBox ID="txt_yet_tel_2" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                        <asp:Label ID="Label23" runat="server" Text="Not"></asp:Label>
                        <asp:TextBox ID="txt_notlar" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                        <asp:Label ID="Label28" runat="server" Text=".."></asp:Label>
                        <asp:Button ID="btn_ytk_save" CssClass=" btn btn-success form-control" runat="server" Text="Temsl. Ekle" OnClick="btn_ytk_save_Click" />
                    </div>
                </div>

                <br />
                <div class="row">
                    <asp:GridView ID="grd_cari_yetkili"
                        AutoGenerateColumns="False"
                        GridLines="None"
                        PageSize="10"
                        AllowPaging="true"
                        CssClass="mGrid_teklif"
                        PagerStyle-CssClass="pgr" DataKeyNames="ID"
                        AlternatingRowStyle-CssClass="alt" runat="server" Width="90%" OnSelectedIndexChanged="SEC_AV" OnRowDeleting="yetkili_delete_av" OnRowDataBound="grd_cari_yetkili_RowDataBound">
                        <Columns>
                            <asp:CommandField HeaderText="Seç" ShowSelectButton="True" ControlStyle-CssClass="btn btn-success" ButtonType="Button" ItemStyle-Width="100px" />
                            <asp:CommandField HeaderText="Sil" ShowDeleteButton="True" ControlStyle-CssClass="btn btn-success" ButtonType="Button" ItemStyle-Width="100px" />
                            <asp:BoundField DataField="ID" HeaderText="Sıra No" ItemStyle-Width="50px" />
                            <asp:BoundField DataField="YETKILI" HeaderText="Yekili Adı Soyadı" />
                            <asp:BoundField DataField="UNVAN" HeaderText="Ünvanı" />
                            <asp:BoundField DataField="MAIL" HeaderText="Mail" />
                            <asp:BoundField DataField="TEL" HeaderText="Tel" ItemStyle-Width="120px" />
                            <asp:BoundField DataField="TEL_2" HeaderText="Tel 2" ItemStyle-Width="120px" />
                            <asp:BoundField DataField="NOTE" HeaderText="Notlar" ItemStyle-Width="200px"  />
                        </Columns>

                    </asp:GridView>
                </div>
                <br />
            </fieldset>

            <br />

            <fieldset>
                <legend>Cari kart için gerekli dökümanlar</legend>
                <div class="row">
                    <div class="row">
                        <div class="col-md-12 col-lg-12 col-xs-12">
                            <div class="from-group">
                                <asp:Label ID="Label21" runat="server" Text="Cari dökümanları"></asp:Label>
                                <div cssclass="form-control">
                                    <table>
                                        <thead>
                                            <tr>
                                                <th style="width: 30%">
                                                    <asp:FileUpload ID="stok_file_upload" runat="server" Width="100%" CssClass="btn  btn-info" />
                                                </th>
                                                <th style="width: 60%">
                                                    <asp:TextBox ID="stok_file_dosya_yolu" Width="100%" runat="server" />
                                                </th>
                                                <th>
                                                    <asp:Button ID="stok_dosya_upload" runat="server" Text="Dosyayı Yükle" OnClick="stok_file_upload_file" />
                                                </th>
                                            </tr>
                                        </thead>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
            <br />

            <fieldset>
                <legend>Cari Dökümanları</legend>
                <asp:GridView ID="grd_cari_dokumanlar"
                    AutoGenerateColumns="False"
                    GridLines="None"
                    AllowPaging="True"
                    CssClass="mGrid_teklif"
                    PagerStyle-CssClass="pgr" DataKeyNames="ID"
                    AlternatingRowStyle-CssClass="alt" runat="server" OnSelectedIndexChanged="dokumnan_sec_av">
                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                    <Columns>
                        <asp:CommandField HeaderText="Seç" ShowSelectButton="True" SelectText="seç" />
                        <asp:BoundField DataField="ID" HeaderText="Dosya / Video Adı" />
                    </Columns>

                    <PagerStyle CssClass="pgr"></PagerStyle>
                </asp:GridView>

            </fieldset>


            <%-- <div class="row">
            <table style="text-align: center">
                <tr>
                    <th style="text-align: center">
                        <asp:Button ID="btn_save_update" runat="server" CssClass=" btn btn-success" Width="40%" Text="Bilgileri Kaydet / Güncelle" OnClick="save_2_av" />
                    </th>
                </tr>
            </table>
        </div>--%>
            <br />
            <br />
            <hr />
            <div class="row">
                <asp:Label ID="lbl_cr1" runat="server" Text="Kayıt Yapan" Font-Bold="true" Font-Size="Small" Font-Italic="true" Width="9%"></asp:Label>
                <asp:Label ID="lblcr_user" runat="server" Text=" " Font-Size="Small" Font-Italic="true" Width="30%"></asp:Label>

                <asp:Label ID="lbl_chg" runat="server" Text="Son Değişiklik" Font-Bold="true" Font-Size="Small" Font-Italic="true" Width="10%"></asp:Label>
                <asp:Label ID="lbl_cng_user" runat="server" Text=" " Font-Size="Small" Font-Italic="true" Width="30%"></asp:Label>
            </div>
            <br />

        </div>

        <%-- Cari kart liste ekranı --%>
        <div id="div4" style="display: none; text-align: left">
            <div class="row">
                <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                    <asp:Label ID="Label1" runat="server" Text="Cari Adı"></asp:Label>
                    <asp:TextBox ID="txt_cariadi_liste" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                    <asp:Label ID="Label85" runat="server" Text="Müşteri Yetkilsi"></asp:Label>
                    <asp:TextBox ID="txt_cari_yetkili" CssClass="form-control" runat="server"></asp:TextBox>
                </div>

                <div class="col-md-3 col-lg-2 col-sm-2 col-xs-2">
                    <div class="from-group">
                        <asp:Label ID="Label86" runat="server" Text="Cari Sınıfı"></asp:Label>
                        <asp:DropDownList ID="drp_cari_sınıf_liste" runat="server" CssClass="ddl form-control" Width="100%" AppendDataBoundItems="true">
                            <Items>
                                <asp:ListItem Text="Cari sınıfınız Giriniz" />
                            </Items>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                    <div class="from-group">
                        <asp:Label ID="Label6" runat="server" Text="Cari Ülkesi"></asp:Label>
                        <asp:DropDownList ID="drp_cari_ulke_list" runat="server" CssClass="ddl form-control" Width="100%" AppendDataBoundItems="true">
                            <Items>
                            </Items>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                    <div class="from-group">
                        <asp:Label ID="Label18" runat="server" Text="Cari İli"></asp:Label>
                        <asp:DropDownList ID="drp_cari_il_list" runat="server" CssClass="ddl form-control" Width="100%" AppendDataBoundItems="true">
                            <Items>
                            </Items>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                    <div class="from-group">
                        <asp:Label ID="Label29" runat="server" Text="Cari İlçesi"></asp:Label>
                        <asp:DropDownList ID="drp_cari_ilce_liste" runat="server" CssClass="ddl form-control" Width="100%" AppendDataBoundItems="true">
                            <Items>
                            </Items>
                        </asp:DropDownList>
                    </div>
                </div>
                <%--<div class="col-md-3 col-lg-3 col-sm-3 col-xs-3">
                    <asp:Label ID="Label6" runat="server" Text="Cari Grubu"></asp:Label>
                    <asp:DropDownList ID="drp_carigrup_liste" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                        <Items>
                            <asp:ListItem Text="Cari grubunu Giriniz" />
                        </Items>
                    </asp:DropDownList>
                </div>--%>
            </div>
            <br />
            <div class="row">
                <table style="text-align: center">
                    <tr>
                        <th style="text-align: center">
                            <asp:Button ID="btn_stokkart_listele" CssClass="btn btn-success" runat="server" Width="20%" Text="Cari Kartları Listele" OnClick="listele_av" />
                        </th>
                    </tr>
                </table>
            </div>
            <br />

            <div>
                <asp:GridView ID="grd_cariler"
                    AutoGenerateColumns="False"
                    GridLines="None"
                    AllowPaging="true"
                    CssClass="mGrid_teklif" PageSize="20"
                    PagerStyle-CssClass="pgr" DataKeyNames="ID"
                    AlternatingRowStyle-CssClass="alt" runat="server" OnSelectedIndexChanged="sec_av" OnPageIndexChanging="page_change_av">
                    <Columns>
                        <asp:CommandField HeaderText="Seç" ShowSelectButton="True" SelectText="seç" />
                        <asp:BoundField DataField="ID" HeaderText="Sıra No" />
                        <asp:BoundField DataField="CARI_KOD" HeaderText="Cari Kodu" />
                        <asp:BoundField DataField="CARI_AD" HeaderText="Cari Adı" />
                        <asp:BoundField DataField="SINIF_AD" HeaderText="Cari Sınıfı" />

                        <asp:BoundField DataField="ULKE_ADI" HeaderText="Cari Ülkesi" />
                        <asp:BoundField DataField="IL_ADI" HeaderText="Cari İli" />
                        <asp:BoundField DataField="ILCE_ADI" HeaderText="Cari İlçesi" />

                        <asp:BoundField DataField="CARI_NOT" HeaderText="Cari Not" />

                    </Columns>

                </asp:GridView>
            </div>

        </div>
    </div>

</asp:Content>
