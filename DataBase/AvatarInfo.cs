using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace DataBase
{
    public partial class AvatarInfo : Form
    {
        public AvatarInfo()
        {
            InitializeComponent();
            tabControl1.SelectedIndex = 1;
        }

        private void AvatarInfo_Load(object sender, EventArgs e) { tabControl1.SelectedIndex = 0; }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0: LoadEquipment();break;
                case 1: Characteristic(); break;
                case 2: Spells(); break;
                case 3: Characteristic(); Inventar(); break;
                case 4: PVP(); break;
            }
        }

        private void DefaultLoad()
        {
            OleDbConnection Connection = new OleDbConnection(Login.Path);
            Connection.Open();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT AvatarBag.id, AvatarBag.Equipment, Objects.ObjectType, Objects.ObjectModelPath " +
            "FROM AvatarBag INNER JOIN Objects ON AvatarBag.ObjectID = Objects.ObjectID " +
            "WHERE AvatarID = @AvatarID AND Equipment = @Equipment";
            cmd.Parameters.Add("@AvatarID", OleDbType.Integer).Value = 9;// AvatarID = 9 -- для получения стандартного изображения
            cmd.Parameters.Add("@Equipment", OleDbType.Boolean).Value = true;
            DataTable dataTable = new DataTable();
            var objDataAdapter = new OleDbDataAdapter(cmd);
            objDataAdapter.Fill(dataTable);

            var idCollection2 = dataTable.AsEnumerable().Where(b => b.Field<int>("id") > 0);
            foreach (var row in idCollection2)
            {
                if (row.Field<Boolean>("Equipment"))
                {
                    if (row.Field<String>("ObjectType") == "голова")
                        pictureBox2.Load(row.Field<String>("ObjectModelPath"));
                    if (row.Field<String>("ObjectType") == "наплечники")
                        pictureBox3.Load(row.Field<String>("ObjectModelPath"));
                    if (row.Field<String>("ObjectType") == "накидка")
                        pictureBox4.Load(row.Field<String>("ObjectModelPath"));
                    if (row.Field<String>("ObjectType") == "доспех")
                        pictureBox5.Load(row.Field<String>("ObjectModelPath"));
                    if (row.Field<String>("ObjectType") == "перчатки")
                        pictureBox6.Load(row.Field<String>("ObjectModelPath"));
                    if (row.Field<String>("ObjectType") == "штаны")
                        pictureBox7.Load(row.Field<String>("ObjectModelPath"));
                    if (row.Field<String>("ObjectType") == "сапоги")
                        pictureBox8.Load(row.Field<String>("ObjectModelPath"));
                    if (row.Field<String>("ObjectType") == "ожерелье")
                        pictureBox9.Load(row.Field<String>("ObjectModelPath"));
                    if (row.Field<String>("ObjectType") == "кольцо")
                    {
                        pictureBox10.Load(row.Field<String>("ObjectModelPath"));
                        pictureBox11.Load(row.Field<String>("ObjectModelPath"));
                    }
                    if (row.Field<String>("ObjectType") == "оружие")
                        pictureBox12.Load(row.Field<String>("ObjectModelPath"));
                    if (row.Field<String>("ObjectType") == "оружие" ||
                        row.Field<String>("ObjectType") == "щит")
                        pictureBox13.Load(row.Field<String>("ObjectModelPath"));
                    if (row.Field<String>("ObjectType") == "оружие дальнего боя")
                        pictureBox14.Load(row.Field<String>("ObjectModelPath"));
                }
            }
        }
        private void LoadEquipment()
        {
            pictureBox1.Load(Account.path);
            DefaultLoad();
            OleDbConnection Connection = new OleDbConnection(Login.Path);
            Connection.Open();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT AvatarBag.id, AvatarBag.Equipment,"+
                " Objects.*,ObjectDescriptions.*" +
            "FROM (AvatarBag INNER JOIN (Objects " +
            "INNER JOIN ObjectDescriptions ON Objects.ObjectID = ObjectDescriptions.ObjectID) " +
            "ON AvatarBag.ObjectID = Objects.ObjectID) " +// INNER JOIN Classes " +
            //"ON Classes.ClassName = ObjectDescriptions.Class " +
            "WHERE AvatarID = @AvatarID AND Equipment = @Equipment";
            cmd.Parameters.Add("@AvatarID", OleDbType.Integer).Value = Account.AvatarID;
            cmd.Parameters.Add("@Equipment", OleDbType.Boolean).Value = true;
            DataTable dataTable = new DataTable();
            var objDataAdapter = new OleDbDataAdapter(cmd);
            objDataAdapter.Fill(dataTable);

            var idCollection2 = dataTable.AsEnumerable().Where(b => b.Field<int>("id") > 0);
            bool IsTwoRings = false;
            bool IsRightHands = false;
            
            foreach (var row in idCollection2)
            {
                if (row.Field<Boolean>("Equipment"))
                {
                    if (row.Field<String>("ObjectType") == "голова")
                    {
                        pictureBox2.Load(row.Field<String>("ObjectModelPath"));
                        String Description = GetDescriptionString(row);                        
                        toolTip1.SetToolTip(pictureBox2, Description);
                        continue;
                    }
                    if (row.Field<String>("ObjectType") == "наплечники")
                    {
                        pictureBox3.Load(row.Field<String>("ObjectModelPath"));
                        String Description = GetDescriptionString(row);
                        toolTip1.SetToolTip(pictureBox3, Description);
                        continue;
                    }
                    if (row.Field<String>("ObjectType") == "накидка")
                    {
                        pictureBox4.Load(row.Field<String>("ObjectModelPath"));
                        String Description = GetDescriptionString(row);
                        toolTip1.SetToolTip(pictureBox4, Description);
                        continue;
                    }
                    if (row.Field<String>("ObjectType") == "доспех")
                    {
                        pictureBox5.Load(row.Field<String>("ObjectModelPath"));
                        String Description = GetDescriptionString(row);
                        toolTip1.SetToolTip(pictureBox5, Description);
                        continue;
                    }
                    if (row.Field<String>("ObjectType") == "перчатки")
                    {
                        pictureBox6.Load(row.Field<String>("ObjectModelPath"));
                        String Description = GetDescriptionString(row);
                        toolTip1.SetToolTip(pictureBox6, Description);
                        continue;
                    }
                    if (row.Field<String>("ObjectType") == "штаны")
                    {
                        pictureBox7.Load(row.Field<String>("ObjectModelPath"));
                        String Description = GetDescriptionString(row);
                        toolTip1.SetToolTip(pictureBox7, Description);
                        continue;
                    }
                    if (row.Field<String>("ObjectType") == "сапоги")
                    {
                        pictureBox8.Load(row.Field<String>("ObjectModelPath"));
                        String Description = GetDescriptionString(row);
                        toolTip1.SetToolTip(pictureBox8, Description);
                        continue;
                    }
                    if (row.Field<String>("ObjectType") == "ожерелье")
                    {
                        pictureBox9.Load(row.Field<String>("ObjectModelPath"));
                        String Description = GetDescriptionString(row);
                        toolTip1.SetToolTip(pictureBox9, Description);
                        continue;
                    }
                    if (row.Field<String>("ObjectType") == "кольцо")
                    {
                        if (!IsTwoRings)
                        {
                            pictureBox10.Load(row.Field<String>("ObjectModelPath"));
                            String Description = GetDescriptionString(row);
                            toolTip1.SetToolTip(pictureBox10, Description);
                            IsTwoRings = true;
                            continue;
                        }
                        else
                        {
                            pictureBox11.Load(row.Field<String>("ObjectModelPath"));
                            String Description = GetDescriptionString(row);
                            toolTip1.SetToolTip(pictureBox11, Description);
                            continue;
                        }
                    }
                    if (row.Field<String>("ObjectType") == "оружие дальнего боя")
                    {
                        pictureBox14.Load(row.Field<String>("ObjectModelPath"));
                        String Description = GetDescriptionString(row);
                        toolTip1.SetToolTip(pictureBox14, Description);
                        continue;
                    }
                    /// TODO: if add new equipment slot, then get 
                    /// then get the image above this condition 
                    if (row.Field<String>("ObjectType") == "оружие")
                    {
                        pictureBox12.Load(row.Field<String>("ObjectModelPath"));
                        String Description = GetDescriptionString(row);
                        toolTip1.SetToolTip(pictureBox12, Description);
                        IsRightHands = true;
                        continue;
                    }
                    if (row.Field<String>("ObjectType") == "щит" ||
                        (row.Field<String>("ObjectType") == "оружие") && IsRightHands)
                    {
                        pictureBox13.Load(row.Field<String>("ObjectModelPath"));
                        String Description = GetDescriptionString(row);
                        toolTip1.SetToolTip(pictureBox13, Description);
                        IsRightHands = false;
                        continue;
                    }
                }
            }            
        }
        private String GetDescriptionString(DataRow row)
        {
            String Description;
            Description = row.Field<String>("ObjectName") + System.Environment.NewLine;
            Description += row.Field<String>("Class") != null ?
                "только для класса: " + row.Field<String>("Class") + System.Environment.NewLine : "";
            Description += row.Field<int>("minLVL") > 1 ?
                "minLVL = " + row.Field<int>("minLVL") + System.Environment.NewLine : "";
            Description += row.Field<int>("Attack") > 0 ?
                "Урон = " + row.Field<int>("Attack") + System.Environment.NewLine : "";
            Description += row.Field<int>("Range") > 1 ?
                "Дальность действия = " + row.Field<int>("Range") + System.Environment.NewLine : "";
            Description += row.Field<int>("ExpendedMana") > 0 ?
                "Необходимо " + row.Field<int>("ExpendedMana") + " MP" + System.Environment.NewLine : "";
            Description += row.Field<int>("ExpendedStamina") > 0 ?
                "Необходимо " + row.Field<int>("ExpendedStamina") + " SP" + System.Environment.NewLine : "";
            Description += row.Field<int>("BonusMaxHealth") > 0 ?
                "+ " + row.Field<int>("BonusMaxHealth") + " HP" + System.Environment.NewLine : "";
            Description += row.Field<int>("BonusMaxMana") > 0 ?
                "+ " + row.Field<int>("BonusMaxMana") + " MP" + System.Environment.NewLine : "";
            Description += row.Field<int>("BonusMaxStamina") > 0 ?
                "+ " + row.Field<int>("BonusMaxStamina") + " SP" + System.Environment.NewLine : "";
            Description += row.Field<int>("BonusHealth") > 0 ?
                "Востанавливает " + row.Field<int>("BonusHealth") + " HP" + System.Environment.NewLine : "";
            Description += row.Field<int>("BonusMana") > 0 ?
                "Востанавливает " + row.Field<int>("BonusMana") + " MP" + System.Environment.NewLine : "";
            Description += row.Field<int>("BonusStamina") > 0 ?
               "Востанавливает " + row.Field<int>("BonusStamina") + " SP" + System.Environment.NewLine : "";
            Description += row.Field<int>("BonusStrenght") > 0 ?
                "+ " + row.Field<int>("BonusStrenght") + " Сила" + System.Environment.NewLine : "";
            Description += row.Field<int>("BonusAgillity") > 0 ?
                "+ " + row.Field<int>("BonusAgillity") + " Скорость" + System.Environment.NewLine : "";
            Description += row.Field<int>("BonusIntellect") > 0 ?
                "+ " + row.Field<int>("BonusIntellect") + " Интелект" + System.Environment.NewLine : "";
            Description += row.Field<int>("BonusDefens") > 0 ?
                "+ " + row.Field<int>("BonusDefens") + " Защита" + System.Environment.NewLine : "";
            Description += row.Field<int>("BonusAttack") > 0 ?
                "+ " + row.Field<int>("BonusAttack") + " Атака" + System.Environment.NewLine : "";
            Description += row.Field<String>("DescriptionText") + System.Environment.NewLine;
            return Description;
        }

        private void Characteristic()
        {
            timer1.Start();
            String Description;
            OleDbConnection Connection = new OleDbConnection(Login.Path);
            Connection.Open();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT Avatar.AvatarName, Avatar.Gender, Races.RaceName, " +
                    "Classes.ClassName,Avatar.AvatarModelPath, AvatarStats.*" +
                    "FROM (((AvatarStats INNER JOIN Avatar ON Avatar.AvatarID = AvatarStats.AvatarID)" +
                    "INNER JOIN Races ON Avatar.AvatarRace = Races.RaceID) " +
                    "INNER JOIN Classes ON Avatar.AvatarClass = Classes.ClassID) " +
                    "WHERE AvatarStats.AvatarID = @AvatarID";
            cmd.Parameters.Add("@AvatarID", OleDbType.Integer).Value = Account.AvatarID;
            DataTable dataTable = new DataTable();
            var objDataAdapter = new OleDbDataAdapter(cmd);
            objDataAdapter.Fill(dataTable);            

            var idCollection2 = dataTable.AsEnumerable().FirstOrDefault(b => b.Field<int>("AvatarID") > 0);

            pictureBox15.Load(idCollection2.Field<String>("AvatarModelPath").Replace("image","icon"));
            AvatarLVLLable.Text = idCollection2.Field<int>("AvatarLVL").ToString();
            AvatarNameLabel.Text = idCollection2.Field<String>("AvatarName");
            ClassLabel.Text = idCollection2.Field<String>("ClassName");
            Racelabel.Text = idCollection2.Field<String>("RaceName");
            EXPlabel.Text = idCollection2.Field<int>("AvatarEXP").ToString();
            EXPprogress.Maximum = idCollection2.Field<int>("AvatarLVL") * 3567;
            EXPprogress.Value = idCollection2.Field<int>("AvatarEXP");            
            EXPlabel.Text = idCollection2.Field<int>("AvatarEXP").ToString() + "/" + EXPprogress.Maximum.ToString();

            HPprogress.Maximum = idCollection2.Field<int>("AvatarMaxHealth");
            HPprogress.Value = idCollection2.Field<int>("AvatarHealth");
            Description = HPprogress.Value.ToString() + "/" + HPprogress.Maximum.ToString();
            toolTip1.SetToolTip(HPprogress, Description);

            MPprogress.Maximum = idCollection2.Field<int>("AvatarMaxMana");
            MPprogress.Value = idCollection2.Field<int>("AvatarMana");
            Description = MPprogress.Value.ToString() + "/" + MPprogress.Maximum.ToString();
            toolTip1.SetToolTip(MPprogress, Description);

            SPprogress.Maximum = idCollection2.Field<int>("AvatarMaxStamina");
            SPprogress.Value = idCollection2.Field<int>("AvatarStamina");
            Description = SPprogress.Value.ToString() + "/" + SPprogress.Maximum.ToString();
            toolTip1.SetToolTip(SPprogress, Description);
            labelNumPoints.Text = idCollection2.Field<int>("PointsToStats").ToString();
            if (idCollection2.Field<int>("PointsToStats") > 0)
            {
                PlusHP.Visible = true;
                PlusMP.Visible = true;
                PlusSP.Visible = true;
                PlusAgility.Visible = true;
                PlusAttack.Visible = true;
                PlusDefens.Visible = true;
                PlusIntellect.Visible = true;
                PlusStrenght.Visible = true;
                labelStenght.Text = idCollection2.Field<int>("AvatarStrenght").ToString();
                labelAgility.Text = idCollection2.Field<int>("AvatarAgillity").ToString();
                labelIntellect.Text = idCollection2.Field<int>("AvatarIntellect").ToString();
                labelAttack.Text = idCollection2.Field<int>("AvatarAttack").ToString();
                labelDefens.Text = idCollection2.Field<int>("AvatarDefens").ToString();
            }
            
        }
        
        private void PlusNotVisible()
        {
            PlusHP.Visible = false;
            PlusMP.Visible = false;
            PlusSP.Visible = false;
            PlusAgility.Visible = false;
            PlusAttack.Visible = false;
            PlusDefens.Visible = false;
            PlusIntellect.Visible = false;
            PlusStrenght.Visible = false;
        }

        private void UpdateMPSPHP(object sender, EventArgs e)
        {
            if ((HPprogress.Value == HPprogress.Maximum) &&
                (MPprogress.Value == MPprogress.Maximum) &&
                (SPprogress.Value == SPprogress.Maximum))
            {
                timer1.Stop();
                return;
            }
            OleDbConnection Connection = new OleDbConnection(Login.Path);
            Connection.Open();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "UPDATE AvatarStats SET ";
            bool flag = false;
            if (HPprogress.Value < HPprogress.Maximum)
            {
                cmd.CommandText += "AvatarHealth = AvatarHealth+1 ";
                flag = true;
            }
            if (MPprogress.Value < MPprogress.Maximum)
            {
                if (flag) cmd.CommandText += ",";
                else flag = true;
                cmd.CommandText += "AvatarMana = AvatarMana+1 ";
            }
            if (SPprogress.Value < SPprogress.Maximum)
            {
                if (flag) cmd.CommandText += ",";
                cmd.CommandText += "AvatarStamina = AvatarStamina+1 ";
            }
            cmd.CommandText += "WHERE AvatarID = @AvatarID";
            cmd.Parameters.Add("@AvatarID", OleDbType.Integer, 255).Value = Account.AvatarID;
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            Connection.Close();
            if (HPprogress.Value < HPprogress.Maximum) ++HPprogress.Value;
            if (MPprogress.Value < MPprogress.Maximum) ++MPprogress.Value;
            if (SPprogress.Value < SPprogress.Maximum) ++SPprogress.Value;
            String Description = HPprogress.Value.ToString() + "/" + HPprogress.Maximum.ToString();
            toolTip1.SetToolTip(HPprogress, Description);
            Description = MPprogress.Value.ToString() + "/" + MPprogress.Maximum.ToString();
            toolTip1.SetToolTip(MPprogress, Description);
            Description = SPprogress.Value.ToString() + "/" + SPprogress.Maximum.ToString();
            toolTip1.SetToolTip(SPprogress, Description);
        }

        private void PlusHP_Click(object sender, EventArgs e)
        {
            OleDbConnection Connection = new OleDbConnection(Login.Path);
            Connection.Open();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "UPDATE AvatarStats SET AvatarMaxHealth = AvatarMaxHealth+1, " +
                "PointsToStats = PointsToStats-1 " +
                "WHERE AvatarID = @AvatarID";
            cmd.Parameters.Add("@AvatarID", OleDbType.Integer, 255).Value = Account.AvatarID;
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            Connection.Close();
            ++HPprogress.Maximum;
            String Description = HPprogress.Value.ToString() + "/" + HPprogress.Maximum.ToString();
            toolTip1.SetToolTip(HPprogress, Description);
            int points = Convert.ToInt32(labelNumPoints.Text) - 1;
            labelNumPoints.Text = points.ToString();
            if (points == 0) PlusNotVisible();
            timer1.Start();
        }

        private void PlusMP_Click(object sender, EventArgs e)
        {
            OleDbConnection Connection = new OleDbConnection(Login.Path);
            Connection.Open();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "UPDATE AvatarStats SET AvatarMaxMana = AvatarMaxMana+1, " +
                "PointsToStats = PointsToStats-1 " +
                "WHERE AvatarID = @AvatarID";
            cmd.Parameters.Add("@AvatarID", OleDbType.Integer, 255).Value = Account.AvatarID;
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            Connection.Close();
            ++MPprogress.Maximum;
            String Description = MPprogress.Value.ToString() + "/" + MPprogress.Maximum.ToString();
            toolTip1.SetToolTip(MPprogress, Description);
            int points = Convert.ToInt32(labelNumPoints.Text) - 1;
            labelNumPoints.Text = points.ToString();
            if (points == 0) PlusNotVisible();
            timer1.Start();
        }

        private void PlusSP_Click(object sender, EventArgs e)
        {
            OleDbConnection Connection = new OleDbConnection(Login.Path);
            Connection.Open();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "UPDATE AvatarStats SET AvatarMaxStamina = AvatarMaxStamina+1, " +
                "PointsToStats = PointsToStats-1 " +
                "WHERE AvatarID = @AvatarID";
            cmd.Parameters.Add("@AvatarID", OleDbType.Integer, 255).Value = Account.AvatarID;
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            Connection.Close();
            ++SPprogress.Maximum;
            String Description = SPprogress.Value.ToString() + "/" + SPprogress.Maximum.ToString();
            toolTip1.SetToolTip(SPprogress, Description);
            int points = Convert.ToInt32(labelNumPoints.Text) - 1;
            labelNumPoints.Text = points.ToString();
            if (points == 0) PlusNotVisible();
            timer1.Start();
        }

        private void PlusStrenght_Click(object sender, EventArgs e)
        {
            OleDbConnection Connection = new OleDbConnection(Login.Path);
            Connection.Open();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "UPDATE AvatarStats SET AvatarStrenght = AvatarStrenght+1, " +
                "PointsToStats = PointsToStats-1 " +
                "WHERE AvatarID = @AvatarID";
            cmd.Parameters.Add("@AvatarID", OleDbType.Integer, 255).Value = Account.AvatarID;
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            Connection.Close();
            int points = Convert.ToInt32(labelNumPoints.Text) - 1;
            labelNumPoints.Text = points.ToString();
            if (points == 0) PlusNotVisible();
            labelStenght.Text = (Convert.ToInt32(labelStenght.Text)+1).ToString();
        }

        private void PlusAgility_Click(object sender, EventArgs e)
        {
            OleDbConnection Connection = new OleDbConnection(Login.Path);
            Connection.Open();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "UPDATE AvatarStats SET AvatarAgillity = AvatarAgillity+1, " +
                "PointsToStats = PointsToStats-1 " +
                "WHERE AvatarID = @AvatarID";
            cmd.Parameters.Add("@AvatarID", OleDbType.Integer, 255).Value = Account.AvatarID;
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            Connection.Close();
            int points = Convert.ToInt32(labelNumPoints.Text) - 1;
            labelNumPoints.Text = points.ToString();
            if (points == 0) PlusNotVisible();
            labelAgility.Text = (Convert.ToInt32(labelAgility.Text) + 1).ToString();
        }

        private void PlusDefens_Click(object sender, EventArgs e)
        {
            OleDbConnection Connection = new OleDbConnection(Login.Path);
            Connection.Open();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "UPDATE AvatarStats SET AvatarDefens = AvatarDefens+1, " +
                "PointsToStats = PointsToStats-1 " +
                "WHERE AvatarID = @AvatarID";
            cmd.Parameters.Add("@AvatarID", OleDbType.Integer, 255).Value = Account.AvatarID;
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            Connection.Close();
            int points = Convert.ToInt32(labelNumPoints.Text) - 1;
            labelNumPoints.Text = points.ToString();
            if (points == 0) PlusNotVisible();
            labelDefens.Text = (Convert.ToInt32(labelDefens.Text) + 1).ToString();
        }

        private void PlusIntellect_Click(object sender, EventArgs e)
        {
            OleDbConnection Connection = new OleDbConnection(Login.Path);
            Connection.Open();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "UPDATE AvatarStats SET AvatarIntellect = AvatarIntellect+1, " +
                "PointsToStats = PointsToStats-1 " +
                "WHERE AvatarID = @AvatarID";
            cmd.Parameters.Add("@AvatarID", OleDbType.Integer, 255).Value = Account.AvatarID;
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            Connection.Close();
            int points = Convert.ToInt32(labelNumPoints.Text) - 1;
            labelNumPoints.Text = points.ToString();
            if (points == 0) PlusNotVisible();
            labelIntellect.Text = (Convert.ToInt32(labelIntellect.Text) + 1).ToString();
        }

        private void PlusAttack_Click(object sender, EventArgs e)
        {
            OleDbConnection Connection = new OleDbConnection(Login.Path);
            Connection.Open();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "UPDATE AvatarStats SET AvatarAttack = AvatarAttack+1, " +
                "PointsToStats = PointsToStats-1 " +
                "WHERE AvatarID = @AvatarID";
            cmd.Parameters.Add("@AvatarID", OleDbType.Integer, 255).Value = Account.AvatarID;
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            Connection.Close();
            int points = Convert.ToInt32(labelNumPoints.Text) - 1;
            labelNumPoints.Text = points.ToString();
            if (points == 0) PlusNotVisible();
            labelAttack.Text = (Convert.ToInt32(labelAttack.Text) + 1).ToString();
        }
        private void Spells()
        {
            OleDbConnection Connection = new OleDbConnection(Login.Path);
            Connection.Open();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT Spells.SpellName, Spells.HowUse,"+
                " Spells.RecoveryTime, Spells.Time,Spells.Distance, " +
                "Spells.Health,Spells.Mana,Spells.Stamina,Spells.Description " +
            "FROM AvatarSpell INNER JOIN Spells " +
            "ON Spells.IDSpell = AvatarSpell.IDSpell " +
            "WHERE AvatarID = @AvatarID";
            cmd.Parameters.Add("@AvatarID", OleDbType.Integer).Value = Account.AvatarID;
            DataTable dataTable = new DataTable();
            var objDataAdapter = new OleDbDataAdapter(cmd);
            objDataAdapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            NewSpell spell = new NewSpell();
            spell.ShowDialog();
            Spells();
        }

        DataSet ds1 = new DataSet();
        int LastCurrentRowIndataGridView2 = 0;  
        private void Inventar()
        {
            LastCurrentRowIndataGridView2 = 0;  
            OleDbConnection Connection = new OleDbConnection(Login.Path);
            Connection.Open();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT Objects.ObjectID, Objects.ObjectName, Objects.ObjectType, Objects.Cost, " +
                "AvatarBag.id,AvatarBag.Amount, AvatarBag.Equipment " +
            "FROM AvatarBag INNER JOIN Objects " +
            "ON Objects.ObjectID = AvatarBag.ObjectID " +
            "WHERE AvatarID = @AvatarID";
            cmd.Parameters.Add("@AvatarID", OleDbType.Integer).Value = Account.AvatarID;
            DataTable dataTable = new DataTable();
            var objDataAdapter = new OleDbDataAdapter(cmd);
            //objDataAdapter.Fill(dataTable);
            objDataAdapter.Fill(ds1);
            dataGridView2.DataSource = ds1.Tables[0];

            //dataGridView2.DataSource = dataTable;
            dataGridView2.Columns["ObjectID"].Visible = false;
            dataGridView2.Columns["id"].Visible = false;
            dataGridView2.Select();
            cmd.Dispose();
            Connection.Close();
        }
        private String GetObjectInfo(DataRow row)
        {
            String Description;
            Description = row.Field<String>("Class") != null ?
                "только для класса: " + row.Field<String>("Class") + System.Environment.NewLine : "";
            Description += row.Field<int>("minLVL") > 1 ?
                "minLVL = " + row.Field<int>("minLVL") + System.Environment.NewLine : "";
            Description += row.Field<int>("Attack") > 0 ?
                "Урон = " + row.Field<int>("Attack") + System.Environment.NewLine : "";
            Description += row.Field<int>("Range") > 1 ?
                "Дальность действия = " + row.Field<int>("Range") + System.Environment.NewLine : "";
            Description += row.Field<int>("ExpendedMana") > 0 ?
                "Необходимо " + row.Field<int>("ExpendedMana") + " MP" + System.Environment.NewLine : "";
            Description += row.Field<int>("ExpendedStamina") > 0 ?
                "Необходимо " + row.Field<int>("ExpendedStamina") + " SP" + System.Environment.NewLine : "";
            Description += row.Field<int>("BonusMaxHealth") > 0 ?
                "+ " + row.Field<int>("BonusMaxHealth") + " HP" + System.Environment.NewLine : "";
            Description += row.Field<int>("BonusMaxMana") > 0 ?
                "+ " + row.Field<int>("BonusMaxMana") + " MP" + System.Environment.NewLine : "";
            Description += row.Field<int>("BonusMaxStamina") > 0 ?
                "+ " + row.Field<int>("BonusMaxStamina") + " SP" + System.Environment.NewLine : "";
            Description += row.Field<int>("BonusHealth") > 0 ?
                "Востанавливает " + row.Field<int>("BonusHealth") + " HP" + System.Environment.NewLine : "";
            Description += row.Field<int>("BonusMana") > 0 ?
                "Востанавливает " + row.Field<int>("BonusMana") + " MP" + System.Environment.NewLine : "";
            Description += row.Field<int>("BonusStamina") > 0 ?
               "Востанавливает " + row.Field<int>("BonusStamina") + " SP" + System.Environment.NewLine : "";
            Description += row.Field<int>("BonusStrenght") > 0 ?
                "+ " + row.Field<int>("BonusStrenght") + " Сила" + System.Environment.NewLine : "";
            Description += row.Field<int>("BonusAgillity") > 0 ?
                "+ " + row.Field<int>("BonusAgillity") + " Скорость" + System.Environment.NewLine : "";
            Description += row.Field<int>("BonusIntellect") > 0 ?
                "+ " + row.Field<int>("BonusIntellect") + " Интелект" + System.Environment.NewLine : "";
            Description += row.Field<int>("BonusDefens") > 0 ?
                "+ " + row.Field<int>("BonusDefens") + " Защита" + System.Environment.NewLine : "";
            Description += row.Field<int>("BonusAttack") > 0 ?
                "+ " + row.Field<int>("BonusAttack") + " Атака" + System.Environment.NewLine : "";
            Description += row.Field<String>("DescriptionText") + System.Environment.NewLine;
            return Description;
        }        
        private void button3_Click(object sender, EventArgs e)
        {
            OleDbConnection Connection = new OleDbConnection(Login.Path);
            Connection.Open();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT ObjectDescriptions.* " +
           "FROM Objects INNER JOIN ObjectDescriptions " +
           "ON ObjectDescriptions.DescriptionID = Objects.DescriptionID " +
           "WHERE Objects.ObjectID = @ObjectID";
            cmd.Parameters.Add("@ObjectID", OleDbType.Integer).Value =
                dataGridView2["ObjectID", dataGridView2.CurrentRow.Index].Value.ToString();
            DataTable dataTable = new DataTable();
            var objDataAdapter = new OleDbDataAdapter(cmd);
            objDataAdapter.Fill(dataTable);
            cmd.Dispose();
            Connection.Close();
            var row = dataTable.AsEnumerable().FirstOrDefault(b => b.Field<int>("DescriptionID") > 0);
            String Description;
            Description = GetObjectInfo(row);
            MessageBox.Show(Description);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            String type = dataGridView2["ObjectType", dataGridView2.CurrentRow.Index].Value.ToString();
            int amount = (int)dataGridView2["Amount", dataGridView2.CurrentRow.Index].Value;
            bool Equipmentflag = (bool)dataGridView2["Equipment", dataGridView2.CurrentRow.Index].Value;
            if (type == "квестовое" || type == "хлам" || type == "ингредиент") 
            {
                MessageBox.Show("Невозможно использовать");
                return;
            }
            if (type == "пища" || type == "зелье")
            {
                OleDbConnection Connection = new OleDbConnection(Login.Path);

                Connection.Open();
                var cmd = Connection.CreateCommand();                
                cmd = Connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM ObjectDescriptions " +
                   "WHERE id = @id";
                cmd.Parameters.Add("@id", OleDbType.Integer).Value =
                        dataGridView2["id", dataGridView2.CurrentRow.Index].Value;
                DataTable dataTable = new DataTable();
                var objDataAdapter = new OleDbDataAdapter(cmd);
                objDataAdapter.Fill(dataTable);
                Connection.Close();
                Connection.Open();
                cmd.CommandText = "UPDATE AvatarStats SET ";
                bool flag = false;

                if (HPprogress.Value + (int)dataGridView2["BonusHealth", dataGridView2.CurrentRow.Index].Value
                    < HPprogress.Maximum)
                {
                    cmd.CommandText += "AvatarHealth = AvatarHealth+@Health ";
                    cmd.Parameters.Add("@Health", OleDbType.Integer).Value =
                        (int)dataGridView2["BonusHealth", dataGridView2.CurrentRow.Index].Value;
                    flag = true;
                }
                else
                {
                    cmd.CommandText += "AvatarHealth = @Health ";
                    cmd.Parameters.Add("@Health", OleDbType.Integer).Value = HPprogress.Maximum;
                    flag = true;
                }

                if (MPprogress.Value + (int)dataGridView2["BonusMana", dataGridView2.CurrentRow.Index].Value 
                    < MPprogress.Maximum)
                {
                    if (flag) cmd.CommandText += ",";
                    else flag = true;
                    cmd.CommandText += "AvatarMana = AvatarMana+@Mana";
                    cmd.Parameters.Add("@Mana", OleDbType.Integer).Value =
                        (int)dataGridView2["BonusMana", dataGridView2.CurrentRow.Index].Value;
                }
                else
                {
                    cmd.CommandText += "AvatarMana = @Mana ";
                    cmd.Parameters.Add("@Mana", OleDbType.Integer).Value = MPprogress.Maximum;
                    flag = true;
                }

                if (SPprogress.Value + (int)dataGridView2["BonusStamina", dataGridView2.CurrentRow.Index].Value
                    < SPprogress.Maximum)
                {
                    if (flag) cmd.CommandText += ",";
                    else flag = true;
                    cmd.CommandText += "AvatarStamina = AvatarStamina+@Stamina";
                    cmd.Parameters.Add("@Stamina", OleDbType.Integer).Value =
                        (int)dataGridView2["BonusStamina", dataGridView2.CurrentRow.Index].Value;
                }
                else
                {
                    cmd.CommandText += "AvatarStamina = @Stamina ";
                    cmd.Parameters.Add("@Stamina", OleDbType.Integer).Value = SPprogress.Maximum;
                    flag = true;
                }
                cmd.CommandText += "WHERE AvatarID = @AvatarID";
                cmd.Parameters.Add("@AvatarID", OleDbType.Integer, 255).Value = Account.AvatarID;
                cmd.ExecuteNonQuery();
                dataGridView2["Amount", dataGridView2.CurrentRow.Index].Value =
                    (int)dataGridView2["Amount", dataGridView2.CurrentRow.Index].Value - 1;
                if ((int)dataGridView2["Amount", dataGridView2.CurrentRow.Index].Value == 0)
                {
                    cmd.CommandText = "DELETE FROM AvatarBag WHERE id = @id";
                    cmd.Parameters.Add("@id", OleDbType.Integer, 255).Value =
                        (int)dataGridView2["id", dataGridView2.CurrentRow.Index].Value;
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd.CommandText = "UPDATE AvatarBag SET Amount = @Amount "+
                        "WHERE id = @id";
                    cmd.Parameters.Add("@Amount", OleDbType.Integer, 255).Value =
                        (int)dataGridView2["Amount", dataGridView2.CurrentRow.Index].Value;
                    cmd.Parameters.Add("@id", OleDbType.Integer, 255).Value =
                        (int)dataGridView2["id", dataGridView2.CurrentRow.Index].Value;
                    cmd.ExecuteNonQuery();
                }
                cmd.Dispose();
                Connection.Close();
            }
            else
            {
                if (Equipmentflag)
                {
                    dataGridView2["Equipment", dataGridView2.CurrentRow.Index].Value = false;
                    dataGridView2["Amount", dataGridView2.CurrentRow.Index].Value = amount + 1;
                    OleDbConnection Connection = new OleDbConnection(Login.Path);

                    Connection.Open();
                    var cmd = Connection.CreateCommand();
                    cmd.CommandText = "UPDATE AvatarBag SET Amount = @Amount, " +
                        "Equipment = @Equipment " +
                        "WHERE id = @id";
                    cmd.Parameters.Add("@Equipment", OleDbType.Integer, 255).Value = false;
                    cmd.Parameters.Add("@Amount", OleDbType.Integer, 255).Value =
                        (int)dataGridView2["Amount", dataGridView2.CurrentRow.Index].Value;
                    cmd.Parameters.Add("@id", OleDbType.Integer, 255).Value =
                        (int)dataGridView2["id", dataGridView2.CurrentRow.Index].Value;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    Connection.Close();

                    Connection.Open();
                    cmd = Connection.CreateCommand();
                    cmd.CommandText = "SELECT * FROM ObjectDescriptions " +
                       "WHERE id = @id";
                    cmd.Parameters.Add("@id", OleDbType.Integer).Value =
                            dataGridView2["id", dataGridView2.CurrentRow.Index].Value;
                    DataTable dataTable = new DataTable();
                    var objDataAdapter = new OleDbDataAdapter(cmd);
                    objDataAdapter.Fill(dataTable);
                    Connection.Close();
                    cmd.Dispose();
                    var row = dataTable.AsEnumerable().FirstOrDefault(b => b.Field<int>("DescriptionID") > 0);
                    
                    Connection.Open();
                    cmd = Connection.CreateCommand();
                    if (row != null)
                    {
                        cmd.CommandText = "UPDATE AvatarStats SET AvatarMaxMana = AvatarMaxMana-@AvatarMaxMana, " +
                            "AvatarMaxHealth = AvatarMaxHealth-@AvatarMaxHealth, " +
                            "AvatarMaxStamina = AvatarMaxStamina-@AvatarMaxStamina, " +
                            "AvatarStrenght = AvatarStrenght-@AvatarStrenght, " +
                            "AvatarAgillity = AvatarAgillity-@AvatarAgillity, " +
                            "AvatarIntellect = AvatarIntellect-@AvatarIntellect, " +
                            "AvatarAttack = AvatarAttack-@AvatarAttack, " +
                            "AvatarDefens = AvatarDefens-@AvatarDefens " +
                            "WHERE AvatarID = @AvatarID";
                        cmd.Parameters.Add("@AvatarID", OleDbType.Integer).Value = Account.AvatarID;
                        cmd.Parameters.Add("@AvatarMaxMana", OleDbType.Integer, 255).Value =
                            row.Field<int>("BonusMaxMana");
                        cmd.Parameters.Add("@AvatarMaxHealth", OleDbType.Integer, 255).Value =
                            row.Field<int>("BonusMaxHealth");
                        cmd.Parameters.Add("@AvatarMaxStamina", OleDbType.Integer, 255).Value =
                            row.Field<int>("BonusMaxStamina");
                        cmd.Parameters.Add("@AvatarStrenght", OleDbType.Integer, 255).Value =
                            row.Field<int>("BonusStrenght");
                        cmd.Parameters.Add("@AvatarAgillity", OleDbType.Integer, 255).Value =
                            row.Field<int>("BonusAgillity");
                        cmd.Parameters.Add("@AvatarIntellect", OleDbType.Integer, 255).Value =
                            row.Field<int>("BonusIntellect");
                        cmd.Parameters.Add("@AvatarAttack", OleDbType.Integer, 255).Value =
                            row.Field<int>("BonusAttack");
                        cmd.Parameters.Add("@AvatarDefens", OleDbType.Integer, 255).Value =
                            row.Field<int>("BonusDefens");
                        cmd.ExecuteNonQuery();
                    }
                    cmd.Dispose();
                    Connection.Close();
                }
                else
                {
                    dataGridView2["Equipment", dataGridView2.CurrentRow.Index].Value = true;
                    dataGridView2["Amount", dataGridView2.CurrentRow.Index].Value =
                        (int)dataGridView2["Amount", dataGridView2.CurrentRow.Index].Value - 1;
                    OleDbConnection Connection = new OleDbConnection(Login.Path);

                    Connection.Open();
                    var cmd = Connection.CreateCommand();
                    cmd.CommandText = "UPDATE AvatarBag SET Amount = @Amount, " +
                        "Equipment = @Equipment " +
                        "WHERE id = @id";
                    cmd.Parameters.Add("@Equipment", OleDbType.Integer, 255).Value = false;
                    cmd.Parameters.Add("@Amount", OleDbType.Integer, 255).Value =
                        (int)dataGridView2["Amount", dataGridView2.CurrentRow.Index].Value;
                    cmd.Parameters.Add("@id", OleDbType.Integer, 255).Value =
                        (int)dataGridView2["id", dataGridView2.CurrentRow.Index].Value;
                    cmd.ExecuteNonQuery();
                    Connection.Close();
                    cmd.Dispose();

                    Connection.Open();
                    cmd = Connection.CreateCommand();
                    cmd.CommandText = "SELECT ObjectDescriptions.* " +
                       "FROM Objects INNER JOIN ObjectDescriptions " +
                       "ON ObjectDescriptions.DescriptionID = Objects.DescriptionID " +
                       "WHERE Objects.ObjectID = @ObjectID";
                    cmd.Parameters.Add("@ObjectID", OleDbType.Integer).Value =
                            dataGridView2["ObjectID", dataGridView2.CurrentRow.Index].Value;
                    DataTable dataTable = new DataTable();
                    var objDataAdapter = new OleDbDataAdapter(cmd);
                    objDataAdapter.Fill(dataTable);
                    Connection.Close();
                    cmd.Dispose();
                    var row = dataTable.AsEnumerable().FirstOrDefault(b => b.Field<int>("DescriptionID") > 0);
                    
                    Connection.Open();
                    cmd = Connection.CreateCommand();
                    if (row != null)
                    {
                        cmd.CommandText = "UPDATE AvatarStats SET AvatarMaxMana = AvatarMaxMana+@AvatarMaxMana, " +
                            "AvatarMaxHealth = AvatarMaxHealth+@AvatarMaxHealth, " +
                            "AvatarMaxStamina = AvatarMaxStamina+@AvatarMaxStamina, " +
                            "AvatarStrenght = AvatarStrenght+@AvatarStrenght, " +
                            "AvatarAgillity = AvatarAgillity+@AvatarAgillity, " +
                            "AvatarIntellect = AvatarIntellect+@AvatarIntellect, " +
                            "AvatarAttack = AvatarAttack+@AvatarAttack, " +
                            "AvatarDefens = AvatarDefens+@AvatarDefens " +
                            "WHERE AvatarID = @AvatarID";
                        cmd.Parameters.Add("@AvatarID", OleDbType.Integer).Value = Account.AvatarID;
                        cmd.Parameters.Add("@AvatarMaxMana", OleDbType.Integer, 255).Value =
                            row.Field<int>("BonusMaxMana");
                        cmd.Parameters.Add("@AvatarMaxHealth", OleDbType.Integer, 255).Value =
                            row.Field<int>("BonusMaxHealth");
                        cmd.Parameters.Add("@AvatarMaxStamina", OleDbType.Integer, 255).Value =
                            row.Field<int>("BonusMaxStamina");
                        cmd.Parameters.Add("@AvatarStrenght", OleDbType.Integer, 255).Value =
                            row.Field<int>("BonusStrenght");
                        cmd.Parameters.Add("@AvatarAgillity", OleDbType.Integer, 255).Value =
                            row.Field<int>("BonusAgillity");
                        cmd.Parameters.Add("@AvatarIntellect", OleDbType.Integer, 255).Value =
                            row.Field<int>("BonusIntellect");
                        cmd.Parameters.Add("@AvatarAttack", OleDbType.Integer, 255).Value =
                            row.Field<int>("BonusAttack");
                        cmd.Parameters.Add("@AvatarDefens", OleDbType.Integer, 255).Value =
                            row.Field<int>("BonusDefens");
                        cmd.ExecuteNonQuery();
                    }
                    cmd.Dispose();
                    Connection.Close();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OleDbConnection Connection = new OleDbConnection(Login.Path);
            Connection.Open();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "UPDATE AvatarBag SET Amount = @Amount " +
                "WHERE id = @id";
            cmd.Parameters.Add("@Amount", OleDbType.Integer, 255).Value =
                (int)dataGridView2["Amount", dataGridView2.CurrentRow.Index].Value - 1;
            cmd.Parameters.Add("@id", OleDbType.Integer, 255).Value =
                (int)dataGridView2["id", dataGridView2.CurrentRow.Index].Value;
            cmd.ExecuteNonQuery();
            Connection.Close();
            cmd.Dispose();
            if (((int)dataGridView2["Amount", dataGridView2.CurrentRow.Index].Value - 1 == 0) &&
                    !(bool)dataGridView2["Amount", dataGridView2.CurrentRow.Index].Value)
            {
                Connection.Open();
                cmd.CommandText = "DELETE FROM AvatarBag WHERE id = @id";
                cmd.Parameters.Add("@id", OleDbType.Integer, 255).Value =
                    (int)dataGridView2["id", dataGridView2.CurrentRow.Index].Value;
                cmd.ExecuteNonQuery();
                Connection.Close();
            }
            dataGridView2["Amount", dataGridView2.CurrentRow.Index].Value =
                (int)dataGridView2["Amount", dataGridView2.CurrentRow.Index].Value - 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        DataSet ds2 = new DataSet();

        //dsSorted.Tables.Add(dvPrograms.ToTable());        
        private void PVP()
        {
            OleDbConnection Connection = new OleDbConnection(Login.Path);
            Connection.Open();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT Avatar.AvatarName, PVP.* " +
           "FROM PVP INNER JOIN Avatar " +
           "ON Avatar.AvatarID = PVP.AvatarID ";
            DataTable dataTable = new DataTable();
            var objDataAdapter = new OleDbDataAdapter(cmd);
            objDataAdapter.Fill(ds2);
            dataGridView3.DataSource = ds2.Tables[0];
            dataGridView3.Columns["AvatarID"].Visible = false;
        }
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {            
            try
            {
                CurrencyManager currencyManager = (CurrencyManager)BindingContext[
                    dataGridView3.DataSource, dataGridView3.DataMember]; //dgMain - это ГРИД
                if (textBox1.Text == string.Empty) //tbMask - это ТекстБокс
                {
                    currencyManager.Position = -1;
                    return;
                }
                DataView dataView = (DataView)currencyManager.List;
                string query = string.Format("{0} LIKE '{1}%'", 
                    Convert.ToString("AvatarName"), Convert.ToString(textBox1.Text)); // COL_NAME - имя колонки
                DataTable dataTable = dataView.Table;
                DataRow[] rows = dataTable.Select(query, dataView.Sort);
                if (rows.Length > 0)
                {
                    DataRow[] tempRows;
                    if (dataView.Sort == string.Empty)
                    {//Если сортировка не задана - просто находим заданную строку в массиве строк DataTable
                        tempRows = new DataRow[dataView.Count];
                        dataTable.Rows.CopyTo(tempRows, 0);
                    }
                    else
                        tempRows = dataTable.Select(dataView.RowFilter, dataView.Sort);
                    int rowIndex = Array.IndexOf(tempRows, rows[0]);
                    currencyManager.Position = rowIndex;
                }
            }
            catch{}
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                dataGridView3.DataSource = ds2.Tables[0];
            ((DataTable)dataGridView3.DataSource).DefaultView.RowFilter = "AvatarName LIKE '%"+textBox1.Text+"%'";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DataView dv;
            if(radioButton1.Checked)
                dv = new DataView(ds2.Tables[0], "BattlesNum > "+textBox2.Text,
                "BattlesNum Desc", DataViewRowState.CurrentRows);
            else
                dv = new DataView(ds2.Tables[0], "BattlesNum < " + textBox2.Text,
                "BattlesNum Desc", DataViewRowState.CurrentRows);
            dataGridView3.DataSource = dv;
        }
        
        private void button9_Click(object sender, EventArgs e)
        {
            int i = dataGridView2.CurrentRow.Index + 1;

            for (; i < dataGridView2.RowCount; i++)
            {
                dataGridView2[2, i].Style.BackColor = Color.White;
                if (dataGridView2[2, i].FormattedValue.ToString().Contains(textBox4.Text))
                {
                    //dataGridView2[2, i].Style.BackColor = Color.Red;
                    dataGridView2.CurrentCell = dataGridView2.Rows[i].Cells[2];
                    break;
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            int i = dataGridView2.RowCount-1;

            for (; i > 0; i--)
            {
                dataGridView2[2, i].Style.BackColor = Color.White;
                if (dataGridView2[2, i].FormattedValue.ToString().Contains(textBox4.Text))
                {
                    //dataGridView2[2, i].Style.BackColor = Color.Red;
                    dataGridView2.CurrentCell = dataGridView2.Rows[i].Cells[2];
                    //break;
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            int i = 0;

            for (; i < dataGridView2.RowCount; i++)
            {
                dataGridView2[2, i].Style.BackColor = Color.White;
                if (dataGridView2[2, i].FormattedValue.ToString().Contains(textBox4.Text))
                {
                    //dataGridView2[2, i].Style.BackColor = Color.Red;
                    dataGridView2.CurrentCell = dataGridView2.Rows[i].Cells[2];
                    //break;
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            int i = dataGridView2.CurrentRow.Index - 1;

            for (; i >=0; i--)
            {
                dataGridView2[2, i].Style.BackColor = Color.White;
                if (dataGridView2[2, i].FormattedValue.ToString().Contains(textBox4.Text))
                {
                    //dataGridView2[2, i].Style.BackColor = Color.Red;
                    dataGridView2.CurrentCell = dataGridView2.Rows[i].Cells[2];
                    break;
                }
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CurrencyManager currencyManager = (CurrencyManager)BindingContext[
                    dataGridView2.DataSource, dataGridView2.DataMember]; //dgMain - это ГРИД
                if (textBox3.Text == string.Empty) //tbMask - это ТекстБокс
                {
                    currencyManager.Position = -1;
                    return;
                }
                DataView dataView = (DataView)currencyManager.List;
                string query = string.Format("{0} LIKE '{1}%'",
                    Convert.ToString("ObjectName"), Convert.ToString(textBox3.Text)); // COL_NAME - имя колонки
                DataTable dataTable = dataView.Table;
                DataRow[] rows = dataTable.Select(query, dataView.Sort);
                if (rows.Length > 0)
                {
                    DataRow[] tempRows;
                    if (dataView.Sort == string.Empty)
                    {//Если сортировка не задана - просто находим заданную строку в массиве строк DataTable
                        tempRows = new DataRow[dataView.Count];
                        dataTable.Rows.CopyTo(tempRows, 0);
                    }
                    else
                        tempRows = dataTable.Select(dataView.RowFilter, dataView.Sort);
                    int rowIndex = Array.IndexOf(tempRows, rows[0]);
                    currencyManager.Position = rowIndex;
                }
            }
            catch { }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (textBox4.Text == "")
                dataGridView2.DataSource = ds1.Tables[0];
            ((DataTable)dataGridView2.DataSource).DefaultView.RowFilter = "ObjectType LIKE '%" + textBox4.Text + "%'";
        }

        //bool equipmentornot = false;
        private void button2_Click_1(object sender, EventArgs e)
        {
           // equipmentornot = !equipmentornot;
            //if (textBox4.Text == "")
            //    dataGridView2.DataSource = ds.Tables[0];
            //((DataTable)dataGridView2.DataSource).DefaultView.RowFilter = "Equipment LIKE '%" + equipmentornot.ToString() + "%'";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (textBox4.Text == "")
                dataGridView2.DataSource = ds1.Tables[0];
            ((DataTable)dataGridView2.DataSource).DefaultView.RowFilter = "ObjectName LIKE '%" + textBox3.Text + "%'";
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            //dsSorted
            chart1.Series.Clear();
            Series ser = new Series();

            OleDbConnection Connection = new OleDbConnection(Login.Path);
            Connection.Open();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT AvatarName FROM Avatar";
            DataTable dataTable = new DataTable();
            var objDataAdapter = new OleDbDataAdapter(cmd);
            objDataAdapter.Fill(dataTable);
            var idCollection2 = dataTable.AsEnumerable().Where(b => b.Field<String>("AvatarName") !="");

            Dictionary<String, int> IdRace = new Dictionary<String, int>();
            foreach (var row in idCollection2)
            {
                IdRace.Add(row.Field<String>("AvatarName"), 0);
            }

            DataTable dt = new DataTable();
            dt = ds2.Tables[0];

            idCollection2 = dt.AsEnumerable().Where(b => b.Field<int>("BattlesNum") > 0);

            foreach (var row in idCollection2)
            {
                IdRace[row.Field<String>("AvatarName")] = row.Field<int>("BattlesNum");
            }

            int i = 0;
            idCollection2 = dataTable.AsEnumerable().Where(b => b.Field<String>("AvatarName") != "");
            foreach (var row in idCollection2)
            {
                if (IdRace[row.Field<String>("AvatarName")]>0)
                {
                    ser.Points.AddXY(row.Field<String>("AvatarName"), IdRace[row.Field<String>("AvatarName")]);
                    ++i;
                }
            }
            chart1.Series.Add(ser);
            chart1.Legends.Clear();
            chart1.Series[0].ChartType = SeriesChartType.Line;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            Series ser = new Series();

            OleDbConnection Connection = new OleDbConnection(Login.Path);
            Connection.Open();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT AvatarName FROM Avatar";
            DataTable dataTable = new DataTable();
            var objDataAdapter = new OleDbDataAdapter(cmd);
            objDataAdapter.Fill(dataTable);
            var idCollection2 = dataTable.AsEnumerable().Where(b => b.Field<String>("AvatarName") != "");

            Dictionary<String, int> IdRace = new Dictionary<String, int>();
            foreach (var row in idCollection2)
            {
                IdRace.Add(row.Field<String>("AvatarName"), 0);
            }

            DataTable dt = new DataTable();
            dt = ds2.Tables[0];

            idCollection2 = dt.AsEnumerable().Where(b => b.Field<int>("BattlesNum") > 0);

            foreach (var row in idCollection2)
            {
                IdRace[row.Field<String>("AvatarName")] =
                    row.Field<int>("VictoryNum") - row.Field<int>("LossNum");
            }

            int i = 0;
            idCollection2 = dataTable.AsEnumerable().Where(b => b.Field<String>("AvatarName") != "");
            foreach (var row in idCollection2)
            {
                if (IdRace[row.Field<String>("AvatarName")] > 0)
                {
                    ser.Points.AddXY(row.Field<String>("AvatarName"), IdRace[row.Field<String>("AvatarName")]);
                    ++i;
                }
            }
            chart1.Series.Add(ser);
            chart1.Legends.Clear();
            chart1.Series[0].ChartType = SeriesChartType.Line;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            Series ser = new Series();

            OleDbConnection Connection = new OleDbConnection(Login.Path);
            Connection.Open();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT AvatarName FROM Avatar";
            DataTable dataTable = new DataTable();
            var objDataAdapter = new OleDbDataAdapter(cmd);
            objDataAdapter.Fill(dataTable);
            var idCollection2 = dataTable.AsEnumerable().Where(b => b.Field<String>("AvatarName") != "");

            Dictionary<String, int> IdRace = new Dictionary<String, int>();
            foreach (var row in idCollection2)
            {
                IdRace.Add(row.Field<String>("AvatarName"), 0);
            }

            DataTable dt = new DataTable();
            dt = ds2.Tables[0];

            idCollection2 = dt.AsEnumerable().Where(b => b.Field<int>("BattlesNum") > 0);

            foreach (var row in idCollection2)
            {
                IdRace[row.Field<String>("AvatarName")] = row.Field<int>("VictoryNum");
            }

            int i = 0;
            idCollection2 = dataTable.AsEnumerable().Where(b => b.Field<String>("AvatarName") != "");
            foreach (var row in idCollection2)
            {
                if (IdRace[row.Field<String>("AvatarName")] > 0)
                {
                    ser.Points.AddXY(row.Field<String>("AvatarName"), IdRace[row.Field<String>("AvatarName")]);
                    ++i;
                }
            }
            chart1.Series.Add(ser);
            chart1.Legends.Clear();
            chart1.Series[0].ChartType = SeriesChartType.Line;
        }
    }
}
