using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Data;
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
                        Result = db.Scalar<int>(
                            db.From<Jmjm4>()
                            .Select(Sql.Count("*"))
                            .Where(j4 => j4.PhoneNumber == request.PhoneNumber)
                        );
                    }
                    else if (request.CustomerCode.Length > 0 && request.JobNo.Length > 0)
                    {
                        Result = db.Scalar<int>(
                            db.From<Jmjm6>()
                            .LeftJoin<Jmjm6, Jmjm1>((j6,j1) => j6.JobNo == j1.JobNo)
                            .Select(Sql.Count("*"))
                            .Where<Jmjm1>(j1 => j1.StatusCode != "DEL" && j1.JobNo == request.JobNo)
                        );
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
                    Result = db.Single<string>("Select Top 1 ISNULL(DriverName,'') From Jmjm4 Where PhoneNumber=" + Modfunction.SQLSafeValue(request.PhoneNumber) + "");
                }
            }
            catch { throw; }
            return Result;
        }
    }
}
