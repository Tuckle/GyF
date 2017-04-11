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
    public partial class Register : Form
    {
        private OleDbConnection conn;

        public Register()
        {
            InitializeComponent();

            string connectionString = "Provider=OraOLEDB.Oracle;DATA SOURCE=localhost:1521/XE;PERSIST SECURITY INFO=True;USER ID=STUDENT;Password=STUDENT;";
            conn = new OleDbConnection(connectionString);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String firstName = textBox1.Text;
            String lastName = textBox2.Text;
            String email = textBox3.Text;
            String cellphone = textBox4.Text;
            String password = textBox5.Text;
            String confirmPassword = textBox6.Text;
            if (confirmPassword != password)
            {
                MessageBox.Show("The two passwords do not match. Make sure they are the same password!", "GyF Register", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Regex nameValidator = new Regex(@"^\s*\w+\s*$");
            Regex emailValidator = new Regex(@"^([\w\.]+)@(\w+)((\.(\w){2,3})+)$");
            Regex phoneNumberValidator = new Regex(@"^\s*\+?\(?\d+\)?\s*\d*\s*$");
            Regex passwordValidator = new Regex("^[A-Za-z0-9.@!#$%]");
            if (!nameValidator.IsMatch(firstName))
            {
                MessageBox.Show("First name contains unwanted special characters!", "GyF Register", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!nameValidator.IsMatch(lastName))
            {
                MessageBox.Show("Last name contains unwanted special characters!", "GyF Register", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(!emailValidator.IsMatch(email))
            {
                MessageBox.Show("The email is not valid!", "GyF Register", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(!phoneNumberValidator.IsMatch(cellphone)) {
                MessageBox.Show("The cellphone is not valid!", "GyF Register", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(!passwordValidator.IsMatch(password))
            {
                MessageBox.Show("The password is not valid!", "GyF Register", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                conn.Open();
                try
                {
                    //OleDbCommand command = new OleDbCommand("INSERT INTO users_ (firstname, lastname, email, cellphone, password) VALUES('" + firstName + "','" + lastName + "','" + email +"','" + cellphone + "','" + password + "')", conn);
                    //OleDbCommand command = new OleDbCommand("select user_manager.addUser('" + firstName + "','" + lastName + "','" + email + "','" + cellphone + "','" + password + "') from dual", conn);
                    OleDbCommand command = new OleDbCommand("user_manager.addUser", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("firstName", OleDbType.VarChar).Value = firstName;
                    command.Parameters.Add("lasttName", OleDbType.VarChar).Value = lastName;
                    command.Parameters.Add("email", OleDbType.VarChar).Value = email;
                    command.Parameters.Add("cellphone", OleDbType.VarChar).Value = cellphone;
                    command.Parameters.Add("passwd", OleDbType.VarChar).Value = password;
                    command.ExecuteNonQuery();
                    MessageBox.Show("User has been created!");
                    this.Close();
                    
                }
                catch(OleDbException exception)
                {
                    string result = "";
                    for (int i = 0; i < exception.Errors.Count; i++)
                    {
                        result += "Index #" + i + "\n" + "Message: " + exception.Errors[i].Message + "\n";
                    }
                    MessageBox.Show("Error!\n" + result);
                }
                conn.Close();
            }
        }
    }
}
