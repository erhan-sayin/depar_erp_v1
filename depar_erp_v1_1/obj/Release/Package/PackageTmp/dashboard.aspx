<%@ Page Title="" Language="C#" MasterPageFile="~/master_page.Master" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="depar_erp_v1_1.dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="alt_header_clas" style="margin-left: 2%; margin-right: 96%; margin-right: 2%; text-align: left" ">


        <div class="row">
            <asp:Label ID="deneme1" runat="server" Text="Döviz kurları MB Döviz satış kurlarıdır." Width="30%" Font-Italic="true" Font-Size="Small"></asp:Label>
            <asp:Label ID="deneme2" runat="server" Text=" " Width="5%" Font-Italic="true" Font-Size="Small"></asp:Label>
            <asp:Label ID="deneme3" runat="server" Text="LME değerleri para birimi USD dir. 1 günlük gecikme ile kapanış değeri alınmaktadır." Width="45%" Font-Italic="true" Font-Size="Small"></asp:Label>
        </div>

        <div class="row">
            <table style="width: 30%; font-size: x-small" class="table table-striped table-bordered table-condensed">
                <%-- Dovız bilgileri yazdırılıyor --%>
                <tr style="border: 1px solid black">
                    <th bgcolor="#EEEEFF">
                        <asp:Label ID="lbl_tarih" runat="server" Text="Tarih"></asp:Label>
                    </th>
                    <th bgcolor="#EEEEFF">
                        <asp:Label ID="lbl_usd_baslik" runat="server" Text="USD"></asp:Label>
                    </th>
                    <th bgcolor="#EEEEFF">
                        <asp:Label ID="lbl_eur_baslik" runat="server" Text="EUR"></asp:Label>
                    </th>
                    <th bgcolor="#EEEEFF">
                        <asp:Label ID="lbl_gbp_baslik" runat="server" Text="GBP"></asp:Label>
                    </th>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="lbl_tarih_today" runat="server" Text="Tarih"></asp:Label>
                    </th>
                    <th>
                        <asp:Label ID="lbl_usd" runat="server" Text="USD"></asp:Label>
                    </th>
                    <th>
                        <asp:Label ID="lbl_eur" runat="server" Text="EUR"></asp:Label>
                    </th>
                    <th>
                        <asp:Label ID="lbl_gbp" runat="server" Text="GBP"></asp:Label>
                    </th>
                </tr>
            </table>

            <table style="width: 5%">
                <tr>
                    <th></th>
                </tr>
            </table>

            <table style="width: 45%; font-size: x-small" class="table table-striped table-bordered table-condensed">
                <tr style="border: 1px solid black">
                    <th bgcolor="#EEEEFF">
                        <asp:Label ID="Label1" runat="server" Text="Tarih"></asp:Label>
                    </th>
                    <th bgcolor="#EEEEFF">
                        <asp:Label ID="Label2" runat="server" Text="Alüminyum (al)"></asp:Label>
                    </th>
                    <th bgcolor="#EEEEFF">
                        <asp:Label ID="Label3" runat="server" Text="Bakır (cu)"></asp:Label>
                    </th>
                    <th bgcolor="#EEEEFF">
                        <asp:Label ID="Label4" runat="server" Text="Çinko (zn)"></asp:Label>
                    </th>
                    <th bgcolor="#EEEEFF">
                        <asp:Label ID="Label5" runat="server" Text="Kurşun (pb)"></asp:Label>
                    </th>
                    <th bgcolor="#EEEEFF">
                        <asp:Label ID="Label6" runat="server" Text="Nikel (ni)"></asp:Label>
                    </th>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="lme_tarih" runat="server" Text="Bugün"></asp:Label>
                    </th>
                    <th>
                        <asp:Label ID="lbl_lme_al" Text=" " runat="server" />
                    </th>
                    <th>
                        <asp:Label ID="lbl_lme_cu" Text=" " runat="server" />
                    </th>
                    <th>
                        <asp:Label ID="lbl_lme_zn" Text=" " runat="server" />
                    </th>
                    <th>
                        <asp:Label ID="lbl_lme_pb" Text=" " runat="server" />
                    </th>
                    <th>
                        <asp:Label ID="lbl_lme_ni" Text=" " runat="server" />
                    </th>

                </tr>
            </table>
        </div>

        <br />
        <div class="row" style="text-align: center">
            <div class="container">
                <table>
                    <tr>
                        <th>
                            <div id="chart_div" style="width: 300px; height: 240px"></div>
                        </th>
                    </tr>
                </table>
            </div>
            <br />
            <br />

            

        </div>
        <br />

        <div class="row" style="text-align:center">
            <div class="col-md-12 col-xs-12 col-sm-12">
                <div class="from-group">
                    <asp:Label ID="label_1" runat="server" Text="Müşteri ve stok bazlı teslimatı geciken siparişler." Font-Bold="true"></asp:Label>
                    <table style="text-align: center" CssClass="form-control">
                        <tr  style="font-size:small">
                            <th>
                                <div id="table_div2" style="width: 100%; height: 280px"></div>
                            </th>
                        </tr>
                    </table>
            </div>    
            </div>
        </div>

    </div>
</asp:Content>

<asp:Content ContentPlaceHolderID="scripts" runat="server">
    <%--<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>--%>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script type="text/javascript">



        //*** line grafik

        //var linechartDataClient2;
        //google.load("visualization", "1", { packages: ["corechart"] });

        //$(document).ready(function () {
        //    // Client Pie Chart
        //    $.ajax({
        //        url: "dashboard.aspx/GetlineChartData_Client",
        //        data: "",
        //        dataType: "json",
        //        type: "POST",
        //        contentType: "application/json; chartset=utf-8",
        //        //success: function (data) {
        //        //    linechartDataClient2 = data.d;
        //        //},
        //        //error: function () {
        //        //    alert("Error loading data! Please try again.");
        //        //}
        //        success: function (r) {
        //            var data = new google.visualization.DataTable();
        //            data.addColumn('date', 'TRH');
        //            data.addColumn('number', 'BAKIR');
        //            data.addColumn('number', 'KALAY');
        //            data.addColumn('number', 'KURSUN');
        //            data.addColumn('number', 'NIKEL');
        //            data.addColumn('number', 'ALUMINYUM');

        //            for (var i = 0; i < r.d.length; i++) {
        //                data.addRow([new Date(parseInt(r.d[i][0].substr(6))), parseInt(r.d[i][1]), parseInt(r.d[i][2]), parseInt(r.d[i][3]), parseInt(r.d[i][4]), parseInt(r.d[i][5])]);
        //            }
        //        }
        //    }).done(function () {
        //        // after complete loading data
        //        google.setOnLoadCallback(drawlineChartClient2);
        //        drawlineChartClient2();
        //    });
        //});
        //function drawlineChartClient2() {
        //    var data = google.visualization.arrayToDataTable(linechartDataClient2);
        //    var options = {
        //        title: 'LME Trendi',
        //        pieHole: 0.2,
        //        pointSize: 5
        //    };
        //    var chart = new google.charts.Line(document.getElementById('chart_div_2'));
        //    chart.draw(data, google.charts.Line.convertOptions(options));
        //}
    </script>
</asp:Content>
