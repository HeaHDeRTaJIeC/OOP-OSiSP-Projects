using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace OOP3
{
    public partial class Form1 : Form
    {
        List<Type> ListOfTypes = new List<Type>();
        List<Weapons> list = new List<Weapons>();
        TreeNode ChosenNode;

        public Form1()
        {
            InitializeComponent();

            AddClass();
            propertyBox.Enabled = false;
            propertyButton.Enabled = false;
            for (int i = 0; i < 4; i++)
                propertyBox.Controls[i].Enabled = false;
        }

        private void AddClass()
        {
            var directory = new DirectoryInfo("ProjectDll");
            ListOfTypes = directory.GetFiles("*.dll").
                Select(x => Assembly.LoadFile(x.FullName).GetTypes().
                    Where(y => y.IsSubclassOf(typeof(Weapons))))
                    .Aggregate(new List<Type>(), (acc, x) => { acc.AddRange(x); return acc; });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label5.Text = "";
            Weapons Item;
            foreach (var type in ListOfTypes)
            {
                label5.Text += (type.Name + " ");
                Item = (Weapons)Activator.CreateInstance(type);
                Item.WeaponTitle = type.Name;
                list.Add(Item);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream("file.txt", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            foreach(Weapons item in list)
                label1.Text += item.WeaponTitle;
            bf.Serialize(fs, list);
            fs.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            label2.Text = "";
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream("file.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
            list = new List<Weapons>();
            list = (List<Weapons>)bf.Deserialize(fs);
            foreach (Weapons item in list)
                label2.Text += item.GetType().ToString();
            fs.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label3.Text = "";
            Type[] extraTypes = new Type[ListOfTypes.Count];
            extraTypes = ListOfTypes.ToArray();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Weapons>), extraTypes);
            using (FileStream fs = new FileStream("persons.xml", FileMode.OpenOrCreate))
            {
                foreach (var item in list)
                    label3.Text += item.WeaponTitle;
                xmlSerializer.Serialize(fs, list);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label4.Text = "";
            list = new List<Weapons>();
            Type[] extraTypes = new Type[ListOfTypes.Count];
            extraTypes = ListOfTypes.ToArray();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Weapons>), extraTypes);
            using (FileStream fs = new FileStream("persons.xml", FileMode.OpenOrCreate))
            {
                list = (List<Weapons>)xmlSerializer.Deserialize(fs);
            }

            foreach (var item in list)
                label4.Text += item.GetType().ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Add(TreeBuilder.GetTree(list));
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            DisableControls();
            TreeNodeTag nodeTag = e.Node.Tag as TreeNodeTag; 

            if (nodeTag != null)
                if (nodeTag.NodeType.IsValueType || nodeTag.Value is String)
                    if (nodeTag.PropertiesInfo.CanWrite)
                    {
                        ChosenNode = e.Node;
                        propertyBox.Enabled = true;
                        propertyButton.Enabled = true;
                        if (nodeTag.Value is int)
                        {
                            numericUpDown.Value = (int)nodeTag.Value;
                            numericUpDown.Enabled = true;
                        }
                        else if (nodeTag.Value is String)
                        {
                            textBox.Text = nodeTag.Value.ToString();
                            textBox.Enabled = true;
                        }
                        else if (nodeTag.Value is bool)
                        {
                            if ((bool)nodeTag.Value)
                                boolComboBox.SelectedIndex = 0;
                            else
                                boolComboBox.SelectedIndex = 1;
                            boolComboBox.Enabled = true;
                        }
                        else
                        {
                            foreach (var item in Enum.GetValues(nodeTag.Value.GetType()))
                            {
                                enumComboBox.Items.Add(item);
                                if (item.Equals(nodeTag.Value))
                                    enumComboBox.SelectedIndex = enumComboBox.Items.IndexOf(item);
                            }
                            enumComboBox.Enabled = true;
                        }
                    }
        }

        void DisableControls()
        {
            propertyBox.Enabled = false;
            propertyButton.Enabled = false;
            boolComboBox.Enabled = false;
            enumComboBox.Enabled = false;
            enumComboBox.Items.Clear();
            numericUpDown.Enabled = false;
            textBox.Enabled = false;
            textBox.Clear();

        }

        private void propertyButton_Click(object sender, EventArgs e)
        {
            if (ChosenNode != null)
            {
                TreeNodeTag nodeTag = (TreeNodeTag)ChosenNode.Tag;
                TreeNodeTag parentTag = (TreeNodeTag)ChosenNode.Parent.Tag;
                ChosenNode.Name = nodeTag.PropertiesInfo.Name + " = ";
                if (nodeTag.Value is int)
                {
                    ChosenNode.Name += numericUpDown.Value.ToString();
                    nodeTag.PropertiesInfo.SetValue(parentTag.Value, (int) numericUpDown.Value);
                    nodeTag.Value = numericUpDown.Value;
                }
                else if (nodeTag.Value is String)
                {
                    ChosenNode.Name += textBox.Text;
                    nodeTag.PropertiesInfo.SetValue(parentTag.Value, textBox.Text);
                    nodeTag.Value = textBox.Text;
                }
                else if (nodeTag.Value is bool)
                {
                    if (boolComboBox.SelectedIndex == 0)
                    {
                        ChosenNode.Name += "True";
                        nodeTag.PropertiesInfo.SetValue(parentTag.Value, true);
                        nodeTag.Value = true;
                    }
                    else
                    {
                        ChosenNode.Name += "False";
                        nodeTag.PropertiesInfo.SetValue(parentTag.Value, false);
                        nodeTag.Value = false;
                    }
                }
                else
                {
                    var item = enumComboBox.Items[enumComboBox.SelectedIndex];
                    ChosenNode.Name += item.ToString();
                    nodeTag.PropertiesInfo.SetValue(parentTag.Value, item);
                    nodeTag.Value = item;
                }
            }
            DisableControls();
            RefreshTree();
        }

        private void RefreshTree()
        {
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add(TreeBuilder.GetTree(list));
        }

    }
}
