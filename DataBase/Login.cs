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
    public partial class Login : Form
    {
        public static String Path = "Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data Source=.//data/Game.accdb";
        public static String PersonLogin;
        public static String PersonPass;
        public static int PersonID;
        public Login()
        {
            InitializeComponent();                      
        }
        private void Login_Load(object sender, EventArgs e) {}

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" && textBox2.Text == "")
            {
                this.Visible = false;
                Admin ad = new Admin();
                ad.ShowDialog();
                this.Visible = true;
            }
            else
            {
                OleDbConnection Connection = new OleDbConnection(Path);  
                OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM CharacterAccount", Connection);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet, "CharacterAccount");

                DataTable table = dataSet.Tables[0];
                var LogIn =
                    from account in dataSet.Tables[dataSet.Tables.IndexOf("CharacterAccount")].AsEnumerable()
                    where account.Field<String>("CharacterLogin") == textBox1.Text &&
                        account.Field<String>("CharacterPass") == textBox2.Text
                    select account;

                if (LogIn.LongCount() > 0)
                {
                    PersonLogin = textBox1.Text;
                    PersonPass = textBox2.Text;                
                    //var idCollection = LogIn.Where(b => b.Field<int>("CharacterID") > 0).Select(b => b.Field<int>("CharacterID"));
                    var idCollection2 = LogIn.Where(b => b.Field<int>("CharacterID") > 0);
                    foreach (var row in idCollection2)
                    {
                        PersonID = row.Field<int>("CharacterID");
                    }
                    //LogInAccount.AccountID = LogIn.Where(b => b.Field<int>("CharacterID") > 0).Field<int>("CharacterID");
                
                    this.Visible = false;
                    Account LogInAccount = new Account();
                    LogInAccount.ShowDialog();
                    this.Visible = true;
                }
                else
                    MessageBox.Show("Соединение не установленно. Неверный логин или пароль!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Registration reg = new Registration();
            reg.ShowDialog();
        }

        //gameDataSet.Reset();
        //gameDataSet.AcceptChanges
    }
}

//получение значений ячейки через команду
/*
OleDbConnection Connection = new OleDbConnection(Login.dbPuth);
            Connection.Open();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM CharacterAccount WHERE CharacterLogin = @CharacterLogin AND CharacterPass = @CharacterPass";
            cmd.Parameters.Add("@CharacterLogin", OleDbType.Char, 255).Value = Login.PersonLogin;
            cmd.Parameters.Add("@CharacterPass", OleDbType.Char, 255).Value = Login.PersonPass;

            using (var reader = cmd.ExecuteReader())
            {
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            MessageBox.Show(reader.GetValue(0).ToString());
                        }
                    }
                }
            }
            cmd.Dispose();*/

//получение значений ячейки через DataTable
/*
OleDbConnection Connection = new OleDbConnection(dbPuth);  
            OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM CharacterAccount", Connection);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "CharacterAccount");

            DataTable table = dataSet.Tables[0];
            var LogIn =
                from account in dataSet.Tables[dataSet.Tables.IndexOf("CharacterAccount")].AsEnumerable()
                where account.Field<String>("CharacterLogin") == textBox1.Text &&
                    account.Field<String>("CharacterPass") == textBox2.Text
                select account;

            if (LogIn.LongCount() > 0)
            {
                PersonLogin = textBox1.Text;
                PersonPass = textBox2.Text;                
                //var idCollection = LogIn.Where(b => b.Field<int>("CharacterID") > 0).Select(b => b.Field<int>("CharacterID"));
                var idCollection2 = LogIn.Where(b => b.Field<int>("CharacterID") > 0);
                foreach (var row in idCollection2)
                {
                    PersonID = row.Field<int>("CharacterId");
                }
                //LogInAccount.AccountID = LogIn.Where(b => b.Field<int>("CharacterID") > 0).Field<int>("CharacterID");
                
                this.Visible = false;
                Account LogInAccount = new Account();
                LogInAccount.ShowDialog();
                this.Visible = true;
            }*/