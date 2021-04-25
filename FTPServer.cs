using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Net;
using System.Collections;
using System.Windows;

namespace _01electronics_erp
{
    public class FTPServer
    {
        public FTPServer()
        {
            
        }

        public bool UploadFile(String sourceFilePath, String destinationFilePath)
        {
            WebClient ftpConnection = new WebClient();
            ftpConnection.Credentials = new NetworkCredential(BASIC_MACROS.FTP_SERVER_USERNAME, BASIC_MACROS.FTP_SERVER_PASSWORD);

            try
            {
                ftpConnection.UploadFile("ftp://" + BASIC_MACROS.FTP_SERVER_IP + destinationFilePath, sourceFilePath);
            }
            catch (WebException exception)
            {
                MessageBox.Show("File upload failed! Please check your internet connection and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        public bool DownloadFile(String sourceFilePath, String destinationFilePath)
        {
            WebClient ftpConnection = new WebClient();
            ftpConnection.Credentials = new NetworkCredential(BASIC_MACROS.FTP_SERVER_USERNAME, BASIC_MACROS.FTP_SERVER_PASSWORD);

            try
            {
                ftpConnection.DownloadFile("ftp://" + BASIC_MACROS.FTP_SERVER_IP + sourceFilePath, destinationFilePath);
            }
            catch (WebException exception)
            {
                MessageBox.Show("File download failed! Please check your internet connection and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        public bool CheckExistingFolder(String directoryPath)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + BASIC_MACROS.FTP_SERVER_IP + directoryPath);

                request.Credentials = new NetworkCredential(BASIC_MACROS.FTP_SERVER_USERNAME, BASIC_MACROS.FTP_SERVER_PASSWORD);
                request.Method = WebRequestMethods.Ftp.ListDirectory;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            }
            catch (WebException exception)
            {
                if (exception.Response != null)
                {
                    FtpWebResponse exceptionResponse = (FtpWebResponse)exception.Response;

                    if (exceptionResponse.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                        return false;
                }
            }

            return true;
        }

        public bool CreateNewFolder(String directoryPath)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + BASIC_MACROS.FTP_SERVER_IP + directoryPath);

                request.Credentials = new NetworkCredential(BASIC_MACROS.FTP_SERVER_USERNAME, BASIC_MACROS.FTP_SERVER_PASSWORD);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            }
            catch (Exception exception)
            {
                return false;
            }

            return true;
        }
    }
}
