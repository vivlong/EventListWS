using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.ServiceHost;
using ServiceStack.OrmLite;

namespace TmsWS.ServiceModel.Event
{
    [Route("/event/action/update/done", "Post")]
    public class Update_Done : IReturn<CommonResponse>
    {
        public string JobNo { get; set; }
        public int JobLineItemNo { get; set; }
        public int LineItemNo { get; set; }
        public string DoneFlag { get; set; }
        public DateTime DoneDateTime { get; set; }
        public string Remark { get; set; }
        public string ContainerNo { get; set; }
    }
    public class Update_Done_Logic
    {
        private class Jmjm4
        {
            public DateTime DoneDateTime { get; set; }
            public string DoneFlag { get; set; }
            public string JobNo { get; set; }
            public int JobLineItemNo { get; set; }
            public int LineItemNo { get; set; }
            public string Remark { get; set; }
            public string ContainerNo { get; set; }
        }
        private class Jmjm6
        {
            public string JobNo { get; set; }
            public int LineItemNo { get; set; }
            public string ContainerNo { get; set; }
        }
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public int UpdateDone(Update_Done request) 
        {
            int Result = -1;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {
                    Result = db.Update<Jmjm4>(new { DoneDateTime = request.DoneDateTime, DoneFlag = request.DoneFlag, Remark = request.Remark, ContainerNo = request.ContainerNo }, p => p.JobNo == request.JobNo && p.JobLineItemNo == request.JobLineItemNo && p.LineItemNo == request.LineItemNo);
                }
            }
            catch { throw; } 
            return Result;
        }
        public long InsertContainerNo(Update_Done request)
        {
            long Result = -1;
            try
            {
                if (string.IsNullOrEmpty(request.ContainerNo) || request.ContainerNo.Length < 1)
                {
                    return Result;
                }
                using (var db = DbConnectionFactory.OpenDbConnection())
                {                    
                    Result = db.Scalar<int>(
                        "Select count(*) From Jmjm6 Where Jmjm6.JobNo={0} And jmjm6.ContainerNo={1}",request.JobNo,request.ContainerNo
                    );
                    if (Result < 1)
                    {
                        int count = db.Scalar<int>(
                            "Select count(*) From Jmjm6 Where Jmjm6.JobNo={0}",request.JobNo
                        );
                        db.Insert<Jmjm6>(new Jmjm6 { JobNo = request.JobNo, LineItemNo = count + 1, ContainerNo = request.ContainerNo });
                        Result = 0;
                    }
                    else { Result = -1; }
                }
            }
            catch { throw; }
            return Result;
        }
    }
}
