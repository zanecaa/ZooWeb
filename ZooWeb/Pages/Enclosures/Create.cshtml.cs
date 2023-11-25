using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Reflection;

namespace ZooWeb.Pages.Enclosures
{
	public class CreateModel : PageModel
	{
		public EnclosureInfo info = new EnclosureInfo();
		public string errorMsg = "";
		public string successMsg = "";
		public void OnGet()
		{
		}

		public void OnPost()
		{
			//must add check for null later
			info.LocationID = Request.Form["LocationId"];
			info.Type = Request.Form["Type"];
			info.Capacity = Request.Form["Capacity"];
			info.Occupant_Num = Request.Form["OccupantNum"];

			FieldInfo[] fields = info.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

			foreach (FieldInfo field in fields)
			{
				object fieldValue = field.GetValue(info);
				if (fieldValue == "" || fieldValue == null)
				{
					errorMsg = "All fields are required";
					return;
				}
			}

			try
			{
				string connectionString = "Server=tcp:zoowebdb.database.windows.net,1433;Database=ZooWeb_db;User ID=zooadmin;Password=peanuts420!;Trusted_Connection=False;Encrypt=True;";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					string sql = "INSERT INTO Enclosure VALUES (@LocationId, @Type, @Capacity, @OccupantNum)";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@LocationId", int.Parse(info.LocationID));
						command.Parameters.AddWithValue("@Type", info.Type);
						command.Parameters.AddWithValue("@Capacity", info.Capacity);
						command.Parameters.AddWithValue("@OccupantNum", info.Occupant_Num);

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
			successMsg = "New Enclosure Added";

			Response.Redirect("/Enclosures/Index");
		}
	}
}
