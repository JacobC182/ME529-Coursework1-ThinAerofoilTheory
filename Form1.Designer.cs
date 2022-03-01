namespace Coursework_1
{
    partial class Form1
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
            this.SolveButton = new System.Windows.Forms.Button();
            this.AnalyticalSolveButton = new System.Windows.Forms.RadioButton();
            this.NumericalSolveButton = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.InfoButton = new System.Windows.Forms.Button();
            this.NACAtextBox = new System.Windows.Forms.TextBox();
            this.AttackAngletextBox = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.AoARenderCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // SolveButton
            // 
            this.SolveButton.Location = new System.Drawing.Point(962, 12);
            this.SolveButton.Name = "SolveButton";
            this.SolveButton.Size = new System.Drawing.Size(69, 52);
            this.SolveButton.TabIndex = 0;
            this.SolveButton.Text = "Solve";
            this.SolveButton.UseVisualStyleBackColor = true;
            this.SolveButton.Click += new System.EventHandler(this.SolveButton_Click);
            // 
            // AnalyticalSolveButton
            // 
            this.AnalyticalSolveButton.AutoSize = true;
            this.AnalyticalSolveButton.Location = new System.Drawing.Point(852, 85);
            this.AnalyticalSolveButton.Name = "AnalyticalSolveButton";
            this.AnalyticalSolveButton.Size = new System.Drawing.Size(112, 19);
            this.AnalyticalSolveButton.TabIndex = 1;
            this.AnalyticalSolveButton.TabStop = true;
            this.AnalyticalSolveButton.Text = "Analytical Solver";
            this.AnalyticalSolveButton.UseVisualStyleBackColor = true;
            // 
            // NumericalSolveButton
            // 
            this.NumericalSolveButton.AutoSize = true;
            this.NumericalSolveButton.Location = new System.Drawing.Point(852, 110);
            this.NumericalSolveButton.Name = "NumericalSolveButton";
            this.NumericalSolveButton.Size = new System.Drawing.Size(115, 19);
            this.NumericalSolveButton.TabIndex = 2;
            this.NumericalSolveButton.TabStop = true;
            this.NumericalSolveButton.Text = "Numerical Solver";
            this.NumericalSolveButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(730, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "NACA Aerofoil Code";
            // 
            // InfoButton
            // 
            this.InfoButton.Location = new System.Drawing.Point(962, 497);
            this.InfoButton.Name = "InfoButton";
            this.InfoButton.Size = new System.Drawing.Size(69, 52);
            this.InfoButton.TabIndex = 5;
            this.InfoButton.Text = "Info";
            this.InfoButton.UseVisualStyleBackColor = true;
            this.InfoButton.Click += new System.EventHandler(this.InfoButton_Click);
            // 
            // NACAtextBox
            // 
            this.NACAtextBox.Location = new System.Drawing.Point(852, 12);
            this.NACAtextBox.Name = "NACAtextBox";
            this.NACAtextBox.Size = new System.Drawing.Size(100, 23);
            this.NACAtextBox.TabIndex = 6;
            // 
            // AttackAngletextBox
            // 
            this.AttackAngletextBox.Location = new System.Drawing.Point(852, 41);
            this.AttackAngletextBox.Name = "AttackAngletextBox";
            this.AttackAngletextBox.Size = new System.Drawing.Size(100, 23);
            this.AttackAngletextBox.TabIndex = 7;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(852, 149);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 23);
            this.textBox3.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(816, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "AoA";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(767, 152);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 15);
            this.label3.TabIndex = 10;
            this.label3.Text = "Zero-Lift AoA";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(808, 182);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 15);
            this.label4.TabIndex = 12;
            this.label4.Text = "label4";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(852, 179);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(100, 23);
            this.textBox4.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(808, 215);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 15);
            this.label5.TabIndex = 14;
            this.label5.Text = "label5";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(852, 212);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(100, 23);
            this.textBox5.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(808, 248);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 15);
            this.label6.TabIndex = 16;
            this.label6.Text = "label6";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(852, 245);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(100, 23);
            this.textBox6.TabIndex = 15;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(12, 15);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(558, 520);
            this.panel1.TabIndex = 18;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(816, 386);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 15);
            this.label7.TabIndex = 19;
            this.label7.Text = "AoA";
            // 
            // AoARenderCheckBox
            // 
            this.AoARenderCheckBox.AutoSize = true;
            this.AoARenderCheckBox.Location = new System.Drawing.Point(852, 386);
            this.AoARenderCheckBox.Name = "AoARenderCheckBox";
            this.AoARenderCheckBox.Size = new System.Drawing.Size(115, 19);
            this.AoARenderCheckBox.TabIndex = 20;
            this.AoARenderCheckBox.Text = "Render with AoA";
            this.AoARenderCheckBox.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1044, 561);
            this.Controls.Add(this.AoARenderCheckBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.AttackAngletextBox);
            this.Controls.Add(this.NACAtextBox);
            this.Controls.Add(this.InfoButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NumericalSolveButton);
            this.Controls.Add(this.AnalyticalSolveButton);
            this.Controls.Add(this.SolveButton);
            this.MinimumSize = new System.Drawing.Size(1060, 600);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button SolveButton;
        private RadioButton AnalyticalSolveButton;
        private RadioButton NumericalSolveButton;
        private Label label1;
        private Button InfoButton;
        private TextBox NACAtextBox;
        private TextBox AttackAngletextBox;
        private TextBox textBox3;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox textBox4;
        private Label label5;
        private TextBox textBox5;
        private Label label6;
        private TextBox textBox6;
        private Panel panel1;
        private Label label7;
        private CheckBox AoARenderCheckBox;
    }
}