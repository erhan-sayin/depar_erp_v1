<%@ Page Title="" Language="C#" MasterPageFile="~/master_page.Master" AutoEventWireup="true" CodeBehind="open_orders.aspx.cs" Inherits="depar_erp_v1_1.open_orders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div style="margin-left: 2%; margin-right: 96%; margin-right: 2%; text-align: left">
        <br />
        <div class="row">
            <table style="width: 50%">
                <tr>
                    <th>
                        <asp:Button ID="new_stok_kart" runat="server" class="btn btn-info" Text="Seçil Satır İş Emri Çıkar" OnClick="new_stok_kart_Click" />
                    </th>
                    <th></th>
                    <th>
                        <asp:Button ID="btn_statu_update" runat="server" class="btn btn-info" Text="Seçili satırlarda Statu Güncelle " OnClick="statu_update" OnClientClick="return confirm('Seçili satırlar için statu güncellemesi yapılacak onaylıyor musunuz?');" />
                    </th>
                    <th></th>
                    <th style="width: 20%">
                        <asp:DropDownList ID="drp_siparis_durum" runat="server" CssClass="form-control" AppendDataBoundItems="true" >
                            <Items>
                                <asp:ListItem Text="Statu Seçiniz" Value="0" />
                                <asp:ListItem Text="Planlamada" Value="1" />
                                <asp:ListItem Text="Üretimde" Value="2" />
                                <asp:ListItem Text="Kalite Kontrolde" Value="3" />
                                <asp:ListItem Text="Depoda" Value="4" />
                               
                            </Items>
                        </asp:DropDownList>
                    </th>
                    <th></th>
                    <th style="width: 20%">
                        <asp:Button ID="btn_search" runat="server" class="btn btn-info" Text="Listeyi Güncelle" OnClick="btn_search_Click" />
                    </th>
                </tr>
            </table>
        </div>

        <h5 style="font-size: small; color: red">Ekranda gelen sipariş listesinde sipariş onayı verilmemiş ve sevkiyatı tamamlanmış olan sipariler yoktur. </h5>
        <br />

        <div class="row">
            <asp:GridView ID="grd_siparis_uretim"
                AutoGenerateColumns="False"
                Allowsorting="true"
                GridLines="None"
                PageSize="40"
                AllowPaging="true"
                CssClass="mGrid_teklif"
                PagerStyle-CssClass="pgr" DataKeyNames="ID"
                AlternatingRowStyle-CssClass="alt" runat="server" OnSorting="SORT_AV" OnPageIndexChanging="PAGE_AV">
                <Columns>
                    <asp:TemplateField HeaderText="Seç">
                        <ItemTemplate>
                            <asp:CheckBox ID="chc_sec" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ID" HeaderText="Sıra No" />
                    <asp:BoundField DataField="SIPNO" HeaderText="Sipariş No" SortExpression="SIPNO" />
                    <asp:BoundField DataField="CH_ADI" HeaderText="Müşteri Adı" ItemStyle-Width="200px" SortExpression="CH_ADI" />
                    <asp:BoundField DataField="STOK_AD" HeaderText="Stok Adı" ItemStyle-Width="200px"  SortExpression="STOK_AD"/>
                    <asp:BoundField DataField="DEPAR_KOD" HeaderText="Depar Kod" ItemStyle-Width="75px" SortExpression="DEPAR_KOD" />
                    <asp:BoundField DataField="REVIZYON" HeaderText="Rvz"  />
                    <asp:BoundField DataField="MIKTAR" HeaderText="Miktar" DataFormatString="{0:#,##0;(#,##0);0}" />
                    <asp:BoundField DataField="KALAN_MIKTAR" HeaderText="Bakiye miktar" DataFormatString="{0:#,##0;(#,##0);0}" />
                    <asp:BoundField DataField="BIRIM" HeaderText="Brm" />
                    <asp:BoundField DataField="TARIH" HeaderText="Sipariş Tarihi" DataFormatString="{0:yyyy-M-dd}" ItemStyle-Width="120px"  SortExpression="TARIH"/>
                    <asp:BoundField DataField="TESLIM_TARIH" HeaderText="Teslim Tarihi" DataFormatString="{0:yyyy-M-dd}" ItemStyle-Width="120px" SortExpression="TESLIM_TARIH" />
                    <asp:BoundField DataField="FARK" HeaderText="Kalan Süre"  SortExpression="FARK"/>
                    <asp:BoundField DataField="K_MIN" HeaderText="Mil Min." SortExpression="K_MIN"/>
                    <asp:BoundField DataField="K_MAX" HeaderText="Mil Max." SortExpression="K_MAX" />
                    <asp:BoundField DataField="YU_MIN" HeaderText="Yuva Min." SortExpression="YU_MIN" />
                    <asp:BoundField DataField="YU_MAX" HeaderText="Yuva Max." SortExpression="YU_MAX" />
                    <asp:BoundField DataField="YAG_MIN" HeaderText="Yağ Min." SortExpression="YAG_MIN" />
                    <asp:BoundField DataField="YAG_MAX" HeaderText="Yağ Max." SortExpression="YAG_MAX" />
                    <asp:BoundField DataField="CI_MIN" HeaderText="Cidar Min." SortExpression="CI_MIN" />
                    <asp:BoundField DataField="CI_MAX" HeaderText="Cidar Max." SortExpression="CI_MAX" />
                    <asp:BoundField DataField="DURUM_AD" HeaderText="Sip. Durum" ItemStyle-Width="120px" SortExpression="DURUM_AD" />
                    <asp:BoundField DataField="SATIR_NOT" HeaderText="Satır notu" ItemStyle-Width="120px" SortExpression="SATIR_NOT" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
