<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="odeme_page.aspx.cs" Inherits="depar_erp_v1_1.odeme_page" %>

<!DOCTYPE html>


<link rel="stylesheet" href="//code.jquery.com/ui/1.13.1/themes/base/jquery-ui.css">
<script src="https://code.jquery.com/jquery-3.6.0.js"></script>
<script src="https://code.jquery.com/ui/1.13.1/jquery-ui.js"></script>
<script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
<script type="text/javascript" src="https://www.google.com/jsapi"></script>

<script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/js/select2.min.js" integrity="sha512-2ImtlRlf2VVmiGZsjm9bEyhjGW4dU7B6TNwh/hx/iSByxNENtj3WVE6o/9Lj4TJeVXPi4bnOIMXFIJJAeufa0A==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/css/select2.min.css" integrity="sha512-nMNlpuaDPrqlEls3IX/Q56H36qvBASwb3ipuo3MxeWbsQB1881ox0cRv7UPTgBlriqoynt35KjEwgGUeUXIPnw==" crossorigin="anonymous" referrerpolicy="no-referrer" />

<%--<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>--%>
<link href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-multiselect/0.9.13/css/bootstrap-multiselect.css">
<script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-multiselect/0.9.13/js/bootstrap-multiselect.js"></script>

<script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/js/select2.min.js" integrity="sha512-2ImtlRlf2VVmiGZsjm9bEyhjGW4dU7B6TNwh/hx/iSByxNENtj3WVE6o/9Lj4TJeVXPi4bnOIMXFIJJAeufa0A==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/css/select2.min.css" integrity="sha512-nMNlpuaDPrqlEls3IX/Q56H36qvBASwb3ipuo3MxeWbsQB1881ox0cRv7UPTgBlriqoynt35KjEwgGUeUXIPnw==" crossorigin="anonymous" referrerpolicy="no-referrer" />


<script>
    $(function () {
        $("#datepicker1").datepicker({
            dateFormat: 'yy-mm-dd'
        });
    });
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="bootstrap/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="row" style="margin-left: 3%; margin-top: 4%">

            <div class="row">
                <div class="col-md-2 col-xs-2 col-sm-2">
                    <div class="from-group">
                        <asp:Label ID="Label3" runat="server" Text="Ödeme Tarihi: " Font-Size="14px"></asp:Label>
                        <input type="text" data-date-format='yyyy-mm-dd' class="form-control" font-size="14px" id="datepicker1" name="tarih1" runat="server" clientidmode="Static">
                    </div>
                </div>

                <div class="col-md-8 col-xs-8 col-sm-8">
                    <div class="from-group">
                        <asp:Label ID="Label18" runat="server" Text="Cari Adı: " Font-Size="14px"></asp:Label>
                        <asp:TextBox ID="txt_cariadi" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>

                <div class="col-md-2 col-xs-2 col-sm-2">
                    <div class="from-group">
                        <asp:Label ID="Label4" runat="server" Text="Cari Kodu: " Font-Size="14px"></asp:Label>
                        <asp:TextBox ID="txt_carikodu" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>

            <br />

            <div class="row">
                <div class="col-md-2 col-xs-2 col-sm-2">
                    <div class="from-group">
                        <asp:Label ID="Label6" runat="server" Text="Tutar: " Font-Size="14px"></asp:Label>
                        <asp:TextBox ID="TextBox1" CssClass="form-control"  runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-2 col-xs-2 col-sm-2">
                    <div class="from-group">
                        <asp:Label ID="Label2" runat="server" Text="Para Birimi: " Font-Size="14px"></asp:Label>
                        <asp:DropDownList ID="DropDownList1" runat="server"  CssClass="form-control" Font-Size="14px" AppendDataBoundItems="true">
                            <Items>
                                <asp:ListItem Text="Para Birim Seçiniz" />
                                <asp:ListItem Text="TL" />
                                <asp:ListItem Text="USD" />
                                <asp:ListItem Text="EUR" />
                                <asp:ListItem Text="GBP" />
                            </Items>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-8 col-xs-8 col-sm-8">
                    <div class="from-group">
                        <asp:Label ID="Label1" runat="server" Text="Ödeme Notu: "  Font-Size="14px"></asp:Label>
                        <asp:TextBox ID="txt_not" CssClass="form-control" TextMode="MultiLine"  runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <hr />
            <br />
            <br />
            <br />
            <div class="row" style="margin-left: 42%">
                <asp:Button ID="BTN_ODEME" runat="server" CssClass="btn btn-info" Text="Ödeme Girişi" Font-Size="14px" Width="15%" OnClick="BTN_ODEME_Click" OnClientClick="return confirm('Aşagıda belirtilen cari için ödeme girişi yapılacak. Emin misiniz?');" />
            </div>

        </div>
    </form>
</body>
</html>
