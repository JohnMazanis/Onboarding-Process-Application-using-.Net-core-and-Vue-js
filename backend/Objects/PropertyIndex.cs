using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Objects
{
    /// <summary>
    /// Specifically set the index of the property declared in a class
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyIndex : Attribute
    {
        private int index;

        public PropertyIndex(int index)
        {
            this.index = index;
        }

        public int Index
        {
            get { return this.index; }
        }
    }
}
