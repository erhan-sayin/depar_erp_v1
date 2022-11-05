<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login_page.aspx.cs" Inherits="depar_erp_v1_1.login_page" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Depar Motor Yatakları Web ERP</title>
    <link href="bootstrap/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
     <script type="text/javascript" src='https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <link rel="stylesheet" href='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css' media="screen" />

    <form id="form1" runat="server">
        <div class="container">
            <div class="col-xl-12 col-md-12 col-lg-12 col-sx-12 mx-auto">
                <center>

                    <img width="300px" src="images/depar_logo.png" />
                    <h4>Depar Motor Yatakları Web ERP</h4>
                    <br />
                    <br />

                    <label for="txtUsername">
                        Username</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" Width="350px" placeholder="Enter Username" required />
                    <br />
                    <label for="txtPassword">
                        Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="350px" CssClass="form-control"
                        placeholder="Enter Password" required />

                    <div class="checkbox">
                        <asp:CheckBox ID="chkRememberMe" Text="Beni Hatırla" Width="200px" runat="server" />
                    </div>
                    <asp:Button ID="btnLogin" Text="Login" runat="server" OnClick="login_Click" Class="btn btn-primary" />
                    <br />
                    <br />
                    <div id="dvMessage" runat="server" visible="false" class="alert alert-danger">
                        <strong>Error!</strong>
                        <asp:Label ID="lblMessage" runat="server" />
                    </div>
                </center>
            </div>
        </div>
    </form>
</body>
</html>
