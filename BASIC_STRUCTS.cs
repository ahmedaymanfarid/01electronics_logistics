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
    public class BASIC_STRUCTS
    {
        enum STATISTICSCRITERIA
        {
            SALES_EMPLOYEE_YEARLY_STATISTICS,
            SALES_EMPLOYEE_QUARTERLY_STATISTICS,
            TECH_EMPLOYEE_YEARLY_STATISTICS,
            TECH_EMPLOYEE_QUARTERLY_STATISTICS,
            MARKT_AND_SALES_EMPLOYEE_YEARLY_STATISTICS,
            MARKT_AND_SALES_EMPLOYEE_QUARTERLY_STATISTICS,
            PRODUCT_YEARLY_STATISTICS,
            PRODUCT_QUARTERLY_STATISTICS,
            BRAND_YEARLY_STATISTICS,
            BRAND_QUARTERLY_STATISTICS,
        };

        public struct DOMAIN_FORM
        {
            public bool charFound;
            public int charIndex;
        };

        public struct CONTRACT_STRUCT
        {
            public String contractName;
            public int contractId;
        };

        public struct DELIVERY_POINT_STRUCT
        {
            public String pointName;
            public int pointId;
        };

        public struct CURRENCY_STRUCT
        {
            public String currencyName;
            public int currencyId;
        };

        public struct TIMEUNIT_STRUCT
        {
            public String timeUnit;
            public int timeUnitId;
        };

        public struct COUNTRY_STRUCT
        {
            public String country_name;
            public int country_id;
        };

        public struct STATE_STRUCT
        {
            public String state_name;
            public int state_id;
        };

        public struct CITY_STRUCT
        {
            public String city_name;
            public int city_id;
        };

        public struct DISTRICT_STRUCT
        {
            public String district_name;
            public int district_id;
        };

        public struct PRIMARY_FIELD_STRUCT
        {
            public String field_name;
            public int field_id;
        };

        public struct SECONDARY_FIELD_STRUCT
        {
            public String field_name;
            public int field_id;
        };

        public struct SQL_COLUMN_COUNT_STRUCT
        {
            public int sql_int;
            public int sql_tinyint;
            public int sql_smallint;
            public int sql_bigint;
            public int sql_money;
            public int sql_decimal;
            public int sql_datetime;
            public int sql_string;
            public int sql_bit;
        };

        public struct SQL_ROW_STRUCT
        {
            public List<Int32> sql_int;
            public List<Byte> sql_tinyint;
            public List<Int16> sql_smallint;
            public List<Int64> sql_bigint;
            public List<Decimal> sql_money;
            public List<Decimal> sql_decimal;
            public List<DateTime> sql_datetime;
            public List<String> sql_string;
            public List<Boolean> sql_bit;
        }
    }
}
        