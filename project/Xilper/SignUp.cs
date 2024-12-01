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
    public partial class SignUp : Form
    {
        private SqlConnection BooksForLearningConnection = null;
        public SignUp()
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
            string newNickname = textBox1.Text;
            string newEmail = textBox2.Text;
            string newPassword = textBox3.Text;
            string newPassword2 = textBox4.Text;
            int accessLevel = 0;

            if (string.IsNullOrWhiteSpace(newNickname) ||
                string.IsNullOrWhiteSpace(newPassword) ||
                string.IsNullOrWhiteSpace(newEmail) ||
                string.IsNullOrWhiteSpace(newPassword2))
            {
                label6.Text = "Заповніть усі поля!";
                label6.ForeColor = Color.Red;
                label6.Visible = true;
                return;
            }

            try
            {
                string checkQuery = "SELECT COUNT(*) FROM Аккаунт WHERE Почта = @Email";

                using (SqlCommand checkCmd = new SqlCommand(checkQuery, BooksForLearningConnection))
                {
                    checkCmd.Parameters.AddWithValue("@Email", newEmail);
                    int existingAccounts = (int)checkCmd.ExecuteScalar();

                    if (existingAccounts > 0)
                    {
                        label6.Text = "Акаунт з такою поштою вже існує!";
                        label6.ForeColor = Color.Red;
                        label6.Visible = true;
                        return;
                    }
                    if (newPassword != newPassword2)
                    {
                        label6.Text = "Введіть повторно ідентичний пароль!";
                        label6.ForeColor = Color.Red;
                        label6.Visible = true;
                        return;
                    }
                }
                string query = "INSERT INTO Аккаунт (Нікнейм, Пароль, Почта, Доступ) VALUES (@Nickname, @Password, @Email, @Access)";

                using (SqlCommand cmd = new SqlCommand(query, BooksForLearningConnection))
                {
                    cmd.Parameters.AddWithValue("@Nickname", newNickname);
                    cmd.Parameters.AddWithValue("@Password", newPassword);
                    cmd.Parameters.AddWithValue("@Email", newEmail);
                    cmd.Parameters.AddWithValue("@Access", accessLevel);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        ClearInputFields();
                        var LoginForm = new LoginForm();
                        LoginForm.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Не вдалося додати акаунт. Спробуйте ще раз.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час додавання: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearInputFields()
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;
        }

        private void SignUp_FormClosed(object sender, FormClosedEventArgs e)
        {
            var LoginForm = new LoginForm();
            LoginForm.Show();
        }
    }
}
