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
    public class COMPANY_ORGANISATION_MACROS
    {
        //DEPARTMENTS AND TEAMS MACROS
        public const int MARKETING_AND_SALES_DEPARTMENT_ID          = 102;
        public const int MARKETING_AND_SALES_DEPARTMENT_MANAGEMENT  = 102;
        public const int SALES_TEAM_ID                              = 10202;
        public const int TECHNICAL_OFFICE_TEAM_ID                   = 10203;

        public const int HUMAN_RESOURCES_DEPARTMENT_ID          = 104;
        public const int HUMAN_RESOURCES_DEPARTMENT_MANAGEMENT  = 104;
        public const int RECTRUITMENT_TEAM_ID                   = 10401;

        //DEPARTMENTS AND TEAMS MACROS
        public const int MANAGER_POSTION        = 8;
        public const int TEAM_LEAD_POSTION      = 800;
        public const int SENIOR_POSTION         = 801;
        public const int JUNIOR_POSTION         = 802;
        
                //EMPLOYEMENT LIMITS MACROS
        public const int MAX_NUMBER_OF_EMPLOYEES = 100;
        
                //CONTACTS LIMITS MACROS
        public const int MAX_NUMBER_OF_COMPANIES     = 1000;
        public const int MAX_CONTACTS_PER_COMPANY    = 100;
        public const int MAX_TELEPHONES_PER_CONTACT  = 3;
        public const int MAX_EMAILS_PER_CONTACT      = 2;
        
        public const int MAX_NUMBER_OF_DEPARTMENTS           = 100;
        public const int MAX_NUMBER_OF_MANAGING_DEPARTMENTS  = 100;
        public const int MAX_NUMBER_OF_TEAMS_PER_DEP         = 100;
        
        public const int MAX_MANAGEMENT_TEAMS    = MAX_NUMBER_OF_TEAMS_PER_DEP;
        public const int MAX_DEPARTMENT_MANAGERS = MAX_NUMBER_OF_DEPARTMENTS;
        public const int MAX_TEAM_LEADS          = MAX_NUMBER_OF_TEAMS_PER_DEP;
        public const int MAX_SENIORS             = MAX_NUMBER_OF_TEAMS_PER_DEP;

        //COMPANIES STRUCTS
        public struct COMPANY_STRUCT
        {
            public String company_name;
            public int company_serial;
        };

        public struct BRANCH_STRUCT
        {
            public int address_serial;
            public int address;
            
            public String district;
            public String city;
            public String state_governorate;
            public String country;
        };

        //COMPANY ORGANISATIONS STRUCTS
        public struct DEPARTMENT_STRUCT
        {
            public String department_name;
            public int department_id;
        };

        public struct TEAM_STRUCT
        {
            public String team_name;
            public int team_id;
        };

        public struct EMPLOYEE_POSITION_STRUCT
        {
            public String position_name;
            public int position_id;
        };

        //PAYROLL AND SALARIES STRUCTS
        public struct BANK_STRUCT
        {
            public String bank_name;
            public int bank_id;

            public String branch_name;
            public Int16 branch_id;

            public int payroll_id;

            public ulong account_id;
            public ulong iban_id;
        };

        public struct PAYROLL_STRUCT
        {
            public String employee_name;
            public int employee_id;

            public List<BANK_STRUCT> banksList;
        }

        public struct SALARY_STRUCT
        {
            public String employee_name;
            public int employee_id;
            public Decimal salary;
        };

        //EMPLOYEES STRUCT
        public struct EMPLOYEE_STRUCT
        {
            public String employee_name;
            public int employee_id;

            public DEPARTMENT_STRUCT department;
            public TEAM_STRUCT team;
            public EMPLOYEE_POSITION_STRUCT position;

            public Decimal salary;
            public List<BANK_STRUCT> payroll_info;

        };

        //CONTACTS STRUCTS
        public struct CONTACT_COMMENT_STRUCT
        {
            public String commentDate;
            public String comment;
        };

        public struct CONTACT_BASIC_STRUCT
        {
            public String contact_name;
            public int contact_id;
        };

        public struct CONTACT_AMATEUR_STRUCT
        {
            public CONTACT_BASIC_STRUCT contact;
            public int sales_person_id;
            public int branch_serial;
        };

        public struct CONTACT_PRO_STRUCT
        {
            public EMPLOYEE_STRUCT sales_person;
            public COMPANY_STRUCT company;
            public BRANCH_STRUCT branch;
            public DEPARTMENT_STRUCT contact_department;

            public String contact_name;
            public int contact_id;

            public String gender;
            public String mobile;

            public String business_email;
            public String personal_email;
        };
    }
}

