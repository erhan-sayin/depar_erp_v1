<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/master_page.Master" AutoEventWireup="true" CodeBehind="stok_kartlari.aspx.cs" Inherits="depar_erp_v1_1.stok_kartlari" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="margin-left: 2%; margin-right: 96%; margin-right: 2%; text-align: left">

        <table style="width: 100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td style="width: 50%"><a href="#2151624#" id="a1" class="tab_selected" onclick="ChangeForm(1)">Stok Kartı Giriş</a></td>
                <td style="width: 50%"><a href="#2151625#" id="a2" class="tab" onclick="ChangeForm(2)">stok Kartı liste</a></td>
            </tr>
        </table>

        <%-- stok kartı giriş bilgileri ekranı --%>
        <div id="div1">
            <br />
            <div class="row">
                <table style="width: 35%">
                    <tr>
                        <th>
                            <asp:Button ID="new_stok_kart" runat="server" class="btn btn-info" Text="Ekle" OnClick="save_kart_av" />
                        </th>
                        <th>
                            <asp:Button ID="btn_save_update" runat="server" CssClass=" btn btn-info" Text="Güncelle" OnClick="update_av" />
                        </th>
                        <th>
                            <asp:Button ID="delete_kart" runat="server" class="btn btn-danger" Text="Sil" OnClick="delete_kart_av" OnClientClick="return confirm('Stok kartını silmek istediginize emin misiniz?');" />
                        </th>
                        <%--<th>
                            <asp:Button ID="Button1" runat="server" class="btn btn-info" Text="Ürün Spec. Yazdir." OnClick="print_spec_av" />
                        </th>--%>
                    </tr>
                </table>

            </div>
            <%-- stok kartı oluşturmak için gerekli olan grup kodları alanı --%>
            <fieldset>
                <legend>Grup Kodları </legend>
                <div class="row" style="margin-left: 2%; margin-bottom: 2%">
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label9" runat="server" Text="Ana Grubu"></asp:Label>
                            <asp:DropDownList ID="drp_gk_1_stok" runat="server" CssClass="form-control" AppendDataBoundItems="true" AutoPostBack="true" OnTextChanged="gk_1_av">
                                <Items>
                                    <asp:ListItem Text="Grup kodu 1 Seçiniz" />
                                </Items>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label2" runat="server" Text="Kategori"></asp:Label>
                            <asp:DropDownList ID="drp_gk_2_stok" runat="server" CssClass="form-control" AppendDataBoundItems="true" OnTextChanged="gk_2_av">
                                <Items>
                                    <asp:ListItem Text="Grup kodu 2 Seçiniz" />
                                </Items>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label3" runat="server" Text="Ürün Grubu"></asp:Label>
                            <asp:DropDownList ID="drp_gk_3_stok" runat="server" CssClass="form-control" AppendDataBoundItems="true" OnTextChanged="gk_3_av">
                                <Items>
                                    <asp:ListItem Text="Grup kodu 3 Seçiniz" />
                                </Items>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label4" runat="server" Text="Ürün Tipi"></asp:Label>
                            <asp:DropDownList ID="drp_gk_4_stok" runat="server" CssClass="form-control" AppendDataBoundItems="true" OnTextChanged="gk_4_av">
                                <Items>
                                    <asp:ListItem Text="Grup kodu 4 Seçiniz" />
                                </Items>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label13" runat="server" Text="Marka"></asp:Label>
                            <asp:DropDownList ID="drp_gk_5_stok" runat="server" CssClass="form-control" AppendDataBoundItems="true" OnTextChanged="gk_5_av">
                                <Items>
                                    <asp:ListItem Text="Grup kodu 5 Seçiniz" />
                                </Items>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label43" runat="server" Text="Model"></asp:Label>
                            <asp:DropDownList ID="drp_gk_6_stok" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                <Items>
                                    <asp:ListItem Text="Grup kodu 6 Seçiniz" />
                                </Items>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <table style="text-align: center; width: 100%; height: 80px">
                            <tr>
                                <th style="text-align: center">
                                    <asp:Button ID="btn_stokkod_olustur" runat="server" CssClass=" btn btn-success" Text="Stok Kodunu oluştur" OnClick="stokkodu_oluştur_av" />
                                </th>
                            </tr>
                        </table>
                    </div>
                </div>
            </fieldset>

            <br />
            <%-- Stok kartlı için detay tanım alanları --%>
            <fieldset>
                <legend>Stok kodu Bilgileri</legend>
                <div class="row" style="margin-left: 1%; margin-bottom: 2%">
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label20" runat="server" Text="Depar Kodu: "></asp:Label>
                            <asp:TextBox ID="txt_stokid" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="lbl1" runat="server" Text="Stok Kodu: "></asp:Label>
                            <asp:TextBox ID="txt_stokkodu_kart" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3 col-xs-3 col-sm-3">
                        <div class="from-group">
                            <asp:Label ID="Label1" runat="server" Text="Stok Açıklama: "></asp:Label>
                            <asp:TextBox ID="txt_stokadi1_kart" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3 col-xs-3 col-sm-3">
                        <div class="from-group">
                            <asp:Label ID="Label5" runat="server" Text="Stok Adı 2: "></asp:Label>
                            <asp:TextBox ID="txt_stokadi2_kart" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label30" runat="server" Text="Eski Kod: "></asp:Label>
                            <asp:TextBox ID="txt_stokkodu_eskikod_kart" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row" style="margin-left: 1%; margin-bottom: 2%">
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label22" runat="server" Text="Üretici Parça No: "></asp:Label>
                            <asp:TextBox ID="txt_parcano1" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label23" runat="server" Text="Teknik Resim No: "></asp:Label>
                            <asp:TextBox ID="txt_parcano2" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label24" runat="server" Text="NSN No: "></asp:Label>
                            <asp:TextBox ID="txt_parcano3" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label25" runat="server" Text="Alt Yatak No: "></asp:Label>
                            <asp:TextBox ID="txt_parcano4" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label26" runat="server" Text="Üst Yatak No: "></asp:Label>
                            <asp:TextBox ID="txt_parcano5" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label27" runat="server" Text="Diğer No: "></asp:Label>
                            <asp:TextBox ID="txt_parcano6" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </fieldset>
            <br />
            <%-- hammadde ölçü bilgileri ekranı --%>
            <fieldset>
                <legend>Hammadde Ölçü bilgileri</legend>
                <div class="row" style="margin-left: 1%; margin-bottom: 2%">
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label14" runat="server" Text="En"></asp:Label>
                            <asp:TextBox ID="txt_olcu_en_" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label15" runat="server" Text="Boy"></asp:Label>
                            <asp:TextBox ID="txt_olcu_boy" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label16" runat="server" Text="Çap/Et Kalınlığı"></asp:Label>
                            <asp:TextBox ID="txt_olcu_kalinlik" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <%-- <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label17" runat="server" Text="Ölçü 2 Max: "></asp:Label>
                            <asp:TextBox ID="txt_olcu_2_max" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label18" runat="server" Text="Ölçü 3 Min: "></asp:Label>
                            <asp:TextBox ID="txt_olcu_3_min" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label19" runat="server" Text="Ölçü 3 Max: "></asp:Label>
                            <asp:TextBox ID="txt_olcu_3_max" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>--%>
                </div>
            </fieldset>
            <br />

            <div class="row">
                <table style="width: 85%; margin-left: 5%; margin-right: 10%; text-align: right">
                    <tr>
                        <th>
                            <asp:Button ID="btn_revziyon_kod" runat="server" CssClass="btn btn-success" Text="Revize Kod Oluştur" OnClick="btn_revziyon_kod_Click" />
                        </th>
                    </tr>
                </table>
            </div>


            <br />

            <div class="row">
                <%-- ÜRÜN YERİ ÖLÇÜLERİ --%>
                <fieldset style="width: 30%">
                    <legend>Ürün Yeri Ölçüleri</legend>
                    <div class="row">
                        <div class="col-md-6">
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="lbl_min" runat="server" Text="Min."></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="lbl_max" runat="server" Text="Max."></asp:Label>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="Label42" runat="server" Text="Mil Çapı"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txt_mamulmil_min" CssClass="form-control" runat="server" onchange="javascript: cidar_hesapla( this );"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txt_mamulmil_max" CssClass="form-control" runat="server" onchange="javascript: cidar_hesapla( this );"></asp:TextBox>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="Label44" runat="server" Text="Yuva Çapı"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txt_mamulyuva_min" CssClass="form-control" runat="server" onchange="javascript: cidar_hesapla( this );"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txt_mamulyuva_max" CssClass="form-control" runat="server" onchange="javascript: cidar_hesapla( this );"></asp:TextBox>
                        </div>
                    </div>



                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="Label46" runat="server" Text="Yağ Boşlugu"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txt_mamulyag_min" CssClass="form-control" runat="server" onchange="javascript: cidar_hesapla( this );"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txt_mamulyag_max" CssClass="form-control" runat="server" onchange="javascript: cidar_hesapla( this );"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="Label29" runat="server" Text="Çakma Sıkılığı"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txt_cakma_sikiligi_min" CssClass="form-control" runat="server" onchange="javascript: cidar_hesapla( this );"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txt_cakma_sikiligi_max" CssClass="form-control" runat="server" onchange="javascript: cidar_hesapla( this );"></asp:TextBox>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="Label28" runat="server" Text="Revizyon Açıklaması"></asp:Label>
                        </div>
                        <div class="col-md-6">
                            <asp:TextBox ID="txt_revizyon_notlar" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>


                </fieldset>

                <%-- ÜRÜN SERBEST HALDEKİ ÖLÇÜLERİ --%>
                <fieldset style="width: 30%">
                    <legend>Serbest Haldeki Ürün Ölçüleri</legend>

                    <div class="row">
                        <div class="col-md-6">
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="Label47" runat="server" Text="Min."></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="Label48" runat="server" Text="Max."></asp:Label>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="Label49" runat="server" Text="iç Çap"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txt_mamul_iccap_min" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txt_mamul_iccap_max" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="Label50" runat="server" Text="Dış Çap"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txt_mamul_discap_min" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txt_mamul_discap_max" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="Label51" runat="server" Text="Cidar"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txt_mamul_cidar_min" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txt_mamul_cidar_max" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="Label45" runat="server" Text="Boy"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txt_mamulboy_min" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txt_mamulboy_max" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="Label53" runat="server" Text="Boy 2"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txt_mamulboy2_min" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txt_mamulboy2_max" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="Label54" runat="server" Text="Çene Ara"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txt_mamul_cenearacap_min" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txt_mamul_cenearacap_max" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <asp:Label ID="Label55" runat="server" Text="Çene Çap"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txt_mamul_cenecap_min" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txt_mamul_cenecap_max" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>




                </fieldset>

                <%-- revizyon bilgileri listesi --%>
                <fieldset style="width: 40%">
                    <legend>Revizyonlar</legend>
                    <div>
                        <asp:GridView ID="grd_mamul_olculer"
                            AutoGenerateColumns="False"
                            GridLines="None"
                            AllowPaging="true"
                            CssClass="mGrid_teklif" PageSize="20"
                            PagerStyle-CssClass="pgr" DataKeyNames="ID"
                            AlternatingRowStyle-CssClass="alt" runat="server" OnSelectedIndexChanged="rev_sec_av" OnRowDeleting="rev_delete_av" OnRowCommand="grd_mamul_olculer_RowCommand" OnRowDataBound="grd_mamul_olculer_RowDataBound">
                            <Columns>
                                <asp:CommandField HeaderText="Sil" ShowDeleteButton="True" ControlStyle-CssClass="btn btn-success" ButtonType="Button" />
                                <asp:CommandField HeaderText="Seç" ShowSelectButton="True" ControlStyle-CssClass="btn btn-success" ButtonType="Button" />
                                <asp:BoundField DataField="ID" HeaderText="Sırano" />
                                <asp:BoundField DataField="REVIZYON" HeaderText="Revizyon No" />
                                <asp:BoundField DataField="REV_AD" HeaderText="Revizyon Adı" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="spe_pr" runat="server" class="btn btn-info" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="AddRecord" Text="Spec Yazdır" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                        </asp:GridView>
                    </div>
                </fieldset>

            </div>
            <br />
            <br />
            <div>
                <asp:HiddenField ID="rev_id_no" runat="server" />
            </div>



            <br />
            <br />
            <hr />
            <div class="row">
                <asp:Label ID="lbl_cr1" runat="server" Text="Kayıt Yapan" Font-Bold="true" Font-Size="Small" Font-Italic="true" Width="9%"></asp:Label>
                <asp:Label ID="lblcr_user" runat="server" Text=" " Font-Size="Small" Font-Italic="true" Width="30%"></asp:Label>

                <asp:Label ID="lbl_chg" runat="server" Text="Son Değişiklik" Font-Bold="true" Font-Size="Small" Font-Italic="true" Width="10%"></asp:Label>
                <asp:Label ID="lbl_cng_user" runat="server" Text=" " Font-Size="Small" Font-Italic="true" Width="30%"></asp:Label>
            </div>
        </div>

        <%-- stok kartı liste ekranı bilgileri --%>
        <div id="div2" style="display: none; text-align: left">
            <div class="row">
                <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                    <asp:Label ID="Label85" runat="server" Text="Stok Kodu"></asp:Label>
                    <asp:TextBox ID="txt_stokkodu_liste" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-md-3 col-lg-3 col-sm-3 col-xs-3">
                    <asp:Label ID="Label86" runat="server" Text="Stok Adı1"></asp:Label>
                    <asp:TextBox ID="txt_stokadi1_liste" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-md-3 col-lg-3 col-sm-3 col-xs-3">
                    <asp:Label ID="Label6" runat="server" Text="Stok Adı2"></asp:Label>
                    <asp:TextBox ID="txt_stokadi2_liste" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                    <asp:Label ID="Label31" runat="server" Text="Depar Kodu"></asp:Label>
                    <asp:TextBox ID="txt_eskistokkodu_liste" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                    <asp:Label ID="Label21" runat="server" Text="Durum"></asp:Label>
                    <asp:DropDownList ID="drp_eski_yeni_id" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                        <Items>
                            <asp:ListItem Selected="True" Text="YENI" />
                            <asp:ListItem Text="ESKI" />
                        </Items>
                    </asp:DropDownList>
                </div>
            </div>

            <div class="row">
                <div class="col-md-2 col-xs-2 col-sm-2">
                    <div class="from-group">
                        <asp:Label ID="Label7" runat="server" Text="Ana Grubu"></asp:Label>
                        <asp:DropDownList ID="drp_gk1_liste" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                            <Items>
                                <asp:ListItem Text="Grup kodu 1 Seçiniz" />
                            </Items>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-2 col-xs-2 col-sm-2">
                    <div class="from-group">
                        <asp:Label ID="Label8" runat="server" Text="Kategori"></asp:Label>
                        <asp:DropDownList ID="drp_gk2_liste" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                            <Items>
                                <asp:ListItem Text="Grup kodu 2 Seçiniz" />
                            </Items>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-2 col-xs-2 col-sm-2">
                    <div class="from-group">
                        <asp:Label ID="Label10" runat="server" Text="Ürün Grubu"></asp:Label>
                        <asp:DropDownList ID="drp_gk3_liste" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                            <Items>
                                <asp:ListItem Text="Grup kodu 3 Seçiniz" />
                            </Items>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-2 col-xs-2 col-sm-2">
                    <div class="from-group">
                        <asp:Label ID="Label11" runat="server" Text="Ürün Tipi"></asp:Label>
                        <asp:DropDownList ID="drp_gk4_liste" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                            <Items>
                                <asp:ListItem Text="Grup kodu 4 Seçiniz" />
                            </Items>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-2 col-xs-2 col-sm-2">
                    <div class="from-group">
                        <asp:Label ID="Label12" runat="server" Text="Marka"></asp:Label>
                        <asp:DropDownList ID="drp_gk5_liste" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                            <Items>
                                <asp:ListItem Text="Marka Seçiniz" />
                            </Items>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                    <asp:Label ID="Label32" runat="server" Text="Model"></asp:Label>
                    <asp:DropDownList ID="drp_gk6_liste" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                        <Items>
                            <asp:ListItem Text="Model Seçiniz" />
                        </Items>
                    </asp:DropDownList>
                </div>
            </div>

            <br />
            <div class="row">
                <table style="text-align: center">
                    <tr>
                        <th style="text-align: center">
                            <asp:Button ID="btn_stokkart_listele" CssClass="btn btn-info" runat="server" Width="20%" Text="Stok Kartları Listele" OnClick="listele_av" />
                        </th>
                    </tr>
                </table>
            </div>
            <br />
            <div class="row">
                <asp:GridView ID="grd_stoklar"
                    AutoGenerateColumns="False"
                    GridLines="None"
                    AllowPaging="true"
                    CssClass="mGrid_teklif" PageSize="20"
                    PagerStyle-CssClass="pgr" DataKeyNames="ID"
                    AlternatingRowStyle-CssClass="alt" runat="server" OnSelectedIndexChanged="sec_av" OnPageIndexChanging="page_change_av">
                    <Columns>
                        <asp:CommandField HeaderText="Seç" ShowSelectButton="True" SelectText="seç" ItemStyle-Width="40px" />
                        <asp:BoundField DataField="ID" HeaderText="Sıra No" ItemStyle-Width="40px" />
                        <asp:BoundField DataField="ESKI_KOD" HeaderText="D.kod" ItemStyle-Width="70px" />
                        <asp:BoundField DataField="KOD" HeaderText="Stok Kodu" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="KOD_AD" HeaderText="Stok Kodu" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="REVIZYON" HeaderText="Rvz" ItemStyle-Width="30px" />
                        <asp:BoundField DataField="GK_6_AD" HeaderText="Model" />
                        <asp:BoundField DataField="GK_3_AD" HeaderText="Ürün Grubu" />
                        <asp:BoundField DataField="GK_4_AD" HeaderText="Ürün Tipi" />

                        <asp:BoundField DataField="MIL_MIN" HeaderText="Mil Min." />
                        <asp:BoundField DataField="MIL_MAX" HeaderText="Mil Max." />
                        <asp:BoundField DataField="YUVA_MIN" HeaderText="Yuva Min." />
                        <asp:BoundField DataField="YUVA_MAX" HeaderText="Yuva Max." />
                        <asp:BoundField DataField="BOY_MIN" HeaderText="Boy Min." />
                        <asp:BoundField DataField="BOY_MAX" HeaderText="Boy Max." />
                        <asp:BoundField DataField="CENECAP_MIN" HeaderText="Çene Çap Min." />
                        <asp:BoundField DataField="CENECAP_MAX" HeaderText="Çene Çap Max." />
                        <asp:BoundField DataField="CENE_ARA_MIN" HeaderText="Çene Ara Min." />
                        <asp:BoundField DataField="CENE_ARA_MAX" HeaderText="Çene Ara Max." />
                        <asp:BoundField DataField="YAG_MIN" HeaderText="Yağ Min." />
                        <asp:BoundField DataField="YAG_MAX" HeaderText="Yağ Max." />
                        <asp:BoundField DataField="CIDAR_MIN" HeaderText="Cidar Min." />
                        <asp:BoundField DataField="CIDAR_MAX" HeaderText="Cidar Max." />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ContentPlaceHolderID="scripts" runat="server">
    <script type="text/javascript">
        function cidar_hesapla() {

            //var getValue = document.getElementById('txt_mamulmil_min').value;
            //var mil_min = document.getElementById('txt_mamulmil_min');
            var mil_min = document.getElementById('<%=txt_mamulmil_min.ClientID%>').value;
            var mil_max = document.getElementById('<%=txt_mamulmil_max.ClientID%>').value;
            var yu_min = document.getElementById('<%=txt_mamulyuva_min.ClientID%>').value;
            var yu_max = document.getElementById('<%=txt_mamulyuva_max.ClientID%>').value;
            var yag_min = document.getElementById('<%=txt_mamulyag_min.ClientID%>').value;
            var yag_max = document.getElementById('<%=txt_mamulyag_max.ClientID%>').value;
            var yuva_min = document.getElementById('<%=txt_mamulyuva_min.ClientID%>').value;
            var yuva_max = document.getElementById('<%=txt_mamulyuva_max.ClientID%>').value;
            var skl_min = document.getElementById('<%=txt_cakma_sikiligi_min.ClientID%>').value;
            var skl_max = document.getElementById('<%=txt_cakma_sikiligi_max.ClientID%>').value;

            var num4 = parseFloat(yuva_min.replace(',', '.')).toFixed(3);
            var num5 = parseFloat(skl_min.replace(',', '.')).toFixed(3);
            var dis_cap_min = (parseFloat(num4) + parseFloat(num5)).toFixed(3);



            var num6 = parseFloat(yuva_max.replace(',', '.')).toFixed(3);
            var num7 = parseFloat(skl_max.replace(',', '.')).toFixed(3);
            var dis_cap_max = (parseFloat(num6) + parseFloat(num7)).toFixed(3);


            var num1 = parseFloat(yu_min.replace(',', '.')).toFixed(3);
            var num2 = parseFloat(mil_max.replace(',', '.')).toFixed(3);
            var num3 = parseFloat(yag_max.replace(',', '.')).toFixed(3);
            var ci_min = parseFloat((num1 - num2 - num3) / 2).toFixed(3);


            var num1_1 = parseFloat(yu_max.replace(',', '.')).toFixed(3);
            var num2_2 = parseFloat(mil_min.replace(',', '.')).toFixed(3);
            var num3_3 = parseFloat(yag_min.replace(',', '.')).toFixed(3);
            var ci_max = parseFloat((num1_1 - num2_2 - num3_3) / 2).toFixed(3);

            var ic_cap_min = (parseFloat(num2_2) + parseFloat(num3_3)).toFixed(3);
            var ic_cap_max = (parseFloat(num2) + parseFloat(num3)).toFixed(3);

            document.getElementById('<%= txt_mamul_cidar_min.ClientID %>').value = ci_min.replace('.', ',');
            document.getElementById('<%= txt_mamul_cidar_max.ClientID %>').value = ci_max.replace('.', ',');

            document.getElementById('<%= txt_mamul_discap_min.ClientID %>').value = dis_cap_min.replace('.',',');
            document.getElementById('<%= txt_mamul_discap_max.ClientID %>').value = dis_cap_max.replace('.', ',');
            document.getElementById('<%= txt_mamul_iccap_min.ClientID %>').value = ic_cap_min.replace('.', ',');
            document.getElementById('<%= txt_mamul_iccap_max.ClientID %>').value = ic_cap_max.replace('.', ',');




        }
    </script>
</asp:Content>
