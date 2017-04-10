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

        public Main(String username)
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

            label1.Text = "Welcome, " + username;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            /* connect to database and retrieve list of user companies */
            //CTRL+K+C to comment and CTRL+K+U to uncomment lines of code
            //conn.Open();
            //OleDbCommand command = new OleDbCommand("SELECT * FROM companies WHERE email='" + email + "'", conn);
            //OleDbDataReader dataReader = command.ExecuteReader();
            
            //conn.Close();
        }
    }
}
