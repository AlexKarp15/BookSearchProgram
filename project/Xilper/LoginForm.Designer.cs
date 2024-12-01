namespace Xilper
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            linkLabel1 = new LinkLabel();
            label5 = new Label();
            button1 = new Button();
            label6 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label1.Location = new Point(34, 25);
            label1.Name = "label1";
            label1.Size = new Size(383, 31);
            label1.TabIndex = 0;
            label1.Text = "Вітаємо Вас! Зайдіть, будь-ласка,";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label2.Location = new Point(94, 97);
            label2.Name = "label2";
            label2.Size = new Size(93, 28);
            label2.TabIndex = 1;
            label2.Text = "Нікнейм:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label3.Location = new Point(94, 182);
            label3.Name = "label3";
            label3.Size = new Size(85, 28);
            label3.TabIndex = 2;
            label3.Text = "Пароль:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label4.Location = new Point(34, 306);
            label4.Name = "label4";
            label4.Size = new Size(153, 28);
            label4.TabIndex = 3;
            label4.Text = "Немає акаунту?";
            // 
            // textBox1
            // 
            textBox1.BackColor = SystemColors.MenuHighlight;
            textBox1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            textBox1.ForeColor = SystemColors.Window;
            textBox1.Location = new Point(94, 128);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(264, 34);
            textBox1.TabIndex = 4;
            // 
            // textBox2
            // 
            textBox2.BackColor = SystemColors.MenuHighlight;
            textBox2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            textBox2.ForeColor = SystemColors.Window;
            textBox2.Location = new Point(94, 213);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(267, 34);
            textBox2.TabIndex = 5;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            linkLabel1.LinkColor = Color.RoyalBlue;
            linkLabel1.Location = new Point(205, 306);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(200, 28);
            linkLabel1.TabIndex = 6;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "Створіть його зараз!";
            linkLabel1.VisitedLinkColor = Color.RoyalBlue;
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label5.Location = new Point(145, 56);
            label5.Name = "label5";
            label5.Size = new Size(164, 31);
            label5.TabIndex = 7;
            label5.Text = "у свій акаунт!";
            // 
            // button1
            // 
            button1.BackColor = SystemColors.MenuHighlight;
            button1.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 204);
            button1.ForeColor = SystemColors.ButtonFace;
            button1.Location = new Point(145, 351);
            button1.Name = "button1";
            button1.Size = new Size(163, 42);
            button1.TabIndex = 8;
            button1.Text = "Увійти";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(83, 269);
            label6.Name = "label6";
            label6.Size = new Size(0, 20);
            label6.TabIndex = 9;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(464, 424);
            Controls.Add(label6);
            Controls.Add(button1);
            Controls.Add(label5);
            Controls.Add(linkLabel1);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Xilper";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox textBox1;
        private TextBox textBox2;
        private LinkLabel linkLabel1;
        private Label label5;
        private Button button1;
        private Label label6;
    }
}