using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Numerics;

namespace ZooWeb.Pages.Restricted
{
	[Authorize(Policy = "admin")]
	public class IndexModel : PageModel
    {
        public List<RestrictedInfo> Restrictions = new List<RestrictedInfo>();
        public void OnGet()
        {
            //try
            //{
                string connectionString = "Server=tcp:zoowebdbserver.database.windows.net,1433;Database=ZooWeb_db;User ID=zooadmin;Password=peanuts420!;Trusted_Connection=False;Encrypt=True;";

                using (SqlConnection connection = new SqlConnection(connectionString)) 
                { 
                    connection.Open();
                    String sql = "SELECT * FROM restricted";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()) 
                            { 
                                RestrictedInfo info = new RestrictedInfo();
                                info.Location_ID = reader.GetInt64(0).ToString();
                                info.Close_date = reader.GetDateTime(1).ToString("yyyy-MM-dd HH:mm:ss");
                                info.Reopen_date = reader.GetDateTime(2).ToString("yyyy-MM-dd HH:mm:ss");

                            Restrictions.Add(info);
                            }
                        }
                    }
                }
        }
    }

    public class RestrictedInfo
    {
        public string Location_ID;
        public string Close_date;
        public string Reopen_date;
    }
}  
//hello
