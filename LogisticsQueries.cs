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
            this.commonQueriesSqlObject = new SQLServer();

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


        public bool GetFullAgentInfo(int agentId,ref AGENCY_MACROS.AGENCY_FULL_INFO agency)
        {
            String sqlQuery = @"select agency_name.agency_name,[name]
from agency_name,employees_info
where agency_name.added_by = employees_info.employee_id
AND agency_name.agency_serial = " + agentId;
            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();
            
            queryColumns.sql_string = 2;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            agency.agency_name = commonQueriesSqlObject.rows[0].sql_string[0];
            agency.employee_name = commonQueriesSqlObject.rows[0].sql_string[1];

            sqlQuery = @"select agency_address.[address],district,cities.city,states_governorates.state_governorate,countries.country 
from agency_address,districts,cities,states_governorates,countries
where districts.id = agency_address.[address]
AND districts.city = cities.id 
AND cities.state_governorate = states_governorates.id
AND states_governorates.country = countries.id
AND agency_address.agency_serial = "+agentId;

            queryColumns.sql_int = 1;
            queryColumns.sql_string = 4;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;
            agency.branches  = new List<AGENCY_MACROS.AGENCY_ADDRESS>(); 
            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                AGENCY_MACROS.AGENCY_ADDRESS temp;
                temp.addressId = commonQueriesSqlObject.rows[i].sql_int[0];
                temp.district = commonQueriesSqlObject.rows[i].sql_string[0];
                temp.city = commonQueriesSqlObject.rows[i].sql_string[1];
                temp.state = commonQueriesSqlObject.rows[i].sql_string[2];
                temp.country = commonQueriesSqlObject.rows[i].sql_string[3];
                agency.branches.Add(temp);
            }

            sqlQuery = @"select telephone_number
from agency_telephone,agency_address
where agency_address.address_serial = agency_telephone.branch_serial
AND agency_address.address_serial = " + agentId;

            queryColumns.sql_int = 0;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;
            agency.telephones = new List<String>();
            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                agency.telephones.Add(commonQueriesSqlObject.rows[i].sql_string[0]);
            }

            sqlQuery = @"select fax
from agency_fax,agency_address
where agency_address.address_serial = agency_fax.branch_serial
AND agency_address.address_serial = " + agentId;

            queryColumns.sql_int = 0;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;
            agency.faxes = new List<String>();
            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                agency.faxes.Add(commonQueriesSqlObject.rows[i].sql_string[0]);
            }


            return true;
        }

        public bool GetAllContactsOfBranch(int branchSerial,List<AGENCY_MACROS.AGENCY_CONTACT_INFO> contacts)
        {
            String sqlQuery = @"select agency_contact_person_info.contact_id,employee_id,branch_serial,agency_contact_person_info.department,email,[name],gender,departments_type.department
from agency_contact_person_info,departments_type
where agency_contact_person_info.department = departments_type.id 
AND branch_serial = " + branchSerial;
            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 4;
            queryColumns.sql_string = 4;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                AGENCY_MACROS.AGENCY_CONTACT_INFO temp = new AGENCY_MACROS.AGENCY_CONTACT_INFO();

                temp.contactId = commonQueriesSqlObject.rows[i].sql_int[0];
                temp.employeeId = commonQueriesSqlObject.rows[i].sql_int[1];
                temp.branchId = commonQueriesSqlObject.rows[i].sql_int[2];
                temp.department.department_id = commonQueriesSqlObject.rows[i].sql_int[3];
                temp.email = commonQueriesSqlObject.rows[i].sql_string[0];
                temp.name = commonQueriesSqlObject.rows[i].sql_string[1];
                temp.gender = commonQueriesSqlObject.rows[i].sql_string[2];
                temp.department.department_name = commonQueriesSqlObject.rows[i].sql_string[3];
                temp.telephones = new List<String>();

                contacts.Add(temp);
            }

            GetContactTelephones(contacts);

            return true;
        }

        private bool GetContactTelephones(List<AGENCY_MACROS.AGENCY_CONTACT_INFO> contacts)
        {
            for (int i = 0; i < contacts.Count; i++)
            {
                AGENCY_MACROS.AGENCY_CONTACT_INFO contact = contacts[i];
                

                String sqlQuery = @"select mobile from agency_contact_person_mobile where branch_serial = " + contact.branchId + " AND contact_id = " + contact.contactId;
                BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

                queryColumns.sql_string = 1;

                if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                    return false;

                if (commonQueriesSqlObject.rows.Count == 0)
                    contact.telephones.Add("");

                for (int j = 0; j < commonQueriesSqlObject.rows.Count; j++)
                    contact.telephones.Add(commonQueriesSqlObject.rows[j].sql_string[0]);
            }
            return true;
        }

        public bool UpdateAgent(int agentId, ref AGENCY_MACROS.AGENCY_FULL_INFO agent)
        {
//            String sqlQuery = @"UPDATE agency_name "+
//"SET agency_name =\'"+agent.agency_name+
//"\',added_by =(select employee_id from employees_info where [name] =  \'"+ agent.employee_name+"\')" +
//" where agency_serial = " + agentId;
//            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();
//            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
//                return false;

//            sqlQuery = @"UPDATE agency_address "+
//"SET [address] = (select id from districts where district = '"+agent.district+"\') ,"+
//"added_by = (select employee_id from employees_info where [name] =  \'" + agent.employee_name + "\')"+
//" where agency_serial = " + agentId;
//            queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();
//            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
//                return false;

//            sqlQuery = @"UPDATE agency_telephone "+
//"SET telephone_number = \'"+agent.telephone+
//"\',added_by = (select employee_id from employees_info where [name] =  '" + agent.employee_name + "\')"+
//"where branch_serial = " + agentId;
//            queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();
//            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
//                return false;

            return true;
        }

        public bool AddAgent(ref AGENCY_MACROS.AGENCY_FULL_INFO agent)
        {
            //// Get new serial
            //String sqlQuery = @" (select max(agency_serial)+1 from agency_name);";
            //BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();
            //queryColumns.sql_int = 1;
            //if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
            //    return false;
            //int newSerial = commonQueriesSqlObject.rows[0].sql_int[0];
            //// Get employee id
            //sqlQuery = @"select employee_id from employees_info where [name] =  '" + agent.employee_name+"\'";
            //if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
            //    return false;
            //int newEmployeeId = commonQueriesSqlObject.rows[0].sql_int[0];
            //// Add entry to agency_name table
            //sqlQuery = "INSERT INTO agency_name (agency_serial,agency_name,added_by)values(" + newSerial
            //    + ",\'" + agent.agency_name + "\'," + newEmployeeId + ");";
            //queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();
            //if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
            //    return false;
            //// Add entry to agency_address table
            //sqlQuery = "INSERT INTO agency_address (address_serial,agency_serial,address,added_by)" +
            //    " values(" + newSerial + "," + newSerial + "," + agent.addressId + "," + newEmployeeId + "); ";
            //if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
            //    return false;
            //// Add entry to agency_telephone table
            //sqlQuery = "INSERT INTO agency_telephone (branch_serial,telephone_number,added_by) values(" +
            //    newSerial + ",\'" + agent.telephone + "\'," + newEmployeeId + "); ";
            //if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
            //    return false;
            

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
