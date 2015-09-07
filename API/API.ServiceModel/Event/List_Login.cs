using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace API.ServiceModel.Event
{
    [Route("/event/action/list/login", "Post")]
    public class List_Login : IReturn<CommonResponse>
    {
        public string PhoneNumber { get; set; }
    }
    public class List_Login_Logic
    {
        private class Jmjm4
        {
            public string PhoneNumber { get; set; }
        }
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public int LoginCheck(List_Login request) 
        {
            int Result = -1;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {
                    Result = db.Scalar<int>(
                        db.From<Jmjm4>()
                        .Select(Sql.Count("*"))
                        .Where(j4 => j4.PhoneNumber == request.PhoneNumber)
                    );
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
