using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using AbstructClasses;

namespace OOP4
{
    static class TreeBuilder
    {
        public static TreeNode GetTree(List<Weapons> items)
        {
            TreeNode result = new TreeNode("root") 
            { 
                Tag = items 
            };
            foreach(var item in items)
            {
                result.Nodes.Add(GetNodeByItem(item));
            }

            return result;
        }

        private static TreeNode GetNodeByItem(object item)
        {
            Type itemType = item.GetType();
            TreeNode result = new TreeNode(itemType.Name) 
            { 
                Tag = new TreeNodeTag { NodeType = itemType, Value = item } 
            };
            PropertyInfo[] properties = itemType.GetProperties();
            foreach (var property in properties)
            {
                result.Nodes.Add(GetNodeByProperty(property.GetValue(item) , property));
            }

            return result;
        }

        private static TreeNode GetNodeByProperty(object item, PropertyInfo itemProperty)
        {
            Type itemType = item.GetType();
            TreeNode result;

            if (!itemType.IsValueType && !(item is String))
            {
                result = TreeNodeFromProperty(item, itemProperty, itemType);
            }
            else
            {
                result = new TreeNode(itemProperty.Name + " = " + item);
            }
            result.Tag = new TreeNodeTag
            {
                NodeType = itemType,
                Value = item,
                PropertiesInfo = itemProperty
            };

            return result;
        }

        public static TreeNode TreeNodeFromProperty(object item, PropertyInfo itemProperty, Type itemType)
        {
            TreeNode result = new TreeNode(itemProperty.Name);
            PropertyInfo[] properties = itemType.GetProperties();
            foreach (var property in properties)
            {
                result.Nodes.Add(GetNodeByProperty(property.GetValue(item), property));
            }

            return result;
        }


    }

}