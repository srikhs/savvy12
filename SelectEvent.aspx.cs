using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace iIECaB
{
    public partial class SelectEvent : System.Web.UI.Page
    {
        int maxQues;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserName"] == null)
                    Response.Redirect("default.aspx");
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            maxQues = Convert.ToInt32(ConfigurationManager.AppSettings["WebHuntMaxQues"]);
            Session["MaxQues"] = maxQues;
            Session["Event"] = 1;
            Response.Redirect("Questions.aspx?type=1");
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            maxQues = Convert.ToInt32(ConfigurationManager.AppSettings["QuizMaxQues"]);
            Session["MaxQues"] = maxQues;
            Session["Event"] = 2;
            Response.Redirect("Questions.aspx?type=2");
        }
    }
}