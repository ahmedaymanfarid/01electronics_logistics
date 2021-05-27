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
    public class COMPANY_WORK_MACROS
    {
        //VISIT PLANS MACROS
        public const int NO_OF_VISITS_STATUS = 4;
        public const int PENDING_VISIT_PLAN = 1;
        public const int CANCELLED_VISIT_PLAN = 2;
        public const int RESCHEDULED_VISIT_PLAN = 3;
        public const int CONFIRMED_VISIT_PLAN = 4;

        //PRODUCTS TYPES IDS
        public const int BATTERY_ID = 1;
        public const int COMMERCIAL_UPS_ID = 2;
        public const int GENSET_ID = 4;
        public const int INDUSTRIAL_UPS_ID = 5;
        public const int STABILIZER_ID = 9;

        //PRODUCTS OFFERS FORM PATHS
        public const String MODELS_OFFERS_PATH = "erp_system/products_offer_models/generic/";
        public const String MODELS_PHOTOS_PATH = "erp_system/products_photos/";
        public const String OFFER_FILES_PATH = "erp_system/work_offers/";

        public const String YEARLY_SALES_EMPLOYEE_STATISTICS_EXCEL_PATH = "erp_system/statistics_sheet_models/yearly_sales_employee_statistics.xls";
        public const String QUARTERLY_SALES_EMPLOYEE_STATISTICS_EXCEL_PATH = "erp_system/statistics_sheet_models/quarterly_sales_employee_statistics.xls";
        public const String YEARLY_TECH_EMPLOYEE_STATISTICS_EXCEL_PATH = "erp_system/statistics_sheet_models/yearly_tech_employee_statistics.xls";
        public const String QUARTERLY_TECH_EMPLOYEE_STATISTICS_EXCEL_PATH = "erp_system/statistics_sheet_models/quarterly_tech_employee_statistics.xls";

        //WORK ORDERS MARCROS
        public const int OPEN_WORK_ORDER = 1;
        public const int CLOSED_WORK_ORDER = 2;

        public const int TOTALLY_COLLECTED = 1;
        public const int PARTIALLY_COLLECTED = 2;
        public const int NONE_COLLECTED = 3;

        //WORK OFFERS MACROS                    
        public const int NO_OF_OFFERS_STATUS = 3;
        public const int FAILED_WORK_OFFER = 1;
        public const int CONFIRMED_WORK_OFFER = 2;
        public const int PENDING_WORK_OFFER = 3;

        public const int MAx_NUMBER_OF_OFFER_SERIALS = 10000;
        public const int MAx_NUMBER_OF_OFFER_VERSIONS = 100;

        //RFQS MACROS
        public const int NO_OF_RFQS_STATUS = 3;
        public const int PENDING_RFQ = 1;
        public const int CONFIRMED_RFQ = 2;
        public const int REJECTED_RFQ = 3;


        public const int MAX_RFQ_PRODUCTS = 4;
        public const int MAX_OFFER_PRODUCTS = MAX_RFQ_PRODUCTS;
        public const int MAX_ORDER_PRODUCTS = MAX_OFFER_PRODUCTS;

        public struct PRODUCT_STRUCT
        {
            public String typeName;
            public int typeId;
        };

        public struct BRAND_STRUCT
        {
            public String brandName;
            public int brandId;
        };

        public struct MODEL_STRUCT
        {
            public String modelName;
            public int modelId;
        };

        public struct RFQ_PRODUCT_STRUCT : IComparable<RFQ_PRODUCT_STRUCT>
        {
            public PRODUCT_STRUCT productType;
            public BRAND_STRUCT productBrand;
            public MODEL_STRUCT productModel;

            public int productNumber;
            public int productQuantity;

            public int CompareTo(RFQ_PRODUCT_STRUCT other)
            {
                return this.productNumber.CompareTo(other.productNumber);

                throw new NotImplementedException();
            }
        };

        public struct OFFER_PRODUCT_STRUCT : IComparable<OFFER_PRODUCT_STRUCT>
        {
            public PRODUCT_STRUCT productType;
            public BRAND_STRUCT productBrand;
            public MODEL_STRUCT productModel;

            public int productNumber;
            public int productQuantity;
            public int productPrice;

            public int CompareTo(OFFER_PRODUCT_STRUCT other)
            {
                return this.productNumber.CompareTo(other.productNumber);

                throw new NotImplementedException();
            }
        };

        public struct ORDER_PRODUCT_STRUCT : IComparable<ORDER_PRODUCT_STRUCT>
        {
            public PRODUCT_STRUCT productType;
            public BRAND_STRUCT productBrand;
            public MODEL_STRUCT productModel;

            public int productNumber;
            public int productQuantity;
            public int productPrice;

            public int CompareTo(ORDER_PRODUCT_STRUCT other)
            {
                return this.productNumber.CompareTo(other.productNumber);

                throw new NotImplementedException();
            }
        };
        public struct RFQ_BASIC_STRUCT
        {
            public String rfq_id;
            public int rfq_serial;
            public int rfq_version;
        };

        public struct VISIT_PURPOSE_STRUCT
        {
            public int purpose_id;
            public String purpose_name;
        };

        public struct VISIT_RESULT_STRUCT
        {
            public int result_id;
            public String result_name;
        };

        public struct CALL_PURPOSE_STRUCT
        {
            public int purpose_id;
            public String purpose_name;
        };

        public struct CALL_RESULT_STRUCT
        {
            public int result_id;
            public String result_name;
        };

        public struct FAILURE_REASON_STRUCT
        {
            public int reason_id;
            public String reason_name;
        };

        public struct RFQ_MAX_STRUCT
        {
            public int sales_person_id;
            public int rfq_serial;
            public int rfq_version;

            public int assignee_id;

            public int company_serial;
            public int branch_serial;
            public int contact_id;

            public int rfq_status_id;
            public int failure_reason_id;
            public int contract_type_id;

            public String rfq_id;

            public String issue_date;
            public String deadline_date;

            public String rfq_status;
            public String failure_reason;
            public String contract_type;

            public String sales_person_name;
            public String assignee_name;

            public String company_name;
            public String contact_name;

            public List<RFQ_PRODUCT_STRUCT> products;
        };

        public struct WORK_OFFER_MAX_STRUCT
        {
            public int offer_proposer_id;
            public int offer_serial;
            public int offer_version;

            public int sales_person_id;

            public int company_serial;
            public int branch_serial;
            public int contact_id;

            public int offer_status_id;
            public int failure_reason_id;
            public int contract_type_id;

            public String offer_id;

            public String issue_date;

            public String offer_status;
            public String failure_reason;
            public String contract_type;

            public String sales_person_name;
            public String offer_proposer_name;

            public String company_name;
            public String contact_name;

            public List<OFFER_PRODUCT_STRUCT> products;
        };

        public struct WORK_ORDER_MAX_STRUCT
        {
            public int sales_person_id;
            public int order_serial;

            public int offer_proposer_id;

            public int company_serial;
            public int branch_serial;
            public int contact_id;

            public int order_status_id;
            public int contract_type_id;

            public String order_id;

            public String issue_date;

            public String order_status;
            public String contract_type;

            public String sales_person_name;
            public String offer_proposer_name;

            public String company_name;
            public String contact_name;

            public List<ORDER_PRODUCT_STRUCT> products;
        };

        public struct CLIENT_VISIT_STRUCT
        {
            public int sales_person_id;
            public int visit_serial;

            public int visit_purpose_id;
            public int visit_result_id;

            public String issue_date;
            public String visit_date;

            public String sales_person_name;

            public String company_name;
            public String contact_name;

            public String visit_purpose;
            public String visit_result;
        };

        public struct CLIENT_CALL_STRUCT
        {
            public int sales_person_id;
            public int call_serial;

            public int call_purpose_id;
            public int call_result_id;

            public String issue_date;
            public String call_date;

            public String sales_person_name;

            public String company_name;
            public String contact_name;

            public String call_purpose;
            public String call_result;
        };
    }
}

