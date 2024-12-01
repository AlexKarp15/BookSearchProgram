using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Xilper
{
    public partial class mainForm : Form
    {
        private SqlConnection BooksForLearningConnection = null;
        public string Code { get; }
        public string Nickname { get; }
        public string Email { get; }
        public string Password { get; }
        public string Access { get; }
        public mainForm(string code, string nickname, string email, string password, string access)
        {
            InitializeComponent();
            InitializeConnection();
            menuButtons(button2);
            menuButtons(button3);
            menuButtons(button4);
            menuButtons(button48);
            LoadData7();
            LoadLanguages();
            Nickname = nickname;
            Email = email;
            Access = access;
            Code = code;
            if (access == "False")
            {
                button1.Visible = false;
            }
            else
            {
                menuButtons(button1);
            }
            LoadLevels();
        }
        private void InitializeConnection()
        {
            BooksForLearningConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["BooksForLearning"].ConnectionString);
            BooksForLearningConnection.Open();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (BooksForLearningConnection.State != ConnectionState.Open)
                BooksForLearningConnection.Open();
            panel1.Visible = true;
            panel2.Visible = false;
            panel34.Visible = false;
            panel36.Visible = false;
            LoadData1();
            LoadData2();
            LoadData3();
            LoadData4();
            LoadData5();
            LoadData6();
            if (BooksForLearningConnection.State == ConnectionState.Open)
                BooksForLearningConnection.Close();
        }
        private void main_Load(object sender, EventArgs e)
        {
            BooksForLearningConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["BooksForLearning"].ConnectionString);
            BooksForLearningConnection.Open();
        }
        //скугление кнопок
        private void menuButtons(System.Windows.Forms.Button button)
        {
            // Устанавливаем светло-жёлтый цвет для кнопки
            button.BackColor = Color.LightYellow;
            button.FlatAppearance.BorderColor = Color.Goldenrod;
            button.FlatAppearance.BorderSize = 3;
            button.ForeColor = Color.Black;
            button.Font = new Font(button1.Font.FontFamily, 12);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 3;
            button.FlatAppearance.BorderColor = Color.Goldenrod;
            button.FlatAppearance.MouseOverBackColor = Color.Khaki;
            button.FlatAppearance.MouseDownBackColor = Color.Gold;
            RoundButtonCorners(button, 20);
        }

        private void RoundButtonCorners(System.Windows.Forms.Button button, int borderRadius)
        {
            button.Paint += (sender, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                using (GraphicsPath path = new GraphicsPath())
                {
                    int adjustedRadius = Math.Min(borderRadius, Math.Min(button.Width, button.Height) / 2);
                    int diameter = adjustedRadius * 2;
                    path.AddArc(0, 0, diameter, diameter, 180, 90);
                    path.AddArc(button.Width - diameter - 1, 0, diameter, diameter, 270, 90);
                    path.AddArc(button.Width - diameter - 1, button.Height - diameter - 1, diameter, diameter, 0, 90);
                    path.AddArc(0, button.Height - diameter - 1, diameter, diameter, 90, 90);
                    path.CloseFigure();
                    button.Region = new Region(path);
                    using (Brush brush = new SolidBrush(button.BackColor))
                    {
                        g.FillPath(brush, path);
                    }
                    RectangleF borderRect = new RectangleF(2, 2, button.Width - 4, button.Height - 4); // Отступы увеличены
                    using (Pen pen = new Pen(button.FlatAppearance.BorderColor == Color.Empty ? Color.Black : button.FlatAppearance.BorderColor,
                                             4))
                    {
                        g.DrawPath(pen, path);
                    }
                    TextRenderer.DrawText(
                        g,
                        button.Text,
                        new Font(button.Font.FontFamily, 14),
                        new Rectangle(0, 0, button.Width, button.Height),
                        Color.Black,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            };
        }
        /*
        Завантаження таблиць адміна Книга
        */
        private void LoadData1()
        {
            try
            {
                string query = "SELECT Код, Назва, Фото, Код_Рівень, Код_Автор, Код_Мова, Посилання FROM Книга";
                using (SqlDataAdapter da = new SqlDataAdapter(query, BooksForLearningConnection))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["Фото"] != DBNull.Value)
                        {
                            byte[] imageBytes = (byte[])row["Фото"];
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                Image image = Image.FromStream(ms);
                                Image resizedImage = ResizeImage(image, 150, 150);
                                using (MemoryStream resizedStream = new MemoryStream())
                                {
                                    resizedImage.Save(resizedStream, System.Drawing.Imaging.ImageFormat.Png);
                                    row["Фото"] = resizedStream.ToArray();
                                }
                            }
                        }
                    }
                    dataGridView1.DataSource = dt;
                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        if (column.Name == "Фото")
                        {
                            column.Width = 150;
                        }
                    }
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        row.Height = 150;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }
        private Image ResizeImage(Image img, int width, int height)
        {
            Bitmap b = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.DrawImage(img, 0, 0, width, height);
            }
            return b;
        }
        /*
         Завантаження таблиць адміна Аккаунт
         */
        private void LoadData2()
        {
            try
            {
                // Instead of clearing rows, just bind a fresh DataTable
                string query = "SELECT * FROM Аккаунт";
                using (SqlDataAdapter da = new SqlDataAdapter(query, BooksForLearningConnection))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView2.DataSource = dt; // This will refresh the data in DataGridView
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }
        /*
        Завантаження таблиць адміна Автор
        */
        private void LoadData3()
        {
            try
            {
                string query = "SELECT Код, [Ім'я] FROM Автор;";
                using (SqlDataAdapter da = new SqlDataAdapter(query, BooksForLearningConnection))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView3.DataSource = dt; // Bind data directly to the DataGridView
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data for Authors: {ex.Message}");
            }
        }
        /*
        Завантаження таблиць адміна Мова
        */
        private void LoadData4()
        {
            try
            {
                string query = "SELECT Код, Мова FROM Мова;";
                using (SqlDataAdapter da = new SqlDataAdapter(query, BooksForLearningConnection))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView4.DataSource = dt; // Bind data directly to the DataGridView
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data for Languages: {ex.Message}");
            }
        }
        /*
        Завантаження таблиць адміна Рівень
        */
        private void LoadData5()
        {
            try
            {
                string query = "SELECT Код, Рівень FROM Рівень;";
                using (SqlDataAdapter da = new SqlDataAdapter(query, BooksForLearningConnection))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView5.DataSource = dt; // Bind data directly to the DataGridView
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data for Levels: {ex.Message}");
            }
        }
        /*
        Завантаження таблиць адміна Історія
        */
        private void LoadData6()
        {
            try
            {
                string query = "SELECT Код, Код_Аккаунт, Код_Книга FROM Історія;";
                using (SqlDataAdapter da = new SqlDataAdapter(query, BooksForLearningConnection))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView6.DataSource = dt; // Bind data directly to the DataGridView
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data for History: {ex.Message}");
            }
        }

        /* Дії таблиці акаунта для адміна  */
        /*  Пошук аккаунта  */
        public void SearchAccounts(string keyword, DataGridView dataGridView)
        {
            try
            {
                dataGridView.Columns.Clear();
                string query = "SELECT * FROM Аккаунт WHERE Нікнейм LIKE @keyword OR Почта LIKE @keyword";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, BooksForLearningConnection))
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching accounts: {ex.Message}");
            }
        }
        private void button36_Click(object sender, EventArgs e)
        {
            SearchAccounts(textBox8.Text, dataGridView2); ;
        }
        private void button37_Click(object sender, EventArgs e)
        {
            LoadData2();
            textBox8.Clear();
            panel4.Visible = true;
            panel12.Visible = false;
        }
        private void button9_Click(object sender, EventArgs e)
        {
            panel12.Visible = true;
            panel4.Visible = false;
        }
        /* Додавання аккаунта */
        private void AddAccount(string nickname, string email, string password, bool access)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Аккаунт (Нікнейм, Почта, Пароль, Доступ) VALUES (@nickname, @email, @password, @access)",
                    BooksForLearningConnection))
                {
                    cmd.Parameters.AddWithValue("@nickname", nickname);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@access", access);

                    BooksForLearningConnection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Account added successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding account: {ex.Message}");
            }
            finally
            {
                BooksForLearningConnection.Close();
                LoadData2();
            }
        }
        private void button34_Click(object sender, EventArgs e)
        {
            AddAccount(textBox5.Text, textBox6.Text, textBox7.Text, checkBox1.Checked);
        }
        private void button35_Click(object sender, EventArgs e)
        {
            LoadData2();
            textBox5.Clear();
            textBox6.Clear();
            checkBox1.Checked = false;
            textBox7.Clear();
            panel11.Visible = false;
            panel4.Visible = true;
        }
        private void button10_Click(object sender, EventArgs e)
        {
            panel11.Visible = true;
            panel4.Visible = false;
        }

        /* Редагування аккаунта */
        private void UpdateAccount(int id, string nickname, string email, string password, bool access)
        {
            try
            {
                // Логування значень
                Console.WriteLine($"Updating account: id = {id}, nickname = {nickname}, email = {email}, password = {password}, access = {access}");

                using (SqlCommand cmd = new SqlCommand(
                    "UPDATE Аккаунт SET Нікнейм = @nickname, Почта = @email, Пароль = @password, Доступ = @access WHERE Код = @id",
                    BooksForLearningConnection))
                {
                    if (BooksForLearningConnection.State != ConnectionState.Open)
                    {
                        BooksForLearningConnection.Open();
                    }

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@nickname", nickname);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@access", access);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Account updated successfully!");
                    }
                    else
                    {
                        MessageBox.Show("No account found to update.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating account: {ex.Message}\n{ex.StackTrace}");
            }
            finally
            {
                if (BooksForLearningConnection.State == ConnectionState.Open)
                {
                    BooksForLearningConnection.Close();
                }

                LoadData2();
            }
        }
        private void button38_Click(object sender, EventArgs e)
        {
            // Перевірка, чи вибрано рядок
            if (dataGridView2.SelectedRows.Count > 0)
            {
                // Отримуємо перший вибраний рядок
                DataGridViewRow selectedRow = dataGridView2.SelectedRows[0];

                // Отримуємо ID акаунта з клітинки "Код"
                int id = Convert.ToInt32(selectedRow.Cells["Код"].Value);

                // Оновлюємо дані акаунта
                UpdateAccount(id, textBox10.Text, textBox11.Text, textBox12.Text, checkBox2.Checked);
            }
            else
            {
                MessageBox.Show("Please select an account to update.");
            }
        }

        private void button39_Click(object sender, EventArgs e)
        {
            LoadData2();
            textBox10.Clear();
            textBox11.Clear();
            checkBox2.Checked = false;
            textBox12.Clear();
            panel13.Visible = false;
            panel4.Visible = true;
        }
        private void button11_Click(object sender, EventArgs e)
        {
            panel13.Visible = true;
            panel4.Visible = false;
        }
        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView2.SelectedRows[0];
                textBox10.Text = selectedRow.Cells["Нікнейм"].Value.ToString();
                textBox11.Text = selectedRow.Cells["Почта"].Value.ToString();
                textBox12.Text = selectedRow.Cells["Пароль"].Value.ToString();
                checkBox2.Checked = Convert.ToBoolean(selectedRow.Cells["Доступ"].Value);
            }
        }

        /* Видалення аккаунта */
        private void DeleteAccount(int id)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Аккаунт WHERE Код = @id",
                    BooksForLearningConnection))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    BooksForLearningConnection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Account deleted successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting account: {ex.Message}");
            }
            finally
            {
                BooksForLearningConnection.Close();
                LoadData2();
            }
        }
        private void button40_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView2.SelectedRows[0];
                int id = Convert.ToInt32(selectedRow.Cells["Код"].Value);
                DeleteAccount(id);
            }
            else
            {
                MessageBox.Show("Please select an account to delete.");
            }
        }
        private void button41_Click(object sender, EventArgs e)
        {
            LoadData2();
            panel14.Visible = false;
            panel4.Visible = true;
        }
        private void button12_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
            panel14.Visible = true;
        }
        /* Дії таблиці Історія для адміна  */
        /*  Пошук історії  */
        private void button43_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT Код, Код_Аккаунт, Код_Книга FROM Історія WHERE Код_Книга LIKE @searchTerm;";
                using (SqlDataAdapter da = new SqlDataAdapter(query, BooksForLearningConnection))
                {
                    da.SelectCommand.Parameters.AddWithValue("@searchTerm", "%" + textBox9.Text + "%");  // Use parameterized queries to prevent SQL injection
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView6.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching: {ex.Message}");
            }
        }
        private void button42_Click(object sender, EventArgs e)
        {
            LoadData6();
            panel29.Visible = false;
            panel8.Visible = true;
        }
        private void button25_Click(object sender, EventArgs e)
        {
            panel29.Visible = true;
            panel8.Visible = false;
        }
        /* Додавання історії */
        private void AddAccount(int accountCode, int bookCode)
        {
            try
            {
                string query = "INSERT INTO Історія (Код_Аккаунт, Код_Книга) VALUES (@accountCode, @bookCode);";
                using (SqlCommand cmd = new SqlCommand(query, BooksForLearningConnection))
                {
                    cmd.Parameters.AddWithValue("@accountCode", accountCode);
                    cmd.Parameters.AddWithValue("@bookCode", bookCode);
                    BooksForLearningConnection.Open();
                    cmd.ExecuteNonQuery();
                    BooksForLearningConnection.Close();
                    MessageBox.Show("History added successfully.");
                    LoadData6(); // Reload the data grid
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding account: {ex.Message}");
            }
        }
        private void button26_Click(object sender, EventArgs e)
        {
            panel30.Visible = true;
            panel8.Visible = false;
        }
        private void button44_Click(object sender, EventArgs e)
        {
            try
            {
                int accountCode = int.Parse(textBox13.Text); // Поле для введення "Код_Аккаунт"
                int bookCode = int.Parse(textBox14.Text);       // Поле для введення "Код_Книга"
                AddAccount(accountCode, bookCode);
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter valid numeric values for account and book codes.");
            }
        }

        private void button45_Click(object sender, EventArgs e)
        {
            panel30.Visible = false;
            panel8.Visible = true;
            LoadData6();
        }
        /* Редагування історії */
        private void UpdateHistory(int id, int accountCode, int bookCode)
        {
            try
            {
                string query = "UPDATE Історія SET Код_Аккаунт = @accountCode, Код_Книга = @bookCode WHERE Код = @id;";
                using (SqlCommand cmd = new SqlCommand(query, BooksForLearningConnection))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@accountCode", accountCode);
                    cmd.Parameters.AddWithValue("@bookCode", bookCode);

                    BooksForLearningConnection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("History entry updated successfully.");
                    }
                    else
                    {
                        MessageBox.Show("No history entry found to update.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating history entry: {ex.Message}");
            }
            finally
            {
                BooksForLearningConnection.Close();
                LoadData6();
            }
        }
        private void button46_Click(object sender, EventArgs e)
        {
            if (dataGridView6.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView6.SelectedRows[0];
                int id = Convert.ToInt32(selectedRow.Cells["Код"].Value);

                try
                {
                    int accountCode = int.Parse(textBox16.Text);
                    int bookCode = int.Parse(textBox15.Text);
                    UpdateHistory(id, accountCode, bookCode);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Please enter valid numeric values for account and book codes.");
                }
            }
            else
            {
                MessageBox.Show("Please select a history entry to update.");
            }
        }
        private void button47_Click(object sender, EventArgs e)
        {
            panel31.Visible = false;
            panel8.Visible = true;
            LoadData6();
        }
        private void button27_Click(object sender, EventArgs e)
        {
            panel8.Visible = false;
            panel31.Visible = true;
            LoadData6();
        }
        private void dataGridView6_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView6.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView6.SelectedRows[0];
                textBox15.Text = selectedRow.Cells["Код_Книга"].Value.ToString();
                textBox16.Text = selectedRow.Cells["Код_Аккаунт"].Value.ToString();
            }
        }
        /* Видалення історії */
        private void DeleteHistory(int id)
        {
            try
            {
                string query = "DELETE FROM Історія WHERE Код = @id;";
                using (SqlCommand cmd = new SqlCommand(query, BooksForLearningConnection))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    BooksForLearningConnection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("History entry deleted successfully.");
                    }
                    else
                    {
                        MessageBox.Show("No history entry found to delete.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting history entry: {ex.Message}");
            }
            finally
            {
                BooksForLearningConnection.Close();
                LoadData6(); // Reload the data grid
            }
        }
        private void button49_Click(object sender, EventArgs e)
        {
            if (dataGridView6.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView6.SelectedRows[0];
                int id = Convert.ToInt32(selectedRow.Cells["Код"].Value);
                DeleteHistory(id);
            }
            else
            {
                MessageBox.Show("Please select a history entry to delete.");
            }
        }
        private void button50_Click(object sender, EventArgs e)
        {
            panel32.Visible = false;
            panel8.Visible = true;
            LoadData6();
        }
        private void button28_Click(object sender, EventArgs e)
        {
            panel8.Visible = false;
            panel32.Visible = true;
            LoadData6();
        }
        /* Дії таблиці Рівень для адміна  */
        /*  Пошук рівня  */
        private void button21_Click(object sender, EventArgs e)
        {
            panel7.Visible = false;
            panel25.Visible = true;
            textBox17.Text = "";
            LoadData5();
        }
        private void button52_Click(object sender, EventArgs e)
        {
            panel25.Visible = false;
            panel7.Visible = true;
            textBox17.Text = "";
            LoadData5();
        }
        private void button51_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT Код, Рівень FROM Рівень WHERE Рівень LIKE @searchTerm;";
                using (SqlDataAdapter da = new SqlDataAdapter(query, BooksForLearningConnection))
                {
                    da.SelectCommand.Parameters.AddWithValue("@searchTerm", "%" + textBox17.Text + "%");  // Use parameterized queries to prevent SQL injection
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView5.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching: {ex.Message}");
            }
        }
        /* Додавання рівня */
        private void AddLevel(string levelName)
        {
            try
            {
                string query = "INSERT INTO Рівень (Рівень) VALUES (@levelName);";
                using (SqlCommand cmd = new SqlCommand(query, BooksForLearningConnection))
                {
                    cmd.Parameters.AddWithValue("@levelName", levelName);
                    BooksForLearningConnection.Open();
                    cmd.ExecuteNonQuery();
                    BooksForLearningConnection.Close();
                    MessageBox.Show("Level added successfully.");
                    LoadData5();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding level: {ex.Message}");
            }
        }
        private void button22_Click(object sender, EventArgs e)
        {
            panel7.Visible = false;
            panel26.Visible = true;
            textBox18.Text = "";
            LoadData5();
        }
        private void button54_Click(object sender, EventArgs e)
        {
            panel26.Visible = false;
            panel7.Visible = true;
            textBox18.Text = "";
            LoadData5();
        }
        private void button53_Click(object sender, EventArgs e)
        {
            try
            {
                string levelName = textBox18.Text;
                if (!string.IsNullOrEmpty(levelName))
                {
                    AddLevel(levelName);
                }
                else
                {
                    MessageBox.Show("Please enter a valid level name.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        /* Редагування рівня */
        private void button23_Click(object sender, EventArgs e)
        {
            panel7.Visible = false;
            panel27.Visible = true;
            LoadData5();
        }
        private void button56_Click(object sender, EventArgs e)
        {
            panel27.Visible = false;
            panel7.Visible = true;
            LoadData5();
        }
        private void UpdateLevel(int id, string levelName)
        {
            try
            {
                string query = "UPDATE Рівень SET Рівень = @levelName WHERE Код = @id;";
                using (SqlCommand cmd = new SqlCommand(query, BooksForLearningConnection))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@levelName", levelName);
                    BooksForLearningConnection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Level updated successfully.");
                    }
                    else
                    {
                        MessageBox.Show("No level found to update.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating level: {ex.Message}");
            }
            finally
            {
                BooksForLearningConnection.Close();
                LoadData5();
            }
        }
        private void button55_Click(object sender, EventArgs e)
        {
            if (dataGridView5.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView5.SelectedRows[0];
                int id = Convert.ToInt32(selectedRow.Cells["Код"].Value);

                try
                {
                    string levelName = textBox19.Text;
                    if (!string.IsNullOrEmpty(levelName))
                    {
                        UpdateLevel(id, levelName);
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid level name.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Please select a level to edit.");
            }
        }
        private void dataGridView5_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView5.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView5.SelectedRows[0];
                textBox19.Text = selectedRow.Cells["Рівень"].Value.ToString();
            }
        }
        /* Видалення рівня */
        private void button24_Click(object sender, EventArgs e)
        {
            panel7.Visible = false;
            panel28.Visible = true;
            LoadData5();
        }
        private void button58_Click(object sender, EventArgs e)
        {
            panel28.Visible = false;
            panel7.Visible = true;
            LoadData5();
        }
        private void DeleteLevel(int id)
        {
            try
            {
                string query = "DELETE FROM Рівень WHERE Код = @id;";
                using (SqlCommand cmd = new SqlCommand(query, BooksForLearningConnection))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    BooksForLearningConnection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Level deleted successfully.");
                    }
                    else
                    {
                        MessageBox.Show("No level found to delete.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting level: {ex.Message}");
            }
            finally
            {
                BooksForLearningConnection.Close();
                LoadData5();
            }
        }
        private void button57_Click(object sender, EventArgs e)
        {
            if (dataGridView5.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView5.SelectedRows[0];
                int id = Convert.ToInt32(selectedRow.Cells["Код"].Value);
                DeleteLevel(id);
            }
            else
            {
                MessageBox.Show("Please select a level to delete.");
            }
        }
        /* Дії таблиці Мова для адміна  */
        /*  Пошук мова  */
        private void button17_Click(object sender, EventArgs e)
        {
            panel6.Visible = false;
            panel21.Visible = true;
            LoadData4();
            textBox20.Text = "";
        }
        private void button60_Click(object sender, EventArgs e)
        {
            panel21.Visible = false;
            panel6.Visible = true;
            LoadData4();
            textBox20.Text = "";
        }
        private void button59_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT Код, Мова FROM Мова WHERE Мова LIKE @searchTerm;";
                using (SqlDataAdapter da = new SqlDataAdapter(query, BooksForLearningConnection))
                {
                    da.SelectCommand.Parameters.AddWithValue("@searchTerm", "%" + textBox20.Text + "%");
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView4.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching: {ex.Message}");
            }
        }
        /* Додавання мова */
        private void AddLanguage(string languageName)
        {
            try
            {
                string query = "INSERT INTO Мова (Мова) VALUES (@languageName);";
                using (SqlCommand cmd = new SqlCommand(query, BooksForLearningConnection))
                {
                    cmd.Parameters.AddWithValue("@languageName", languageName);
                    BooksForLearningConnection.Open();
                    cmd.ExecuteNonQuery();
                    BooksForLearningConnection.Close();
                    MessageBox.Show("Language added successfully.");
                    LoadData4();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding language: {ex.Message}");
            }
        }
        private void button18_Click(object sender, EventArgs e)
        {
            panel6.Visible = false;
            panel22.Visible = true;
            LoadData4();
            textBox21.Text = "";
        }
        private void button61_Click(object sender, EventArgs e)
        {
            try
            {
                string languageName = textBox21.Text;
                if (!string.IsNullOrEmpty(languageName))
                {
                    AddLanguage(languageName);
                }
                else
                {
                    MessageBox.Show("Please enter a valid language name.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        private void button62_Click(object sender, EventArgs e)
        {
            panel22.Visible = false;
            panel6.Visible = true;
            LoadData4();
            textBox21.Text = "";
        }
        /* Редагування мова */
        private void button19_Click(object sender, EventArgs e)
        {
            panel6.Visible = false;
            panel23.Visible = true;
            LoadData4();
        }
        private void UpdateLanguage(int id, string languageName)
        {
            try
            {
                string query = "UPDATE Мова SET Мова = @languageName WHERE Код = @id;";
                using (SqlCommand cmd = new SqlCommand(query, BooksForLearningConnection))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@languageName", languageName);
                    BooksForLearningConnection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Language updated successfully.");
                    }
                    else
                    {
                        MessageBox.Show("No language found to update.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating language: {ex.Message}");
            }
            finally
            {
                BooksForLearningConnection.Close();
                LoadData4();
            }
        }
        private void button63_Click(object sender, EventArgs e)
        {
            if (dataGridView4.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView4.SelectedRows[0];
                int id = Convert.ToInt32(selectedRow.Cells["Код"].Value);

                try
                {
                    string languageName = textBox22.Text;
                    if (!string.IsNullOrEmpty(languageName))
                    {
                        UpdateLanguage(id, languageName);
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid language name.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Please select a language to edit.");
            }
        }
        private void button64_Click(object sender, EventArgs e)
        {
            panel23.Visible = false;
            panel6.Visible = true;
            LoadData4();
        }
        private void dataGridView4_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView4.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView4.SelectedRows[0];
                textBox22.Text = selectedRow.Cells["Мова"].Value.ToString();
            }
        }
        /* Видалення мова */
        private void DeleteLanguage(int id)
        {
            try
            {
                string query = "DELETE FROM Мова WHERE Код = @id;";
                using (SqlCommand cmd = new SqlCommand(query, BooksForLearningConnection))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    BooksForLearningConnection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Language deleted successfully.");
                    }
                    else
                    {
                        MessageBox.Show("No language found to delete.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting language: {ex.Message}");
            }
            finally
            {
                BooksForLearningConnection.Close();
                LoadData4();
            }
        }
        private void button20_Click(object sender, EventArgs e)
        {
            panel6.Visible = false;
            panel24.Visible = true;
            LoadData4();
        }
        private void button66_Click(object sender, EventArgs e)
        {
            panel24.Visible = false;
            panel6.Visible = true;
            LoadData4();
        }
        private void button65_Click(object sender, EventArgs e)
        {
            if (dataGridView4.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView4.SelectedRows[0];
                int id = Convert.ToInt32(selectedRow.Cells["Код"].Value);
                DeleteLanguage(id);
            }
            else
            {
                MessageBox.Show("Please select a language to delete.");
            }
        }
        /* Дії таблиці Автор для адміна  */
        /*  Пошук автор  */
        private void button67_Click(object sender, EventArgs e)
        {
            panel5.Visible = false;
            panel20.Visible = true;
            LoadData3();
            textBox23.Text = "";
        }
        private void button72_Click(object sender, EventArgs e)
        {
            panel20.Visible = false;
            panel5.Visible = true;
            LoadData3();
            textBox23.Text = "";
        }
        private void button71_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT Код, [Ім'я] FROM Автор WHERE [Ім'я] LIKE @searchTerm;";
                using (SqlDataAdapter da = new SqlDataAdapter(query, BooksForLearningConnection))
                {
                    da.SelectCommand.Parameters.AddWithValue("@searchTerm", "%" + textBox23.Text + "%");
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView3.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching: {ex.Message}");
            }
        }
        /* Додавання автор */
        private void AddAuthor(string authorName)
        {
            try
            {
                string query = "INSERT INTO Автор ([Ім'я]) VALUES (@authorName);";
                using (SqlCommand cmd = new SqlCommand(query, BooksForLearningConnection))
                {
                    cmd.Parameters.AddWithValue("@authorName", authorName);
                    BooksForLearningConnection.Open();
                    cmd.ExecuteNonQuery();
                    BooksForLearningConnection.Close();
                    MessageBox.Show("Author added successfully.");
                    LoadData3();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding author: {ex.Message}");
            }
        }
        private void button68_Click(object sender, EventArgs e)
        {
            panel5.Visible = false;
            panel17.Visible = true;
            LoadData3();
            textBox24.Text = "";
        }
        private void button74_Click(object sender, EventArgs e)
        {
            panel17.Visible = false;
            panel5.Visible = true;
            LoadData3();
            textBox24.Text = "";
        }
        private void button73_Click(object sender, EventArgs e)
        {
            try
            {
                string authorName = textBox24.Text;
                if (!string.IsNullOrEmpty(authorName))
                {
                    AddAuthor(authorName);
                }
                else
                {
                    MessageBox.Show("Please enter a valid author name.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        /* Редагування автор */
        private void button69_Click(object sender, EventArgs e)
        {
            panel5.Visible = false;
            panel18.Visible = true;
            LoadData3();
        }
        private void button76_Click(object sender, EventArgs e)
        {
            panel18.Visible = false;
            panel5.Visible = true;
            LoadData3();
        }
        private void UpdateAuthor(int id, string authorName)
        {
            try
            {
                string query = "UPDATE Автор SET [Ім'я] = @authorName WHERE Код = @id;";
                using (SqlCommand cmd = new SqlCommand(query, BooksForLearningConnection))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@authorName", authorName);
                    BooksForLearningConnection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Author updated successfully.");
                    }
                    else
                    {
                        MessageBox.Show("No author found to update.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating author: {ex.Message}");
            }
            finally
            {
                BooksForLearningConnection.Close();
                LoadData3();
            }
        }
        private void button75_Click(object sender, EventArgs e)
        {
            if (dataGridView3.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView3.SelectedRows[0];
                int id = Convert.ToInt32(selectedRow.Cells["Код"].Value);

                try
                {
                    string authorName = textBox25.Text;
                    if (!string.IsNullOrEmpty(authorName))
                    {
                        UpdateAuthor(id, authorName);
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid author name.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Please select an author to edit.");
            }
        }
        private void dataGridView3_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView3.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView3.SelectedRows[0];
                textBox25.Text = selectedRow.Cells["Ім'я"].Value.ToString();
            }
        }
        /* Видалення автор */
        private void DeleteAuthor(int id)
        {
            try
            {
                string query = "DELETE FROM Автор WHERE Код = @id;";
                using (SqlCommand cmd = new SqlCommand(query, BooksForLearningConnection))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    BooksForLearningConnection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Author deleted successfully.");
                    }
                    else
                    {
                        MessageBox.Show("No author found to delete.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting author: {ex.Message}");
            }
            finally
            {
                BooksForLearningConnection.Close();
                LoadData3();
            }
        }
        private void button70_Click(object sender, EventArgs e)
        {
            panel5.Visible = false;
            panel19.Visible = true;
            LoadData3();
        }
        private void button78_Click(object sender, EventArgs e)
        {
            panel19.Visible = false;
            panel5.Visible = true;
            LoadData3();
        }
        private void button77_Click(object sender, EventArgs e)
        {
            if (dataGridView3.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView3.SelectedRows[0];
                int id = Convert.ToInt32(selectedRow.Cells["Код"].Value);
                DeleteAuthor(id);
            }
            else
            {
                MessageBox.Show("Please select an author to delete.");
            }
        }
        /* Дії таблиці Книга для адміна  */
        /*  Пошук книга  */
        private void button5_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            panel9.Visible = true;
            textBox2.Text = "";
            textBox26.Text = "";
            textBox27.Text = "";
            textBox28.Text = "";
            LoadData1();
        }

        private void button30_Click(object sender, EventArgs e)
        {
            panel9.Visible = false;
            panel3.Visible = true;
            LoadData1();
        }

        private void button29_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT Код, Назва, Фото, Код_Рівень, Код_Автор, Код_Мова, Посилання FROM Книга WHERE Назва LIKE @searchTerm1 AND Код_Рівень LIKE @searchTerm2 AND Код_Автор LIKE @searchTerm3 AND Код_Мова LIKE @searchTerm4;";
                using (SqlDataAdapter da = new SqlDataAdapter(query, BooksForLearningConnection))
                {
                    da.SelectCommand.Parameters.AddWithValue("@searchTerm1", "%" + textBox2.Text + "%");
                    da.SelectCommand.Parameters.AddWithValue("@searchTerm2", "%" + textBox26.Text + "%");
                    da.SelectCommand.Parameters.AddWithValue("@searchTerm3", "%" + textBox27.Text + "%");
                    da.SelectCommand.Parameters.AddWithValue("@searchTerm4", "%" + textBox28.Text + "%");
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["Фото"] != DBNull.Value)
                        {
                            byte[] imageBytes = (byte[])row["Фото"];
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                Image image = Image.FromStream(ms);
                                Image resizedImage = ResizeImage(image, 150, 150);
                                using (MemoryStream resizedStream = new MemoryStream())
                                {
                                    resizedImage.Save(resizedStream, System.Drawing.Imaging.ImageFormat.Png);
                                    row["Фото"] = resizedStream.ToArray();
                                }
                            }
                        }
                    }
                    dataGridView1.DataSource = dt;
                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        if (column.Name == "Фото")
                        {
                            column.Width = 150;
                        }
                    }
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        row.Height = 150;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching books: {ex.Message}");
            }
        }
        /* Додавання книга */
        private void button6_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            panel10.Visible = true;
            textBox3.Text = "";
            textBox4.Text = "";
            textBox29.Text = "";
            textBox30.Text = "";
            textBox31.Text = "";
            LoadData1();
        }
        private void button32_Click(object sender, EventArgs e)
        {
            panel10.Visible = false;
            panel3.Visible = true;
            LoadData1();
        }
        private void AddBook(string name, int levelCode, int authorCode, int languageCode, string link, byte[] photo)
        {
            try
            {
                string query = "INSERT INTO Книга (Назва, Фото, Код_Рівень, Код_Автор, Код_Мова, Посилання) VALUES (@name, @photo, @levelCode, @authorCode, @languageCode, @link);";
                using (SqlCommand cmd = new SqlCommand(query, BooksForLearningConnection))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.Add("@photo", SqlDbType.VarBinary).Value = photo ?? (object)DBNull.Value;
                    cmd.Parameters.AddWithValue("@levelCode", levelCode);
                    cmd.Parameters.AddWithValue("@authorCode", authorCode);
                    cmd.Parameters.AddWithValue("@languageCode", languageCode);
                    cmd.Parameters.AddWithValue("@link", link);

                    BooksForLearningConnection.Open();
                    cmd.ExecuteNonQuery();
                    BooksForLearningConnection.Close();

                    MessageBox.Show("Book added successfully.");
                    LoadData1();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding book: {ex.Message}");
            }
        }
        private void button31_Click(object sender, EventArgs e)
        {
            try
            {
                string name = textBox4.Text;
                int levelCode = int.Parse(textBox29.Text);
                int authorCode = int.Parse(textBox30.Text);
                int languageCode = int.Parse(textBox31.Text);
                string link = textBox3.Text;

                byte[] photo = null;
                if (!string.IsNullOrEmpty(pictureBox2.ImageLocation))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Image img = Image.FromFile(pictureBox2.ImageLocation);
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        photo = ms.ToArray();
                    }
                }

                AddBook(name, levelCode, authorCode, languageCode, link, photo);
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter valid numeric values for level, author, and language codes.");
            }
        }
        private void button33_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.ImageLocation = openFileDialog.FileName;
            }
        }
        /* Редагування книга */
        private void UpdateBook(int id, string name, int levelCode, int authorCode, int languageCode, string link, byte[] photo)
        {
            try
            {
                string query = "UPDATE Книга SET Назва = @name, Фото = @photo, Код_Рівень = @levelCode, Код_Автор = @authorCode, Код_Мова = @languageCode, Посилання = @link WHERE Код = @id;";
                using (SqlCommand cmd = new SqlCommand(query, BooksForLearningConnection))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.Add("@photo", SqlDbType.VarBinary).Value = photo ?? (object)DBNull.Value; // Якщо фото = null, передається DBNull.Value
                    cmd.Parameters.AddWithValue("@levelCode", levelCode);
                    cmd.Parameters.AddWithValue("@authorCode", authorCode);
                    cmd.Parameters.AddWithValue("@languageCode", languageCode);
                    cmd.Parameters.AddWithValue("@link", link);

                    BooksForLearningConnection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Book updated successfully.");
                    }
                    else
                    {
                        MessageBox.Show("No book found to update.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating book: {ex.Message}");
            }
            finally
            {
                BooksForLearningConnection.Close();
                LoadData1();
            }
        }
        private void button81_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int id = Convert.ToInt32(selectedRow.Cells["Код"].Value);
                try
                {
                    string name = textBox35.Text;
                    int levelCode = int.Parse(textBox34.Text);
                    int authorCode = int.Parse(textBox33.Text);
                    int languageCode = int.Parse(textBox32.Text);
                    string link = textBox36.Text;

                    byte[] photo = null;
                    if (!string.IsNullOrEmpty(pictureBox3.ImageLocation))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            Image img = Image.FromFile(pictureBox3.ImageLocation);
                            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            photo = ms.ToArray();
                        }
                    }
                    else
                    {
                        photo = null;
                    }

                    UpdateBook(id, name, levelCode, authorCode, languageCode, link, photo);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Please enter valid numeric values for level, author, and language codes.");
                }
            }
            else
            {
                MessageBox.Show("Please select a book to update.");
            }
        }
        private void button83_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox3.ImageLocation = openFileDialog.FileName;
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                textBox35.Text = selectedRow.Cells["Назва"].Value?.ToString() ?? string.Empty;
                textBox34.Text = selectedRow.Cells["Код_Рівень"].Value?.ToString() ?? string.Empty;
                textBox33.Text = selectedRow.Cells["Код_Автор"].Value?.ToString() ?? string.Empty;
                textBox32.Text = selectedRow.Cells["Код_Мова"].Value?.ToString() ?? string.Empty;
                textBox36.Text = selectedRow.Cells["Посилання"].Value?.ToString() ?? string.Empty;
                if (selectedRow.Cells["Фото"].Value != DBNull.Value && selectedRow.Cells["Фото"].Value != null)
                {
                    byte[] imageBytes = (byte[])selectedRow.Cells["Фото"].Value;
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        pictureBox3.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    pictureBox3.Image = null;
                }
            }
        }

        private void button82_Click(object sender, EventArgs e)
        {
            panel16.Visible = false;
            panel3.Visible = true;
            LoadData1();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            panel16.Visible = true;
            LoadData1();
        }
        /* Видалення книга */
        private void DeleteBook(int id)
        {
            try
            {
                string query = "DELETE FROM Книга WHERE Код = @id;";
                using (SqlCommand cmd = new SqlCommand(query, BooksForLearningConnection))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    BooksForLearningConnection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Book deleted successfully.");
                    }
                    else
                    {
                        MessageBox.Show("No book found to delete.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting book: {ex.Message}");
            }
            finally
            {
                BooksForLearningConnection.Close();
                LoadData1();
            }
        }
        private void button79_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int id = Convert.ToInt32(selectedRow.Cells["Код"].Value);
                DeleteBook(id);
            }
            else
            {
                MessageBox.Show("Please select a book to delete.");
            }
        }
        private void button80_Click(object sender, EventArgs e)
        {
            panel15.Visible = false;
            panel3.Visible = true;
            LoadData1();
        }
        private void button8_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            panel15.Visible = true;
            LoadData1();
        }
        /* Завантаження таблиць Пошуку */
        public class MyItem
        {
            public int Код { get; set; }
            public string Назва { get; set; }
            public Image Фото { get; set; }
            public string Рівень { get; set; }
            public string Автор { get; set; }
            public string Мова { get; set; }
            public string Посилання { get; set; }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            panel1.Visible = false;
            panel36.Visible = false;
            panel34.Visible = false;
            LoadData7();
        }
        private List<MyItem> items = new List<MyItem>();
        private int currentIndex = 0;
        private void LoadData7()
        {
            items.Clear();
            if (BooksForLearningConnection.State != ConnectionState.Open)
            {
                BooksForLearningConnection.Open();
            }

            SqlDataReader reader = null;

            try
            {
                string query = @"
    SELECT 
        Книга.Код, 
        Книга.Назва, 
        Книга.Фото, 
        Рівень.Рівень, 
        Автор.[Ім'я] AS Автор, 
        Мова.Мова, 
        Книга.Посилання
    FROM Книга 
    LEFT JOIN Автор ON Автор.Код = Книга.Код_Автор 
    LEFT JOIN Мова ON Мова.Код = Книга.Код_Мова 
    LEFT JOIN Рівень ON Рівень.Код = Книга.Код_Рівень;
";

                SqlCommand cmd = new SqlCommand(query, BooksForLearningConnection);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    MyItem item = new MyItem
                    {
                        Код = Convert.ToInt32(reader["Код"]),
                        Назва = Convert.ToString(reader["Назва"]),
                        Рівень = Convert.ToString(reader["Рівень"]),
                        Автор = Convert.ToString(reader["Автор"]),
                        Мова = Convert.ToString(reader["Мова"]),
                        Посилання = Convert.ToString(reader["Посилання"])
                    };

                    if (reader["Фото"] != DBNull.Value)
                    {
                        byte[] imageBytes = (byte[])reader["Фото"];
                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            item.Фото = Image.FromStream(ms);
                        }
                    }

                    items.Add(item);
                }

                if (items.Count > 0)
                {
                    DisplayItem(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                reader?.Close();

                if (BooksForLearningConnection.State == ConnectionState.Open)
                {
                    BooksForLearningConnection.Close();
                }
            }
        }
        private void DisplayItem(int index)
        {
            if (index < 0 || index >= items.Count)
                return;

            MyItem item = items[index];
            label36.Text = item.Назва;
            label37.Text = item.Автор;
            label38.Text = item.Рівень;
            label39.Text = item.Мова;
            pictureBox4.Image = item.Фото;
            linkLabel1.Text = "Посилання на книгу";
            linkLabel1.Links.Clear();
            linkLabel1.Links.Add(0, item.Посилання.Length, item.Посилання);
        }
        private void button84_Click(object sender, EventArgs e)
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                DisplayItem(currentIndex);
            }
        }
        private void button85_Click(object sender, EventArgs e)
        {
            if (currentIndex < items.Count - 1)
            {
                currentIndex++;
                DisplayItem(currentIndex);
            }
        }
        private void button86_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            LoadData7();
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (currentIndex < 0 || currentIndex >= items.Count)
                return;

            MyItem currentItem = items[currentIndex];

            if (Code == "0")
            {
                MessageBox.Show("Код користувача не встановлений!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string query = "INSERT INTO Історія (Код_Аккаунт, Код_Книга) VALUES (@CodeAccount, @CodeBook)";

                if (BooksForLearningConnection.State != ConnectionState.Open)
                {
                    BooksForLearningConnection.Open();
                }

                using (SqlCommand cmd = new SqlCommand(query, BooksForLearningConnection))
                {
                    cmd.Parameters.AddWithValue("@CodeAccount", Code);
                    cmd.Parameters.AddWithValue("@CodeBook", currentItem.Код);

                    int rowsAffected = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час додавання до історії: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (BooksForLearningConnection.State == ConnectionState.Open)
                {
                    BooksForLearningConnection.Close();
                }
            }
            string url = e.Link.LinkData as string;
            if (!string.IsNullOrEmpty(url))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
        }
        /* Реалізація Пошуку */
        private void LoadLanguages()
        {
            try
            {
                string query = "SELECT Код, Мова FROM Мова;";
                using (SqlDataAdapter da = new SqlDataAdapter(query, BooksForLearningConnection))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    comboBox1.DataSource = dt;
                    comboBox1.DisplayMember = "Мова";
                    comboBox1.ValueMember = "Код";
                    comboBox1.SelectedIndex = -1; // Нічого не вибрано за замовчуванням
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data for Languages: {ex.Message}");
            }
        }
        private void LoadLevels()
        {
            try
            {
                string query = "SELECT Код, Рівень FROM Рівень;";
                using (SqlDataAdapter da = new SqlDataAdapter(query, BooksForLearningConnection))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    comboBox2.DataSource = dt;
                    comboBox2.DisplayMember = "Рівень";
                    comboBox2.ValueMember = "Код";
                    comboBox2.SelectedIndex = -1; // Нічого не вибрано за замовчуванням
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data for Levels: {ex.Message}");
            }
        }
        private void SearchItems()
        {
            items.Clear();
            if (BooksForLearningConnection.State != ConnectionState.Open)
            {
                BooksForLearningConnection.Open();
            }
            SqlDataReader reader = null;
            try
            {
                string query = @"
        SELECT 
            Книга.Код, 
            Книга.Назва, 
            Книга.Фото, 
            Рівень.Рівень, 
            Автор.[Ім'я] AS Автор, 
            Мова.Мова, 
            Книга.Посилання
        FROM Книга 
        LEFT JOIN Автор ON Автор.Код = Книга.Код_Автор 
        LEFT JOIN Мова ON Мова.Код = Книга.Код_Мова 
        LEFT JOIN Рівень ON Рівень.Код = Книга.Код_Рівень
        WHERE 1=1";
                // Додаткові умови пошуку
                if (!string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    query += " AND Книга.Назва LIKE @Назва";
                }
                if (comboBox1.SelectedIndex != -1)
                {
                    query += " AND Мова.Код = @Код_Мова";
                }
                if (comboBox2.SelectedIndex != -1)
                {
                    query += " AND Рівень.Код = @Код_Рівень";
                }
                SqlCommand cmd = new SqlCommand(query, BooksForLearningConnection);
                if (!string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    cmd.Parameters.AddWithValue("@Назва", $"%{textBox1.Text}%");
                }
                if (comboBox1.SelectedIndex != -1)
                {
                    cmd.Parameters.AddWithValue("@Код_Мова", comboBox1.SelectedValue);
                }
                if (comboBox2.SelectedIndex != -1)
                {
                    cmd.Parameters.AddWithValue("@Код_Рівень", comboBox2.SelectedValue);
                }
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    MyItem item = new MyItem
                    {
                        Код = Convert.ToInt32(reader["Код"]),
                        Назва = Convert.ToString(reader["Назва"]),
                        Рівень = Convert.ToString(reader["Рівень"]),
                        Автор = Convert.ToString(reader["Автор"]),
                        Мова = Convert.ToString(reader["Мова"]),
                        Посилання = Convert.ToString(reader["Посилання"])
                    };
                    if (reader["Фото"] != DBNull.Value)
                    {
                        byte[] imageBytes = (byte[])reader["Фото"];
                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            item.Фото = Image.FromStream(ms);
                        }
                    }
                    items.Add(item);
                }
                if (items.Count > 0)
                {
                    currentIndex = 0;
                    DisplayItem(currentIndex);
                }
                else
                {
                    MessageBox.Show("Нічого не знайдено.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                reader?.Close();

                if (BooksForLearningConnection.State == ConnectionState.Open)
                {
                    BooksForLearningConnection.Close();
                }
            }
        }
        private void button87_Click(object sender, EventArgs e)
        {
            SearchItems();
        }
        // кнопка виходу
        private void button48_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        // дані акаунта
        private void button3_Click(object sender, EventArgs e)
        {
            panel34.Visible = true;
            panel1.Visible = false;
            panel2.Visible = false;
            panel36.Visible = false;
            menuButtons(button88);
            menuButtons(button89);
            textBox37.Text = Nickname;
            textBox38.Text = Email;
            label44.Text = "";
        }
        private void button88_Click(object sender, EventArgs e)
        {
            string newNickname = textBox37.Text;
            string newEmail = textBox38.Text;

            if (string.IsNullOrWhiteSpace(newNickname) || string.IsNullOrWhiteSpace(newEmail))
            {
                label44.Text = "Усі поля мають бути заповнені!";
                label44.ForeColor = Color.Red;
                label44.Visible = true;
                return;
            }
            if (Code == "0")
            {
                MessageBox.Show("Код акаунта не встановлений!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string query = "UPDATE Аккаунт SET Нікнейм = @NewNickname, Почта = @NewEmail WHERE Код = @Code";

                using (SqlCommand cmd = new SqlCommand(query, BooksForLearningConnection))
                {
                    if (BooksForLearningConnection.State == ConnectionState.Closed)
                        BooksForLearningConnection.Open();
                    cmd.Parameters.AddWithValue("@NewNickname", newNickname);
                    cmd.Parameters.AddWithValue("@NewEmail", newEmail);
                    cmd.Parameters.AddWithValue("@Code", Code);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        label44.Text = "Дані успішно змінено!";
                        label44.ForeColor = Color.Green;
                        label44.Visible = true;
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Не вдалося оновити акаунт. Спробуйте ще раз.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час редагування: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (BooksForLearningConnection.State == ConnectionState.Open)
                    BooksForLearningConnection.Close();
            }
        }

        private void button89_Click(object sender, EventArgs e)
        {
            panel35.Visible = true;
            button90.Visible = true;
            button91.Visible = true;
            textBox39.Text = string.Empty;
            textBox40.Text = string.Empty;
            menuButtons(button90);
            menuButtons(button91);
            button89.Visible = false;
        }

        private void button91_Click(object sender, EventArgs e)
        {
            panel35.Visible = false;
            button90.Visible = false;
            button91.Visible = false;
            button89.Visible = true;
        }

        private void button90_Click(object sender, EventArgs e)
        {
            string oldPassword = textBox39.Text; // Старий пароль із текстового поля
            string newPassword = textBox40.Text; // Новий пароль із текстового поля

            if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword))
            {
                label44.Text = "Усі поля мають бути заповнені!";
                label44.ForeColor = Color.Red;
                label44.Visible = true;
                return;
            }

            if (Code == "0")
            {
                MessageBox.Show("Код акаунта не встановлений!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (BooksForLearningConnection.State == ConnectionState.Closed)
                    BooksForLearningConnection.Open();

                string queryCheckPassword = "SELECT Пароль FROM Аккаунт WHERE Код = @Code";

                using (SqlCommand cmdCheck = new SqlCommand(queryCheckPassword, BooksForLearningConnection))
                {
                    cmdCheck.Parameters.AddWithValue("@Code", Code);
                    string existingPassword = cmdCheck.ExecuteScalar()?.ToString();

                    if (existingPassword != oldPassword)
                    {
                        label44.Text = "Старий пароль невірний!";
                        label44.ForeColor = Color.Red;
                        label44.Visible = true;
                        return;
                    }
                }
                string queryUpdatePassword = "UPDATE Аккаунт SET Пароль = @NewPassword WHERE Код = @Code";

                using (SqlCommand cmdUpdate = new SqlCommand(queryUpdatePassword, BooksForLearningConnection))
                {



                    cmdUpdate.Parameters.AddWithValue("@NewPassword", newPassword);
                    cmdUpdate.Parameters.AddWithValue("@Code", Code);

                    int rowsAffected = cmdUpdate.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        label44.Text = "Пароль успішно змінено!";
                        label44.ForeColor = Color.Green;
                        label44.Visible = true;
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Не вдалося змінити пароль. Спробуйте ще раз.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час зміни пароля: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                panel35.Visible = false;
                button90.Visible = false;
                button91.Visible = false;
                button89.Visible = true;
                if (BooksForLearningConnection.State == ConnectionState.Open)
                    BooksForLearningConnection.Close();
            }
        }

        //історія
        private void button4_Click(object sender, EventArgs e)
        {
            panel36.Visible = true;
            listBox1.Visible = true;
            label45.Visible = false;
            if (string.IsNullOrWhiteSpace(Code))
            {
                MessageBox.Show("Код акаунта не встановлений!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string query = @"
                    SELECT Історія.Код AS КодІсторії, Книга.Назва 
                    FROM Історія
                    INNER JOIN Книга ON Історія.Код_Книга = Книга.Код
                    WHERE Історія.Код_Аккаунт = @CodeAccount";

                List<KeyValuePair<int, string>> history = new List<KeyValuePair<int, string>>();

                using (SqlCommand cmd = new SqlCommand(query, BooksForLearningConnection))
                {
                    if (BooksForLearningConnection.State == ConnectionState.Closed)
                        BooksForLearningConnection.Open();
                    cmd.Parameters.AddWithValue("@CodeAccount", Code);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int historyCode = Convert.ToInt32(reader["КодІсторії"]);
                        string bookName = reader["Назва"].ToString();
                        history.Add(new KeyValuePair<int, string>(historyCode, bookName));
                    }

                    reader.Close();
                }
                listBox1.Items.Clear();
                foreach (var record in history)
                {
                    listBox1.Items.Add($"{record.Value}");
                }

                if (history.Count == 0)
                {
                    listBox1.Visible = false;
                    label45.Visible = true;
                    label45.Text = "Історія пошуку на даний момент порожня.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час завантаження історії: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (BooksForLearningConnection.State == ConnectionState.Open)
                    BooksForLearningConnection.Close();
            }
        }


    }
}