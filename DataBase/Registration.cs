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
    public partial class Registration : Form
    {
        OleDbConnection Connection;
        public Registration()
        {
            InitializeComponent();
            Connection = new OleDbConnection(Login.Path);
            Connection.Open();
        }
        public DataSet gameDataSet;
        private void button1_Click(object sender, EventArgs e)
        {
            OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM CharacterAccount", Connection);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "CharacterAccount");

            //DataTable table = dataSet.Tables[0];
            var Registration =
                from account in dataSet.Tables[dataSet.Tables.IndexOf("CharacterAccount")].AsEnumerable()
                where account.Field<String>("CharacterLogin") == textBox1.Text
                select account;
            if (Registration.LongCount() == 0)
            {                
                if (textBox2.Text == textBox3.Text)
                {
                    /*OleDbCommand Command;
                    Command = new OleDbCommand("INSERT INTO CharacterAccount (CharacterLogin,CharacterPass)" +
                        "VALUES(?,?)", Connection);
                    Command.Parameters.Add("CharacterLogin", OleDbType.Char, 100, textBox1.Text);
                    Command.Parameters.Add("CharacterPass", OleDbType.Char, 100, textBox2.Text);
                    adapter.InsertCommand = Command;

                    var table = Registration.ToArray();
                    MessageBox.Show(table[0]["CharacterLogin"].ToString());*/
                    var cmd = Connection.CreateCommand();
                    cmd.CommandText = "INSERT INTO CharacterAccount" +
                    "(CharacterLogin, CharacterPass)" +
                    "VALUES" +
                    "(?,?)";
                    cmd.Parameters.Add("CharacterLogin", OleDbType.Char, 255).Value = textBox1.Text;
                    cmd.Parameters.Add("CharacterPass", OleDbType.Char, 255).Value = textBox2.Text;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    this.Close();
                }
                else
                    MessageBox.Show("Пароли не совпадают!");
            }
            else
                MessageBox.Show("К сожалению, такой логин уже есть в нашей базе, попробуйте зарегистрировать другой.");
      
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*Connection = new OleDbConnection(Login.dbPuth);
            Connection.Open();
               var cmd = Connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM CharacterAccount ";

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader != null)
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                MessageBox.Show(reader.GetValue(1).ToString());
                            }
                        }
                        //MessageBox.Show(reader.GetValue(1).ToString());
                        //MessageBox.Show(reader.GetString(1).ToString());
                    }
                }
                cmd.Dispose();*/
            this.Close();
        }
    }
}
