using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Reflection;


namespace ZooWeb.Pages.ZooUsers
{
	public class CreateModel : PageModel
	{
		public ZooUserInfo info = new ZooUserInfo();
		public string errorMsg = "";
		public string successMsg = "";
		public void OnGet()
		{
		}

		public void OnPost()
		{
			//must add check for null later
			info.Username = Request.Form["Username"];
			info.PasswordHash = Argon2.Hash(Request.Form["Password"]);
			//info.IsActive = Request.Form["Status"];

			FieldInfo[] fields = info.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

			foreach (FieldInfo field in fields)
			{
				object fieldValue = field.GetValue(info);
				/*String acct_status = Request.Form["Status"];
				if (acct_status != "disabled" && acct_status != "enabled")
				{
					errorMsg = "Account Status must be \"enabled\" or \"disabled\", not whatever \"" + acct_status + "\" is...";
					return;
				}*/
				// we should have to check for status but it doesn't seem to work otherwise
				if (field.Name != "UserId"
					&& field.Name != "CreationDate" 
					&& field.Name != "IsActive"
					&& (fieldValue == "" || fieldValue == null))
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
					string sql = "INSERT INTO zoo_user (Username, PasswordHash, IsActive)" +
						"VALUES (@Username, @Password, @Status)";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@Username", info.Username);
						command.Parameters.AddWithValue("@Password", info.PasswordHash);
						//command.Parameters.AddWithValue("@Status", (info.IsActive == "enabled"));
						command.Parameters.AddWithValue("@Status", true);

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
			successMsg = "New User Added";

			Response.Redirect("/ZooUsers/Index");
		}
	}
}
