using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01electronics_erp
{
    public class RFQ
    {
        protected const int RFQ_ID_IDENTIFIER_TOKEN			=	3;
        protected const int RFQ_ID_SERIAL_TOKEN				=	4;
        protected const int RFQ_ID_EMPLOYEE_INITIALS_TOKEN	=	9;
        protected const int RFQ_ID_DATE_TOKEN				=	8;
        protected const int RFQ_ID_REVISION_OFFSET_TOKEN	=	3;
        protected const int RFQ_ID_REVISION_SERIAL_TOKEN     =   2;
        
        protected const int BASE_RFQ_ID_DASH_SEPARATORS        =    3;
        protected const int REVISED_RFQ_ID_DASH_SEPARATORS     =    BASE_RFQ_ID_DASH_SEPARATORS + 1;
        
        protected const int BASE_RFQ_ID_LENGTH = RFQ_ID_IDENTIFIER_TOKEN + RFQ_ID_SERIAL_TOKEN + RFQ_ID_EMPLOYEE_INITIALS_TOKEN + RFQ_ID_DATE_TOKEN + BASE_RFQ_ID_DASH_SEPARATORS;
        protected const int REVISED_RFQ_ID_LENGTH = BASE_RFQ_ID_LENGTH + RFQ_ID_REVISION_OFFSET_TOKEN + RFQ_ID_REVISION_SERIAL_TOKEN + 1;
        
        protected const int RFQ_ID_SERIAL_START_INDEX = RFQ_ID_IDENTIFIER_TOKEN + 1;
        protected const int RFQ_ID_EMPLOYEE_INITIALS_START_INDEX = RFQ_ID_SERIAL_START_INDEX + RFQ_ID_SERIAL_TOKEN + 1;
        protected const int RFQ_ID_DATE_START_INDEX = RFQ_ID_EMPLOYEE_INITIALS_START_INDEX + RFQ_ID_EMPLOYEE_INITIALS_TOKEN + 1;
        protected const int RFQ_ID_REVISION_SERIAL_START_INDEX = RFQ_ID_DATE_START_INDEX + RFQ_ID_DATE_TOKEN + RFQ_ID_REVISION_OFFSET_TOKEN + 1;
        
        protected const string RFQ_ID_FORMAT = "RFQ-0001-XXXX.XXXX-DDMMYYYY";
        protected const string REVISED_RFQ_ID_FORMAT = "RFQ-0001-XXXX.XXXX-DDMMYYYY";

        protected SQLServer sqlDatabase;

        protected CommonFunctions commonFunctions;
        protected CommonQueries commonQueries;

        protected String sqlQuery;


        //RFQ INFO
        protected char[] RFQIdString;
        protected char[] revisedRFQIdString;

        protected String RFQId;

        protected Contact contact;
        protected Employee assignedEngineer;
        protected Employee salesPerson;

        protected int rfqSerial;
        protected int rfqVersion;

        protected int rfqStatusId;

        protected COMPANY_WORK_MACROS.RFQ_PRODUCT_STRUCT[] RFQProductsList;
        protected bool[] RFQProductsValid;

        protected int contractTypeId;

        protected int rfqFailureReasonId;

        protected int noOfSavedRFQProducts;

        protected String contractType;

        protected DateTime rfqDeadlineDate;
        protected DateTime rfqIssueDate;
        protected DateTime rfqRejectionDate;

        protected String rfqNotes;

        protected String rfqStatus;
        protected String rfqFailureReason;

        public RFQ()
        {
            sqlDatabase = new SQLServer();

            commonFunctions = new CommonFunctions();
            commonQueries = new CommonQueries();

            assignedEngineer = new Employee();
            contact = new Contact();

            RFQProductsList = new COMPANY_WORK_MACROS.RFQ_PRODUCT_STRUCT[COMPANY_WORK_MACROS.MAX_RFQ_PRODUCTS];
            RFQProductsValid = new bool[COMPANY_WORK_MACROS.MAX_RFQ_PRODUCTS];

            RFQIdString = new char[BASE_RFQ_ID_LENGTH + 1];
            revisedRFQIdString = new char[REVISED_RFQ_ID_LENGTH + 1];

            ResetRFQInfo();
        }

        public RFQ(SQLServer mSqlDatabase)
        {
            sqlDatabase = mSqlDatabase;

            commonFunctions = new CommonFunctions();
            commonQueries = new CommonQueries(mSqlDatabase);

            assignedEngineer = new Employee(mSqlDatabase);
            contact = new Contact(mSqlDatabase);

            RFQProductsList = new COMPANY_WORK_MACROS.RFQ_PRODUCT_STRUCT[COMPANY_WORK_MACROS.MAX_RFQ_PRODUCTS];
            RFQProductsValid = new bool[COMPANY_WORK_MACROS.MAX_RFQ_PRODUCTS];

            RFQIdString = new char[BASE_RFQ_ID_LENGTH + 1];
            revisedRFQIdString = new char[REVISED_RFQ_ID_LENGTH + 1];

            ResetRFQInfo();
        }

        //////////////////////////////////////////////////////////////////////
        //INITIALIZATION FUNCTIONS
        //////////////////////////////////////////////////////////////////////

        public void SetDatabase(SQLServer mSqlDatabase)
        {
            sqlDatabase = mSqlDatabase;
            assignedEngineer.SetDatabase(mSqlDatabase);
            contact.SetDatabase(mSqlDatabase);
            commonQueries.SetDatabase(mSqlDatabase);
        }


        public bool InitializeRFQInfo(int mRFQSerial, int mRFQVersion)
        {
            ResetRFQInfo();

            rfqSerial = mRFQSerial;
            rfqVersion = mRFQVersion;

            String sqlQueryPart1 = @"with   get_product1_type	as	(select products_type.id as product1_type_id, 
                                                                            products_type.product_name as product1_type 
                                                                     from erp_system.dbo.products_type), 

                                            get_product1_brand as (select brands_type.id as product1_brand_id, 
                                            	                          brands_type.brand_name as product1_brand 
                                            	                   from erp_system.dbo.brands_type), 

							                get_product1_model as (select brands_models.model_id as product1_model_id, 
							                	                          brands_models.product_id as product1_type_id, 
							                	                          brands_models.brand_id as product1_brand_id, 
							                	                          brands_models.brand_model as product1_model 
                                                                    from erp_system.dbo.brands_models), 

							                get_product2_type as (select products_type.id as product2_type_id, 
							                	                        products_type.product_name as product2_type 
							                	                  from erp_system.dbo.products_type), 

							                get_product2_brand as (select brands_type.id as product2_brand_id, 
								                                          brands_type.brand_name as product2_brand 
								                                   from erp_system.dbo.brands_type), 

							                get_product2_model as (select brands_models.model_id as product2_model_id, 
							                	brands_models.product_id as product2_type_id, 
							                	brands_models.brand_id as product2_brand_id, 
							                	brands_models.brand_model as product2_model 
							                	from erp_system.dbo.brands_models), 

							                get_product3_type as (select products_type.id as product3_type_id, 
							                	products_type.product_name as product3_type 
							                	from erp_system.dbo.products_type), 

							                get_product3_brand as (select brands_type.id as product3_brand_id, 
							                	brands_type.brand_name as product3_brand 
							                	from erp_system.dbo.brands_type), 

							                get_product3_model as (select brands_models.model_id as product3_model_id, 
							                	brands_models.product_id as product3_type_id, 
							                	brands_models.brand_id as product3_brand_id, 
							                	brands_models.brand_model as product3_model 
							                	from erp_system.dbo.brands_models), 

							                get_product4_type as (select products_type.id as product4_type_id, 
							                	products_type.product_name as product4_type 
							                	from erp_system.dbo.products_type), 

							                get_product4_brand as (select brands_type.id as product4_brand_id, 
							                	brands_type.brand_name as product4_brand 
							                	from erp_system.dbo.brands_type), 

							                get_product4_model as (select brands_models.model_id as product4_model_id, 
							                	brands_models.product_id as product4_type_id, 
							                	brands_models.brand_id as product4_brand_id, 
							                	brands_models.brand_model as product4_model 
							                	from erp_system.dbo.brands_models) 

							select rfqs.assigned_engineer, 
									rfqs.branch_serial, 
									rfqs.contact_id, 

									rfqs.product1_type, 
									rfqs.product1_brand, 
									rfqs.product1_model, 
									rfqs.product1_quantity, 

									rfqs.product2_type, 
									rfqs.product2_brand, 
									rfqs.product2_model, 
									rfqs.product2_quantity, 

									rfqs.product3_type, 
									rfqs.product3_brand, 
									rfqs.product3_model, 
									rfqs.product3_quantity, 

									rfqs.product4_type, 
									rfqs.product4_brand, 
									rfqs.product4_model, 
									rfqs.product4_quantity, 

									rfqs.contract_type, 
									rfqs.rfq_status, 
									rfq_failure_reasons.id as failure_id, 

									rfqs.issue_date, 
									rfqs.deadline_date, 

									rfqs.rfq_id, 
									rfqs.rfq_notes, 

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
							and rfqs.rfq_version = unsuccessful_rfqs.rfq_version 
							and rfqs.sales_person = unsuccessful_rfqs.sales_person 

							left join erp_system.dbo.rfq_failure_reasons 
							on unsuccessful_rfqs.failure_reason = rfq_failure_reasons.id 

							where rfqs.sales_person = ";

            String sqlQueryPart2 = " and rfqs.rfq_serial = ";
            String sqlQueryPart3 = " and rfqs.rfq_version = ";
            String sqlQueryPart4 = ";";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += GetSalesPersonId();
            sqlQuery += sqlQueryPart2;
            sqlQuery += rfqSerial;
            sqlQuery += sqlQueryPart3;
            sqlQuery += rfqVersion;
            sqlQuery += sqlQueryPart4;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 22;
            queryColumns.sql_datetime = 2;
            queryColumns.sql_string = 17;

            if (!sqlDatabase.GetRows(sqlQuery, queryColumns))
                return false;

            int numericColumnsCount = 0;
            int stringColumnsCount = 0;

            //////////////////////////////////////////////////////////////////////
            //RFQ BASIC INFO
            //////////////////////////////////////////////////////////////////////

            int assigneeId = sqlDatabase.rows[0].sql_int[numericColumnsCount++];
            int branchSerial = sqlDatabase.rows[0].sql_int[numericColumnsCount++];
            int contactId = sqlDatabase.rows[0].sql_int[numericColumnsCount++];

            rfqIssueDate = sqlDatabase.rows[0].sql_datetime[0];
            rfqDeadlineDate = sqlDatabase.rows[0].sql_datetime[1];

            RFQId = sqlDatabase.rows[0].sql_string[stringColumnsCount++];
            rfqNotes = sqlDatabase.rows[0].sql_string[stringColumnsCount++];

            //////////////////////////////////////////////////////////////////////
            //PRODUCTS NAMES
            //////////////////////////////////////////////////////////////////////

            for (int i = 0; i < COMPANY_WORK_MACROS.MAX_RFQ_PRODUCTS; i++)
            {
                RFQProductsList[i].productType.typeName = sqlDatabase.rows[0].sql_string[stringColumnsCount++];
                RFQProductsList[i].productBrand.brandName = sqlDatabase.rows[0].sql_string[stringColumnsCount++];
                RFQProductsList[i].productModel.modelName = sqlDatabase.rows[0].sql_string[stringColumnsCount++];
            }

            //////////////////////////////////////////////////////////////////////
            //PRODUCT IDs
            //////////////////////////////////////////////////////////////////////

            for (int i = 0; i < COMPANY_WORK_MACROS.MAX_RFQ_PRODUCTS; i++)
            {
                RFQProductsList[i].productType.typeId = sqlDatabase.rows[0].sql_int[numericColumnsCount++];
                RFQProductsList[i].productBrand.brandId = sqlDatabase.rows[0].sql_int[numericColumnsCount++];
                RFQProductsList[i].productModel.modelId = sqlDatabase.rows[0].sql_int[numericColumnsCount++];
                RFQProductsList[i].productQuantity = sqlDatabase.rows[0].sql_int[numericColumnsCount++];
            }

            //////////////////////////////////////////////////////////////////////
            //RFQ ADDITIONAL INFO
            //////////////////////////////////////////////////////////////////////

            contractTypeId = sqlDatabase.rows[0].sql_int[numericColumnsCount++];
            contractType = sqlDatabase.rows[0].sql_string[stringColumnsCount++];

            rfqStatusId = sqlDatabase.rows[0].sql_int[numericColumnsCount++];
            rfqStatus = sqlDatabase.rows[0].sql_string[stringColumnsCount++];

            rfqFailureReasonId = sqlDatabase.rows[0].sql_int[numericColumnsCount++];
            rfqFailureReason = sqlDatabase.rows[0].sql_string[stringColumnsCount++];

            SetNoOfSavedRFQProducts();

            //////////////////////////////////////////////////////////////////////
            //RFQ BASIC INFO
            //////////////////////////////////////////////////////////////////////

            if (!InitializeAssignedEngineerInfo(assigneeId))
                return false;
            if (!InitializeBranchInfo(branchSerial))
                return false;
            if (!InitializeContactInfo(contactId))
                return false;

            return true;
        }
        public bool InitializeRFQInfo(int mRFQSerial, int mRFQVersion, int mSalesPersonId)
        {
            if (!InitializeSalesPersonInfo(mSalesPersonId))
                return false;
            if (!InitializeRFQInfo(mRFQSerial, mRFQVersion))
                return false;

            return true;
        }
        public bool InitializeRFQInfo(int mRFQSerial, int mRFQVersion, Employee mSalesPerson)
        {
            contact.SetSalesPerson(mSalesPerson);
            return InitializeRFQInfo(mRFQSerial, mRFQVersion);
        }

        public bool InitializeAssignedEngineerInfo(int mEmployeeId)
        {
            return assignedEngineer.InitializeEmployeeInfo(mEmployeeId);
        }
        public bool InitializeSalesPersonInfo(int mEmployeeId)
        {
            return contact.InitializeSalesPersonInfo(mEmployeeId);
        }
        public bool InitializeCompanyInfo(int mCompanySerial)
        {
            return contact.InitializeCompanyInfo(mCompanySerial);
        }
        public bool InitializeBranchInfo(int mAddressSerial)
        {
            return contact.InitializeBranchInfo(mAddressSerial);
        }
        public bool InitializeContactInfo(int mContactId)
        {
            return contact.InitializeContactInfo(mContactId);
        }

        public void CopyRFQ(RFQ sourceRFQ)
        {
            //SERVER INFO
            SQLServer sqlDatabase;

            //RFQ INFO
            RFQId = sourceRFQ.RFQId;
            RFQIdString = sourceRFQ.RFQIdString;
            revisedRFQIdString = sourceRFQ.revisedRFQIdString;

            contact = sourceRFQ.contact;
            assignedEngineer = sourceRFQ.assignedEngineer;
            salesPerson = sourceRFQ.salesPerson;

            rfqSerial = sourceRFQ.rfqSerial;
            rfqVersion = sourceRFQ.rfqVersion;

            rfqStatusId = sourceRFQ.rfqStatusId;

            for (int i = 0; i < COMPANY_WORK_MACROS.MAX_RFQ_PRODUCTS; i++)
                RFQProductsList[i] = sourceRFQ.RFQProductsList[i];

            for (int i = 0; i < COMPANY_WORK_MACROS.MAX_RFQ_PRODUCTS; i++)
                RFQProductsValid[i] = sourceRFQ.RFQProductsValid[i];

            contractTypeId = sourceRFQ.contractTypeId;

            rfqFailureReasonId = sourceRFQ.rfqFailureReasonId;

            noOfSavedRFQProducts = sourceRFQ.noOfSavedRFQProducts;
            contractType = sourceRFQ.contractType;

            rfqDeadlineDate = sourceRFQ.rfqDeadlineDate;
            rfqIssueDate = sourceRFQ.rfqIssueDate;
            rfqRejectionDate = sourceRFQ.rfqIssueDate;

            rfqNotes = sourceRFQ.rfqNotes;

            rfqStatus = sourceRFQ.rfqStatus;
            rfqFailureReason = sourceRFQ.rfqFailureReason;
        }

        //////////////////////////////////////////////////////////////////////
        //RESET FUNCTIONS
        //////////////////////////////////////////////////////////////////////

        public void ResetRFQInfo()
        {
            salesPerson = contact.GetSalesPerson();

            ResetRFQSerial();
            ResetRFQVersion();

            ResetRFQProduct1Type();
            ResetRFQProduct2Type();
            ResetRFQProduct3Type();
            ResetRFQProduct4Type();

            ResetRFQProduct1Brand();
            ResetRFQProduct2Brand();
            ResetRFQProduct3Brand();
            ResetRFQProduct4Brand();

            ResetRFQProduct1Model();
            ResetRFQProduct2Model();
            ResetRFQProduct3Model();
            ResetRFQProduct4Model();

            ResetRFQProduct1Quantity();
            ResetRFQProduct2Quantity();
            ResetRFQProduct3Quantity();
            ResetRFQProduct4Quantity();

            ResetRFQContractType();
            ResetRFQStatus();
        }

        public void ResetRFQSerial()
        {
            rfqSerial = 0;
        }
        public void ResetRFQVersion()
        {
            rfqVersion = 1;
        }

        public void ResetRFQProduct1Type()
        {
            RFQProductsList[0].productType.typeId = 0;
            RFQProductsList[0].productType.typeName = "Other";

            SetNoOfSavedRFQProducts();
        }
        public void ResetRFQProduct2Type()
        {
            RFQProductsList[1].productType.typeId = 0;
            RFQProductsList[1].productType.typeName = "Other";

            SetNoOfSavedRFQProducts();
        }
        public void ResetRFQProduct3Type()
        {
            RFQProductsList[2].productType.typeId = 0;
            RFQProductsList[2].productType.typeName = "Other";

            SetNoOfSavedRFQProducts();
        }
        public void ResetRFQProduct4Type()
        {
            RFQProductsList[3].productType.typeId = 0;
            RFQProductsList[3].productType.typeName = "Other";

            SetNoOfSavedRFQProducts();
        }
        public void ResetRFQProductType(int productNumber)
        {
            RFQProductsList[productNumber - 1].productType.typeId = 0;
            RFQProductsList[productNumber - 1].productType.typeName = "Other";

            SetNoOfSavedRFQProducts();
        }

        public void ResetRFQProduct1Brand()
        {
            RFQProductsList[0].productBrand.brandId = 0;
            RFQProductsList[0].productBrand.brandName = "Other";
        }
        public void ResetRFQProduct2Brand()
        {
            RFQProductsList[1].productBrand.brandId = 0;
            RFQProductsList[1].productBrand.brandName = "Other";
        }
        public void ResetRFQProduct3Brand()
        {
            RFQProductsList[2].productBrand.brandId = 0;
            RFQProductsList[2].productBrand.brandName = "Other";
        }
        public void ResetRFQProduct4Brand()
        {
            RFQProductsList[3].productBrand.brandId = 0;
            RFQProductsList[3].productBrand.brandName = "Other";
        }
        public void ResetRFQProductBrand(int productNumber)
        {
            RFQProductsList[productNumber - 1].productBrand.brandId = 0;
            RFQProductsList[productNumber - 1].productBrand.brandName = "Other";
        }

        public void ResetRFQProduct1Model()
        {
            RFQProductsList[0].productModel.modelId = 0;
            RFQProductsList[0].productModel.modelName = "Other";
        }
        public void ResetRFQProduct2Model()
        {
            RFQProductsList[1].productModel.modelId = 0;
            RFQProductsList[1].productModel.modelName = "Other";
        }
        public void ResetRFQProduct3Model()
        {
            RFQProductsList[2].productModel.modelId = 0;
            RFQProductsList[2].productModel.modelName = "Other";
        }
        public void ResetRFQProduct4Model()
        {
            RFQProductsList[3].productModel.modelId = 0;
            RFQProductsList[3].productModel.modelName = "Other";
        }
        public void ResetRFQProductModel(int productNumber)
        {
            RFQProductsList[productNumber - 1].productModel.modelId = 0;
            RFQProductsList[productNumber - 1].productModel.modelName = "Other";
        }

        public void ResetRFQProduct1Quantity()
        {
            RFQProductsList[0].productQuantity = 0;

            SetNoOfSavedRFQProducts();
        }
        public void ResetRFQProduct2Quantity()
        {
            RFQProductsList[1].productQuantity = 0;

            SetNoOfSavedRFQProducts();
        }
        public void ResetRFQProduct3Quantity()
        {
            RFQProductsList[2].productQuantity = 0;

            SetNoOfSavedRFQProducts();
        }
        public void ResetRFQProduct4Quantity()
        {
            RFQProductsList[3].productQuantity = 0;

            SetNoOfSavedRFQProducts();
        }
        public void ResetRFQProductQuantity(int productNumber)
        {
            RFQProductsList[productNumber - 1].productQuantity = 0;

            SetNoOfSavedRFQProducts();
        }

        public void ResetRFQContractType()
        {
            contractTypeId = 0;
            contractType = "0";
        }
        public void ResetRFQStatus()
        {
            rfqStatusId = COMPANY_WORK_MACROS.PENDING_RFQ;
            rfqStatus = "Pending";
        }

        //////////////////////////////////////////////////////////////////////
        //SET FUNCTIONS
        //////////////////////////////////////////////////////////////////////

        public void SetRFQSerial(int mRFQSerial)
        {
            rfqSerial = mRFQSerial;
        }
        public void SetRFQVersion(int mRFQVersion)
        {
            rfqVersion = mRFQVersion;
        }

        public void SetRFQProduct1Type(int mProduct1TypeId, String mProduct1Type)
        {
            RFQProductsList[0].productType.typeId = mProduct1TypeId;
            RFQProductsList[0].productType.typeName = mProduct1Type;

            SetNoOfSavedRFQProducts();
        }
        public void SetRFQProduct2Type(int mProduct2TypeId, String mProduct2Type)
        {
            RFQProductsList[1].productType.typeId = mProduct2TypeId;
            RFQProductsList[1].productType.typeName = mProduct2Type;

            SetNoOfSavedRFQProducts();
        }
        public void SetRFQProduct3Type(int mProduct3TypeId, String mProduct3Type)
        {
            RFQProductsList[2].productType.typeId = mProduct3TypeId;
            RFQProductsList[2].productType.typeName = mProduct3Type;

            SetNoOfSavedRFQProducts();
        }
        public void SetRFQProduct4Type(int mProduct4TypeId, String mProduct4Type)
        {
            RFQProductsList[3].productType.typeId = mProduct4TypeId;
            RFQProductsList[3].productType.typeName = mProduct4Type;

            SetNoOfSavedRFQProducts();
        }
        public void SetRFQProductType(int productNumber, int mProduct4TypeId, String mProduct4Type)
        {
            RFQProductsList[productNumber - 1].productType.typeId = mProduct4TypeId;
            RFQProductsList[productNumber - 1].productType.typeName = mProduct4Type;

            SetNoOfSavedRFQProducts();
        }

        public void SetRFQProduct1Brand(int mProduct1BrandId, String mProduct1Brand)
        {
            RFQProductsList[0].productBrand.brandId = mProduct1BrandId;
            RFQProductsList[0].productBrand.brandName = mProduct1Brand;
        }
        public void SetRFQProduct2Brand(int mProduct2BrandId, String mProduct2Brand)
        {
            RFQProductsList[1].productBrand.brandId = mProduct2BrandId;
            RFQProductsList[1].productBrand.brandName = mProduct2Brand;
        }
        public void SetRFQProduct3Brand(int mProduct3BrandId, String mProduct3Brand)
        {
            RFQProductsList[2].productBrand.brandId = mProduct3BrandId;
            RFQProductsList[2].productBrand.brandName = mProduct3Brand;
        }
        public void SetRFQProduct4Brand(int mProduct4BrandId, String mProduct4Brand)
        {
            RFQProductsList[3].productBrand.brandId = mProduct4BrandId;
            RFQProductsList[3].productBrand.brandName = mProduct4Brand;
        }
        public void SetRFQProductBrand(int productNumber, int mProduct4BrandId, String mProduct4Brand)
        {
            RFQProductsList[productNumber - 1].productBrand.brandId = mProduct4BrandId;
            RFQProductsList[productNumber - 1].productBrand.brandName = mProduct4Brand;
        }

        public void SetRFQProduct1Model(int mProduct1ModelId, String mProduct1Model)
        {
            RFQProductsList[0].productModel.modelId = mProduct1ModelId;
            RFQProductsList[0].productModel.modelName = mProduct1Model;
        }
        public void SetRFQProduct2Model(int mProduct2ModelId, String mProduct2Model)
        {
            RFQProductsList[1].productModel.modelId = mProduct2ModelId;
            RFQProductsList[1].productModel.modelName = mProduct2Model;
        }
        public void SetRFQProduct3Model(int mProduct3ModelId, String mProduct3Model)
        {
            RFQProductsList[2].productModel.modelId = mProduct3ModelId;
            RFQProductsList[2].productModel.modelName = mProduct3Model;
        }
        public void SetRFQProduct4Model(int mProduct4ModelId, String mProduct4Model)
        {
            RFQProductsList[3].productModel.modelId = mProduct4ModelId;
            RFQProductsList[3].productModel.modelName = mProduct4Model;
        }
        public void SetRFQProductModel(int productNumber, int mProduct4ModelId, String mProduct4Model)
        {
            RFQProductsList[productNumber - 1].productModel.modelId = mProduct4ModelId;
            RFQProductsList[productNumber - 1].productModel.modelName = mProduct4Model;
        }

        public void SetRFQProduct1Quantity(int mProduct1Quantity)
        {
            RFQProductsList[0].productQuantity = mProduct1Quantity;

            SetNoOfSavedRFQProducts();
        }
        public void SetRFQProduct2Quantity(int mProduct2Quantity)
        {
            RFQProductsList[1].productQuantity = mProduct2Quantity;

            SetNoOfSavedRFQProducts();
        }
        public void SetRFQProduct3Quantity(int mProduct3Quantity)
        {
            RFQProductsList[2].productQuantity = mProduct3Quantity;

            SetNoOfSavedRFQProducts();
        }
        public void SetRFQProduct4Quantity(int mProduct4Quantity)
        {
            RFQProductsList[3].productQuantity = mProduct4Quantity;

            SetNoOfSavedRFQProducts();
        }
        public void SetRFQProductQuantity(int productNumber, int mProduct4Quantity)
        {
            RFQProductsList[productNumber - 1].productQuantity = mProduct4Quantity;

            SetNoOfSavedRFQProducts();
        }

        public void SetNoOfSavedRFQProducts()
        {
            RFQProductsValid[0] = Convert.ToBoolean(RFQProductsList[0].productType.typeId) && Convert.ToBoolean(RFQProductsList[0].productQuantity);

            if (RFQProductsValid[0])
                noOfSavedRFQProducts = 1;
            else
                noOfSavedRFQProducts = 0;

            for (int i = 1; i < COMPANY_WORK_MACROS.MAX_RFQ_PRODUCTS; i++)
            {
                RFQProductsValid[i] = Convert.ToBoolean(RFQProductsValid[i - 1]) && Convert.ToBoolean(RFQProductsList[i].productType.typeId) && Convert.ToBoolean(RFQProductsList[i].productQuantity);

                if (RFQProductsValid[i])
                    noOfSavedRFQProducts = i + 1;
            }
        }

        public void SetRFQStatus(int mRFQStatusId, String mRFQStatus)
        {
            rfqStatusId = mRFQStatusId;
            rfqStatus = mRFQStatus;
        }
        public void SetRFQContractType(int mContractTypeId, String mContractType)
        {
            contractTypeId = mContractTypeId;
            contractType = mContractType;
        }

        public void SetRFQIssueDate(DateTime mIssueDate)
        {
            rfqIssueDate = mIssueDate;
        }
        public void SetRFQDeadlineDate(DateTime mDeadlineDate)
        {
            rfqDeadlineDate = mDeadlineDate;
        }
        public void SetRFQRejectionDate(DateTime mRejectionDate)
        {
            rfqRejectionDate = mRejectionDate;
        }

        public void SetRFQNotes(String mNotes)
        {
            rfqNotes = mNotes;
        }

        //////////////////////////////////////////////////////////////////////
        //RETURN FUNCTIONS
        //////////////////////////////////////////////////////////////////////

        public int GetRFQSerial()
        {
            return rfqSerial;
        }
        public int GetRFQVersion()
        {
            return rfqVersion;
        }

        public int GetRFQProduct1TypeId()
        {
            return RFQProductsList[0].productType.typeId;
        }
        public int GetRFQProduct2TypeId()
        {
            return RFQProductsList[1].productType.typeId;
        }
        public int GetRFQProduct3TypeId()
        {
            return RFQProductsList[2].productType.typeId;
        }
        public int GetRFQProduct4TypeId()
        {
            return RFQProductsList[3].productType.typeId;
        }
        public int GetRFQProductTypeId(int productNumber)
        {
            return RFQProductsList[productNumber - 1].productType.typeId;
        }

        public int GetRFQProduct1BrandId()
        {
            return RFQProductsList[0].productBrand.brandId;
        }
        public int GetRFQProduct2BrandId()
        {
            return RFQProductsList[1].productBrand.brandId;
        }
        public int GetRFQProduct3BrandId()
        {
            return RFQProductsList[2].productBrand.brandId;
        }
        public int GetRFQProduct4BrandId()
        {
            return RFQProductsList[3].productBrand.brandId;
        }
        public int GetRFQProductBrandId(int productNumber)
        {
            return RFQProductsList[productNumber - 1].productBrand.brandId;
        }

        public int GetRFQProduct1ModelId()
        {
            return RFQProductsList[0].productModel.modelId;
        }
        public int GetRFQProduct2ModelId()
        {
            return RFQProductsList[1].productModel.modelId;
        }
        public int GetRFQProduct3ModelId()
        {
            return RFQProductsList[2].productModel.modelId;
        }
        public int GetRFQProduct4ModelId()
        {
            return RFQProductsList[3].productModel.modelId;
        }
        public int GetRFQProductModelId(int productNumber)
        {
            return RFQProductsList[productNumber - 1].productModel.modelId;
        }

        public int GetRFQProduct1Quantity()
        {
            return RFQProductsList[0].productQuantity;
        }
        public int GetRFQProduct2Quantity()
        {
            return RFQProductsList[1].productQuantity;
        }
        public int GetRFQProduct3Quantity()
        {
            return RFQProductsList[2].productQuantity;
        }
        public int GetRFQProduct4Quantity()
        {
            return RFQProductsList[3].productQuantity;
        }
        public int GetRFQProductQuantity(int productNumber)
        {
            return RFQProductsList[productNumber - 1].productQuantity;
        }

        public int GetRFQContractTypeId()
        {
            return contractTypeId;
        }
        public int GetRFQStatusId()
        {
            return rfqStatusId;
        }

        public int GetRFQFailureReasonId()
        {
            return rfqFailureReasonId;
        }

        public int GetSalesPersonId()
        {
            return salesPerson.GetEmployeeId();
        }
        public int GetCompanySerial()
        {
            return contact.GetCompanySerial();
        }
        public int GetAddressSerial()
        {
            return contact.GetAddressSerial();
        }
        public int GetContactId()
        {
            return contact.GetContactId();
        }
        public int GetAssigneeId()
        {
            return assignedEngineer.GetEmployeeId();
        }
        public int GetSalesPersonTeamId()
        {
            return contact.GetSalesPersonTeamId();
        }

        char[] GetRFQIdString()
        {
            return RFQIdString;
        }
        char[] GetRevisedRFQIdString()
        {
            return revisedRFQIdString;
        }

        public String GetRFQID()
        {
            return RFQId;
        }

        public String GetAssigneeName()
        {
            return assignedEngineer.GetEmployeeName();
        }
        public String GetAssigneeTeam()
        {
            return assignedEngineer.GetEmployeeTeam();
        }
        public String GetAssigneePosition()
        {
            return assignedEngineer.GetEmployeePosition();
        }
        public String GetAssigneebusinessEmail()
        {
            return assignedEngineer.GetEmployeeBusinessEmail();
        }
        public String GetAssigneeCompanyPhone()
        {
            return assignedEngineer.GetEmployeeBusinessPhone();
        }

        public String GetSalesPersonName()
        {
            return salesPerson.GetEmployeeName();
        }
        public String GetSalesPersonTeam()
        {
            return salesPerson.GetEmployeeTeam();
        }
        public String GetSalesPersonPosition()
        {
            return salesPerson.GetEmployeePosition();
        }
        public String GetSalesPersonbusinessEmail()
        {
            return salesPerson.GetEmployeeBusinessEmail();
        }
        public String GetSalesPersonCompanyPhone()
        {
            return salesPerson.GetEmployeeBusinessPhone();
        }

        public String GetRFQProduct1Type()
        {
            return RFQProductsList[0].productType.typeName;
        }
        public String GetRFQProduct2Type()
        {
            return RFQProductsList[1].productType.typeName;
        }
        public String GetRFQProduct3Type()
        {
            return RFQProductsList[2].productType.typeName;
        }
        public String GetRFQProduct4Type()
        {
            return RFQProductsList[3].productType.typeName;
        }
        public String GetRFQProductType(int productNumber)
        {
            return RFQProductsList[productNumber - 1].productType.typeName;
        }

        public String GetRFQProduct1Brand()
        {
            return RFQProductsList[0].productBrand.brandName;
        }
        public String GetRFQProduct2Brand()
        {
            return RFQProductsList[1].productBrand.brandName;
        }
        public String GetRFQProduct3Brand()
        {
            return RFQProductsList[2].productBrand.brandName;
        }
        public String GetRFQProduct4Brand()
        {
            return RFQProductsList[3].productBrand.brandName;
        }
        public String GetRFQProductBrand(int productNumber)
        {
            return RFQProductsList[productNumber - 1].productBrand.brandName;
        }

        public String GetRFQProduct1Model()
        {
            return RFQProductsList[0].productModel.modelName;
        }
        public String GetRFQProduct2Model()
        {
            return RFQProductsList[1].productModel.modelName;
        }
        public String GetRFQProduct3Model()
        {
            return RFQProductsList[2].productModel.modelName;
        }
        public String GetRFQProduct4Model()
        {
            return RFQProductsList[3].productModel.modelName;
        }
        public String GetRFQProductModel(int productNumber)
        {
            return RFQProductsList[productNumber - 1].productModel.modelName;
        }

        public String GetRFQContractType()
        {
            return contractType;
        }
        public String GetRFQStatus()
        {
            return rfqStatus;
        }

        public String GetRFQFailureReason()
        {
            return rfqFailureReason;
        }

        DateTime GetRFQIssueDate()
        {
            return rfqIssueDate;
        }
        DateTime GetRFQDeadlineDate()
        {
            return rfqDeadlineDate;
        }
        DateTime GetRFQRejectionDate()
        {
            return rfqRejectionDate;
        }

        public String GetRFQNotes()
        {
            return rfqNotes;
        }

        //////////////////////////////////////////////////////////////////////
        //MODIFICATION FUNCTIONS
        //////////////////////////////////////////////////////////////////////

        public void SetRFQFailureReason(int mFailureReasonId, String mFailureReason)
        {
            rfqFailureReasonId = mFailureReasonId;
            rfqFailureReason = mFailureReason;
        }

        public void ConfirmRFQStatus()
        {
            rfqStatusId = COMPANY_WORK_MACROS.CONFIRMED_RFQ;
            rfqStatus = "Offered";
        }
        public void RejectRFQStatus()
        {
            rfqStatusId = COMPANY_WORK_MACROS.REJECTED_RFQ;
            rfqStatus = "Rejected";
        }

        public bool IssueNewRFQ()
        {
            SetRFQIssueDateToToday();

            if (!GetNewRFQSerial())
                return false;

            GetNewRFQID();

            return true;
        }
        public bool ReviseRFQ()
        {
            SetRFQIssueDateToToday();

            if (!GetNewRFQVersion())
                return false;

            GetNewRFQID();

            return true;
        }
        public void ConfirmRFQ()
        {
            ConfirmRFQStatus();
        }
        public void RejectRFQ(int mFailureReasonId, String mFailureReason)
        {
            SetRFQRejectionDateToToday();
            RejectRFQStatus();
            SetRFQFailureReason(mFailureReasonId, mFailureReason);
        }

        //////////////////////////////////////////////////////////////////////
        //GET ADDITIONAL INSERT DATA FUNCTIONS
        //////////////////////////////////////////////////////////////////////

        public void SetRFQIssueDateToToday()
        {
            rfqIssueDate = commonFunctions.GetTodaysDate();
        }
        public void SetRFQRejectionDateToToday()
        {
            rfqRejectionDate = commonFunctions.GetTodaysDate();
        }

        public bool GetNewRFQSerial()
        {
            String sqlQueryPart1 = "select max(rfqs.rfq_serial) from erp_system.dbo.rfqs where rfqs.sales_person = ";
            String sqlQueryPart2 = ";";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += GetSalesPersonId();
            sqlQuery += sqlQueryPart2;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;

            if (!sqlDatabase.GetRows(sqlQuery, queryColumns))
                return false;

            rfqSerial = sqlDatabase.rows[0].sql_int[0] + 1;

            return true;
        }
        public bool GetNewRFQVersion()
        {
            String sqlQueryPart1 = "select max(rfqs.rfq_version) from erp_system.dbo.rfqs where rfqs.sales_person = ";
            String sqlQueryPart2 = " and rfqs.rfq_serial = ";
            String sqlQueryPart3 = ";";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += GetSalesPersonId();
            sqlQuery += sqlQueryPart2;
            sqlQuery += rfqSerial;
            sqlQuery += sqlQueryPart3;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;

            if (!sqlDatabase.GetRows(sqlQuery, queryColumns))
                return false;

            rfqVersion = sqlDatabase.rows[0].sql_int[0] + 1;

            return true;
        }

        public void GetNewRFQID()
        {
            String rfqDateID = commonFunctions.GetTimeID(commonFunctions.GetTodaysDate());
            String employeeInitials = salesPerson.GetEmployeeInitials();
            String rfqSerialString = rfqSerial.ToString();

            int rfqSerialLastIndex = rfqSerialString.Length - 1;


            for (int i = 0; i < RFQ_ID_FORMAT.Length; i++)
                RFQIdString[i] = RFQ_ID_FORMAT[i];

            for (int i = 0; i < REVISED_RFQ_ID_FORMAT.Length; i++)
                revisedRFQIdString[i] = REVISED_RFQ_ID_FORMAT[i];

            for (int i = rfqSerialLastIndex; i >= 0; i--)
            {
                RFQIdString[RFQ_ID_IDENTIFIER_TOKEN + RFQ_ID_SERIAL_TOKEN - (rfqSerialLastIndex - i)] = rfqSerialString[i];
                revisedRFQIdString[RFQ_ID_IDENTIFIER_TOKEN + RFQ_ID_SERIAL_TOKEN - (rfqSerialLastIndex - i)] = rfqSerialString[i];
            }

            for (int i = 0; i < employeeInitials.Length; i++)
            {
                RFQIdString[RFQ_ID_EMPLOYEE_INITIALS_START_INDEX + i] = employeeInitials[i];
                revisedRFQIdString[RFQ_ID_EMPLOYEE_INITIALS_START_INDEX + i] = employeeInitials[i];
            }

            for (int i = 0; i < rfqDateID.Length; i++)
            {
                RFQIdString[RFQ_ID_DATE_START_INDEX + i] = rfqDateID[i];
                revisedRFQIdString[RFQ_ID_DATE_START_INDEX + i] = rfqDateID[i];
            }

            revisedRFQIdString[RFQ_ID_REVISION_SERIAL_START_INDEX] = (char)((rfqVersion - 1) / 10);
            revisedRFQIdString[RFQ_ID_REVISION_SERIAL_START_INDEX + 1] = (char)((rfqVersion - 1) % 10);

            if (rfqVersion > 1)
                RFQId = revisedRFQIdString.ToString();
            else
                RFQId = RFQIdString.ToString();

        }



    }
}
