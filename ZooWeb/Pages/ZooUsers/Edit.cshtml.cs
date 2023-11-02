using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Reflection;

namespace ZooWeb.Pages.ZooUsers
{
	public class EditModel : PageModel
	{
		public ZooUserInfo info = new ZooUserInfo();
		public string errorMsg = "";
		public string successMsg = "";
		public void OnGet()
		{
			String UserId = Request.Query["id"];
			// TODO: actually use this (addresses race condition)
			if (UserId == null || UserId == "") { errorMsg = "y tho?"; return; };

			string connectionString = "Server=tcp:zoowebdbserver.database.windows.net,1433;Database=ZooWeb_db;User ID=zooadmin;Password=peanuts420!;Trusted_Connection=False;Encrypt=True;";
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				String sql = "SELECT * FROM zoo_user " +
					"WHERE UserId=@UserId";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@UserId", UserId);
					using (SqlDataReader reader = command.ExecuteReader())
					{
						if (reader.Read())
						{
							info.UserId = reader.GetInt32(0).ToString();
							info.Username = reader.GetString(1);
							info.Password = reader.GetString(2);
							Boolean accountStatusData = (Boolean)reader["AccountDisabled"];
							if (accountStatusData) { info.AccountDisabled = "disabled"; }
							else { info.AccountDisabled = "enabled";  }							
							info.CreationDate = reader.GetDateTime(4).ToString();
							info.EmployeeId = reader.GetInt32(5).ToString();
						}
					}
				}
			}
		}
		public void OnPost()
		{
			//must add check for null later
			info.UserId = Request.Form["UserId"];
			info.Username = Request.Form["Username"];
			info.Password = Request.Form["Password"];
			info.AccountDisabled = Request.Form["AccountDisabled"];
			info.CreationDate = Request.Form["CreationDate"];
			info.EmployeeId = Request.Form["EmployeeId"];

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
					string sql = "UPDATE zoo_user " +
						"SET Username=@Username, Password=@Password, AccountDisabled=@AccountDisabled, EmployeeId=@EmployeeId " +
						"WHERE UserId=@UserId";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{

						command.Parameters.AddWithValue("@Username",info.Username);
						command.Parameters.AddWithValue("@Password", info.Password);
						command.Parameters.AddWithValue("@AccountDisabled", (info.AccountDisabled == "disabled"));
						//command.Parameters.AddWithValue("@CreationDate", info.CreationDate);
						command.Parameters.AddWithValue("@EmployeeId", info.EmployeeId);

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
			successMsg = "User Updated";

			Response.Redirect("/ZooUsers/Index");
		}
	}
}
