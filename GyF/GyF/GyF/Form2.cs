﻿using System;
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
    public partial class Details : Form
    {
        private int companyId;
        private OleDbConnection conn;
        public Details(int companyId)
        {
            InitializeComponent();

            int pageNumber = 1;
            

            this.companyId = companyId;
            string connectionString = "Provider=OraOLEDB.Oracle;DATA SOURCE=localhost:1521/XE;PERSIST SECURITY INFO=True;USER ID=STUDENT;Password=STUDENT;";
            conn = new OleDbConnection(connectionString);
            conn.Open();
            //OleDbDataAdapter adapter = new OleDbDataAdapter("Select * from services where companyid = " + companyId, conn);
            DataTable dt = new DataTable();
            OleDbDataAdapter adapter = new OleDbDataAdapter("Select * from ( select a.*, rownum r__ from (Select * from resourcesMaterializedView where companyid = " + companyId + ") a where rownum < (" + pageNumber * 10 + ")) where r__ >= (" + (pageNumber - 1) * 10 + ")", conn);
            adapter.Fill(dt);
            dataGridView2.DataSource = dt;
            conn.Close();

            conn.Open();
            adapter = new OleDbDataAdapter("Select * from ( select a.*, rownum r__ from (Select * from services where companyid = " + companyId + ") a where rownum < (" + pageNumber * 10 + ")) where r__ >= (" + (pageNumber - 1) * 10 + ")", conn);
            dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Details_Load(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            String filterQuery = textBox1.Text;
            Regex filterVerifier = new Regex(@"^[\w\s'\(\)\<\>=!%]*$");
            if (!filterVerifier.IsMatch(filterQuery))
            {
                MessageBox.Show("Invalid filter query!", "Invalid Query!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (filterQuery.Length == 0)
                {
                    filterQuery = "'a' = 'a'";
                }
                try
                {
                    conn.Open();
                    OleDbDataAdapter adapter = new OleDbDataAdapter("Select * from services where companyid = " + companyId + " and (" + filterQuery + ")", conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                    conn.Close();
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Invalid filter command!\n" + exp.ToString(), "Exception!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String filterQuery = textBox2.Text;
            Regex filterVerifier = new Regex(@"^[\w\s'\(\)\<\>=!%]*$");
            if (!filterVerifier.IsMatch(filterQuery))
            {
                MessageBox.Show("Invalid filter query!", "Invalid Query!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    conn.Open();
                    int pageNumber;
                    if (textBox3.Text != null && textBox3.Text != "")
                        pageNumber = int.Parse(textBox3.Text);
                    else
                        pageNumber = 1;
                    if (pageNumber < 1)
                        pageNumber = 1;
                    OleDbDataAdapter adapter = new OleDbDataAdapter("Select * from ( select a.*, rownum r__ from (Select * from resourcesMaterializedView where companyid = " + companyId + " and " + filterQuery + ") a where rownum < (" + pageNumber * 10 + ")) where r__ >= (" + (pageNumber - 1) * 10 + ")", conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView2.DataSource = dt;
                    conn.Close();
                    this.Show();
                }
                catch(Exception exp)
                {
                    MessageBox.Show("Invalid filter command!\n" + exp.ToString(), "Exception!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            int selectedRowCount = dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount != 1)
            {
                MessageBox.Show("Only one row must pe selected!", "Error selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                int id = int.Parse(Convert.ToString(row.Cells["serviceId"].Value));

                // delete from db
                conn.Open();
                OleDbCommand cmd = new OleDbCommand("delete from services where serviceid=" + id, conn);
                cmd.ExecuteNonQuery();
                conn.Close();

                // refresh gridview table
                conn.Open();
                OleDbDataAdapter adapter = new OleDbDataAdapter("Select * from services where companyid = " + companyId, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
                conn.Close();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            int selectedRowCount = dataGridView2.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount != 1)
            {
                MessageBox.Show("Only one row must pe selected!", "Error selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DataGridViewRow row = dataGridView2.SelectedRows[0];
                int id = int.Parse(Convert.ToString(row.Cells["resourceId"].Value));
                // delete from db
                conn.Open();
                OleDbCommand cmd = new OleDbCommand("delete from resourcesMaterializedView where resourceid=" + id, conn);
                cmd.ExecuteNonQuery();
                conn.Close();

                // refresh gridview table
                conn.Open();
                OleDbDataAdapter adapter = new OleDbDataAdapter("Select * from resourcesMaterializedView where companyid = " + companyId, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView2.DataSource = dt;
                conn.Close();
            }
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ServiceModify sm = new ServiceModify(-1, companyId, "", "", false);
            this.Hide();
            sm.ShowDialog();
            // refresh gridview table
            conn.Open();
            OleDbDataAdapter adapter = new OleDbDataAdapter("Select * from services where companyid = " + companyId, conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
            this.Show();
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
                int id = int.Parse(Convert.ToString(row.Cells["serviceId"].Value));
                ServiceModify sm = new ServiceModify(id, companyId, Convert.ToString(row.Cells["name"].Value), Convert.ToString(row.Cells["description"].Value), true);
                this.Hide();
                sm.ShowDialog();
                // refresh gridview table
                conn.Open();
                OleDbDataAdapter adapter = new OleDbDataAdapter("Select * from services where companyid = " + companyId, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
                conn.Close();
                this.Show();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ResourceModify rm = new ResourceModify(-1, companyId, "", "", -1, false);
            this.Hide();
            rm.ShowDialog();
            // refresh gridview table
            conn.Open();
            OleDbDataAdapter adapter = new OleDbDataAdapter("Select * from resourcesMaterializedView where companyid = " + companyId, conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView2.DataSource = dt;
            conn.Close();
            this.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int selectedRowCount = dataGridView2.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount != 1)
            {
                MessageBox.Show("Only one row must pe selected!", "Error selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DataGridViewRow row = dataGridView2.SelectedRows[0];
                int id = int.Parse(Convert.ToString(row.Cells["resourceId"].Value));
                ResourceModify rm = new ResourceModify(id, companyId, Convert.ToString(row.Cells["name"].Value), Convert.ToString(row.Cells["description"].Value), int.Parse(Convert.ToString(row.Cells["cost"].Value)),  true);
                this.Hide();
                rm.ShowDialog();
                // refresh gridview table
                conn.Open();
                int pageNumber = 1;
                OleDbDataAdapter adapter = new OleDbDataAdapter("Select * from ( select a.*, rownum r__ from (Select * from resourcesMaterializedView where companyid = " + companyId + ") a where rownum < ("+ pageNumber * 10 + ")) where r__ >= (" + (pageNumber - 1) * 10 + ")" , conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView2.DataSource = dt;
                conn.Close();
                this.Show();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            conn.Open();
            int pageNumber = int.Parse(textBox3.Text);
            if (pageNumber < 1)
                pageNumber = 1;
            OleDbDataAdapter adapter = new OleDbDataAdapter("Select * from ( select a.*, rownum r__ from (Select * from resourcesMaterializedView where companyid = " + companyId + ") a where rownum < (" + pageNumber * 10 + ")) where r__ >= (" + (pageNumber - 1) * 10 + ")", conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView2.DataSource = dt;
            conn.Close();
            this.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            conn.Open();
            int pageNumber = int.Parse(textBox4.Text);
            if (pageNumber < 1)
                pageNumber = 1;
            OleDbDataAdapter adapter = new OleDbDataAdapter("Select * from ( select a.*, rownum r__ from (Select * from services where companyid = " + companyId + ") a where rownum < (" + pageNumber * 10 + ")) where r__ >= (" + (pageNumber - 1) * 10 + ")", conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
            this.Show();
        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            conn.Open();
            int pageNumber = int.Parse(textBox3.Text);
            if (pageNumber < 1)
                pageNumber = 1;
            OleDbDataAdapter adapter = new OleDbDataAdapter("Select * from ( select a.*, rownum r__ from (Select * from resourcesMaterializedView where companyid = " + companyId + ") a where rownum < (" + pageNumber * 10 + ")) where r__ >= (" + (pageNumber - 1) * 10 + ")", conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView2.DataSource = dt;
            conn.Close();
            this.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {

        }
    }
}
