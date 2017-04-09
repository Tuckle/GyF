using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.OleDb;

namespace GyF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to exit? Are you sure?", "GyF Login", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private String connectionString = null;
        private OleDbConnection connection;
        private OleDbCommand command;
        private OleDbDataReader dataReader;
        private void button1_Click(object sender, EventArgs e)
        {

            String email = textBox1.Text;
            String password = textBox2.Text;
            bool emailOk = true;
            bool passwordOk = true;
            Regex emailValidator = new Regex(@"^([\w\.]+)@(\w+)((\.(\w){2,3})+)$");
            if (!emailValidator.IsMatch(email))
            {
                emailOk = false;
            }

            Regex passwordValidator = new Regex("^[A-Za-z0-9.@!#$%]");
            if (!passwordValidator.IsMatch(password))
            {
                passwordOk = false;
            }

            if (emailOk == false && passwordOk == false)
            {
                MessageBox.Show("Both email and password fields contains invalid characters!", "Invalid email / password", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (emailOk == false)
            {
                MessageBox.Show("Email field contain invalid characters!", "Invalid email", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (passwordOk == false)
            {
                MessageBox.Show("Password field contains invalid characters!", "Invalid password", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                connectionString = "Provider=OraOLEDB.Oracle;DATA SOURCE=localhost:1521/XE;PERSIST SECURITY INFO=True;USER ID=STUDENT;Password=STUDENT;";
                connection = new OleDbConnection(connectionString);
                try
                {
                    connection.Open();
                    MessageBox.Show("Connection with the database has been made!", "Succesfull connection!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    command = new OleDbCommand("Select count(*) from users_ where email='" + email + "' and password='" + password + "'", connection);
                    dataReader = command.ExecuteReader();
                    dataReader.Read();
                    int value = int.Parse(dataReader.GetValue(0).ToString());
                    connection.Close();
                    if (value != 0)
                    {
                        MessageBox.Show("Succesfull login!", "GyF Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Invalid login!", "GyF Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Invalid connection to the database! Error:\n" + ex.ToString(), "Database error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
    }
}
