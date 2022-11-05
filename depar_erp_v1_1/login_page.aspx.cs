using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace depar_erp_v1_1
{
    public partial class login_page : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void login_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim().ToUpper();
            string password = txtPassword.Text.Trim();
            int userId = 0;
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Validate_User"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Connection = con;
                    con.Open();
                    userId = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }
                if (userId == -1)
                {
                    dvMessage.Visible = true;
                    lblMessage.Text = "Kullanıcı veya şifre hatalı tekrar deneyiniz.";
                }
                else
                {
                    //login page web.config üzerinden tanımlanmıştır.
                    FormsAuthentication.RedirectFromLoginPage(username, chkRememberMe.Checked);
                    //Response.Redirect("defult url den başka bir url girileek ise buraya yazılır.");
                }
            }

        }
    }
}