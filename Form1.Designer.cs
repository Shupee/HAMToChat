namespace HR
{
    partial class ToChat
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            checkBox1 = new CheckBox();
            checkBox2 = new CheckBox();
            panel1 = new Panel();
            checkBox5 = new CheckBox();
            checkBox4 = new CheckBox();
            checkBox3 = new CheckBox();
            label1 = new Label();
            panel2 = new Panel();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(14, 3);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(98, 24);
            checkBox1.TabIndex = 0;
            checkBox1.Text = "Heartbeat";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(118, 3);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(69, 24);
            checkBox2.TabIndex = 1;
            checkBox2.Text = "Stress";
            checkBox2.UseVisualStyleBackColor = true;
            checkBox2.CheckedChanged += checkBox2_CheckedChanged;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ButtonShadow;
            panel1.Controls.Add(checkBox5);
            panel1.Controls.Add(checkBox4);
            panel1.Controls.Add(checkBox3);
            panel1.Controls.Add(label1);
            panel1.Location = new Point(9, 42);
            panel1.Name = "panel1";
            panel1.Size = new Size(213, 122);
            panel1.TabIndex = 2;
            // 
            // checkBox5
            // 
            checkBox5.AutoSize = true;
            checkBox5.Location = new Point(14, 53);
            checkBox5.Name = "checkBox5";
            checkBox5.Size = new Size(72, 24);
            checkBox5.TabIndex = 3;
            checkBox5.Text = "Artists";
            checkBox5.UseVisualStyleBackColor = true;
            checkBox5.CheckedChanged += checkBox5_CheckedChanged;
            // 
            // checkBox4
            // 
            checkBox4.AutoSize = true;
            checkBox4.Location = new Point(108, 23);
            checkBox4.Name = "checkBox4";
            checkBox4.Size = new Size(66, 24);
            checkBox4.TabIndex = 2;
            checkBox4.Text = "Lyrics";
            checkBox4.UseVisualStyleBackColor = true;
            checkBox4.CheckedChanged += checkBox4_CheckedChanged;
            // 
            // checkBox3
            // 
            checkBox3.AutoSize = true;
            checkBox3.Location = new Point(14, 23);
            checkBox3.Name = "checkBox3";
            checkBox3.Size = new Size(88, 24);
            checkBox3.TabIndex = 1;
            checkBox3.Text = "Play Info";
            checkBox3.UseVisualStyleBackColor = true;
            checkBox3.CheckedChanged += checkBox3_CheckedChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(74, 0);
            label1.Name = "label1";
            label1.Size = new Size(56, 20);
            label1.TabIndex = 0;
            label1.Text = "Spotify";
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.ButtonShadow;
            panel2.Controls.Add(checkBox2);
            panel2.Controls.Add(checkBox1);
            panel2.Location = new Point(9, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(213, 35);
            panel2.TabIndex = 3;
            // 
            // ToChat
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.GrayText;
            ClientSize = new Size(234, 173);
            Controls.Add(panel1);
            Controls.Add(panel2);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "ToChat";
            Text = "HRToChat";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private Panel panel1;
        private Label label1;
        private Panel panel2;
        private CheckBox checkBox5;
        private CheckBox checkBox4;
        private CheckBox checkBox3;
    }
}