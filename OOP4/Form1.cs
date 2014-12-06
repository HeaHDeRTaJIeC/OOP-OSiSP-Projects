using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Xml.Serialization;
using AbstructClasses;
using IConverterInterface;

namespace OOP4
{
    public partial class Form1 : Form
    {
        List<Type> listOfClasses = new List<Type>();
        List<Type> listOfTypes = new List<Type>();
        Dictionary<string, Type> converters = new Dictionary<string, Type>();
        List<Weapons> list = new List<Weapons>();
        TreeNode chosenNode;
        private TreeNodeTag selectedNode;

        public Form1()
        {
            InitializeComponent();

            AddClass();
            GetConverters();
            propertyBox.Enabled = false;
            propertyButton.Enabled = false;
            for (int i = 0; i < 4; i++)
                propertyBox.Controls[i].Enabled = false;
        }


        private void GetConverters()
        {
            var dir = new DirectoryInfo("Converters");
            Type convertersInterface = dir.GetFiles("*.dll").
                Select(x => Assembly.LoadFrom(x.FullName).GetTypes().
                FirstOrDefault(y => y.IsInterface)).First();
            Type baseType = typeof(IConverter);
            Type[] convertersTypes = dir.GetFiles("*.dll").
                Select(x => Assembly.LoadFrom(x.FullName).
                    GetExportedTypes().
                    Where(baseType.IsAssignableFrom)).
                Aggregate(new List<Type>(), (acc, x) =>
                {
                    acc.AddRange(x);
                    return acc;
                }).ToArray();
            foreach (var converterType in convertersTypes)
            {
                converters.Add(converterType.Name, converterType);
                jsonBox.Items.Add(converterType.Name);
            }
            jsonBox.Enabled = false;

        }

        private void AddClass()
        {
            var directory = new DirectoryInfo("Classes");
            listOfClasses = directory.GetFiles("*.dll").
                Select(x => Assembly.LoadFile(x.FullName).GetTypes().
                    Where(y => y.IsSubclassOf(typeof(Weapons))))
                    .Aggregate(new List<Type>(), (acc, x) => { acc.AddRange(x); return acc; });

            listOfTypes = directory.GetFiles("*.dll").
                Select(x => Assembly.LoadFile(x.FullName).GetTypes()).
                Aggregate(new List<Type>(), (acc, x) => { acc.AddRange(x); return acc; });
            listOfTypes.Add(typeof(Bullets));
            foreach (var type in listOfClasses)
            {
                var menuItem = new ToolStripButton
                {
                    Tag = type,
                    Name = type.Name,
                    Text = type.Name
                };
                menuItem.Click += WeaponClick;
                addToolStripMenuItem.DropDownItems.Add(menuItem);
            }
        }

        private void WeaponClick(object sender, EventArgs e)
        {
            Random rand = new Random();
            Weapons item = (Weapons)Activator.CreateInstance((Type)((ToolStripItem)sender).Tag);
            item.WeaponTitle = item.GetType().Name;
            item.Effectivedistance = (rand.Next(5) * 100) + 400;
            ((Firearm)item).BulletSpeed = (rand.Next(5) * 100) + 400;
            list.Add(item);
            RefreshTree();
        }

        private void SerializeButton_Click(object sender, EventArgs e)
        {
            int index = serializeBox.SelectedIndex;
            switch (index)
            {
                case 0:                                 //binary
                    var bf = new BinaryFormatter { Binder = new MyBinaryBinder(listOfTypes) };
                    using (var binaryFileStream = new FileStream("binary.txt", FileMode.Create))
                    {
                        bf.Serialize(binaryFileStream, list);
                    }
                    break;
                case 1:                                 //XML
                    Type[] extraTypes = listOfClasses.ToArray();
                    var xmlSerializer = new XmlSerializer(typeof(List<Weapons>), extraTypes);
                    using (var xmlFileStream = new FileStream("xml.txt", FileMode.Create))
                    {
                        var stringWriter = new StringWriter();
                        xmlSerializer.Serialize(stringWriter, list);

                        string key = jsonBox.SelectedItem.ToString();
                        Type jsonConverterType = converters[key];
                        var jsonConverter = (IConverter)Activator.CreateInstance(jsonConverterType);
                        var fileWriterEmpty = new StringWriter();
                        fileWriterEmpty.Write(jsonConverter.ConvertOutput(stringWriter.GetStringBuilder().ToString()));
                        for (int i = 0; i < fileWriterEmpty.GetStringBuilder().Length; i++)
                            xmlFileStream.WriteByte((byte)fileWriterEmpty.GetStringBuilder()[i]);
                    }
                    break;
                case 2:                                 //user
                    var userSerializer = new MySerializer();
                    using (var userFileStream = new FileStream("user.txt", FileMode.Create))
                    {
                        var streamWriter = new StreamWriter(userFileStream) { AutoFlush = true };
                        userSerializer.Serialize(streamWriter, list);
                    }
                    break;
            }

        }

        private void DeserializeButton_Click(object sender, EventArgs e)
        {
            int index = serializeBox.SelectedIndex;
            switch (index)
            {
                case 0:                                     //binary
                    var bf = new BinaryFormatter { Binder = new MyBinaryBinder(listOfTypes) };
                    using (var binaryFileStream = new FileStream("binary.txt", FileMode.Open))
                    {
                        list.Clear();
                        list = (List<Weapons>)bf.Deserialize(binaryFileStream);
                    }
                    break;
                case 1:                                     //xml
                    Type[] extraTypes = listOfClasses.ToArray();
                    var xmlSerializer = new XmlSerializer(typeof(List<Weapons>), extraTypes);
                    using (var xmlFileStream = new FileStream("xml.txt", FileMode.Open))
                    {
                        var stringWriter = new StringWriter();
                        var streamReader = new StreamReader(xmlFileStream);
                        string fullText = streamReader.ReadToEnd();
                        list.Clear();

                        string key = jsonBox.SelectedItem.ToString();
                        Type jsonConverterType = converters[key];
                        var jsonConverter = (IConverter)Activator.CreateInstance(jsonConverterType);
                        stringWriter.Write(jsonConverter.ConvertInput(fullText));
                        var stringReader = new StringReader(stringWriter.GetStringBuilder().ToString());
                        list = (List<Weapons>)xmlSerializer.Deserialize(stringReader);
                    }
                    break;
                case 2:
                    var userSerializer = new MySerializer();
                    using (var userFileStream = new FileStream("user.txt", FileMode.Open))
                    {
                        var streamReader = new StreamReader(userFileStream);
                        list.Clear();
                        list = (List<Weapons>)userSerializer.Deserialize(streamReader, listOfTypes);

                    }
                    break;
            }

            RefreshTree();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            DisableControls();
            TreeNodeTag nodeTag = e.Node.Tag as TreeNodeTag; 

            if (nodeTag != null)
                if (nodeTag.NodeType.IsValueType || nodeTag.Value is String)
                    if (nodeTag.PropertiesInfo.CanWrite)
                    {
                        chosenNode = e.Node;
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
                        else if (nodeTag.Value is double)
                        {
                            doubleBox.Text = nodeTag.Value.ToString();
                            doubleBox.Enabled = true;
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

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            selectedNode = e.Node.Tag as TreeNodeTag;
        }

        private void treeView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'r' || e.KeyChar == 'R')
                if (selectedNode != null && selectedNode.NodeType.IsSubclassOf(typeof(Weapons)))
                {
                    list.Remove((Weapons)selectedNode.Value);
                    RefreshTree();
                }
        }

        void DisableControls()
        {
            propertyBox.Enabled = false;
            propertyButton.Enabled = false;
            boolComboBox.Enabled = false;
            doubleBox.Enabled = false;
            doubleBox.Clear();
            enumComboBox.Enabled = false;
            enumComboBox.Items.Clear();
            numericUpDown.Enabled = false;
            textBox.Enabled = false;
            textBox.Clear();

        }

        private void RefreshTree()
        {
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add(TreeBuilder.GetTree(list));
        }

        private void propertyButton_Click(object sender, EventArgs e)
        {
            if (chosenNode != null)
            {
                TreeNodeTag nodeTag = (TreeNodeTag)chosenNode.Tag;
                TreeNodeTag parentTag = (TreeNodeTag)chosenNode.Parent.Tag;
                chosenNode.Name = nodeTag.PropertiesInfo.Name + " = ";
                if (nodeTag.Value is int)
                {
                    chosenNode.Name += numericUpDown.Value.ToString(CultureInfo.InvariantCulture);
                    nodeTag.PropertiesInfo.SetValue(parentTag.Value, (int) numericUpDown.Value);
                    nodeTag.Value = numericUpDown.Value;
                }
                else if (nodeTag.Value is String)
                {
                    chosenNode.Name += textBox.Text;
                    nodeTag.PropertiesInfo.SetValue(parentTag.Value, textBox.Text);
                    nodeTag.Value = textBox.Text;
                }
                else if (nodeTag.Value is bool)
                {
                    if (boolComboBox.SelectedIndex == 0)
                    {
                        chosenNode.Name += "True";
                        nodeTag.PropertiesInfo.SetValue(parentTag.Value, true);
                        nodeTag.Value = true;
                    }
                    else
                    {
                        chosenNode.Name += "False";
                        nodeTag.PropertiesInfo.SetValue(parentTag.Value, false);
                        nodeTag.Value = false;
                    }
                }
                else if (nodeTag.Value is double)
                {
                    double temp;
                    try
                    {
                        temp = double.Parse(doubleBox.Text);
                    }
                    catch
                    {
                        MessageBox.Show("Incorrect value");
                        return;
                    }
                    chosenNode.Name += temp.ToString(CultureInfo.InvariantCulture);
                    nodeTag.PropertiesInfo.SetValue(parentTag.Value, temp);
                    nodeTag.Value = temp;
                }
                else
                {
                    var item = enumComboBox.Items[enumComboBox.SelectedIndex];
                    chosenNode.Name += item.ToString();
                    nodeTag.PropertiesInfo.SetValue(parentTag.Value, item);
                    nodeTag.Value = item;
                }
            }
            DisableControls();
            RefreshTree();
        }

        private void serializeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            jsonBox.Enabled = serializeBox.SelectedIndex == 1;
        }


    }
}
