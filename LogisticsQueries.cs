using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _01electronics_erp;

namespace _01electronics_logistics
{
    class LogisticsQueries
    {
        private String sqlQuery;
        private SQLServer commonQueriesSqlObject;
        public LogisticsQueries()
        {
            commonQueriesSqlObject = new SQLServer();
        }

        //////////////////////////////////////////////////////////////////////
        //GET AGENT INFO FUNCTIONS
        //////////////////////////////////////////////////////////////////////
        public bool GetAgentsSerialsAndNames(ref List<AGENCY_MACROS.AGENCY_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = "select agency_serial, agency_name from erp_system.dbo.agency_name order by agency_name;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                AGENCY_MACROS.AGENCY_STRUCT tempItem;

                tempItem.agency_serial = commonQueriesSqlObject.rows[i].sql_int[0];
                tempItem.agency_name = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(tempItem);
            }

            return true;
        }


        public bool GetFullAgentInfo(int agentId,ref AGENCY_MACROS.AGENCY_FULL_INFO agent)
        {
            String sqlQuery = @"select districts.id,agency_name.agency_name,[name],
countries.country,states_governorates.state_governorate,cities.city,districts.district
,agency_telephone.telephone_number
from agency_name,agency_address,
countries,states_governorates,cities,districts,
employees_info,agency_telephone
where agency_address.[address] = districts.id 
AND agency_name.added_by = employees_info.employee_id
AND agency_name.agency_serial = agency_address.agency_serial
AND agency_telephone.branch_serial = agency_address.address_serial
AND districts.city = cities.id 
AND cities.state_governorate = states_governorates.id
AND states_governorates.country = countries.id
AND agency_name.agency_serial = " + agentId;
            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();
            
            queryColumns.sql_int = 1;
            queryColumns.sql_string = 7;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            agent.agency_name = commonQueriesSqlObject.rows[0].sql_string[0];
            agent.employee_name = commonQueriesSqlObject.rows[0].sql_string[1];
            agent.country = commonQueriesSqlObject.rows[0].sql_string[2];
            agent.state = commonQueriesSqlObject.rows[0].sql_string[3];
            agent.city = commonQueriesSqlObject.rows[0].sql_string[4];
            agent.district = commonQueriesSqlObject.rows[0].sql_string[5];
            agent.telephone = commonQueriesSqlObject.rows[0].sql_string[6];
            agent.addressId = commonQueriesSqlObject.rows[0].sql_int[0];

            return true;
        }

        public bool UpdateAgent(int agentId, ref AGENCY_MACROS.AGENCY_FULL_INFO agent)
        {
            String sqlQuery = @"UPDATE agency_name "+
"SET agency_name =\'"+agent.agency_name+
"\',added_by =(select employee_id from employees_info where [name] =  \'"+ agent.employee_name+"\')" +
" where agency_serial = " + agentId;
            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();
            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            sqlQuery = @"UPDATE agency_address "+
"SET [address] = (select id from districts where district = '"+agent.district+"\') ,"+
"added_by = (select employee_id from employees_info where [name] =  \'" + agent.employee_name + "\')"+
" where agency_serial = " + agentId;
            queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();
            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            sqlQuery = @"UPDATE agency_telephone "+
"SET telephone_number = \'"+agent.telephone+
"\',added_by = (select employee_id from employees_info where [name] =  '" + agent.employee_name + "\')"+
"where branch_serial = " + agentId;
            queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();
            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            return true;
        }

        public bool AddAgent(ref AGENCY_MACROS.AGENCY_FULL_INFO agent)
        {
            // Get new serial
            String sqlQuery = @" (select max(agency_serial)+1 from agency_name);";
            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();
            queryColumns.sql_int = 1;
            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;
            int newSerial = commonQueriesSqlObject.rows[0].sql_int[0];
            // Get employee id
            sqlQuery = @"select employee_id from employees_info where [name] =  '" + agent.employee_name+"\'";
            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;
            int newEmployeeId = commonQueriesSqlObject.rows[0].sql_int[0];
            // Add entry to agency_name table
            sqlQuery = "INSERT INTO agency_name (agency_serial,agency_name,added_by)values(" + newSerial
                + ",\'" + agent.agency_name + "\'," + newEmployeeId + ");";
            queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();
            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;
            // Add entry to agency_address table
            sqlQuery = "INSERT INTO agency_address (address_serial,agency_serial,address,added_by)" +
                " values(" + newSerial + "," + newSerial + "," + agent.addressId + "," + newEmployeeId + "); ";
            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;
            // Add entry to agency_telephone table
            sqlQuery = "INSERT INTO agency_telephone (branch_serial,telephone_number,added_by) values(" +
                newSerial + ",\'" + agent.telephone + "\'," + newEmployeeId + "); ";
            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;
            

            return true;
        }

        public int GetDistrictAddress(String districtName)
        {
            String sqlQuery = @"select id from districts where district = '"+districtName+"\';";
            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();
            queryColumns.sql_int = 1;
            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return -5;
            return commonQueriesSqlObject.rows[0].sql_int[0];
        }
    }
}
