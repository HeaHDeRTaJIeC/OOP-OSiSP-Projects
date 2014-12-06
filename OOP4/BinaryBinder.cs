using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace OOP4
{
    public class MyBinaryBinder : SerializationBinder
    {
        public MyBinaryBinder(List<Type> listOfTypes)
        {
            this.listOfTypes = listOfTypes;
        }

        public override Type BindToType(string assemblyName, string typeName)
        {
            return listOfTypes.FirstOrDefault(
                x => x.Assembly.FullName == assemblyName && x.FullName == typeName) ??
                Type.GetType(typeName + ", " + assemblyName);
        }

        private readonly List<Type> listOfTypes;
    }
}
