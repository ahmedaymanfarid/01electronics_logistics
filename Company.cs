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
    public class Company
    {
        //SQL QUERY
        protected String sqlQuery;

        //SQL OBJECTS
        protected SQLServer sqlDatabase;

        //COMPANY IDENTIFIERS
        private int ownerUserId;

        private int companySerial;
        private int addressSerial;

        //COMPANY BASIC INFO
        private String ownerUser;

        private String companyName;

        private String primaryField;
        private String secondaryField;

        private int primaryFieldId;
        private int secondaryFieldId;

        private bool addressKnown;

        //COMPANY ADDITIONAL INFO
        private List<String> companyPhones;
        private List<String> companyFaxes;

        //COMPANY ADDRESS
        private String country;
        private String state;
        private String city;
        private String district;

        private int address;

        private int countryId;
        private int stateId;
        private int cityId;
        private int districtId;

        public Company()
        {
            sqlDatabase = new SQLServer();
        }

        public Company(SQLServer mSqlDatabase)
        {
            sqlDatabase = mSqlDatabase;
        }

        public void SetDatabase(SQLServer mSqlDatabase)
        {
            sqlDatabase = mSqlDatabase;
        }


        public bool InitializeCompanyInfo(int mCompanySerial)
        {
            companySerial = mCompanySerial;

            String sqlQueryPart1 = @"select generic_work_fields.id, 

                                    specific_work_fields.id, 
									employees_info.employee_id, 

									company_name.company_name, 
									generic_work_fields.generic_work_field, 
									specific_work_fields.specific_work_field, 
									employees_info.name 

							from erp_system.dbo.company_name 
							inner join erp_system.dbo.employees_info 
							on company_name.added_by = employees_info.employee_id 
							inner join erp_system.dbo.company_field_of_work 
							on company_name.company_serial = company_field_of_work.company_serial 
							inner join erp_system.dbo.specific_work_fields 
							on company_field_of_work.work_field = specific_work_fields.id 
							inner join erp_system.dbo.generic_work_fields 
							on specific_work_fields.generic_work_field = generic_work_fields.id 
							where company_name.company_serial = ";

            String sqlQueryPart2 = ";";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += companySerial;
            sqlQuery += sqlQueryPart2;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 3;
            queryColumns.sql_string = 4;

            if (!sqlDatabase.GetRows(sqlQuery, queryColumns, BASIC_MACROS.SEVERITY_HIGH))
                return false;

            primaryFieldId = sqlDatabase.rows[0].sql_int[0];
            secondaryFieldId = sqlDatabase.rows[1].sql_int[0];
            ownerUserId = sqlDatabase.rows[2].sql_int[0];

            companyName = sqlDatabase.rows[0].sql_string[0];
            primaryField = sqlDatabase.rows[1].sql_string[0];
            secondaryField = sqlDatabase.rows[2].sql_string[0];
            ownerUser = sqlDatabase.rows[3].sql_string[0];

            addressKnown = false;

            return true;
        }

        public bool InitializeBranchInfo(int mAddressSerial)
        {
            addressSerial = mAddressSerial;

            String sqlQueryPart1 = @"select company_address.company_serial, 
        
                                    company_address.address, 
									generic_work_fields.id, 
									specific_work_fields.id, 
									employees_info.employee_id, 

									company_name.company_name, 
									generic_work_fields.generic_work_field, 
									specific_work_fields.specific_work_field, 
									employees_info.name 

							from erp_system.dbo.company_address 
							inner join erp_system.dbo.company_name 
							on company_address.company_serial = company_name.company_serial 
							inner join erp_system.dbo.employees_info 
							on company_name.added_by = employees_info.employee_id 
							inner join erp_system.dbo.company_field_of_work 
							on company_address.company_serial = company_field_of_work.company_serial 
							inner join erp_system.dbo.specific_work_fields 
							on company_field_of_work.work_field = specific_work_fields.id 
							inner join erp_system.dbo.generic_work_fields 
							on specific_work_fields.generic_work_field = generic_work_fields.id 
							where company_address.address_serial = ";

            String sqlQueryPart2 = ";";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += addressSerial;
            sqlQuery += sqlQueryPart2;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 5;
            queryColumns.sql_string = 4;

            if (!sqlDatabase.GetRows(sqlQuery, queryColumns, BASIC_MACROS.SEVERITY_LOW))
                return false;

            companySerial = sqlDatabase.rows[0].sql_int[0];
            address = sqlDatabase.rows[0].sql_int[1];
            primaryFieldId = sqlDatabase.rows[0].sql_int[2];
            secondaryFieldId = sqlDatabase.rows[0].sql_int[3];
            ownerUserId = sqlDatabase.rows[0].sql_int[4];

            companyName = sqlDatabase.rows[0].sql_string[0];
            primaryField = sqlDatabase.rows[0].sql_string[1];
            secondaryField = sqlDatabase.rows[0].sql_string[2];
            ownerUser = sqlDatabase.rows[0].sql_string[3];

            SetCompanyAddressIDs();

            if (!GetCompanyAddress() && !QueryCompanyPhones() && !QueryCompanyFaxes())
                return false;

            addressKnown = true;

            return true;
        }

        //////////////////////////////////////////////////////////////////////
        //SET FUNCTIONS
        //////////////////////////////////////////////////////////////////////

        public void SetCompanyName(String mCompanyName)
        {
            companyName = mCompanyName;
        }

        public void SetCompanySerial(int mCompanySerial)
        {
            companySerial = mCompanySerial;
        }
        public void SetAddressSerial(int mAddressSerial)
        {
            addressSerial = mAddressSerial;
        }

        public void SetOwnerUser(int mOwnerUserId, String mOwnerUser)
        {
            ownerUserId = mOwnerUserId;
            ownerUser = mOwnerUser;
        }
        public void SetCompanyCountry(int mCountryId, String mCountry)
        {
            countryId = mCountryId;
            country = mCountry;
        }
        public void SetCompanyState(int mStateId, String mState)
        {
            stateId = mStateId;
            state = mState;
        }
        public void SetCompanyCity(int mCityId, String mCity)
        {
            cityId = mCityId;
            city = mCity;
        }
        public void SetCompanyDistrict(int mDistrictId, String mDistrict)
        {
            districtId = mDistrictId;
            district = mDistrict;
        }

        public void SetCompanyPrimaryField(int mPrimaryFieldId, String mPrimaryField)
        {
            primaryFieldId = mPrimaryFieldId;
            primaryField = mPrimaryField;
        }
        public void SetCompanySecondaryField(int mSecondaryFieldId, String mSecondaryField)
        {
            secondaryFieldId = mSecondaryFieldId;
            secondaryField = mSecondaryField;
        }

        public void AddCompanyPhone(String mPhone)
        {
            companyPhones.Add(mPhone);
        }
        public void AddCompanyFax(String mFax)
        {
            companyFaxes.Add(mFax);
        }

        public void RemoveCompanyTelephone(int telephoneId)
        {
            List<String> tempPhones = new List<String>();

            for (int i = 0; i < companyPhones.Count; i++)
            {
                if (i != telephoneId - 1)
                    tempPhones.Add(companyPhones[i]);
            }

            companyPhones = tempPhones;
        }
        public void RemoveCompanyFax(int faxId)
        {
            List<String> tempFaxes = new List<String>();

            for (int i = 0; i < companyFaxes.Count; i++)
            {
                if (i != faxId - 1)
                    tempFaxes.Add(companyFaxes[i]);
            }

            companyFaxes = tempFaxes;
        }

        //////////////////////////////////////////////////////////////////////
        //RETURN FUNCTIONS
        //////////////////////////////////////////////////////////////////////

        public bool isAddressKnown()
        {
            return addressKnown;
        }

        public int GetOwnerUserId()
        {
            return ownerUserId;
        }

        public int GetCompanySerial()
        {
            return companySerial;
        }
        public int GetAddressSerial()
        {
            return addressSerial;
        }

        public int GetCompanyCountryId()
        {
            return countryId;
        }
        public int GetCompanyStateId()
        {
            return stateId;

        }
        public int GetCompanyCityId()
        {
            return cityId;

        }
        public int GetCompanyDistrictId()
        {
            return districtId;

        }

        public int GetCompanyPrimaryFieldId()
        {
            return primaryFieldId;

        }
        public int GetCompanySecondaryFieldId()
        {
            return secondaryFieldId;

        }

        public int GetNumberOfSavedCompanyPhones()
        {
            return (int) companyPhones.Count;
        }
        public int GetNumberOfSavedCompanyFaxes()
        {
            return (int) companyFaxes.Count;
        }

        public String GetOwnerUser()
        {
            return ownerUser;
        }

        public String GetCompanyName()
        {
            return companyName;
        }

        public String GetCompanyCountry()
        {
            return country;
        }
        public String GetCompanyState()
        {
            return state;

        }
        public String GetCompanyCity()
        {
            return city;
        }
        public String GetCompanyDistrict()
        {
            return district;
        }

        public String GetCompanyPrimaryField()
        {
            return primaryField;
        }
        public String GetCompanySecondaryField()
        {
            return secondaryField;
        }

        public List<String> GetCompanyPhones()
        {
            return companyPhones;
        }
        public List<String> GetCompanyFaxes()
        {
            return companyFaxes;
        }

        //////////////////////////////////////////////////////////////////////
        //GET ADDITIONAL INFO FUNCTIONS
        //////////////////////////////////////////////////////////////////////

        public bool GetNewCompanySerial()
        {
            String sqlQueryPart1 = @"select max(company_serial) from erp_system.dbo.company_name;";

            sqlQuery = String.Empty; ; ;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;

            if (!sqlDatabase.GetRows(sqlQuery, queryColumns, BASIC_MACROS.SEVERITY_HIGH))
                return false;

            SetCompanySerial(sqlDatabase.rows[0].sql_int[0] + 1);

            return true;
        }
        public bool GetNewAddressSerial()
        {
            String sqlQueryPart1 = @"select max(address_serial) from erp_system.dbo.company_address;";

            sqlQuery = String.Empty; ;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;

            if (!sqlDatabase.GetRows(sqlQuery, queryColumns, BASIC_MACROS.SEVERITY_HIGH))
                return false;

            SetAddressSerial(sqlDatabase.rows[0].sql_int[0] + 1);

            return true;
        }

        //////////////////////////////////////////////////////////////////////
        //QUERY FUNCTIONS
        //////////////////////////////////////////////////////////////////////

        public bool GetCompanyAddress()
        {
            String sqlQueryPart1 = @"with get_countries_and_states as(select countries.id as country_id, states_governorates.id as state_id, countries.country, states_governorates.state_governorate from erp_system.dbo.states_governorates inner join erp_system.dbo.countries on states_governorates.country = countries.id), get_cities_and_districts as(select cities.state_governorate as state_id, cities.id as city_id, districts.id as district_id, cities.city, districts.district from erp_system.dbo.cities inner join erp_system.dbo.districts on cities.id = districts.city) select get_cities_and_districts.district, get_cities_and_districts.city, get_countries_and_states.state_governorate, get_countries_and_states.country from get_cities_and_districts inner join get_countries_and_states on get_cities_and_districts.state_id = get_countries_and_states.state_id inner join erp_system.dbo.company_address on get_cities_and_districts.district_id = company_address.address where company_address.company_serial = ";
            String sqlQueryPart2 = @" and get_cities_and_districts.district_id = ";
            String sqlQueryPart3 = @";";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += companySerial;
            sqlQuery += sqlQueryPart2;
            sqlQuery += districtId;
            sqlQuery += sqlQueryPart3;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_string = 4;

            if (!sqlDatabase.GetRows(sqlQuery, queryColumns, BASIC_MACROS.SEVERITY_HIGH))
                return false;

            country = sqlDatabase.rows[0].sql_string[3];
            state = sqlDatabase.rows[0].sql_string[2];
            city = sqlDatabase.rows[0].sql_string[1];
            district = sqlDatabase.rows[0].sql_string[0];

            return true;
        }

        public bool QueryCompanyPhones()
        {
            String sqlQueryPart1 = @"select company_telephone.telephone_number from erp_system.dbo.company_telephone where company_telephone.branch_serial = ";
            String sqlQueryPart2 = @";";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += addressSerial;
            sqlQuery += sqlQueryPart2;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_string = 1;

            if (!sqlDatabase.GetRows(sqlQuery, queryColumns, BASIC_MACROS.SEVERITY_LOW))
                return false;

            for (int i = 0; i < sqlDatabase.rows.Count; i++)
                companyPhones.Add(sqlDatabase.rows[0].sql_string[i]);

            return true;
        }
        public bool QueryCompanyFaxes()
        {
            String sqlQueryPart1 = "select company_fax.fax from erp_system.dbo.company_fax where company_fax.branch_serial = ";
            String sqlQueryPart2 = ";";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += addressSerial;
            sqlQuery += sqlQueryPart2;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_string = 1;

            if (!sqlDatabase.GetRows(sqlQuery, queryColumns, BASIC_MACROS.SEVERITY_LOW))
                return false;

            for (int i = 0; i < sqlDatabase.rows.Count; i++)
                companyFaxes.Add(sqlDatabase.rows[0].sql_string[i]);


            return true;
        }

        public void SetCompanyAddressIDs()
        {
            countryId = address / (BASIC_MACROS.MAXIMUM_STATES_NO * BASIC_MACROS.MAXIMUM_CITIES_NO * BASIC_MACROS.MAXIMUM_DISTRICTS_NO);
            stateId = address / (BASIC_MACROS.MAXIMUM_CITIES_NO * BASIC_MACROS.MAXIMUM_DISTRICTS_NO);
            cityId = address / (BASIC_MACROS.MAXIMUM_DISTRICTS_NO);
            districtId = address;
        }

    }

}