using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01electronics_erp
{
    public class WorkOrder : WorkOffer
    {
        protected const int ORDER_ID_IDENTIFIER_TOKEN				=	4;
        protected const int ORDER_ID_SERIAL_TOKEN					=	4;
        protected const int ORDER_ID_EMPLOYEE_INITIALS_TOKEN		=	9;
        protected const int ORDER_ID_DATE_TOKEN						=	8;

        protected const int BASE_ORDER_ID_DASH_SEPARATORS			=	3;
        
        protected const int BASE_ORDER_ID_LENGTH 					=	OFFER_ID_IDENTIFIER_TOKEN + OFFER_ID_SERIAL_TOKEN + OFFER_ID_EMPLOYEE_INITIALS_TOKEN + OFFER_ID_DATE_TOKEN + BASE_OFFER_ID_DASH_SEPARATORS;

        protected const int ORDER_ID_SERIAL_START_INDEX = OFFER_ID_IDENTIFIER_TOKEN + 1;
        protected const int ORDER_ID_EMPLOYEE_INITIALS_START_INDEX = OFFER_ID_SERIAL_START_INDEX + OFFER_ID_SERIAL_TOKEN + 1;
        protected const int ORDER_ID_DATE_START_INDEX = OFFER_ID_EMPLOYEE_INITIALS_START_INDEX + OFFER_ID_EMPLOYEE_INITIALS_TOKEN + 1;

        protected const String ORDER_ID_FORMAT = "ORDR-0001-XXXX.XXXX-DDMMYYYY";

        //WORK OFFER INFO
        char[] orderIdString;

        String orderId;

        int orderSerial;

        int orderStatusId;

        DateTime orderIssueDate;

        String orderNotes;

        String orderStatus;

        public WorkOrder()
        {
            orderStatusId = COMPANY_WORK_MACROS.OPEN_WORK_ORDER;

            orderIdString = new char[BASE_ORDER_ID_LENGTH + 1];
        }

        public WorkOrder(SQLServer mSqlDatabase)
        {
            new WorkOffer(mSqlDatabase);

            orderStatusId = COMPANY_WORK_MACROS.OPEN_WORK_ORDER;

            orderIdString = new char[BASE_ORDER_ID_LENGTH + 1];
        }

        //////////////////////////////////////////////////////////////////////
        //INITIALIZATION FUNCTIONS
        //////////////////////////////////////////////////////////////////////

        bool InitializeWorkOrderInfo(int mOrderSerial)
        {
            orderSerial = mOrderSerial;

            String sqlQueryPart1 = @"select work_orders.offer_serial, 
                                    work_orders.offer_version, 
									work_orders.offer_proposer, 
									orders_status.id, 

									work_orders.issue_date, 

									work_orders.order_id, 
									orders_status.order_status 
									from erp_system.dbo.work_orders 

									inner join erp_system.dbo.orders_status 
									on work_orders.order_status = orders_status.id 

							where work_orders.sales_person = ";

            String sqlQueryPart2 = " and work_orders.order_serial =  ";
            String sqlQueryPart3 = ";";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += GetSalesPersonId();
            sqlQuery += sqlQueryPart2;
            sqlQuery += orderSerial;
            sqlQuery += sqlQueryPart3;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 4;
            queryColumns.sql_datetime = 1;
            queryColumns.sql_string = 2;

            if (!sqlDatabase.GetRows(sqlQuery, queryColumns))
                return false;

            orderStatusId = sqlDatabase.rows[0].sql_int[3];

            orderIssueDate = sqlDatabase.rows[0].sql_datetime[0];

            orderId = sqlDatabase.rows[0].sql_string[0];
            orderStatus = sqlDatabase.rows[0].sql_string[1];

            if (salesPerson.GetEmployeeTeamId() == COMPANY_ORGANISATION_MACROS.SALES_TEAM_ID)
            {
                if (!InitializeSalesWorkOfferInfo(sqlDatabase.rows[0].sql_int[0], sqlDatabase.rows[0].sql_int[1], sqlDatabase.rows[0].sql_int[2]))
                    return false;
            }
            else if (salesPerson.GetEmployeeTeamId() == COMPANY_ORGANISATION_MACROS.TECHNICAL_OFFICE_TEAM_ID)
            {
                if (!InitializeTechnicalOfficeWorkOfferInfo(sqlDatabase.rows[0].sql_int[0], sqlDatabase.rows[0].sql_int[1], sqlDatabase.rows[0].sql_int[2]))
                    return false;
            }

            return true;
        }
        bool InitializeWorkOrderInfo(int mOrderSerial, int mSalesPersonId)
        {
            if (!InitializeSalesPersonInfo(mSalesPersonId))
                return false;
            if (!InitializeWorkOrderInfo(mOrderSerial))
                return false;

            return true;
        }


        //////////////////////////////////////////////////////////////////////
        //SET FUNCTIONS
        //////////////////////////////////////////////////////////////////////

        public void SetOrderStatus(int mOrderStatusId, String mOrderStatus)
        {
            orderStatusId = mOrderStatusId;
            orderStatus = mOrderStatus;
        }

        public void SetOrderNotes(String mNotes)
        {
            orderNotes = mNotes;
        }

        //////////////////////////////////////////////////////////////////////
        //RETURN FUNCTIONS
        //////////////////////////////////////////////////////////////////////

        public int GetOrderSerial()
        {
            return orderSerial;
        }

        public int GetOrderStatusId()
        {
            return orderStatusId;
        }

        public String GetOrderID()
        {
            return orderId;
        }

        public String GetOrderStatus()
        {
            return orderStatus;
        }

        public DateTime GetOrderIssueDate()
        {
            return orderIssueDate;
        }

        public String GetOrderNotes()
        {
            return orderNotes;
        }

        //////////////////////////////////////////////////////////////////////
        //MODIFICATION FUNCTIONS
        //////////////////////////////////////////////////////////////////////

        public void ConfirmOrderStatus()
        {
            offerStatusId = COMPANY_WORK_MACROS.CLOSED_WORK_ORDER;
            offerStatus = "Closed";
        }

        public bool IssueNewOrder()
        {
            SetOrderIssueDateToToday();

            if (!GetNewOrderSerial())
                return false;

            GetNewOrderID();

            return true;
        }

        public void ConfirmOrder()
        {
            ConfirmOrderStatus();
        }

        //////////////////////////////////////////////////////////////////////
        //GET ADDITIONAL INSERT DATA FUNCTIONS
        //////////////////////////////////////////////////////////////////////

        public void SetOrderIssueDateToToday()
        {
            orderIssueDate = commonFunctions.GetTodaysDate();
        }

        public bool GetNewOrderSerial()
        {
            String sqlQueryPart1 = "select max(work_orders.order_serial) from erp_system.dbo.work_orders where work_orders.sales_person = ";
            String sqlQueryPart2 = ";";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += GetSalesPersonId();
            sqlQuery += sqlQueryPart2;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;

            if (!sqlDatabase.GetRows(sqlQuery, queryColumns))
                return false;

            orderSerial = sqlDatabase.rows[0].sql_int[0] + 1;

            return true;
        }

        public void GetNewOrderID()
        {
            String employeeInitials = String.Empty;
            String orderDateID = commonFunctions.GetTimeID(commonFunctions.GetTodaysDate());
            String orderSerialString = orderSerial.ToString();

            int orderSerialLastIndex = orderSerialString.Length - 1;

            for (int i = 0; i < ORDER_ID_FORMAT.Length; i++)
                orderIdString[i] = ORDER_ID_FORMAT[i];

            if (GetSalesPersonTeamId() == COMPANY_ORGANISATION_MACROS.SALES_TEAM_ID)
                employeeInitials = salesPerson.GetEmployeeInitials();
            else if (GetSalesPersonTeamId() == COMPANY_ORGANISATION_MACROS.TECHNICAL_OFFICE_TEAM_ID)
                employeeInitials = assignedEngineer.GetEmployeeInitials();


            for (int i = orderSerialString.Length - 1; i >= 0; i--)
                orderIdString[ORDER_ID_IDENTIFIER_TOKEN + ORDER_ID_SERIAL_TOKEN - (orderSerialLastIndex - i)] = orderSerialString[i];

            for (int i = 0; i < employeeInitials.Length; i++)
                orderIdString[ORDER_ID_EMPLOYEE_INITIALS_START_INDEX + i] = employeeInitials[i];

            for (int i = 0; i < orderDateID.Length; i++)
                orderIdString[ORDER_ID_DATE_START_INDEX + i] = orderDateID[i];

            orderId = orderIdString.ToString();
        }
    }

    

}
