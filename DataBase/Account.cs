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
    public partial class Account : Form
    {
        public int AccountID;
        public static String path = "";
        public static int AvatarID = 0;
        public static int AvatarClass = 0;
        public Account()
        {
            InitializeComponent();
            UpdateGridView1();     
        }

        public void UpdateGridView1()
        {
            OleDbConnection Connection = new OleDbConnection(Login.Path);
            
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT Avatar.AvatarName, Avatar.Gender, Races.RaceName, " +
                    "Classes.ClassName, WorldMap.LocationName, " +
                    "Avatar.CharacterPositionX, Avatar.CharacterPositionY " +
                    "FROM ((Avatar " +
                    "INNER JOIN Races ON Avatar.AvatarRace = Races.RaceID) " +
                    "INNER JOIN Classes ON Avatar.AvatarClass = Classes.ClassID) " +
                    "INNER JOIN WorldMap ON Avatar.CharacterLocated = WorldMap.LocateID " +
                    "WHERE CharacterID = @CharacterID";
            cmd.Parameters.Add("@CharacterID", OleDbType.Integer).Value = Login.PersonID;

            DataTable dataTable = new DataTable();
            var objDataAdapter = new OleDbDataAdapter(cmd);
            objDataAdapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NewAvatar NADialog = new NewAvatar();
            NADialog.ShowDialog();
            UpdateGridView1();
        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            OleDbConnection Connection = new OleDbConnection(Login.Path);
            OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM Avatar", Connection);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "Avatar");

            DataTable table = dataSet.Tables[0];
            var LogIn =
                from account in dataSet.Tables[dataSet.Tables.IndexOf("Avatar")].AsEnumerable()
                where account.Field<int>("CharacterID") == Login.PersonID
                select account;

            var idCollection2 = LogIn.Where(b => b.Field<int>("AvatarID") > 0);
            int n = 0;
            foreach (var row in idCollection2)
            {
                if (n == dataGridView1.CurrentRow.Index)
                    path = row.Field<String>("AvatarModelPath");
                ++n;
            }

            adapter = new OleDbDataAdapter("SELECT * FROM Avatar", Connection);
            dataSet = new DataSet();
            adapter.Fill(dataSet, "Avatar");
            table = dataSet.Tables[0];
            LogIn =
                from account in dataSet.Tables[dataSet.Tables.IndexOf("Avatar")].AsEnumerable()
                where account.Field<String>("AvatarName") ==
                    Convert.ToString(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString())
                select account;
            var idCollection1 = LogIn.FirstOrDefault(b => b.Field<int>("AvatarID") > 0);
            AvatarID = idCollection1.Field<int>("AvatarID");
            AvatarClass = idCollection1.Field<int>("AvatarClass");

            AvatarInfo avatarinf = new AvatarInfo();
            avatarinf.Text = idCollection1.Field<String>("AvatarName");
            avatarinf.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogOK OK = new DialogOK();
            OK.ShowDialog();
            if (OK.isClickOK)
            {
                OleDbConnection Connection = new OleDbConnection(Login.Path);
                OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM Avatar", Connection);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet, "Avatar");
                DataTable table = dataSet.Tables[0];
                var LogIn =
                    from account in dataSet.Tables[dataSet.Tables.IndexOf("Avatar")].AsEnumerable()
                    where account.Field<String>("AvatarName") ==
                        Convert.ToString(dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString())
                    select account;
                var idCollection2 = LogIn.FirstOrDefault(b => b.Field<int>("AvatarID") > 0);
                int AvatarID = idCollection2.Field<int>("AvatarID");

                Connection.Open();
                var cmd = Connection.CreateCommand();
                cmd.CommandText = "DELETE FROM Avatar WHERE AvatarID = @AvatarID";
                cmd.Parameters.Add("@AvatarID", OleDbType.Integer, 255).Value = AvatarID;
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM AvatarStats WHERE AvatarID = @AvatarID";
                cmd.Parameters.Add("@AvatarID", OleDbType.Integer, 255).Value = AvatarID;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                Connection.Close();
                UpdateGridView1();
            }
        }
    }
}
