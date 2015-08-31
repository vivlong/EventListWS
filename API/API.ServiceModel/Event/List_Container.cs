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
    [Route("/api/event/action/list/container/{PhoneNumber}/{JobNo}", "Get")]
    public class List_Container : IReturn<CommonResponse>
    {
        public string PhoneNumber { get; set; }
        public string JobNo { get; set; }
    }
    public class List_Container_Response
    {
        public int Index { get; set; }
        public string JobNo { get; set; }
        public int JobLineItemNo { get; set; }
        public int LineItemNo { get; set; }
        public string ContainerNo { get; set; }
        public string Description { get; set; }
        public string Remark { get; set; }
        public string ItemName { get; set; }
        public string AllowSkipFlag { get; set; }
    }
    public class List_Container_Logic
    {
        private class Jmjm3
        {
            public string JobNo { get; set; }
            public int LineItemNo { get; set; }
            public string Description { get; set; }
        }
        private class Jmjm4
        {
            public string JobNo { get; set; }
            public int JobLineItemNo { get; set; }
            public int LineItemNo { get; set; }
            public string ContainerNo { get; set; }
            public string PhoneNumber { get; set; }
            public string DoneFlag { get; set; }
            public string Remark { get; set; }
            public string ItemName { get; set; }
        }
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public IConnectString ConnectString { get; set; }
        public List<List_Container_Response> GetList(List_Container request)
        {
            List<List_Container_Response> Result = null;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {
                    Result = db.Select<List_Container_Response>(
                        db.From<Jmjm4>()
                        .LeftJoin<Jmjm4, Jmjm3>((j4, j3) => j4.JobNo == j3.JobNo && j4.JobLineItemNo == j3.LineItemNo)
                        .Where(j4 => j4.PhoneNumber == request.PhoneNumber && j4.JobNo == request.JobNo && (j4.DoneFlag != "Y" || j4.DoneFlag == null))
                    );
                }
            }
            catch { throw; }
            return Result;
        }
    }
}
