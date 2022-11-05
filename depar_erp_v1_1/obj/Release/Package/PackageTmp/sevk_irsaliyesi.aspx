<%@ Page Title="" Language="C#" MasterPageFile="~/master_page.Master" AutoEventWireup="true" CodePage="28599" CodeBehind="sevk_irsaliyesi.aspx.cs" Inherits="depar_erp_v1_1.sevk_irsaliyesi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="margin-left: 2%; margin-right: 96%; margin-right: 2%; text-align: left">

        <table style="width: 100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td style="width: 50%"><a href="#2151624#" id="a10" class="tab_selected" onclick="ChangeForm(10)">İrsaliye Giriş</a></td>
                <td style="width: 30%"><a href="#2151625#" id="a11" class="tab" onclick="ChangeForm(11)">İrsaliye liste</a></td>
                <td style="width: 20%"><a href="#2151625#" id="a12" class="tab" onclick="ChangeForm(12)">Açık Siparişler</a></td>
            </tr>
        </table>

        <%--irsaliye giriş ekran tasarımı--%>
        <div id="div10">
            <br />
            <div class="row">
                <table style="width: 25%">
                    <tr>
                        <th>
                            <asp:Button ID="new_stok_kart" runat="server" class="btn btn-info" Text="Ekle" OnClick="save_av" />
                        </th>
                        <th>
                            <asp:Button ID="btn_update" runat="server" class="btn btn-info" Text="Güncelle" OnClick="update_av" />
                        </th>
                        <th>
                            <asp:Button ID="delete_kart" runat="server" class="btn btn-danger" Text="Sil" OnClick="delete_av" OnClientClick="return confirm('İrsaliye evrakını silmek istediginize emin misiniz?');" />
                        </th>
                        <%--<th>
                            <asp:Button ID="btn_print" runat="server" class="btn btn-info" Text="İrsaliye Yazdır" OnClick="print_av" />
                        </th>--%>
                        <th>
                            <div class="dropdown">
                                <button type="button" class="btn btn-info" data-toggle="dropdown">Yazdır<span></span></button>
                                <ul class="dropdown-menu" runat="server">
                                    <li runat="server">
                                        <button type="button" id="print1" runat="server" class="btn btn-info" style="width: 100%" onserverclick="print_av">İrsaliyeyi Mail Gönder</button></li>
                                    <li>
                                        <button type="button" id="print2" runat="server" class="btn btn-info" style="width: 100%" onserverclick="print_av2">İrsaliyey PDF olarak indir</button></li>
                                </ul>
                            </div>
                        </th>
                    </tr>
                </table>
            </div>

            <fieldset>
                <legend>İrsaliye Başlık Bilgileri</legend>
                <div class="row">
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="lbl1" runat="server" Text="Evrakno No: "></asp:Label>
                            <asp:TextBox ID="txt_siparisno_kart" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label3" runat="server" Text="İrsaliye Tarihi: "></asp:Label>
                            <input type="text" data-date-format='yyyy-mm-dd' class="form-control" id="datepicker16" name="tarih16" runat="server" clientidmode="Static">
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label2" runat="server" Text="Sevk Tarihi: "></asp:Label>
                            <input type="text" data-date-format='yyyy-mm-dd' class="form-control" id="datepicker17" name="tarih17" runat="server" clientidmode="Static">
                        </div>
                    </div>
                    <div class="col-md-6 col-xs-6 col-sm-6">
                        <div class="from-group">
                            <asp:Label ID="Label1" runat="server" Text="Sipariş No: "></asp:Label>
                            <asp:DropDownList ID="drp_ordernumber" runat="server" CssClass="ddl form-control" Width="100%" AppendDataBoundItems="true" AutoPostBack="true" OnTextChanged="siparis_search_av">
                                <Items>
                                </Items>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>

                <br />
                <div class="row">
                    <div class="col-md-3 col-xs-3 col-sm-3">
                        <div class="from-group">
                            <asp:Label ID="Label4" runat="server" Text="Müşteri Adı"></asp:Label>
                            <%--<asp:TextBox ID="drp_cari" CssClass="form-control" runat="server"></asp:TextBox>--%>
                            <asp:DropDownList ID="drp_cari" runat="server" CssClass="ddl form-control" Width="100%" AppendDataBoundItems="true">
                                <Items>
                                    <asp:ListItem Text="Cari Seçiniz" />
                                </Items>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label18" runat="server" Text="Teslim Eden"></asp:Label>
                            <asp:TextBox ID="txt_teslim_eden" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label19" runat="server" Text="Teslim Alan"></asp:Label>
                            <asp:TextBox ID="txt_teslim_alan" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label12" runat="server" Text="Araç Plaka"></asp:Label>
                            <asp:TextBox ID="txt_plaka" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-md-3 col-xs-3 col-sm-3">
                        <div class="from-group">
                            <asp:Label ID="lbl_banka" runat="server" Text="Sevk notu"></asp:Label>
                            <asp:TextBox ID="txt_sevknotu" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </fieldset>
            <br />
            <asp:HiddenField ID="LISTEDENMI" runat="server" />
            <div class="row">
                <asp:GridView ID="grd_irsaliye_detay"
                    AutoGenerateColumns="False"
                    GridLines="None"
                    PageSize="40"
                    AllowPaging="true"
                    CssClass="mGrid_teklif"
                    PagerStyle-CssClass="pgr" DataKeyNames="ID"
                    AlternatingRowStyle-CssClass="alt" runat="server">
                    <Columns>
                        <asp:CommandField HeaderText="Seç" ShowSelectButton="True" SelectText="seç" />
                        <asp:BoundField DataField="ID" HeaderText="Sıra No" />
                        <asp:BoundField DataField="SIPNO" HeaderText="İrsaliye No" />
                        <asp:BoundField DataField="CH_ADI" HeaderText="Müşteri Adı" />
                        <asp:BoundField DataField="TARIH" HeaderText="Teslim Tarihi" DataFormatString="{0:yyyy-M-dd}" />
                        <asp:BoundField DataField="TESLIM_TARIH" HeaderText="Düzenlenme Tarihi" DataFormatString="{0:yyyy-M-dd}" />
                        <asp:BoundField DataField="DEPAR_KOD" HeaderText="Depar Kod" />
                        <asp:BoundField DataField="STOK_KOD" HeaderText="Stok Kodu" />
                        <asp:BoundField DataField="STOK_AD" HeaderText="Stok Adı" />
                        <asp:TemplateField HeaderText="Miktar">
                            <ItemTemplate>
                                <asp:TextBox ID="MIKTAR" runat="server" Text='<%#Eval("MIKTAR") %>' DataFormatString="{0:N0}"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="BIRIM" HeaderText="Brm" />
                        <asp:BoundField DataField="SIP_ID" HeaderText="S.no" ItemStyle-Width="50px" />

                    </Columns>
                </asp:GridView>
            </div>

        </div>

        <%--irsaliye liste ekran tasarımı--%>
        <div id="div11" style="display: none; text-align: left">
            <br />
            <fieldset>
                <legend>İrsaliye kriterleri</legend>
                <div class="row">
                    <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                        <asp:Label ID="Label85" runat="server" Text="İrsaliye No"></asp:Label>
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

                    <div class="col-md-3 col-lg-3 col-sm-3 col-xs-3" style="text-align: center">
                        <asp:Label ID="Label36" runat="server" Text="İrsaliye Tarihi"></asp:Label>
                        <div class="form-group row">
                            <input type="text" data-date-format='yyyy-mm-dd' class="form-control" id="datepicker18" style="width: 50%" name="tarih18" runat="server" clientidmode="Static" value="Başlangıç Tarihi">
                            <input type="text" data-date-format='yyyy-mm-dd' class="form-control" id="datepicker19" style="width: 50%" name="tarih19" runat="server" clientidmode="Static" value="Bitiş Tarihi">
                        </div>
                    </div>

                    <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                        <asp:Label ID="Label23" runat="server" Text="Depar Kodu"></asp:Label>
                        <asp:TextBox ID="txt_deparkod_liste" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4 col-xs-4 col-sm-4">
                        <div class="from-group">
                            <asp:Label ID="Label25" runat="server" Text="Malzeme Kodu: "></asp:Label>
                            <asp:DropDownList ID="drp_malzemekodu_liste" runat="server" CssClass="ddl form-control" Width="100%" AppendDataBoundItems="true">
                                <Items>
                                    <asp:ListItem Text="Malzeme Seçiniz" />
                                </Items>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-4 col-lg-4 col-sm-4 col-xs-4">
                        <asp:Label ID="Label24" runat="server" Text="Malzeme Ad"></asp:Label>
                        <asp:TextBox ID="txt_stokadi_liste" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
            </fieldset>

            <br />
            <div class="row">
                <table style="text-align: center">
                    <tr>
                        <th style="text-align: center">
                            <asp:Button ID="btn_stokkart_listele" CssClass="btn btn-info" runat="server" Width="20%" Text="İrsaliye Listele" OnClick="listele_av" />
                        </th>
                    </tr>
                </table>
            </div>
            <br />

            <div class="row">
                <asp:GridView ID="grd_irsaliye_listesi"
                    AutoGenerateColumns="False"
                    GridLines="None"
                    PageSize="40"
                    AllowPaging="true"
                    CssClass="mGrid_teklif"
                    PagerStyle-CssClass="pgr" DataKeyNames="ID"
                    AlternatingRowStyle-CssClass="alt" runat="server" OnSelectedIndexChanged="sec_av">
                    <Columns>
                        <asp:CommandField HeaderText="Seç" ShowSelectButton="True" SelectText="seç" />
                        <asp:BoundField DataField="ID" HeaderText="Sıra No" />
                        <asp:BoundField DataField="EVRAKNO" HeaderText="İrsaliye No" />
                        <asp:BoundField DataField="CH_ADI" HeaderText="Müşteri Adı" />
                        <asp:BoundField DataField="SEVK_TARIH" HeaderText="Sevkiyat Tarihi" DataFormatString="{0:yyyy-M-dd}" />
                        <asp:BoundField DataField="DUZ_TARIH" HeaderText="Düzenlenme Tarihi" DataFormatString="{0:yyyy-M-dd}" />
                        <asp:BoundField DataField="DEPAR_KOD" HeaderText="Depar Kod" />
                        <asp:BoundField DataField="KOD_AD" HeaderText="Stok Adı" />
                        <asp:BoundField DataField="MIKTAR" HeaderText="Miktar" DataFormatString="{0:N0}" />
                        <asp:BoundField DataField="BIRIM" HeaderText="Brm" />

                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <%-- irsaliye içine çekilecek olan açık sipariş listesi --%>
        <div id="div12">
            <br />
            <fieldset>
                <legend>Sipariş kriterleri</legend>
                <div class="row">
                    <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                        <asp:Label ID="Label5" runat="server" Text="Sipariş No"></asp:Label>
                        <asp:TextBox ID="txt_order_no" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-4 col-lg-4 col-sm-2 col-xs-2">
                        <asp:Label ID="Label6" runat="server" Text="Müşteri Adı"></asp:Label>
                        <asp:DropDownList ID="drp_cari_sip_liste" runat="server" CssClass="ddl form-control" Width="100%" AppendDataBoundItems="true">
                            <Items>
                                <asp:ListItem Text="Cari Seçiniz" />
                            </Items>
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-3 col-lg-3 col-sm-3 col-xs-3" style="text-align: center">
                        <asp:Label ID="Label7" runat="server" Text="Sipariş Tarihi"></asp:Label>
                        <div class="form-group row">
                            <input type="text" data-date-format='yyyy-mm-dd' class="form-control" id="datepicker20" style="width: 50%" name="tarih20" runat="server" clientidmode="Static" value="Başlangıç Tarihi">
                            <input type="text" data-date-format='yyyy-mm-dd' class="form-control" id="datepicker21" style="width: 50%" name="tarih21" runat="server" clientidmode="Static" value="Bitiş Tarihi">
                        </div>
                    </div>

                    <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                        <asp:Label ID="Label8" runat="server" Text="Depar Kodu"></asp:Label>
                        <asp:TextBox ID="txt_deparkod" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>

                </div>
                <div class="row">
                    <div class="col-md-4 col-xs-4 col-sm-4">
                        <div class="from-group">
                            <asp:Label ID="Label9" runat="server" Text="Malzeme Kodu: "></asp:Label>
                            <asp:DropDownList ID="drp_malzemekodu_liste_2" runat="server" CssClass="ddl form-control" Width="100%" AppendDataBoundItems="true">
                                <Items>
                                    <asp:ListItem Text="Malzeme Seçiniz" />
                                </Items>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-4 col-lg-4 col-sm-4 col-xs-4">
                        <asp:Label ID="Label10" runat="server" Text="Malzeme Ad"></asp:Label>
                        <asp:TextBox ID="txt_malzemead" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
            </fieldset>
            <h5 style="font-size: small; color: red">Sistemdeki açık siparişler; sipariş statüsü DEPO ve KISMI SEVKİYAT olan sipariş satırlarını içerir</h5>
            <div class="row">

                <div class="col-md-3 col-lg-3 col-sm-3 col-xs-3">
                </div>
                <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                    <asp:Label ID="Label11" runat="server" Text=".."></asp:Label>
                    <asp:Button ID="btn_listele" CssClass="btn btn-success form-control" runat="server" Width="100%" Text="SiparişListele" OnClick="listele_av_2" />
                </div>
                <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                </div>
                <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                    <asp:Label ID="Label13" runat="server" Text=".."></asp:Label>
                    <asp:Button ID="btn_secimler" CssClass="btn btn-success" runat="server" Width="100%" Text="Seçili Satırları Sevket" OnClick="sevket_av" />
                </div>
                <div class="col-md-3 col-lg-3 col-sm-3 col-xs-3">
                </div>
            </div>
            <br />

            <div class="row">
                <asp:GridView ID="grd_acik_siparisler"
                    AutoGenerateColumns="False"
                    GridLines="None"
                    PageSize="40"
                    AllowPaging="true"
                    CssClass="mGrid_teklif"
                    PagerStyle-CssClass="pgr" DataKeyNames="ID"
                    AlternatingRowStyle-CssClass="alt" runat="server">
                    <Columns>
                        <asp:TemplateField HeaderText="Seç">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chc_1" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ID" HeaderText="Sıra No" />
                        <asp:BoundField DataField="SIPNO" HeaderText="Sipariş No" />
                        <asp:BoundField DataField="CH_KODU" HeaderText="Müşteri Kodu" />
                        <asp:BoundField DataField="CH_ADI" HeaderText="Müşteri Adı" />
                        <asp:BoundField DataField="TARIH" HeaderText="Sipariş Tarihi" DataFormatString="{0:yyyy-M-dd}" />
                        <asp:BoundField DataField="TESLIM_TARIH" HeaderText="Teslim Tarihi" DataFormatString="{0:yyyy-M-dd}" />
                        <asp:BoundField DataField="DEPAR_KOD" HeaderText="Depar Kod" />
                        <asp:BoundField DataField="STOK_KOD" HeaderText="Stok Kodu" />
                        <asp:BoundField DataField="STOK_AD" HeaderText="Stok Adı" />
                        <asp:BoundField DataField="MIKTAR" HeaderText="Miktar" DataFormatString="{0:N0}" />
                        <asp:BoundField DataField="KAPANAN_MIKTAR" HeaderText="Kapanan Miktar" DataFormatString="{0:N0}" />
                        <asp:BoundField DataField="BIRIM" HeaderText="Brm" />


                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
