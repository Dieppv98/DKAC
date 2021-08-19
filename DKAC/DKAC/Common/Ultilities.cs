using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DKAC.Common
{
    public class Ultilities
    {
        public static string GetPathTemplateExcel()
        {
            string result = string.Empty;
            result = ConfigurationManager.AppSettings["templateFile_Folder"];
            return result;
        }
        public static string GetPathTempFolder()
        {
            string result = string.Empty;
            result = ConfigurationManager.AppSettings["temp_Folder"];
            return result;
        }
        public static string GetPathUploadFolder()
        {
            string result = string.Empty;
            result = ConfigurationManager.AppSettings["uploadFile_Folder"];
            return result;
        }
    }
}