using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;


namespace iIECaB
{
    public class Common
    {
        private static string DBName;
        private static string ServerName;
        private static string uName;
        private static string pwd;
        internal static string GetConnectionString(string UserName, string Password)
        {
            return "Server=" + ServerName + ";database=" + DBName + ";uid=" + uName + ";pwd=" + pwd + ";";
        }
        static Common()
        {
            try
            {
                DBName = ConfigurationManager.AppSettings["DBName"];
                ServerName = ConfigurationManager.AppSettings["DBServerName"];
                uName = ConfigurationManager.AppSettings["DBUserName"];
                pwd = ConfigurationManager.AppSettings["DBPassword"];
            }
            catch (Exception ex)
            {
                
            }
        }
        internal static UserContext Authenticate(string UserName, string Password)
        {
            //     Change History
            //
            //      Date        Edit    Author      Comment
            //   -----------+-------+-------+---------------------------------------------
            //     02-Feb-12    [100]    SSN        Created
            //   -----------+-------+-------+---------------------------------------------

           
                UserContext userInfo = new UserContext();
                userInfo.Authenticated = false;
                if (UserName.Trim() != "" && Password.Trim() != "")
                {
                    try
                    {

                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = GetConnectionString(UserName, Password);
                        con.Open();
                        if (con.State == ConnectionState.Open)
                        {
                            using (SqlCommand cmd = con.CreateCommand())
                            {
                                //cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandText = "select users_refno from users where username='" + UserName + "' and password='" + Password + "'";
                                using (SqlDataReader dr = cmd.ExecuteReader())
                                {
                                    if (dr.HasRows)
                                    {
                                        //   return userInfo;
                                        dr.Read();
                                        userInfo.Authenticated = true;
                                        userInfo.userName = UserName;
                                        userInfo.UserRefno = ((int)dr.GetValue(dr.GetOrdinal("users_REFNO")));



                                    }
                                }
                            }
                        }
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        userInfo.Authenticated = false;
                    }
                }
                else
                {
                    userInfo.Authenticated = false;
                }
           
            return userInfo;
        }
        internal static Dictionary<string,string> GetQuesAndAns(int level_refno,int event_refno)
        {
            Dictionary<string, string> dictResults = new Dictionary<string, string>();
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@level_refno", Convert.ToInt32(level_refno));
            Parameters.Add("@event_refno", Convert.ToInt32(event_refno));
            results = GetResultsFromSp("sp_GetQuestionAndAnswer", Parameters);
            foreach (Dictionary<string, object> Row in results)
            {
                dictResults.Add((string)Row["QUESTIONS"], (string)Row["ANSWERS"]);
            }
            return dictResults;
        }
        internal static int GetUserLevel(int user_refno,int event_refno)
        {
            int userLevel;
            userLevel = 0;
            List<Dictionary<string, object>> userLevels = new List<Dictionary<string, object>>();
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@USER_REFNO", Convert.ToInt32(user_refno));
            Parameters.Add("@event_refno", Convert.ToInt32(event_refno));
            userLevels = GetResultsFromSp("sp_GetUserLevel", Parameters);
            foreach (Dictionary<string, object> Row in userLevels)
            {
                userLevel = (Int32)Row["LEVEL_REFNO"];
            }
            return userLevel;
        }
        internal static List<Dictionary<string, Object>> GetResultsFromSp(string SPName, Dictionary<string, object> Parameters)
        {
            //     Change History
            //
            //      Date        Edit    Author      Comment
            //   -----------+-------+-------+---------------------------------------------
            //     02-Feb-12    [100]    SSN        Created
            //   -----------+-------+-------+---------------------------------------------

            SqlDataReader reader;
            reader = null;
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();
            Dictionary<string, Object> ColValue = new Dictionary<string, object>();
            try
            {
                SqlConnection cn = new SqlConnection(GetConnectionString(uName,pwd));
                cn.Open();
                if (cn.State == ConnectionState.Open)
                {
                    SqlCommand Cmd = cn.CreateCommand();
                    Cmd.CommandType = CommandType.StoredProcedure;
                    Cmd.CommandText = SPName;
                    foreach (var item in Parameters)
                    {
                        Cmd.Parameters.AddWithValue(item.Key, item.Value);
                    }
                    reader = Cmd.ExecuteReader();
                    results = CheckAndReturnValue(reader);

                }
                cn.Close();
            }
            catch (Exception ex)
            {
                //UserInfo.Authenticated = false;
            }
            return results;
        }
        
        private static List<Dictionary<string, object>> CheckAndReturnValue(SqlDataReader reader)
        {
            //     Change History
            //
            //      Date        Edit    Author      Comment
            //   -----------+-------+-------+---------------------------------------------
            //     02-Feb-12    [100]    SSN        Created
            //   -----------+-------+-------+---------------------------------------------

            object obj;
            int Ordinal = 0;
            obj = null;
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();
            Dictionary<string, object> ColValues;
            while (reader.Read())
            {
                ColValues = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetFieldType(i) == Type.GetType("System.String"))
                    {
                        obj = reader.IsDBNull(i) ? String.Empty : reader.GetValue(i);
                    }
                    else if (reader.GetFieldType(i) == Type.GetType("System.Decimal"))
                    {
                        obj = reader.IsDBNull(i) ? 0 : reader.GetValue(i);
                    }
                    else if (reader.GetFieldType(i) == Type.GetType("System.DateTime"))
                    {
                        obj = reader.IsDBNull(i) ? new DateTime() : reader.GetDateTime(i);
                    }
                    else
                    {
                        obj = reader.IsDBNull(Ordinal) ? null : reader.GetValue(i);
                    }
                    ColValues.Add(reader.GetName(i).ToUpper(), obj);
                }
                results.Add(ColValues);
            }
            return results;
        }
        internal static int InsertNewLevel(int level,int user_refno,int event_refno)
        {
            SqlConnection con=new SqlConnection(GetConnectionString(uName,pwd));
            con.Open();
            if(con.State==ConnectionState.Open)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "insert into userlevels(USERS_REFNO,LEVEL_REFNO,EVENT_REFNO) values(" + user_refno + "," + level + "," + event_refno + ")";
                return cmd.ExecuteNonQuery();
            }
            return 1;
        }
        internal static int UpdateNewLevel(int level,int user_refno,int event_refno)
        {
            SqlConnection con = new SqlConnection(GetConnectionString(uName, pwd));
            con.Open();
            if (con.State == ConnectionState.Open)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "update userlevels set LEVEL_REFNO=" + level + " where users_refno=" + user_refno;
                return cmd.ExecuteNonQuery();
            }
            return 1;
        }
        internal static string[] GetImagesPath(int nLevel)
        {
            string[] imgPath=new string[2];
            int i;
            i = 0;
            List<Dictionary<string, object>> imgPaths = new List<Dictionary<string, object>>();
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@LEVEL_REFNO", Convert.ToInt32(nLevel));
            imgPaths = GetResultsFromSp("sp_GetImagePath", Parameters);
            foreach (Dictionary<string, object> Row in imgPaths)
            {
                imgPath[i]=(String)Row["PATH"];
                i++;
            }
            return imgPath;
        }
        internal static int CreateNewUser(string userName,string email,string passWord)
        {
            bool bValidate;
            bValidate=ValidateData(userName,"USERNAME");
            if(bValidate)
                bValidate=ValidateData(email,"EMAILID");
            if (bValidate)
            {
                SqlConnection con = new SqlConnection(GetConnectionString(uName, pwd));
                con.Open();
                if (con.State == ConnectionState.Open)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "insert into USERS(USERNAME,PASSWORD,EMAILID) values('" + userName + "','" + passWord + "','" + email + "')";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "select @@identity userrefno";
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            return (Convert.ToInt32(dr.GetDecimal(0)));
                      
                        }
                    }
                }
            }
            return 0;
            
        }
        private static bool ValidateData(string value,string fieldName)
        {
            
                    
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = GetConnectionString(uName, pwd);
                    con.Open();
                    if (con.State == ConnectionState.Open)
                    {
                        using (SqlCommand cmd = con.CreateCommand())
                        {
                            //cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "select users_refno from users where " + fieldName + "='" + value + "'";
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.HasRows)
                                {
                                    return false;
                                }
                            }
                        }
                    }
            return true;
        }
        private void CallErrorPage()
        {
           
        }
    }
}