using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Numerics;

namespace ZooWeb.Pages.FeedingPatterns
{
    public class IndexModel : PageModel
    {
        public List<FeedingPatternInfo> listFeedingPatterns = new List<FeedingPatternInfo>();
        public void OnGet()
        {
            string connectionString = "Server=tcp:zoowebdbserver.database.windows.net,1433;Database=ZooWeb_db;User ID=zooadmin;Password=peanuts420!;Trusted_Connection=False;Encrypt=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                String sql = "SELECT * FROM feeding_pattern";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FeedingPatternInfo info = new FeedingPatternInfo();
                            info.Animal_ID = reader.GetInt32(0).ToString();
                            info.Meal = reader.GetString(1);
                            info.Portion = reader.GetDecimal(2).ToString();
                            info.Schedule_days = reader.GetString(3);
							info.Schedule_time = reader.GetTimeSpan(4).ToString();

							listFeedingPatterns.Add(info);
                        }
                    }
                }
            }
        }
    }

    public class FeedingPatternInfo
    {
        public string Animal_ID;
        public string Meal;
        public string Portion;
        public string Schedule_days;
        public string Schedule_time;
    }
}