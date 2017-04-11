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
    public partial class ServiceModify : Form
    {
        private int serviceId;
        private int companyId;
        private String name;
        private String description;
        private Boolean modify;
        private OleDbConnection conn;
        public ServiceModify(int serviceId, int companyId, String name, String description, Boolean modify)
        {
            InitializeComponent();

            this.serviceId = serviceId;
            this.companyId = companyId;
            this.name = name;
            this.description = description;
            this.modify = modify;
            if (this.modify == true)
            {
                textBox1.Text = this.name;
                textBox2.Text = this.description;
            }
            string connectionString = "Provider=OraOLEDB.Oracle;DATA SOURCE=localhost:1521/XE;PERSIST SECURITY INFO=True;USER ID=STUDENT;Password=STUDENT;";
            conn = new OleDbConnection(connectionString);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Regex textValidator = new Regex(@"^[^;\-]+$");

            if (!textValidator.IsMatch(textBox1.Text))
            {
                MessageBox.Show("Invalid name!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!textValidator.IsMatch(textBox2.Text))
            {
                MessageBox.Show("Invalid description!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {

                if (modify == true)
                {
                    try
                    {
                        conn.Open();
                        OleDbCommand cmd = new OleDbCommand("update services set name = '" + textBox1.Text + "', description = '" + textBox2.Text + "' where serviceid=" + serviceId + " and companyId =" + companyId, conn);
                        MessageBox.Show("Data has been saved!", "OK!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch (Exception ext)
                    {
                        MessageBox.Show("Error!\n" + ext.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    try
                    {
                        conn.Open();
                        OleDbCommand cmd = new OleDbCommand("insert into services (companyId, name, description) values(" + companyId + ",'" + textBox1.Text + "', '" + textBox2.Text + "')", conn);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data has been saved!", "OK!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        conn.Close();
                    }
                    catch(Exception ext)
                    {
                        MessageBox.Show("Error!\n" + ext.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                this.Close();
            }
        }
    }
}
