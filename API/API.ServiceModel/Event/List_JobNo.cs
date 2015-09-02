using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace API.ServiceModel.Event
{
    [Route("/event/action/list/jobno/{PhoneNumber}", "Get")]
    public class List_JobNo : IReturn<CommonResponse>
    {
        public string PhoneNumber { get; set; }
    }

    public class List_JobNo_Logic
    {
        private class Jmjm4
        {
            public string JobNo { get; set; }
            public int JobLineItemNo { get; set; }
            public string PhoneNumber { get; set; }
            public string DoneFlag { get; set; }
        }
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public IConnectString ConnectString { get; set; }
        public HashSet<string> GetList(List_JobNo request)
        {
            HashSet<string> Result = null;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {                    
                    Result = db.ColumnDistinct<string>(
                        db.From<Jmjm4>()
                        .Select(j4 => j4.JobNo)
                        .Where(j4 => j4.PhoneNumber == request.PhoneNumber && (j4.DoneFlag != "Y" || j4.DoneFlag == null))
                    );
                }
            }
            catch { throw; }
            return Result;
        }

        public long GetCount(string strPhoneNumber, string strJobNo)
        {
            long Result = -1;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {
                    Result = db.Count<Jmjm4>(j4 => j4.PhoneNumber == strPhoneNumber && j4.JobNo == strJobNo && (j4.DoneFlag != "Y" || j4.DoneFlag == null));
                }
            }
            catch { throw; }
            return Result;
        }
    }
}
