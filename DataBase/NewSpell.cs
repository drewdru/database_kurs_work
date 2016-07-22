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
    public partial class NewSpell : Form
    {
        public NewSpell()
        {
            InitializeComponent();
        }

        /*"IDSpell<>(SELECT IDSpell FROM AvatarSpell"+
            "WHERE AvatarID = @AvatarID)";*/
        private int getPointsToSpell()
        {
            OleDbConnection Connection = new OleDbConnection(Login.Path);
            Connection.Open();
            int flag = 0;
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT PointsToSpells FROM AvatarStats " +
                "WHERE AvatarID = @AvatarID";
            cmd.Parameters.Add("AvatarID", OleDbType.Char, 255).Value = Account.AvatarID;
            DataTable dataTable = new DataTable();
            var objDataAdapter = new OleDbDataAdapter(cmd);
            objDataAdapter.Fill(dataTable);
            flag = (int)dataTable.AsEnumerable().FirstOrDefault(b => b.Field<int>("PointsToSpells") > -1).Field<int>("PointsToSpells");
            return flag;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OleDbConnection Connection = new OleDbConnection(Login.Path);
            Connection.Open();
            int flag = 0;
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT PointsToSpells FROM AvatarStats " +
                "WHERE AvatarID = @AvatarID";
            cmd.Parameters.Add("AvatarID", OleDbType.Char, 255).Value = Account.AvatarID;
            DataTable dataTable = new DataTable();
            var objDataAdapter = new OleDbDataAdapter(cmd);
            objDataAdapter.Fill(dataTable);
            flag = getPointsToSpell();
            if(flag>0)
            {
                cmd = Connection.CreateCommand();
                cmd.CommandText = "INSERT INTO AvatarSpell " +
                "(AvatarID, IDSpell)" +
                "VALUES" +
                "(?,?)";
                cmd.Parameters.Add("AvatarID", OleDbType.Char, 255).Value = Account.AvatarID;
                cmd.Parameters.Add("IDSpell", OleDbType.Char, 255).Value =
                    (int)dataGridView1["IDSpell", dataGridView1.CurrentRow.Index].Value ;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                Connection.Close();
                NewSpell_Load(sender, e);
                --flag;
            }
            cmd.Dispose();
            Connection.Close();

            Connection.Open();
            cmd.CommandText = "UPDATE AvatarStats SET PointsToSpells = @PointsToSpells " +
                "WHERE AvatarID = @AvatarID";
            cmd.Parameters.Add("AvatarID", OleDbType.Integer, 255).Value = flag;
            cmd.Parameters.Add("AvatarID", OleDbType.Integer, 255).Value = Account.AvatarID;

            Connection.Close();
        }
        /*"IDSpell<>(SELECT IDSpell FROM AvatarSpell"+
            "WHERE AvatarID = @AvatarID)";*/
        /*SELECT *
FROM Customers
WHERE Country NOT IN (SELECT Country
FROM Customers
WHERE City= 'London');*/
        private void NewSpell_Load(object sender, EventArgs e)
        {
            OleDbConnection Connection = new OleDbConnection(Login.Path);
            Connection.Open();
            var cmd = Connection.CreateCommand();
            /*"SELECT SpellName, HowUse, Time, RecoveryTime, " +
            "Distance, Health, Mana, Stamina, IDSpell,Description "*/
            cmd.CommandText = "SELECT* " +
            "FROM Spells " +
            "WHERE Class = @Class AND " +
            "IDSpell NOT IN (SELECT IDSpell FROM AvatarSpell " +
            "WHERE AvatarID > 0)";
            cmd.Parameters.Add("@Class", OleDbType.Integer).Value = Account.AvatarClass;
            //cmd.Parameters.Add("@AvatarID", OleDbType.Integer).Value = Account.AvatarID;
            DataTable dataTable = new DataTable();
            var objDataAdapter = new OleDbDataAdapter(cmd);
            objDataAdapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
            dataGridView1.Columns["IDSpell"].Visible = false;
            dataGridView1.Columns["Description"].Visible = false;

            cmd.Dispose();
            Connection.Close();

            label2.Text = getPointsToSpell().ToString();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            textBox1.Text = dataGridView1["Description", dataGridView1.CurrentRow.Index].Value.ToString();
        }
    }
}
