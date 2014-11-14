using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace OOP3
{
    public class TreeNodeTag
    {
        public Object Value { get; set; }

        public Type NodeType { get; set; }

        public PropertyInfo PropertiesInfo { get; set; } 
    }
}
