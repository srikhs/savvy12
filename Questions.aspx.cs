using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace iIECaB
{
    public partial class Questions : System.Web.UI.Page
    {
        string mQuestion;
        string mAnswer;
        int mLevel;
        int mUserRefno;
        int maxLevel;
        int mEvent;
        protected void Page_Load(object sender, EventArgs e)
        {
            int level;
            //Image1.ImageUrl = "DSC00144.jpg";
            try
            {
                Image1.Visible = false;
                Image2.Visible = false;
                LoadDefault();
                if (Session["MaxQues"] != null)
                    maxLevel = (int)Session["MaxQues"];
                if (Session["Event"] != null)
                    mEvent = (int)Session["Event"];
                if (!IsPostBack)
                {
                    //Image1.Visible = false;
                    //Image2.Visible = false;
                    string username;
                    username = "";
                    int userRefno;
                    userRefno = 0;
                    Dictionary<string, string> dictQA = new Dictionary<string, string>();
                    if (Session["UserName"] != null)
                    {
                        username = (string)Session["UserName"];
                    }
                    else
                        Response.Redirect("Default.aspx");
                    if (Session["UserRefno"] != null)
                    {
                        userRefno = (int)Session["UserRefno"];
                        mUserRefno = userRefno;
                    }
                    else
                        Response.Redirect("Default.aspx");

                    level = Common.GetUserLevel(userRefno, mEvent);
                    Label2.Text = "";
                    //Session["Level"] = mLevel;
                    Label1.Text = level.ToString();
                    if (level <= maxLevel)
                    {
                        if (level == 0)
                        {
                            level = 1;
                            Common.InsertNewLevel(1, userRefno, mEvent);
                            dictQA = Common.GetQuesAndAns(1, mEvent);
                            //Session["Level"] = 1;
                        }
                        else
                            dictQA = Common.GetQuesAndAns(level, mEvent);
                        foreach (var row in dictQA)
                        {
                            mLevel = level;
                            mQuestion = (string)row.Key;
                            Session["Question"] = mQuestion;
                            mAnswer = (String)row.Value;
                            Session["Answer"] = mAnswer;
                            Session["Level"] = level;
                            Label1.Text = (string)row.Key;
                        }
                        LevelLabel.Text = level.ToString() + "/" + maxLevel.ToString();

                        if (mEvent == 1)
                            SetImages();
                    }
                    else
                    {
                        Response.Redirect("Completed.aspx",false);
                    }
                }
                else
                {
                    if ((Session["UserName"] == null) || (Session["UserRefno"] == null))
                    {
                        Response.Redirect("Default.aspx");
                    }
                    //Label1.Text = (string)Session["UserName"];
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("error.aspx");
            }
        }

       

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Session.RemoveAll();
                Response.Redirect("default.aspx");
            }
            catch (Exception ex)
            {
                Response.Redirect("error.aspx");
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {

                if (Session["UserName"] != null)
                {
                    Dictionary<string, string> dictQA = new Dictionary<string, string>();
                    GetSessionVars();
                    mUserRefno = (int)Session["UserRefno"];
                    if (TextBox1.Text.ToUpper().CompareTo(mAnswer) == 0)
                    {
                        mLevel = mLevel + 1;
                        Common.UpdateNewLevel(mLevel, mUserRefno, mEvent);
                        if (maxLevel >= mLevel)
                        {
                            Label2.Text = "";
                            dictQA = Common.GetQuesAndAns(mLevel, mEvent);
                            foreach (var row in dictQA)
                            {
                                mQuestion = (string)row.Key;
                                mAnswer = (String)row.Value;
                                SetSessionVars();

                            }
                        }
                        else
                        {
                            Response.Redirect("Completed.aspx",false);
                        }
                    }
                    else
                    {
                        Label2.Text = "Wrong! Try Again";
                    }
                    InitializeControls();
                }
                else
                {
                    Response.Redirect("Default.aspx");
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("error.aspx");
            }
        }
        private void SetSessionVars()
        {
            try
            {
                Session["Question"] = mQuestion;
                Session["Answer"] = mAnswer;
                Session["Level"] = mLevel;
            }
            catch (Exception ex)
            {
                Response.Redirect("error.aspx");
            }
        }
        private void GetSessionVars()
        {
            try
            {
                mQuestion = (String)Session["Question"];
                mAnswer = (String)Session["Answer"];
                mLevel = (int)Session["Level"];
            }
            catch (Exception ex)
            {
                Response.Redirect("error.aspx");
            }
            
        }
        private void SetImages()
        {
            try
            {
                string[] imgPath;
                imgPath = Common.GetImagesPath(mLevel);
                for (int i = 0; i < imgPath.Length; i++)
                {
                    if (i == 0)
                    {
                        if (imgPath[0] != null)
                        {
                            Image1.Visible = true;
                            Image1.ImageUrl = imgPath[0] + ".jpg";
                        }
                    }
                    else
                    {   
                        if (imgPath[1] != null)
                        {
                            Image2.Visible = true;
                            Image2.ImageUrl = imgPath[1];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("error.aspx");
            }
        }
        private void InitializeControls()
        {
            try
            {
                TextBox1.Text = "";
                Label1.Text = mQuestion;
                LevelLabel.Text = mLevel.ToString() + "/" + maxLevel.ToString();
                if(mEvent==1)
                    SetImages();
            }
            catch (Exception ex)
            {
                Response.Redirect("error.aspx");
            }
        }
        private void LoadDefault()
        {
            try
            {
                mEvent = Convert.ToInt32(Request.QueryString["type"]);
                if (mEvent == 1)
                {
                    maxLevel = Convert.ToInt32(ConfigurationManager.AppSettings["WebHuntMaxQues"]);
                }
                else if (mEvent == 2)
                {
                    maxLevel = Convert.ToInt32(ConfigurationManager.AppSettings["QuizMaxQues"]);

                }
                Session["MaxQues"] = maxLevel;
                Session["Event"] = mEvent;
            }
            catch (Exception ex)
            {
                Response.Redirect("error.aspx");
            }
        }
    }
}