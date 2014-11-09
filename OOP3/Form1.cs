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
using System.Runtime.Serialization.Formatters.Binary;

namespace OOP3
{
    public partial class Form1 : Form
    {
        List<Weapons> list = new List<Weapons>();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Shotguns shotgun = new Shotguns();
            shotgun.Aim = TypeOfAim.Mechanical;
            shotgun.Bullet.Caliber = 9.4;
            shotgun.Bullet.Type = TypeOfBullet.Shot;
            shotgun.BulletSpeed = 500;
            shotgun.Effectivedistance = 50;
            shotgun.ShotReload = TypeOfShotReload.Manual;
            shotgun.NumberOfBullets = 7;
            shotgun.ReloadMechanism = TypeOfReload.PrechargeReload;
            shotgun.ShotReload = TypeOfShotReload.Manual;
            shotgun.WeaponTitle = "ShotGun M11";

            AutomaticRifles autoRifle = new AutomaticRifles();
            autoRifle.Aim = TypeOfAim.Holographic;
            autoRifle.Bayonet = true;
            autoRifle.Bullet.Caliber = 7.62;
            autoRifle.Bullet.Type = TypeOfBullet.Usual;
            autoRifle.BulletSpeed = 900;
            autoRifle.Effectivedistance = 800;
            autoRifle.Grenade = TypeOfGrenade.Bursting;
            autoRifle.Mode = FiringMode.Queues;
            autoRifle.NBulletsPerShot = 1;
            autoRifle.NumberOfBullets = 30;
            autoRifle.ReloadMechanism = TypeOfReload.MagazineReload;
            autoRifle.Silencer = false;
            autoRifle.WeaponTitle = "AK-47";

            SemiautomaticSniperRifles sniperRifle = new SemiautomaticSniperRifles();
            sniperRifle.Aim = TypeOfAim.Optical;
            sniperRifle.Bullet.Caliber = 11.2;
            sniperRifle.Bullet.Type = TypeOfBullet.ArmorPiercing;
            sniperRifle.BulletSpeed = 1200;
            sniperRifle.Effectivedistance = 1500;
            sniperRifle.FireMode = TypeOfShotReload.Manual;
            sniperRifle.NumberOfBullets = 10;
            sniperRifle.ReloadMechanism = TypeOfReload.MagazineReload;
            sniperRifle.Silencer = true;
            sniperRifle.Zoom = 16;
            sniperRifle.WeaponTitle = "SVD";

            SubmachineGun submachineGun = new SubmachineGun();
            submachineGun.Aim = TypeOfAim.Laser;
            submachineGun.Bullet.Caliber = 9;
            submachineGun.Bullet.Type = TypeOfBullet.Incendiary;
            submachineGun.Effectivedistance = 300;
            submachineGun.Mode = FiringMode.NBulletsPerShot;
            submachineGun.NBulletsPerShot = 3;
            submachineGun.NumberOfBullets = 20;
            submachineGun.Silencer = true;
            submachineGun.WeaponTitle = "Glock";

            list.Add((Weapons)submachineGun);
            list.Add((Weapons)shotgun);
            list.Add((Weapons)sniperRifle);
            list.Add((Weapons)autoRifle);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream("file.txt", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            foreach(var item in list)
                label1.Text += item.WeaponTitle;
            bf.Serialize(fs, list);
            fs.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Type[] extraTypes = new Type[]{typeof(Pistols), typeof(SubmachineGun), 
            //    typeof(SniperRifles), typeof(SemiautomaticSniperRifles),
            //    typeof(AutomaticRifles), typeof(Shotguns)};
            label1.Text = "";
            label2.Text = "";
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream("file.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
            list = new List<Weapons>();
            list = (List<Weapons>)bf.Deserialize(fs);
            foreach (var item in list)
                label2.Text += item.GetType().ToString();
            fs.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label3.Text = "";
            using (FileStream fs = new FileStream("persons.xml", FileMode.OpenOrCreate))
            {
                foreach (var item in list)
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(item.GetType());
                    xmlSerializer.Serialize(fs, item);
                    label3.Text += item.WeaponTitle;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label4.Text = "";
            list = new List<Weapons>();
            //Type[] extraTypes = new Type[]{typeof(Pistols), typeof(SubmachineGun), 
            //    typeof(SniperRifles), typeof(SemiautomaticSniperRifles),
            //    typeof(AutomaticRifles), typeof(Shotguns)};
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Weapons>));
            using (FileStream fs = new FileStream("persons.xml", FileMode.OpenOrCreate))
            {
                list = (List<Weapons>)xmlSerializer.Deserialize(fs);
            }

            foreach (var item in list)
                label4.Text += item.GetType().ToString();
        }
    }
}
