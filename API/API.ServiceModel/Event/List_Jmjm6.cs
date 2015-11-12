using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace TmsWS.ServiceModel.Event
{
    [Route("/event/action/list/jmjm6/{jobno}", "Get")]
    public class List_Jmjm6 : IReturn<CommonResponse>
    {
        public string JobNo { get; set; }
    }
    public class List_Jmjm6_Response
    {
        public string JobNo { get; set; }
        public int LineItemNo { get; set; }
        public string ContainerNo { get; set; }
        public string Remark { get; set; }
        public string JobType { get; set; }
        public string VehicleNo { get; set; }
        public string DriverNo { get; set; }
        public string CargoStatusCode { get; set; }
        public DateTime TruckDateTime { get; set; }
        public DateTime RecevieDateTime { get; set; }
        public DateTime ReadyDateTime { get; set; }
        public DateTime UnLoadDateTime { get; set; }
    }
    public class List_Jmjm6_Logic
    {
        private class Jmjm1
        {
            public string JobNo { get; set; }
            public string JobType { get; set; }
            public string StatusCode { get; set; }
        }
        private class Jmjm6
        {
            public string JobNo { get; set; }
            public int LineItemNo { get; set; }
            public string ContainerNo { get; set; }
            public string Remark { get; set; }
            public string VehicleNo { get; set; }
            public string DriverNo { get; set; }
            public string CargoStatusCode { get; set; }
            public DateTime TruckDateTime { get; set; }
            public DateTime RecevieDateTime { get; set; }
            public DateTime ReadyDateTime { get; set; }
            public DateTime UnLoadDateTime { get; set; }
        }
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public List<List_Jmjm6_Response> GetList(List_Jmjm6 request)
        {
            List<List_Jmjm6_Response> Result = null;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {
                    Result = db.Select<List_Jmjm6_Response>(
                        db.From<Jmjm6>()
                        .LeftJoin<Jmjm6, Jmjm1>((j6, j1) => j6.JobNo == j1.JobNo)
                        .Where<Jmjm1>(j1 => j1.StatusCode != "DEL" && j1.JobNo == request.JobNo)
                    );
                }
            }
            catch { throw; }
            return Result;
        }
    }
}
