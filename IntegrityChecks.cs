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
using System.Security.Cryptography;

namespace _01electronics_erp
{
    public class IntegrityChecks
    {
        //SQL SERVER
        SQLServer initializationObject;

        //SQL Query
        String sqlQuery;

        //COMMON QUERIES
        CommonQueries commonQueries;


        //DOMAIN TLDS
        String countryTld;
        String originalTld;
        String businessDomain;

        //DOMAIN FORM OBJECTS
        BASIC_STRUCTS.DOMAIN_FORM domainAt;
        BASIC_STRUCTS.DOMAIN_FORM domainFirstDot;
        BASIC_STRUCTS.DOMAIN_FORM domainSecondDot;
        BASIC_STRUCTS.DOMAIN_FORM domainThirdDot;

        public IntegrityChecks()
        {
            initializationObject = new SQLServer();
            commonQueries = new CommonQueries();
        }

        public void RemoveExtraSpaces(String inputString, ref String outputString)
        {
            outputString = String.Empty;

            char[] tempString = new char[BASIC_MACROS.EDIT_BOX_STRING_LENGTH];

            int startPoint = 0;
            for (int i = 0; i < inputString.Length; i++)
            {
                if (inputString[i] != ' ')
                    break;
                else
                    startPoint++;

            }

            int spaceCount = 0;
            int skippedIndeces = 0;

            for (int i = startPoint; i < inputString.Length; i++)
            {
                if (inputString[i] == ' ')
                {
                    if (spaceCount++ >= 1)
                        skippedIndeces++;
                    else if (i < inputString.Length - 1)
                        tempString[i - (startPoint + skippedIndeces)] = inputString[i];
                }
                else
                {
                    tempString[i - (startPoint + skippedIndeces)] = inputString[i];
                    spaceCount = 0;
                }
            }

            outputString = new String(tempString, 0, inputString.Length - (startPoint + skippedIndeces));
        }

        public void CheckCapitalizedInitials(String inputString, ref String outputString)
        {
            outputString = String.Empty;

            char[] tempString = new char[BASIC_MACROS.EDIT_BOX_STRING_LENGTH];

            bool checkCurrentCharacter = true;

            for (int i = 0; i < inputString.Length; i++)
            {
                if (checkCurrentCharacter && inputString[i] >= 'a' && inputString[i] <= 'z')
                    tempString[i] = char.ToUpper(inputString[i]);
                else
                    tempString[i] = inputString[i];

                if (inputString[i] == ' ')
                    checkCurrentCharacter = true;
                else
                    checkCurrentCharacter = false;
            }

            outputString = new String(tempString, 0, inputString.Length);
        }

        ///////////////////////////////////////////////////////////
        //BASIC TEXT CHECKER FUNCTIONS
        ///////////////////////////////////////////////////////////

        public bool CheckNonEmptyEditBox(String inputString)
        {
            if (inputString.Length == 0)
                return false;

            for (int i = 0; i < inputString.Length; i++)
            {
                if (inputString[i] != ' ')
                    return true;
            }
            return false;
        }

        public bool CheckInvalidCharacters(String inputString, int checkType)
        {
            bool isNumber;
            bool isAlphabet;
            bool isRegularSpecialCharacter;
            bool isEmailSpecialCharacter;
            bool isDomainSpecialCharacter;
            bool isPhoneSpecialCharacter;
            bool isMonetarySpecialCharacter;

            for (int i = 0; i < inputString.Length; i++)
            {
                isNumber = false;
                isAlphabet = false;
                isRegularSpecialCharacter = false;
                isEmailSpecialCharacter = false;
                isDomainSpecialCharacter = false;
                isPhoneSpecialCharacter = false;
                isMonetarySpecialCharacter = false;

                if ((inputString[i] >= '0') && (inputString[i] <= '9'))
                    isNumber = true;
                if (((inputString[i] >= 'a') && (inputString[i] <= 'z')) || ((inputString[i] >= 'A') && (inputString[i] <= 'Z')))
                    isAlphabet = true;
                if (inputString[i] == '-' || inputString[i] == '.' || inputString[i] == ' ')
                    isRegularSpecialCharacter = true;
                if (inputString[i] == '@' || inputString[i] == '.' || inputString[i] == '-' || inputString[i] == '_')
                    isEmailSpecialCharacter = true;
                if (inputString[i] == '.' || inputString[i] == '-')
                    isDomainSpecialCharacter = true;
                if (inputString[i] == '+' && i == 0)
                    isPhoneSpecialCharacter = true;
                if (inputString[i] == '.')
                    isMonetarySpecialCharacter = true;

                if (checkType == BASIC_MACROS.REGULAR_STRING && (isAlphabet || isNumber || isRegularSpecialCharacter))
                    continue;
                if (checkType == BASIC_MACROS.EMAIL_STRING && (isAlphabet || isNumber || isEmailSpecialCharacter))
                    continue;
                if (checkType == BASIC_MACROS.DOMAIN_STRING && (isAlphabet || isNumber || isDomainSpecialCharacter))
                    continue;
                if (checkType == BASIC_MACROS.PHONE_STRING && (isNumber || isPhoneSpecialCharacter))
                    continue;
                if (checkType == BASIC_MACROS.MONETARY_STRING && (isNumber || isMonetarySpecialCharacter))
                    continue;
                if (checkType == BASIC_MACROS.NUMERIC_STRING && (isNumber))
                    continue;

                return false;
            }
            return true;
        }

        public bool CheckInBetweenSpaces(String inputString)
        {
            int startPoint = 0;
            for (int i = 0; i < inputString.Length; i++)
            {
                if (inputString[i] != ' ')
                    break;
                else
                    startPoint++;
            }

            int endPoint = inputString.Length - 1;
            for (int i = inputString.Length - 1; i >= 0; i--)
            {
                if (inputString[i] != ' ')
                    break;
                else
                    endPoint--;
            }

            for (int i = startPoint; i <= endPoint; i++)
                if (inputString[i] == ' ')
                    return false;
            return true;
        }

        ///////////////////////////////////////////////////////////
        //ADVANCED TEXT CHECKER FUNCTIONS
        ///////////////////////////////////////////////////////////

        public bool CheckDomainForm(String inputString, int countryId)
        {
            domainFirstDot.charFound = false;
            domainSecondDot.charFound = false;
            domainThirdDot.charFound = false;

            List<String> domainForms = new List<string>();

            if (!commonQueries.GetOriginalTLDs(ref domainForms))
                return false;

            for (int i = 0; i < inputString.Length; i++)
            {
                if (domainFirstDot.charFound == false && inputString[i] == '.')
                {
                    domainFirstDot.charFound = true;
                    domainFirstDot.charIndex = i;
                }
                else if (domainFirstDot.charFound == true && domainSecondDot.charFound == false && inputString[i] == '.')
                {
                    domainSecondDot.charFound = true;
                    domainSecondDot.charIndex = i;
                }
                else if (domainSecondDot.charFound == true && domainThirdDot.charFound == false && inputString[i] == '.')
                {
                    domainThirdDot.charFound = true;
                    domainThirdDot.charIndex = i;
                }
            }

            if (!domainFirstDot.charFound)
                return false;

            if (domainThirdDot.charFound == true)
            {
                if (domainSecondDot.charIndex - domainFirstDot.charIndex != 4)
                    return false;
                if (domainThirdDot.charIndex - domainSecondDot.charIndex != 4)
                    return false;
                if (inputString.Length - domainThirdDot.charIndex != 3)
                    return false;

                char[] tempCountryTld = new char[BASIC_MACROS.EDIT_BOX_STRING_LENGTH];
                char[] tempOriginalTld = new char[BASIC_MACROS.EDIT_BOX_STRING_LENGTH];

                for (int i = domainThirdDot.charIndex; i < inputString.Length; i++)
                    tempCountryTld[i - (domainThirdDot.charIndex)] = inputString[i];

                for (int i = domainSecondDot.charIndex; i < domainThirdDot.charIndex; i++)
                    tempOriginalTld[i - (domainSecondDot.charIndex)] = inputString[i];

                countryTld = new String(tempCountryTld, 0, inputString.Length - domainThirdDot.charIndex);
                originalTld = new String(tempOriginalTld, 0, domainThirdDot.charIndex - domainSecondDot.charIndex);

                bool[] matchFound = { false, false, false };
                for (int j = 0; j < domainForms.Count; j++)
                {
                    for (int k = 1; k < 4; k++)
                    {
                        if (domainForms[j][k] == originalTld[k])
                            matchFound[k - 1] = true;
                        else
                            break;
                    }
                    if (matchFound[0] && matchFound[1] && matchFound[2])
                        break;
                }

                if (matchFound[0] && matchFound[1] && matchFound[2])
                {
                    String expectedCountryTld = String.Empty;

                    if (!commonQueries.GetCountryTLD(countryId, ref expectedCountryTld))
                        return false;

                    for (int j = 1; j < 3; j++)
                        if (expectedCountryTld[j] != countryTld[j])
                            return false;

                    return true;
                }

                return false;
            }
            else if (domainSecondDot.charFound == true)
            {
                if (domainSecondDot.charIndex - domainFirstDot.charIndex != 4)
                    return false;
                if (inputString.Length - domainSecondDot.charIndex != 3)
                    return false;

                char[] tempCountryTld = new char[BASIC_MACROS.EDIT_BOX_STRING_LENGTH];
                char[] tempOriginalTld = new char[BASIC_MACROS.EDIT_BOX_STRING_LENGTH];

                for (int i = domainSecondDot.charIndex; i < inputString.Length; i++)
                    tempCountryTld[i - (domainSecondDot.charIndex)] = inputString[i];

                for (int i = domainFirstDot.charIndex; i < domainSecondDot.charIndex; i++)
                    tempOriginalTld[i - (domainFirstDot.charIndex)] = inputString[i];

                countryTld = new String(tempCountryTld, 0, inputString.Length - domainSecondDot.charIndex);
                originalTld = new String(tempOriginalTld, 0, domainSecondDot.charIndex - domainFirstDot.charIndex);

                bool[] matchFound = { false, false, false };
                for (int j = 0; j < domainForms.Count; j++)
                {
                    for (int k = 1; k < 4; k++)
                    {
                        if (domainForms[j][k] == originalTld[k])
                            matchFound[k - 1] = true;
                        else
                            break;
                    }
                    if (matchFound[0] && matchFound[1] && matchFound[2])
                        break;
                }

                if (matchFound[0] && matchFound[1] && matchFound[2])
                {
                    String expectedCountryTld = String.Empty;

                    if (!commonQueries.GetCountryTLD(countryId, ref expectedCountryTld))
                        return false;

                    for (int j = 1; j < 3; j++)
                        if (expectedCountryTld[j] != countryTld[j])
                            return false;

                    return true;
                }

                return false;
            }
            else if (domainFirstDot.charFound == true)
            {
                if (inputString.Length - domainFirstDot.charIndex != 4)
                    return false;

                char[] tempOriginalTld = new char[BASIC_MACROS.EDIT_BOX_STRING_LENGTH];

                for (int j = domainFirstDot.charIndex; j < inputString.Length; j++)
                    tempOriginalTld[j - (domainFirstDot.charIndex)] = inputString[j];

                originalTld = new String(tempOriginalTld, 0, inputString.Length - domainFirstDot.charIndex);

                bool[] matchFound = { false, false, false };
                for (int j = 0; j < domainForms.Count; j++)
                {
                    for (int k = 1; k < 4; k++)
                    {
                        if (domainForms[j][k] == originalTld[k])
                            matchFound[k - 1] = true;
                        else
                            break;
                    }
                    if (matchFound[0] && matchFound[1] && matchFound[2])
                        return true;
                }
                return false;
            }

            return true;
        }
        public bool CheckDomainForm(String inputString)
        {
            domainFirstDot.charFound = false;

            List<String> domainForms = new List<string>();

            if (!commonQueries.GetOriginalTLDs(ref domainForms))
                return false;

            for (int i = 0; i < inputString.Length; i++)
            {
                if (domainFirstDot.charFound == false && inputString[i] == '.')
                {
                    domainFirstDot.charFound = true;
                    domainFirstDot.charIndex = i;
                }
            }

            if (!domainFirstDot.charFound)
                return false;

            if (domainFirstDot.charFound == true)
            {
                if (inputString.Length - domainFirstDot.charIndex != 4)
                    return false;

                char[] tempOriginalTld = new char[BASIC_MACROS.EDIT_BOX_STRING_LENGTH];

                for (int j = domainFirstDot.charIndex; j < inputString.Length; j++)
                    tempOriginalTld[j - (domainFirstDot.charIndex)] = inputString[j];

                originalTld = new String(tempOriginalTld, 0, inputString.Length - domainFirstDot.charIndex);

                bool[] matchFound = { false, false, false };
                for (int j = 0; j < domainForms.Count; j++)
                {
                    for (int k = 1; k < 4; k++)
                    {
                        if (domainForms[j][k] == originalTld[k])
                            matchFound[k - 1] = true;
                        else
                            break;
                    }
                    if (matchFound[0] && matchFound[1] && matchFound[2])
                        return true;
                }

                return false;
            }

            return false;
        }

        public bool CheckEmailForm(String inputString, int countryId)
        {
            domainAt.charFound = false;
            domainFirstDot.charFound = false;
            domainSecondDot.charFound = false;
            domainThirdDot.charFound = false;

            List<String> domainForms = new List<String>();

            if (!commonQueries.GetOriginalTLDs(ref domainForms))
                return false;

            for (int i = 0; i < inputString.Length; i++)
            {
                if (inputString[i] == '@')
                {
                    domainAt.charFound = true;
                    domainAt.charIndex = i;
                }
                else if (domainAt.charFound == true && domainFirstDot.charFound == false && inputString[i] == '.')
                {
                    domainFirstDot.charFound = true;
                    domainFirstDot.charIndex = i;
                }
                else if (domainFirstDot.charFound == true && domainSecondDot.charFound == false && inputString[i] == '.')
                {
                    domainSecondDot.charFound = true;
                    domainSecondDot.charIndex = i;
                }
                else if (domainSecondDot.charFound == true && domainThirdDot.charFound == false && inputString[i] == '.')
                {
                    domainThirdDot.charFound = true;
                    domainThirdDot.charIndex = i;
                }
            }

            if (!domainAt.charFound || !domainFirstDot.charFound)
                return false;

            if (domainThirdDot.charFound == true)
            {
                if (domainSecondDot.charIndex - domainFirstDot.charIndex != 4)
                    return false;
                if (domainThirdDot.charIndex - domainSecondDot.charIndex != 4)
                    return false;
                if (inputString.Length - domainThirdDot.charIndex != 3)
                    return false;

                char[] tempCountryTld = new char[BASIC_MACROS.EDIT_BOX_STRING_LENGTH];
                char[] tempOriginalTld = new char[BASIC_MACROS.EDIT_BOX_STRING_LENGTH];

                for (int i = domainThirdDot.charIndex; i < inputString.Length; i++)
                    tempCountryTld[i - (domainThirdDot.charIndex)] = inputString[i];

                for (int i = domainSecondDot.charIndex; i < domainThirdDot.charIndex; i++)
                    tempOriginalTld[i - (domainSecondDot.charIndex)] = inputString[i];

                countryTld = new String(tempCountryTld, 0, inputString.Length - domainThirdDot.charIndex);
                originalTld = new String(tempOriginalTld, 0, domainThirdDot.charIndex - domainSecondDot.charIndex);


                bool[] matchFound = { false, false, false };
                for (int j = 0; j < domainForms.Count; j++)
                {
                    for (int k = 1; k < 4; k++)
                    {
                        if (domainForms[j][k] == originalTld[k])
                            matchFound[k - 1] = true;
                        else
                            break;
                    }
                    if (matchFound[0] && matchFound[1] && matchFound[2])
                        break;
                }

                if (matchFound[0] && matchFound[1] && matchFound[2])
                {
                    String expectedCountryTld = String.Empty;

                    if (!commonQueries.GetCountryTLD(countryId, ref expectedCountryTld))
                        return false;

                    for (int j = 1; j < 3; j++)
                        if (expectedCountryTld[j] != countryTld[j])
                            return false;

                    return true;
                }

                return false;
            }
            else if (domainSecondDot.charFound == true)
            {
                if (domainSecondDot.charIndex - domainFirstDot.charIndex != 4)
                    return false;
                if (inputString.Length - domainSecondDot.charIndex != 3)
                    return false;

                char[] tempCountryTld = new char[BASIC_MACROS.EDIT_BOX_STRING_LENGTH];
                char[] tempOriginalTld = new char[BASIC_MACROS.EDIT_BOX_STRING_LENGTH];

                for (int i = domainSecondDot.charIndex; i < inputString.Length; i++)
                    tempCountryTld[i - (domainSecondDot.charIndex)] = inputString[i];

                for (int i = domainFirstDot.charIndex; i < domainSecondDot.charIndex; i++)
                    tempOriginalTld[i - (domainFirstDot.charIndex)] = inputString[i];

                countryTld = new String(tempCountryTld, 0, inputString.Length - domainSecondDot.charIndex);
                originalTld = new String(tempOriginalTld, 0, domainSecondDot.charIndex - domainFirstDot.charIndex);

                bool[] matchFound = { false, false, false };
                for (int j = 0; j < domainForms.Count; j++)
                {
                    for (int k = 1; k < 4; k++)
                    {
                        if (domainForms[j][k] == originalTld[k])
                            matchFound[k - 1] = true;
                        else
                            break;
                    }
                    if (matchFound[0] && matchFound[1] && matchFound[2])
                        break;
                }

                if (matchFound[0] && matchFound[1] && matchFound[2])
                {
                    String expectedCountryTld = String.Empty;

                    if (!commonQueries.GetCountryTLD(countryId, ref expectedCountryTld))
                        return false;

                    for (int j = 1; j < 3; j++)
                        if (expectedCountryTld[j] != countryTld[j])
                            return false;

                    return true;
                }

                return false;
            }
            else if (domainFirstDot.charFound == true)
            {
                if (inputString.Length - domainFirstDot.charIndex != 4)
                    return false;

                char[] tempOriginalTld = new char[BASIC_MACROS.EDIT_BOX_STRING_LENGTH];

                for (int j = domainFirstDot.charIndex; j < inputString.Length; j++)
                    tempOriginalTld[j - (domainFirstDot.charIndex)] = inputString[j];

                originalTld = new String(tempOriginalTld, 0, inputString.Length - domainFirstDot.charIndex);

                bool[] matchFound = { false, false, false };
                for (int j = 0; j < domainForms.Count; j++)
                {
                    for (int k = 1; k < 4; k++)
                    {
                        if (domainForms[j][k] == originalTld[k])
                            matchFound[k - 1] = true;
                        else
                            break;
                    }
                    if (matchFound[0] && matchFound[1] && matchFound[2])
                        return true;
                }
                return false;
            }

            return false;
        }
        public bool CheckEmailForm(String inputString)
        {
            domainAt.charFound = false;
            domainFirstDot.charFound = false;

            List<String> domainForms = new List<String>();

            if (!commonQueries.GetOriginalTLDs(ref domainForms))
                return false;

            for (int i = 0; i < inputString.Length; i++)
            {
                if (inputString[i] == '@')
                {
                    domainAt.charFound = true;
                    domainAt.charIndex = i;
                }
                else if (domainAt.charFound == true && domainFirstDot.charFound == false && inputString[i] == '.')
                {
                    domainFirstDot.charFound = true;
                    domainFirstDot.charIndex = i;
                }
            }

            if (!domainAt.charFound || !domainFirstDot.charFound)
                return false;

            if (domainFirstDot.charFound == true)
            {
                if (inputString.Length - domainFirstDot.charIndex != 4)
                    return false;

                char[] tempOriginalTld = new char[BASIC_MACROS.EDIT_BOX_STRING_LENGTH];

                for (int j = domainFirstDot.charIndex; j < inputString.Length; j++)
                    tempOriginalTld[j - (domainFirstDot.charIndex)] = inputString[j];

                originalTld = new String(tempOriginalTld, 0, inputString.Length - domainFirstDot.charIndex);

                bool[] matchFound = { false, false, false };
                for (int j = 0; j < domainForms.Count; j++)
                {
                    for (int k = 1; k < 4; k++)
                    {
                        if (domainForms[j][k] == originalTld[k])
                            matchFound[k - 1] = true;
                        else
                            break;
                    }
                    if (matchFound[0] && matchFound[1] && matchFound[2])
                        return true;
                }

                return false;
            }

            return false;
        }
        public bool CheckEmailForm(String inputString, String businessDomain = BASIC_MACROS.COMPANY_DOMAIN)
        {
            domainAt.charFound = false;
            domainFirstDot.charFound = false;

            List<String> domainForms = new List<String>();

            if (!commonQueries.GetOriginalTLDs(ref domainForms))
                return false;

            for (int i = 0; i < inputString.Length; i++)
            {
                if (inputString[i] == '@')
                {
                    domainAt.charFound = true;
                    domainAt.charIndex = i;
                }
                else if (domainAt.charFound == true && domainFirstDot.charFound == false && inputString[i] == '.')
                {
                    domainFirstDot.charFound = true;
                    domainFirstDot.charIndex = i;
                }
            }

            if (!domainAt.charFound || !domainFirstDot.charFound)
                return false;

            for (int j = domainAt.charIndex; j < inputString.Length; j++)
                if (businessDomain[j - domainAt.charIndex] != inputString[j])
                    return false;

            return true;
        }

        public bool CheckPhoneForm(String inputString)
        {
            //TO DO: CHECK COUNTRY CALLING CODE
            return true;
        }

        ///////////////////////////////////////////////////////////
        //UNIQUENESS CHECKER FUNCTIONS
        ///////////////////////////////////////////////////////////

        public bool CheckUniqueContact(String contactEmail, int noOfPhones, int noOfEmails, String[] contactPhones, String[] contactPersonalEmails)
        {
            int contactBusinessEmailCount = 0;

            if (!commonQueries.GetContactBusinessEmailCount(contactEmail, ref contactBusinessEmailCount))
                return false;

            if (contactBusinessEmailCount > 0)
            {
                MessageBox.Show("The contact specified is already associated with another sales person, please contact your team leader for support.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            for (int i = 0; i < COMPANY_ORGANISATION_MACROS.MAX_TELEPHONES_PER_CONTACT && i < noOfPhones; i++)
            {
                int contactPhoneCount = 0;

                if (!commonQueries.GetContactPhoneCount(contactPhones[i], ref contactPhoneCount))
                    return false;

                if (contactPhoneCount > 0)
                {
                    MessageBox.Show("The contact specified is already associated with another sales person, please contact your team leader for support.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            for (int i = 0; i < COMPANY_ORGANISATION_MACROS.MAX_EMAILS_PER_CONTACT && i < noOfEmails; i++)
            {
                int contactPersonalEmailCount = 0;

                if (!commonQueries.GetContactPersonalEmailCount(contactPersonalEmails[i], ref contactPersonalEmailCount))
                    return false;

                if (contactPersonalEmailCount > 0)
                {
                    MessageBox.Show("The contact specified is already associated with another sales person, please contact your team leader for support.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            return true;
        }

        public bool CheckUniqueEmployeeEmail(String employeeEmail)
        {
            int employeeEmailCount = 0;

            if (!commonQueries.GetEmployeeEmailCount(employeeEmail, ref employeeEmailCount))
                return false;

            if (employeeEmailCount > 0)
                return false;

            return true;
        }

        public bool CheckAvailableEmployeeEmail(String employeeEmail)
        {
            int employeeEmailCount = 0;

            if (!commonQueries.GetEmployeeEmailCount(employeeEmail, ref employeeEmailCount))
                return false;

            if (employeeEmailCount > 0)
                return true;

            return false;
        }

        public bool CheckUniqueSignUp(String employeeEmail)
        {
            int employeePasswordCount = 0;

            if (!commonQueries.GetEmployeePasswordCount(employeeEmail, ref employeePasswordCount))
                return false;

            if (employeePasswordCount > 0)
                return false;

            return true;
        }
        public bool CheckAvailableSignUp(String employeeEmail)
        {
            int employeePasswordCount = 0;

            if (!commonQueries.GetEmployeePasswordCount(employeeEmail, ref employeePasswordCount))
                return false;

            if (employeePasswordCount == 0)
                return false;

            return true;
        }

        public bool CheckUniqueDomainName(String companyDomain)
        {
            int companyDomainCount = 0;

            if (!commonQueries.GetCompanyDomainCount(companyDomain, ref companyDomainCount))
                return false;

            if (companyDomainCount > 0)
                return false;

            return true;
        }

        public bool CheckUpdatedContact(int updatedInfo, ref Contact oldContact, ref Contact newContact)
        {
            if ((updatedInfo & BASIC_MACROS.CONTACT_BUSINESS_EMAIL_EDITED) > 0)
            {
                int contactBusinessEmailCount = 0;

                if (!commonQueries.GetContactBusinessEmailCount(newContact.GetContactBusinessEmail(), oldContact.GetSalesPersonId(), oldContact.GetAddressSerial(), oldContact.GetContactId(), ref contactBusinessEmailCount))
                    return false;
                if (contactBusinessEmailCount > 0)
                    return false;
            }
            if (((updatedInfo & BASIC_MACROS.CONTACT_PHONE1_EDITED) > 0) || ((updatedInfo & BASIC_MACROS.CONTACT_PHONE2_EDITED) > 0) || ((updatedInfo & BASIC_MACROS.CONTACT_PHONE3_EDITED) > 0))
            {
                for (int i = 0; i < COMPANY_ORGANISATION_MACROS.MAX_TELEPHONES_PER_CONTACT; i++)
                {
                    int contactPhoneCount = 0;

                    if (!commonQueries.GetContactPhoneCount(newContact.GetContactPhones()[i], oldContact.GetSalesPersonId(), oldContact.GetAddressSerial(), oldContact.GetContactId(), ref contactPhoneCount))
                        return false;
                    if (contactPhoneCount > 0)
                        return false;
                }
            }
            if (((updatedInfo & BASIC_MACROS.CONTACT_EMAIL1_EDITED) > 0) || ((updatedInfo & BASIC_MACROS.CONTACT_EMAIL2_EDITED) > 0))
            {
                for (int i = 0; i < COMPANY_ORGANISATION_MACROS.MAX_EMAILS_PER_CONTACT; i++)
                {
                    int contactPersonalEmailCount = 0;

                    if (!commonQueries.GetContactPersonalEmailCount(newContact.GetContactPersonalEmails()[i], oldContact.GetSalesPersonId(), oldContact.GetAddressSerial(), oldContact.GetContactId(), ref contactPersonalEmailCount))
                        return false;
                    if (contactPersonalEmailCount > 0)
                        return false;
                }

            }

            return true;
        }

        ///////////////////////////////////////////////////////////
        //EDIT-BOX CHECKER FUNCTIONS
        ///////////////////////////////////////////////////////////

        public bool CheckEmployeeNameEditBox(String inputString, ref String outputString, bool isRequired)
        {
            RemoveExtraSpaces(inputString, ref outputString);

            bool isEmpty = !CheckNonEmptyEditBox(outputString);

            if (isRequired && isEmpty)
            {
                MessageBox.Show("Employee name must be specified.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!CheckInvalidCharacters(inputString, BASIC_MACROS.REGULAR_STRING))
            {
                MessageBox.Show("Invalid employee name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            CheckCapitalizedInitials(outputString, ref outputString);

            return true;
        }
        public bool CheckEmployeeLoginEmailEditBox(String inputString, ref String outputString, bool isRequired)
        {
            RemoveExtraSpaces(inputString, ref outputString);

            bool isEmpty = !CheckNonEmptyEditBox(outputString);

            if (isRequired && isEmpty)
            {
                MessageBox.Show("Email must be specified", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckInvalidCharacters(outputString, BASIC_MACROS.EMAIL_STRING))
            {
                MessageBox.Show("Invalid email.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckInBetweenSpaces(outputString))
            {
                MessageBox.Show("Invalid email.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckAvailableEmployeeEmail(outputString))
            {
                MessageBox.Show("The specified email was not found, Please contact your adminstration to complete your employee profile.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckAvailableSignUp(outputString))
            {
                MessageBox.Show("No existing signup was found for the specified email, Please signup first then try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
        public bool CheckEmployeeSignUpEmailEditBox(String inputString, ref String outputString, bool isRequired)
        {
            RemoveExtraSpaces(inputString, ref outputString);

            bool isEmpty = !CheckNonEmptyEditBox(outputString);

            if (isRequired && isEmpty)
            {
                MessageBox.Show("Email must be specified", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckInvalidCharacters(outputString, BASIC_MACROS.EMAIL_STRING))
            {
                MessageBox.Show("Invalid email.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckInBetweenSpaces(outputString))
            {
                MessageBox.Show("Invalid email.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckAvailableEmployeeEmail(outputString))
            {
                MessageBox.Show("The specified email was not found, Please contact your adminstration to complete your employee profile.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckUniqueSignUp(outputString))
            {
                MessageBox.Show("An existing signup is already associated with the email specificed, Please contact your adminstration for help.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
        public bool CheckEmployeePasswordEditBox(String employeePassword, int employeeId)
        {
            SHA256 hashing = SHA256.Create();

            byte[] hashedBytes = hashing.ComputeHash(Encoding.UTF8.GetBytes(employeePassword));

            StringBuilder employeePasswordBuilder = new StringBuilder(hashedBytes.Length * 2);

            foreach (byte currentByte in hashedBytes)
                employeePasswordBuilder.AppendFormat("{0:x2}", currentByte);

            String employeeHashedPassword = employeePasswordBuilder.ToString();
            String employeeServerHashedPassword = String.Empty;

            if (!commonQueries.GetEmployeePassword(employeeId, ref employeeServerHashedPassword))
                return false;

            if (employeeHashedPassword != employeeServerHashedPassword)
            {
                MessageBox.Show("Incorrect Password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
        public bool CheckEmployeePersonalEmailEditBox(String inputString, ref String outputString, bool isRequired)
        {
            RemoveExtraSpaces(inputString, ref outputString);

            bool isEmpty = !CheckNonEmptyEditBox(outputString);

            if (isRequired && isEmpty)
            {
                MessageBox.Show("A Personal email must be specified", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckInvalidCharacters(outputString, BASIC_MACROS.EMAIL_STRING))
            {
                MessageBox.Show("Invalid personal email.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckInBetweenSpaces(outputString))
            {
                MessageBox.Show("Invalid personal email.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckEmailForm(outputString))
            {
                MessageBox.Show("Invalid personal email.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
        public bool CheckEmployeeBusinessEmailEditBox(String inputString, String businessDomain, ref String outputString, bool isRequired)
        {
            RemoveExtraSpaces(inputString, ref outputString);

            bool isEmpty = !CheckNonEmptyEditBox(outputString);

            if (isRequired && isEmpty)
            {
                MessageBox.Show("Business Email must be specified.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckInvalidCharacters(outputString, BASIC_MACROS.EMAIL_STRING))
            {
                MessageBox.Show("Invalid business email.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckInBetweenSpaces(outputString))
            {
                MessageBox.Show("Invalid business email.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckEmailForm(outputString, businessDomain))
            {
                MessageBox.Show("Invalid business email.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckUniqueEmployeeEmail(outputString))
            {
                MessageBox.Show("The provided business email is already associated with another employee.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
        public bool CheckCompanyNameEditBox(String inputString, ref String outputString, bool isRequired)
        {
            RemoveExtraSpaces(inputString, ref outputString);

            bool isEmpty = !CheckNonEmptyEditBox(outputString);

            if (!isEmpty && !CheckNonEmptyEditBox(inputString))
            {
                MessageBox.Show("Company name must be specified.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckInvalidCharacters(inputString, BASIC_MACROS.REGULAR_STRING))
            {
                MessageBox.Show("Invalid company name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            CheckCapitalizedInitials(outputString, ref outputString);

            return true;
        }
        public bool CheckCompanyDomainEditBox(String inputString, int countryId, ref String outputString, bool isRequired)
        {
            RemoveExtraSpaces(inputString, ref outputString);

            bool isEmpty = !CheckNonEmptyEditBox(outputString);

            if (isRequired && isEmpty)
            {
                MessageBox.Show("Domain name must be specified", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckInvalidCharacters(outputString, BASIC_MACROS.DOMAIN_STRING))
            {
                MessageBox.Show("Invalid domain name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckInBetweenSpaces(outputString))
            {
                MessageBox.Show("Invalid domain name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckDomainForm(outputString, countryId))
            {
                MessageBox.Show("Invalid domain name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckUniqueDomainName(outputString))
            {
                String messageString = String.Empty;
                String existingCompanyName = String.Empty;

                if (!commonQueries.GetExistingCompanyName(outputString, ref existingCompanyName))
                    return false;

                messageString += "The domain name already exists associated with the name ";
                messageString += existingCompanyName;
                messageString += ".";

                MessageBox.Show(messageString, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        public bool CheckCompanyPhoneEditBox(String inputString, ref String outputString, bool isRequired)
        {
            RemoveExtraSpaces(inputString, ref outputString);

            bool isEmpty = !CheckNonEmptyEditBox(outputString);

            if (isRequired && isEmpty)
            {
                MessageBox.Show("Company phone must be specified", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckInvalidCharacters(outputString, BASIC_MACROS.PHONE_STRING))
            {
                MessageBox.Show("Invalid phone number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckInBetweenSpaces(outputString))
            {
                MessageBox.Show("Invalid phone number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckPhoneForm(outputString))
            {
                MessageBox.Show("Invalid phone number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
        public bool CheckCompanyFaxEditBox(String inputString, ref String outputString, bool isRequired)
        {
            RemoveExtraSpaces(inputString, ref outputString);

            bool isEmpty = !CheckNonEmptyEditBox(outputString);

            if (isRequired && isEmpty)
            {
                MessageBox.Show("Company fax must be specified", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckInvalidCharacters(outputString, BASIC_MACROS.PHONE_STRING))
            {
                MessageBox.Show("Invalid fax number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckInBetweenSpaces(outputString))
            {
                MessageBox.Show("Invalid fax number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckPhoneForm(outputString))
            {
                MessageBox.Show("Invalid fax number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        public bool CheckContactNameEditBox(String inputString, ref String outputString, bool isRequired)
        {
            RemoveExtraSpaces(inputString, ref outputString);

            bool isEmpty = !CheckNonEmptyEditBox(outputString);

            if (isRequired && isEmpty)
            {
                MessageBox.Show("Contact name must be specified.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckInvalidCharacters(inputString, BASIC_MACROS.REGULAR_STRING))
            {
                MessageBox.Show("Invalid contact name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }


            CheckCapitalizedInitials(outputString, ref outputString);

            return true;
        }

        public bool CheckContactBusinessEmailEditBox(String inputString, int countryId, ref String outputString, bool isRequired)
        {
            RemoveExtraSpaces(inputString, ref outputString);

            bool isEmpty = !CheckNonEmptyEditBox(outputString);

            if (isRequired && isEmpty)
            {
                MessageBox.Show("Contact business email must be specified", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckInvalidCharacters(outputString, BASIC_MACROS.EMAIL_STRING))
            {
                MessageBox.Show("Invalid contact business email.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckInBetweenSpaces(outputString))
            {
                MessageBox.Show("Invalid contact business email.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckEmailForm(outputString, countryId))
            {
                MessageBox.Show("Invalid contact business email.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
        public bool CheckContactPersonalEmailEditBox(String inputString, int countryId, ref String outputString, bool isRequired)
        {
            RemoveExtraSpaces(inputString, ref outputString);

            bool isEmpty = !CheckNonEmptyEditBox(outputString);

            if (isRequired && isEmpty)
            {
                MessageBox.Show("Contact email must be specified", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckInvalidCharacters(outputString, BASIC_MACROS.EMAIL_STRING))
            {
                MessageBox.Show("Invalid personal email.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckInBetweenSpaces(outputString))
            {
                MessageBox.Show("Invalid personal email.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckEmailForm(outputString, countryId))
            {
                MessageBox.Show("Invalid personal email.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
        public bool CheckContactPersonalEmailEditBox(String inputString, int emailEditNumber, int countryId, ref String outputString, bool isRequired)
        {
            RemoveExtraSpaces(inputString, ref outputString);

            bool isEmpty = !CheckNonEmptyEditBox(outputString);

            String messageString;

            messageString = String.Empty;
            messageString += "Invalid personal email ";
            messageString += emailEditNumber;
            messageString += ".";

            if (isRequired && isEmpty)
            {
                MessageBox.Show(messageString, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckInvalidCharacters(outputString, BASIC_MACROS.EMAIL_STRING))
            {
                MessageBox.Show(messageString, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckInBetweenSpaces(outputString))
            {
                MessageBox.Show(messageString, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckEmailForm(outputString, countryId))
            {
                MessageBox.Show(messageString, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        public bool CheckContactPhoneEditBox(String inputString, ref String outputString, bool isRequired)
        {
            RemoveExtraSpaces(inputString, ref outputString);

            bool isEmpty = !CheckNonEmptyEditBox(outputString);

            if (isRequired && isEmpty)
            {
                MessageBox.Show("Contact Phone must be specified", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckInvalidCharacters(outputString, BASIC_MACROS.PHONE_STRING))
            {
                MessageBox.Show("Invalid Contact Phone.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckInBetweenSpaces(outputString))
            {
                MessageBox.Show("Invalid Contact Phone.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckPhoneForm(outputString))
            {
                MessageBox.Show("Invalid Contact Phone.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
        public bool CheckContactPhoneEditBox(String inputString, int phoneEditNumber, ref String outputString, bool isRequired)
        {
            RemoveExtraSpaces(inputString, ref outputString);

            bool isEmpty = !CheckNonEmptyEditBox(outputString);

            String messageString = String.Empty;

            messageString += "Invalid phone number ";
            messageString += phoneEditNumber;
            messageString += ".";

            if (isRequired && isEmpty)
            {
                MessageBox.Show(messageString, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckInvalidCharacters(outputString, BASIC_MACROS.PHONE_STRING))
            {
                MessageBox.Show(messageString, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckInBetweenSpaces(outputString))
            {
                MessageBox.Show(messageString, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckPhoneForm(outputString))
            {
                MessageBox.Show(messageString, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        public bool CheckDistrictEditBox(String inputString, ref String outputString, bool isRequired)
        {
            RemoveExtraSpaces(inputString, ref outputString);

            bool isEmpty = !CheckNonEmptyEditBox(outputString);

            if (isRequired && isEmpty)
            {
                MessageBox.Show("District name must be specified", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckInvalidCharacters(inputString, BASIC_MACROS.REGULAR_STRING))
            {
                MessageBox.Show("Invalid district name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            CheckCapitalizedInitials(outputString, ref outputString);

            return true;
        }

        public bool CheckEmployeeSalaryEditBox(String inputString, ref String outputString, bool isRequired)
        {
            RemoveExtraSpaces(inputString, ref outputString);

            bool isEmpty = !CheckNonEmptyEditBox(outputString);

            if (isRequired && isEmpty)
            {
                MessageBox.Show("Employee salary must be specified.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!isEmpty && !CheckInvalidCharacters(inputString, BASIC_MACROS.MONETARY_STRING))
            {
                MessageBox.Show("Invalid employee salary.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
        public bool CheckNationalIdEditBox(String inputString, ref String outputString, bool isRequired)
        {
            RemoveExtraSpaces(inputString, ref outputString);

            bool isEmpty = !CheckNonEmptyEditBox(outputString);

            if (isRequired && isEmpty)
            {
                MessageBox.Show("National ID must be specified.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!isEmpty && !CheckInvalidCharacters(inputString, BASIC_MACROS.NUMERIC_STRING))
            {
                MessageBox.Show("Invalid National ID.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
        public bool CheckFileEditBox(String inputString)
        {
            if (!CheckNonEmptyEditBox(inputString))
            {
                MessageBox.Show("File must be specified.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        ///////////////////////////////////////////////////////////
        //FILES INTEGRITY
        ///////////////////////////////////////////////////////////

        public bool CheckPdfFileFormat(String inputString)
        {
            String pdfFormatString = ".pdf";

            int lastDotIndex = 0;

            bool dotFound = false;

            for (int i = 0; i < inputString.Length; i++)
            {
                if (inputString[i] == '.')
                {
                    dotFound = true;
                    lastDotIndex = i;
                }
            }

            if (inputString.Length - lastDotIndex != 4)
                return false;

            for (int i = lastDotIndex; i < inputString.Length; i++)
            {
                if (inputString[i] != pdfFormatString[i - lastDotIndex])
                    return false;
            }

            return true;
        }
    }
}