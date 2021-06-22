using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _01electronics_erp;

namespace _01electronics_logistics
{
    class AGENCY_MACROS
    {

        // TIME UNITES
        public const int DAYS = 1;
        public const int WEEKS = 2;
        public const int MONTHS = 3;
        public const int YEARS = 4;
        public const int HOURS = 5;
        public const int MINUTES = 6;
        public const int SECONDS = 7;


        // AGENCIES STRUCTS
        public struct AGENCY_STRUCT
        {
            public String agency_name;
            public int agency_serial;
        };

        public struct AGENCY_FULL_INFO
        {

            public String agency_name;
            public String employee_name;
            public List<String> telephones;
            public List<String> faxes;
            public List<AGENCY_ADDRESS> branches;


        }

        public struct AGENCY_ADDRESS
        {
            public int addressId;

            public String country;
            public String state;
            public String city;
            public String district;

        }

        public struct AGENCY_CONTACT_INFO
        {
            public int contactId;
            public int employeeId;
            public int branchId;
            public COMPANY_ORGANISATION_MACROS.DEPARTMENT_STRUCT department;

            public String name;
            public String email;
            public String gender;
            public List<String> telephones;

        }



        
    }
}
