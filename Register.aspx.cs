using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace iIECaB
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void CreatedUserButton_Click(object sender, EventArgs e)
        {
           // Register reg = new Register();
            int i;
            int userRefno;
            userRefno=Common.CreateNewUser(UserName.Text, Email.Text, Password.Text);
            if (userRefno != 0)
            {
                Session["UserName"] =  UserName.Text;
                Session["UserRefno"] = userRefno;
                Response.Redirect("SelectEvent.aspx");
            }
        }

        protected void RegisterUser_CreatedUser(object sender, EventArgs e)
        {
           // FormsAuthentication.SetAuthCookie(RegisterUser.UserName, false /* createPersistentCookie */);

            string continueUrl = ""; // RegisterUser.ContinueDestinationPageUrl;
            if (String.IsNullOrEmpty(continueUrl))
            {
                continueUrl = "Questions.aspx";
            }
            Response.Redirect(continueUrl);
        }
    }
}