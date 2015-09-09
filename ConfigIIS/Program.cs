using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigIIS
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string folderPath = "C:\\inetpub\\wwwroot\\TmsWS";
                string applicationPoolName = "TmsWebService";
                FolderSecurityHelper.SetFolderRights(folderPath);
                IISControlHelper.CreateApplicationPool(applicationPoolName);
                IISControlHelper.CreateApplication(folderPath, applicationPoolName);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}
