using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GyF
{
    public partial class Main : Form
    {
        private int userId;
        private string firstName;
        private string lastName;
        private string email;
        private string cellphone;
        private OleDbConnection conn;

        public Main(String email)
        {
            InitializeComponent();

            /* select from database the row matching the input email
             * and initialize private attributes */
            string connectionString = "Provider=OraOLEDB.Oracle;DATA SOURCE=localhost:1521/XE;PERSIST SECURITY INFO=True;USER ID=STUDENT;Password=STUDENT;";
            conn = new OleDbConnection(connectionString);

            conn.Open();
            OleDbCommand command = new OleDbCommand("SELECT * FROM users_ WHERE email='" + email + "'", conn);
            OleDbDataReader dataReader = command.ExecuteReader();
            dataReader.Read();
            userId = int.Parse(dataReader.GetValue(0).ToString());
            firstName = dataReader.GetValue(1).ToString();
            lastName = dataReader.GetValue(2).ToString();
            email = dataReader.GetValue(3).ToString();
            cellphone = dataReader.GetValue(4).ToString();
            conn.Close();

            label1.Text = "Welcome, " + firstName + " " + lastName;

            conn.Open();
            OleDbDataAdapter adapter = new OleDbDataAdapter("Select c.companyid as \"Id\", c.name as \"Name\", c.website as \"Website\", c.location  as \"Location\" from companies c join owners o on c.companyId = o.companyId join users_  u on u.userId = o.userId where u.email='" + email + "'", conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();

        }

        private void Main_Load(object sender, EventArgs e)
        {
            /* connect to database and retrieve list of user companies */
            //CTRL+K+C to comment and CTRL+K+U to uncomment lines of code
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int selectedRowCount = dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount != 1)
            {
                MessageBox.Show("Only one row must pe selected!", "Error selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                int id = int.Parse(Convert.ToString(row.Cells["Id"].Value));
                Details details = new Details(id);
                this.Hide();
                details.ShowDialog();
                this.Show();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            conn.Open();
            OleDbCommand command = new OleDbCommand("getUserRanking", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("user_id", OleDbType.Numeric).Value = userId;
            command.Parameters.Add("rank_", OleDbType.Numeric).Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add("rank_nr", OleDbType.Numeric).Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add("total", OleDbType.Numeric).Direction = ParameterDirection.ReturnValue;
            command.ExecuteNonQuery();
            string result = "";
            result = "Your rank is: " + command.Parameters["rank_"].Value.ToString() + ".\n";
            result += "You are: " + command.Parameters["rank_nr"].Value.ToString() + " out of " + command.Parameters["total"].Value.ToString();
            MessageBox.Show(result);
            conn.Close();
        }
    }
}
