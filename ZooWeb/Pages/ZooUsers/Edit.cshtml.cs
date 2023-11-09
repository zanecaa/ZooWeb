using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Reflection;
using Isopoh.Cryptography.Argon2;

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
							info.PasswordHash = reader.GetString(2);
							Boolean accountStatusData = (Boolean)reader["IsActive"];
							if (accountStatusData) { info.IsActive = "enabled"; }
							else { info.IsActive = "disabled";  }							
							info.CreationDate = reader.GetDateTime(4).ToString();
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
			info.PasswordHash = Argon2.Hash(Request.Form["Password"]);
			info.IsActive = Request.Form["Status"];
			info.CreationDate = Request.Form["CreationDate"];

			FieldInfo[] fields = info.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

			foreach (FieldInfo field in fields)
			{
				object fieldValue = field.GetValue(info);
				String acct_status = Request.Form["Status"];
				if (acct_status != "disabled" && acct_status != "enabled")
				{
					errorMsg = "Account Status must be \"enabled\" or \"disabled\", not whatever \"" + acct_status + "\" is...";
					return;
				}
				if (field.Name != "UserId" && field.Name != "CreationDate" && (fieldValue == "" || fieldValue == null))
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
						"SET Username=@Username, PasswordHash=@Password, IsActive=@Status " +
						"WHERE UserId=@UserId";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@UserId", info.UserId);
						command.Parameters.AddWithValue("@Username",info.Username);
						command.Parameters.AddWithValue("@Password", info.PasswordHash);
						command.Parameters.AddWithValue("@Status", (info.IsActive == "enabled"));
						//command.Parameters.AddWithValue("@CreationDate", info.CreationDate);

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
