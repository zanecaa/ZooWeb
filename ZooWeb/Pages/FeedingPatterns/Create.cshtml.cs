using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Reflection;

namespace ZooWeb.Pages.FeedingPatterns
{
	public class CreateModel : PageModel
	{
		public FeedingPatternInfo info = new FeedingPatternInfo();
		public string errorMsg = "";
		public string successMsg = "";
		public void OnGet()
		{
		}

		public void OnPost()
		{
			//must add check for null later
			info.Animal_ID = Request.Form["AnimalId"];
			info.Meal = Request.Form["Meal"];
			info.Portion = Request.Form["Portion"];
			info.Schedule_days = Request.Form["ScheduleDays"];
			info.Schedule_time = Request.Form["ScheduleTime"];

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
				string connectionString = "Server=tcp:zoowebdbserver.database.windows.net,1433;Database=ZooWeb_db;User ID=zooadmin;Password=peanuts420!;Trusted_Connection=False;Encrypt=True;";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					string sql = "INSERT INTO Feeding_Pattern VALUES (@AnimalId, @Meal, @Portion, @ScheduleDays, @ScheduleTime)";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@AnimalId", int.Parse(info.Animal_ID));
						command.Parameters.AddWithValue("@Meal", info.Meal);
						command.Parameters.AddWithValue("@Portion", info.Portion);
						command.Parameters.AddWithValue("@ScheduleDays", info.Schedule_days);
						command.Parameters.AddWithValue("@ScheduleTime", info.Schedule_time);

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
			successMsg = "New FeedingPattern Added";

			Response.Redirect("/FeedingPatterns/Index");
		}
	}
}
