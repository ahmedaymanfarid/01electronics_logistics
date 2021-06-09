using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01electronics_erp
{
    public class WorkOffer : RFQ
    {
        public const int OFFER_ID_IDENTIFIER_TOKEN = 4;
        public const int OFFER_ID_SERIAL_TOKEN = 4;
        public const int OFFER_ID_EMPLOYEE_INITIALS_TOKEN = 9;
        public const int OFFER_ID_DATE_TOKEN = 8;
        public const int OFFER_ID_REVISION_OFFSET_TOKEN = 3;
        public const int OFFER_ID_REVISION_SERIAL_TOKEN = 2;

        public const int BASE_OFFER_ID_DASH_SEPARATORS = 3;
        public const int REVISED_OFFER_ID_DASH_SEPARATORS = BASE_OFFER_ID_DASH_SEPARATORS + 1;

        public const int BASE_OFFER_ID_LENGTH = OFFER_ID_IDENTIFIER_TOKEN + OFFER_ID_SERIAL_TOKEN + OFFER_ID_EMPLOYEE_INITIALS_TOKEN + OFFER_ID_DATE_TOKEN + BASE_OFFER_ID_DASH_SEPARATORS;
        public const int REVISED_OFFER_ID_LENGTH = BASE_OFFER_ID_LENGTH + OFFER_ID_REVISION_OFFSET_TOKEN + OFFER_ID_REVISION_SERIAL_TOKEN + 1;

        public const int OFFER_ID_SERIAL_START_INDEX = OFFER_ID_IDENTIFIER_TOKEN + 1;
        public const int OFFER_ID_EMPLOYEE_INITIALS_START_INDEX = OFFER_ID_SERIAL_START_INDEX + OFFER_ID_SERIAL_TOKEN + 1;
        public const int OFFER_ID_DATE_START_INDEX = OFFER_ID_EMPLOYEE_INITIALS_START_INDEX + OFFER_ID_EMPLOYEE_INITIALS_TOKEN + 1;
        public const int OFFER_ID_REVISION_SERIAL_START_INDEX = OFFER_ID_DATE_START_INDEX + OFFER_ID_DATE_TOKEN + OFFER_ID_REVISION_OFFSET_TOKEN + 1;

        private const string OFFER_ID_FORMAT = "OFFR-0001-XXXX.XXXX-DDMMYYYY";
        private const string REVISED_OFFER_ID_FORMAT = "OFFR-0001-XXXX.XXXX-DDMMYYYY-REV01";

        //WORK OFFER INFO
        protected char[] offerIdString;
        protected char[] revisedOfferIdString;

        protected String offerId;

        protected Employee offerProposer;

        protected int offerSerial;
        protected int offerVersion;

        protected int offerStatusId;
        protected int offerFailureReasonId;

        //////////////////////////////////////////////////////////////////////
        //PAYMENT CONDITIONS
        //////////////////////////////////////////////////////////////////////

        protected String priceCurrency;

        protected int priceCurrencyId;

        protected int percentDownPayment;
        protected int percentOnDelivery;
        protected int percentOnInstallation;

        protected int totalPriceValue;
        protected int priceValueDownPayment;
        protected int priceValueOnDelivery;
        protected int priceValueOnInstallation;

        //////////////////////////////////////////////////////////////////////
        //DRAWINGS CONIDITIONS
        //////////////////////////////////////////////////////////////////////

        protected bool hasDrawings;

        protected int drawingDeadlineMinimum;
        protected int drawingDeadlineMaximum;
        protected int drawingDeadlineTimeUnitId;

        protected String drawingDeadlineTimeUnit;

        //////////////////////////////////////////////////////////////////////
        //DELIVERY CONIDITIONS
        //////////////////////////////////////////////////////////////////////

        protected int deliveryTimeMinimum;
        protected int deliveryTimeMaximum;
        protected int deliveryTimeUnitId;

        protected int deliveryPointId;

        protected String deliveryTimeUnit;
        protected String deliveryPoint;

        //////////////////////////////////////////////////////////////////////
        //ADDITIONAL OFFER INFO
        //////////////////////////////////////////////////////////////////////

        protected int modifiedContractTypeId;

        protected int warrantyPeriod;
        protected int warrantyPeriodTimeUnitId;

        protected int offerValidityPeriod;
        protected int offerValidityTimeUnitId;

        protected String warrantyPeriodTimeUnit;
        protected String offerValidityTimeUnit;

        protected String offerLocalPath;
        protected String offerServerPath;

        //////////////////////////////////////////////////////////////////////
        //PRODUCTS TYPES' AND BRANDS' NAMES
        //////////////////////////////////////////////////////////////////////

        protected COMPANY_WORK_MACROS.OFFER_PRODUCT_STRUCT[] OfferProductsList;

        protected bool modifiedProduct1Valid;
        protected bool modifiedProduct2Valid;
        protected bool modifiedProduct3Valid;
        protected bool modifiedProduct4Valid;

        protected bool[] OfferProductsValid;

        protected int noOfSavedOfferProducts;

        //////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////

        protected String modifiedContractType;

        protected DateTime offerIssueDate;
        protected DateTime offerRejectionDate;

        protected String offerNotes;

        protected String offerStatus;
        protected String offerFailureReason;

        public WorkOffer()
        {
            new RFQ();

            offerProposer = assignedEngineer;
            OfferProductsList = new COMPANY_WORK_MACROS.OFFER_PRODUCT_STRUCT[COMPANY_WORK_MACROS.MAX_OFFER_PRODUCTS];
            OfferProductsValid = new bool[COMPANY_WORK_MACROS.MAX_OFFER_PRODUCTS];

            offerIdString = new char[BASE_OFFER_ID_LENGTH + 1];
            revisedOfferIdString = new char[REVISED_OFFER_ID_LENGTH + 1];
        }

        public WorkOffer(SQLServer mSqlDatabase)
        {
            new RFQ(mSqlDatabase);

            offerProposer = assignedEngineer;
            OfferProductsList = new COMPANY_WORK_MACROS.OFFER_PRODUCT_STRUCT[COMPANY_WORK_MACROS.MAX_OFFER_PRODUCTS];
            OfferProductsValid = new bool[COMPANY_WORK_MACROS.MAX_OFFER_PRODUCTS];

            offerIdString = new char[BASE_OFFER_ID_LENGTH + 1];
            revisedOfferIdString = new char[REVISED_OFFER_ID_LENGTH + 1];
        }

        //////////////////////////////////////////////////////////////////////
        //INITIALIZATION FUNCTIONS
        //////////////////////////////////////////////////////////////////////

        public void LinkRFQInfo()
        {
            //////////////////////////////////////////////////////////////////////
            //PRODUCTS 
            //////////////////////////////////////////////////////////////////////

            for (int i = 0; i < COMPANY_WORK_MACROS.MAX_OFFER_PRODUCTS; i++)
            {
                OfferProductsList[i].productType.typeId = RFQProductsList[i].productType.typeId;
                OfferProductsList[i].productBrand.brandId = RFQProductsList[i].productBrand.brandId;
                OfferProductsList[i].productModel.modelId = RFQProductsList[i].productModel.modelId;

                OfferProductsList[i].productType.typeName = RFQProductsList[i].productType.typeName;
                OfferProductsList[i].productBrand.brandName = RFQProductsList[i].productBrand.brandName;
                OfferProductsList[i].productModel.modelName = RFQProductsList[i].productModel.modelName;

                OfferProductsList[i].productQuantity = RFQProductsList[i].productQuantity;
            }

            //////////////////////////////////////////////////////////////////////
            //PRODUCTS QUANTITY
            //////////////////////////////////////////////////////////////////////

            noOfSavedOfferProducts = noOfSavedRFQProducts;

            modifiedContractType = contractType;

            modifiedContractTypeId = contractTypeId;

        }

        public bool InitializeSalesWorkOfferInfo(int mOfferSerial, int mOfferVersion)
        {
            ResetWorkOfferInfo(COMPANY_ORGANISATION_MACROS.SALES_TEAM_ID);

            offerSerial = mOfferSerial;
            offerVersion = mOfferVersion;

            String sqlQueryPart1 = @"select work_offers_rfqs.sales_person, 
        work_offers_rfqs.rfq_serial, 
		work_offers_rfqs.rfq_version
        from erp_system.dbo.work_offers
        inner join erp_system.dbo.work_offers_rfqs
        on work_offers.offer_proposer = work_offers_rfqs.offer_proposer

        and work_offers.offer_serial = work_offers_rfqs.offer_serial

        and work_offers.offer_version = work_offers_rfqs.offer_version

        where work_offers.offer_proposer = ";

            String sqlQueryPart2 = " and work_offers.offer_serial = ";
            String sqlQueryPart3 = " and work_offers.offer_version = ";
            String sqlQueryPart4 = ";";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += GetOfferProposerId();
            sqlQuery += sqlQueryPart2;
            sqlQuery += offerSerial;
            sqlQuery += sqlQueryPart3;
            sqlQuery += offerVersion;
            sqlQuery += sqlQueryPart4;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 3;

            if (!sqlDatabase.GetRows(sqlQuery, queryColumns))
                return false;

            if (!InitializeRFQInfo(sqlDatabase.rows[0].sql_int[1], sqlDatabase.rows[0].sql_int[2], sqlDatabase.rows[0].sql_int[0]))
                return false;

            LinkRFQInfo();

            String sqlQueryPart5 = @"with get_product_type		as	(	select products_type.id as product_type_id, 
		products_type.product_name as product_type 
		from erp_system.dbo.products_type 
		), 
		get_product_brand as (select brands_type.id as product_brand_id, 
		brands_type.brand_name as product_brand 
		from erp_system.dbo.brands_type 
		), 
		get_product_model as (select brands_models.model_id as product_model_id, 
		brands_models.product_id as product_type_id, 
		brands_models.brand_id as product_brand_id, 
		brands_models.brand_model as product_model 
		from erp_system.dbo.brands_models 
		), 
		get_price_currency as (select currencies_type.id as price_currency_id, 
		currencies_type.currency as price_currency 
		from erp_system.dbo.currencies_type 
		), 
		get_delivery_time_unit as (select time_units.id as delivery_unit_id, 
		time_units.time_unit as delivery_point_unit 
		from erp_system.dbo.time_units 
		), 
		get_warranty_time_unit as (select time_units.id as warranty_unit_id, 
		time_units.time_unit as warranty_unit 
		from erp_system.dbo.time_units 
		), 
		get_validity_time_unit as (select time_units.id as validity_unit_id, 
		time_units.time_unit as validity_unit 
		from erp_system.dbo.time_units 
		) 
		
		select get_price_currency.price_currency_id, 
		
		work_offers.percent_down_payment, 
		work_offers.percent_on_delivery, 
		work_offers.percent_on_installation, 
		
		work_offers.delivery_period_minimum, 
		work_offers.delivery_period_maximum, 
		get_delivery_time_unit.delivery_unit_id, 
		
		delivery_points.id as delivery_point_id, 
		contracts_type.id as contract_id, 
		
		work_offers.warranty_period, 
		get_warranty_time_unit.warranty_unit_id, 
		
		work_offers.offer_validity_period, 
		get_validity_time_unit.validity_unit_id, 
		
		offers_status.id as status_id, 
		
		work_offers_products_info.product_number, 
		work_offers_products_info.product_type, 
		work_offers_products_info.product_brand, 
		work_offers_products_info.product_model, 
		work_offers_products_info.product_quantity, 
		work_offers_products_info.product_price, 
		
		work_offers.issue_date, 
		
		work_offers.offer_id, 
		work_offers.offer_notes, 
		
		get_price_currency.price_currency, 
		
		get_delivery_time_unit.delivery_point_unit, 
		delivery_points.delivery_point as delivery_point, 
		
		contracts_type.contract_type, 
		
		get_warranty_time_unit.warranty_unit, 
		get_validity_time_unit.validity_unit, 
		
		offers_status.offer_status, 
		
		get_product_type.product_type, 
		get_product_brand.product_brand, 
		get_product_model.product_model,
		
		work_offers.isDrawing
		
		from erp_system.dbo.work_offers 
		
		inner join erp_system.dbo.work_offers_rfqs 
		on work_offers.offer_proposer = work_offers_rfqs.offer_proposer 
		and work_offers.offer_serial = work_offers_rfqs.offer_serial 
		and work_offers.offer_version = work_offers_rfqs.offer_version 
		
		inner join erp_system.dbo.rfqs 
		on work_offers_rfqs.sales_person = rfqs.sales_person 
		and work_offers_rfqs.rfq_serial = rfqs.rfq_serial 
		and work_offers_rfqs.rfq_version = rfqs.rfq_version 
		
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
		
		inner join get_delivery_time_unit 
		on work_offers.delivery_time_unit = get_delivery_time_unit.delivery_unit_id 
		
		inner join erp_system.dbo.delivery_points 
		on work_offers.delivery_point = delivery_points.id 
		
		inner join erp_system.dbo.contracts_type 
		on work_offers.contract_type = contracts_type.id 
		
		inner join get_warranty_time_unit 
		on work_offers.warranty_time_unit = get_warranty_time_unit.warranty_unit_id 
		
		inner join get_validity_time_unit 
		on work_offers.offer_validity_unit = get_validity_time_unit.validity_unit_id 
		
		inner join erp_system.dbo.offers_status 
		on work_offers.offer_status = offers_status.id 
		
		inner join get_price_currency 
		on work_offers.price_currency = get_price_currency.price_currency_id 
		
		where work_offers.offer_proposer = ";
            String sqlQueryPart6 = " and work_offers.offer_serial = ";
            String sqlQueryPart7 = " and work_offers.offer_version = ";
            String sqlQueryPart8 = ";";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart5;
            sqlQuery += GetOfferProposerId();
            sqlQuery += sqlQueryPart6;
            sqlQuery += offerSerial;
            sqlQuery += sqlQueryPart7;
            sqlQuery += offerVersion;
            sqlQuery += sqlQueryPart8;

            queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 20;
            queryColumns.sql_datetime = 1;
            queryColumns.sql_string = 12;
            queryColumns.sql_bit = 1;

            if (!sqlDatabase.GetRows(sqlQuery, queryColumns))
                return false;

            int intColumnsCount = 0;
            int stringColumnsCount = 0;

            offerIssueDate = sqlDatabase.rows[0].sql_datetime[0];

            offerId = sqlDatabase.rows[0].sql_string[stringColumnsCount++];
            offerNotes = sqlDatabase.rows[0].sql_string[stringColumnsCount++];

            SetCurrency(sqlDatabase.rows[0].sql_int[intColumnsCount++], sqlDatabase.rows[0].sql_string[stringColumnsCount++]);

            SetPercentDownPayment(sqlDatabase.rows[0].sql_int[intColumnsCount++]);
            SetPercentOnDelivery(sqlDatabase.rows[0].sql_int[intColumnsCount++]);
            SetPercentOnInstallation(sqlDatabase.rows[0].sql_int[intColumnsCount++]);

            SetHasDrawings(sqlDatabase.rows[0].sql_bit[intColumnsCount++]);

            SetDeliveryTimeMinimum(sqlDatabase.rows[0].sql_int[intColumnsCount++]);
            SetDeliveryTimeMaximum(sqlDatabase.rows[0].sql_int[intColumnsCount++]);
            SetDeliveryTimeUnit(sqlDatabase.rows[0].sql_int[intColumnsCount++], sqlDatabase.rows[0].sql_string[stringColumnsCount++]);

            SetDeliveryPoint(sqlDatabase.rows[0].sql_int[intColumnsCount++], sqlDatabase.rows[0].sql_string[stringColumnsCount++]);
            SetOfferContractType(sqlDatabase.rows[0].sql_int[intColumnsCount++], sqlDatabase.rows[0].sql_string[stringColumnsCount++]);

            SetWarrantyPeriod(sqlDatabase.rows[0].sql_int[intColumnsCount++]);
            SetWarrantyPeriodTimeUnit(sqlDatabase.rows[0].sql_int[intColumnsCount++], sqlDatabase.rows[0].sql_string[stringColumnsCount++]);

            SetOfferValidityPeriod(sqlDatabase.rows[0].sql_int[intColumnsCount++]);
            SetOfferValidityTimeUnit(sqlDatabase.rows[0].sql_int[intColumnsCount++], sqlDatabase.rows[0].sql_string[stringColumnsCount++]);

            SetOfferStatus(sqlDatabase.rows[0].sql_int[intColumnsCount++], sqlDatabase.rows[0].sql_string[stringColumnsCount++]);

            int productNoColumn = intColumnsCount;
            int productName = stringColumnsCount;

            for (int i = 0; i < sqlDatabase.rows.Count; i++)
            {
                int productNumber = sqlDatabase.rows[i].sql_int[productNoColumn];

                intColumnsCount = productNoColumn + 1;
                stringColumnsCount = productName;

                SetOfferProductType(productNumber, sqlDatabase.rows[productNumber - 1].sql_int[intColumnsCount++], sqlDatabase.rows[productNumber - 1].sql_string[stringColumnsCount++]);
                SetOfferProductBrand(productNumber, sqlDatabase.rows[productNumber - 1].sql_int[intColumnsCount++], sqlDatabase.rows[productNumber - 1].sql_string[stringColumnsCount++]);
                SetOfferProductModel(productNumber, sqlDatabase.rows[productNumber - 1].sql_int[intColumnsCount++], sqlDatabase.rows[productNumber - 1].sql_string[stringColumnsCount++]);

                OfferProductsList[i].productQuantity = sqlDatabase.rows[productNumber - 1].sql_int[intColumnsCount++];
                OfferProductsList[i].productPrice = sqlDatabase.rows[productNumber - 1].sql_int[intColumnsCount++];
            }

            SetTotalValues();
            SetNoOfSavedOfferProducts();
            GetNewOfferServerPath();

            return true;
        }
        public bool InitializeSalesWorkOfferInfo(int mOfferSerial, int mOfferVersion, int mOfferProposerId)
        {
            if (!InitializeOfferProposerInfo(mOfferProposerId, COMPANY_ORGANISATION_MACROS.SALES_TEAM_ID))
                return false;
            if (!InitializeSalesWorkOfferInfo(mOfferSerial, mOfferVersion))
                return false;

            return true;
        }

        public bool InitializeTechnicalOfficeWorkOfferInfo(int mOfferSerial, int mOfferVersion)
        {
            ResetWorkOfferInfo(COMPANY_ORGANISATION_MACROS.TECHNICAL_OFFICE_TEAM_ID);

            offerSerial = mOfferSerial;
            offerVersion = mOfferVersion;

            String sqlQueryPart1 = @"with get_product_type		as	(	select products_type.id as product_type_id, 
        products_type.product_name as product_type

        from erp_system.dbo.products_type
		), 
		get_product_brand as (select brands_type.id as product_brand_id, 
		brands_type.brand_name as product_brand

        from erp_system.dbo.brands_type
		), 
		get_product_model as (select brands_models.model_id as product_model_id, 
		brands_models.product_id as product_type_id, 
		brands_models.brand_id as product_brand_id, 
		brands_models.brand_model as product_model

        from erp_system.dbo.brands_models
		), 
		get_price_currency as (select currencies_type.id as price_currency_id, 
		currencies_type.currency as price_currency

        from erp_system.dbo.currencies_type
		), 
		get_delivery_time_unit as (select time_units.id as delivery_unit_id, 
		time_units.time_unit as delivery_point_unit

        from erp_system.dbo.time_units
		), 
		get_warranty_time_unit as (select time_units.id as warranty_unit_id, 
		time_units.time_unit as warranty_unit

        from erp_system.dbo.time_units
		), 
		get_validity_time_unit as (select time_units.id as validity_unit_id, 
		time_units.time_unit as validity_unit

        from erp_system.dbo.time_units
		) 
		
		select work_offers.branch_serial, 
		work_offers.contact_id, 
		
		get_price_currency.price_currency_id, 
		
		work_offers.percent_down_payment, 
		work_offers.percent_on_delivery, 
		work_offers.percent_on_installation, 
		
		work_offers.delivery_period_minimum, 
		work_offers.delivery_period_maximum, 
		get_delivery_time_unit.delivery_unit_id, 
		
		delivery_points.id as delivery_point_id, 
		contracts_type.id as contract_id, 
		
		work_offers.warranty_period, 
		get_warranty_time_unit.warranty_unit_id, 
		
		work_offers.offer_validity_period, 
		get_validity_time_unit.validity_unit_id, 
		
		offers_status.id as status_id, 
		
		work_offers_products_info.product_number, 
		work_offers_products_info.product_type, 
		work_offers_products_info.product_brand, 
		work_offers_products_info.product_model, 
		work_offers_products_info.product_quantity, 
		work_offers_products_info.product_price, 

		work_offers.issue_date, 
		
		work_offers.offer_id, 
		work_offers.offer_notes, 
		
		get_price_currency.price_currency, 
		
		get_delivery_time_unit.delivery_point_unit, 
		delivery_points.delivery_point as delivery_point, 
		
		contracts_type.contract_type, 
		
		get_warranty_time_unit.warranty_unit, 
		get_validity_time_unit.validity_unit, 
		
		offers_status.offer_status, 
		
		get_product_type.product_type, 
		get_product_brand.product_brand, 
		get_product_model.product_model,

        work_offers.isDrawing

        from erp_system.dbo.work_offers

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


        inner join get_delivery_time_unit
        on work_offers.delivery_time_unit = get_delivery_time_unit.delivery_unit_id


        inner join erp_system.dbo.delivery_points
        on work_offers.delivery_point = delivery_points.id


        inner join erp_system.dbo.contracts_type
        on work_offers.contract_type = contracts_type.id


        inner join get_warranty_time_unit
        on work_offers.warranty_time_unit = get_warranty_time_unit.warranty_unit_id


        inner join get_validity_time_unit
        on work_offers.offer_validity_unit = get_validity_time_unit.validity_unit_id


        inner join erp_system.dbo.offers_status
        on work_offers.offer_status = offers_status.id


        inner join get_price_currency
        on work_offers.price_currency = get_price_currency.price_currency_id


        where work_offers.offer_proposer = ";

            String sqlQueryPart2 = " and work_offers.offer_serial = ";
            String sqlQueryPart3 = " and work_offers.offer_version = ";
            String sqlQueryPart4 = ";";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += GetOfferProposerId();
            sqlQuery += sqlQueryPart2;
            sqlQuery += offerSerial;
            sqlQuery += sqlQueryPart3;
            sqlQuery += offerVersion;
            sqlQuery += sqlQueryPart4;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 22;
            queryColumns.sql_datetime = 1;
            queryColumns.sql_string = 12;
            queryColumns.sql_bit = 1;

            if (!sqlDatabase.GetRows(sqlQuery, queryColumns))
                return false;

            int intColumnsCount = 2;
            int stringColumnsCount = 0;

            offerIssueDate = sqlDatabase.rows[0].sql_datetime[0];

            offerId = sqlDatabase.rows[0].sql_string[stringColumnsCount++];
            offerNotes = sqlDatabase.rows[0].sql_string[stringColumnsCount++];

            SetCurrency(sqlDatabase.rows[0].sql_int[intColumnsCount++], sqlDatabase.rows[0].sql_string[stringColumnsCount++]);

            SetPercentDownPayment(sqlDatabase.rows[0].sql_int[intColumnsCount++]);
            SetPercentOnDelivery(sqlDatabase.rows[0].sql_int[intColumnsCount++]);
            SetPercentOnInstallation(sqlDatabase.rows[0].sql_int[intColumnsCount++]);

            SetHasDrawings(sqlDatabase.rows[0].sql_bit[intColumnsCount++]);

            SetDeliveryTimeMinimum(sqlDatabase.rows[0].sql_int[intColumnsCount++]);
            SetDeliveryTimeMaximum(sqlDatabase.rows[0].sql_int[intColumnsCount++]);
            SetDeliveryTimeUnit(sqlDatabase.rows[0].sql_int[intColumnsCount++], sqlDatabase.rows[0].sql_string[stringColumnsCount++]);

            SetDeliveryPoint(sqlDatabase.rows[0].sql_int[intColumnsCount++], sqlDatabase.rows[0].sql_string[stringColumnsCount++]);
            SetOfferContractType(sqlDatabase.rows[0].sql_int[intColumnsCount++], sqlDatabase.rows[0].sql_string[stringColumnsCount++]);

            SetWarrantyPeriod(sqlDatabase.rows[0].sql_int[intColumnsCount++]);
            SetWarrantyPeriodTimeUnit(sqlDatabase.rows[0].sql_int[intColumnsCount++], sqlDatabase.rows[0].sql_string[stringColumnsCount++]);

            SetOfferValidityPeriod(sqlDatabase.rows[0].sql_int[intColumnsCount++]);
            SetOfferValidityTimeUnit(sqlDatabase.rows[0].sql_int[intColumnsCount++], sqlDatabase.rows[0].sql_string[stringColumnsCount++]);

            SetOfferStatus(sqlDatabase.rows[0].sql_int[intColumnsCount++], sqlDatabase.rows[0].sql_string[stringColumnsCount++]);

            int productNoColumn = intColumnsCount;
            int productName = stringColumnsCount;

            for (int i = 0; i < sqlDatabase.rows[0].sql_int[productNoColumn]; i++)
            {
                int productNumber = sqlDatabase.rows[i].sql_int[productNoColumn];

                intColumnsCount = productNoColumn + 1;
                stringColumnsCount = productName;

                SetOfferProductType(productNumber, sqlDatabase.rows[productNumber - 1].sql_int[intColumnsCount++], sqlDatabase.rows[productNumber - 1].sql_string[stringColumnsCount++]);
                SetOfferProductBrand(productNumber, sqlDatabase.rows[productNumber - 1].sql_int[intColumnsCount++], sqlDatabase.rows[productNumber - 1].sql_string[stringColumnsCount++]);
                SetOfferProductModel(productNumber, sqlDatabase.rows[productNumber - 1].sql_int[intColumnsCount++], sqlDatabase.rows[productNumber - 1].sql_string[stringColumnsCount++]);

                OfferProductsList[i].productQuantity = sqlDatabase.rows[productNumber - 1].sql_int[intColumnsCount++];
                OfferProductsList[i].productPrice = sqlDatabase.rows[productNumber - 1].sql_int[intColumnsCount++];
            }

            SetTotalValues();
            SetNoOfSavedOfferProducts();
            GetNewOfferServerPath();

            if (!InitializeBranchInfo(sqlDatabase.rows[0].sql_int[0]))
                return false;
            if (!InitializeContactInfo(sqlDatabase.rows[0].sql_int[1]))
                return false;

            return true;
        }
        public bool InitializeTechnicalOfficeWorkOfferInfo(int mOfferSerial, int mOfferVersion, int mOfferProposerId)
        {
            if (!InitializeOfferProposerInfo(mOfferProposerId, COMPANY_ORGANISATION_MACROS.TECHNICAL_OFFICE_TEAM_ID))
                return false;
            if (!InitializeTechnicalOfficeWorkOfferInfo(mOfferSerial, mOfferVersion))
                return false;

            return true;
        }

        public bool InitializeOfferProposerInfo(int mEmployeeId, int teamId)
        {
            if (teamId == COMPANY_ORGANISATION_MACROS.TECHNICAL_OFFICE_TEAM_ID)
                offerProposer = contact.GetSalesPerson();
            else if (teamId == COMPANY_ORGANISATION_MACROS.SALES_TEAM_ID)
                offerProposer = assignedEngineer;
            //SHALL BE ASSIGNED TO POINTER

            return offerProposer.InitializeEmployeeInfo(mEmployeeId);
        }

        //////////////////////////////////////////////////////////////////////
        //RESET FUNCTIONS
        //////////////////////////////////////////////////////////////////////

        public void ResetWorkOfferInfo(int teamId)
        {
            if (teamId == COMPANY_ORGANISATION_MACROS.TECHNICAL_OFFICE_TEAM_ID)
                offerProposer = contact.GetSalesPerson();
            else if (teamId == COMPANY_ORGANISATION_MACROS.SALES_TEAM_ID)
                offerProposer = assignedEngineer;
            //SHALL BE ASSIGNED TO POINTER

            ResetOfferSerial();
            ResetOfferVersion();

            ResetOfferProduct1Type();
            ResetOfferProduct2Type();
            ResetOfferProduct3Type();
            ResetOfferProduct4Type();

            ResetOfferProduct1Brand();
            ResetOfferProduct2Brand();
            ResetOfferProduct3Brand();
            ResetOfferProduct4Brand();

            ResetOfferProduct1Model();
            ResetOfferProduct2Model();
            ResetOfferProduct3Model();
            ResetOfferProduct4Model();

            ResetOfferProduct1Quantity();
            ResetOfferProduct2Quantity();
            ResetOfferProduct3Quantity();
            ResetOfferProduct4Quantity();

            ResetOfferContractType();
            ResetOfferStatus();

            //////////////////////////////////////////////////////////////////////
            //OFFER PRODUCTS INFO
            //////////////////////////////////////////////////////////////////////

            OfferProductsList[0].productPrice = 0;
            OfferProductsList[1].productPrice = 0;
            OfferProductsList[2].productPrice = 0;
            OfferProductsList[3].productPrice = 0;
        }

        public void ResetOfferSerial()
        {
            offerSerial = 0;
        }
        public void ResetOfferVersion()
        {
            offerVersion = 1;
        }

        public void ResetOfferProduct1Type()
        {
            OfferProductsList[0].productType.typeId = 0;
            OfferProductsList[0].productType.typeName = "Other";

            SetNoOfSavedOfferProducts();
        }
        public void ResetOfferProduct2Type()
        {
            OfferProductsList[1].productType.typeId = 0;
            OfferProductsList[1].productType.typeName = "Other";

            SetNoOfSavedOfferProducts();
        }
        public void ResetOfferProduct3Type()
        {
            OfferProductsList[2].productType.typeId = 0;
            OfferProductsList[2].productType.typeName = "Other";

            SetNoOfSavedOfferProducts();
        }
        public void ResetOfferProduct4Type()
        {
            OfferProductsList[3].productType.typeId = 0;
            OfferProductsList[3].productType.typeName = "Other";

            SetNoOfSavedOfferProducts();
        }
        public void ResetOfferProductType(int i)
        {
            OfferProductsList[i - 1].productType.typeId = 0;
            OfferProductsList[i - 1].productType.typeName = "Other";

            SetNoOfSavedOfferProducts();
        }

        public void ResetOfferProduct1Brand()
        {
            OfferProductsList[0].productBrand.brandId = 0;
            OfferProductsList[0].productBrand.brandName = "Other";

            SetNoOfSavedOfferProducts();
        }
        public void ResetOfferProduct2Brand()
        {
            OfferProductsList[1].productBrand.brandId = 0;
            OfferProductsList[1].productBrand.brandName = "Other";

            SetNoOfSavedOfferProducts();
        }
        public void ResetOfferProduct3Brand()
        {
            OfferProductsList[2].productBrand.brandId = 0;
            OfferProductsList[2].productBrand.brandName = "Other";

            SetNoOfSavedOfferProducts();
        }
        public void ResetOfferProduct4Brand()
        {
            OfferProductsList[3].productBrand.brandId = 0;
            OfferProductsList[3].productBrand.brandName = "Other";

            SetNoOfSavedOfferProducts();
        }
        public void ResetOfferProductBrand(int i)
        {
            OfferProductsList[i - 1].productBrand.brandId = 0;
            OfferProductsList[i - 1].productBrand.brandName = "Other";

            SetNoOfSavedOfferProducts();
        }

        public void ResetOfferProduct1Model()
        {
            OfferProductsList[0].productModel.modelId = 0;
            OfferProductsList[0].productModel.modelName = "Other";

            SetNoOfSavedOfferProducts();
        }
        public void ResetOfferProduct2Model()
        {
            OfferProductsList[1].productModel.modelId = 0;
            OfferProductsList[1].productModel.modelName = "Other";

            SetNoOfSavedOfferProducts();
        }
        public void ResetOfferProduct3Model()
        {
            OfferProductsList[2].productModel.modelId = 0;
            OfferProductsList[2].productModel.modelName = "Other";

            SetNoOfSavedOfferProducts();
        }
        public void ResetOfferProduct4Model()
        {
            OfferProductsList[3].productModel.modelId = 0;
            OfferProductsList[3].productModel.modelName = "Other";

            SetNoOfSavedOfferProducts();
        }
        public void ResetOfferProductModel(int i)
        {
            OfferProductsList[i - 1].productModel.modelId = 0;
            OfferProductsList[i - 1].productModel.modelName = "Other";

            SetNoOfSavedOfferProducts();
        }

        public void ResetOfferProduct1Quantity()
        {
            OfferProductsList[0].productQuantity = 0;
            SetNoOfSavedOfferProducts();
        }
        public void ResetOfferProduct2Quantity()
        {
            OfferProductsList[1].productQuantity = 0;
            SetNoOfSavedOfferProducts();
        }
        public void ResetOfferProduct3Quantity()
        {
            OfferProductsList[2].productQuantity = 0;
            SetNoOfSavedOfferProducts();
        }
        public void ResetOfferProduct4Quantity()
        {
            OfferProductsList[3].productQuantity = 0;
            SetNoOfSavedOfferProducts();
        }
        public void ResetOfferProductQuantity(int i)
        {
            OfferProductsList[i - 1].productQuantity = 0;
            SetNoOfSavedOfferProducts();
        }

        public void ResetOfferProduct1Price()
        {
            OfferProductsList[0].productPrice = 0;
            SetNoOfSavedOfferProducts();
        }
        public void ResetOfferProduct2Price()
        {
            OfferProductsList[1].productPrice = 0;
            SetNoOfSavedOfferProducts();
        }
        public void ResetOfferProduct3Price()
        {
            OfferProductsList[2].productPrice = 0;
            SetNoOfSavedOfferProducts();
        }
        public void ResetOfferProduct4Price()
        {
            OfferProductsList[3].productPrice = 0;
            SetNoOfSavedOfferProducts();
        }
        public void ResetOfferProductPrice(int i)
        {
            OfferProductsList[i - 1].productPrice = 0;
            SetNoOfSavedOfferProducts();
        }

        public void ResetOfferProduct(int i)
        {
            ResetOfferProductType(i);
            ResetOfferProductBrand(i);
            ResetOfferProductModel(i);
            ResetOfferProductQuantity(i);
            ResetOfferProductPrice(i);
        }

        public void ResetOfferContractType()
        {
            modifiedContractTypeId = 0;
            modifiedContractType = "0";
        }
        public void ResetOfferStatus()
        {
            offerStatusId = COMPANY_WORK_MACROS.PENDING_WORK_OFFER;
            offerStatus = "Pending";
        }

        //////////////////////////////////////////////////////////////////////
        //MODIFICATION FUNCTIONS
        //////////////////////////////////////////////////////////////////////


        //////////////////////////////////////////////////////////////////////
        //PRODUCTS
        //////////////////////////////////////////////////////////////////////

        public void SetOfferProduct1Type(int mProduct1TypeId, String mProduct1Type)
        {
            OfferProductsList[0].productType.typeId = mProduct1TypeId;
            OfferProductsList[0].productType.typeName = mProduct1Type;

            SetNoOfSavedOfferProducts();
        }
        public void SetOfferProduct2Type(int mProduct2TypeId, String mProduct2Type)
        {
            OfferProductsList[1].productType.typeId = mProduct2TypeId;
            OfferProductsList[1].productType.typeName = mProduct2Type;

            SetNoOfSavedOfferProducts();
        }
        public void SetOfferProduct3Type(int mProduct3TypeId, String mProduct3Type)
        {
            OfferProductsList[2].productType.typeId = mProduct3TypeId;
            OfferProductsList[2].productType.typeName = mProduct3Type;

            SetNoOfSavedOfferProducts();
        }
        public void SetOfferProduct4Type(int mProduct4TypeId, String mProduct4Type)
        {
            OfferProductsList[3].productType.typeId = mProduct4TypeId;
            OfferProductsList[3].productType.typeName = mProduct4Type;

            SetNoOfSavedOfferProducts();
        }
        public void SetOfferProductType(int i, int mProduct4TypeId, String mProduct4Type)
        {
            OfferProductsList[i - 1].productType.typeId = mProduct4TypeId;
            OfferProductsList[i - 1].productType.typeName = mProduct4Type;

            SetNoOfSavedOfferProducts();
        }

        public void SetOfferProduct1Brand(int mProduct1BrandId, String mProduct1Brand)
        {
            OfferProductsList[0].productBrand.brandId = mProduct1BrandId;
            OfferProductsList[0].productBrand.brandName = mProduct1Brand;

            SetNoOfSavedOfferProducts();
        }
        public void SetOfferProduct2Brand(int mProduct2BrandId, String mProduct2Brand)
        {
            OfferProductsList[1].productBrand.brandId = mProduct2BrandId;
            OfferProductsList[1].productBrand.brandName = mProduct2Brand;

            SetNoOfSavedOfferProducts();
        }
        public void SetOfferProduct3Brand(int mProduct3BrandId, String mProduct3Brand)
        {
            OfferProductsList[2].productBrand.brandId = mProduct3BrandId;
            OfferProductsList[2].productBrand.brandName = mProduct3Brand;

            SetNoOfSavedOfferProducts();
        }
        public void SetOfferProduct4Brand(int mProduct4BrandId, String mProduct4Brand)
        {
            OfferProductsList[3].productBrand.brandId = mProduct4BrandId;
            OfferProductsList[3].productBrand.brandName = mProduct4Brand;

            SetNoOfSavedOfferProducts();
        }
        public void SetOfferProductBrand(int i, int mProduct4BrandId, String mProduct4Brand)
        {
            OfferProductsList[i - 1].productBrand.brandId = mProduct4BrandId;
            OfferProductsList[i - 1].productBrand.brandName = mProduct4Brand;

            SetNoOfSavedOfferProducts();
        }

        public void SetOfferProduct1Model(int mProduct1ModelId, String mProduct1Model)
        {
            OfferProductsList[0].productModel.modelId = mProduct1ModelId;
            OfferProductsList[0].productModel.modelName = mProduct1Model;

            SetNoOfSavedOfferProducts();
        }
        public void SetOfferProduct2Model(int mProduct2ModelId, String mProduct2Model)
        {
            OfferProductsList[1].productModel.modelId = mProduct2ModelId;
            OfferProductsList[1].productModel.modelName = mProduct2Model;

            SetNoOfSavedOfferProducts();
        }
        public void SetOfferProduct3Model(int mProduct3ModelId, String mProduct3Model)
        {
            OfferProductsList[2].productModel.modelId = mProduct3ModelId;
            OfferProductsList[2].productModel.modelName = mProduct3Model;

            SetNoOfSavedOfferProducts();
        }
        public void SetOfferProduct4Model(int mProduct4ModelId, String mProduct4Model)
        {
            OfferProductsList[3].productModel.modelId = mProduct4ModelId;
            OfferProductsList[3].productModel.modelName = mProduct4Model;

            SetNoOfSavedOfferProducts();
        }
        public void SetOfferProductModel(int i, int mProduct4ModelId, String mProduct4Model)
        {
            OfferProductsList[i - 1].productModel.modelId = mProduct4ModelId;
            OfferProductsList[i - 1].productModel.modelName = mProduct4Model;

            SetNoOfSavedOfferProducts();
        }

        public void SetOfferProduct1Quantity(int mProduct1Quantity)
        {
            OfferProductsList[0].productQuantity = mProduct1Quantity;

            SetTotalValues();
            SetPercentValues();
            SetNoOfSavedOfferProducts();
        }
        public void SetOfferProduct2Quantity(int mProduct2Quantity)
        {
            OfferProductsList[1].productQuantity = mProduct2Quantity;

            SetTotalValues();
            SetPercentValues();
            SetNoOfSavedOfferProducts();
        }
        public void SetOfferProduct3Quantity(int mProduct3Quantity)
        {
            OfferProductsList[2].productQuantity = mProduct3Quantity;

            SetTotalValues();
            SetPercentValues();
            SetNoOfSavedOfferProducts();
        }
        public void SetOfferProduct4Quantity(int mProduct4Quantity)
        {
            OfferProductsList[3].productQuantity = mProduct4Quantity;

            SetTotalValues();
            SetPercentValues();
            SetNoOfSavedOfferProducts();
        }
        public void SetOfferProductQuantity(int i, int mProduct4Quantity)
        {
            OfferProductsList[i - 1].productQuantity = mProduct4Quantity;

            SetTotalValues();
            SetPercentValues();
            SetNoOfSavedOfferProducts();
        }

        public void SetOfferProduct1PriceValue(int mProduct1PriceValue)
        {
            OfferProductsList[0].productPrice = mProduct1PriceValue;
            SetTotalValues();
            SetPercentValues();
        }
        public void SetOfferProduct2PriceValue(int mProduct2PriceValue)
        {
            OfferProductsList[1].productPrice = mProduct2PriceValue;
            SetTotalValues();
            SetPercentValues();
        }
        public void SetOfferProduct3PriceValue(int mProduct3PriceValue)
        {
            OfferProductsList[2].productPrice = mProduct3PriceValue;
            SetTotalValues();
            SetPercentValues();
        }
        public void SetOfferProduct4PriceValue(int mProduct4PriceValue)
        {
            OfferProductsList[3].productPrice = mProduct4PriceValue;
            SetTotalValues();
            SetPercentValues();
        }
        public void SetOfferProductPriceValue(int i, int mProduct4PriceValue)
        {
            OfferProductsList[i - 1].productPrice = mProduct4PriceValue;
            SetTotalValues();
            SetPercentValues();
        }

        public void SetNoOfSavedOfferProducts()
        {
            OfferProductsValid[0] = Convert.ToBoolean(OfferProductsList[0].productType.typeId) && Convert.ToBoolean(OfferProductsList[0].productQuantity);

            if (OfferProductsValid[0])
                noOfSavedOfferProducts = 1;
            else
                noOfSavedOfferProducts = 0;

            for (int i = 1; i < COMPANY_WORK_MACROS.MAX_OFFER_PRODUCTS; i++)
            {
                OfferProductsValid[i] = Convert.ToBoolean(OfferProductsValid[i - 1]) && Convert.ToBoolean(OfferProductsList[i].productType.typeId) && Convert.ToBoolean(OfferProductsList[i].productQuantity);

                if (OfferProductsValid[i])
                    noOfSavedOfferProducts = i + 1;
            }
        }

        //////////////////////////////////////////////////////////////////////
        //PAYMENT CONDITIONS
        //////////////////////////////////////////////////////////////////////

        public void SetTotalValues()
        {
            totalPriceValue = 0;

            for (int i = 0; i < COMPANY_WORK_MACROS.MAX_OFFER_PRODUCTS; i++)
                totalPriceValue += OfferProductsList[i].productPrice * OfferProductsList[i].productQuantity;

        }
        public void SetPercentValues()
        {
            priceValueDownPayment = percentDownPayment * totalPriceValue / 100;
            priceValueOnDelivery = percentOnDelivery * totalPriceValue / 100;
            priceValueOnInstallation = percentOnInstallation * totalPriceValue / 100;
        }

        public void SetCurrency(int mCurrencyId, String mCurrency)
        {
            priceCurrency = mCurrency;
            priceCurrencyId = mCurrencyId;
        }
        public void SetCurrency(int mCurrencyId)
        {
            priceCurrencyId = mCurrencyId;
        }

        public void SetPercentDownPayment(int mPercentDownPayment)
        {
            percentDownPayment = mPercentDownPayment;
            SetPercentValues();
        }
        public void SetPercentOnDelivery(int mPercentOnDelivery)
        {
            percentOnDelivery = mPercentOnDelivery;
            SetPercentValues();
        }
        public void SetPercentOnInstallation(int mPercentOnInstallation)
        {
            percentOnInstallation = mPercentOnInstallation;
            SetPercentValues();
        }

        //////////////////////////////////////////////////////////////////////
        //DRAWINGS CONDITIONS
        //////////////////////////////////////////////////////////////////////

        public void SetHasDrawings(bool mHasDrawings)
        {
            hasDrawings = mHasDrawings;
        }
        public void SetDrawingSubmissionDeadlineMinimum(int mDrawingDeadlineMinimum)
        {
            drawingDeadlineMinimum = mDrawingDeadlineMinimum;
        }
        public void SetDrawingSubmissionDeadlineMaximum(int mDrawingDeadlineMaximum)
        {
            drawingDeadlineMaximum = mDrawingDeadlineMaximum;
        }
        public void SetDrawingSubmissionDeadlineTimeUnit(int mTimeUnitId, String mTimeUnit)
        {
            drawingDeadlineTimeUnitId = mTimeUnitId;
            drawingDeadlineTimeUnit = mTimeUnit;

        }

        //////////////////////////////////////////////////////////////////////
        //DELIVERY CONDITIONS
        //////////////////////////////////////////////////////////////////////

        public void SetDeliveryTimeMinimum(int mDeliveryTimeMinimum)
        {
            deliveryTimeMinimum = mDeliveryTimeMinimum;
        }
        public void SetDeliveryTimeMaximum(int mDeliveryTimeMaximum)
        {
            deliveryTimeMaximum = mDeliveryTimeMaximum;
        }
        public void SetDeliveryTimeUnit(int mDeliveryTimeUnitId, String mDeliveryTimeUnit)
        {
            deliveryTimeUnitId = mDeliveryTimeUnitId;
            deliveryTimeUnit = mDeliveryTimeUnit;

        }
        public void SetDeliveryPoint(int mDeliveryPointId, String mDeliveryPoint)
        {
            deliveryPointId = mDeliveryPointId;
            deliveryPoint = mDeliveryPoint;
        }

        //////////////////////////////////////////////////////////////////////
        //ADDITIONAL OFFER INFO
        //////////////////////////////////////////////////////////////////////

        public void SetOfferContractType(int mContractTypeId, String mContractType)
        {
            modifiedContractTypeId = mContractTypeId;
            modifiedContractType = mContractType;
        }
        public void SetWarrantyPeriod(int mWarrantyPeriod)
        {
            warrantyPeriod = mWarrantyPeriod;
        }
        public void SetWarrantyPeriodTimeUnit(int mTimeUnitId, String mTimeUnit)
        {
            warrantyPeriodTimeUnitId = mTimeUnitId;
            warrantyPeriodTimeUnit = mTimeUnit;
        }

        public void SetOfferValidityPeriod(int mValidityPeriod)
        {
            offerValidityPeriod = mValidityPeriod;
        }
        public void SetOfferValidityTimeUnit(int mTimeUnitId, String mTimeUnit)
        {
            offerValidityTimeUnitId = mTimeUnitId;
            offerValidityTimeUnit = mTimeUnit;
        }

        public void SetOfferNotes(String mNotes)
        {
            offerNotes = mNotes;
        }

        public void SetLocalOfferFilePath(String mPath)
        {
            offerLocalPath = mPath;
        }
        public void SetDatabaseOfferFilePath(String mPath)
        {
            offerServerPath = mPath;
        }

        public void SetOfferIssueDate(DateTime mIssueDate)
        {
            offerIssueDate = mIssueDate;
        }
        public void SetOfferRejectionDate(DateTime mRejectionDate)
        {
            offerRejectionDate = mRejectionDate;
        }
        public void SetOfferStatus(int mOfferStatusId, String mOfferStatus)
        {
            offerStatusId = mOfferStatusId;
            offerStatus = mOfferStatus;
        }

        //////////////////////////////////////////////////////////////////////
        //RETURN FUNCTIONS
        //////////////////////////////////////////////////////////////////////

        public int GetOfferProposerId()
        {
            return offerProposer.GetEmployeeId();
        }
        public int GetOfferProposerTeamId()
        {
            return offerProposer.GetEmployeeTeamId();
        }

        public int GetOfferSerial()
        {
            return offerSerial;
        }
        public int GetOfferVersion()
        {
            return offerVersion;
        }

        public int GetOfferProduct1TypeId()
        {
            return OfferProductsList[0].productType.typeId;
        }
        public int GetOfferProduct2TypeId()
        {
            return OfferProductsList[1].productType.typeId;
        }
        public int GetOfferProduct3TypeId()
        {
            return OfferProductsList[2].productType.typeId;
        }
        public int GetOfferProduct4TypeId()
        {
            return OfferProductsList[3].productType.typeId;
        }
        public int GetOfferProductTypeId(int i)
        {
            return OfferProductsList[i - 1].productType.typeId;
        }

        public int GetOfferProduct1BrandId()
        {
            return OfferProductsList[0].productBrand.brandId;
        }
        public int GetOfferProduct2BrandId()
        {
            return OfferProductsList[1].productBrand.brandId;
        }
        public int GetOfferProduct3BrandId()
        {
            return OfferProductsList[2].productBrand.brandId;
        }
        public int GetOfferProduct4BrandId()
        {
            return OfferProductsList[3].productBrand.brandId;
        }
        public int GetOfferProductBrandId(int i)
        {
            return OfferProductsList[i - 1].productBrand.brandId;
        }

        public int GetOfferProduct1ModelId()
        {
            return OfferProductsList[0].productModel.modelId;
        }
        public int GetOfferProduct2ModelId()
        {
            return OfferProductsList[1].productModel.modelId;
        }
        public int GetOfferProduct3ModelId()
        {
            return OfferProductsList[2].productModel.modelId;
        }
        public int GetOfferProduct4ModelId()
        {
            return OfferProductsList[3].productModel.modelId;
        }
        public int GetOfferProductModelId(int i)
        {
            return OfferProductsList[i - 1].productModel.modelId;
        }

        public int GetOfferProduct1Quantity()
        {
            return OfferProductsList[0].productQuantity;
        }
        public int GetOfferProduct2Quantity()
        {
            return OfferProductsList[1].productQuantity;
        }
        public int GetOfferProduct3Quantity()
        {
            return OfferProductsList[2].productQuantity;
        }
        public int GetOfferProduct4Quantity()
        {
            return OfferProductsList[3].productQuantity;
        }
        public int GetOfferProductQuantity(int i)
        {
            return OfferProductsList[i - 1].productQuantity;
        }

        public int GetProduct1PriceValue()
        {
            return OfferProductsList[0].productPrice;
        }
        public int GetProduct2PriceValue()
        {
            return OfferProductsList[1].productPrice;
        }
        public int GetProduct3PriceValue()
        {
            return OfferProductsList[2].productPrice;
        }
        public int GetProduct4PriceValue()
        {
            return OfferProductsList[4].productPrice;
        }
        public int GetProductPriceValue(int i)
        {
            return OfferProductsList[i - 1].productPrice;
        }

        public int GetNoOfOfferSavedProducts()
        {
            return noOfSavedOfferProducts;
        }

        //////////////////////////////////////////////////////////////////////
        //PAYMENT CONDITIONS
        //////////////////////////////////////////////////////////////////////

        public int GetCurrencyId()
        {
            return priceCurrencyId;
        }

        public int GetPercentDownPayment()
        {
            return percentDownPayment;
        }
        public int GetPercentOnDelivery()
        {
            return percentOnDelivery;
        }
        public int GetPercentOnInstallation()
        {
            return percentOnInstallation;
        }

        public int GetTotalPriceValue()
        {
            return totalPriceValue;
        }
        public int GetPriceValueDownPayment()
        {
            return priceValueDownPayment;
        }
        public int GetPriceValueOnDelivery()
        {
            return priceValueOnDelivery;
        }
        public int GetPriceValueOnInstallation()
        {
            return priceValueOnInstallation;
        }

        //////////////////////////////////////////////////////////////////////
        //DRAWINGS CONDITIONS
        //////////////////////////////////////////////////////////////////////

        bool GetHasDrawings()
        {
            return hasDrawings;
        }

        public int GetDrawingSubmissionDeadlineMinimum()
        {
            return drawingDeadlineMinimum;
        }
        public int GetDrawingSubmissionDeadlineMaximum()
        {
            return drawingDeadlineMaximum;
        }
        public int GetDrawingSubmissionDeadlineTimeUnitId()
        {
            return drawingDeadlineTimeUnitId;
        }

        //////////////////////////////////////////////////////////////////////
        //DELIVERY CONDITIONS
        //////////////////////////////////////////////////////////////////////

        public int GetDeliveryTimeMinimum()
        {
            return deliveryTimeMinimum;
        }
        public int GetDeliveryTimeMaximum()
        {
            return deliveryTimeMaximum;
        }
        public int GetDeliveryTimeUnitId()
        {
            return deliveryTimeUnitId;
        }
        public int GetDeliveryPointId()
        {
            return deliveryPointId;
        }

        //////////////////////////////////////////////////////////////////////
        //ADDITIONAL OFFER INFO
        //////////////////////////////////////////////////////////////////////

        public int GetOfferContractTypeId()
        {
            return modifiedContractTypeId;
        }

        public int GetWarrantyPeriod()
        {
            return warrantyPeriod;
        }
        public int GetWarrantyPeriodTimeUnitId()
        {
            return warrantyPeriodTimeUnitId;
        }

        public int GetOfferValidityPeriod()
        {
            return offerValidityPeriod;
        }
        public int GetOfferValidityTimeUnitId()
        {
            return offerValidityTimeUnitId;
        }

        public int GetOfferStatusId()
        {
            return offerStatusId;
        }
        public int GetOfferFailureReasonId()
        {
            return offerFailureReasonId;
        }


        public String GetOfferID()
        {
            return offerId;
        }

        public String GetOfferProposerName()
        {
            return offerProposer.GetEmployeeName();
        }
        public String GetOfferProposerTeam()
        {
            return offerProposer.GetEmployeeTeam();
        }
        public String GetOfferProposerPosition()
        {
            return offerProposer.GetEmployeePosition();
        }
        public String GetOfferProposerbusinessEmail()
        {
            return offerProposer.GetEmployeeBusinessEmail();
        }
        public String GetOfferProposerCompanyPhone()
        {
            return offerProposer.GetEmployeeBusinessPhone();
        }

        public String GetCompanyName()
        {
            return contact.GetCompanyName();
        }
        public String GetContactName()
        {
            return contact.GetContactName();
        }

        public String GetOfferProduct1Type()
        {
            return OfferProductsList[0].productType.typeName;
        }
        public String GetOfferProduct2Type()
        {
            return OfferProductsList[1].productType.typeName;
        }
        public String GetOfferProduct3Type()
        {
            return OfferProductsList[2].productType.typeName;
        }
        public String GetOfferProduct4Type()
        {
            return OfferProductsList[3].productType.typeName;
        }
        public String GetOfferProductType(int i)
        {
            return OfferProductsList[i - 1].productType.typeName;
        }

        public String GetOfferProduct1Brand()
        {
            return OfferProductsList[0].productBrand.brandName;
        }
        public String GetOfferProduct2Brand()
        {
            return OfferProductsList[1].productBrand.brandName;
        }
        public String GetOfferProduct3Brand()
        {
            return OfferProductsList[2].productBrand.brandName;
        }
        public String GetOfferProduct4Brand()
        {
            return OfferProductsList[3].productBrand.brandName;
        }
        public String GetOfferProductBrand(int i)
        {
            return OfferProductsList[i - 1].productBrand.brandName;
        }

        public String GetOfferProduct1Model()
        {
            return OfferProductsList[0].productModel.modelName;
        }
        public String GetOfferProduct2Model()
        {
            return OfferProductsList[1].productModel.modelName;
        }
        public String GetOfferProduct3Model()
        {
            return OfferProductsList[2].productModel.modelName;
        }
        public String GetOfferProduct4Model()
        {
            return OfferProductsList[3].productModel.modelName;
        }
        public String GetOfferProductModel(int i)
        {
            return OfferProductsList[i - 1].productModel.modelName;
        }

        //////////////////////////////////////////////////////////////////////
        //PAYMENT CONDITIONS
        //////////////////////////////////////////////////////////////////////

        public String GetCurrency()
        {
            return priceCurrency;
        }

        //////////////////////////////////////////////////////////////////////
        //DRAWINGS CONDITIONS
        //////////////////////////////////////////////////////////////////////

        public String GetDrawingDeadlineTimeUnit()
        {
            return drawingDeadlineTimeUnit;
        }

        //////////////////////////////////////////////////////////////////////
        //DELIVERY CONDITIONS
        //////////////////////////////////////////////////////////////////////

        public String GetDeliveryTimeUnit()
        {
            return deliveryTimeUnit;
        }
        public String GetDeliveryPoint()
        {
            return deliveryPoint;
        }

        //////////////////////////////////////////////////////////////////////
        //ADDITIONAL OFFER INFO
        //////////////////////////////////////////////////////////////////////

        public String GetOfferContractType()
        {
            return contractType;
        }
        public String GetWarrantyPeriodTimeUnit()
        {
            return warrantyPeriodTimeUnit;
        }
        public String GetOfferValidityTimeUnit()
        {
            return offerValidityTimeUnit;
        }

        public String GetOfferStatus()
        {
            return offerStatus;
        }

        public String GetOfferFailureReason()
        {
            return offerFailureReason;
        }

        public DateTime GetOfferIssueDate()
        {
            return offerIssueDate;
        }
        public DateTime GetOfferRejectionDate()
        {
            return offerRejectionDate;
        }

        public String GetOfferNotes()
        {
            return offerNotes;
        }

        public String GetLocalOfferFilePath()
        {
            return offerLocalPath;
        }
        public String GetServerOfferFilePath()
        {
            return offerServerPath;
        }

        //////////////////////////////////////////////////////////////////////
        //MODIFICATION FUNCTIONS
        //////////////////////////////////////////////////////////////////////

        public void SetOfferFailureReason(int mFailureReasonId, String mFailureReason)
        {
            offerFailureReasonId = mFailureReasonId;
            offerFailureReason = mFailureReason;
        }

        public void ConfirmOfferStatus()
        {
            offerStatusId = COMPANY_WORK_MACROS.CONFIRMED_WORK_OFFER;
            offerStatus = "P.O.";
        }
        public void RejectOfferStatus()
        {
            offerStatusId = COMPANY_WORK_MACROS.FAILED_WORK_OFFER;
            offerStatus = "Failed";
        }

        bool IssueNewOffer()
        {
            SetOfferIssueDateToToday();

            if (!GetNewOfferSerial())
                return false;

            GetNewOfferID();

            GetNewOfferServerPath();

            return true;
        }
        bool ReviseOffer()
        {
            SetOfferIssueDateToToday();

            if (!GetNewOfferVersion())
                return false;

            GetNewOfferID();

            GetNewOfferServerPath();

            return true;
        }
        public void ConfirmOffer()
        {
            ConfirmOfferStatus();
        }
        public void RejectOffer(int mFailureReasonId, String mFailureReason)
        {
            SetOfferRejectionDateToToday();
            RejectOfferStatus();
            SetOfferFailureReason(mFailureReasonId, mFailureReason);
        }

        //////////////////////////////////////////////////////////////////////
        //GET ADDITIONAL INSERT DATA FUNCTIONS
        //////////////////////////////////////////////////////////////////////

        public void SetOfferIssueDateToToday()
        {
            offerIssueDate = commonFunctions.GetTodaysDate();
        }
        public void SetOfferRejectionDateToToday()
        {
            offerRejectionDate = commonFunctions.GetTodaysDate();
        }

        public bool GetNewOfferSerial()
        {
            String sqlQueryPart1 = "select max(work_offers.offer_serial) from erp_system.dbo.work_offers where work_offers.offer_proposer = ";
            String sqlQueryPart2 = ";";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += GetOfferProposerId();
            sqlQuery += sqlQueryPart2;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;

            if (!sqlDatabase.GetRows(sqlQuery, queryColumns))
                return false;

            offerSerial = sqlDatabase.rows[0].sql_int[0] + 1;

            return true;
        }
        public bool GetNewOfferVersion()
        {
            String sqlQueryPart1 = "select max(work_offers.offer_version) from erp_system.dbo.work_offers where work_offers.offer_proposer = ";
            String sqlQueryPart2 = " and work_offers.offer_serial = ";
            String sqlQueryPart3 = ";";

            sqlQuery = String.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += GetOfferProposerId();
            sqlQuery += sqlQueryPart2;
            sqlQuery += offerSerial;
            sqlQuery += sqlQueryPart3;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 1;

            if (!sqlDatabase.GetRows(sqlQuery, queryColumns))
                return false;

            offerVersion = sqlDatabase.rows[0].sql_int[0] + 1;

            return true;
        }

        public void GetNewOfferID()
        {
            String offerDateID = commonFunctions.GetTimeID(commonFunctions.GetTodaysDate());
            String employeeInitials = offerProposer.GetEmployeeInitials();
            String offerSerialString = offerSerial.ToString();

            int offerSerialLastIndex = offerSerialString.Length - 1;

            for (int i = 0; i < OFFER_ID_FORMAT.Length; i++)
                offerIdString[i] = OFFER_ID_FORMAT[i];

            for (int i = 0; i < REVISED_OFFER_ID_FORMAT.Length; i++)
                revisedOfferIdString[i] = REVISED_OFFER_ID_FORMAT[i];

            for (int i = offerSerialLastIndex; i >= 0; i--)
            {
                offerIdString[OFFER_ID_IDENTIFIER_TOKEN + OFFER_ID_SERIAL_TOKEN - (offerSerialLastIndex - i)] = offerSerialString[i];
                revisedOfferIdString[OFFER_ID_IDENTIFIER_TOKEN + OFFER_ID_SERIAL_TOKEN - (offerSerialLastIndex - i)] = offerSerialString[i];
            }

            for (int i = 0; i < employeeInitials.Length; i++)
            {
                offerIdString[OFFER_ID_EMPLOYEE_INITIALS_START_INDEX + i] = employeeInitials[i];
                revisedOfferIdString[OFFER_ID_EMPLOYEE_INITIALS_START_INDEX + i] = employeeInitials[i];
            }

            for (int i = 0; i < offerDateID.Length; i++)
            {
                offerIdString[OFFER_ID_DATE_START_INDEX + i] = offerDateID[i];
                revisedOfferIdString[OFFER_ID_DATE_START_INDEX + i] = offerDateID[i];
            }

            revisedOfferIdString[OFFER_ID_REVISION_SERIAL_START_INDEX] = (char)((offerVersion - 1) / 10);
            revisedOfferIdString[OFFER_ID_REVISION_SERIAL_START_INDEX + 1] = (char)((offerVersion - 1) % 10);

            if (offerVersion > 1)
                offerId = revisedOfferIdString.ToString();
            else
                offerId = offerIdString.ToString();
        }

        public void GetNewOfferServerPath()
        {
            offerServerPath = String.Empty;
            offerServerPath += COMPANY_WORK_MACROS.OFFER_FILES_PATH;
            offerServerPath += GetOfferID();
            offerServerPath += ".pdf";

            offerServerPath = offerServerPath;
        }

    }
}
