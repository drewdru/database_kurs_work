using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Windows.Forms.DataVisualization.Charting;

namespace DataBase
{
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();
            tabControl1.SelectedIndex = 1;
        }

        private void Admin_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'GameDataSet.Avatar' table. You can move, or remove it, as needed.
            this.AvatarTableAdapter.Fill(this.GameDataSet.Avatar);
            tabControl1.SelectedIndex = 0; this.reportViewer1.RefreshReport();
            this.reportViewer1.RefreshReport();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0: GameTimeStat(); break;
                case 1: VisitToLocation(); break;
                case 2: Reports(); break;
            }
        }
        
        DataSet dsGTS = new DataSet();
        DataSet dsGTSFilter = new DataSet();
        private void GameTimeStat()
        {
            OleDbConnection Connection = new OleDbConnection(Login.Path);
            Connection.Open();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT ComeToPlay.*,Avatar.AvatarName,Races.RaceName,Classes.ClassName,Avatar.Gender " +
                "FROM ((ComeToPlay INNER JOIN " +
                "Avatar ON Avatar.AvatarID = ComeToPlay.AvatarID)" +
                "INNER JOIN Races ON Races.RaceID = Avatar.AvatarRace)" +
                "INNER JOIN Classes ON Classes.ClassID = Avatar.AvatarClass";
            DataTable dataTable = new DataTable();
            var objDataAdapter = new OleDbDataAdapter(cmd);
            objDataAdapter.Fill(dsGTS);
            objDataAdapter.Fill(dsGTSFilter);
            dataGridView1.DataSource = dsGTS.Tables[0];
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CurrencyManager currencyManager = (CurrencyManager)BindingContext[
                    dataGridView1.DataSource, dataGridView1.DataMember]; //dgMain - это ГРИД
                if (textBox2.Text == string.Empty) //tbMask - это ТекстБокс
                {
                    currencyManager.Position = -1;
                    return;
                }
                DataView dataView = (DataView)currencyManager.List;
                string query = string.Format("{0} LIKE '{1}%'",
                    Convert.ToString("AvatarName"), Convert.ToString(textBox2.Text)); // COL_NAME - имя колонки
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

        private void button9_Click(object sender, EventArgs e)
        {
            int i = dataGridView1.CurrentRow.Index + 1;
            string date1 = dateTimePicker3.Value.ToString("dd/MM/yyyy");
            for (; i < dataGridView1.RowCount; i++)
            {
                dataGridView1[3, i].Style.BackColor = Color.White;
                if (dataGridView1[3, i].FormattedValue.ToString().Contains(date1))
                {
                    //dataGridView2[2, i].Style.BackColor = Color.Red;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[3];
                    break;
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            int i = dataGridView1.CurrentRow.Index - 1;
            string date1 = dateTimePicker3.Value.ToString("dd/MM/yyyy");
            for (; i > 0; i--)
            {
                dataGridView1[3, i].Style.BackColor = Color.White;
                if (dataGridView1[3, i].FormattedValue.ToString().Contains(date1))
                {
                    //dataGridView2[2, i].Style.BackColor = Color.Red;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[3];
                    break;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                dataGridView1.DataSource = dsGTS.Tables[0];
            ((DataTable)dataGridView1.DataSource).DefaultView.RowFilter = "RaceName LIKE '%" + textBox1.Text + "%'";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                dataGridView1.DataSource = dsGTS.Tables[0];
            ((DataTable)dataGridView1.DataSource).DefaultView.RowFilter = "ClassName LIKE '%" + textBox1.Text + "%'";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OleDbConnection Connection = new OleDbConnection(Login.Path);
            Connection.Open();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT ComeToPlay.*,Avatar.AvatarName,Races.RaceName,Classes.ClassName,Avatar.Gender " +
                "FROM ((ComeToPlay INNER JOIN " +
                "Avatar ON Avatar.AvatarID = ComeToPlay.AvatarID)" +
                "INNER JOIN Races ON Races.RaceID = Avatar.AvatarRace)" +
                "INNER JOIN Classes ON Classes.ClassID = Avatar.AvatarClass " +
                "WHERE ComeDate BETWEEN @date1 AND @date2";
            cmd.Parameters.Add("@date1", OleDbType.Date).Value = dateTimePicker1.Value;
            cmd.Parameters.Add("@date2", OleDbType.Date).Value = dateTimePicker2.Value;
            DataTable dataTable = new DataTable();
            var objDataAdapter = new OleDbDataAdapter(cmd);
            objDataAdapter.Fill(dataTable);
            dsGTSFilter.Clear();
            objDataAdapter.Fill(dsGTSFilter);
            dataGridView1.DataSource = dataTable;
        }

        DataSet dsVTL = new DataSet();
        DataSet dsVTLFilter = new DataSet();
        private void VisitToLocation()
        {
            OleDbConnection Connection = new OleDbConnection(Login.Path);
            Connection.Open();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT LocalVisitors.*,WorldMap.LocationName  FROM LocalVisitors " +
                "INNER JOIN WorldMap ON WorldMap.LocateID = LocalVisitors.LocateID";
            DataTable dataTable = new DataTable();
            var objDataAdapter = new OleDbDataAdapter(cmd);
            objDataAdapter.Fill(dsVTL);
            dsVTLFilter.Clear();
            objDataAdapter.Fill(dsVTLFilter);
            dataGridView2.DataSource = dsVTL.Tables[0];
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int i = dataGridView2.CurrentRow.Index + 1;
            string date1 = dateTimePicker5.Value.ToString("dd/MM/yyyy");
            for (; i < dataGridView2.RowCount; i++)
            {
                dataGridView2[2, i].Style.BackColor = Color.White;
                if (dataGridView2[2, i].FormattedValue.ToString().Contains(date1))
                {
                    //dataGridView2[2, i].Style.BackColor = Color.Red;
                    dataGridView2.CurrentCell = dataGridView2.Rows[i].Cells[2];
                    break;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int i = dataGridView2.CurrentRow.Index - 1;
            string date1 = dateTimePicker5.Value.ToString("dd/MM/yyyy");
            for (; i > 0; i--)
            {
                dataGridView2[2, i].Style.BackColor = Color.White;
                if (dataGridView2[2, i].FormattedValue.ToString().Contains(date1))
                {
                    //dataGridView2[2, i].Style.BackColor = Color.Red;
                    dataGridView2.CurrentCell = dataGridView2.Rows[i].Cells[2];
                    break;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OleDbConnection Connection = new OleDbConnection(Login.Path);
            Connection.Open();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT LocalVisitors.*,WorldMap.LocationName  FROM LocalVisitors " +
                "INNER JOIN WorldMap ON WorldMap.LocateID = LocalVisitors.LocateID " +
                "WHERE VisitDate BETWEEN @date1 AND @date2";
            cmd.Parameters.Add("@date1", OleDbType.Date).Value = dateTimePicker4.Value;
            cmd.Parameters.Add("@date2", OleDbType.Date).Value = dateTimePicker6.Value;
            DataTable dataTable = new DataTable();
            var objDataAdapter = new OleDbDataAdapter(cmd);
            objDataAdapter.Fill(dataTable);
            dsVTLFilter.Clear();
            objDataAdapter.Fill(dsVTLFilter);
            dataGridView2.DataSource = dataTable;
        }

        Series ser1 = new Series();
        private void button1_Click(object sender, EventArgs e)
        {
            ser1.Points.Clear();
            chart1.Series.Clear();

            OleDbConnection Connection = new OleDbConnection(Login.Path);
            Connection.Open();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT RaceID,RaceName FROM Races";
            DataTable dataTable = new DataTable();
            var objDataAdapter = new OleDbDataAdapter(cmd);
            objDataAdapter.Fill(dataTable);
            var idCollection2 = dataTable.AsEnumerable().Where(b => b.Field<int>("RaceID") > 0);

            Dictionary<String, int> IdRace = new Dictionary<String, int>();
            foreach (var row in idCollection2)
            {
                IdRace.Add(row.Field<String>("RaceName"), 0);
            }

            DataTable dt = new DataTable();
            dt = dsGTSFilter.Tables[0];

            idCollection2 = dt.AsEnumerable().Where(b => b.Field<int>("id") > 0);
            
            foreach (var row in idCollection2)
            {
                ++IdRace[row.Field<String>("RaceName")];
            }
                        
            int i = 0;
            idCollection2 = dataTable.AsEnumerable().Where(b => b.Field<int>("RaceID") > 0);
            foreach (var row in idCollection2)
            {
                ser1.Points.AddXY(row.Field<String>("RaceName"), IdRace[row.Field<String>("RaceName")]);
                ++i;
            }
            chart1.Series.Add(ser1);
            chart1.Legends.Clear();
            chart1.Series[0].ChartType = SeriesChartType.Bar;
        }

        Series ser2 = new Series();
        private void button8_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            ser2.Points.Clear();

            OleDbConnection Connection = new OleDbConnection(Login.Path);
            Connection.Open();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT ClassID,ClassName FROM Classes";
            DataTable dataTable = new DataTable();
            var objDataAdapter = new OleDbDataAdapter(cmd);
            objDataAdapter.Fill(dataTable);
            var idCollection2 = dataTable.AsEnumerable().Where(b => b.Field<int>("ClassID") > 0);

            Dictionary<String, int> IdRace = new Dictionary<String, int>();
            foreach (var row in idCollection2)
            {
                IdRace.Add(row.Field<String>("ClassName"), 0);
            }

            DataTable dt = new DataTable();
            dt = dsGTSFilter.Tables[0];

            idCollection2 = dt.AsEnumerable().Where(b => b.Field<int>("id") > 0);

            foreach (var row in idCollection2)
            {
                ++IdRace[row.Field<String>("ClassName")];
            }

            int i = 0;
            idCollection2 = dataTable.AsEnumerable().Where(b => b.Field<int>("ClassID") > 0);
            foreach (var row in idCollection2)
            {
                ser2.Points.AddXY(row.Field<String>("ClassName"), IdRace[row.Field<String>("ClassName")]);
                ++i;
            }
            chart1.Series.Add(ser2);
            chart1.Legends.Clear();
            chart1.Series[0].ChartType = SeriesChartType.Bar;
        }

        Series ser3 = new Series();
        private void button11_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            ser3.Points.Clear();

            DataTable table = new DataTable("Gender");
            DataColumn column;
            DataRow row;
            column = new DataColumn();
            column.DataType= System.Type.GetType("System.String");
            column.ColumnName = "Gender";
            column.AutoIncrement = false;
            column.Caption = "Пол";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);
            row = table.NewRow();
            row["Gender"] = "Не определён";
            table.Rows.Add(row);
            row = table.NewRow();
            row["Gender"] = "Мужской";
            table.Rows.Add(row);
            row = table.NewRow();
            row["Gender"] = "Женский";
            table.Rows.Add(row);

            var idCollection2 = table.AsEnumerable().Where(b => b.Field<String>("Gender") != "");

            Dictionary<String, int> IdRace = new Dictionary<String, int>();
            foreach (var row1 in idCollection2)
            {
                IdRace.Add(row1.Field<String>("Gender"), 0);
            }

            DataTable dt = new DataTable();
            dt = dsGTSFilter.Tables[0];

            idCollection2 = dt.AsEnumerable().Where(b => b.Field<int>("id") > 0);

            foreach (var row1 in idCollection2)
            {
                ++IdRace[row1.Field<String>("Gender")];
            }

            int i = 0;
            idCollection2 = table.AsEnumerable().Where(b => b.Field<String>("Gender") != "");
            foreach (var row1 in idCollection2)
            {
                ser3.Points.AddXY(row1.Field<String>("Gender"), IdRace[row1.Field<String>("Gender")]);
                ++i;
            }
            chart1.Series.Add(ser3);
            chart1.Legends.Clear();
            chart1.Series[0].ChartType = SeriesChartType.Bar;
        }
        Series ser4 = new Series();
        private void button12_Click(object sender, EventArgs e)
        {
            chart2.Series.Clear();
            ser4.Points.Clear();

            OleDbConnection Connection = new OleDbConnection(Login.Path);
            Connection.Open();
            var cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT LocateID FROM WorldMap";
            DataTable dataTable = new DataTable();
            var objDataAdapter = new OleDbDataAdapter(cmd);
            objDataAdapter.Fill(dataTable);
            var idCollection2 = dataTable.AsEnumerable().Where(b => b.Field<int>("LocateID") > 0);

            Dictionary<int, int> IdRace = new Dictionary<int, int>();
            foreach (var row in idCollection2)
            {
                IdRace.Add(row.Field<int>("LocateID"), 0);
            }

            DataTable dt = new DataTable();
            dt = dsVTLFilter.Tables[0];

            idCollection2 = dt.AsEnumerable().Where(b => b.Field<int>("id") > 0);

            foreach (var row in idCollection2)
            {
                ++IdRace[row.Field<int>("LocateID")];
            }

            int i = 0;
            idCollection2 = dataTable.AsEnumerable().Where(b => b.Field<int>("LocateID") > 0);
            foreach (var row in idCollection2)
            {
                ser4.Points.AddXY(row.Field<int>("LocateID"), IdRace[row.Field<int>("LocateID")]);
                ++i;
            }
            chart2.Series.Add(ser4);
            chart2.Legends.Clear();
            chart2.Series[0].ChartType = SeriesChartType.Column;
        }

        private void Reports()
        {
            
        }
    }
}
