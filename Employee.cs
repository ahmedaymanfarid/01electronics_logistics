using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01electronics_erp
{
    public class Employee
    {
        //SQL QUERY
        private String sqlQuery;

        //SQL OBJECTS
        private SQLServer initializationObject;
         
        //EMPLOYEE INFO
        private int employeeId;
        
        private int departmentId;
        private int teamId;
        private int positionId;

        private int educationalQualificationId;
        private int majorId;

        private int graduationYear;

        private Decimal salary;

        private List<COMPANY_ORGANISATION_MACROS.BANK_STRUCT> payrollInfo;

        private String businessEmail;
        private String personalEmail;
        
        private String businessPhone;
        private String personalPhone;

        private String name;
        private String initials;
        
        private String gender;
        
        private String department;
        private String team;
        private String position;
        
        private DateTime birthDateStruct;
        private DateTime joinDateStruct;
        
        private String birthDate;
        private String joinDate;
        
        private String educationalQualification;
        private String major;

        public Employee()
        {
            initializationObject = new SQLServer();
        }

        public bool InitializeEmployeeInfo(String mBusinessEmail)
        {
            businessEmail = mBusinessEmail;

            String sqlQueryPart1 = @"select employees_info.employee_id, 
		employees_info.employee_department, 
		employees_info.employee_team, 
		employees_info.employee_position, 

		employees_educational_qualifications.graduation_year, 
		employees_educational_qualifications.certificate, 
		employees_educational_qualifications.major, 

		employees_info.birth_date, 
		employees_info.join_date, 

		employees_info.name, 
		employees_info.gender, 
		departments_type.department, 
		teams_type.team, 
		positions_type.position, 

		employees_personal_emails.email, 
		employees_business_phones.phone, 
		employees_personal_phones.phone, 

		educational_degrees.educational_degree, 
		educational_majors.educational_major, 

        employees_initials.employee_initial

		from erp_system.dbo.employees_info 
		inner join erp_system.dbo.departments_type 
		on employees_info.employee_department = departments_type.id 
		inner join erp_system.dbo.teams_type 
		on employees_info.employee_team = teams_type.id 
		inner join erp_system.dbo.positions_type 
		on employees_info.employee_position = positions_type.id 
		inner join erp_system.dbo.employees_business_emails 
		on employees_info.employee_id = employees_business_emails.id 
		left join erp_system.dbo.employees_personal_emails 
		on employees_info.employee_id = employees_personal_emails.id 
		left join erp_system.dbo.employees_business_phones 
		on employees_info.employee_id = employees_business_phones.id 
		left join erp_system.dbo.employees_personal_phones 
		on employees_info.employee_id = employees_personal_phones.id 
		left join erp_system.dbo.employees_initials 
		on employees_info.employee_id = employees_initials.id 
		left join erp_system.dbo.employees_educational_qualifications
		on employees_info.employee_id = employees_educational_qualifications.employee_id
		left join erp_system.dbo.educational_degrees 
		on employees_educational_qualifications.certificate = educational_degrees.id 
		left join erp_system.dbo.educational_majors 
		on employees_educational_qualifications.major = educational_majors.id 
		where employees_business_emails.email = '";
            String sqlQueryPart2 = "';";

            sqlQuery = string.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += businessEmail;
            sqlQuery += sqlQueryPart2;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 7;
            queryColumns.sql_bigint = 0;
            queryColumns.sql_money = 0;
            queryColumns.sql_datetime = 2;
            queryColumns.sql_string = 11;

            if (!initializationObject.GetRows(sqlQuery, queryColumns, BASIC_MACROS.SEVERITY_HIGH))
                return false;

            employeeId = initializationObject.rows[0].sql_int[0];
            departmentId = initializationObject.rows[0].sql_int[1];
            teamId = initializationObject.rows[0].sql_int[2];
            positionId = initializationObject.rows[0].sql_int[3];
            graduationYear = initializationObject.rows[0].sql_int[4];

            educationalQualificationId = initializationObject.rows[0].sql_int[5];
            majorId = initializationObject.rows[0].sql_int[6];

            birthDateStruct = initializationObject.rows[0].sql_datetime[0];
            joinDateStruct = initializationObject.rows[0].sql_datetime[1];

            joinDate = joinDateStruct.ToString();
            birthDate = birthDateStruct.ToString();

            name = initializationObject.rows[0].sql_string[0];
            gender = initializationObject.rows[0].sql_string[1];

            department = initializationObject.rows[0].sql_string[2];
            team = initializationObject.rows[0].sql_string[3];
            position = initializationObject.rows[0].sql_string[4];

            personalEmail = initializationObject.rows[0].sql_string[5];
            businessPhone = initializationObject.rows[0].sql_string[6];
            personalPhone = initializationObject.rows[0].sql_string[7];
            
            educationalQualification = initializationObject.rows[0].sql_string[8];
            major = initializationObject.rows[0].sql_string[9];

            initials = initializationObject.rows[0].sql_string[10];

            return true;
        }

        public bool InitializeEmployeeInfo(int mEmployeeId)
        {
            employeeId = mEmployeeId;

            String sqlQueryPart1 = @"select employees_info.employee_department, 
		employees_info.employee_team, 
		employees_info.employee_position, 
		employees_info.graduation_year, 

		educational_degrees.id, 
		educational_majors.id,

        banks_names.id,
        employees_payroll_info.payroll_id,
        employees_payroll_info.account_id,

        employees_salaries.salary,

		employees_info.birth_date, 
		employees_info.join_date, 

		employees_info.name, 
		employees_info.gender, 

		departments_type.department, 
		teams_type.team, 
		positions_type.position, 

		employees_business_emails.email, 
		employees_personal_emails.email, 

		employees_business_phones.phone, 
		employees_personal_phones.phone, 

		educational_degrees.educational_degree, 
		educational_majors.educational_major, 

		employees_initials.employee_initial,

		banks_names.bank_name

		from erp_system.dbo.employees_info 
		inner join erp_system.dbo.departments_type 
		on employees_info.employee_department = departments_type.id 
		inner join erp_system.dbo.teams_type 
		on employees_info.employee_team = teams_type.id 
		inner join erp_system.dbo.positions_type 
		on employees_info.employee_position = positions_type.id 
		inner join erp_system.dbo.employees_business_emails 
		on employees_info.employee_id = employees_business_emails.id 
		left join erp_system.dbo.employees_personal_emails 
		on employees_info.employee_id = employees_personal_emails.id 
		left join erp_system.dbo.employees_business_phones 
		on employees_info.employee_id = employees_business_phones.id 
		left join erp_system.dbo.employees_personal_phones 
		on employees_info.employee_id = employees_personal_phones.id 
		left join erp_system.dbo.employees_initials 
		on employees_info.employee_id = employees_initials.id 
		left join erp_system.dbo.educational_degrees 
		on employees_info.educational_degree = educational_degrees.id 
		left join erp_system.dbo.educational_majors 
		on employees_info.major = educational_majors.id 
        left join erp_system.dbo.employees_salaries
        on employees_info.employee_id = employees_salaries.id
        left join erp_system.dbo.employees_payroll_info
        on employees_info.employee_id = employees_payroll_info.employee_id
        inner join erp_system.dbo.banks_names
        on employees_payroll_info.bank_id = banks_names.id
	where employees_info.employee_id = ";
        
    String sqlQueryPart2 = ";";

            sqlQuery = string.Empty;
            sqlQuery += sqlQueryPart1;
            sqlQuery += employeeId;
            sqlQuery += sqlQueryPart2;

            BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT queryColumns = new BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT();

            queryColumns.sql_int = 8;
            queryColumns.sql_bigint = 1;
            queryColumns.sql_money = 1;
            queryColumns.sql_datetime = 2;
            queryColumns.sql_string = 13;

            if (!initializationObject.GetRows(sqlQuery, queryColumns, BASIC_MACROS.SEVERITY_HIGH))
                return false;

            departmentId = initializationObject.rows[0].sql_int[0];
            teamId = initializationObject.rows[0].sql_int[1];
            positionId = initializationObject.rows[0].sql_int[2];
            graduationYear = initializationObject.rows[0].sql_int[3];

            educationalQualificationId = initializationObject.rows[0].sql_int[4];
            majorId = initializationObject.rows[0].sql_int[5];

            salary = initializationObject.rows[0].sql_money[0];

            birthDateStruct = initializationObject.rows[0].sql_datetime[0];
            joinDateStruct = initializationObject.rows[0].sql_datetime[1];

            joinDate = joinDateStruct.ToString();
            birthDate = birthDateStruct.ToString();

            name = initializationObject.rows[0].sql_string[0];
            gender = initializationObject.rows[0].sql_string[1];

            department = initializationObject.rows[0].sql_string[2];
            team = initializationObject.rows[0].sql_string[3];
            position = initializationObject.rows[0].sql_string[4];

            businessEmail = initializationObject.rows[0].sql_string[5];
            personalEmail = initializationObject.rows[0].sql_string[6];

            businessPhone = initializationObject.rows[0].sql_string[7];
            personalPhone = initializationObject.rows[0].sql_string[8];

            educationalQualification = initializationObject.rows[0].sql_string[9];
            major = initializationObject.rows[0].sql_string[10];

            initials = initializationObject.rows[0].sql_string[11];

            payrollInfo = new List<COMPANY_ORGANISATION_MACROS.BANK_STRUCT>();

            for (int i = 0; i < initializationObject.rows.Count; i++)
            {
                COMPANY_ORGANISATION_MACROS.BANK_STRUCT tempItem = new COMPANY_ORGANISATION_MACROS.BANK_STRUCT();

                tempItem.bank_id = initializationObject.rows[i].sql_int[6];
                tempItem.payroll_id = initializationObject.rows[i].sql_int[7];
                tempItem.account_id = (ulong) initializationObject.rows[i].sql_bigint[0];

                tempItem.bank_name = initializationObject.rows[i].sql_string[12];

                payrollInfo.Add(tempItem);
            }

            return true;
        }

        //////////////////////////////////////////////////////////////////////
        //SET FUNCTIONS
        //////////////////////////////////////////////////////////////////////

        public void SetEmployeeName(String mName)
        {
            name = mName;
        }
        public void SetEmployeeInitials(String mInitials)
        {
            initials = mInitials;
        }

        public void SetEmployeeGender(String mGender)
        {
            gender = mGender;
        }

        public void SetEmployeeDepartment(int mDepartmentId, String mDepartment)
        {
            departmentId = mDepartmentId;
            department = mDepartment;
        }
        public void SetEmployeeTeam(int mTeamId, String mTeam)
        {
            teamId = mTeamId;
            team = mTeam;
        }
        public void SetEmployeePosition(int mPositionId, String mPosition)
        {
            positionId = mPositionId;
            position = mPosition;
        }

        public void SetEmployeeCompanyEmail(String mBusinessEmail)
        {
            businessEmail = mBusinessEmail;
        }
        public void SetEmployeePersonalEmail(String mPersonalEmail)
        {
            personalEmail = mPersonalEmail;
        }

        public void SetEmployeeCompanyPhone(String mBusinessPhone)
        {
            businessPhone = mBusinessPhone;
        }

        public void SetEmployeeBirthDate(String mBirthDate)
        {
            birthDate = mBirthDate;
        }
        public void SetEmployeeJoinDate(String mJoinDate)
        {
            joinDate = mJoinDate;
        }

        public void SetEmployeeEducationalQualification(int mEducationalQualificationId, String mEducationalQualification)
        {
            educationalQualificationId = mEducationalQualificationId;
            educationalQualification = mEducationalQualification;
        }

        public void SetEmployeeMajor(int mMajorId, String mMajor)
        {
            majorId = mMajorId;
            major = mMajor;
        }
        //////////////////////////////////////////////////////////////////////
        //RETURN FUNCTIONS
        //////////////////////////////////////////////////////////////////////

        public String GetEmployeeName()
        {
            return name;
        }
        public String GetEmployeeInitials()
        {
            return initials;
        }

        public String GetEmployeeGender()
        {
            return gender;
        }

        public String GetEmployeeDepartment()
        {
            return department;
        }
        public String GetEmployeeTeam()
        {
            return team;
        }
        public String GetEmployeePosition()
        {
            return position;
        }

        public String GetEmployeeBusinessEmail()
        {
            return businessEmail;
        }
        public String GetEmployeePersonalEmail()
        {
            return personalEmail;
        }

        public String GetEmployeeBusinessPhone()
        {
            return businessPhone;
        }

        public String GetEmployeeBirthDate()
        {
            return birthDate;
        }
        public String GetEmployeeJoinDate()
        {
            return joinDate;
        }

        public String GetEmployeeEducationalQualification()
        {
            return educationalQualification;
        }

        public int GetEmployeeId()
        {
            return employeeId;
        }
        public int GetEmployeeDepartmentId()
        {
            return departmentId;
        }
        public int GetEmployeeTeamId()
        {
            return teamId;
        }
        public int GetEmployeePositionId()
        {
            return positionId;
        }
        public int GetEmployeeEducationalQualificationId()
        {
            return educationalQualificationId;
        }
        public int GetEmployeeMajorId()
        {
            return majorId;
        }
        public DateTime GetEmployeeBirthDateStruct()
        {
            return birthDateStruct;
        }
        public DateTime GetEmployeeJoinDateStruct()
        {
            return joinDateStruct;
        }
    }
}
