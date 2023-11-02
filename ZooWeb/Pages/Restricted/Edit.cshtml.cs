using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using System.Data.SqlClient;
using System.Reflection;

namespace ZooWeb.Pages.Restricted
{
	public class EditModel : PageModel
	{
		public RestrictedInfo info = new RestrictedInfo();
		public string errorMsg = "";
		public string successMsg = "";
		public void OnGet()
		{
			string locationid = Request.Query["id"];
			string connectionString = "Server=tcp:zoowebdbserver.database.windows.net,1433;Database=ZooWeb_db;User ID=zooadmin;Password=peanuts420!;Trusted_Connection=False;Encrypt=True;";
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				String sql = "SELECT * FROM restricted WHERE Location_ID=@Location_ID";
				using (SqlCommand command = new SqlCommand(sql, connection))
                {
					command.Parameters.AddWithValue("@Location_ID", locationid);
					using (SqlDataReader reader = command.ExecuteReader())
					{
						if (reader.Read())
						{
                            RestrictedInfo info = new RestrictedInfo();
                            info.Location_ID = reader.GetInt64(0).ToString();
                            info.Close_date = reader.GetDateTime(1).ToString("yyyy-MM-dd HH:mm:ss");
                            info.Reopen_date = reader.GetDateTime(2).ToString("yyyy-MM-dd HH:mm:ss");
                        }
					}
				}
			}
		}
		public void OnPost()
		{
            //must add check for null later
            info.Location_ID = Request.Form["Location_ID"];
            info.Close_date = Request.Form["Close_date"];
            info.Reopen_date = Request.Form["Reopen_date"];

            FieldInfo[] fields = info.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

			foreach (FieldInfo field in fields)
			{
				object fieldValue = field.GetValue(info);
				if ((fieldValue == "" || fieldValue == null))
				{
					errorMsg = "All fields are required";
					return;
				}
			}

			try
			{
				string connectionString = "Server=tcp:zoowebdbserver.database.windows.net,1433;Database=ZooWeb_db;User ID=zooadmin;Password=peanuts420!;Trusted_Connection=False;Encrypt=True;";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					string sql = "UPDATE restricted " +
						"SET Close_date=@Close_date, Reopen_date=@Reopen_date " +
                        "WHERE Location_ID=@Location_ID";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
                        command.Parameters.AddWithValue("@Location_ID", info.Location_ID);
                        command.Parameters.AddWithValue("@Close_date", info.Close_date);
                        command.Parameters.AddWithValue("@Reopen_date", info.Reopen_date);

                        command.ExecuteNonQuery();
                    }
				}
			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
				return;
			}

			foreach (FieldInfo field in fields)
			{
				field.SetValue(info, "");
			}
			successMsg = "Restriction Updated";

            Response.Redirect("/Restricted/Index");
		}
	}
}
