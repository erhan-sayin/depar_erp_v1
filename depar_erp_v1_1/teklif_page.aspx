<%@ Page Title="" Language="C#" MasterPageFile="~/master_page.Master" AutoEventWireup="true" CodeBehind="teklif_page.aspx.cs" Inherits="depar_erp_v1_1.teklif_page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="margin-left: 2%; margin-right: 96%; margin-right: 2%; text-align: left">
        <table style="width: 100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td style="width: 50%"><a href="#2151624#" id="a7" class="tab_selected" onclick="ChangeForm(7)">Teklif Giriş</a></td>
                <td style="width: 50%"><a href="#2151625#" id="a8" class="tab" onclick="ChangeForm(8)">Teklif liste</a></td>
            </tr>
        </table>

        <div id="div7">

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
                            <asp:Button ID="delete_kart" runat="server" class="btn btn-danger" Text="Sil" OnClick="delete_av" OnClientClick="return confirm('Teklif evrakını silmek istediginize emin misiniz?');" />
                        </th>
                        <th>
                            <div class="dropdown">
                                <button type="button" class="btn btn-info" data-toggle="dropdown">Yazdır<span></span></button>
                                <ul class="dropdown-menu" runat="server">
                                    <li runat="server">
                                        <button type="button" id="print1" runat="server" class="btn btn-info" style="width: 100%" onserverclick="print_av">Türkçe</button>

                                    </li>
                                    <li>
                                        <button type="button" id="print2" runat="server" class="btn btn-info" style="width: 100%" onserverclick="print_av2">İngilizce</button>

                                    </li>
                                    <li>
                                        <button type="button" id="print3" runat="server" class="btn btn-info" style="width: 100%" onserverclick="print_av3">Proforma Fatura</button>

                                    </li>
                                </ul>
                            </div>
                        </th>
                        <th>
                            <asp:Button ID="Button1" runat="server" class="btn btn-info" Text="Sipariş Oluştur" OnClick="siparis_av" />
                        </th>

                    </tr>
                </table>

                <table style="width: 25%; text-align: right">
                </table>

                <table style="width: 25%; text-align: right">
                    <tr>

                        <th style="text-align: right">
                            <asp:Label ID="label_onay" runat="server" Text="Teklif Onay Durumu"></asp:Label>
                            <asp:DropDownList ID="drp_teklif_durum" runat="server" CssClass="form-control" AppendDataBoundItems="true" AutoPostBack="true" OnTextChanged="durum_change_av">
                                <Items>
                                    <asp:ListItem Text="Cevap Bekliyor" Value="1" />
                                    <asp:ListItem Text="Müşteri Red Etti" Value="2" />
                                    <asp:ListItem Text="Müşteri Onayladı" Value="3" />
                                    <asp:ListItem Text="Müşteri Kismi Onaylandi" Value="4" />
                                    <asp:ListItem Text="Siparişe Dönüştü" Value="5" />
                                </Items>
                            </asp:DropDownList>
                        </th>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lbl_onay_info" runat="server" Font-Bold="false" Font-Italic="true" Font-Size="Small"></asp:Label>
                        </th>
                    </tr>
                </table>

            </div>
            <%-- TEKLİF BAŞLIK BİLGİLERİ --%>
            <fieldset>
                <legend>Teklif Başlık Bilgileri</legend>
                <div class="row">
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="lbl1" runat="server" Text="teklif No: "></asp:Label>
                            <asp:TextBox ID="txt_teklifno_kart" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label3" runat="server" Text="Teklif Tarihi: "></asp:Label>
                            <input type="text" data-date-format='yyyy-mm-dd' class="form-control" id="datepicker4" name="tarih4" runat="server" clientidmode="Static">
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
                            <asp:TextBox ID="txt_ödeme" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1 col-xs-1 col-sm-1">
                        <div class="from-group">
                            <asp:Label ID="Label21" runat="server" Text="KDV"></asp:Label>
                            <asp:DropDownList ID="drp_kdv" runat="server" CssClass="ddl form-control" Width="100%" AppendDataBoundItems="true">
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
                    <div class="col-md-6 col-xs-6 col-sm-6">
                        <div class="from-group">
                            <asp:Label ID="Label4" runat="server" Text="Teklif notları: "></asp:Label>
                            <asp:TextBox ID="txt_teklifnotlar_kart" CssClass="form-control" TextMode="MultiLine" Height="100px" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label5" runat="server" Text="Teklif Veren:"></asp:Label>
                            <asp:TextBox ID="txt_teklifveren_kart" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label24" runat="server" Text="Müşteri Yetkilisi"></asp:Label>
                            <%--<asp:TextBox ID="txt_musttemsilci" CssClass="form-control" runat="server"></asp:TextBox>--%>
                            <asp:DropDownList ID="drp_must_yetkili" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                <Items>
                                    <asp:ListItem Text="Yetkili seçiniz" />
                                </Items>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label11" runat="server" Text="Teslim Yeri/Tarihi:"></asp:Label>
                            <asp:TextBox ID="txt_teklifteslimsekli" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </fieldset>
            <br />

            <asp:HiddenField ID="HDN_SATIR_ID" runat="server" />
            <asp:HiddenField ID="HDN_MLZ_KODU" runat="server" />
            <%-- TEKLİF SATIR BİLGİLERİ --%>
            <fieldset>
                <legend>Teklif Satırları</legend>
                <div class="row">
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label6" runat="server" Text="Malzeme Kodu: "></asp:Label>
                            <asp:DropDownList ID="drp_siparis_malz_kart" runat="server" CssClass="ddl form-control" Font-Size="Small" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="revizyon_bul_av">
                                <Items>
                                    <asp:ListItem Text="Malzeme Seçiniz" />
                                    <asp:ListItem Text="Yeni Ürün" />
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
                            <asp:Label ID="Label7" runat="server" Text=" Parç / Marka  / Model "></asp:Label>
                            <asp:TextBox ID="txt_siparismalzemeadi_kart" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label12" runat="server" Text="Ölçü:"></asp:Label>
                            <asp:TextBox ID="txt_olcu" CssClass="form-control" runat="server"></asp:TextBox>
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
                                    <asp:ListItem Text="Para Birim Seçiniz" />
                                    <asp:ListItem Text="TL" />
                                    <asp:ListItem Text="USD" Selected="True" />
                                    <asp:ListItem Text="EUR" />
                                    <asp:ListItem Text="GBP" />
                                </Items>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-1 col-xs-1 col-sm-1">
                        <div class="from-group">
                            <asp:Label ID="Label20" runat="server" Text="Ürün No: "></asp:Label>
                            <asp:DropDownList ID="drp_urunno" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                <Items>
                                    <asp:ListItem Text="Bilgi seçiniz" Value="0" />
                                    <asp:ListItem Text="Üretici Parça No" Value="1" />
                                    <asp:ListItem Text="Teknik Resim No" Value="2" />
                                    <asp:ListItem Text="NSN No" Value="3" />
                                    <asp:ListItem Text="Alt yatak No" Value="4" />
                                    <asp:ListItem Text="Üst yatak No" Value="5" />
                                    <asp:ListItem Text="Diğer" Value="6" />
                                </Items>
                            </asp:DropDownList>
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
            <br />
            <div class="row">
                <table style="text-align: center">
                    <tr>
                        <th style="text-align: center">
                            <asp:Button ID="btn_save_update" runat="server" CssClass=" btn btn-success" Width="40%" Text="Teklif Satır ekle" OnClick="save_av_2" />
                        </th>
                    </tr>
                </table>
            </div>
            <br />

            <div class="row">
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
                        <asp:BoundField DataField="KOD" HeaderText="Malzeme Kodu" />
                        <asp:BoundField DataField="KOD_AD" HeaderText="Malzeme Adı" />
                        <asp:BoundField DataField="REVIZYON" HeaderText="Revizyon No" />
                        <asp:BoundField DataField="FIYAT" HeaderText="B.fiyat" />
                        <asp:BoundField DataField="PARA_BIRIMI" HeaderText="P.Birimi" />
                        <asp:BoundField DataField="MIKTAR" HeaderText="Miktar" />
                        <asp:BoundField DataField="BIRIM" HeaderText="Birim" />
                        <asp:BoundField DataField="OLCU" HeaderText="Ölçü" />
                        <asp:BoundField DataField="URUNNO" HeaderText="Ürün Numarası" />
                        <%--<asp:BoundField DataField="KDV" HeaderText="KDV" />--%>
                        <asp:BoundField DataField="TOPLAM_TUTAR" HeaderText="Toplam Tutar" />
                        <asp:BoundField DataField="TESLIM_TARIHI" HeaderText="Teslim Tarihi" Visible="false" DataFormatString="{0:yyyy-M-dd}" />
                        <asp:BoundField DataField="ACIKLAMA" HeaderText="Satır Açıklama" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <br />
        <br />

        <%-- TEKLİF LİSTE BİLGİLERİ EKRANI --%>
        <div id="div8" style="display: none; text-align: left">
            <br />
            <fieldset>
                <legend>Teklif kriterleri</legend>
                <div class="row">
                    <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                        <asp:Label ID="Label85" runat="server" Text="Teklif Kodu"></asp:Label>
                        <asp:TextBox ID="txt_siparino_liste" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>

                    <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                        <asp:Label ID="Label13" runat="server" Text="Teklif Not"></asp:Label>
                        <asp:TextBox ID="txt_siparisnot_liste" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-3 col-lg-3 col-sm-3 col-xs-3" style="text-align: center">
                        <asp:Label ID="Label16" runat="server" Text="Teklif Tarihi"></asp:Label>
                        <div class="form-group row">
                            <input type="text" data-date-format='yyyy-mm-dd' class="form-control" id="datepicker5" style="width: 50%" name="tarih5" runat="server" clientidmode="Static" value="Başlangıç Tarihi">
                            <input type="text" data-date-format='yyyy-mm-dd' class="form-control" id="datepicker6" style="width: 50%" name="tarih6" runat="server" clientidmode="Static" value="Bitiş Tarihi">
                        </div>
                    </div>

                    <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                        <div class="from-group">
                            <asp:Label ID="Label17" runat="server" Text="Açık Tklf."></asp:Label>
                            <asp:CheckBox ID="chc1" runat="server" class="form-control" />
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                        <asp:Label ID="Label23" runat="server" Text="Depar Kodu"></asp:Label>
                        <asp:TextBox ID="txt_depar_kodu_liste" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-4 col-lg-4 col-sm-4 col-xs-4">
                        <asp:Label ID="Label86" runat="server" Text="Müşteri Adı"></asp:Label>
                        <asp:DropDownList ID="drp_cari_teklif_liste" runat="server" CssClass="ddl form-control" Width="100%" AppendDataBoundItems="true">
                            <Items>
                                <asp:ListItem Text="Cari Seçiniz" />
                            </Items>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-4 col-lg-4 col-sm-4 col-xs-4">
                        <asp:Label ID="Label22" runat="server" Text="Stok Adı"></asp:Label>
                        <asp:DropDownList ID="drp_malzeme_adi" runat="server" CssClass="ddl form-control" Width="100%" AppendDataBoundItems="true">
                            <Items>
                                <asp:ListItem Text="Malzeme Seçiniz" />
                            </Items>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                        <asp:Label ID="Label25" runat="server" Text="Teklif Durum"></asp:Label>
                        <asp:DropDownList ID="drp_durum_liste" runat="server" CssClass="form-control" Width="100%" AppendDataBoundItems="true">
                            <Items>
                                <asp:ListItem Text="Durum Seçiniz" Value="0" />
                                <asp:ListItem Text="Cevap Bekliyor" Value="1" />
                                <asp:ListItem Text="Müşteri Red Etti" Value="2" />
                                <asp:ListItem Text="Müşteri Onayladı" Value="3" />
                                <asp:ListItem Text="Müşteri Kismi Onaylandi" Value="4" />
                                <asp:ListItem Text="Siparişe Dönüştü" Value="5" />
                            </Items>
                        </asp:DropDownList>
                    </div>

                </div>
            </fieldset>
            <br />
            <div class="row">
                <table style="text-align: center">
                    <tr>
                        <th style="text-align: center">
                            <asp:Button ID="btn_stokkart_listele" CssClass="btn btn-success" runat="server" Width="40%" Text="Teklif Listele" OnClick="listele_av" />
                        </th>
                    </tr>
                </table>
            </div>
            <br />

            <div class="row">
                <asp:GridView ID="grd_teklif_listesi"
                    AutoGenerateColumns="False"
                    AllowSorting="true"
                    GridLines="None"
                    PageSize="40"
                    AllowPaging="true"
                    CssClass="mGrid_teklif"
                    PagerStyle-CssClass="pgr" DataKeyNames="ID"
                    AlternatingRowStyle-CssClass="alt" runat="server" OnSelectedIndexChanged="liste_sec_av" OnSorting="SORT_AV" OnPageIndexChanging="PAGE_AV">
                    <Columns>
                        <asp:CommandField HeaderText="Seç" ShowSelectButton="True" ItemStyle-Width="50px" SelectText="seç" />
                        <asp:BoundField DataField="ID" HeaderText="Sıra No" SortExpression="ID" ItemStyle-Width="50px"  />
                        <asp:BoundField DataField="EVRAKNO" HeaderText="Teklif No" SortExpression="EVRAKNO" ItemStyle-Width="120px"  />
                        <asp:BoundField DataField="CHKODU_AD" HeaderText="Müşteri Adı" SortExpression="CHKODU_AD"  />
                        <asp:BoundField DataField="TARIH" HeaderText="Teklif Tarihi" DataFormatString="{0:yyyy-M-dd}" SortExpression="TARIH" ItemStyle-Width="90px"  />
                        <asp:BoundField DataField="TESLIM_SEKLI" HeaderText="Teslim Şekli" SortExpression="TESLIM_SEKLI"   />
                        <asp:BoundField DataField="DEPAR_KOD" HeaderText="D. Kodu" SortExpression="DEPAR_KOD" ItemStyle-Width="50px"  />
                        <asp:BoundField DataField="KOD_AD" HeaderText="Stok Adı" SortExpression="KOD_AD" />
                        <asp:BoundField DataField="MIKTAR" HeaderText="Miktar" DataFormatString="{0:N0}" ItemStyle-Width="50px"  />
                        <asp:BoundField DataField="BIRIM" HeaderText="Brm" SortExpression="BIRIM" ItemStyle-Width="50px"  />
                        <asp:BoundField DataField="TOPLAM_TUTAR" HeaderText="Toplam Tutar" DataFormatString="{0:N2}" SortExpression="TOPLAM_TUTAR" ItemStyle-Width="80px"  />
                        <asp:BoundField DataField="PARA_BIRIMI" HeaderText="P.Birim" SortExpression="PARA_BIRIMI" ItemStyle-Width="50px"  />
                        <asp:BoundField DataField="DURUM_AD" HeaderText="Sip. Durum" SortExpression="DURUM_AD" ItemStyle-Width="90px"  />
                        <asp:BoundField DataField="SIPARISNO" HeaderText="Sip. No." SortExpression="SIPARISNO" ItemStyle-Width="100px"  />
                    </Columns>

                </asp:GridView>
            </div>
        </div>

    </div>

</asp:Content>
