﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.ServiceHost;
using ServiceStack.OrmLite;

namespace TmsWS.ServiceModel.Event
{
    [Route("/event/action/list/login", "Post")]
    public class List_Login : IReturn<CommonResponse>
    {
        public string PhoneNumber { get; set; }
        public string CustomerCode { get; set; }
        public string JobNo { get; set; }
    }
    public class List_Login_Logic
    {
        private class Jmjm1
        {
            public string JobNo { get; set; }
            public string CustomerCode { get; set; }
            public string StatusCode { get; set; }
        }
        private class Jmjm4
        {
            public string PhoneNumber { get; set; }
        }
        private class Jmjm6
        {
            public string JobNo { get; set; }
        }
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public int LoginCheck(List_Login request) 
        {
            int Result = -1;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {
                    if (request.PhoneNumber.Length > 0)
                    {
                        Result = db.Scalar<int>("Select count(*) From Jmjm4 Where PhoneNumber={0}", request.PhoneNumber);
                    }
                    else if (request.CustomerCode.Length > 0 && request.JobNo.Length > 0)
                    {
                        Result = db.Scalar<int>("Select count(*) From Jmjm6 Left Join Jmjm1 on Jmjm6.JobNo=Jmjm1.JobNo Where Jmjm1.StatusCode<>'DEL' and Jmjm1.JobNo={0}", request.JobNo);
                    }                    
                }
            }
            catch { throw; }
            return Result;
        }
        public string GetUserInfo(List_Login request)
        {
            string Result = "";
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {
                    Result = db.QuerySingle<string>("Select Top 1 ISNULL(DriverName,'') From Jmjm4 Where PhoneNumber=" + Modfunction.SQLSafeValue(request.PhoneNumber));
                }
            }
            catch { throw; }
            return Result;
        }
    }
}
