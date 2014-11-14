using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace OOP3
{
    static class TreeBuilder
    {
        public static TreeNode GetTree(List<Weapons> Items)
        {
            TreeNode Result = new TreeNode("root") 
            { 
                Tag = Items 
            };
            foreach(var item in Items)
            {
                Result.Nodes.Add(GetNodeByItem(item));
            }

            return Result;
        }

        private static TreeNode GetNodeByItem(object Item)
        {
            Type ItemType = Item.GetType();
            TreeNode Result = new TreeNode(ItemType.Name) 
            { 
                Tag = new TreeNodeTag { NodeType = ItemType, Value = Item } 
            };
            PropertyInfo[] Properties = ItemType.GetProperties();
            foreach (var Property in Properties)
            {
                Result.Nodes.Add(GetNodeByProperty(Property.GetValue(Item) , Property));
            }

            return Result;
        }

        private static TreeNode GetNodeByProperty(object Item, PropertyInfo ItemProperty)
        {
            Type ItemType = Item.GetType();
            TreeNode Result;

            if (!ItemType.IsValueType && !(Item is String))
            {
                Result = TreeNodeFromProperty(Item, ItemProperty, ItemType);
            }
            else
            {
                Result = new TreeNode(ItemProperty.Name + " = " + Item.ToString());
            }
            Result.Tag = new TreeNodeTag
            {
                NodeType = ItemType,
                Value = Item,
                PropertiesInfo = ItemProperty
            };

            return Result;
        }

        public static TreeNode TreeNodeFromProperty(object Item, PropertyInfo ItemProperty, Type ItemType)
        {
            TreeNode Result = new TreeNode(ItemProperty.Name);
            PropertyInfo[] Properties = ItemType.GetProperties();
            foreach (var property in Properties)
            {
                Result.Nodes.Add(GetNodeByProperty(property.GetValue(Item), property));
            }

            return Result;
        }


    }

}