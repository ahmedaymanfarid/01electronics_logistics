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
    public class SQLServer
    {
        private String sqlConnectionString = "Data Source=AHMED-AYMAN;Initial Catalog=erp_system;Integrated Security=True";
        private SqlConnection sqlConnection = null;
        private SqlCommand sqlCommand = null;
        private SqlDataReader sqlReader = null;

        private BASIC_STRUCTS.SQL_ROW_STRUCT currentRow;
        private int currentColumn;

        public List<BASIC_STRUCTS.SQL_ROW_STRUCT> rows;
        public SQLServer()
        {

        }
    
        public void InitializeColumns(BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT columnCount)
        {
            currentRow = new BASIC_STRUCTS.SQL_ROW_STRUCT();

            currentRow.sql_tinyint = new List<Byte>();
            currentRow.sql_smallint = new List<Int16>();
            currentRow.sql_int = new List<Int32>();
            currentRow.sql_bigint = new List<Int64>();
            currentRow.sql_money = new List<Decimal>();
            currentRow.sql_decimal = new List<Decimal>();
            currentRow.sql_datetime = new List<DateTime>();
            currentRow.sql_string = new List<String>();
            currentRow.sql_bit = new List<Boolean>();
        }
        public bool GetRows(String sqlQuery, BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT columnCount)
        {
            try
            {
                rows = new List<BASIC_STRUCTS.SQL_ROW_STRUCT>();
                
                sqlConnection = new SqlConnection(sqlConnectionString);
                sqlConnection.Open();

                sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
                sqlReader = sqlCommand.ExecuteReader();


                while (sqlReader.Read())
                {
                    currentColumn = 0;
                    InitializeColumns(columnCount);

                    for (int i = 0; i < columnCount.sql_int; i++)
                    {
                        if (sqlReader[currentColumn] == DBNull.Value)
                            currentRow.sql_int.Add(0);
                        else
                            currentRow.sql_int.Add((Int32)sqlReader[currentColumn]);

                        currentColumn++;
                    }

                    for (int i = 0; i < columnCount.sql_tinyint; i++)
                    {
                        if (sqlReader[currentColumn] == DBNull.Value)
                            currentRow.sql_tinyint.Add(0);
                        else
                            currentRow.sql_tinyint.Add((Byte)sqlReader[currentColumn]);

                        currentColumn++;
                    }

                    for (int i = 0; i < columnCount.sql_smallint; i++)
                    {
                        if (sqlReader[currentColumn] == DBNull.Value)
                            currentRow.sql_smallint.Add(0);
                        else
                            currentRow.sql_smallint.Add((Int16)sqlReader[currentColumn]);

                        currentColumn++;
                    }

                    for (int i = 0; i < columnCount.sql_bigint; i++)
                    {
                        if (sqlReader[currentColumn] == DBNull.Value)
                            currentRow.sql_bigint.Add(0);
                        else
                            currentRow.sql_bigint.Add((Int64)sqlReader[currentColumn]);

                        currentColumn++;
                    }

                    for (int i = 0; i < columnCount.sql_money; i++)
                    {
                        if (sqlReader[currentColumn] == DBNull.Value)
                            currentRow.sql_money.Add(0);
                        else
                            currentRow.sql_money.Add((Decimal)sqlReader[currentColumn]);

                        currentColumn++;
                    }

                    for (int i = 0; i < columnCount.sql_decimal; i++)
                    {
                        if (sqlReader[currentColumn] == DBNull.Value)
                            currentRow.sql_decimal.Add(0);
                        else
                            currentRow.sql_decimal.Add((Decimal)sqlReader[currentColumn]);

                        currentColumn++;
                    }

                    for (int i = 0; i < columnCount.sql_datetime; i++)
                    {
                        DateTime nullDatetime = new DateTime(1900, 1, 1);

                        if (sqlReader[currentColumn] == DBNull.Value)
                            currentRow.sql_datetime.Add(nullDatetime);
                        else
                            currentRow.sql_datetime.Add((DateTime)sqlReader[currentColumn]);

                        currentColumn++;
                    }

                    for (int i = 0; i < columnCount.sql_string; i++)
                    {
                        if (sqlReader[currentColumn] == DBNull.Value)
                            currentRow.sql_string.Add(string.Empty);
                        else
                            currentRow.sql_string.Add((String)sqlReader[currentColumn]);

                    currentColumn++;
                }

                    for (int i = 0; i < columnCount.sql_bit; i++)
                    {
                        if (sqlReader[currentColumn] == DBNull.Value)
                            currentRow.sql_bit.Add(false);
                        else
                            currentRow.sql_bit.Add((Boolean)sqlReader[currentColumn]);

                        currentColumn++;
                    }

                    rows.Add(currentRow);
                }

                sqlReader.Close();
            }
            catch (Exception sqlException)
            {
                MessageBox.Show("Server connection failed! Please check your internet connection and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                
                sqlConnection.Close();

                return false;
            }
            finally
            {
                sqlConnection.Close();
            }

            return true;
        }

        public bool GetRows(String sqlQuery, BASIC_STRUCTS.SQL_COLUMN_COUNT_STRUCT columnCount, int severityLevel)
        {
            try
            {
                rows = new List<BASIC_STRUCTS.SQL_ROW_STRUCT>();

                sqlConnection = new SqlConnection(sqlConnectionString);
                sqlConnection.Open();

                sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
                sqlReader = sqlCommand.ExecuteReader();

                if (severityLevel == BASIC_MACROS.SEVERITY_HIGH && !sqlReader.HasRows)
                {
                    MessageBox.Show("SQL Query returned null rows. Please report this to your system adminstrator.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    sqlReader.Close();
                    sqlConnection.Close();

                    return false;
                }
                else if (!sqlReader.HasRows)
                {
                    sqlReader.Close();
                    sqlConnection.Close();

                    return true;
                }

                while (sqlReader.Read())
                {
                    currentColumn = 0;
                    InitializeColumns(columnCount);

                    for (int i = 0; i < columnCount.sql_int; i++)
                    {
                        if (sqlReader[currentColumn] == DBNull.Value)
                            currentRow.sql_int.Add(0);
                        else
                            currentRow.sql_int.Add((Int32)sqlReader[currentColumn]);

                        currentColumn++;
                    }

                    for (int i = 0; i < columnCount.sql_tinyint; i++)
                    {
                        if (sqlReader[currentColumn] == DBNull.Value)
                            currentRow.sql_tinyint.Add(0);
                        else
                            currentRow.sql_tinyint.Add((Byte)sqlReader[currentColumn]);

                        currentColumn++;
                    }

                    for (int i = 0; i < columnCount.sql_smallint; i++)
                    {
                        if (sqlReader[currentColumn] == DBNull.Value)
                            currentRow.sql_smallint.Add(0);
                        else
                            currentRow.sql_smallint.Add((Int16)sqlReader[currentColumn]);

                        currentColumn++;
                    }

                    for (int i = 0; i < columnCount.sql_bigint; i++)
                    {
                        if (sqlReader[currentColumn] == DBNull.Value)
                            currentRow.sql_bigint.Add(0);
                        else
                            currentRow.sql_bigint.Add((Int64)sqlReader[currentColumn]);

                        currentColumn++;
                    }

                    for (int i = 0; i < columnCount.sql_money; i++)
                    {
                        if (sqlReader[currentColumn] == DBNull.Value)
                            currentRow.sql_money.Add(0);
                        else
                            currentRow.sql_money.Add((Decimal)sqlReader[currentColumn]);

                        currentColumn++;
                    }

                    for (int i = 0; i < columnCount.sql_decimal; i++)
                    {
                        if (sqlReader[currentColumn] == DBNull.Value)
                            currentRow.sql_decimal.Add(0);
                        else
                            currentRow.sql_decimal.Add((Decimal)sqlReader[currentColumn]);

                        currentColumn++;
                    }

                    for (int i = 0; i < columnCount.sql_datetime; i++)
                    {
                        DateTime nullDatetime = new DateTime(1900, 1, 1);

                        if (sqlReader[currentColumn] == DBNull.Value)
                            currentRow.sql_datetime.Add(nullDatetime);
                        else
                            currentRow.sql_datetime.Add((DateTime)sqlReader[currentColumn]);

                        currentColumn++;
                    }

                    for (int i = 0; i < columnCount.sql_string; i++)
                    {
                        if (sqlReader[currentColumn] == DBNull.Value)
                            currentRow.sql_string.Add(string.Empty);
                        else
                            currentRow.sql_string.Add((String)sqlReader[currentColumn]);

                        currentColumn++;
                    }

                    for (int i = 0; i < columnCount.sql_bit; i++)
                    {
                        if (sqlReader[currentColumn] == DBNull.Value)
                            currentRow.sql_bit.Add(false);
                        else
                            currentRow.sql_bit.Add((Boolean)sqlReader[currentColumn]);

                        currentColumn++;
                    }

                    rows.Add(currentRow);
                }

                sqlReader.Close();
            }
            catch (Exception sqlException)
            {
                MessageBox.Show("Server connection failed! Please check your internet connection and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                sqlConnection.Close();

                return false;
            }
            finally
            {
                sqlConnection.Close();
            }

            return true; 
        }

        public bool InsertRows(String sqlQuery)
        {
            try
            {
                sqlConnection = new SqlConnection(sqlConnectionString);
                sqlConnection.Open();

                sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
                sqlCommand.ExecuteNonQuery();

            }
            catch (Exception sqlException)
            {
                MessageBox.Show("Server connection failed! Please check your internet connection and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                sqlConnection.Close();

                return false;
            }
            finally
            {
                sqlConnection.Close();
            }

            return true;
        }

    }
}
