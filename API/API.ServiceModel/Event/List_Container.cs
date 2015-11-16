﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ServiceStack;
using ServiceStack.ServiceHost;
using ServiceStack.OrmLite;

namespace TmsWS.ServiceModel.Event
{
    [Route("/event/action/list/container/{PhoneNumber}/{JobNo}", "Get")]
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
            public string EventCode { get; set; }
        }
        private class Jmje1
        {
            public string EventCode { get; set; }
            public string AllowSkipFlag { get; set; }
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
        public List<List_Container_Response> GetList(List_Container request)
        {
            List<List_Container_Response> Result = null;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {
                    Result = db.Select<List_Container_Response>(
                        "Select * From Jmjm4 Left Join Jmjm3 On Jmjm4.JobNo=Jmjm3.JobNo And Jmjm4.JobLineItemNo=Jmjm3.LineItemNo " +
                        "Left Join Jmje1 On Jmjm3.EventCode=Jmje1.EventCode " +
                        "Where Jmjm4.PhoneNumber={0} And Jmjm4.JobNo={1} And IsNull(Jmjm4.DoneFlag,'')<>'Y'"
                        , request.PhoneNumber, request.JobNo
                    );
                }
            }
            catch { throw; }
            return Result;
        }
    }
}
