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

    }
}
