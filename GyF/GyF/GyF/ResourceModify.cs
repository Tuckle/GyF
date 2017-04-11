using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Text.RegularExpressions;

namespace GyF
{
    
    public partial class ResourceModify : Form
    {
        private int serviceId;
        private int companyId;
        private int cost;
        private String name;
        private String description;
        private Boolean modify;
        private OleDbConnection conn;
        public ResourceModify(int serviceId, int companyId, String name, String description, int Cost, Boolean modify)
        {
            InitializeComponent();
            this.serviceId = serviceId;
            this.companyId = companyId;
            this.name = name;
            this.description = description;
            this.modify = modify;
            this.cost = Cost;
            if (this.modify == true)
            {
                textBox1.Text = this.name;
                textBox3.Text = this.description;
                textBox2.Text = Convert.ToString(this.cost);
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
            Regex costValidator = new Regex(@"^[0-9]+$");
            if (!textValidator.IsMatch(textBox1.Text))
            {
                MessageBox.Show("Invalid name!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!textValidator.IsMatch(textBox3.Text))
            {
                MessageBox.Show("Invalid description!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!costValidator.IsMatch(textBox2.Text))
            {
                MessageBox.Show("Invalid cost!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {

                if (modify == true)
                {
                    try
                    {
                        conn.Open();
                        
                        OleDbCommand cmd = new OleDbCommand("update resources set name = '" + textBox1.Text + "', description = '" + textBox3.Text + "', cost =" + int.Parse(textBox2.Text) + "where resourceId=" + serviceId + " and companyId =" + companyId, conn);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data has been saved!", "OK!", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        
                        OleDbCommand cmd = new OleDbCommand("insert into resources (companyId, name, description, cost) values(" + companyId + ",'" + textBox1.Text + "', '" + textBox3.Text + "'," + int.Parse(textBox2.Text) + ")", conn);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data has been saved!", "OK!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        conn.Close();
                    }
                    catch (Exception ext)
                    {
                        MessageBox.Show("Error!\n" + ext.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                this.Close();
            }
        }
    }
}
