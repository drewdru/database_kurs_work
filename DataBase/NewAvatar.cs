using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
namespace DataBase
{
    public partial class NewAvatar : Form
    {
        int[] arrayRaceID;
        int[] arrayClassID;
        String imagePath = "";
        int Health = 0, Mana = 0, Strenght = 0, Agility = 0, Defens = 0, Stamina = 0, Intellect = 0;
        String RaceInfo = "", ClassInfo = "", BonusInfo = "";
        public NewAvatar()
        {
            InitializeComponent();

            OleDbConnection Connection = new OleDbConnection(Login.Path);
            OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM Races", Connection);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "Races");
            DataTable table = dataSet.Tables[0];
            var LogIn =
                from account in dataSet.Tables[dataSet.Tables.IndexOf("Races")].AsEnumerable()
                select account;

            var idCollection2 = LogIn.Where(b => b.Field<int>("RaceID") > 0);
            arrayRaceID = new int[idCollection2.Count()];
            int i = 0;
            foreach (var row in idCollection2)
            {
                comboBox1.Items.Add(row.Field<String>("RaceName"));
                arrayRaceID[i] = row.Field<int>("RaceID");
                ++i;
            }
            comboBox1.SelectedIndex = 0;
            
            adapter = new OleDbDataAdapter("SELECT * FROM Classes", Connection);
            dataSet = new DataSet();
            adapter.Fill(dataSet, "Classes");
            table = new DataTable();
            table = dataSet.Tables[0];
            LogIn =
                from account in dataSet.Tables[dataSet.Tables.IndexOf("Classes")].AsEnumerable()
                select account;
            idCollection2 = LogIn.Where(b => b.Field<int>("ClassID") > 0);
            arrayClassID = new int[idCollection2.Count()];
            i = 0;
            foreach (var row in idCollection2)
            {
                comboBox2.Items.Add(row.Field<String>("ClassName"));
                arrayClassID[i] = row.Field<int>("ClassID");
                ++i;
            }
            comboBox2.SelectedIndex = 0;

            UpdateInfo();
        }

        public void UpdateInfo()
        {
            //обновление изображения
            OleDbConnection Connection = new OleDbConnection(Login.Path);
            OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM Avatar", Connection);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "Avatar");
            var LogIn =
                from account in dataSet.Tables[dataSet.Tables.IndexOf("Avatar")].AsEnumerable()
                where account.Field<int>("CharacterID") == 1 &&
                    account.Field<int>("AvatarRace") == arrayRaceID[comboBox1.SelectedIndex] &&
                    account.Field<int>("AvatarClass") == arrayClassID[comboBox2.SelectedIndex]
                select account;
            var idCollection2 = LogIn.Where(b => b.Field<int>("CharacterID") > 0);
            foreach (var row in idCollection2)
            {
                imagePath = row.Field<String>("AvatarModelPath");
            }
            pictureBox1.Load(imagePath);

            //обновление описаний
            Health = 0; Mana = 0; Strenght = 0; Agility = 0; Defens = 0; Stamina = 0; Intellect = 0;
            adapter = new OleDbDataAdapter("SELECT * FROM Classes", Connection);
            dataSet = new DataSet();
            adapter.Fill(dataSet, "Classes");
            LogIn =
                from account in dataSet.Tables[dataSet.Tables.IndexOf("Classes")].AsEnumerable()
                where account.Field<int>("ClassID") == arrayClassID[comboBox2.SelectedIndex]
                select account;
            idCollection2 = LogIn.Where(b => b.Field<int>("ClassID") > 0);            
            foreach (var row in idCollection2)
            {
                Health += row.Field<int>("HealthBonus");
                Mana += row.Field<int>("ManaBonus");
                Strenght += row.Field<int>("StrenghtBonus");
                Agility += row.Field<int>("AgilityBonus");
                Defens += row.Field<int>("DefensBonus");
                Stamina += row.Field<int>("StaminaBonus");
                Intellect += row.Field<int>("IntellectBonus");
                ClassInfo = row.Field<String>("Description");
            }
            adapter = new OleDbDataAdapter("SELECT * FROM Races", Connection);
            dataSet = new DataSet();
            adapter.Fill(dataSet, "Races");
            LogIn =
                from account in dataSet.Tables[dataSet.Tables.IndexOf("Races")].AsEnumerable()
                where account.Field<int>("RaceID") == arrayRaceID[comboBox1.SelectedIndex]
                select account;
            idCollection2 = LogIn.Where(b => b.Field<int>("RaceID") > 0);
            foreach (var row in idCollection2)
            {
                Health += row.Field<int>("HealthBonus");
                Mana += row.Field<int>("ManaBonus");
                Strenght += row.Field<int>("StrenghtBonus");
                Agility += row.Field<int>("AgilityBonus");
                Defens += row.Field<int>("DefensBonus");
                Stamina += row.Field<int>("StaminaBonus");
                Intellect += row.Field<int>("IntellectBonus");
                RaceInfo = row.Field<String>("Description");
            }
            BonusInfo = "+" + Health.ToString() + " к жизненной силе" + System.Environment.NewLine +
                "+" + Mana.ToString() + " к магической энергии" + System.Environment.NewLine +
                "+" + Strenght.ToString() + " к силе" + System.Environment.NewLine +
                "+" + Agility.ToString() + " к ловкостие" + System.Environment.NewLine +
                "+" + Defens.ToString() + " к защите" + System.Environment.NewLine +
                "+" + Stamina.ToString() + " к выносливости" + System.Environment.NewLine +
                "+" + Intellect.ToString() + " к интелекту";
            textBox3.Text = BonusInfo;
            textBox2.Text = ClassInfo;
            textBox1.Text = RaceInfo;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateInfo();
            }
            catch{}
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateInfo();
            }
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (maskedTextBox1.Text == "")
            {
                MessageBox.Show("Не допускаются пустые имена. Первый символ должен быть буквой");
                return;
            }
            try
            {
                OleDbConnection Connection = new OleDbConnection(Login.Path);
                Connection.Open();
                var cmd = Connection.CreateCommand();
                cmd.CommandText = "INSERT INTO Avatar" +
                "(CharacterID, AvatarName, Gender,AvatarRace,AvatarClass,CharacterLocated,AvatarModelPath)" +
                "VALUES(?,?,?,?,?,?,?)";
                cmd.Parameters.Add("CharacterID", OleDbType.Integer).Value = Login.PersonID;
                cmd.Parameters.Add("AvatarName", OleDbType.Char, 255).Value = maskedTextBox1.Text.Trim();
                if (radioButton1.Checked)
                    cmd.Parameters.Add("Gender", OleDbType.Char, 255).Value = "Мужской";
                else
                    cmd.Parameters.Add("Gender", OleDbType.Char, 255).Value = "Женский";
                cmd.Parameters.Add("AvatarRace", OleDbType.Integer).Value = arrayRaceID[comboBox1.SelectedIndex];
                cmd.Parameters.Add("AvatarClass", OleDbType.Integer).Value = arrayClassID[comboBox2.SelectedIndex];
                cmd.Parameters.Add("CharacterLocated", OleDbType.Integer).Value = 2;
                cmd.Parameters.Add("AvatarModelPath", OleDbType.Char, 255).Value = imagePath;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                Connection.Close();
            }
            catch 
            { 
                MessageBox.Show("Извените, но персонаж с данным именем уже существует." +
                    System.Environment.NewLine + "Попробуйте придумать другое имя.");
                return;
            }

            InsertAvatarStats();
            
            this.Close();
        }
        private void InsertAvatarStats()
        {
            OleDbConnection Connection = new OleDbConnection(Login.Path);
            OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM Avatar", Connection);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "Avatar");
            var LogIn =
                from account in dataSet.Tables[dataSet.Tables.IndexOf("Avatar")].AsEnumerable()
                where account.Field<int>("CharacterID") == Login.PersonID
                select account;
            var idCollection2 = LogIn.LastOrDefault(b=>b.Field<int>("AvatarID")>0);

            Connection.Open();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "INSERT INTO AvatarStats" +
            "(AvatarID, AvatarLVL,AvatarMaxMana,AvatarMaxHealth,AvatarMaxStamina,AvatarStrenght," +
            "AvatarAgillity, AvatarIntellect, AvatarDefens,PointsToStats,PointsToSpells)"+
            "VALUES(?,?,?,?,?,?,?,?,?,?,?)";
            cmd.Parameters.Add("AvatarID", OleDbType.Integer).Value = idCollection2.Field<int>("AvatarID");
            cmd.Parameters.Add("AvatarLVL", OleDbType.Integer).Value = 1;
            cmd.Parameters.Add("AvatarMaxMana", OleDbType.Integer).Value = Mana+100;
            cmd.Parameters.Add("AvatarMaxHealth", OleDbType.Integer).Value = Health + 100;
            cmd.Parameters.Add("AvatarMaxStamina", OleDbType.Integer).Value = Stamina + 100;
            cmd.Parameters.Add("AvatarStrenght", OleDbType.Integer).Value = Strenght;
            cmd.Parameters.Add("AvatarAgillity", OleDbType.Integer).Value = Agility;
            cmd.Parameters.Add("AvatarIntellect", OleDbType.Integer).Value = Intellect;
            cmd.Parameters.Add("AvatarDefens", OleDbType.Integer).Value = Defens;
            cmd.Parameters.Add("PointsToStats", OleDbType.Integer).Value = 10;
            cmd.Parameters.Add("PointsToSpells", OleDbType.Integer).Value = 10;

            cmd.ExecuteNonQuery();
            cmd.Dispose();
            Connection.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
