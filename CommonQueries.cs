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
    public class CommonQueries
    {
        private String sqlQuery;
        private SQLServer commonQueriesSqlObject;

        public CommonQueries()
        {
            commonQueriesSqlObject = new SQLServer();
        }

        //////////////////////////////////////////////////////////////////////
        //GET COMPANY INFO FUNCTIONS
        //////////////////////////////////////////////////////////////////////

        public bool GetCompaniesSerialsAndNames(ref List<COMPANY_ORGANISATION_MACROS.COMPANY_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = "select company_serial, company_name from erp_system.dbo.company_name order by company_name;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                COMPANY_ORGANISATION_MACROS.COMPANY_STRUCT tempItem;

                tempItem.company_serial = commonQueriesSqlObject.rows[i].sql_int[0];
                tempItem.company_name = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(tempItem);
            }

            return true;
        }
        public bool GetCompanyAddresses(int mCompanySerial, ref List<COMPANY_ORGANISATION_MACROS.BRANCH_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = @"with get_countries_and_states as(	select countries.id as country_id, 
                                                                        states_governorates.id as state_id, 
																	countries.country, 
																	states_governorates.state_governorate 
																from erp_system.dbo.states_governorates 
																inner join erp_system.dbo.countries 
																on states_governorates.country = countries.id 
															), 
							get_cities_and_districts as (select cities.state_governorate as state_id, 
																	cities.id as city_id, 
																	districts.id as district_id, 
																	cities.city, 
																	districts.district 
																from erp_system.dbo.cities 
																inner join erp_system.dbo.districts 
																on cities.id = districts.city 
															) 
							select company_address.address_serial, 
							company_address.address, 
							get_cities_and_districts.district, 
							get_cities_and_districts.city, 
							get_countries_and_states.state_governorate, 
							get_countries_and_states.country 
							from get_cities_and_districts 
							inner join get_countries_and_states 
							on get_cities_and_districts.state_id = get_countries_and_states.state_id 
							inner join erp_system.dbo.company_address 
							on get_cities_and_districts.district_id = company_address.address 
							where company_address.company_serial = ";

            String sqlQueryPart2 = " order by country, state_governorate, city, district;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += mCompanySerial.ToString();
            sqlQuery += sqlQueryPart2;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 2;
            queryColumns.sql_string = 4;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                COMPANY_ORGANISATION_MACROS.BRANCH_STRUCT tempItem;

                tempItem.address_serial = commonQueriesSqlObject.rows[i].sql_int[0];
                tempItem.address = commonQueriesSqlObject.rows[i].sql_int[1];

                tempItem.district = commonQueriesSqlObject.rows[i].sql_string[0];
                tempItem.city = commonQueriesSqlObject.rows[i].sql_string[1];
                tempItem.state_governorate = commonQueriesSqlObject.rows[i].sql_string[2];
                tempItem.country = commonQueriesSqlObject.rows[i].sql_string[3];

                returnVector.Add(tempItem);
            }

            return true;
        }

        public bool GetExistingCompanyName(String companyDomain, ref String returnVector)
        {
            returnVector = String.Empty;

            String sqlQueryPart3 = "select company_name.company_name from erp_system.dbo.company_name where company_name.domain_name = '";
            String sqlQueryPart4 = "';";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart3;
            sqlQuery += companyDomain;
            sqlQuery += sqlQueryPart4;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 0;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            returnVector = commonQueriesSqlObject.rows[0].sql_string[0];

            return true;
        }

        //////////////////////////////////////////////////////////////////////
        //GET BASIC INFO FUNCTIONS
        //////////////////////////////////////////////////////////////////////

        public bool GetDepartmentsType(ref List<COMPANY_ORGANISATION_MACROS.DEPARTMENT_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = "select departments_type.id, departments_type.department from erp_system.dbo.departments_type order by departments_type.department;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                COMPANY_ORGANISATION_MACROS.DEPARTMENT_STRUCT tempItem;

                tempItem.department_id = commonQueriesSqlObject.rows[i].sql_int[0];
                tempItem.department_name = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(tempItem);
            }

            return true;
        }
        public bool GetTeamsType(ref List<COMPANY_ORGANISATION_MACROS.TEAM_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = "select teams_type.id, teams_type.team from erp_system.dbo.teams_type order by teams_type.id;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                COMPANY_ORGANISATION_MACROS.TEAM_STRUCT tempItem;

                tempItem.team_id = commonQueriesSqlObject.rows[i].sql_int[0];
                tempItem.team_name = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(tempItem);
            }

            return true;
        }
        public bool GetPositionTypes(ref List<COMPANY_ORGANISATION_MACROS.EMPLOYEE_POSITION_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = "select positions_type.id, positions_type.position from erp_system.dbo.positions_type order by positions_type.id;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                COMPANY_ORGANISATION_MACROS.EMPLOYEE_POSITION_STRUCT tempItem;

                tempItem.position_id = commonQueriesSqlObject.rows[i].sql_int[0];
                tempItem.position_name = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(tempItem);
            }

            return true;
        }

        public bool GetDepartmentTeams(int departmentId, ref List<COMPANY_ORGANISATION_MACROS.TEAM_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = @"select teams_type.id, teams_type.team from erp_system.dbo.teams_type 
                                    where(teams_type.id > ";
            String sqlQueryPart2 = " and teams_type.id <= ";
            String sqlQueryPart3 = @") 
                                    or teams_type.id = ";
            String sqlQueryPart4 = "order by teams_type.id;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += departmentId * COMPANY_ORGANISATION_MACROS.MAX_NUMBER_OF_TEAMS_PER_DEP;
            sqlQuery += sqlQueryPart2;
            sqlQuery += departmentId * COMPANY_ORGANISATION_MACROS.MAX_NUMBER_OF_TEAMS_PER_DEP + COMPANY_ORGANISATION_MACROS.MAX_NUMBER_OF_TEAMS_PER_DEP - 1;
            sqlQuery += sqlQueryPart3;
            sqlQuery += departmentId;
            sqlQuery += sqlQueryPart4;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                COMPANY_ORGANISATION_MACROS.TEAM_STRUCT tempItem;

                tempItem.team_id = commonQueriesSqlObject.rows[i].sql_int[0];
                tempItem.team_name = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(tempItem);
            }

            return true;
        }

        public bool GetPrimaryWorkFields(ref List<BASIC_STRUCTS.PRIMARY_FIELD_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = "select generic_work_fields.id, generic_work_fields.generic_work_field from erp_system.dbo.generic_work_fields order by generic_work_fields.id;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                BASIC_STRUCTS.PRIMARY_FIELD_STRUCT tempItem;

                tempItem.field_id = commonQueriesSqlObject.rows[i].sql_int[0];
                tempItem.field_name = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(tempItem);
            }

            return true;

        }
        public bool GetSecondaryWorkFields(int primaryFieldId, List<BASIC_STRUCTS.SECONDARY_FIELD_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = "select specific_work_fields.id, specific_work_fields.specific_work_field from erp_system.dbo.specific_work_fields where specific_work_fields.generic_work_field = ";
            String sqlQueryPart2 = " order by specific_work_fields.id;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += primaryFieldId;
            sqlQuery += sqlQueryPart2;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                BASIC_STRUCTS.SECONDARY_FIELD_STRUCT tempItem;

                tempItem.field_id = commonQueriesSqlObject.rows[i].sql_int[0];
                tempItem.field_name = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(tempItem);
            }

            return true;
        }

        public bool GetAllCountries(ref List<BASIC_STRUCTS.COUNTRY_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = "select countries.id, countries.country from erp_system.dbo.countries order by countries.country;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                BASIC_STRUCTS.COUNTRY_STRUCT tempItem;

                tempItem.country_id = commonQueriesSqlObject.rows[i].sql_int[0];
                tempItem.country_name = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(tempItem);
            }

            return true;
        }
        public bool GetAllCountryStates(int countryId, ref List<BASIC_STRUCTS.STATE_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = "select states_governorates.id, states_governorates.state_governorate from erp_system.dbo.states_governorates where states_governorates.country = ";
            String sqlQueryPart2 = "order by states_governorates.state_governorate;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += countryId;
            sqlQuery += sqlQueryPart2;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                BASIC_STRUCTS.STATE_STRUCT tempItem;

                tempItem.state_id = commonQueriesSqlObject.rows[i].sql_int[0];
                tempItem.state_name = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(tempItem);
            }

            return true;
        }
        public bool GetAllStateCities(int stateId, ref List<BASIC_STRUCTS.CITY_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = "select cities.id, cities.city from erp_system.dbo.cities where cities.state_governorate = ";
            String sqlQueryPart2 = "order by cities.city;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += stateId;
            sqlQuery += sqlQueryPart2;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                BASIC_STRUCTS.CITY_STRUCT tempItem;

                tempItem.city_id = commonQueriesSqlObject.rows[i].sql_int[0];
                tempItem.city_name = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(tempItem);
            }

            return true;
        }
        public bool GetAllCityDistricts(int cityId, ref List<BASIC_STRUCTS.DISTRICT_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = "select districts.id, districts.district from erp_system.dbo.districts where districts.city = ";
            String sqlQueryPart2 = "order by districts.district;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += cityId;
            sqlQuery += sqlQueryPart2;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                BASIC_STRUCTS.DISTRICT_STRUCT tempItem;

                tempItem.district_id = commonQueriesSqlObject.rows[i].sql_int[0];
                tempItem.district_name = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(tempItem);
            }

            return true;
        }

        public bool GetVisitPurposes(ref List<COMPANY_WORK_MACROS.VISIT_PURPOSE_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = "select visits_purpose.id, visits_purpose.visit_purpose from erp_system.dbo.visits_purpose where visits_purpose.id > 0 order by id;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                COMPANY_WORK_MACROS.VISIT_PURPOSE_STRUCT purposeItem;

                purposeItem.purpose_id = commonQueriesSqlObject.rows[i].sql_int[0];
                purposeItem.purpose_name = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(purposeItem);
            }

            COMPANY_WORK_MACROS.VISIT_PURPOSE_STRUCT tempItem;

            tempItem.purpose_id = 0;
            tempItem.purpose_name = "Other";

            returnVector.Add(tempItem);

            return true;
        }
        public bool GetVisitResults(ref List<COMPANY_WORK_MACROS.VISIT_RESULT_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = "select visits_result.id, visits_result.visit_result from erp_system.dbo.visits_result where visits_result.id > 0 order by visits_result.id;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                COMPANY_WORK_MACROS.VISIT_RESULT_STRUCT resultItem;

                resultItem.result_id = commonQueriesSqlObject.rows[i].sql_int[0];
                resultItem.result_name = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(resultItem);
            }

            COMPANY_WORK_MACROS.VISIT_RESULT_STRUCT tempItem;

            tempItem.result_id = 0;
            tempItem.result_name = "Other";

            returnVector.Add(tempItem);

            return true;
        }

        public bool GetCallPurposes(ref List<COMPANY_WORK_MACROS.CALL_PURPOSE_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = "select calls_purpose.id, calls_purpose.call_purpose from erp_system.dbo.calls_purpose where calls_purpose.id > 0 order by id;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                COMPANY_WORK_MACROS.CALL_PURPOSE_STRUCT purposeItem;

                purposeItem.purpose_id = commonQueriesSqlObject.rows[i].sql_int[0];
                purposeItem.purpose_name = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(purposeItem);
            }

            COMPANY_WORK_MACROS.CALL_PURPOSE_STRUCT tempItem;

            tempItem.purpose_id = 0;
            tempItem.purpose_name = "Other";

            returnVector.Add(tempItem);

            return true;
        }
        public bool GetCallResults(ref List<COMPANY_WORK_MACROS.CALL_RESULT_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = "select calls_result.id, calls_result.call_result from erp_system.dbo.calls_result where calls_result.id > 0 order by calls_result.id;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                COMPANY_WORK_MACROS.CALL_RESULT_STRUCT resultItem;

                resultItem.result_id = commonQueriesSqlObject.rows[i].sql_int[0];
                resultItem.result_name = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(resultItem);
            }

            COMPANY_WORK_MACROS.CALL_RESULT_STRUCT tempItem;

            tempItem.result_id = 0;
            tempItem.result_name = "Other";

            returnVector.Add(tempItem);

            return true;
        }
        public bool GetRFQFailureReasons(ref List<COMPANY_WORK_MACROS.FAILURE_REASON_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = "select rfq_failure_reasons.id, rfq_failure_reasons.failure_reason from erp_system.dbo.rfq_failure_reasons order by rfq_failure_reasons.id;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                COMPANY_WORK_MACROS.FAILURE_REASON_STRUCT reasonItem;

                reasonItem.reason_id = commonQueriesSqlObject.rows[i].sql_int[0];
                reasonItem.reason_name = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(reasonItem);
            }

            return true;
        }
        public bool GetOfferFailureReasons(ref List<COMPANY_WORK_MACROS.FAILURE_REASON_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = "select offer_failure_reasons.id, offer_failure_reasons.failure_reason from erp_system.dbo.offer_failure_reasons order by offer_failure_reasons.id;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                COMPANY_WORK_MACROS.FAILURE_REASON_STRUCT reasonItem;

                reasonItem.reason_id = commonQueriesSqlObject.rows[i].sql_int[0];
                reasonItem.reason_name = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(reasonItem);
            }

            return true;
        }

        public bool GetContractTypes(ref List<BASIC_STRUCTS.CONTRACT_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = "select contracts_type.id, contracts_type.contract_type from erp_system.dbo.contracts_type order by contracts_type.id;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                BASIC_STRUCTS.CONTRACT_STRUCT tempItem;

                tempItem.contractId = commonQueriesSqlObject.rows[i].sql_int[0];
                tempItem.contractName = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(tempItem);
            }

            return true;
        }
        public bool GetDeliveryPoints(ref List<BASIC_STRUCTS.DELIVERY_POINT_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = "select delivery_points.id, delivery_points.delivery_point from erp_system.dbo.delivery_points order by delivery_points.delivery_point;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                BASIC_STRUCTS.DELIVERY_POINT_STRUCT tempItem;

                tempItem.pointId = commonQueriesSqlObject.rows[i].sql_int[0];
                tempItem.pointName = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(tempItem);
            }

            return true;
        }

        public bool GetCurrencyTypes(ref List<BASIC_STRUCTS.CURRENCY_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = "select currencies_type.id, currencies_type.currency from erp_system.dbo.currencies_type order by currencies_type.currency;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                BASIC_STRUCTS.CURRENCY_STRUCT tempItem;

                tempItem.currencyId = commonQueriesSqlObject.rows[i].sql_int[0];
                tempItem.currencyName = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(tempItem);
            }

            return true;
        }
        public bool GetTimeUnits(ref List<BASIC_STRUCTS.TIMEUNIT_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = "select time_units.id, time_units.time_unit from erp_system.dbo.time_units order by time_units.id;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                BASIC_STRUCTS.TIMEUNIT_STRUCT tempItem;

                tempItem.timeUnitId = commonQueriesSqlObject.rows[i].sql_int[0];
                tempItem.timeUnit = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(tempItem);
            }

            return true;
        }

        //////////////////////////////////////////////////////////////////////
        //GET EMPLOYEE INFO FUNCTIONS
        //////////////////////////////////////////////////////////////////////

        public bool GetCompanyEmployees(ref List<COMPANY_ORGANISATION_MACROS.EMPLOYEE_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = @"select employees_info.employee_id, 
                                        teams_type.id,
                                        departments_type.id, 
                                        positions_type.id,
										employees_info.name,
                                        teams_type.team,
                                        departments_type.department,
                                        positions_type.position

								from erp_system.dbo.employees_info

                                inner join erp_system.dbo.teams_type
                                on employees_info.employee_team = teams_type.id

                                inner join erp_system.dbo.departments_type
                                on employees_info.employee_department = departments_type.id

                                inner join erp_system.dbo.positions_type
                                on employees_info.employee_position = positions_type.id

                                order by employees_info.name;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 4;
            queryColumns.sql_string = 4;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                COMPANY_ORGANISATION_MACROS.EMPLOYEE_STRUCT tempItem = new COMPANY_ORGANISATION_MACROS.EMPLOYEE_STRUCT();
                tempItem.team = new COMPANY_ORGANISATION_MACROS.TEAM_STRUCT();
                tempItem.department = new COMPANY_ORGANISATION_MACROS.DEPARTMENT_STRUCT();

                tempItem.employee_id = commonQueriesSqlObject.rows[i].sql_int[0];
                tempItem.team.team_id = commonQueriesSqlObject.rows[i].sql_int[1];
                tempItem.department.department_id = commonQueriesSqlObject.rows[i].sql_int[2];
                tempItem.position.position_id = commonQueriesSqlObject.rows[i].sql_int[3];

                tempItem.employee_name = commonQueriesSqlObject.rows[i].sql_string[0];
                tempItem.team.team_name = commonQueriesSqlObject.rows[i].sql_string[1];
                tempItem.department.department_name = commonQueriesSqlObject.rows[i].sql_string[2];
                tempItem.position.position_name = commonQueriesSqlObject.rows[i].sql_string[3];

                returnVector.Add(tempItem);
            }

            return true;
        }
        public bool GetDepartmentEmployees(int departmentId, ref List<COMPANY_ORGANISATION_MACROS.EMPLOYEE_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = @"select employees_info.employee_id, 
                                        employees_info.employee_team, 
										employees_info.name 
								from erp_system.dbo.employees_info 
								where employees_info.employee_department = ";
            String sqlQueryPart2 = " order by employees_info.name; ";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += departmentId;
            sqlQuery += sqlQueryPart2;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 2;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                COMPANY_ORGANISATION_MACROS.EMPLOYEE_STRUCT tempItem = new COMPANY_ORGANISATION_MACROS.EMPLOYEE_STRUCT();

                tempItem.team = new COMPANY_ORGANISATION_MACROS.TEAM_STRUCT();
                tempItem.department = new COMPANY_ORGANISATION_MACROS.DEPARTMENT_STRUCT();
                tempItem.position = new COMPANY_ORGANISATION_MACROS.EMPLOYEE_POSITION_STRUCT();

                tempItem.employee_id = commonQueriesSqlObject.rows[i].sql_int[0];
                tempItem.team.team_id = commonQueriesSqlObject.rows[i].sql_int[1];

                tempItem.employee_name = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(tempItem);
            }

            return true;
        }
        public bool GetTeamEmployees(int teamId, ref List<COMPANY_ORGANISATION_MACROS.EMPLOYEE_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = @"select employees_info.employee_id, 
                                        employees_info.employee_team, 
										employees_info.name 
								from erp_system.dbo.employees_info 
								where employees_info.employee_team = ";
            String sqlQueryPart2 = " order by employees_info.name; ";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += teamId;
            sqlQuery += sqlQueryPart2;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 2;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                COMPANY_ORGANISATION_MACROS.EMPLOYEE_STRUCT tempItem = new COMPANY_ORGANISATION_MACROS.EMPLOYEE_STRUCT();

                tempItem.team = new COMPANY_ORGANISATION_MACROS.TEAM_STRUCT();
                tempItem.department = new COMPANY_ORGANISATION_MACROS.DEPARTMENT_STRUCT();
                tempItem.position = new COMPANY_ORGANISATION_MACROS.EMPLOYEE_POSITION_STRUCT();

                tempItem.employee_id = commonQueriesSqlObject.rows[i].sql_int[0];
                tempItem.team.team_id = commonQueriesSqlObject.rows[i].sql_int[1];
                tempItem.employee_name = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(tempItem);
            }

            return true;
        }

        public bool GetEmployeesPayrollInfo(ref List<COMPANY_ORGANISATION_MACROS.PAYROLL_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = @"select employees_info.employee_id,
                               		employees_payroll_info.bank_id,
                               		employees_payroll_info.payroll_id,
                                    employees_payroll_info.branch_code,
                               		employees_payroll_info.account_id,
                                    employees_payroll_info.iban_id,
                               		employees_info.name,
                               		banks_names.bank_name
                                from erp_system.dbo.employees_payroll_info
                               inner join erp_system.dbo.employees_info
                               on employees_payroll_info.employee_id = employees_info.employee_id
                               inner join erp_system.dbo.banks_names
                               on employees_payroll_info.bank_id = banks_names.id
                               order by employees_info.name, bank_id;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 3;
            queryColumns.sql_smallint = 1;
            queryColumns.sql_bigint = 2;
            queryColumns.sql_string = 2;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                COMPANY_ORGANISATION_MACROS.PAYROLL_STRUCT tempItem = new COMPANY_ORGANISATION_MACROS.PAYROLL_STRUCT();
                tempItem.banksList = new List<COMPANY_ORGANISATION_MACROS.BANK_STRUCT>();

                tempItem.employee_id = commonQueriesSqlObject.rows[i].sql_int[0];
                tempItem.employee_name = commonQueriesSqlObject.rows[i].sql_string[0];

                if (i > 0 && returnVector.Last().employee_id == tempItem.employee_id)
                {
                    COMPANY_ORGANISATION_MACROS.BANK_STRUCT tempBankItem = new COMPANY_ORGANISATION_MACROS.BANK_STRUCT();

                    tempBankItem.bank_id = commonQueriesSqlObject.rows[i].sql_int[1];
                    tempBankItem.branch_id = commonQueriesSqlObject.rows[i].sql_smallint[0];
                    tempBankItem.payroll_id = commonQueriesSqlObject.rows[i].sql_int[2];

                    tempBankItem.bank_name = commonQueriesSqlObject.rows[i].sql_string[1];

                    tempBankItem.account_id = (ulong)commonQueriesSqlObject.rows[i].sql_bigint[0];
                    tempBankItem.iban_id = (ulong)commonQueriesSqlObject.rows[i].sql_bigint[1];

                    returnVector.Last().banksList.Add(tempBankItem);
                }
                else
                {
                    COMPANY_ORGANISATION_MACROS.BANK_STRUCT tempBankItem = new COMPANY_ORGANISATION_MACROS.BANK_STRUCT();

                    tempBankItem.bank_id = commonQueriesSqlObject.rows[i].sql_int[1];
                    tempBankItem.branch_id = commonQueriesSqlObject.rows[i].sql_smallint[0];
                    tempBankItem.payroll_id = commonQueriesSqlObject.rows[i].sql_int[2];

                    tempBankItem.bank_name = commonQueriesSqlObject.rows[i].sql_string[1];

                    tempBankItem.account_id = (ulong)commonQueriesSqlObject.rows[i].sql_bigint[0];
                    tempBankItem.iban_id = (ulong)commonQueriesSqlObject.rows[i].sql_bigint[1];

                    tempItem.banksList.Add(tempBankItem);
                    returnVector.Add(tempItem);
                }
            }

            return true;
        }

        public bool GetEmployeesSalaries(ref List<COMPANY_ORGANISATION_MACROS.SALARY_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = @"select employees_info.employee_id,
                               		employees_salaries.salary,
                                    employees_info.name
                                from erp_system.dbo.employees_salaries
                               inner join erp_system.dbo.employees_info
                               on employees_salaries.id = employees_info.employee_id
                               order by employees_info.name";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;
            queryColumns.sql_money = 1;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                COMPANY_ORGANISATION_MACROS.SALARY_STRUCT tempItem;
                tempItem.employee_id = commonQueriesSqlObject.rows[i].sql_int[0];
                tempItem.salary = commonQueriesSqlObject.rows[i].sql_money[0];
                tempItem.employee_name = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(tempItem);
            }

            return true;
        }
        public bool GetEmployeeEmailCount(String employeeEmail, ref int returnValue)
        {
            returnValue = 0;

            String sqlQueryPart1 = "select count(employees_business_emails.email) from erp_system.dbo.employees_business_emails where employees_business_emails.email = '";
            String sqlQueryPart2 = "';";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += employeeEmail;
            sqlQuery += sqlQueryPart2;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            returnValue = commonQueriesSqlObject.rows[0].sql_int[0];

            return true;
        }
        public bool GetEmployeePasswordCount(String employeeEmail, ref int returnValue)
        {
            returnValue = 0;

            String sqlQueryPart1 = "select count(employees_passwords.id) from erp_system.dbo.employees_passwords inner join erp_system.dbo.employees_business_emails on employees_passwords.id = employees_business_emails.id where employees_business_emails.email = '";
            String sqlQueryPart2 = "';";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += employeeEmail;
            sqlQuery += sqlQueryPart2;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            returnValue = commonQueriesSqlObject.rows[0].sql_int[0];

            return true;
        }

        public bool GetTeamLead(int teamId, ref int returnValue)
        {
            returnValue = 0;

            String sqlQueryPart1 = @"select employees_info.employee_id 
                                    from erp_system.dbo.employees_info 
							        where employees_info.employee_team = ";
            String sqlQueryPart2 = "and employees_info.employee_position = ";
            String sqlQueryPart3 = ";";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += teamId;
            sqlQuery += sqlQueryPart2;
            sqlQuery += COMPANY_ORGANISATION_MACROS.TEAM_LEAD_POSTION;
            sqlQuery += sqlQueryPart3;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            returnValue = commonQueriesSqlObject.rows[0].sql_int[0];

            return true;
        }
        public bool GetEmployeeTeam(int employeeId, ref int returnValue)
        {
            returnValue = 0;

            String sqlQueryPart1 = @"select employees_info.employee_team 
                                    from erp_system.dbo.employees_info 
        							where employees_info.employee_id = ";


            String sqlQueryPart2 = ";";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += employeeId;
            sqlQuery += sqlQueryPart2;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            returnValue = commonQueriesSqlObject.rows[0].sql_int[0];

            return true;
        }

        public bool GetEmployeeInitials(int employeeId, ref String returnValue)
        {
            returnValue = String.Empty;

            String sqlQueryPart1 = "select employees_initials.employee_initial from erp_system.dbo.employees_initials where employees_initials.id = ";
            String sqlQueryPart2 = ";";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += employeeId;
            sqlQuery += sqlQueryPart2;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            returnValue = commonQueriesSqlObject.rows[0].sql_string[0];

            return true;
        }
        public bool GetEmployeePassword(int employeeId, ref String returnValue)
        {
            returnValue = String.Empty;

            String sqlQueryPart1 = "select password from erp_system.dbo.employees_passwords where employees_passwords.id = ";
            String sqlQueryPart2 = ";";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += employeeId;
            sqlQuery += sqlQueryPart2;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            if (commonQueriesSqlObject.rows.Count == 0)
            {
                MessageBox.Show("There is no existing signup for this email account! Please signup first and then try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            returnValue = commonQueriesSqlObject.rows[0].sql_string[0];

            return true;
        }

        //////////////////////////////////////////////////////////////////////
        //GET CONTACT INFO FUNCTIONS
        //////////////////////////////////////////////////////////////////////

        public bool GetCompanyContacts(int salesPersonId, int addressSerial, ref List<COMPANY_ORGANISATION_MACROS.CONTACT_BASIC_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = "select contact_person_info.contact_id, contact_person_info.name from erp_system.dbo.contact_person_info where contact_person_info.sales_person_id = ";
            String sqlQueryPart2 = " and contact_person_info.branch_serial = ";
            String sqlQueryPart3 = @" and contact_person_info.is_valid = 1 
                                    order by contact_person_info.name; ";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += salesPersonId;
            sqlQuery += sqlQueryPart2;
            sqlQuery += addressSerial;
            sqlQuery += sqlQueryPart3;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                COMPANY_ORGANISATION_MACROS.CONTACT_BASIC_STRUCT tempItem;
                tempItem.contact_id = commonQueriesSqlObject.rows[i].sql_int[0];
                tempItem.contact_name = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(tempItem);
            }

            return true;
        }
        public bool GetCompanyContacts(int addressSerial, ref List<COMPANY_ORGANISATION_MACROS.CONTACT_PRO_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = @"select	contact_person_info.contact_id, 
                
                                    departments_type.id as contact_department_id, 

									employees_info.employee_id as sales_person_id, 
									teams_type.id as sales_person_team_id, 

									contact_person_info.name as contact_person_name, 
									departments_type.department as contact_department_name, 

									employees_info.name as sales_person_name, 
									teams_type.team as sales_person_team_name, 

									contact_person_info.gender, 
									contact_person_mobile.mobile, 
									contact_person_info.email, 
									contact_person_personal_emails.personal_email 

							from erp_system.dbo.contact_person_info 

							inner join erp_system.dbo.employees_info 
							on contact_person_info.sales_person_id = employees_info.employee_id 

							inner join erp_system.dbo.departments_type 
							on contact_person_info.department = departments_type.id 

							inner join erp_system.dbo.teams_type 
							on employees_info.employee_team = teams_type.id 

							left join erp_system.dbo.contact_person_mobile 
							on contact_person_info.sales_person_id = contact_person_mobile.sales_person_id 
							and contact_person_info.branch_serial = contact_person_mobile.branch_serial 
							and contact_person_info.contact_id = contact_person_mobile.contact_id 

							left join erp_system.dbo.contact_person_personal_emails 
							on contact_person_info.sales_person_id = contact_person_personal_emails.sales_person_id 
							and contact_person_info.branch_serial = contact_person_personal_emails.branch_serial 
							and contact_person_info.contact_id = contact_person_personal_emails.contact_id 

							inner join erp_system.dbo.company_address 
							on contact_person_info.branch_serial = company_address.address_serial 

							where company_address.address_serial = ";
            String sqlQueryPart2 = @" and contact_person_info.is_valid = 1 
                                        order by contact_person_info.name; ";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += addressSerial.ToString();
            sqlQuery += sqlQueryPart2;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 4;
            queryColumns.sql_string = 8;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                COMPANY_ORGANISATION_MACROS.CONTACT_PRO_STRUCT tempItem;

                tempItem.sales_person.team = new COMPANY_ORGANISATION_MACROS.TEAM_STRUCT();
                tempItem.sales_person = new COMPANY_ORGANISATION_MACROS.EMPLOYEE_STRUCT();
                tempItem.company = new COMPANY_ORGANISATION_MACROS.COMPANY_STRUCT();
                tempItem.branch = new COMPANY_ORGANISATION_MACROS.BRANCH_STRUCT();
                tempItem.contact_department = new COMPANY_ORGANISATION_MACROS.DEPARTMENT_STRUCT();

                tempItem.contact_id = commonQueriesSqlObject.rows[i].sql_int[0];
                tempItem.contact_name = commonQueriesSqlObject.rows[i].sql_string[0];

                tempItem.contact_department.department_id = commonQueriesSqlObject.rows[i].sql_int[1];
                tempItem.contact_department.department_name = commonQueriesSqlObject.rows[i].sql_string[1];

                tempItem.sales_person.employee_id = commonQueriesSqlObject.rows[i].sql_int[2];
                tempItem.sales_person.employee_name = commonQueriesSqlObject.rows[i].sql_string[2];

                tempItem.sales_person.team.team_id = commonQueriesSqlObject.rows[i].sql_int[3];
                tempItem.sales_person.team.team_name = commonQueriesSqlObject.rows[i].sql_string[3];

                tempItem.gender = commonQueriesSqlObject.rows[i].sql_string[4];
                tempItem.mobile = commonQueriesSqlObject.rows[i].sql_string[5];

                tempItem.business_email = commonQueriesSqlObject.rows[i].sql_string[6];
                tempItem.personal_email = commonQueriesSqlObject.rows[i].sql_string[7];

                returnVector.Add(tempItem);
            }

            return true;
        }
        public bool GetEmployeeContacts(int salesPersonId, ref List<COMPANY_ORGANISATION_MACROS.CONTACT_PRO_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = @"select	contact_person_info.contact_id, 
                                    departments_type.id as contact_department_id, 

									employees_info.employee_id as sales_person_id, 
									teams_type.id as sales_person_team_id, 

									company_address.address_serial, 
									company_name.company_serial, 

									contact_person_info.name as contact_person_name, 
									departments_type.department as contact_department_name, 

									employees_info.name as sales_person_name, 
									teams_type.team as sales_person_team_name, 

									company_name.company_name, 

									contact_person_info.gender, 
									contact_person_mobile.mobile, 
									contact_person_info.email, 
									contact_person_personal_emails.personal_email 

							from erp_system.dbo.contact_person_info 

							inner join erp_system.dbo.employees_info 
							on contact_person_info.sales_person_id = employees_info.employee_id 

							inner join erp_system.dbo.departments_type 
							on contact_person_info.department = departments_type.id 

							inner join erp_system.dbo.teams_type 
							on employees_info.employee_team = teams_type.id 

							left join erp_system.dbo.contact_person_mobile 
							on contact_person_info.sales_person_id = contact_person_mobile.sales_person_id 
							and contact_person_info.branch_serial = contact_person_mobile.branch_serial 
							and contact_person_info.contact_id = contact_person_mobile.contact_id 

							left join erp_system.dbo.contact_person_personal_emails 
							on contact_person_info.sales_person_id = contact_person_personal_emails.sales_person_id 
							and contact_person_info.branch_serial = contact_person_personal_emails.branch_serial 
							and contact_person_info.contact_id = contact_person_personal_emails.contact_id 

							inner join erp_system.dbo.company_address 
							on contact_person_info.branch_serial = company_address.address_serial 

							where contact_person_info.sales_person_id = ";


            String sqlQueryPart2 = @" and contact_person_info.is_valid = 1 
                                    order by contact_person_info.name; ";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += salesPersonId;
            sqlQuery += sqlQueryPart2;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 6;
            queryColumns.sql_string = 9;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                COMPANY_ORGANISATION_MACROS.CONTACT_PRO_STRUCT tempItem;

                tempItem.sales_person.team = new COMPANY_ORGANISATION_MACROS.TEAM_STRUCT();
                tempItem.sales_person = new COMPANY_ORGANISATION_MACROS.EMPLOYEE_STRUCT();
                tempItem.company = new COMPANY_ORGANISATION_MACROS.COMPANY_STRUCT();
                tempItem.branch = new COMPANY_ORGANISATION_MACROS.BRANCH_STRUCT();
                tempItem.contact_department = new COMPANY_ORGANISATION_MACROS.DEPARTMENT_STRUCT();

                tempItem.contact_id = commonQueriesSqlObject.rows[i].sql_int[0];
                tempItem.contact_name = commonQueriesSqlObject.rows[i].sql_string[0];

                tempItem.contact_department.department_id = commonQueriesSqlObject.rows[i].sql_int[1];
                tempItem.contact_department.department_name = commonQueriesSqlObject.rows[i].sql_string[1];

                tempItem.sales_person.employee_id = commonQueriesSqlObject.rows[i].sql_int[2];
                tempItem.sales_person.employee_name = commonQueriesSqlObject.rows[i].sql_string[2];

                tempItem.sales_person.team.team_id = commonQueriesSqlObject.rows[i].sql_int[3];
                tempItem.sales_person.team.team_name = commonQueriesSqlObject.rows[i].sql_string[3];

                tempItem.branch.address_serial = commonQueriesSqlObject.rows[i].sql_int[4];
                tempItem.company.company_serial = commonQueriesSqlObject.rows[i].sql_int[5];
                tempItem.company.company_name = commonQueriesSqlObject.rows[i].sql_string[4];

                tempItem.gender = commonQueriesSqlObject.rows[i].sql_string[5];
                tempItem.mobile = commonQueriesSqlObject.rows[i].sql_string[6];

                tempItem.business_email = commonQueriesSqlObject.rows[i].sql_string[7];
                tempItem.personal_email = commonQueriesSqlObject.rows[i].sql_string[8];

                returnVector.Add(tempItem);
            }

            return true;
        }

        public bool GetContactBusinessEmailCount(String contactEmail, ref int returnValue)
        {
            returnValue = 0;

            String sqlQueryPart1 = "select count(contact_person_info.email) from erp_system.dbo.contact_person_info where contact_person_info.email = '";
            String sqlQueryPart2 = "';";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += contactEmail;
            sqlQuery += sqlQueryPart2;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            returnValue = commonQueriesSqlObject.rows[0].sql_int[0];

            return true;
        }
        public bool GetContactBusinessEmailCount(String contactEmail, int salesPersonId, int addressSerial, int contactId, ref int returnValue)
        {
            returnValue = 0;

            String sqlQueryPart1 = "select count(contact_person_info.email) from erp_system.dbo.contact_person_info where contact_person_info.email = '";
            String sqlQueryPart2 = "' and sales_person_id != ";
            String sqlQueryPart3 = " and branch_serial != ";
            String sqlQueryPart4 = " and contact_id != ";
            String sqlQueryPart5 = ";";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += contactEmail;
            sqlQuery += sqlQueryPart2;
            sqlQuery += salesPersonId;
            sqlQuery += sqlQueryPart3;
            sqlQuery += addressSerial;
            sqlQuery += sqlQueryPart4;
            sqlQuery += contactId;
            sqlQuery += sqlQueryPart5;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            returnValue = commonQueriesSqlObject.rows[0].sql_int[0];

            return true;
        }

        public bool GetContactPhoneCount(String contactPhone, ref int returnValue)
        {
            String sqlQueryPart1 = "select count(contact_person_mobile.mobile) from erp_system.dbo.contact_person_mobile where contact_person_mobile.mobile = '";
            String sqlQueryPart2 = "';";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += contactPhone;
            sqlQuery += sqlQueryPart2;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            returnValue = commonQueriesSqlObject.rows[0].sql_int[0];

            return true;
        }
        public bool GetContactPhoneCount(String contactPhone, int salesPersonId, int addressSerial, int contactId, ref int returnValue)
        {
            returnValue = 0;

            String sqlQueryPart1 = "select count(contact_person_mobile.mobile) from erp_system.dbo.contact_person_mobile where contact_person_mobile.mobile = '";
            String sqlQueryPart2 = "' and sales_person_id != ";
            String sqlQueryPart3 = " and branch_serial != ";
            String sqlQueryPart4 = " and contact_id != ";
            String sqlQueryPart5 = ";";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += contactPhone;
            sqlQuery += sqlQueryPart2;
            sqlQuery += salesPersonId;
            sqlQuery += sqlQueryPart3;
            sqlQuery += addressSerial;
            sqlQuery += sqlQueryPart4;
            sqlQuery += contactId;
            sqlQuery += sqlQueryPart5;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            returnValue = commonQueriesSqlObject.rows[0].sql_int[0];

            return true;
        }


        public bool GetContactPersonalEmailCount(String contactEmail, ref int returnValue)
        {
            returnValue = 0;

            String sqlQueryPart1 = "select count(contact_person_personal_emails.personal_email) from erp_system.dbo.contact_person_personal_emails where contact_person_personal_emails.personal_email = '";
            String sqlQueryPart2 = "';";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += contactEmail;
            sqlQuery += sqlQueryPart2;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            returnValue = commonQueriesSqlObject.rows[0].sql_int[0];

            return true;
        }
        public bool GetContactPersonalEmailCount(String contactEmail, int salesPersonId, int addressSerial, int contactId, ref int returnValue)
        {
            returnValue = 0;

            String sqlQueryPart1 = "select count(contact_person_personal_emails.personal_email) from erp_system.dbo.contact_person_personal_emails where contact_person_personal_emails.personal_email = '";
            String sqlQueryPart2 = "' and sales_person_id != ";
            String sqlQueryPart3 = " and branch_serial != ";
            String sqlQueryPart4 = " and contact_id != ";
            String sqlQueryPart5 = ";";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += contactEmail;
            sqlQuery += sqlQueryPart2;
            sqlQuery += salesPersonId;
            sqlQuery += sqlQueryPart3;
            sqlQuery += addressSerial;
            sqlQuery += sqlQueryPart4;
            sqlQuery += contactId;
            sqlQuery += sqlQueryPart5;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            returnValue = commonQueriesSqlObject.rows[0].sql_int[0];

            return true;
        }

        public bool GetCompanyDomainCount(String companyDomain, ref int returnValue)
        {
            returnValue = 0;

            String sqlQueryPart1 = @"select count(company_name.domain_name) 
                                    from erp_system.dbo.company_name where company_name.domain_name like '";
            String sqlQueryPart2 = "';";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += companyDomain;
            sqlQuery += sqlQueryPart2;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            returnValue = commonQueriesSqlObject.rows[0].sql_int[0];

            return true;
        }

        //////////////////////////////////////////////////////////////////////
        //GET DOMAIN INFO FUNCTIONS
        //////////////////////////////////////////////////////////////////////

        public bool GetOriginalTLDs(ref List<String> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = "select * from erp_system.dbo.original_tlds;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            returnVector = commonQueriesSqlObject.rows[0].sql_string;

            return true;
        }

        public bool GetCountryTLD(int countryId, ref String returnValue)
        {
            returnValue = String.Empty;

            String sqlQueryPart1 = "select tld from erp_system.dbo.country_tlds where country = '";
            String sqlQueryPart2 = "';";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += countryId;
            sqlQuery += sqlQueryPart2;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            returnValue = commonQueriesSqlObject.rows[0].sql_string[0];

            return true;
        }

        //////////////////////////////////////////////////////////////////////
        //GET PRODUCTS INFO FUNCTIONS
        //////////////////////////////////////////////////////////////////////

        public bool GetCompanyProducts(ref List<COMPANY_WORK_MACROS.PRODUCT_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = "select products_type.id, products_type.product_name from erp_system.dbo.products_type where products_type.id > 0 order by product_name;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                COMPANY_WORK_MACROS.PRODUCT_STRUCT tempItem;

                tempItem.typeId = commonQueriesSqlObject.rows[i].sql_int[0];
                tempItem.typeName = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(tempItem);
            }

            COMPANY_WORK_MACROS.PRODUCT_STRUCT lastItem;

            lastItem.typeId = 0;
            lastItem.typeName = "Others";

            returnVector.Add(lastItem);

            return true;
        }
        public bool GetCompanyBrands(ref List<COMPANY_WORK_MACROS.BRAND_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = "select brands_type.id, brands_type.brand_name from erp_system.dbo.brands_type where brands_type.id > 0 order by brands_type.id;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                COMPANY_WORK_MACROS.BRAND_STRUCT tempItem;

                tempItem.brandId = commonQueriesSqlObject.rows[i].sql_int[0];
                tempItem.brandName = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(tempItem);
            }

            COMPANY_WORK_MACROS.BRAND_STRUCT lastItem;

            lastItem.brandId = 0;
            lastItem.brandName = "Others";

            returnVector.Add(lastItem);

            return true;
        }
        public bool GetCompanyModels(COMPANY_WORK_MACROS.PRODUCT_STRUCT product, COMPANY_WORK_MACROS.BRAND_STRUCT brand, ref List<COMPANY_WORK_MACROS.MODEL_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = @"select brands_models.model_id, brands_models.brand_model 
                                    from erp_system.dbo.brands_models 
							        where brands_models.product_id = ";


            String sqlQueryPart2 = " and brands_models.brand_id = ";
            String sqlQueryPart3 = " and brands_models.model_id > 0 order by brands_models.model_id; ";


            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += product.typeId;
            sqlQuery += sqlQueryPart2;
            sqlQuery += brand.brandId;
            sqlQuery += sqlQueryPart3;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                COMPANY_WORK_MACROS.MODEL_STRUCT tempItem;

                tempItem.modelId = commonQueriesSqlObject.rows[i].sql_int[0];
                tempItem.modelName = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(tempItem);
            }

            COMPANY_WORK_MACROS.MODEL_STRUCT lastItem;

            lastItem.modelId = 0;
            lastItem.modelName = "Others";

            returnVector.Add(lastItem);

            return true;
        }

        public bool GetModelApplications(int mProductId, int mBrandId, int mModelId, ref List<String> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = @"select model_applications.application 
                                    from erp_system.dbo.model_applications 
							        where model_applications.product_id = ";
            String sqlQueryPart2 = " and model_applications.brand_id = ";
            String sqlQueryPart3 = " and model_applications.model_id = ";
            String sqlQueryPart4 = ";";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += mProductId;
            sqlQuery += sqlQueryPart2;
            sqlQuery += mBrandId;
            sqlQuery += sqlQueryPart3;
            sqlQuery += mModelId;
            sqlQuery += sqlQueryPart4;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
                returnVector.Add(commonQueriesSqlObject.rows[i].sql_string[0]);

            return true;
        }
        public bool GetModelBenefits(int mProductId, int mBrandId, int mModelId, ref List<String> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = @"select model_benefits.benefit 
                                    from erp_system.dbo.model_benefits 
							        where model_benefits.product_id = ";


            String sqlQueryPart2 = " and model_benefits.brand_id = ";
            String sqlQueryPart3 = " and model_benefits.model_id = ";
            String sqlQueryPart4 = ";";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += mProductId;
            sqlQuery += sqlQueryPart2;
            sqlQuery += mBrandId;
            sqlQuery += sqlQueryPart3;
            sqlQuery += mModelId;
            sqlQuery += sqlQueryPart4;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
                returnVector.Add(commonQueriesSqlObject.rows[i].sql_string[0]);

            return true;
        }
        public bool GetModelFeatures(int mProductId, int mBrandId, int mModelId, ref List<String> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = @"select model_standard_features.feature 
                                    from erp_system.dbo.model_standard_features 
        							where model_standard_features.product_id = ";


            String sqlQueryPart2 = " and model_standard_features.brand_id = ";
            String sqlQueryPart3 = " and model_standard_features.model_id = ";
            String sqlQueryPart4 = ";";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += mProductId;
            sqlQuery += sqlQueryPart2;
            sqlQuery += mBrandId;
            sqlQuery += sqlQueryPart3;
            sqlQuery += mModelId;
            sqlQuery += sqlQueryPart4;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
                returnVector.Add(commonQueriesSqlObject.rows[i].sql_string[0]);

            return true;
        }

        //////////////////////////////////////////////////////////////////////
        //GET COMPANY WORK FUNCTIONS
        //////////////////////////////////////////////////////////////////////

        public bool GetRFQs(ref List<COMPANY_WORK_MACROS.RFQ_MAX_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 =
             @"with get_company_info	as	(	select	company_name.company_serial, 
                                                    company_address.address_serial, 
		    										company_name.company_name 
		    								from erp_system.dbo.company_address 
		    								inner join erp_system.dbo.company_name 
		    								on company_address.company_serial = company_name.company_serial 
		    								), 
		    get_sales_person as (select employees_info.employee_id as sales_person_id, 
		    									employees_info.name as sales_person_name 
		    							from erp_system.dbo.employees_info 
		    							), 
		    get_technical_person as (select employees_info.employee_id as technical_person_id, 
		    									employees_info.name as technical_person_name 
		    							from erp_system.dbo.employees_info 
		    							), 
		    get_product1_type as (select products_type.id as product1_type_id, 
		    											products_type.product_name as product1_type 
		    									from erp_system.dbo.products_type 
		    ), 

		    get_product1_brand as (select brands_type.id as product1_brand_id, 
		    	brands_type.brand_name as product1_brand 
		    	from erp_system.dbo.brands_type 
		    ), 
		    get_product1_model as (select brands_models.model_id as product1_model_id, 
		    	brands_models.product_id as product1_type_id, 
		    	brands_models.brand_id as product1_brand_id, 
		    	brands_models.brand_model as product1_model 
		    	from erp_system.dbo.brands_models 
		    ), 

		    get_product2_type as (select products_type.id as product2_type_id, 
		    	products_type.product_name as product2_type 
		    	from erp_system.dbo.products_type 
		    ), 
		    get_product2_brand as (select brands_type.id as product2_brand_id, 
		    	brands_type.brand_name as product2_brand 
		    	from erp_system.dbo.brands_type 
		    ), 
		    get_product2_model as (select brands_models.model_id as product2_model_id, 
		    	brands_models.product_id as product2_type_id, 
		    	brands_models.brand_id as product2_brand_id, 
		    	brands_models.brand_model as product2_model 
		    	from erp_system.dbo.brands_models 
		    ), 

		    get_product3_type as (select products_type.id as product3_type_id, 
		    	products_type.product_name as product3_type 
		    	from erp_system.dbo.products_type 
		    ), 
		    get_product3_brand as (select brands_type.id as product3_brand_id, 
		    	brands_type.brand_name as product3_brand 
		    	from erp_system.dbo.brands_type 
		    ), 
		    get_product3_model as (select brands_models.model_id as product3_model_id, 
		    	brands_models.product_id as product3_type_id, 
		    	brands_models.brand_id as product3_brand_id, 
		    	brands_models.brand_model as product3_model 
		    	from erp_system.dbo.brands_models 
		    ), 

		    get_product4_type as (select products_type.id as product4_type_id, 
		    	products_type.product_name as product4_type 
		    	from erp_system.dbo.products_type 
		    ), 
		    get_product4_brand as (select brands_type.id as product4_brand_id, 
		    	brands_type.brand_name as product4_brand 
		    	from erp_system.dbo.brands_type 
		    ), 
		    get_product4_model as (select brands_models.model_id as product4_model_id, 
		    	brands_models.product_id as product4_type_id, 
		    	brands_models.brand_id as product4_brand_id, 
		    	brands_models.brand_model as product4_model 
		    	from erp_system.dbo.brands_models 
		    ) 

		    select rfqs.sales_person, 
		    		rfqs.rfq_serial, 
		    		rfqs.rfq_version, 

		    		rfqs.assigned_engineer, 

		    		get_company_info.company_serial, 
		    		get_company_info.address_serial, 
		    		contact_person_info.contact_id, 

		    		rfqs.product1_type, 
		    		rfqs.product1_brand, 
		    		rfqs.product1_model, 

		    		rfqs.product2_type, 
		    		rfqs.product2_brand, 
		    		rfqs.product2_model, 

		    		rfqs.product3_type, 
		    		rfqs.product3_brand, 
		    		rfqs.product3_model, 

		    		rfqs.product4_type, 
		    		rfqs.product4_brand, 
		    		rfqs.product4_model, 

		    		contracts_type.id as contract_type_id, 
		    		rfqs_status.id as rfq_status_id, 
		    		rfq_failure_reasons.id as failure_reason_id, 

		    		rfqs.issue_date, 
		    		rfqs.deadline_date, 

		    		rfqs.rfq_id, 

		    		get_sales_person.sales_person_name, 
		    		get_technical_person.technical_person_name, 

		    		get_company_info.company_name, 
		    		contact_person_info.name, 

		    		get_product1_type.product1_type, 
		    		get_product1_brand.product1_brand, 
		    		get_product1_model.product1_model, 

		    		get_product2_type.product2_type, 
		    		get_product2_brand.product2_brand, 
		    		get_product2_model.product2_model, 

		    		get_product3_type.product3_type, 
		    		get_product3_brand.product3_brand, 
		    		get_product3_model.product3_model, 

		    		get_product4_type.product4_type, 
		    		get_product4_brand.product4_brand, 
		    		get_product4_model.product4_model, 

		    		contracts_type.contract_type, 
		    		rfqs_status.rfq_status, 
		    		rfq_failure_reasons.failure_reason 

		    from erp_system.dbo.rfqs 

		    inner join get_sales_person 
		    on rfqs.sales_person = get_sales_person.sales_person_id 

		    inner join get_technical_person 
		    on rfqs.assigned_engineer = get_technical_person.technical_person_id 

		    inner join erp_system.dbo.employees_initials 
		    on get_sales_person.sales_person_id = employees_initials.id 

		    inner join get_company_info 
		    on rfqs.branch_serial = get_company_info.address_serial 

		    inner join erp_system.dbo.contact_person_info 
		    on rfqs.contact_id = contact_person_info.contact_id 
		    and rfqs.branch_serial = contact_person_info.branch_serial 
		    and rfqs.sales_person = contact_person_info.sales_person_id 

		    left join get_product1_type 
		    on rfqs.product1_type = get_product1_type.product1_type_id 
		    left join get_product1_brand 
		    on rfqs.product1_brand = get_product1_brand.product1_brand_id 
		    left join get_product1_model 
		    on rfqs.product1_model = get_product1_model.product1_model_id 
		    and rfqs.product1_type = get_product1_model.product1_type_id 
		    and rfqs.product1_brand = get_product1_model.product1_brand_id 

		    left join get_product2_type 
		    on rfqs.product2_type = get_product2_type.product2_type_id 
		    left join get_product2_brand 
		    on rfqs.product2_brand = get_product2_brand.product2_brand_id 
		    left join get_product2_model 
		    on rfqs.product2_model = get_product2_model.product2_model_id 
		    and rfqs.product2_type = get_product2_model.product2_type_id 
		    and rfqs.product2_brand = get_product2_model.product2_brand_id 

		    left join get_product3_type 
		    on rfqs.product3_type = get_product3_type.product3_type_id 
		    left join get_product3_brand 
		    on rfqs.product3_brand = get_product3_brand.product3_brand_id 
		    left join get_product3_model 
		    on rfqs.product3_model = get_product3_model.product3_model_id 
		    and rfqs.product3_type = get_product3_model.product3_type_id 
		    and rfqs.product3_brand = get_product3_model.product3_brand_id 

		    left join get_product4_type 
		    on rfqs.product4_type = get_product4_type.product4_type_id 
		    left join get_product4_brand 
		    on rfqs.product4_brand = get_product4_brand.product4_brand_id 
		    left join get_product4_model 
		    on rfqs.product4_model = get_product4_model.product4_model_id 
		    and rfqs.product4_type = get_product4_model.product4_type_id 
		    and rfqs.product4_brand = get_product4_model.product4_brand_id 

		    inner join erp_system.dbo.contracts_type 
		    on rfqs.contract_type = contracts_type.id 

		    inner join erp_system.dbo.rfqs_status 
		    on rfqs.rfq_status = rfqs_status.id 

		    left join erp_system.dbo.unsuccessful_rfqs 
		    on rfqs.rfq_serial = unsuccessful_rfqs.rfq_serial 
		    and rfqs.sales_person = unsuccessful_rfqs.sales_person 

		    left join erp_system.dbo.rfq_failure_reasons 
		    on unsuccessful_rfqs.failure_reason = rfq_failure_reasons.id 

		    order by rfqs.issue_date desc, employees_initials.employee_initial, rfqs.rfq_serial desc, rfqs.rfq_version desc;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 22;
            queryColumns.sql_datetime = 2;
            queryColumns.sql_string = 20;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                COMPANY_WORK_MACROS.RFQ_MAX_STRUCT RFQItem = new COMPANY_WORK_MACROS.RFQ_MAX_STRUCT();

                RFQItem.products = new List<COMPANY_WORK_MACROS.RFQ_PRODUCT_STRUCT>();

                int numericCount = 0;
                int stringCount = 0;

                RFQItem.rfq_id = commonQueriesSqlObject.rows[i].sql_string[stringCount++];

                RFQItem.sales_person_name = commonQueriesSqlObject.rows[i].sql_string[stringCount++];
                RFQItem.assignee_name = commonQueriesSqlObject.rows[i].sql_string[stringCount++];

                RFQItem.company_name = commonQueriesSqlObject.rows[i].sql_string[stringCount++];
                RFQItem.contact_name = commonQueriesSqlObject.rows[i].sql_string[stringCount++];


                RFQItem.issue_date = commonQueriesSqlObject.rows[i].sql_datetime[0].ToString();
                RFQItem.deadline_date = commonQueriesSqlObject.rows[i].sql_datetime[1].ToString();


                RFQItem.sales_person_id = commonQueriesSqlObject.rows[i].sql_int[numericCount++];
                RFQItem.rfq_serial = commonQueriesSqlObject.rows[i].sql_int[numericCount++];
                RFQItem.rfq_version = commonQueriesSqlObject.rows[i].sql_int[numericCount++];

                RFQItem.assignee_id = commonQueriesSqlObject.rows[i].sql_int[numericCount++];

                RFQItem.company_serial = commonQueriesSqlObject.rows[i].sql_int[numericCount++];
                RFQItem.branch_serial = commonQueriesSqlObject.rows[i].sql_int[numericCount++];
                RFQItem.contact_id = commonQueriesSqlObject.rows[i].sql_int[numericCount++];

                for (int j = 0; j < COMPANY_WORK_MACROS.MAX_RFQ_PRODUCTS; j++)
                {
                    COMPANY_WORK_MACROS.RFQ_PRODUCT_STRUCT tempProductItem;

                    tempProductItem.productType = new COMPANY_WORK_MACROS.PRODUCT_STRUCT();
                    tempProductItem.productBrand = new COMPANY_WORK_MACROS.BRAND_STRUCT();
                    tempProductItem.productModel = new COMPANY_WORK_MACROS.MODEL_STRUCT();

                    tempProductItem.productType.typeId = commonQueriesSqlObject.rows[i].sql_int[numericCount++];
                    tempProductItem.productType.typeName = commonQueriesSqlObject.rows[i].sql_string[stringCount++];

                    tempProductItem.productBrand.brandId = commonQueriesSqlObject.rows[i].sql_int[numericCount++];
                    tempProductItem.productBrand.brandName = commonQueriesSqlObject.rows[i].sql_string[stringCount++];

                    tempProductItem.productModel.modelId = commonQueriesSqlObject.rows[i].sql_int[numericCount++];
                    tempProductItem.productModel.modelName = commonQueriesSqlObject.rows[i].sql_string[stringCount++];

                    tempProductItem.productNumber = j;
                    tempProductItem.productQuantity = 0;

                    RFQItem.products.Add(tempProductItem);
                }

                RFQItem.contract_type_id = commonQueriesSqlObject.rows[i].sql_int[numericCount++];
                RFQItem.rfq_status_id = commonQueriesSqlObject.rows[i].sql_int[numericCount++];
                RFQItem.failure_reason_id = commonQueriesSqlObject.rows[i].sql_int[numericCount++];

                RFQItem.contract_type = commonQueriesSqlObject.rows[i].sql_string[stringCount++];
                RFQItem.rfq_status = commonQueriesSqlObject.rows[i].sql_string[stringCount++];
                RFQItem.failure_reason = commonQueriesSqlObject.rows[i].sql_string[stringCount++];

                returnVector.Add(RFQItem);
            }

            return true;
        }
        public bool GetRFQs(int salesPersonId, int assigneeId, int rfqStatus, ref List<COMPANY_WORK_MACROS.RFQ_BASIC_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = @"select rfqs.rfq_serial, 
        									rfqs.rfq_version, 
        									rfqs.rfq_id 
        							from erp_system.dbo.rfqs 
        							inner join erp_system.dbo.company_address 
        							on rfqs.branch_serial = company_address.address_serial 
        							where rfqs.sales_person = ";
            String sqlQueryPart2 = " and rfqs.assigned_engineer = ";
            String sqlQueryPart3 = " and rfqs.rfq_status = ";
            String sqlQueryPart4 = " order by rfqs.issue_date;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += salesPersonId;
            sqlQuery += sqlQueryPart2;
            sqlQuery += assigneeId;
            sqlQuery += sqlQueryPart3;
            sqlQuery += rfqStatus;
            sqlQuery += sqlQueryPart4;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 2;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                COMPANY_WORK_MACROS.RFQ_BASIC_STRUCT tempItem;

                tempItem.rfq_serial = commonQueriesSqlObject.rows[i].sql_int[0];
                tempItem.rfq_version = commonQueriesSqlObject.rows[i].sql_int[1];
                tempItem.rfq_id = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(tempItem);
            }

            return true;
        }

        public bool GetWorkOffers(ref List<COMPANY_WORK_MACROS.WORK_OFFER_MAX_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 =
                @"with get_company_info	as	(	select company_name.company_serial, 
        		company_address.address_serial, 
        		company_name.company_name 
        		from erp_system.dbo.company_address 
        		inner join erp_system.dbo.company_name 
        		on company_address.company_serial = company_name.company_serial 
        		), 
        		get_sales_person	as(select employees_info.employee_id as sales_person_id, 
        			employees_info.name as sales_person_name 
        			from erp_system.dbo.employees_info 
        		), 
        		get_offer_proposer	as(select employees_info.employee_id as offer_proposer_id, 
        			employees_info.name as offer_proposer_name 
        			from erp_system.dbo.employees_info 
        		), 
        		get_product_type	as(select products_type.id as product_type_id, 
        			products_type.product_name as product_type 
        			from erp_system.dbo.products_type 
        		), 
        		get_product_brand	as(select brands_type.id as product_brand_id, 
        			brands_type.brand_name as product_brand 
        			from erp_system.dbo.brands_type 
        		), 
        		get_product_model	as(select brands_models.model_id as product_model_id, 
        			brands_models.product_id as product_type_id, 
        			brands_models.brand_id as product_brand_id, 
        			brands_models.brand_model as product_model 
        			from erp_system.dbo.brands_models 
        		) 
        
        		select	work_offers.offer_proposer, 
        		work_offers.offer_serial, 
        		work_offers.offer_version, 
        
        		get_sales_person.sales_person_id, 
        		get_company_info.company_serial, 
        		get_company_info.address_serial, 
        		contact_person_info.contact_id, 
        
        		work_offers_products_info.product_number, 
        		get_product_type.product_type_id, 
        		get_product_brand.product_brand_id, 
        		get_product_model.product_model_id, 
        
        		contracts_type.id as contract_type_id, 
        		offers_status.id as offer_status_id, 
        		offer_failure_reasons.id as failure_reason_id, 
        
        		work_offers.issue_date, 
        
        		work_offers.offer_id, 
        
        		get_sales_person.sales_person_name, 
        		get_offer_proposer.offer_proposer_name, 
        
        		get_company_info.company_name, 
        		contact_person_info.name, 
        
        	 	get_product_type.product_type, 
        		get_product_brand.product_brand, 
        		get_product_model.product_model, 
        
        		contracts_type.contract_type, 
        		offers_status.offer_status, 
        		offer_failure_reasons.failure_reason 
        
        		from erp_system.dbo.work_offers 
        
        		inner join get_company_info 
        		on work_offers.branch_serial = get_company_info.address_serial 
        
        		inner join erp_system.dbo.contact_person_info 
        		on work_offers.sales_person = contact_person_info.sales_person_id 
        		and work_offers.branch_serial = contact_person_info.branch_serial 
        		and work_offers.contact_id = contact_person_info.contact_id 
        
        		inner join get_sales_person 
        		on work_offers.sales_person = get_sales_person.sales_person_id 
        
        		inner join get_offer_proposer 
        		on work_offers.offer_proposer = get_offer_proposer.offer_proposer_id 
        
        		inner join erp_system.dbo.employees_initials 
        		on get_offer_proposer.offer_proposer_id = employees_initials.id 
        
        		inner join erp_system.dbo.work_offers_products_info 
        		on work_offers.offer_proposer = work_offers_products_info.offer_proposer 
        		and work_offers.offer_serial = work_offers_products_info.offer_serial 
        		and work_offers.offer_version = work_offers_products_info.offer_version 
        
        		inner join get_product_type 
        		on work_offers_products_info.product_type = get_product_type.product_type_id 
        		inner join get_product_brand 
        		on work_offers_products_info.product_brand = get_product_brand.product_brand_id 
        		left join get_product_model 
        		on work_offers_products_info.product_model = get_product_model.product_model_id 
        		and work_offers_products_info.product_type = get_product_model.product_type_id 
        		and work_offers_products_info.product_brand = get_product_model.product_brand_id 
        
        		inner join erp_system.dbo.contracts_type 
        		on work_offers.contract_type = contracts_type.id 
        
        		inner join erp_system.dbo.offers_status 
        		on work_offers.offer_status = offers_status.id 
        
        		left join erp_system.dbo.unsucessful_work_offers 
        		on work_offers.offer_proposer = unsucessful_work_offers.offer_proposer 
        		and work_offers.offer_serial = unsucessful_work_offers.offer_serial 
        		and work_offers.offer_version = unsucessful_work_offers.offer_version 
        
        		left join erp_system.dbo.offer_failure_reasons 
        		on unsucessful_work_offers.failure_reason = offer_failure_reasons.id 
        
        		order by work_offers.issue_date desc, employees_initials.employee_initial, offer_serial, offer_version, product_number;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 14;
            queryColumns.sql_datetime = 1;
            queryColumns.sql_string = 11;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                int numericCount = 0;
                int stringCount = 0;

                COMPANY_WORK_MACROS.WORK_OFFER_MAX_STRUCT offerItem = new COMPANY_WORK_MACROS.WORK_OFFER_MAX_STRUCT();

                offerItem.products = new List<COMPANY_WORK_MACROS.OFFER_PRODUCT_STRUCT>();

                COMPANY_WORK_MACROS.OFFER_PRODUCT_STRUCT tempProductItem = new COMPANY_WORK_MACROS.OFFER_PRODUCT_STRUCT();

                tempProductItem.productType = new COMPANY_WORK_MACROS.PRODUCT_STRUCT();
                tempProductItem.productBrand = new COMPANY_WORK_MACROS.BRAND_STRUCT();
                tempProductItem.productModel = new COMPANY_WORK_MACROS.MODEL_STRUCT();

                offerItem.offer_id = commonQueriesSqlObject.rows[i].sql_string[stringCount++];

                offerItem.sales_person_name = commonQueriesSqlObject.rows[i].sql_string[stringCount++];
                offerItem.offer_proposer_name = commonQueriesSqlObject.rows[i].sql_string[stringCount++];

                offerItem.company_name = commonQueriesSqlObject.rows[i].sql_string[stringCount++];
                offerItem.contact_name = commonQueriesSqlObject.rows[i].sql_string[stringCount++];

                offerItem.issue_date = commonQueriesSqlObject.rows[i].sql_datetime[0].ToString();

                offerItem.offer_proposer_id = commonQueriesSqlObject.rows[i].sql_int[numericCount++];
                offerItem.offer_serial = commonQueriesSqlObject.rows[i].sql_int[numericCount++];
                offerItem.offer_version = commonQueriesSqlObject.rows[i].sql_int[numericCount++];

                offerItem.sales_person_id = commonQueriesSqlObject.rows[i].sql_int[numericCount++];
                offerItem.company_serial = commonQueriesSqlObject.rows[i].sql_int[numericCount++];
                offerItem.branch_serial = commonQueriesSqlObject.rows[i].sql_int[numericCount++];
                offerItem.contact_id = commonQueriesSqlObject.rows[i].sql_int[numericCount++];

                int product_number = (int)commonQueriesSqlObject.rows[i].sql_int[numericCount++];

                tempProductItem.productType.typeId = commonQueriesSqlObject.rows[i].sql_int[numericCount++];
                tempProductItem.productType.typeName = commonQueriesSqlObject.rows[i].sql_string[stringCount++];

                tempProductItem.productBrand.brandId = commonQueriesSqlObject.rows[i].sql_int[numericCount++];
                tempProductItem.productBrand.brandName = commonQueriesSqlObject.rows[i].sql_string[stringCount++];

                tempProductItem.productModel.modelId = commonQueriesSqlObject.rows[i].sql_int[numericCount++];
                tempProductItem.productModel.modelName = commonQueriesSqlObject.rows[i].sql_string[stringCount++];

                tempProductItem.productNumber = product_number;
                tempProductItem.productQuantity = 0;

                if (i > 0 && returnVector.Last().offer_proposer_id == offerItem.offer_proposer_id && returnVector.Last().offer_serial == offerItem.offer_serial && returnVector.Last().offer_version == offerItem.offer_version)
                {
                    returnVector.Last().products.Add(tempProductItem);
                    returnVector.Last().products.Sort();
                }
                else
                {
                    offerItem.products.Add(tempProductItem);

                    offerItem.contract_type_id = commonQueriesSqlObject.rows[i].sql_int[numericCount++];
                    offerItem.offer_status_id = commonQueriesSqlObject.rows[i].sql_int[numericCount++];
                    offerItem.failure_reason_id = commonQueriesSqlObject.rows[i].sql_int[numericCount++];

                    offerItem.contract_type = commonQueriesSqlObject.rows[i].sql_string[stringCount++];
                    offerItem.offer_status = commonQueriesSqlObject.rows[i].sql_string[stringCount++];
                    offerItem.failure_reason = commonQueriesSqlObject.rows[i].sql_string[stringCount++];

                    returnVector.Add(offerItem);
                }

            }

            return true;
        }
        public bool GetWorkOrders(ref List<COMPANY_WORK_MACROS.WORK_ORDER_MAX_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 =
                @"with get_company_info	as	(	select company_name.company_serial, 
        		company_address.address_serial, 
        		company_name.company_name 
        		from erp_system.dbo.company_address 
        		inner join erp_system.dbo.company_name 
        		on company_address.company_serial = company_name.company_serial 
        		), 
        		get_sales_person		as(select employees_info.employee_id as sales_person_id, 
        			employees_info.name as sales_person_name 
        			from erp_system.dbo.employees_info 
        		), 
        		get_offer_proposer	as(select employees_info.employee_id as offer_proposer_id, 
        			employees_info.name as offer_proposer_name 
        			from erp_system.dbo.employees_info 
        		), 
        		get_product_type		as(select products_type.id as product_type_id, 
        			products_type.product_name as product_type 
        			from erp_system.dbo.products_type 
        		), 
        		get_product_brand		as(select brands_type.id as product_brand_id, 
        			brands_type.brand_name as product_brand 
        			from erp_system.dbo.brands_type 
        		), 
        		get_product_model		as(select brands_models.model_id as product_model_id, 
        			brands_models.product_id as product_type_id, 
        			brands_models.brand_id as product_brand_id, 
        			brands_models.brand_model as product_model 
        			from erp_system.dbo.brands_models 
        		) 
        
        		select 	get_sales_person.sales_person_id, 
        		work_orders.order_serial, 
        
        		get_offer_proposer.offer_proposer_id, 
        
        		get_company_info.company_serial, 
        		get_company_info.address_serial, 
        		contact_person_info.contact_id, 
        
        		work_offers_products_info.product_number, 
        		get_product_type.product_type_id, 
        		get_product_brand.product_brand_id, 
        		get_product_model.product_model_id, 
        
        		contracts_type.id as contract_type_id, 
        		orders_status.id as order_status_id, 
        
        		work_orders.issue_date, 
        
        		work_orders.order_id, 
        
        		get_sales_person.sales_person_name, 
        		get_offer_proposer.offer_proposer_name, 
        
        		get_company_info.company_name, 
        		contact_person_info.name, 
        
        		get_product_type.product_type, 
        		get_product_brand.product_brand, 
        		get_product_model.product_model, 
        
        		contracts_type.contract_type, 
        		orders_status.order_status 
        
        		from erp_system.dbo.work_orders 
        
        		inner join erp_system.dbo.work_offers 
        		on work_orders.offer_proposer = work_offers.offer_proposer 
        		and work_orders.offer_serial = work_offers.offer_serial 
        		and work_orders.offer_version = work_offers.offer_version 
        
        		inner join get_company_info 
        		on work_offers.branch_serial = get_company_info.address_serial 
        
        		inner join erp_system.dbo.contact_person_info 
        		on work_offers.sales_person = contact_person_info.sales_person_id 
        		and work_offers.branch_serial = contact_person_info.branch_serial 
        		and work_offers.contact_id = contact_person_info.contact_id 
        
        		inner join get_sales_person 
        		on work_offers.sales_person = get_sales_person.sales_person_id 
        
        		inner join get_offer_proposer 
        		on work_offers.offer_proposer = get_offer_proposer.offer_proposer_id 
        
        		inner join erp_system.dbo.employees_initials 
        		on get_offer_proposer.offer_proposer_id = employees_initials.id 
        
        		inner join erp_system.dbo.work_offers_products_info 
        		on work_offers.offer_proposer = work_offers_products_info.offer_proposer 
        		and work_offers.offer_serial = work_offers_products_info.offer_serial 
        		and work_offers.offer_version = work_offers_products_info.offer_version 
        
        		inner join get_product_type 
        		on work_offers_products_info.product_type = get_product_type.product_type_id 
        		inner join get_product_brand 
        		on work_offers_products_info.product_brand = get_product_brand.product_brand_id 
        		left join get_product_model 
        		on work_offers_products_info.product_model = get_product_model.product_model_id 
        		and work_offers_products_info.product_type = get_product_model.product_type_id 
        		and work_offers_products_info.product_brand = get_product_model.product_brand_id 
        
        		inner join erp_system.dbo.contracts_type 
        		on work_offers.contract_type = contracts_type.id 
        
        		inner join erp_system.dbo.orders_status 
        		on work_orders.order_status = orders_status.id 
        
        		order by work_orders.issue_date desc, employees_initials.employee_initial, work_orders.order_serial, work_offers_products_info.product_number;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 12;
            queryColumns.sql_datetime = 1;
            queryColumns.sql_string = 10;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                int numericCount = 0;
                int stringCount = 0;

                COMPANY_WORK_MACROS.WORK_ORDER_MAX_STRUCT orderItem = new COMPANY_WORK_MACROS.WORK_ORDER_MAX_STRUCT();

                orderItem.products = new List<COMPANY_WORK_MACROS.ORDER_PRODUCT_STRUCT>();

                COMPANY_WORK_MACROS.ORDER_PRODUCT_STRUCT tempProductItem = new COMPANY_WORK_MACROS.ORDER_PRODUCT_STRUCT();

                tempProductItem.productType = new COMPANY_WORK_MACROS.PRODUCT_STRUCT();
                tempProductItem.productBrand = new COMPANY_WORK_MACROS.BRAND_STRUCT();
                tempProductItem.productModel = new COMPANY_WORK_MACROS.MODEL_STRUCT();

                orderItem.order_id = commonQueriesSqlObject.rows[i].sql_string[stringCount++];

                orderItem.sales_person_name = commonQueriesSqlObject.rows[i].sql_string[stringCount++];
                orderItem.offer_proposer_name = commonQueriesSqlObject.rows[i].sql_string[stringCount++];

                orderItem.company_name = commonQueriesSqlObject.rows[i].sql_string[stringCount++];
                orderItem.contact_name = commonQueriesSqlObject.rows[i].sql_string[stringCount++];

                orderItem.issue_date = commonQueriesSqlObject.rows[i].sql_datetime[0].ToString();

                orderItem.sales_person_id = commonQueriesSqlObject.rows[i].sql_int[numericCount++];
                orderItem.order_serial = commonQueriesSqlObject.rows[i].sql_int[numericCount++];

                orderItem.offer_proposer_id = commonQueriesSqlObject.rows[i].sql_int[numericCount++];

                orderItem.company_serial = commonQueriesSqlObject.rows[i].sql_int[numericCount++];
                orderItem.branch_serial = commonQueriesSqlObject.rows[i].sql_int[numericCount++];
                orderItem.contact_id = commonQueriesSqlObject.rows[i].sql_int[numericCount++];

                int product_number = commonQueriesSqlObject.rows[i].sql_int[numericCount++];

                tempProductItem.productType.typeId = commonQueriesSqlObject.rows[i].sql_int[numericCount++];
                tempProductItem.productType.typeName = commonQueriesSqlObject.rows[i].sql_string[stringCount++];

                tempProductItem.productBrand.brandId = commonQueriesSqlObject.rows[i].sql_int[numericCount++];
                tempProductItem.productBrand.brandName = commonQueriesSqlObject.rows[i].sql_string[stringCount++];

                tempProductItem.productModel.modelId = commonQueriesSqlObject.rows[i].sql_int[numericCount++];
                tempProductItem.productModel.modelName = commonQueriesSqlObject.rows[i].sql_string[stringCount++];

                tempProductItem.productNumber = product_number;
                tempProductItem.productQuantity = 0;

                if (i > 0 && returnVector.Last().sales_person_id == orderItem.sales_person_id && returnVector.Last().order_serial == orderItem.order_serial)
                {
                    returnVector.Last().products.Add(tempProductItem);
                    returnVector.Last().products.Sort();
                }
                else
                {
                    orderItem.products.Add(tempProductItem);

                    orderItem.contract_type_id = commonQueriesSqlObject.rows[i].sql_int[numericCount++];
                    orderItem.order_status_id = commonQueriesSqlObject.rows[i].sql_int[numericCount++];

                    orderItem.contract_type = commonQueriesSqlObject.rows[i].sql_string[stringCount++];
                    orderItem.order_status = commonQueriesSqlObject.rows[i].sql_string[stringCount++];

                    returnVector.Add(orderItem);
                }
            }

            return true;
        }

        public bool GetClientVisits(ref List<COMPANY_WORK_MACROS.CLIENT_VISIT_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = @"with get_company_name	as	(	select company_address.address_serial, 
        																	company_name.company_name 
        															from erp_system.dbo.company_address 
        															inner join erp_system.dbo.company_name 
        															on company_address.company_serial = company_name.company_serial 
        															), 
        							get_visit_result		as	(	select successful_visit_plans.sales_person, 
        																	successful_visit_plans.visit_serial, 
        																	visit_plans_result.visit_result 
        															from erp_system.dbo.successful_visit_plans 
        															inner join erp_system.dbo.visit_plans_result 
        															on successful_visit_plans.visit_result = visit_plans_result.id 
        														) 
        							select visit_plans.sales_person, 
        							visit_plans.visit_serial, 
        							visit_plans.visit_status, 
        
        							visit_plans.issue_date, 
        							visit_plans.date_of_visit, 
        
        							employees_info.name, 
        							get_company_name.company_name, 
        							contact_person_info.name, 
        							visit_plans_purpose.visit_purpose, 
        							visit_plans_status.visit_status, 
        							get_visit_result.visit_result 
        
        							from erp_system.dbo.visit_plans 
        
        							inner join erp_system.dbo.employees_info 
        							on visit_plans.sales_person = employees_info.employee_id 
        
        							inner join get_company_name 
        							on visit_plans.branch_serial = get_company_name.address_serial 
        
        							inner join erp_system.dbo.visit_plans_purpose 
        							on visit_plans.visit_purpose = visit_plans_purpose.id 
        
        							inner join erp_system.dbo.contact_person_info 
        							on visit_plans.sales_person = contact_person_info.sales_person_id 
        							and visit_plans.branch_serial = contact_person_info.branch_serial 
        							and visit_plans.contact_id = contact_person_info.contact_id 
        
        							inner join erp_system.dbo.visit_plans_status 
        							on visit_plans.visit_status = visit_plans_status.id 
        
        							left join get_visit_result 
        							on visit_plans.sales_person = get_visit_result.sales_person 
        							and visit_plans.visit_serial = get_visit_result.visit_serial 
        
        							order by visit_plans.date_of_visit DESC;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 4;
            queryColumns.sql_datetime = 2;
            queryColumns.sql_string = 5;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                int numericCount = 0;
                int stringCount = 0;

                COMPANY_WORK_MACROS.CLIENT_VISIT_STRUCT visitItem;

                visitItem.sales_person_id = commonQueriesSqlObject.rows[i].sql_int[numericCount++];
                visitItem.visit_serial = commonQueriesSqlObject.rows[i].sql_int[numericCount++];

                visitItem.visit_purpose_id = commonQueriesSqlObject.rows[i].sql_int[numericCount++];
                visitItem.visit_result_id = commonQueriesSqlObject.rows[i].sql_int[numericCount++];

                visitItem.issue_date = commonQueriesSqlObject.rows[i].sql_datetime[0].ToString();
                visitItem.visit_date = commonQueriesSqlObject.rows[i].sql_datetime[1].ToString();

                visitItem.sales_person_name = commonQueriesSqlObject.rows[i].sql_string[stringCount++];

                visitItem.company_name = commonQueriesSqlObject.rows[i].sql_string[stringCount++];
                visitItem.contact_name = commonQueriesSqlObject.rows[i].sql_string[stringCount++];

                visitItem.visit_purpose = commonQueriesSqlObject.rows[i].sql_string[stringCount++];
                visitItem.visit_result = commonQueriesSqlObject.rows[i].sql_string[stringCount++];

                returnVector.Add(visitItem);
            }

            return true;
        }

        public bool GetClientCalls(ref List<COMPANY_WORK_MACROS.CLIENT_CALL_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = @"with get_company_name	as	(	select company_address.address_serial, 

                                                                    company_name.company_name 
															from erp_system.dbo.company_address 
															inner join erp_system.dbo.company_name 
															on company_address.company_serial = company_name.company_serial 
															) 
		    select client_calls.sales_person, 
		    client_calls.call_serial, 
		    calls_purpose.id, 
		    calls_result.id, 

		    client_calls.issue_date, 
		    client_calls.date_of_call, 

		    employees_info.name, 
		    get_company_name.company_name, 
		    contact_person_info.name, 
		    calls_purpose.call_purpose, 
		    calls_result.call_result 

		    from erp_system.dbo.client_calls 

		    inner join erp_system.dbo.employees_info 
		    on client_calls.sales_person = employees_info.employee_id 

		    inner join get_company_name 
		    on client_calls.branch_serial = get_company_name.address_serial 

		    inner join erp_system.dbo.calls_purpose 
		    on client_calls.call_purpose = calls_purpose.id 

		    inner join erp_system.dbo.calls_result 
		    on client_calls.call_result = calls_result.id 

		    inner join erp_system.dbo.contact_person_info 
		    on client_calls.sales_person = contact_person_info.sales_person_id 
		    and client_calls.branch_serial = contact_person_info.branch_serial 
		    and client_calls.contact_id = contact_person_info.contact_id 

		    order by client_calls.date_of_call DESC; ";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 4;
            queryColumns.sql_datetime = 2;
            queryColumns.sql_string = 5;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                int numericCount = 0;
                int stringCount = 0;

                COMPANY_WORK_MACROS.CLIENT_CALL_STRUCT callItem;

                callItem.sales_person_id = commonQueriesSqlObject.rows[i].sql_int[numericCount++];
                callItem.call_serial = commonQueriesSqlObject.rows[i].sql_int[numericCount++];

                callItem.call_purpose_id = commonQueriesSqlObject.rows[i].sql_int[numericCount++];
                callItem.call_result_id = commonQueriesSqlObject.rows[i].sql_int[numericCount++];

                callItem.issue_date = commonQueriesSqlObject.rows[i].sql_datetime[0].ToString();
                callItem.call_date = commonQueriesSqlObject.rows[i].sql_datetime[1].ToString();

                callItem.sales_person_name = commonQueriesSqlObject.rows[i].sql_string[stringCount++];

                callItem.company_name = commonQueriesSqlObject.rows[i].sql_string[stringCount++];
                callItem.contact_name = commonQueriesSqlObject.rows[i].sql_string[stringCount++];

                callItem.call_purpose = commonQueriesSqlObject.rows[i].sql_string[stringCount++];
                callItem.call_result = commonQueriesSqlObject.rows[i].sql_string[stringCount++];

                returnVector.Add(callItem);
            }

            return true;
        }
    }

}
