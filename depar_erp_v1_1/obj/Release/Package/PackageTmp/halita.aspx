<%@ Page Title="" Language="C#" MasterPageFile="~/master_page.Master" AutoEventWireup="true" CodeBehind="halita.aspx.cs" Inherits="depar_erp_v1_1.halita" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="margin-left: 2%; margin-right: 96%; margin-right: 2%; text-align: left">
        <table style="width: 100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td style="width: 50%"><a href="#2151624#" id="a13" class="tab_selected" onclick="ChangeForm(13)">Halita Giriş</a></td>
                <td style="width: 50%"><a href="#2151625#" id="a14" class="tab" onclick="ChangeForm(14)">Halita liste</a></td>
            </tr>
        </table>

        <div id="div13">
            <table style="width: 20%">
                <tr>
                    <th>
                        <asp:Button ID="new_stok_kart" runat="server" class="btn btn-info" Text="Ekle" OnClick="save_av" />
                    </th>
                    <th>
                        <asp:Button ID="btn_update" runat="server" class="btn btn-info" Text="Güncelle" OnClick="update_av" />
                    </th>
                    <th>
                        <asp:Button ID="delete_kart" runat="server" class="btn btn-danger" Text="Sil" OnClick="delete_av" OnClientClick="return confirm('Halita evrakını silmek istediginize emin misiniz?');" />
                    </th>
                </tr>
            </table>
            <fieldset>
                <legend>Halita Bilgileri</legend>
                <div class="row">
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label1" runat="server" Text="Depar Alaşım Kod: "></asp:Label>
                            <asp:TextBox ID="txt_alasimdepar_kod" CssClass="form-control" runat="server" ReadOnly="true" MaxLength="20"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label18" runat="server" Text="Alaşım Adı: "></asp:Label>
                            <asp:TextBox ID="txt_alasim_adi" CssClass="form-control" runat="server" MaxLength="50"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label19" runat="server" Text="SAE No "></asp:Label>
                            <asp:TextBox ID="txt_saeno" CssClass="form-control" runat="server" MaxLength="20"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label2" runat="server" Text="ISO Normu "></asp:Label>
                            <asp:TextBox ID="txt_isonormu" CssClass="form-control" runat="server" MaxLength="10"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-md-1 col-xs-1 col-sm-1">
                        <div class="from-group">
                            <asp:Label ID="Label3" runat="server" Text="Sertlik "></asp:Label>
                            <asp:TextBox ID="txt_sertlik" CssClass="form-control" runat="server" MaxLength="10"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1 col-xs-1 col-sm-1">
                        <div class="from-group">
                            <asp:Label ID="Label4" runat="server" Text="Uzama "></asp:Label>
                            <asp:TextBox ID="txt_uzama" CssClass="form-control" runat="server" MaxLength="10"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1 col-xs-1 col-sm-1">
                        <div class="from-group">
                            <asp:Label ID="Label5" runat="server" Text="Özgül Ağırlık "></asp:Label>
                            <asp:TextBox ID="txt_ozgagirlik" CssClass="form-control" runat="server" MaxLength="10"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </fieldset>
            <br />
            <fieldset>
                <legend>Halita Malzeme Detay</legend>
                <asp:HiddenField ID="HDN_HALITA_ID" runat="server" />

                <div class="row">
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label6" runat="server" Text="Malzeme Kodu: "></asp:Label>
                            <asp:DropDownList ID="drp_malzeme" runat="server" CssClass="ddl form-control" Width="100%" AppendDataBoundItems="true">
                                <Items>
                                </Items>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label8" runat="server" Text="Yüzde Min"></asp:Label>
                            <asp:TextBox ID="txt_yuzdenmin" CssClass="form-control" runat="server" onchange="javascript: yuzde_hesapla( this );"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label9" runat="server" Text="Yüzde Max"></asp:Label>
                            <asp:TextBox ID="txt_yuzdenmax" CssClass="form-control" runat="server" onchange="javascript: yuzde_hesapla( this );"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label10" runat="server" Text="Yüzde Ort."></asp:Label>
                            <asp:TextBox ID="txt_yuzdenort" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label11" runat="server" Text="Öz. Agır."></asp:Label>
                            <asp:TextBox ID="txt_ozagirlik" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2 col-xs-2 col-sm-2">
                        <div class="from-group">
                            <asp:Label ID="Label12" runat="server" Text="Öz. Agır. Ktsy."></asp:Label>
                            <asp:TextBox ID="txt_ozagirlikkats" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </fieldset>
            <br />

            <div class="row">
                <table style="text-align: center">
                    <tr>
                        <th style="text-align: center">
                            <asp:Button ID="btn_save" runat="server" CssClass=" btn btn-success" Width="40%" Text="Halita Malzeme ekle" OnClick="btn_save_update_Click" />
                        </th>
                    </tr>
                </table>
            </div>

            <div class="row">
                <asp:GridView ID="grd_halita_detay"
                    AutoGenerateColumns="False"
                    GridLines="None"
                    AllowPaging="true"
                    CssClass="mGrid_teklif"
                    PagerStyle-CssClass="pgr" DataKeyNames="ID"
                    AlternatingRowStyle-CssClass="alt" runat="server" OnSelectedIndexChanged="SEC_AV">
                    <Columns>
                        <asp:CommandField HeaderText="Seç" ShowSelectButton="True" ItemStyle-Width="40px" SelectText="seç" />
                        <asp:BoundField DataField="ID" HeaderText="Sıra No" ItemStyle-Width="40px" />
                        <asp:BoundField DataField="KOD" HeaderText="Malzeme Kodu" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="KOD_AD" HeaderText="Malzeme Adı" />
                        <asp:BoundField DataField="YUZDE_MIN" HeaderText="Yüzde Min." ItemStyle-Width="150px" />
                        <asp:BoundField DataField="YUZDE_MAX" HeaderText="Yüzde Max." ItemStyle-Width="150px" />
                        <asp:BoundField DataField="YUZDE_ORT" HeaderText="Yüzde Ort." ItemStyle-Width="150px" />
                        <asp:BoundField DataField="OZ_AGIRLIK" HeaderText="Özgül Agırlık" ItemStyle-Width="150px" />
                        <asp:BoundField DataField="OZ_AGIRLIK_KATS" HeaderText="Özgül AGırlık Katsay." ItemStyle-Width="150px" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <div id="div14" style="display: none; text-align: left">
            <fieldset>
                <legend>Teklif kriterleri</legend>
                <div class="row">
                    <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                        <asp:Label ID="Label85" runat="server" Text="Halita Kodu"></asp:Label>
                        <asp:TextBox ID="txt_deparkodliste" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                        <asp:Label ID="Label7" runat="server" Text="SAE No"></asp:Label>
                        <asp:TextBox ID="txt_sae" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-2 col-lg-2 col-sm-2 col-xs-2">
                        <asp:Label ID="Label13" runat="server" Text="ISO Norm"></asp:Label>
                        <asp:TextBox ID="txt_iso" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-4 col-lg-4 col-sm-4 col-xs-4">
                        <asp:Label ID="Label86" runat="server" Text="Malzeme Adı"></asp:Label>
                        <asp:DropDownList ID="drp_malzeme_liste" runat="server" CssClass="ddl form-control" Width="100%" AppendDataBoundItems="true">
                            <Items>
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
                            <asp:Button ID="btn_stokkart_listele" CssClass="btn btn-success" runat="server" Width="40%" Text="Halita Listele" OnClick="LISTELE_AV" />
                        </th>
                    </tr>
                </table>
            </div>
            <br />

            <div class="row">
                <asp:GridView ID="grd_halita_liste"
                    AutoGenerateColumns="False"
                    AllowSorting="true"
                    GridLines="None"
                    PageSize="40"
                    AllowPaging="true"
                    CssClass="mGrid_teklif"
                    PagerStyle-CssClass="pgr" DataKeyNames="ID"
                    AlternatingRowStyle-CssClass="alt" runat="server" OnSelectedIndexChanged="SEC_AV_2" OnPageIndexChanging="PAGE_AV">
                    <Columns>
                        <asp:CommandField HeaderText="Seç" ShowSelectButton="True" ItemStyle-Width="40px" SelectText="seç" />
                        <asp:BoundField DataField="ID" HeaderText="Sıra No" SortExpression="ID" ItemStyle-Width="40px" />
                        <asp:BoundField DataField="DEPARKOD" HeaderText="Depar No" SortExpression="EVRAKNO" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="ALASIM_AD" HeaderText="Alaşım Ad" SortExpression="ALASIM_AD" />
                        <asp:BoundField DataField="SAENO" HeaderText="SAE No" ItemStyle-Width="120px" />
                        <asp:BoundField DataField="ISONORM" HeaderText="ISO Norm" ItemStyle-Width="120px" />
                        <asp:BoundField DataField="KOD" HeaderText="Kod" ItemStyle-Width="200px" />
                        <asp:BoundField DataField="KOD_AD" HeaderText="Kod Adı" />
                        <asp:BoundField DataField="YUZDE_MAX" HeaderText="Yüzde Max" ItemStyle-Width="120px" />
                        <asp:BoundField DataField="YUZDE_MIN" HeaderText="Yüzde Min" ItemStyle-Width="120px" />
                        <asp:BoundField DataField="YUZDE_ORT" HeaderText="Yüzde Ort" ItemStyle-Width="120px" />
                        <asp:BoundField DataField="OZ_AGIRLIK" HeaderText="Özgül Agırlık" ItemStyle-Width="120px" />
                        <asp:BoundField DataField="OZ_AGIRLIK_KATS" HeaderText="Özgül Agırlık Katsayı" ItemStyle-Width="120px" />

                    </Columns>

                </asp:GridView>
            </div>

        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
    <script type="text/javascript">
        function yuzde_hesapla() {
            var yuz_min = document.getElementById('<%=txt_yuzdenmin.ClientID%>').value;
            var yuz_max = document.getElementById('<%=txt_yuzdenmax.ClientID%>').value;
            var num1 = Number.parseFloat(yuz_min.replace(',', '.')).toFixed(3);
            var num2 = Number.parseFloat(yuz_max.replace(',', '.')).toFixed(3);
            var num3 = ((parseFloat(num1) + parseFloat(num2)) / 2).toFixed(3);
            var yuz_ort = parseFloat(num3).toFixed(3);
            document.getElementById('<%= txt_yuzdenort.ClientID %>').value = yuz_ort;
        }
    </script>
</asp:Content>
