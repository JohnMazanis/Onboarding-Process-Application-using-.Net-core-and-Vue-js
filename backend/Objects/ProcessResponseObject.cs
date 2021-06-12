using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Objects
{
    public class ProcessResponseObject
    {
        public bool ErrorFound { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> ErrorsList { get; set; }
    }
}
