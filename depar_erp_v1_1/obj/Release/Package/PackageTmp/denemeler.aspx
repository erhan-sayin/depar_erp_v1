<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="denemeler.aspx.cs" Inherits="depar_erp_v1_1.denemeler" %>


<!DOCTYPE html>
<link href="main_style_sheet.css" rel="stylesheet" />
<link href="main_style_sheet.css" rel="stylesheet" />
<link href="bootstrap/css/bootstrap.min.css" rel="stylesheet" />

<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1">

<link rel="stylesheet" href="//code.jquery.com/ui/1.13.1/themes/base/jquery-ui.css">
<script src="https://code.jquery.com/jquery-3.6.0.js"></script>
<script src="https://code.jquery.com/ui/1.13.1/jquery-ui.js"></script>
<script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
<script type="text/javascript" src="https://www.google.com/jsapi"></script>

<script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/js/select2.min.js" integrity="sha512-2ImtlRlf2VVmiGZsjm9bEyhjGW4dU7B6TNwh/hx/iSByxNENtj3WVE6o/9Lj4TJeVXPi4bnOIMXFIJJAeufa0A==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/css/select2.min.css" integrity="sha512-nMNlpuaDPrqlEls3IX/Q56H36qvBASwb3ipuo3MxeWbsQB1881ox0cRv7UPTgBlriqoynt35KjEwgGUeUXIPnw==" crossorigin="anonymous" referrerpolicy="no-referrer" />



<script>
    $(document).ready(function () {
        $(".ddl").select2({
        });
    });
</script>

<html>
<style>
    fieldset {
        background-color: #eeeeee;
        text-align: left;
        font: 12px;
        border-radius: 12px;
    }

    legend {
        background-color: gray;
        color: white;
        padding: 5px 10px;
        text-align: left;
        font: 10px;
        font-size: small;
        border-radius: 12px;
    }
</style>

<head runat="server">
    <title></title>

    <style type="text/css">
        .Background {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }

        .Popup {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
            width: 400px;
            height: 350px;
        }

        .lbl {
            font-size: 16px;
            font-style: italic;
            font-weight: bold;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <div class="row">
            <div class="col-md-6 col-xs-6 col-sm-6">
                <div class="from-group">
                    <asp:Label ID="Label1" runat="server" Text="Cari Bilgisi: "></asp:Label>
                    <asp:DropDownList ID="drp_carikodu_kart" runat="server" CssClass="ddl form-control" AppendDataBoundItems="true" >
                        <Items>
                            <asp:ListItem Text="Cari Seçiniz" />
                            <asp:ListItem Text="Cari Seçiniz1" />
                            <asp:ListItem Text="Cari Seçiniz2" />
                            <asp:ListItem Text="Cari Seçiniz" />
                            <asp:ListItem Text="Cari Seçiniz" />
                        </Items>
                    </asp:DropDownList>

                  

                </div>
            </div>
        </div>
    </form>
</body>

</html>
