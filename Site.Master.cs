using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace iIECaB
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null)
                UserNameLabel.Text = (string)Session["UserName"];
            else
                UserNameLabel.Text = "Guest";
        }

        protected void HeadLoginStatus_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Session.RemoveAll();

        }

        protected void NavigationMenu1_MenuItemClick(object sender, MenuEventArgs e)
        {
            if (Session["UserName"] != null)
            {
                if (e.Item.Text.CompareTo("Web Hunt") == 0)
                    Session["Event"] = 1;
                else
                {
                    Session["Event"] = 2;
                }
            }
            else
                Response.Redirect("default.aspx");
        }
    }
}
