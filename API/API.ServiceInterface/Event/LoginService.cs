using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TmsWS.ServiceModel;
using TmsWS.ServiceModel.Event;

namespace TmsWS.ServiceInterface.Event
{
    public class LoginService
    {
        public void initial(Auth auth, List_Login request, List_Login_Logic eventloginLogic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {
                if (eventloginLogic.LoginCheck(request) > 0)
                {
                    ecr.meta.code = 200;
                    ecr.meta.message = "OK";
                    ecr.data.results = eventloginLogic.GetUserInfo(request);
                }
                else
                {
                    ecr.meta.code = 612;
                    ecr.meta.message = "Invalid Phone Number";
                }
            }
            else
            {
                ecr.meta.code = 401;
                ecr.meta.message = "Unauthorized";
            }
        }
    }
}
