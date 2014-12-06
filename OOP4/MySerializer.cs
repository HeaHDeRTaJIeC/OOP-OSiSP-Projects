using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AbstructClasses;

namespace OOP4
{
    public class MySerializer
    {
        public void Serialize(StreamWriter serializationStream, object graph)
        {
            serializationStream.WriteLine(((IList)graph).Count);
            Type itemlist = graph.GetType();
            //var i = itemlist as ICollection;
            if (itemlist.IsGenericType)
            {
                Type[] itemType = itemlist.GenericTypeArguments;
                if (itemType[0] != null)
                {
                    if (itemType[0].IsClass)
                    {
                        foreach (var item in (IList)graph)
                            SerializeClass(serializationStream, item);
                    }
                }
            }
        }

        private void SerializeClass(StreamWriter serializationStream, object graph)
        {
            Type itemType = graph.GetType();
            PropertyInfo[] prInfo = itemType.GetProperties();
            serializationStream.WriteLine(itemType.Name);
            foreach(var pr in prInfo)
            {
                serializationStream.WriteLine(pr.Name);
                DefineType(serializationStream, pr.GetValue(graph));
            }
            serializationStream.WriteLine(itemType.Name);
        }

        private void SerializeEnum(StreamWriter serializationStream, object graph)
        {
            Type itemType = graph.GetType();
            serializationStream.WriteLine(Enum.GetName(itemType, graph));
        }

        private void SerializePrimitive(StreamWriter serializationStream, object graph)
        {
            if (graph is int)
                serializationStream.WriteLine((int)graph);
            else if (graph is double)
                serializationStream.WriteLine((double)graph);
            else if (graph is bool)
            {
                if ((bool)graph)
                    serializationStream.WriteLine("true");
                else
                    serializationStream.WriteLine("false");
            }
            else
            {
                var s = graph as string;
                if (s != null)
                    serializationStream.WriteLine(s);
            }
        }

        private void DefineType(StreamWriter serializationStream,object graph)
        {
            Type itemtype = graph.GetType();
            if (itemtype.IsPrimitive || graph is String)
                SerializePrimitive(serializationStream, graph);
            else if (itemtype.IsEnum)
                SerializeEnum(serializationStream, graph);
            else if (itemtype.IsClass)
                SerializeClass(serializationStream, graph);
        }

        private object DefineType(StreamReader deserializationStream, object item, PropertyInfo prInfo)
        {
            Type prType = prInfo.PropertyType;
            string temp;
            if (prType.IsPrimitive || prType == typeof(String))
            {
                object value = prInfo.GetValue(item);
                if (value is int)
                {
                    temp = deserializationStream.ReadLine();
                    if (temp != null) 
                        prInfo.SetValue(item, int.Parse(temp));
                }
                else if (value is double)
                {
                    temp = deserializationStream.ReadLine();
                    if (temp != null) 
                        prInfo.SetValue(item, double.Parse(temp));
                }
                else if (value == null || value is String)
                {
                    temp = deserializationStream.ReadLine();
                    prInfo.SetValue(item, temp);
                }
                else if (value is bool)
                {
                    temp = deserializationStream.ReadLine();
                    prInfo.SetValue(item, temp == "true");
                }
            }
            else if (prType.IsEnum)
            {
                temp = deserializationStream.ReadLine();
                Array arr = Enum.GetNames(prType);
                int index = Array.IndexOf(arr, temp);
                prInfo.SetValue(item, index);
            }
            else if (prType.IsClass)
            {
                prInfo.SetValue(item, DeserializeClass(deserializationStream));
            }

            return item;
        }

        public object Deserialize(StreamReader deserializationStream, List<Type> listOfTypes)
        {
            this.listOfTypes = listOfTypes;
            List<Weapons> result = new List<Weapons>();
            if (deserializationStream != null)
            {
                int count = int.Parse(deserializationStream.ReadLine());
                for (int i = 0; i < count; i++)
                {
                    String temp = deserializationStream.ReadLine();
                    Type itemType = listOfTypes.FirstOrDefault(
                        x => x.Name == temp);
                    if (itemType != null)
                    {
                        object item = Activator.CreateInstance(itemType);
                        item = DeserializeProperties(deserializationStream, item);
                        result.Add((Weapons)item);
                    }
                }
            }
            return result;
        }

        private object DeserializeClass(StreamReader deserializationStream)
        {
            String temp = deserializationStream.ReadLine();
            Type itemType = listOfTypes.FirstOrDefault(
                x => x.Name == temp);
            if (itemType != null)
            {
                object item = Activator.CreateInstance(itemType);
                item = DeserializeProperties(deserializationStream, item);
                return item;
            }
            return null;
        }

        private object DeserializeProperties(StreamReader deserializationStream, object item)
        {
            Type itemType = item.GetType();
            PropertyInfo[] prInfo = itemType.GetProperties();
            int count = prInfo.Length;
            string temp = deserializationStream.ReadLine();
            while (temp != itemType.Name)
            {
                for (int j = 0; j < count; j++)
                    if (temp == prInfo[j].Name)
                        item = DefineType(deserializationStream, item, prInfo[j]);
                temp = deserializationStream.ReadLine();
            }
            return item;
        }

        private List<Type> listOfTypes = new List<Type>();
    }
}
