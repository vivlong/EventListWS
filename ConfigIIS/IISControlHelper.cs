using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Web.Administration;

namespace ConfigIIS
{
    /// <summary>
    /// IIS 操作方法集合 (IIS7 or higher)
    /// </summary>
    public class IISControlHelper
    {
        public static bool ExistWebSite(string webSiteName)
        {
            ServerManager iisManager = new ServerManager();
            Site site = iisManager.Sites[webSiteName];
            return site != null;
        }

        public static void CreateApplication(string folderPath, string applicationPoolName)
        {
            ServerManager iisManager = new ServerManager();
            iisManager.Sites[0].Applications.Add("/TmsWS",folderPath);
            iisManager.Sites[0].Applications["/TmsWS"].ApplicationPoolName = applicationPoolName;
            iisManager.CommitChanges();
        }

        public static void DeleteApplication()
        {
            ServerManager iisManager = new ServerManager();
            iisManager.Sites[0].Applications.Remove(iisManager.Sites[0].Applications["/TmsWS"]);
            iisManager.CommitChanges();
        }

        public static void CreateApplicationPool(string appPoolName)
        {
            ServerManager iisManager = new ServerManager();
            ApplicationPool appPool = iisManager.ApplicationPools.Add(appPoolName);
            appPool.AutoStart = true;
            appPool.ManagedPipelineMode = ManagedPipelineMode.Integrated;
            appPool.ManagedRuntimeVersion = "v4.0";
            appPool.ProcessModel.IdentityType = ProcessModelIdentityType.ApplicationPoolIdentity;
            iisManager.CommitChanges();            
        }

        public static void DeleteApplicationPool(String poolName)
        {
            ServerManager iisManager = new ServerManager();
            ApplicationPool appPool = iisManager.ApplicationPools[poolName];
            iisManager.ApplicationPools.Remove(appPool);
            iisManager.CommitChanges();
        }
    }
}
