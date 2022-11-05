<%@ Page Title="" Language="C#" MasterPageFile="~/master_page.Master" AutoEventWireup="true" CodeBehind="urun_listesi.aspx.cs" Inherits="depar_erp_v1_1.urun_listesi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="margin-left: 2%; margin-right: 96%; margin-right: 2%; text-align: left">

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
                        <asp:Button ID="btn_stokkart_listele" CssClass="btn btn-info" runat="server" Width="20%" Text="Ürünleri Listele" OnClick="btn_stokkart_listele_Click" />
                    </th>
                </tr>
            </table>
        </div>

        <br />

        <div class="row">
            <asp:GridView ID="grd_stoklar"
                AutoGenerateColumns="False"
                AllowSorting="true"
                GridLines="None"
                PageSize="40"
                AllowPaging="true"
                CssClass="mGrid_teklif" 
                PagerStyle-CssClass="pgr" DataKeyNames="ID"
                AlternatingRowStyle-CssClass="alt" runat="server" OnSorting="SORT_AV" OnPageIndexChanging="PAGE_AV">
                <Columns>
                    <asp:CommandField HeaderText="Seç" ShowSelectButton="True" SelectText="seç" ItemStyle-Width="40px" />
                    <asp:BoundField DataField="ID" HeaderText="Sıra No" ItemStyle-Width="40px" SortExpression="ID" />
                    <asp:BoundField DataField="ESKI_KOD" HeaderText="D.kod" ItemStyle-Width="70px" SortExpression="ESKI_KOD" />
                    <asp:BoundField DataField="KOD" HeaderText="Stok Kodu" ItemStyle-Width="100px" SortExpression="KOD" />
                    <asp:BoundField DataField="KOD_AD" HeaderText="Stok Kodu" ItemStyle-Width="100px" SortExpression="KOD_AD" />
                    <asp:BoundField DataField="REVIZYON" HeaderText="Rvz" ItemStyle-Width="30px" />

                    <asp:BoundField DataField="GK_5_AD" HeaderText="Marka" SortExpression="GK_5_AD" />
                    <asp:BoundField DataField="GK_6_AD" HeaderText="Model" SortExpression="GK_6_AD" />
                    <asp:BoundField DataField="GK_3_AD" HeaderText="Ürün Grubu" SortExpression="GK_3_AD" />
                    <asp:BoundField DataField="GK_4_AD" HeaderText="Ürün Tipi" SortExpression="GK_4_AD" />
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

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
