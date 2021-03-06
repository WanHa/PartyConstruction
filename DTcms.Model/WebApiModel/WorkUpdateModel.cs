﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
    public class WorkUpdateModel
    {/// <summary>
    /// user表
    /// </summary>
        
        public int userid { get; set; }

        public int group_id { get; set; }

        public string name { get; set; }

        public string avatar { get; set; }

        public string id { get; set; }

        public string title { get; set; }

        public string content { get; set; }

        private string _time;
        public string time
        {
            get
            {
                return /*DateTime.Parse(*/_time == null ? "" : _time.ToString(/*"yyyy年MM月dd日"*/);
            }
            set
            {
                _time = value;
            }
        }

        public int groupid { get; set; }

        public int managerid { get; set; }

        public int count { get; set; }

        public int feedback { get; set; }



    }
}
