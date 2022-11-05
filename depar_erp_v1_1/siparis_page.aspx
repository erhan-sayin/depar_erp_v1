<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/master_page.Master" AutoEventWireup="true" CodeBehind="siparis_page.aspx.cs" Inherits="depar_erp_v1_1.siparis_page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="margin-left: 2%; margin-right: 96%; margin-right: 2%; text-align: left">

        <table style="width: 100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td style="width: 40%"><a href="#2151624#" id="a5" class="tab_selected" onclick="ChangeForm(5)">Sipariş Giriş</a></td>
                <td style="width: 40%"><a href="#2151625#" id="a6" class="tab" onclick="ChangeForm(6)">Sipariş liste</a></td>
                <td style="width: 20%"><a href="#2151625#" id="a9" class="tab" onclick="ChangeForm(9)">Sipariş liste 2</a></td>
            </tr>
        </table>

        <div id="div5">
            <div class="row">
                <table style="width: 45%">
                    <tr>
                        <th>
                            <asp:Button ID="new_stok_kart" runat="server" class="btn btn-info" Text="Ekle" OnClick="save_av" />
                        </th>
                        <th>
                            <asp:Button ID="btn_update" runat="server" class="btn btn-info" Text="Güncelle" OnClick="update_av" />
                        </th>
                        <th>
                            <asp:Button ID="delete_kart" runat="server" class="btn btn-danger" Text="Sil" OnClick="delete_av" OnClientClick="return confirm('Sipariş evrakını silmek istediginize emin misiniz?');" />
                        </th>
                        <th>
                            <asp:Button ID="btn_print" runat="server" class="btn btn-info" Text="Sipariş Teyit Formu Yazdır" OnClick="print_av" />
                        </th>
                        <th>
                            <asp:Button ID="bnt_uretim" runat="server" class="btn btn-info" Text="imalat Bildirim Formu" OnClick="bnt_uretim_av" />
                        </th>
                    </tr>
                </table>

                <table style="width: 25%; text-align: right">
                    <tr>
                        <th>
                            <asp:Label ID="label_onay" runat="server" Text="Siparişi İmalata Bildir"></asp:Label>
                            <asp:CheckBox ID="onay1" runat="server" AutoPostBack="true" OnCheckedChanged="onay1_CheckedChanged" />
                        </th>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lbl_onay_info" runat="server" Font-Bold="false" Font-Italic="true" Font-Size="Small"></asp:Label>
                        </th>
                    </tr>
                </table>

                <table style="width: 20%">
                    <tr>
                        <th style="text-align: left">
                            <asp:Label ID="label17" runat="server" Text="Sipariş Durumu"></asp:Label>
                            <asp:DropDownList ID="drp_siparis_durum" runat="server" CssClass="form-control" AppendDataBoundItems="true" AutoPostBack="true" OnTextChanged="sip_durum_av">
                                <Items>
                                    <asp:ListItem Text="Onay Bekliyor" Value="0" />
                                    <asp:ListItem Text="Planlamada" Value="1" />
                                    <asp:ListItem Text="Üretimde" Value="2" />
                                    <asp:ListItem Text="Kalite Kontrolde" Value="3" />
                                    <asp:ListItem Text="Depoda" Value="4" />
                                    <asp:ListItem Text="Kısmı Sevkiyat" Value="5" />
                                    <asp:ListItem Text="Sevkiyat" Value="6" />
                                </Items>
                            </asp:DropDownList>
                        </th>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lbl_durum_onay" runat="server" Font-Bold="false" Font-Italic="true" Font-Size="Small"></asp:Label>
                        </th>
                    </tr>
                </table>

                <table style="width: 8%">
                    <tr>
                        <th>
                            <asp:ScriptManager ID="ScriptManager2" runat="server" />
                            <br />
                            <asp:Button ID="btnAdd" Text="Ödeme Giriş" CssClass="btn btn-info" OnClick="odeme_av" runat="server" />
                            <div>
                                <div id="myModal" class="modal fade" role="dialog">
                                    <div class="modal-dialog">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" class="close" data-dismiss="modal">
                                                    &times;</button>
                                                <h4 class="modal-title">Ödeme Giriş</h4>
                                            </div>
                                            <div class="modal-body">
                                                <asp:UpdatePanel ID="UpdatePanel2model" runat="server" UpdateMode="Always">
                                                    <ContentTemplate>
                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>Cari kodu:
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_carikod" runat="server" ReadOnly="true" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Cari Adı:
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_cariadi" runat="server" ReadOnly="true" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Sipariş No:
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_order_no" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Tarih:
                                                                </td>
                                                                <td>
                                                                    <%--<asp:TextBox ID="TextBox1" runat="server" ReadOnly="true" />--%>
                                                                    <input type="text" data-date-format='yyyy-mm-dd' class="form-control" id="datepicker11" name="tarih11" runat="server" clientidmode="Static">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Tutar:
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_tutar" runat="server" />
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="drp_para_birimi2" runat="server" Width="80%" AppendDataBoundItems="true">
                                                                        <Items>
                                                                            <asp:ListItem Text="Para Birim Seçiniz" />
                                                                            <asp:ListItem Text="TL" />
                                                                            <asp:ListItem Text="USD" />
                                                                            <asp:ListItem Text="EUR" />
                                                                            <asp:ListItem Text="GBP" />
                                                                        </Items>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>İskonto:
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_tutar2" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Ödeme Not:
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_odeme_not" runat="server" />
                                                                </td>

                                                            </tr>
                                                        </table>

                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <br />
                                            </div>
                                            <div class="modal-footer">
                                                <asp:Button ID="btn_odeme_1" runat="server" CssClass="btn btn-info" Text="Ödeme Gir" OnClick="odeme_av_2" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </th>
                    </tr>
                </table>

            </div>
            <br />

            <asp:HiddenField ID="TEKLIF_NO" runat="server" />
            <%-- SİPARİŞ BAŞLIK BİLGİLERİ  --%>
            <fieldset>
                <legend>Sipariş Başlık Bilgileri</legend>
                <div class="row">
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="lbl1" runat="server" Text="Sipariş No: "></asp:Label>
                            <asp:TextBox ID="txt_siparisno_kart" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label3" runat="server" Text="Sipariş Tarihi: "></asp:Label>
                            <input type="text" data-date-format='yyyy-mm-dd' class="form-control" id="datepicker1" name="tarih1" runat="server" clientidmode="Static">
                        </div>
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7">
                        <div class="from-group">
                            <asp:Label ID="Label1" runat="server" Text="Cari Bilgisi: "></asp:Label>
                            <asp:DropDownList ID="drp_carikodu_kart" runat="server" CssClass="ddl form-control" Width="100%" AppendDataBoundItems="true" AutoPostBack="true" OnTextChanged="yetkili_bul_av">
                                <Items>
                                    <asp:ListItem Text="Cari Seçiniz" />
                                </Items>
                            </asp:DropDownList>
                        </div>
                    </div>

                </div>
                <br />
                <div class="row">
                    <div class="col-md-3 col-xs-3 col-sm-3">
                        <div class="from-group">
                            <asp:Label ID="Label18" runat="server" Text="Teklif Vadesi: "></asp:Label>
                            <asp:TextBox ID="txt_vade" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-5 col-xs-5 col-sm-5">
                        <div class="from-group">
                            <asp:Label ID="Label19" runat="server" Text="Ödeme: "></asp:Label>
                            <asp:TextBox ID="txt_odeme" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1 col-xs-1 col-sm-1">
                        <div class="from-group">
                            <asp:Label ID="Label16" runat="server" Text="KDV"></asp:Label>
                            <asp:DropDownList ID="drp_kdv" runat="server" CssClass="ddl form-control" AppendDataBoundItems="true">
                                <Items>
                                    <asp:ListItem Text="%18" Selected="True" Value="18" />
                                    <asp:ListItem Text="%8" Value="8" />
                                    <asp:ListItem Text="%1" Value="1" />
                                    <asp:ListItem Text="KDV Yok" Value="0" />
                                </Items>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="lbl_banka" runat="server" Text="Banka Bilgileri"></asp:Label>
                            <asp:ListBox ID="lstbanka" runat="server" SelectionMode="Multiple" CssClass="form-control" Width="100%" AppendDataBoundItems="true">
                                <Items>
                                    <asp:ListItem Text="Banka Seçiniz" Value="0" />
                                </Items>
                            </asp:ListBox>
                        </div>
                    </div>
                </div>
                <br />

                <div class="row">
                    <div class="col-md-7 col-xs-7 col-sm-7">
                        <div class="from-group">
                            <asp:Label ID="Label4" runat="server" Text="Sipariş notları: "></asp:Label>
                            <asp:TextBox ID="txt_siparisnotlar_kart" CssClass="form-control" TextMode="MultiLine" Height="100px" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label5" runat="server" Text="Müşteri Yetkilisi: "></asp:Label>
                            <%--<asp:TextBox ID="txt_siparistemsil_kart" CssClass="form-control" runat="server"></asp:TextBox>--%>
                            <asp:DropDownList ID="drp_must_yetkili" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                <Items>
                                    <asp:ListItem Text="Yetkili seçiniz" />
                                </Items>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label22" runat="server" Text="Teslim Yeri/Şekli:"></asp:Label>
                            <asp:TextBox ID="txt_teklifteslimsekli" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-7 col-xs-7 col-sm-7">
                        <div class="from-group">
                            <asp:Label ID="Label30" runat="server" Text="Üretim için notlar: "></asp:Label>
                            <asp:TextBox ID="txt_uretim_notlar" CssClass="form-control" TextMode="MultiLine" Height="100px" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </fieldset>

            <br />

            <%-- SİPARİŞ İLE İLGLİ DÖKÜMANLAR  --%>
            <fieldset>
                <legend>Sipariş ile ilgili dökümanlar</legend>
                <div class="row">
                    <div class="row">
                        <div class="col-md-12 col-lg-12 col-xs-12">
                            <div class="from-group">
                                <asp:Label ID="Label21" runat="server" Text="Dökümanlar"></asp:Label>
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
                                                    <asp:Button ID="stok_dosya_upload" runat="server" Text="Dosyayı Yükle" OnClick="siparis_dosya_upload_Click" />
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
            <%-- SİPARİŞ İLE İLGİ DÖMAN LİSTESİ --%>
            <fieldset>
                <legend>Sipariş Dökümanları</legend>
                <asp:GridView ID="grd_siparis_dokumanlar"
                    AutoGenerateColumns="False"
                    GridLines="None"
                    AllowPaging="true"
                    CssClass="mGrid_teklif"
                    PagerStyle-CssClass="pgr" DataKeyNames="ID"
                    AlternatingRowStyle-CssClass="alt" runat="server" OnSelectedIndexChanged="dokuman_open_av">
                    <Columns>
                        <asp:CommandField HeaderText="Seç" ShowSelectButton="True" />
                        <asp:BoundField DataField="ID" HeaderText="Dosya / Video Adı" />
                    </Columns>
                </asp:GridView>

            </fieldset>
            <br />
            <div>
                <asp:HiddenField ID="stok_kodu_hdn" runat="server" />
            </div>
            <%-- SİPARİŞ SATIR BİLGİLERİ GİRİŞ ALANI --%>
            <fieldset>
                <legend>Sipariş Satırları</legend>
                <asp:HiddenField ID="hdn_sip_satir" runat="server" />
                <div class="row">
                    <div class="col-md-3 col-xs-3 col-sm-3">
                        <div class="from-group">
                            <asp:Label ID="Label6" runat="server" Text="Malzeme Kodu: "></asp:Label>
                            <asp:DropDownList ID="drp_siparis_malz_kart" runat="server" CssClass="ddl form-control" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="revizyon_bul_av">
                                <Items>
                                    <asp:ListItem Text="Malzeme Seçiniz" />
                                </Items>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="col-md-1 col-xs-1 col-sm-1">
                        <div class="from-group">
                            <asp:Label ID="Label2" runat="server" Text="Revizyon: "></asp:Label>
                            <asp:DropDownList ID="drp_malzeme_revizyon" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                <Items>
                                    <asp:ListItem Text="Revizyon seçiniz" />
                                    <asp:ListItem Text="R" Value="R" />
                                </Items>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label7" runat="server" Text="Malzeme Adı: "></asp:Label>
                            <asp:TextBox ID="txt_siparismalzemeadi_kart" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1 col-xs-1 col-sm-1">
                        <div class="from-group">
                            <asp:Label ID="Label8" runat="server" Text="Miktar "></asp:Label>
                            <asp:TextBox ID="txt_siparismiktar_kart" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-md-1 col-xs-1 col-sm-1">
                        <div class="from-group">
                            <asp:Label ID="Label9" runat="server" Text="Birim "></asp:Label>
                            <asp:DropDownList ID="drp_siparis_birim_kart" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                <Items>
                                    <asp:ListItem Text="Birim Seçiniz" Value="1" />
                                    <asp:ListItem Text="AD" Value="2" />
                                    <asp:ListItem Text="KG" Value="3" />
                                    <asp:ListItem Text="TAKIM" Value="4" />
                                    <asp:ListItem Text="Çift" Value="5" Selected="True" />
                                </Items>
                            </asp:DropDownList>

                        </div>
                    </div>

                    <div class="col-md-1 col-xs-1 col-sm-1">
                        <div class="from-group">
                            <asp:Label ID="Label14" runat="server" Text="Birim Fiyat"></asp:Label>
                            <asp:TextBox ID="txt_birimfiyat_kart" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1 col-xs-1 col-sm-1">
                        <div class="from-group">
                            <asp:Label ID="Label15" runat="server" Text="Para Birimi"></asp:Label>
                            <asp:DropDownList ID="drp_parabirimi_kart" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                <Items>

                                    <asp:ListItem Text="TL" />
                                    <asp:ListItem Text="USD" Selected="True" />
                                    <asp:ListItem Text="EUR" />
                                    <asp:ListItem Text="GBP" />
                                </Items>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label12" runat="server" Text="Teslim Tarihi: "></asp:Label>
                            <input type="text" data-date-format='yyyy-mm-dd' class="form-control" id="datepicker2" name="tarih2" runat="server" clientidmode="Static">
                        </div>
                    </div>
                </div>

                <div class="row" style="margin-left: 7%; margin-right: 7%; text-align: center">
                    <div class="col-md-10 col-xs-10 col-sm-10">
                        <div class="from-group">
                            <asp:Label ID="Label10" runat="server" Text="Sipariş Satır Notları "></asp:Label>
                            <asp:TextBox ID="txt_siparis_satırnot_kart" CssClass="form-control" TextMode="MultiLine" Height="50px" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </fieldset>

            <br />
            <div class="row">
                <table style="text-align: center">
                    <tr>
                        <th style="text-align: center">
                            <asp:Button ID="btn_save_update" runat="server" CssClass=" btn btn-success" Width="40%" Text="Sipariş Satır ekle" OnClick="save_av_2" />
                        </th>
                    </tr>
                </table>
            </div>
            <br />
            <%-- SİPARİŞ DETAY SATIRLARI TABLOSU --%>



            <div clas="row">
                <asp:GridView ID="grd_siparis_detay"
                    AutoGenerateColumns="False"
                    GridLines="None"
                    AllowPaging="true"
                    CssClass="mGrid_teklif"
                    PagerStyle-CssClass="pgr" DataKeyNames="ID"
                    AlternatingRowStyle-CssClass="alt" runat="server" OnRowDeleting="delete_satir_av" OnSelectedIndexChanged="sec_av" OnRowDataBound="grd_siparis_detay_RowDataBound">
                    <Columns>
                        <asp:CommandField HeaderText="Seç" ShowSelectButton="True" SelectText="Seç" ControlStyle-CssClass="btn btn-success" ButtonType="Button" />
                        <asp:CommandField HeaderText="Sil" ShowDeleteButton="True" SelectText="Sil" ControlStyle-CssClass="btn btn-success" ButtonType="Button" />
                        <asp:BoundField DataField="ID" HeaderText="Sıra No" />
                        <asp:BoundField DataField="STOK_KOD" HeaderText="Malzeme Kodu" />
                        <asp:BoundField DataField="STOK_AD" HeaderText="Malzeme Adı" />
                        <asp:BoundField DataField="REVIZYON" HeaderText="Revizyon No" />
                        <asp:BoundField DataField="BIRIM_FIYAT" HeaderText="B.fiyat" />
                        <asp:BoundField DataField="MIKTAR" HeaderText="Miktar" DataFormatString="{0:N0}" />
                        <asp:BoundField DataField="KALAN_MIKTAR" HeaderText="K.Miktar" DataFormatString="{0:N0}" />
                        <asp:BoundField DataField="BIRIM" HeaderText="Birim" />
                        <asp:BoundField DataField="TOPLAM_TUTAR" HeaderText="Toplam Tutar" />
                        <asp:BoundField DataField="TESLIM_TARIH" HeaderText="Teslim Tarihi" DataFormatString="{0:yyyy-M-dd}" />

                        <%--<asp:TemplateField HeaderText="Teslim Tarihi">
                                <ItemTemplate>
                                    <input typ="text" data-date-format='yyyy-mm-dd' class="form-control" id="datepicker23" name="tarih23" text='<%# Eval("TESLIM_TARIH") %>' runat="server" clientidmode="Static">
                                    <asp:TextBox ID="txtDate" runat="server" class="date"></asp:TextBox>
                                </ItemTemplate>
                                <ControlStyle Width="90px" />
                            </asp:TemplateField>--%>

                        <asp:BoundField DataField="SATIR_NOT" HeaderText="Satır Açıklama" />
                        <asp:TemplateField HeaderText="Kırank Ø Min">
                            <ItemTemplate>
                                <asp:TextBox ID="TXTK_MIN" runat="server" Text='<%# Eval("K_MIN") %>' onblur="cidar_hesapla(this.value)"></asp:TextBox>
                            </ItemTemplate>
                            <ControlStyle Width="55px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Kırank Ø Max">
                            <ItemTemplate>
                                <asp:TextBox ID="TXTK_MAX" runat="server" Text='<%# Eval("K_MAX") %>' onblur="cidar_hesapla(this.value)"></asp:TextBox>
                            </ItemTemplate>
                            <ControlStyle Width="55px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Yuva Ø Min">
                            <ItemTemplate>
                                <asp:TextBox ID="TXTYU_MIN" runat="server" Text='<%# Eval("YU_MIN") %>' onblur="cidar_hesapla(this.value)"></asp:TextBox>
                            </ItemTemplate>
                            <ControlStyle Width="55px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Yuva Ø Max">
                            <ItemTemplate>
                                <asp:TextBox ID="TXTYU_MAX" runat="server" Text='<%# Eval("YU_MAX") %>' onblur="cidar_hesapla(this.value)"></asp:TextBox>
                            </ItemTemplate>
                            <ControlStyle Width="55px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Yağ Boşl. Ø Min">
                            <ItemTemplate>
                                <asp:TextBox ID="TXTYAG_MIN" runat="server" Text='<%# Eval("YAG_MIN") %>' onblur="cidar_hesapla(this.value)"></asp:TextBox>
                            </ItemTemplate>
                            <ControlStyle Width="55px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Yağ Boşl. Ø Max">
                            <ItemTemplate>
                                <asp:TextBox ID="TXTYAG_MAX" runat="server" Text='<%# Eval("YAG_MAX") %>' onblur="cidar_hesapla(this.value)"></asp:TextBox>
                            </ItemTemplate>
                            <ControlStyle Width="55px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Cidar Ø Min">
                            <ItemTemplate>
                                <asp:TextBox ID="TXTCI_MIN" runat="server" Text='<%# Eval("CI_MIN") %>'></asp:TextBox>
                                <%--<input runat="server" type="text" id="TXTCI_MIN" />--%>
                            </ItemTemplate>
                            <ControlStyle Width="55px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cidar Ø Max">
                            <ItemTemplate>
                                <asp:TextBox ID="TXTCI_MAX" runat="server" Text='<%# Eval("CI_MAX") %>'></asp:TextBox>
                                <%--<input runat="server" type="text" id="TXTCI_MAX" />--%>
                            </ItemTemplate>
                            <ControlStyle Width="55px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

            <br />
            <br />

        </div>

        <%-- SİPARİŞ LİSTE BİLGİLERİ EKRANIDIR. --%>
        <div id="div6" style="display: none; text-align: left">
            <br />
            <fieldset>
                <legend>Sipariş kriterleri</legend>
                <div class="row">
                    <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                        <asp:Label ID="Label85" runat="server" Text="Sipariş Kodu"></asp:Label>
                        <asp:TextBox ID="txt_siparino_liste" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-4 col-lg-4 col-sm-2 col-xs-2">
                        <asp:Label ID="Label86" runat="server" Text="Müşteri Adı"></asp:Label>
                        <asp:DropDownList ID="drp_cari_liste" runat="server" CssClass="ddl form-control" Width="100%" AppendDataBoundItems="true">
                            <Items>
                                <asp:ListItem Text="Cari Seçiniz" />
                            </Items>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                        <asp:Label ID="Label11" runat="server" Text="Sipariş Not"></asp:Label>
                        <asp:TextBox ID="txt_siparisnot_liste" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                        <asp:Label ID="Label13" runat="server" Text="Sipariş Tarihi"></asp:Label>
                        <input type="text" data-date-format='yyyy-mm-dd' class="form-control" id="datepicker3" name="tarih3" runat="server" clientidmode="Static">
                    </div>

                    <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                        <asp:Label ID="label20" runat="server" Text="Sipariş Durumu"></asp:Label>
                        <asp:DropDownList ID="drp_sip_durum_liste" runat="server" CssClass="form-control" Width="100%" AppendDataBoundItems="true">
                            <Items>
                                <asp:ListItem Text="Hepsi" Value="99" />
                                <asp:ListItem Text="Onay Bekliyor" Value="0" />
                                <asp:ListItem Text="Planlamada" Value="1" />
                                <asp:ListItem Text="Üretimde" Value="2" />
                                <asp:ListItem Text="Kalite Kontrolde" Value="3" />
                                <asp:ListItem Text="Depoda" Value="4" />
                                <asp:ListItem Text="Kısmı Sevkiyat" Value="5" />
                                <asp:ListItem Text="Sevkiyat" Value="6" />
                            </Items>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-1 col-lg-1 col-sm-1 col-xs-1">
                        <asp:Label ID="Label23" runat="server" Text="Depar Kodu"></asp:Label>
                        <asp:TextBox ID="txt_deparkod_liste" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-3 col-xs-3 col-sm-3">
                        <div class="from-group">
                            <asp:Label ID="Label25" runat="server" Text="Malzeme Kodu: "></asp:Label>
                            <asp:DropDownList ID="drp_malzemekodu_liste" runat="server" CssClass="ddl form-control" Width="100%" AppendDataBoundItems="true">
                                <Items>
                                    <asp:ListItem Text="Malzeme Seçiniz" />
                                </Items>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                        <asp:Label ID="Label24" runat="server" Text="Stok Ad"></asp:Label>
                        <asp:TextBox ID="txt_stokadi_liste" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>

                    <div class="col-md-3 col-lg-3 col-sm-3 col-xs-3" style="text-align: center">
                        <asp:Label ID="Label26" runat="server" Text="Sipariş Tarihi"></asp:Label>
                        <div class="form-group row">
                            <input type="text" data-date-format='yyyy-mm-dd' class="form-control" id="datepicker7" style="width: 50%" name="tarih7" runat="server" clientidmode="Static" value="Başlangıç Tarihi">
                            <input type="text" data-date-format='yyyy-mm-dd' class="form-control" id="datepicker8" style="width: 50%" name="tarih8" runat="server" clientidmode="Static" value="Bitiş Tarihi">
                        </div>
                    </div>

                    <div class="col-md-3 col-lg-3 col-sm-3 col-xs-3" style="text-align: center">
                        <asp:Label ID="Label27" runat="server" Text="Teslim Tarihi"></asp:Label>
                        <div class="form-group row">
                            <input type="text" data-date-format='yyyy-mm-dd' class="form-control" id="datepicker9" style="width: 50%" name="tarih9" runat="server" clientidmode="Static" value="Başlangıç Tarihi">
                            <input type="text" data-date-format='yyyy-mm-dd' class="form-control" id="datepicker10" style="width: 50%" name="tarih10" runat="server" clientidmode="Static" value="Bitiş Tarihi">
                        </div>
                    </div>
                </div>
            </fieldset>

            <br />
            <div class="row">
                <table style="text-align: center">
                    <tr>
                        <th style="text-align: center">
                            <asp:Button ID="btn_stokkart_listele" CssClass="btn btn-success" runat="server" Width="40%" Text="SiparişListele" OnClick="listele_av" />
                        </th>
                    </tr>
                </table>
            </div>
            <br />

            <div class="row">
                <asp:GridView ID="grd_siparis_listesi"
                    AutoGenerateColumns="False"
                    GridLines="None"
                    AllowSorting="true"
                    PageSize="40"
                    AllowPaging="true"
                    CssClass="mGrid_teklif"
                    PagerStyle-CssClass="pgr" DataKeyNames="ID"
                    AlternatingRowStyle-CssClass="alt" runat="server" OnSelectedIndexChanged="sec_av_liste" OnSorting="SORT_AV" OnPageIndexChanging="PAGE_LISTE_AV">
                    <Columns>
                        <asp:CommandField HeaderText="Seç" ShowSelectButton="True" SelectText="seç" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="ID" HeaderText="Sıra No" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="SIPNO" HeaderText="Sipariş No" SortExpression="SIPNO" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="CH_ADI" HeaderText="Müşteri Adı" SortExpression="CH_ADI" />
                        <asp:BoundField DataField="TARIH" HeaderText="Sipariş Tarihi" DataFormatString="{0:yyyy-M-dd}" SortExpression="TARIH" />
                        <asp:BoundField DataField="TESLIM_TARIH" HeaderText="Teslim Tarihi" DataFormatString="{0:yyyy-M-dd}" SortExpression="TESLIM_TARIH" />
                        <asp:BoundField DataField="DEPAR_KOD" HeaderText="Depar Kod" ItemStyle-Width="100px" SortExpression="DEPAR_KOD" />
                        <asp:BoundField DataField="STOK_AD" HeaderText="Stok Adı" SortExpression="STOK_AD" />
                        <asp:BoundField DataField="MIKTAR" HeaderText="Miktar" DataFormatString="{0:N0}" />
                        <asp:BoundField DataField="BIRIM" HeaderText="Brm" ItemStyle-Width="50px" SortExpression="BIRIM" />
                        <asp:BoundField DataField="BIRIM_FIYAT" HeaderText="Brm Fiyat" />
                        <asp:BoundField DataField="TOPLAM_TUTAR" HeaderText="Toplam Tutar" DataFormatString="{0:N2}" />
                        <asp:BoundField DataField="PARA_BIRIMI" HeaderText="Para Birimi" ItemStyle-Width="70px" SortExpression="PARA_BIRIMI" />
                        <asp:BoundField DataField="DURUM_AD" HeaderText="Sip.Durumu" ItemStyle-Width="100px" SortExpression="DURUM_AD" />
                        <%--<asp:BoundField DataField="NOTLAR" HeaderText="Sipariş Notlar" />--%>
                    </Columns>
                </asp:GridView>
            </div>
        </div>


        <%-- ÖDEME BİLGİLERİ GİRİŞ EKRANIDIR. --%>
        <asp:HiddenField ID="HDN_ODEME" runat="server" />
        <div id="div9" style="display: none; text-align: left">
            <br />
            <fieldset>
                <legend>Sipariş kriterleri</legend>
                <div class="row">
                    <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                        <asp:Label ID="Label28" runat="server" Text="Sipariş Kodu"></asp:Label>
                        <asp:TextBox ID="txt_siparino_liste_2" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-4 col-lg-4 col-sm-2 col-xs-2">
                        <asp:Label ID="Label29" runat="server" Text="Müşteri Adı"></asp:Label>
                        <asp:DropDownList ID="drp_cari_liste_2" runat="server" CssClass="ddl form-control" Width="100%" AppendDataBoundItems="true">
                            <Items>
                                <asp:ListItem Text="Cari Seçiniz" />
                            </Items>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                        <asp:Label ID="label32" runat="server" Text="Sipariş Durumu"></asp:Label>
                        <asp:DropDownList ID="drp_sip_durum_liste_2" runat="server" CssClass="form-control" Width="100%" AppendDataBoundItems="true">
                            <Items>
                                <asp:ListItem Text="Durum Seçiniz" Value="10" />
                                <asp:ListItem Text="Onay Bekliyor" Value="0" />
                                <asp:ListItem Text="Planlamada" Value="1" />
                                <asp:ListItem Text="Üretimde" Value="2" />
                                <asp:ListItem Text="Sevk Bekliyor" Value="3" />
                                <asp:ListItem Text="Kapandı" Value="4" />
                            </Items>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3 col-lg-3 col-sm-3 col-xs-3" style="text-align: center">
                        <asp:Label ID="Label36" runat="server" Text="Sipariş Tarihi"></asp:Label>
                        <div class="form-group row">
                            <input type="text" data-date-format='yyyy-mm-dd' class="form-control" id="datepicker12" style="width: 50%" name="tarih12" runat="server" clientidmode="Static" value="Başlangıç Tarihi">
                            <input type="text" data-date-format='yyyy-mm-dd' class="form-control" id="datepicker13" style="width: 50%" name="tarih13" runat="server" clientidmode="Static" value="Bitiş Tarihi">
                        </div>
                    </div>

                    <div class="col-md-3 col-lg-3 col-sm-3 col-xs-3" style="text-align: center">
                        <asp:Label ID="Label37" runat="server" Text="Teslim Tarihi"></asp:Label>
                        <div class="form-group row">
                            <input type="text" data-date-format='yyyy-mm-dd' class="form-control" id="datepicker14" style="width: 50%" name="tarih14" runat="server" clientidmode="Static" value="Başlangıç Tarihi">
                            <input type="text" data-date-format='yyyy-mm-dd' class="form-control" id="datepicker15" style="width: 50%" name="tarih15" runat="server" clientidmode="Static" value="Bitiş Tarihi">
                        </div>
                    </div>
                </div>
            </fieldset>

            <br />
            <div class="row">
                <table style="text-align: center">
                    <tr>
                        <th style="text-align: center">
                            <asp:Button ID="Button1" CssClass="btn btn-success" runat="server" Width="40%" Text="SiparişListele" OnClick="listele_av_2" />
                        </th>
                    </tr>
                </table>
            </div>
            <br />

            <div class="row">
                <asp:GridView ID="grd_siparis_listesi_2"
                    AutoGenerateColumns="False"
                    GridLines="None"
                    PageSize="40"
                    AllowPaging="true"
                    CssClass="mGrid_teklif"
                    PagerStyle-CssClass="pgr" DataKeyNames="TARIH" ForeColor="Black"
                    AlternatingRowStyle-CssClass="alt" runat="server" OnSelectedIndexChanged="sec_av_liste" OnRowDataBound="grd_siparis_listesi_RowDataBound_2" OnRowCommand="grd_siparis_listesi_2_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="Ödeme Gir">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnEdit" runat="server" Text="Ödeme Gir" CssClass="btn btn-info"
                                    OnClick="Display"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Sipariş No">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnEdit_2" runat="server" Text='<%# Eval("SIPNO") %>' OnClick="Display_2"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField DataField="SIPNO" HeaderText="Sipariş No" />--%>
                        <asp:BoundField DataField="CH_KODU" HeaderText="Müşteri Kodu" />
                        <asp:BoundField DataField="CH_ADI" HeaderText="Müşteri Adı" />
                        <asp:BoundField DataField="TARIH" HeaderText="Sipariş Tarihi" DataFormatString="{0:yyyy-M-dd}" />
                        <asp:BoundField DataField="ALACAK_TUTAR" HeaderText="Alacak Tutar" DataFormatString="{0:N2}" />
                        <asp:BoundField DataField="PARA_BIRIMI" HeaderText="Para Birimi" />
                        <asp:BoundField DataField="ALINAN_ODEME" HeaderText="Alınan Ödeme" DataFormatString="{0:N2}" />
                        <asp:BoundField DataField="BAKIYE" HeaderText="Bakiye" DataFormatString="{0:N2}" />
                        <asp:BoundField DataField="ISKONTO" HeaderText="İskonto" DataFormatString="{0:N2}" />
                          <asp:TemplateField HeaderText="Ekstre Al">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnEdit_3" runat="server" Text='<%# Eval("EKSTRE") %>' OnClick="EKSTRE_AV"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>
            </div>
            <br />



            <%-- MODAL 2 BASLIYOR --%>
            <div id="myModal_3" class="modal fade" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">
                                &times;</button>
                            <h4 class="modal-title">Sipariş Detay Bilgileri</h4>
                        </div>

                        <div class="modal-body">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                <ContentTemplate>

                                    <div class="row">
                                        <asp:GridView ID="grd_popup_siparisler"
                                            AutoGenerateColumns="False"
                                            GridLines="None"
                                            PageSize="40"
                                            AllowPaging="true"
                                            CssClass="mGrid_teklif"
                                            PagerStyle-CssClass="pgr" DataKeyNames="ID" ForeColor="Black"
                                            AlternatingRowStyle-CssClass="alt" runat="server">
                                            <Columns>
                                                <asp:BoundField DataField="STOK_KOD" HeaderText="Stok Kodu" ItemStyle-Width="90px" />
                                                <asp:BoundField DataField="STOK_AD" HeaderText="Stok Adı" />
                                                <asp:BoundField DataField="MIKTAR" HeaderText="Miktar" DataFormatString="{0:N2}" ItemStyle-Width="50px" />
                                                <asp:BoundField DataField="BIRIM" HeaderText="Birim" ItemStyle-Width="50px" />
                                                <asp:BoundField DataField="BIRIM_FIYAT" HeaderText="Birim Fiyat" DataFormatString="{0:N2}" />
                                                <asp:BoundField DataField="PARA_BIRIMI" HeaderText="Para Birimi" ItemStyle-Width="50px" />
                                                <asp:BoundField DataField="TOPLAM_TUTAR" HeaderText="Toplam Tutar" DataFormatString="{0:N2}" ItemStyle-Width="80px" />
                                            </Columns>

                                        </asp:GridView>
                                    </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <br />
                        </div>

                    </div>
                </div>
            </div>
            <%-- MODAL 2 BİTİYOR --%>


            <div id="myModal_2" class="modal fade" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">
                                &times;</button>
                            <h4 class="modal-title">Ödeme Giriş</h4>
                        </div>

                        <div class="modal-body">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>Cari kodu:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox1" runat="server" ReadOnly="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Cari Adı:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox2" runat="server" ReadOnly="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Sipariş No:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox3" runat="server" Width="120px" />
                                            </td>
                                            <td>Sip.Tutar</td>
                                            <td>
                                                <asp:Label ID="sip_txt_tutar" runat="server" Width="130px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Tarih:
                                            </td>
                                            <td>
                                                <input type="text" data-date-format='yyyy-mm-dd' class="form-control" id="datepicker22" name="Tarih22" runat="server" clientidmode="Static">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Tutar:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox4" runat="server" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DropDownList1" runat="server" Width="80%" AppendDataBoundItems="true">
                                                    <Items>
                                                        <asp:ListItem Text="Para Birim Seçiniz" />
                                                        <asp:ListItem Text="TL" />
                                                        <asp:ListItem Text="USD" />
                                                        <asp:ListItem Text="EUR" />
                                                        <asp:ListItem Text="GBP" />
                                                    </Items>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>İskonto:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox5" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Ödeme Not:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox6" runat="server" />
                                            </td>

                                        </tr>
                                    </table>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <br />
                        </div>

                        <div class="modal-footer">
                            <asp:Button ID="Button3" runat="server" CssClass="btn btn-info" Text="Ödeme Gir" OnClick="odeme_av_3" />
                        </div>
                    </div>
                </div>
            </div>

            <div id="myModal_4" class="modal fade" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">
                                &times;</button>
                            <h4 class="modal-title">Cari ekstre</h4>
                        </div>

                        <div class="modal-body">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>Cari kodu:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_ekstre_carikod" runat="server" ReadOnly="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Cari Adı:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_ekstre_cariad" runat="server" ReadOnly="true" />
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td>Tarih:
                                            </td>
                                            <td>
                                                <input type="text" data-date-format='yyyy-mm-dd' class="form-control" id="datepicker24" name="Tarih24" runat="server" clientidmode="Static">
                                            </td>
                                            <td>
                                                <input type="text" data-date-format='yyyy-mm-dd' class="form-control" id="datepicker25" name="Tarih25" runat="server" clientidmode="Static">
                                            </td>
                                        </tr>
                                       
                                    </table>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <br />
                        </div>

                        <div class="modal-footer">
                            <asp:Button ID="Button2" runat="server" CssClass="btn btn-info" Text="Ekstre Al" OnClick="ekstreal_av2" />
                        </div>
                    </div>
                </div>
            </div>

        </div>

    </div>
</asp:Content>

<asp:Content ContentPlaceHolderID="scripts" runat="server">

    <script type="text/javascript">
        function cidar_hesapla() {
            var grid = document.getElementById("<%= grd_siparis_detay.ClientID%>");
            for (var i = 0; i < grid.rows.length - 1; i++) {
                var k_min = $("input[id*=TXTK_MIN]");
                var k_max = $("input[id*=TXTK_MAX]");
                var yu_min = $("input[id*=TXTYU_MIN]");
                var yu_max = $("input[id*=TXTYU_MAX]");
                var ya_min = $("input[id*=TXTYAG_MIN]");
                var ya_max = $("input[id*=TXTYAG_MAX]");

                var num1 = parseFloat(yu_min[i].value.replace(',', '.')).toFixed(3);
                var num2 = parseFloat(k_max[i].value.replace(',', '.')).toFixed(3);
                var num3 = parseFloat(ya_max[i].value.replace(',', '.')).toFixed(3);
                var sonuc = num1 - num2 - num3;

                var num1_1 = parseFloat(yu_max[i].value.replace(',', '.')).toFixed(3);
                var num2_2 = parseFloat(k_min[i].value.replace(',', '.')).toFixed(3);
                var num3_3 = parseFloat(ya_min[i].value.replace(',', '.')).toFixed(3);
                var sonuc_1 = num1_1 - num2_2 - num3_3;

                if (sonuc > 0) {
                    //grid.rows[i + 1].cells[19].innerHTML = sonuc.toFixed(2);
                    grid.rows[i + 1].getElementsByTagName("input")[8].value = sonuc.toFixed(2);
                    /*document.getElementById("input").value = data + "69";*/
                }
                if (sonuc_1 > 0) {
                    //grid.rows[i + 1].cells[20].innerHTML = sonuc_1.toFixed(2);
                    grid.rows[i + 1].getElementsByTagName("input")[9].value = sonuc_1.toFixed(2);
                }
            }
        }
    </script>

</asp:Content>
