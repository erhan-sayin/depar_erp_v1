<%@ Page Title="" Language="C#" MasterPageFile="~/master_page.Master" AutoEventWireup="true" CodeBehind="is_merkezleri.aspx.cs" Inherits="depar_erp_v1_1.is_merkezleri" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="margin-left: 2%; margin-right: 96%; margin-right: 2%; text-align: left">
        <br />
        <div class="row">
            <div class="col-md-6 col-xs-6 col-sm-6" >
                <fieldset>
                    <legend style="font-size: small">İş merkezi bilgileri</legend>
                    <asp:HiddenField ID="hdn_ismerkezi" runat="server" />
                    <div clas="row  ">
                        <div class="col-md-5 col-xs-5 col-sm-5">
                            <div class="from-group">
                                <asp:Label ID="Label1" runat="server" Text="iş Merkezi Adı:"></asp:Label>
                                <asp:TextBox ID="txt_ismerkezi_adi" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-5 col-xs-5 col-sm-5">
                            <div class="from-group">
                                <asp:Label ID="Label2" runat="server" Text="iş Merkezi Bölümü:"></asp:Label>
                                <asp:DropDownList ID="drp_bolum" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                    <Items>
                                    </Items>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-md-2 col-xs-2 col-sm-2">
                            <div class="from-group">
                                <asp:Label ID="Label3" runat="server" Text=".."></asp:Label>
                                <asp:Button ID="btn_update" runat="server" CssClass="btn btn-success form-control" Width="100%" Text="Ekle/Güncelle" OnClick="update_av" />
                            </div>
                        </div>
                    </div>
                </fieldset>
            </div>


            <%-- PERSONEL VE İŞ MERKEZİ İLİŞKİSİ KURULUYOR. --%>
            <div class="col-md-6 col-xs-6 col-sm-6" >
                <fieldset>
                    <legend style="font-size: small">İş merkezi Personel</legend>
                    <asp:HiddenField ID="hdn_personel" runat="server" />
                    <div clas="row  ">

                        <div class="col-md-3 col-xs-3 col-sm-3">
                            <div class="from-group">
                                <asp:Label ID="Label4" runat="server" Text="iş Merkezi Adı:"></asp:Label>
                                <asp:DropDownList ID="drp_ismerkezi" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                    <Items>
                                    </Items>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3 col-xs-3 col-sm-3">
                            <div class="from-group">
                                <asp:Label ID="Label5" runat="server" Text="Personel Adı:"></asp:Label>
                                <asp:TextBox ID="txt_personel_ad" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-3 col-xs-3 col-sm-3">
                            <div class="from-group">
                                <asp:Label ID="Label7" runat="server" Text="Personel Soyadı:"></asp:Label>
                                <asp:TextBox ID="txt_personel_soyad" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-md-2 col-xs-2 col-sm-2">
                            <div class="from-group">
                                <asp:Label ID="Label6" runat="server" Text=".."></asp:Label>
                                <asp:Button ID="Button1" runat="server" CssClass="btn btn-success form-control" Width="100%" Text="Ekle/Güncelle" OnClick="update_av_2" />
                            </div>
                        </div>
                    </div>
                </fieldset>
            </div>

        </div>
        <br />

        <div class="row">
            <div class="col-md-6 col-xs-6 col-sm-6" >
                <asp:GridView ID="grd_ismerkezleri"
                    AutoGenerateColumns="False"
                    GridLines="None"
                    PageSize="30"
                    AllowPaging="true"
                    CssClass="mGrid_teklif"
                    PagerStyle-CssClass="pgr" DataKeyNames="ID"
                    AlternatingRowStyle-CssClass="alt" runat="server" Width="85%" OnSelectedIndexChanged="sec_av" OnRowDeleting="delete_av" OnRowDataBound="grd_ismerkezleri_RowDataBound">
                    <Columns>
                        <asp:CommandField HeaderText="Seç" ShowSelectButton="True" SelectText="Seç" ControlStyle-CssClass="btn btn-success" ButtonType="Button" />
                        <asp:CommandField HeaderText="Sil" ShowDeleteButton="True" SelectText="Sil" ControlStyle-CssClass="btn btn-success" ButtonType="Button" />
                        <asp:BoundField DataField="ID" HeaderText="Sıra No" />

                        <asp:BoundField DataField="ADI" HeaderText="İş Merkezi Adı" />
                        <asp:BoundField DataField="GRUBU" HeaderText="İş Merkezi Grubu" />
                    </Columns>
                </asp:GridView>
            </div>

            <div class="col-md-6 col-xs-6 col-sm-6" >
                <asp:GridView ID="grd_ismerkezleri_personel"
                    AutoGenerateColumns="False"
                    GridLines="None"
                    PageSize="30"
                    AllowPaging="true"
                    CssClass="mGrid_teklif"
                    PagerStyle-CssClass="pgr" DataKeyNames="ID"
                    AlternatingRowStyle-CssClass="alt" runat="server"  Width="85%">
                    <Columns>
                        <asp:CommandField HeaderText="Seç" ShowSelectButton="True" SelectText="Seç" ControlStyle-CssClass="btn btn-success" ButtonType="Button" />
                        <asp:CommandField HeaderText="Sil" ShowDeleteButton="True" SelectText="Sil" ControlStyle-CssClass="btn btn-success" ButtonType="Button" />
                        <asp:BoundField DataField="ID" HeaderText="Sıra No" />
                        <asp:BoundField DataField="PERSONEL_AD" HeaderText="Pers. Adı" />
                        <asp:BoundField DataField="PERSONEL_SOYAD" HeaderText="Pers. Soyadı" />
                        <asp:BoundField DataField="TEZGAH" HeaderText="İş Merkezi Adı" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>

    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
