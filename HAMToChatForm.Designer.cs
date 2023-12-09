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
            checkBox8 = new CheckBox();
            checkBox7 = new CheckBox();
            checkBox5 = new CheckBox();
            checkBox4 = new CheckBox();
            checkBox3 = new CheckBox();
            label1 = new Label();
            panel2 = new Panel();
            checkBox9 = new CheckBox();
            checkBox6 = new CheckBox();
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
            checkBox2.Size = new Size(68, 24);
            checkBox2.TabIndex = 1;
            checkBox2.Text = "Tense";
            checkBox2.UseVisualStyleBackColor = true;
            checkBox2.CheckedChanged += checkBox2_CheckedChanged;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ButtonShadow;
            panel1.Controls.Add(checkBox8);
            panel1.Controls.Add(checkBox7);
            panel1.Controls.Add(checkBox5);
            panel1.Controls.Add(checkBox4);
            panel1.Controls.Add(checkBox3);
            panel1.Controls.Add(label1);
            panel1.Location = new Point(9, 67);
            panel1.Name = "panel1";
            panel1.Size = new Size(219, 134);
            panel1.TabIndex = 2;
            // 
            // checkBox8
            // 
            checkBox8.AutoSize = true;
            checkBox8.Location = new Point(25, 97);
            checkBox8.Name = "checkBox8";
            checkBox8.Size = new Size(149, 24);
            checkBox8.TabIndex = 5;
            checkBox8.Text = "Update with lyrics";
            checkBox8.UseVisualStyleBackColor = true;
            checkBox8.CheckedChanged += checkBox8_CheckedChanged;
            // 
            // checkBox7
            // 
            checkBox7.AutoSize = true;
            checkBox7.Location = new Point(121, 69);
            checkBox7.Name = "checkBox7";
            checkBox7.Size = new Size(64, 24);
            checkBox7.TabIndex = 4;
            checkBox7.Text = "Time";
            checkBox7.UseVisualStyleBackColor = true;
            checkBox7.CheckedChanged += checkBox7_CheckedChanged;
            // 
            // checkBox5
            // 
            checkBox5.AutoSize = true;
            checkBox5.Location = new Point(14, 69);
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
            checkBox4.Location = new Point(121, 39);
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
            checkBox3.Location = new Point(14, 39);
            checkBox3.Name = "checkBox3";
            checkBox3.Size = new Size(71, 24);
            checkBox3.TabIndex = 1;
            checkBox3.Text = "Name";
            checkBox3.UseVisualStyleBackColor = true;
            checkBox3.CheckedChanged += checkBox3_CheckedChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(59, 0);
            label1.Name = "label1";
            label1.Size = new Size(95, 20);
            label1.TabIndex = 0;
            label1.Text = "Media Player";
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.ButtonShadow;
            panel2.Controls.Add(checkBox9);
            panel2.Controls.Add(checkBox6);
            panel2.Controls.Add(checkBox2);
            panel2.Controls.Add(checkBox1);
            panel2.Location = new Point(9, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(219, 58);
            panel2.TabIndex = 3;
            // 
            // checkBox9
            // 
            checkBox9.AutoSize = true;
            checkBox9.Location = new Point(118, 31);
            checkBox9.Name = "checkBox9";
            checkBox9.Size = new Size(101, 24);
            checkBox9.TabIndex = 3;
            checkBox9.Text = "AFKDetect";
            checkBox9.UseVisualStyleBackColor = true;
            checkBox9.CheckedChanged += checkBox9_CheckedChanged;
            // 
            // checkBox6
            // 
            checkBox6.AutoSize = true;
            checkBox6.Location = new Point(14, 31);
            checkBox6.Name = "checkBox6";
            checkBox6.Size = new Size(80, 24);
            checkBox6.TabIndex = 2;
            checkBox6.Text = "Activity";
            checkBox6.UseVisualStyleBackColor = true;
            checkBox6.CheckedChanged += checkBox6_CheckedChanged;
            // 
            // ToChat
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.GrayText;
            ClientSize = new Size(234, 206);
            Controls.Add(panel1);
            Controls.Add(panel2);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "ToChat";
            Text = "HAMToChat";
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
        private CheckBox checkBox6;
        private CheckBox checkBox7;
        private CheckBox checkBox8;
        private CheckBox checkBox9;
    }
}