using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private string connectionString = "Server=MSI\\SQLEXPRESS;Database=DB_QLVanban;Integrated Security=True;";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT * FROM Vanban";
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                    DataSet dataSet = new DataSet();

                    connection.Open();
                    dataAdapter.Fill(dataSet, "Vanban");

                    dataGridView.DataSource = dataSet.Tables["Vanban"];
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void ExecuteQuery(string query, SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    command.ExecuteNonQuery();
                    MessageBox.Show("Thực hiện truy vấn thành công.");
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private string GenerateRandomID()
        {
            DateTime now = DateTime.Now;
            string randomID = now.ToString("yyMMddHHmmssff");
            return randomID;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string newID = GenerateRandomID();
                txtID.Text = newID;

                string insertQuery = "INSERT INTO Vanban (id, Noidung) VALUES (@id, @noidung)";
                SqlParameter[] parameters = {
                        new SqlParameter("@id", SqlDbType.BigInt) { Value = newID },
                        new SqlParameter("@noidung", SqlDbType.NVarChar) { Value = txtContent.Text }
                    };

                ExecuteQuery(insertQuery, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

    private void btnDelete_Click(object sender, EventArgs e)
        {
            string deleteQuery = "DELETE FROM Vanban WHERE id = @id";
            SqlParameter[] parameters = {
                new SqlParameter("@id", SqlDbType.BigInt) { Value = long.Parse(txtID.Text) }
            };
            ExecuteQuery(deleteQuery, parameters);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string updateQuery = "UPDATE Vanban SET Noidung = @noidung WHERE id = @id";
            SqlParameter[] parameters = {
                new SqlParameter("@id", SqlDbType.BigInt) { Value = long.Parse(txtID.Text) },
                new SqlParameter("@noidung", SqlDbType.NVarChar) { Value = txtContent.Text }
            };
            ExecuteQuery(updateQuery, parameters);
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView.Rows[e.RowIndex];

                txtID.Text = row.Cells["id"].Value.ToString();
                txtContent.Text = row.Cells["Noidung"].Value.ToString();
            }
        }
    }
}
