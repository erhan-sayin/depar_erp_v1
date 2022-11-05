<%@ Page Title="" Language="C#" MasterPageFile="~/master_page.Master" AutoEventWireup="true" CodeBehind="üretim_asamalari.aspx.cs" Inherits="depar_erp_v1_1.üretim_asamalari" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="margin-left: 2%; margin-right: 96%; margin-right: 2%; text-align: left">
        <br />
        <fieldset>
            <legend style="font-size: small">Proses Spek Tanımlama</legend>
            <asp:HiddenField ID="hdn_spek_id" runat="server" />
            <div clas="row">
                
                <div class="col-md-3 col-xs-3 col-sm-3">
                    <div class="from-group">
                        <asp:Label ID="Label1" runat="server" Text="İşlem Tanımı"></asp:Label>
                        <asp:TextBox ID="txt_islemadi" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-3 col-xs-3 col-sm-3">
                    <div class="from-group">
                        <asp:Label ID="Label8" runat="server" Text="Açıklama"></asp:Label>
                        <asp:TextBox ID="txt_aciklama" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-1 col-xs-1 col-sm-1">
                    <div class="from-group">
                        <asp:Label ID="Label5" runat="server" Text="Bölüm"></asp:Label>
                        <%--<asp:TextBox ID="txt_birim2" CssClass="form-control" runat="server"></asp:TextBox>--%>
                        <asp:DropDownList ID="drp_bolum" runat="server" CssClass="form-control" AppendDataBoundItems="true" >
                                <Items>
                                    
                                </Items>
                          </asp:DropDownList>
                    </div>
                </div>

                <div class="col-md-1 col-xs-1 col-sm-1">
                    <div class="from-group">
                        <asp:Label ID="Label3" runat="server" Text=".."></asp:Label>
                        <asp:Button ID="btn_update" runat="server" CssClass="btn btn-success form-control" Width="100%" Text="Ekle/Gün." OnClick="update_av" />
                    </div>
                </div>
            </div>
        </fieldset>
        <br />
        <div class="row" style="margin-left:10%">
            <asp:GridView ID="grd_uretim_asama"
                AutoGenerateColumns="False"
                GridLines="None"
                PageSize="30"
                AllowPaging="true"
                CssClass="mGrid_teklif"
                PagerStyle-CssClass="pgr" DataKeyNames="ID"
                AlternatingRowStyle-CssClass="alt" runat="server" Width="85%" OnSelectedIndexChanged="sec_av" OnRowDeleting="delete_av" OnRowDataBound="grd_uretim_asama_RowDataBound" OnPageIndexChanging="PAGE_AV" >
                <Columns>
                    <asp:CommandField HeaderText="Seç" ShowSelectButton="True" SelectText="Seç" ControlStyle-CssClass="btn btn-success" ButtonType="Button" ItemStyle-Width="40px" />
                    <asp:CommandField HeaderText="Sil" ShowDeleteButton="True" SelectText="Sil" ControlStyle-CssClass="btn btn-success" ButtonType="Button"  ItemStyle-Width="40px" />
                    <asp:BoundField DataField="ID" HeaderText="Sıra No"   ItemStyle-Width="60px"/>
                    <asp:BoundField DataField="ASAMA" HeaderText="İşlem" />
                    <asp:BoundField DataField="ACIKLAMA" HeaderText="Açıklama" />

                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
