using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Reflection;
using System.Linq;

namespace ZooWeb.Pages.TicketSales
{
    public class CreateModel : PageModel
    {
        public TicketSaleInfo info = new TicketSaleInfo();
        public string errorMsg = "";
		public string successMsg = "";
        public void OnGet()
        {
        }

        public void OnPost() 
        {
			//must add check for null later
			//info.TicketID = Request.Form["TicketID"];
			info.PassType = Request.Form["PassType"];
			info.EmployeeID = Request.Form["EmployeeID"];
			info.VisitorPn = Request.Form["VisitorPn"];
			info.SaleTotal = Request.Form["Total"];
			//info.SaleDate = "placeholder";
			//Decimal total = decimal.Parse(Request.Form["Total"]);


			FieldInfo[] fields = info.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
			string[] excludedFields = { "TicketID", "SaleDate" };

			foreach (FieldInfo field in fields)
			{
				object fieldValue = field.GetValue(info);
				if (!excludedFields.Contains(field.Name) && (fieldValue == "" || fieldValue == null))
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
					string sql = "INSERT INTO ticket_sales (Pass_type, Eid, Visitor_pn, Sale_total) " +
						"VALUES (@PassType, @Eid, @VisitorPn, @SaleTotal)";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@PassType", info.PassType);
						command.Parameters.AddWithValue("@Eid", info.EmployeeID);
						command.Parameters.AddWithValue("@VisitorPn", info.VisitorPn);
						command.Parameters.AddWithValue("@SaleTotal", info.SaleTotal);

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
			successMsg = "New Ticket Added";

			Response.Redirect("/TicketSales/Index");
		}
    }
}
