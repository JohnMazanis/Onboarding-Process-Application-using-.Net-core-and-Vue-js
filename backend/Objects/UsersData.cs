using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Objects
{
    public class UsersData
    {
        public int? id { get; set; }
        public string txtUsernamecreate { get; set; }
        public string txtPasswordcreate { get; set; }
        public string txtfirstname { get; set; }
        public string txtlastname { get; set; }
        public string txtemail { get; set; }
        public DateTime dthiredate { get; set; }
        public int? position { get; set; }
        public int? department { get; set; }
        public int? statusID { get; set; }
    }
}
