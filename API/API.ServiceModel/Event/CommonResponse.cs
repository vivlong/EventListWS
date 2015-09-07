﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TmsWS.ServiceModel.Event
{
    public class CommonResponse_meta_errors
    {
        public int code { get; set; }
        public string field { get; set; }
        public string message { get; set; }
    }

    public class CommonResponse_meta
    {
        public int code { get; set; }
        public string message { get; set; }
        public CommonResponse_meta_errors errors { get; set; }
    }

    public class CommonResponse_data
    {
        public object results { get; set; }
    }

    public class CommonResponse
    {
        public CommonResponse_meta meta { get; set; }
        public CommonResponse_data data { get; set; }
        public void initial()
        {
            meta = new CommonResponse_meta();
            meta.errors = new CommonResponse_meta_errors();
            data = new CommonResponse_data();
        }
    }
}
