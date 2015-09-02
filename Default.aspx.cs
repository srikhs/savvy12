using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace iIECaB
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Label1.Visible = false;
            }
        }

        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    UserContext uInfo = new UserContext();
        //    int maxQues;
        //    uInfo = Common.Authenticate(TextBox1.Text, TextBox2.Text);
        //    if(uInfo.Authenticated)
        //    {
        //        maxQues =Convert.ToInt32(ConfigurationManager.AppSettings["MaxQues"]);
        //        //TextBox1.Text = "";
        //        //TextBox1.Text = "Success";
        //        Session["Username"] = TextBox1.Text;
        //        Session["UserRefno"] = uInfo.UserRefno;
        //        Session["MaxQues"] = maxQues;
        //        Response.Redirect("Questions.aspx");
        //    }
        //    else
        //    {
        //        TextBox1.Text = "Failure";
        //    }
        //}

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            UserContext uInfo = new UserContext();
           // int maxQues;
            uInfo = Common.Authenticate(UserName.Text.ToUpper(), Password.Text);
            if (uInfo.Authenticated)
            {
                
                //TextBox1.Text = "";
                //TextBox1.Text = "Success";
                Session["Username"] = UserName.Text;
                Session["UserRefno"] = uInfo.UserRefno;
               // Session["MaxQues"] = maxQues;
                Response.Redirect("SelectEvent.aspx");
            }
            else
            {
                Label1.Visible = true;
                Label1.Text = "Username or Password is incorrect. Please try again!";
                Password.Text = "";
            }
        }
        protected void RegisterButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("Register.aspx");
        }

        protected void RegisterUser_CreatedUser(object sender, EventArgs e)
        {

        }
    }
}
