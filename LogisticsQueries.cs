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
        public bool GetAgentsSerialsAndNames(ref List<AGENT_MACROS.AGENT_STRUCT> returnVector)
        {
            returnVector.Clear();

            String sqlQueryPart1 = "select agent_serial, agent_name from erp_system.dbo.agent_name order by agent_name;";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;
            queryColumns.sql_string = 1;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            for (int i = 0; i < commonQueriesSqlObject.rows.Count; i++)
            {
                AGENT_MACROS.AGENT_STRUCT tempItem;

                tempItem.agent_serial = commonQueriesSqlObject.rows[i].sql_int[0];
                tempItem.agent_name = commonQueriesSqlObject.rows[i].sql_string[0];

                returnVector.Add(tempItem);
            }

            return true;
        }


        public bool GetFullAgentInfo(int agentId,ref AGENT_MACROS.AGENT_FULL_INFO agent)
        {
            String sqlQueryPart1 = @"select agent_name.agent_name,[name],district,agent_telephone.telephone_number
from agent_name,agent_address,districts,employees_info,agent_telephone
where agent_address.[address] = districts.id 
AND agent_name.added_by = employees_info.employee_id
AND agent_name.agent_serial = agent_address.agent_serial
AND agent_telephone.branch_serial = agent_address.address_serial
AND agent_name.agent_serial = " + agentId;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_string = 4;

            if (!commonQueriesSqlObject.GetRows(sqlQuery, queryColumns))
                return false;

            agent.agent_name = commonQueriesSqlObject.rows[0].sql_string[0];
            agent.employee_name = commonQueriesSqlObject.rows[0].sql_string[1];
            agent.district = commonQueriesSqlObject.rows[0].sql_string[2];
            agent.telephone = commonQueriesSqlObject.rows[0].sql_string[3];

            return true;
        }

    }
}
