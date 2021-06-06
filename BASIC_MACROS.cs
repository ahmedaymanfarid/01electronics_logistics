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

namespace _01electronics_erp
{
    public class BASIC_MACROS
    {
        //INEGRITY CHECKS MACROS
        public const int CONTACT_PHONE = 0;
        public const int COMPANY_PHONE = 1;
        public const int COMPANY_FAX = 2;

        public const int REGULAR_STRING = 1;
        public const int EMAIL_STRING = 2;
        public const int DOMAIN_STRING = 3;
        public const int PHONE_STRING = 4;
        public const int MONETARY_STRING = 5;
        public const int FILE_PATH_STRING = 6;
        public const int NUMERIC_STRING = 7;

        public const int CONTACT_BUSINESS_EMAIL_EDITED = 0x00000001;
        public const int CONTACT_PHONE1_EDITED = 0x00000010;
        public const int CONTACT_PHONE2_EDITED = 0x00000100;
        public const int CONTACT_PHONE3_EDITED = 0x00001000;
        public const int CONTACT_EMAIL1_EDITED = 0x00010000;
        public const int CONTACT_EMAIL2_EDITED = 0x00100000;

        public const int SQL_DATE_LENGTH = 11;
        public const int SQL_DATETIME_LENGTH = 20;
        public const int ID_DATE_LENGTH = 9;

        public const int EDIT_BOX_STRING_LENGTH = 100;

        //YEAR QUARTERS MACROS
        public const int NO_OF_QUARTERS = 4;
        public const int FIRST_QUARTER = 1;
        public const int SECOND_QUARTER = 2;
        public const int THIRD_QUARTER = 3;
        public const int FOURTH_QUARTER = 4;

        public const String FIRST_QUARTER_START_DATE = "01-01";
        public const String SECOND_QUARTER_START_DATE = "04-01";
        public const String THIRD_QUARTER_START_DATE = "07-01";
        public const String FOURTH_QUARTER_START_DATE = "10-01";

        public const int FIRST_QUARTER_START_MONTH = 1;
        public const int SECOND_QUARTER_START_MONTH = 4;
        public const int THIRD_QUARTER_START_MONTH = 7;
        public const int FOURTH_QUARTER_START_MONTH = 10;

        public const int FIRST_QUARTER_END_MONTH = 3;
        public const int SECOND_QUARTER_END_MONTH = 6;
        public const int THIRD_QUARTER_END_MONTH = 9;
        public const int FOURTH_QUARTER_END_MONTH = 12;

        public const int BASE_STATISTICAL_YEAR = 2019;

        public const int DATE_STRING_LENGTH = 11;
        public const int MONTH_AND_DAY_STRING_LENGTH = 6;
        public const int YEAR_STRING_LENGTH = 5;

        //STATISTICS LISTS MACROS
        public const int YEARLY_STATISTICS_LIST_COLUMNS = 6;
        public const int BADGES_LIST_COLUMNS = 3;
        public const int QUARTERLY_CHANGE_RATE_LIST_COLUMNS = 4;
        public const int QUARTERLY_STATISTICS_LIST_COLUMNS = 2;

        public const int MAXIMUM_COMMENT_LENGTH = 150;
        public const int COMMENT_LENGTH_SAFETY_MARGIN = MAXIMUM_COMMENT_LENGTH + 100;

        //WORLD MAP LIMITS MACROS
        public const int MAXIMUM_STATES_NO = 100;
        public const int MAXIMUM_CITIES_NO = 100;
        public const int MAXIMUM_DISTRICTS_NO = 100;

        //SQL MACROS
        public const int MAX_STRING_COLUMNS = 20;
        public const int MAX_NUMERIC_COLUMNS = 30;
        public const int MAX_DATE_COLUMNS = 2;
        public const int MAX_TIMESTAMP_COLUMNS = 2;

        public const int SEVERITY_LOW = 1;
        public const int SEVERITY_MEDIUM = 2;
        public const int SEVERITY_HIGH = 3;

        //FTP MACROS
        public const String FTP_SERVER_IP = "01electronics.net";
        public const String FTP_SERVER_USERNAME = "agent@01electronics.net";
        public const String FTP_SERVER_PASSWORD = "A7mad0layainmO";

        public const int FTP_SERVER_PORT = 21;

        //PRODUCTS OFFERS FORM PATHS
        public const String MODELS_OFFERS_PATH = "/erp_system/products_offer_models/generic/";
        public const String MODELS_PHOTOS_PATH = "/erp_system/products_photos/";
        public const String OFFER_FILES_PATH = "/erp_system/work_offers/";

        public const String YEARLY_SALES_EMPLOYEE_STATISTICS_EXCEL_PATH = "/erp_system/statistics_sheet_models/yearly_sales_employee_statistics.xls";
        public const String QUARTERLY_SALES_EMPLOYEE_STATISTICS_EXCEL_PATH = "/erp_system/statistics_sheet_models/quarterly_sales_employee_statistics.xls";
        public const String YEARLY_TECH_EMPLOYEE_STATISTICS_EXCEL_PATH = "/erp_system/statistics_sheet_models/yearly_tech_employee_statistics.xls";
        public const String QUARTERLY_TECH_EMPLOYEE_STATISTICS_EXCEL_PATH = "/erp_system/statistics_sheet_models/quarterly_tech_employee_statistics.xls";

        public const String EMPLOYEES_DOCUMENTS_PATH = "/erp_system/employees_documents/";
        //BANK MACROS
        public const int NO_OF_PAYROLL_BANKS = 3;
        public const int CASH = 0;
        public const int QNB = 1;
        public const int NBE = 2;

        public const String COMPANY_DOMAIN = "@01electronics.net";
    }
}

