<%@ Page Title="" Language="C#" MasterPageFile="~/master_page.Master" AutoEventWireup="true" CodeBehind="sistem_parametrileri.aspx.cs" Inherits="depar_erp_v1_1.sistem_parametrileri" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="margin-left: 2%; margin-right: 96%; margin-right: 2%; text-align: left">
        <fieldset>
            <legend>Grup kodları</legend>

            <asp:HiddenField ID="hdn_id" runat="server" />
            <div class="row">
                <div class="col-md-2 col-xs-2 col-sm-2">
                    <div class="from-group">
                        <asp:Label ID="lbl1" runat="server" Text="Ana Grup Kodu: "></asp:Label>
                        <asp:DropDownList ID="drp_anagrup" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                            <Items>
                            </Items>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-3 col-xs-3 col-sm-3">
                    <div class="from-group">
                        <asp:Label ID="Label1" runat="server" Text="Kategori: "></asp:Label>
                        <asp:DropDownList ID="drp_kategori" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                            <Items>
                            </Items>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="col-md-2 col-xs-2 col-sm-2">
                    <div class="from-group">
                        <asp:Label ID="Label3" runat="server" Text="Ürün Grubu: "></asp:Label>
                        <asp:DropDownList ID="drp_urungrubu" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                            <Items>
                            </Items>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-3 col-xs-3 col-sm-3">
                    <div class="from-group">
                        <asp:Label ID="Label2" runat="server" Text="Ürün Tipi: "></asp:Label>
                        <asp:TextBox ID="txt_kod_ad" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>

                <div class="col-md-2 col-xs-2 col-sm-2">
                    <div class="from-group">
                        <asp:Label ID="Label4" runat="server" Text="..."></asp:Label>
                        <asp:Button ID="btn_update" runat="server" CssClass="btn btn-success form-control" Width="100%" Text="Ekle/Güncelle" OnClick="update_av" />
                    </div>
                </div>

            </div>
            <br />

            <div class="row">

                <asp:GridView ID="grd_grupkodu"
                    AutoGenerateColumns="False"
                    GridLines="None"
                    PageSize="8"
                    Width="80%"
                    AllowPaging="true"
                    CssClass="mGrid_teklif"
                    PagerStyle-CssClass="pgr" DataKeyNames="ID"
                    AlternatingRowStyle-CssClass="alt" runat="server" OnSelectedIndexChanged="sec_av" OnRowDeleting="delete_av" OnPageIndexChanging="grd_grupkodu_PageIndexChanging" OnRowDataBound="grd_grupkodu_RowDataBound">
                    <Columns>
                        <asp:CommandField HeaderText="Seç" ShowSelectButton="True" SelectText="Seç" />
                        <asp:CommandField HeaderText="Sil" ShowDeleteButton="True" SelectText="Sil" />
                        <asp:BoundField DataField="ID" HeaderText="Sırano" />
                        <asp:BoundField DataField="ANA_GRUP_AD" HeaderText="Ana Grup Kodu" />
                        <asp:BoundField DataField="KATEGORI_AD" HeaderText="Kategori Kodu" />
                        <asp:BoundField DataField="URUN_GRUBU_AD" HeaderText="Ürün Grup kodu" />
                        <asp:BoundField DataField="GRUPKODU_AD" HeaderText="Grup kodu ad" />
                    </Columns>
                </asp:GridView>


            </div>

            <br />
            <hr />

            <asp:HiddenField ID="hdn_mark" runat="server" />
            <div class="row">
                <div class="col-md-2 col-xs-2 col-sm-2">
                    <div class="from-group">
                        <asp:Label ID="Label8" runat="server" Text="Marka: "></asp:Label>
                        <asp:DropDownList ID="drp_marka" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                            <Items>
                            </Items>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="col-md-2 col-xs-2 col-sm-2">
                    <div class="from-group">
                        <asp:Label ID="Label12" runat="server" Text="Model: "></asp:Label>
                        <asp:TextBox ID="txt_model" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>

                <div class="col-md-2 col-xs-2 col-sm-2">
                    <div class="from-group">
                        <asp:Label ID="Label13" runat="server" Text="..."></asp:Label>
                        <asp:Button ID="btn_mark" runat="server" CssClass="btn btn-success form-control" Width="100%" Text="Ekle/Güncelle" OnClick="update_av_3" />
                    </div>
                </div>
                
                <div class="col-md-1 col-xs-1 col-sm-1">
                    <div class="from-group">
                    </div>
                </div>

                <div class="col-md-2 col-xs-2 col-sm-2">
                    <div class="from-group">
                        <asp:Label ID="Label10" runat="server" Text="Marka: "></asp:Label>
                        <asp:TextBox ID="txt_yenimarka" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-2 col-xs-2 col-sm-2">
                    <div class="from-group">
                        <asp:Label ID="Label11" runat="server" Text="..."></asp:Label>
                        <asp:Button ID="btn_new_marka" runat="server" CssClass="btn btn-success form-control" Width="100%" Text="Ekle/Güncelle" OnClick="new_marka_av" />
                    </div>
                </div>

            </div>

            <br />
            <div class="row">
                <div class="col-md-7 col-xs-7 col-sm-7">
                    <asp:GridView ID="grd_grupkodu_2"
                        AutoGenerateColumns="False"
                        GridLines="None"
                        PageSize="8"
                        Width="90%"
                        AllowPaging="true"
                        CssClass="mGrid_teklif"
                        PagerStyle-CssClass="pgr" DataKeyNames="ID"
                        AlternatingRowStyle-CssClass="alt" runat="server" OnSelectedIndexChanged="sec_av_3" OnPageIndexChanging="page_av_3" OnRowDeleting="delete_av_3" OnRowDataBound="grd_grupkodu_2_RowDataBound">
                        <Columns>
                            <asp:CommandField HeaderText="Seç" ShowSelectButton="True" SelectText="Seç" ItemStyle-Width="70px" />
                            <asp:CommandField HeaderText="Sil" ShowDeleteButton="True" SelectText="Sil" ItemStyle-Width="70px" />
                            <asp:BoundField DataField="ID" HeaderText="Sırano" ItemStyle-Width="90px" />
                            <asp:BoundField DataField="MARKA_AD" HeaderText="Marka" ItemStyle-Width="180px" />
                            <asp:BoundField DataField="GRUPKODU_AD" HeaderText="Model" />
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="col-md-5 col-xs-5 col-sm-5">
                    <asp:GridView ID="grd_marka"
                        AutoGenerateColumns="False"
                        GridLines="None"
                        PageSize="8"
                       
                        AllowPaging="true"
                        CssClass="mGrid_teklif"
                        PagerStyle-CssClass="pgr" DataKeyNames="ID"
                        AlternatingRowStyle-CssClass="alt" runat="server" OnRowDeleting="delete_marka_row" OnPageIndexChanging="grd_marka_PageIndexChanging">
                        <Columns>
                            <asp:CommandField HeaderText="Sil" ShowDeleteButton="True" SelectText="Sil" ItemStyle-Width="50px" />
                            <asp:BoundField DataField="ID" HeaderText="Sırano" ItemStyle-Width="70px" />
                            <asp:BoundField DataField="MARKA" HeaderText="Marka" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <br />

        </fieldset>

        <br />


        <asp:HiddenField ID="hdn_banka" runat="server" />
        <fieldset>
            <legend>Banka bilgileri</legend>
            <div class="row">
                <div class="col-md-3 col-xs-3 col-sm-3">
                    <div class="from-group">
                        <asp:Label ID="Label5" runat="server" Text="Banka : "></asp:Label>
                        <asp:TextBox ID="txt_banka" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-3 col-xs-3 col-sm-3">
                    <div class="from-group">
                        <asp:Label ID="Label6" runat="server" Text="IBAN: "></asp:Label>
                        <asp:TextBox ID="txt_iban" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>

                <div class="col-md-3 col-xs-3 col-sm-3">
                    <div class="from-group">
                        <asp:Label ID="Label7" runat="server" Text="Hesap Ad: "></asp:Label>
                        <asp:TextBox ID="txt_hesap" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>


                <div class="col-md-2 col-xs-2 col-sm-2">
                    <div class="from-group">
                        <asp:Label ID="Label9" runat="server" Text="..."></asp:Label>
                        <asp:Button ID="btn_banka" runat="server" CssClass="btn btn-success form-control" Width="100%" Text="Ekle/Güncelle" OnClick="banka_update" />
                    </div>
                </div>
            </div>
            <br />
            <br />
            <div class="row">
                <asp:GridView ID="grd_banka"
                    AutoGenerateColumns="False"
                    GridLines="None"
                    PageSize="8"
                    AllowPaging="true"
                    CssClass="mGrid_teklif"
                    PagerStyle-CssClass="pgr" DataKeyNames="ID"
                    AlternatingRowStyle-CssClass="alt" runat="server" OnSelectedIndexChanged="sec_av_2" OnRowDeleting="delete_av_2" OnPageIndexChanging="page_av_2">
                    <Columns>
                        <asp:CommandField HeaderText="Sil" ShowDeleteButton="True" />
                        <asp:CommandField HeaderText="Seç" ShowSelectButton="True" />
                        <asp:BoundField DataField="ID" HeaderText="Sırano" />
                        <asp:BoundField DataField="BANKA" HeaderText="Banka" />
                        <asp:BoundField DataField="IBAN" HeaderText="IBAN" />
                        <asp:BoundField DataField="HESAP_AD" HeaderText="Hesap Ad" />
                    </Columns>
                </asp:GridView>
            </div>
        </fieldset>
    </div>

</asp:Content>
