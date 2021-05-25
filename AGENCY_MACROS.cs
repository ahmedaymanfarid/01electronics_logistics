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

            public String country;
            public String state;
            public String city;
            public String district;

            public int addressId;

            public String telephone;
        }

        
    }
}
