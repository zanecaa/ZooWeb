using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Reflection;

namespace ZooWeb.Pages.Employees
{
    public class CreateModel : PageModel
    {
		public EmployeeInfo info = new EmployeeInfo();
        public string errorMsg = "";
		public string successMsg = "";
        public void OnGet()
        {
        }

        public void OnPost() 
        {
			//must add check for null later

			info.EmployeeId = Request.Form["Eid"];
			info.Phone_num = Request.Form["Phone_num"];
			info.Dno = Request.Form["Dno"];
			info.Super_Eid = Request.Form["Super_Eid"];
			info.Email = Request.Form["Email"];
			info.Fname = Request.Form["Fname"];
			info.Lname = Request.Form["Lname"];
			info.Salary = Request.Form["Salary"];

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
					string sql = "INSERT INTO employee VALUES (@EmployeeId, @Phone_num, @Dno, @Super_Eid, @Email, @Fname, @Lname, @Salary)";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@EmployeeId", int.Parse(info.EmployeeId));
						command.Parameters.AddWithValue("@Phone_num", long.Parse(info.Phone_num));
						command.Parameters.AddWithValue("@Dno", short.Parse(info.Dno));
						command.Parameters.AddWithValue("@Super_Eid", long.Parse(info.Super_Eid));
						command.Parameters.AddWithValue("@Email", info.Email);
						command.Parameters.AddWithValue("@Fname", info.Fname);
						command.Parameters.AddWithValue("@Lname", info.Lname);
						command.Parameters.AddWithValue("@Salary", int.Parse(info.Salary));

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
			successMsg = "New Employee Added";

			Response.Redirect("/Employees/Index");
		}
    }
}
