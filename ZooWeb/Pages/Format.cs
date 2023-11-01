namespace ZooWeb.Pages
{
	public class Format
	{
		public static string FormatPhoneNumber(string phoneNumber)
		{
			if (phoneNumber.Length != 10)
			{
				throw new ArgumentException("Phone number must be 10 digits long.");
			}

			string areaCode = phoneNumber.Substring(0, 3);
			string prefix = phoneNumber.Substring(3, 3);
			string lineNumber = phoneNumber.Substring(6, 4);

			string formattedNumber = $"{areaCode}-{prefix}-{lineNumber}";

			return formattedNumber;
		}
	}
}
