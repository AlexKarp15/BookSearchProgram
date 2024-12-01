namespace Xilper
{
    partial class SignUp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SignUp));
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            button1 = new Button();
            label6 = new Label();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.BackColor = SystemColors.MenuHighlight;
            textBox1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            textBox1.ForeColor = SystemColors.Window;
            textBox1.Location = new Point(137, 117);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(264, 34);
            textBox1.TabIndex = 5;
            // 
            // textBox2
            // 
            textBox2.BackColor = SystemColors.MenuHighlight;
            textBox2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            textBox2.ForeColor = SystemColors.Window;
            textBox2.Location = new Point(137, 196);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(264, 34);
            textBox2.TabIndex = 6;
            // 
            // textBox3
            // 
            textBox3.BackColor = SystemColors.MenuHighlight;
            textBox3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            textBox3.ForeColor = SystemColors.Window;
            textBox3.Location = new Point(137, 279);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(264, 34);
            textBox3.TabIndex = 7;
            // 
            // textBox4
            // 
            textBox4.BackColor = SystemColors.MenuHighlight;
            textBox4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            textBox4.ForeColor = SystemColors.Window;
            textBox4.Location = new Point(137, 363);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(264, 34);
            textBox4.TabIndex = 8;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label1.Location = new Point(112, 23);
            label1.Name = "label1";
            label1.Size = new Size(305, 41);
            label1.TabIndex = 9;
            label1.Text = "Створення акаунту!";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label2.Location = new Point(137, 86);
            label2.Name = "label2";
            label2.Size = new Size(93, 28);
            label2.TabIndex = 10;
            label2.Text = "Нікнейм:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label3.Location = new Point(137, 165);
            label3.Name = "label3";
            label3.Size = new Size(182, 28);
            label3.TabIndex = 11;
            label3.Text = "Електронна пошта";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label4.Location = new Point(137, 248);
            label4.Name = "label4";
            label4.Size = new Size(85, 28);
            label4.TabIndex = 12;
            label4.Text = "Пароль:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label5.Location = new Point(137, 332);
            label5.Name = "label5";
            label5.Size = new Size(229, 28);
            label5.TabIndex = 13;
            label5.Text = "Підтвердження пароля:";
            // 
            // button1
            // 
            button1.BackColor = Color.Yellow;
            button1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 204);
            button1.Location = new Point(169, 446);
            button1.Name = "button1";
            button1.Size = new Size(197, 61);
            button1.TabIndex = 14;
            button1.Text = "Створити";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(40, 414);
            label6.Name = "label6";
            label6.Size = new Size(0, 20);
            label6.TabIndex = 15;
            // 
            // SignUp
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(563, 531);
            Controls.Add(label6);
            Controls.Add(button1);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBox4);
            Controls.Add(textBox3);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "SignUp";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Xilper";
            FormClosed += SignUp_FormClosed;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Button button1;
        private Label label6;
    }
}