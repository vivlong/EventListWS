using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.ServiceModel;
using API.ServiceModel.Event;
using API.ServiceInterface.Event;
using System.IO;

namespace API.ServiceInterface
{
    public class EventServices : Service
    {        
        public Auth auth { get; set; }
        public List_Login_Logic list_Login_Logic { get; set; }
        public List_Container_Logic list_Container_Logic { get; set; }
        public List_JobNo_Logic list_JobNo_Logic { get; set; }
        public Update_Done_Logic update_Done_Logic { get; set; }
        public object Any(List_Login request)
        {
            CommonResponse ecr = new CommonResponse();
            ecr.initial();
            try 
            {
                LoginService ls = new LoginService();
                ls.initial(auth, request, list_Login_Logic, ecr, this.Request.Headers.GetValues("Signature"), this.Request.RawUrl);
            }
            catch (Exception ex){
                ecr.meta.code = 599;
                ecr.meta.message = "The server handle exceptions, the operation fails.";
                ecr.meta.errors.code = ex.HResult;
                ecr.meta.errors.field = ex.HelpLink;
                ecr.meta.errors.message = ex.Message.ToString();
            }            
            return ecr;
        }
        public object Any(List_JobNo request)
        {
            CommonResponse ecr = new CommonResponse();
            ecr.initial();
            try
            {
                ListService ls = new ListService();
                ls.ListJobNo(auth, request, list_JobNo_Logic, ecr, this.Request.Headers.GetValues("Signature"), this.Request.RawUrl);
            }
            catch (Exception ex)
            {
                ecr.meta.code = 599;
                ecr.meta.message = "The server handle exceptions, the operation fails";
                ecr.meta.errors.code = ex.HResult;
                ecr.meta.errors.field = ex.HelpLink;
                ecr.meta.errors.message = ex.Message.ToString();
            }
            return ecr;
        }
        public object Any(List_Container request)
        {
            CommonResponse ecr = new CommonResponse();
            ecr.initial();
            try
            {
                ListService ls = new ListService();
                ls.ListContainer(auth, request, list_Container_Logic, ecr, this.Request.Headers.GetValues("Signature"), this.Request.RawUrl);
            }
            catch (Exception ex)
            {
                ecr.meta.code = 599;
                ecr.meta.message = "The server handle exceptions, the operation fails.";
                ecr.meta.errors.code = ex.HResult;
                ecr.meta.errors.field = ex.HelpLink;
                ecr.meta.errors.message = ex.Message.ToString();
            }
            return ecr;
        }
        public object Any(Update_Done request)
        {
            CommonResponse ecr = new CommonResponse();
            ecr.initial();
            try
            {
                DoneService ds = new DoneService();
                ds.initial(auth, request, update_Done_Logic, ecr, this.Request.Headers.GetValues("Signature"), this.Request.RawUrl);
            }
            catch (Exception ex)
            {
                ecr.meta.code = 599;
                ecr.meta.message = "The server handle exceptions, the operation fails.";
                ecr.meta.errors.code = ex.HResult;
                ecr.meta.errors.field = ex.HelpLink;
                ecr.meta.errors.message = ex.Message.ToString();
            }
            return ecr;
        }
    }
}
