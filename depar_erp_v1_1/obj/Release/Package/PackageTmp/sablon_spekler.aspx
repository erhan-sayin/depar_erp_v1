<%@ Page Title="" Language="C#" MasterPageFile="~/master_page.Master" AutoEventWireup="true" CodeBehind="sablon_spekler.aspx.cs" Inherits="depar_erp_v1_1.sablon_spekler" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="margin-left: 2%; margin-right: 96%; margin-right: 2%; text-align: left">
        <div class="row">
            <%--<div class="col-md-3 col-xs-3 col-sm-3">
                <div class="from-group">
                    <asp:Label ID="Label1" runat="server" Text="Şablon Adı"></asp:Label>
                    <asp:TextBox ID="txt_sablonadi" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
            </div>--%>
            <div class="col-md-3 col-xs-3 col-sm-3">
                <div class="from-group">
                    <asp:Label ID="Label5" runat="server" Text="Şablon Adı: "></asp:Label>
                    <asp:DropDownList ID="drp_spek_sablonlar" runat="server" CssClass="ddl form-control" Width="100%" AppendDataBoundItems="true" AutoPostBack="true" OnTextChanged="spek_sec_av">
                        <Items>
                            <asp:ListItem Text="Sablon Seciniz" Value="0" />
                            <asp:ListItem Text="İnce Cidarli Düz yatak" Value="1" />
                            <asp:ListItem Text="Kalın Cidarli Düz yatak" Value="2" />
                            <asp:ListItem Text="İnce Cidarli Çeneli" Value="3" />
                            <asp:ListItem Text="Kalın Cidarli Çeneli" Value="4" />
                            <asp:ListItem Text="Kalın Cidarli Çeneli Burç" Value="5" />
                            <asp:ListItem Text="Kalın Cidarli Düz Bm" Value="6" />
                        </Items>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-md-3 col-xs-3 col-sm-3">
                <div class="from-group">
                    <asp:Label ID="Label2" runat="server" Text="Yapılan işlem adı (Rota)"></asp:Label>
                    <%--<asp:TextBox ID="txt_rota" CssClass="form-control" runat="server"></asp:TextBox>--%>
                    <asp:DropDownList ID="drp_islem" runat="server" CssClass="ddl form-control" Width="100%" AppendDataBoundItems="true" >
                        <Items>
                        </Items>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-3 col-xs-3 col-sm-3">
                <div class="from-group">
                    <asp:Label ID="Label3" runat="server" Text="Açıklama"></asp:Label>
                    <asp:TextBox ID="txt_aciklama" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
            </div>

            <div class="col-md-3 col-xs-3 col-sm-3">
                <div class="from-group">
                    <asp:Label ID="Label4" runat="server" Text=".."></asp:Label>
                    <asp:Button ID="btn_ekle" runat="server" Text="Ekle" OnClick="SAVE_AV" CssClass="form-control btn btn-success" />
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdn_spek_satir" runat="server" />
        <br />

        <div class="row" style="margin-left: 10%">
            <asp:GridView ID="grd_sablon_detay"
                AutoGenerateColumns="False"
                GridLines="None"
                PageSize="30"
                AllowPaging="true"
                CssClass="mGrid_teklif"
                PagerStyle-CssClass="pgr" DataKeyNames="ID"
                AlternatingRowStyle-CssClass="alt" runat="server" Width="55%" OnSelectedIndexChanged="SEC_AV" OnRowDeleting="DELETE_AV" OnPageIndexChanging="PAGE_AV">
                <Columns>
                    <asp:CommandField HeaderText="Seç" ShowSelectButton="True" SelectText="Seç" ControlStyle-CssClass="btn btn-success" ButtonType="Button" />
                    <asp:CommandField HeaderText="Sil" ShowDeleteButton="True" SelectText="Sil" ControlStyle-CssClass="btn btn-success" ButtonType="Button" />
                    <asp:BoundField DataField="ID" HeaderText="Sıra No" ItemStyle-Width="50px" />
                    <asp:BoundField DataField="ISLEM_KODU" HeaderText="İşlem Kodu" ItemStyle-Width="75px" />
                    <asp:BoundField DataField="ISLEM_ADI" HeaderText="İşlem Adı" ItemStyle-Width="280px" />
                    <asp:BoundField DataField="ACIKLAMA" HeaderText="Açıklama" ItemStyle-Width="250px" />
                    <asp:BoundField DataField="SABLON_ADI" HeaderText="Şablon Adı"  ItemStyle-Width="200px"/>
                </Columns>
            </asp:GridView>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
