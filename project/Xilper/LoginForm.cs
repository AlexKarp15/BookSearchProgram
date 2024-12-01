using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Xilper
{
    public partial class LoginForm : Form
    {
        private SqlConnection BooksForLearningConnection = null;
        public LoginForm()
        {
            InitializeComponent();
            InitializeConnection();
        }
        private void InitializeConnection()
        {
            BooksForLearningConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["BooksForLearning"].ConnectionString);
            BooksForLearningConnection.Open();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string enteredNickname = textBox1.Text;
            string enteredPassword = textBox2.Text;

            try
            {
                string query = "SELECT * FROM Аккаунт WHERE Нікнейм = @Nickname AND Пароль = @Password";
                using (SqlCommand cmd = new SqlCommand(query, BooksForLearningConnection))
                {
                    cmd.Parameters.AddWithValue("@Nickname", enteredNickname);
                    cmd.Parameters.AddWithValue("@Password", enteredPassword);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string userCode = reader["Код"].ToString();
                        string userNickname = reader["Нікнейм"].ToString();
                        string userEmail = reader["Почта"].ToString();
                        string userAccess = reader["Доступ"].ToString();
                        string userPassword = reader["Пароль"].ToString();
                        var mainForm = new mainForm(userCode, userNickname, userEmail, userPassword, userAccess);
                        mainForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        // Якщо дані не співпадають
                        label6.Text = "Введено невірний нікнейм або пароль!";
                        label6.ForeColor = Color.Red;
                        label6.Visible = true;
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during login: {ex.Message}");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var SignUp = new SignUp();
            SignUp.Show();
            this.Hide();
        }
    }
}
