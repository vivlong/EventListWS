using Funq;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.MiniProfiler;
using ServiceStack.MiniProfiler.Data;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface.Cors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using TmsWS.ServiceInterface;
using System.Reflection;
using ServiceStack.WebHost.Endpoints;
using ServiceStack.ServiceHost;
using ServiceStack.Common.Web;

namespace TmsWS
{
    public class AppHost : AppHostBase
    {
        private static string ver = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        private static string strSecretKey;
        public AppHost()
            : base("TmsWS WebService v" + ver, typeof(EventServices).Assembly)
        {
        }
        public override void Configure(Container container)
        {
            ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;
            //Feature disableFeatures = Feature.Xml | Feature.Jsv | Feature.Csv | Feature.Soap11 | Feature.Soap12 | Feature.Soap;
            SetConfig(new EndpointHostConfig
            {
                DebugMode = false,
                UseCustomMetadataTemplates = true,
                DefaultContentType = ContentType.Json,
                EnableFeatures = Feature.Json | Feature.Metadata
                //ServiceStackHandlerFactoryPath  = "api"                
            });
            CorsFeature cf = new CorsFeature(allowedOrigins: "*", allowedMethods: "GET, POST, PUT, DELETE", allowedHeaders: "Content-Type, Signature", allowCredentials: false);
            this.Plugins.Add(cf);
            
            string strConnectionString = GetConnectionString();
            var dbConnectionFactory = new OrmLiteConnectionFactory(strConnectionString, SqlServerDialect.Provider)
            {
                ConnectionFilter =
                    x =>
                    new ProfiledDbConnection(x, Profiler.Current)
            };
            container.Register<IDbConnectionFactory>(dbConnectionFactory);

            var connectString = new TmsWS.ServiceModel.ConnectStringFactory(strConnectionString);
            container.Register<TmsWS.ServiceModel.IConnectString>(connectString);
            var secretKey = new TmsWS.ServiceModel.SecretKeyFactory(strSecretKey);
            container.Register<TmsWS.ServiceModel.ISecretKey>(secretKey);

            container.RegisterAutoWired<TmsWS.ServiceModel.Auth>();
            container.RegisterAutoWired<TmsWS.ServiceModel.Event.List_Login_Logic>();
            container.RegisterAutoWired<TmsWS.ServiceModel.Event.List_JobNo_Logic>();
            container.RegisterAutoWired<TmsWS.ServiceModel.Event.List_Container_Logic>();
            container.RegisterAutoWired<TmsWS.ServiceModel.Event.List_Jmjm6_Logic>();
            container.RegisterAutoWired<TmsWS.ServiceModel.Event.Update_Done_Logic>();
        }

        //public class CustomUserSession : AuthUserSession
        //{
        //    //public override void OnAuthenticated(IServiceBase authService, IAuthSession session, IAuthTokens tokens, Dictionary<string, string> authInfo)
        //    //{
        //    //    base.OnAuthenticated(authService, session, tokens, authInfo);
        //    //}
        //    public string CompanyName { get; set; }
        //}

        //public class CustomCredentialsAuthProvider : CredentialsAuthProvider
        //{
        //    public override bool TryAuthenticate(IServiceBase authService, string strUserName, string strPassword)
        //    {
        //        if(!CheckSession(authService, strUserName)){
        //            int Result = -1;
        //            try
        //            {
        //                var dbcf = new OrmLiteConnectionFactory(GetConnectionString(), SqlServerDialect.Provider, true);
        //                //string strSQLCommand = "SELECT count(*) FROM saus1 WHERE UserId =" + strUserName + " And Password=" + strPassword + "";
        //                string strSQLCommand = "SELECT count(*) FROM saus1 WHERE UserId ='" + strUserName + "'";
        //                using (var db = dbcf.OpenDbConnection())
        //                {
        //                    Result = db.Scalar<int>(strSQLCommand);
        //                }
        //            }
        //            catch
        //            {
        //                Result = -1;
        //            }
        //            if (Result > 0)
        //            {
        //                var session = (CustomUserSession)authService.GetSession(false);
        //                session.UserAuthId = strUserName;
        //                session.IsAuthenticated = true;
        //                session.Roles = new List<string>();
        //                if (session.UserAuthId == "admin") session.Roles.Add(RoleNames.Admin);
        //                session.Roles.Add("User");
        //                return true;
        //            }
        //            else { return false; }
        //        } else { return false; }
        //    }

        //    private bool CheckSession(IServiceBase authService, string strUserName)
        //    {
        //        var session = (CustomUserSession)authService.GetSession(false);
        //        if (strUserName == session.UserName) { return true; }
        //        else { return false; }
        //    }
        //}

        #region DES
        //private string DESKey = "F322186F";
        //private string DESIV = "F322186F";
        private static string DESEncrypt(string strPlain, string strDESKey, string strDESIV)
        {
            string DESEncrypt = "";
            try
            {
                byte[] bytesDESKey = ASCIIEncoding.ASCII.GetBytes(strDESKey);
                byte[] bytesDESIV = ASCIIEncoding.ASCII.GetBytes(strDESIV);
                byte[] inputByteArray = Encoding.Default.GetBytes(strPlain);
                DESCryptoServiceProvider desEncrypt = new DESCryptoServiceProvider();
                desEncrypt.Key = bytesDESKey;
                desEncrypt.IV = bytesDESIV;
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, desEncrypt.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(inputByteArray, 0, inputByteArray.Length);
                        csEncrypt.FlushFinalBlock();
                        StringBuilder str = new StringBuilder();
                        foreach (byte b in msEncrypt.ToArray())
                        {
                            str.AppendFormat("{0:X2}", b);
                        }
                        DESEncrypt = str.ToString();
                    }
                }
            }
            catch
            { }
            return DESEncrypt;
        }
        private static string DesDecrypt(string strValue)
        {
            string DesDecrypt = "";
            if (string.IsNullOrEmpty(strValue))
            {
                return DesDecrypt;
            }
            try
            {
                byte[] DESKey = new byte[] { 70, 51, 50, 50, 49, 56, 54, 70 };
                byte[] DESIV = new byte[] { 70, 51, 50, 50, 49, 56, 54, 70 };
                DES desprovider = new DESCryptoServiceProvider();
                byte[] inputByteArray = new byte[strValue.Length / 2];
                int intI;
                for (intI = 0; intI < strValue.Length / 2; intI++)
                {
                    inputByteArray[intI] = (byte)(Convert.ToInt32(strValue.Substring(intI * 2, 2), 16));
                }
                desprovider.Key = DESKey;
                desprovider.IV = DESIV;
                using (MemoryStream ms = new MemoryStream())
                {
                    CryptoStream cs = new CryptoStream(ms, desprovider.CreateDecryptor(), CryptoStreamMode.Write);
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    DesDecrypt = Encoding.Default.GetString(ms.ToArray());
                }
            }
            catch
            { throw; }
            return DesDecrypt;
        }
        #endregion
        private static string GetConnectionString()
        {
            string IniConnection = "";
            string strAppSetting = "";
            string[] strDataBase = new string[3];
            if (string.IsNullOrEmpty(strAppSetting))
            {
                strAppSetting = System.Configuration.ConfigurationManager.AppSettings["DataBase"];
                strSecretKey = System.Configuration.ConfigurationManager.AppSettings["SecretKey"];
                strDataBase = strAppSetting.Split(',');
                int intCnt;
                for (intCnt = 0; intCnt <= strDataBase.Length - 1; intCnt++)
                {
                    //if (strDataBase[intCnt].ToLower() == strCatalog.ToLower())
                    //{
                    strAppSetting = System.Configuration.ConfigurationManager.AppSettings[strDataBase[intCnt]];
                    string[] strDatabaseInfo;
                    strDatabaseInfo = strAppSetting.Split(',');
                    if (strDatabaseInfo.Length == 6)
                    {
                        IniConnection = System.Configuration.ConfigurationManager.AppSettings[strDatabaseInfo[5]];
                        string strConnection = "";
                        strConnection = IniConnection.Replace("#DataSource", strDatabaseInfo[0]);
                        strConnection = strConnection.Replace("#Catalog", strDatabaseInfo[1]);
                        strConnection = strConnection.Replace("#UserName", strDatabaseInfo[2]);
                        strConnection = strConnection.Replace("#Password", DesDecrypt(strDatabaseInfo[3]));
                        return strConnection;
                    }
                    //}
                }
            }
            return "";
        }
    }
}