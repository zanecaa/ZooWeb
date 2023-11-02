using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using System.Data.SqlClient;
using System.Numerics;

namespace ZooWeb.Pages.ZooUsers
{
    public class IndexModel : PageModel
    {
        public List<ZooUserInfo> listZooUsers = new List<ZooUserInfo>();
        public void OnGet()
        {
            string connectionString = "Server=tcp:zoowebdbserver.database.windows.net,1433;Database=ZooWeb_db;User ID=zooadmin;Password=peanuts420!;Trusted_Connection=False;Encrypt=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                String sql = "SELECT UserID, e.EmployeeId, Username, Passwd, AccountDisabled, CreationDate " 
                    + "FROM zoo_user AS u, employee AS e "
                    + "WHERE u.EmployeeId = e.EmployeeId";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ZooUserInfo info = new ZooUserInfo();
                            info.UserId = reader.GetInt32(0).ToString();
                            info.EmployeeId = reader.GetInt32(1).ToString();
                            info.Username = reader.GetString(2);
                            info.Password = reader.GetString(3);
                            byte[] accountStatusData = (byte[])reader["AccountDisabled"];
                            if (accountStatusData[0] == 1) { info.AccountDisabled = "disabled"; } else { info.AccountDisabled = "enabled"; }
							info.CreationDate = reader.GetDateTime(5).ToString();
                            
							listZooUsers.Add(info);
                        }
                    }
                }
            }
        }
    }

    public class ZooUserInfo
    {
        public string UserId;
        public string EmployeeId;
        public string Username;
        public string Password;
        public string AccountDisabled;
        public string CreationDate;   
    }
}