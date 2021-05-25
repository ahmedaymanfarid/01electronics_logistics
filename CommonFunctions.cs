using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Collections;
using System.Windows;
using System.Globalization;

namespace _01electronics_erp
{
	public class CommonFunctions
	{
		char[] SQLdateString;
		char[] IDdateString;
		public CommonFunctions()
		{
		}

		public DateTime GetTodaysDate()
		{
			DateTime todaysDate = DateTime.Now;
			return todaysDate;
		}

		public String GetTimeID(DateTime inputStruct)
		{
			String outputString;

			IDdateString = new char[BASIC_MACROS.ID_DATE_LENGTH];

			int tempDay = inputStruct.Day;
			int tempMonth = inputStruct.Month;
			int tempYear = inputStruct.Year;

			for (int i = 0; i < BASIC_MACROS.ID_DATE_LENGTH; i++)
			{
				if (i < 2)
				{
					IDdateString[1 - i] = Convert.ToChar(tempDay % 10);
					tempDay /= 10;
				}
				else if (i < 4)
				{
					IDdateString[5 - i] = Convert.ToChar(tempMonth % 10);
					tempMonth /= 10;
				}
				else if (i < 8)
				{
					IDdateString[11 - i] = Convert.ToChar(tempYear % 10);
					tempYear /= 10;
				}
				else
					IDdateString[i] = '\0';
			}

			outputString = new String(IDdateString);

			return outputString;
		}

		public String GetQuarterName(int quarterNumber)
		{
			String quarterName = String.Empty;

			if (quarterNumber == BASIC_MACROS.FIRST_QUARTER)
				quarterName = "1st QUARTER";
			if (quarterNumber == BASIC_MACROS.SECOND_QUARTER)
				quarterName = "2nd QUARTER";
			if (quarterNumber == BASIC_MACROS.THIRD_QUARTER)
				quarterName = "3rd QUARTER";
			if (quarterNumber == BASIC_MACROS.FOURTH_QUARTER)
				quarterName = "4th QUARTER";

			return quarterName;
		}

		public String GetQuarterStartDate(int quarterNumber, int year)
		{
			String returnDate;

			String quarterStartYear = String.Empty;
			String quarterStartMonthAndDay = String.Empty;
			String quarterStartDate = String.Empty;

			quarterStartYear += year;

			if (quarterNumber == BASIC_MACROS.FIRST_QUARTER)
				quarterStartMonthAndDay += BASIC_MACROS.FIRST_QUARTER_START_DATE;

			else if (quarterNumber == BASIC_MACROS.SECOND_QUARTER)
				quarterStartMonthAndDay += BASIC_MACROS.SECOND_QUARTER_START_DATE;

			else if (quarterNumber == BASIC_MACROS.THIRD_QUARTER)
				quarterStartMonthAndDay += BASIC_MACROS.THIRD_QUARTER_START_DATE;

			else if (quarterNumber == BASIC_MACROS.FOURTH_QUARTER)
				quarterStartMonthAndDay += BASIC_MACROS.FOURTH_QUARTER_START_DATE;

			quarterStartDate += quarterStartYear;
			quarterStartDate += "-";
			quarterStartDate += quarterStartMonthAndDay;

			returnDate = quarterStartDate;
			return returnDate;
		}
		public String GetQuarterEndDate(int quarterNumber, int year)
		{
			String returnDate;

			String quarterEndYear = String.Empty;
			String quarterEndMonthAndDay = String.Empty;
			String quarterEndDate = String.Empty;

			if (quarterNumber == BASIC_MACROS.FOURTH_QUARTER)
				quarterEndYear += year + 1;
			else
				quarterEndYear += year;

			if (quarterNumber == BASIC_MACROS.FIRST_QUARTER)
				quarterEndMonthAndDay += BASIC_MACROS.SECOND_QUARTER_START_DATE;

			else if (quarterNumber == BASIC_MACROS.SECOND_QUARTER)
				quarterEndMonthAndDay += BASIC_MACROS.THIRD_QUARTER_START_DATE;

			else if (quarterNumber == BASIC_MACROS.THIRD_QUARTER)
				quarterEndMonthAndDay += BASIC_MACROS.FOURTH_QUARTER_START_DATE;

			else if (quarterNumber == BASIC_MACROS.FOURTH_QUARTER)
				quarterEndMonthAndDay += BASIC_MACROS.FIRST_QUARTER_START_DATE;

			quarterEndDate += quarterEndYear;
			quarterEndDate += "-";
			quarterEndDate += quarterEndMonthAndDay;

			returnDate = quarterEndDate;
			return returnDate;
		}
		public List<String> GetListOfMonths()
		{
			List<String> listOfMonths = new List<String>();

			listOfMonths.Add("January");
			listOfMonths.Add("February");
			listOfMonths.Add("March");
			listOfMonths.Add("April");
			listOfMonths.Add("May");
			listOfMonths.Add("June");
			listOfMonths.Add("July");
			listOfMonths.Add("August");
			listOfMonths.Add("September");
			listOfMonths.Add("November");
			listOfMonths.Add("December");

			return listOfMonths;
		}
		public int GetMonthLength(int month, int year)
		{
			if (month == 2 && year % 4 == 0)
				return 29;
			else if (month == 2)
				return 28;
			else if (month <= 7 && month % 2 == 0)
				return 30;
			else if (month <= 7 && month % 2 == 1)
				return 31;
			else if (month > 7 && month % 2 == 0)
				return 31;
			else
				return 30;
		}

		int GetCurrentQuarter()
		{
			DateTime todaysDate = DateTime.Now;

			if (todaysDate.Month >= BASIC_MACROS.FIRST_QUARTER_START_MONTH && todaysDate.Month < BASIC_MACROS.SECOND_QUARTER_START_MONTH)
				return BASIC_MACROS.FIRST_QUARTER;
			else if (todaysDate.Month >= BASIC_MACROS.SECOND_QUARTER_START_MONTH && todaysDate.Month < BASIC_MACROS.THIRD_QUARTER_START_MONTH)
				return BASIC_MACROS.SECOND_QUARTER;
			else if (todaysDate.Month >= BASIC_MACROS.THIRD_QUARTER_START_MONTH && todaysDate.Month < BASIC_MACROS.FOURTH_QUARTER_START_MONTH)
				return BASIC_MACROS.THIRD_QUARTER;
			else
				return BASIC_MACROS.FOURTH_QUARTER;
		}

	}

}
