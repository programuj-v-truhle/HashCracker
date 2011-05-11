namespace HashCracker
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textCharset = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numericLength = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.comboAlgo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textHash = new System.Windows.Forms.TextBox();
            this.labelHash = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelTimeRemaining = new System.Windows.Forms.Label();
            this.labelSpeed = new System.Windows.Forms.Label();
            this.labelRemainingCombinations = new System.Windows.Forms.Label();
            this.labelTotalCombinations = new System.Windows.Forms.Label();
            this.progressTotal = new System.Windows.Forms.ProgressBar();
            this.buttonStartStop = new System.Windows.Forms.Button();
            this.timerUpdater = new System.Windows.Forms.Timer(this.components);
            this.labelResult = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericLength)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textCharset);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.numericLength);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.comboAlgo);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textHash);
            this.groupBox1.Controls.Add(this.labelHash);
            this.groupBox1.Location = new System.Drawing.Point(2, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(291, 136);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Zadání";
            // 
            // textCharset
            // 
            this.textCharset.Location = new System.Drawing.Point(103, 99);
            this.textCharset.Name = "textCharset";
            this.textCharset.Size = new System.Drawing.Size(182, 20);
            this.textCharset.TabIndex = 7;
            this.textCharset.Text = "abcdefghijklmnopqrstuvwxyz1234567890";
            this.textCharset.Validating += new System.ComponentModel.CancelEventHandler(this.textCharset_Validating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Znaková sada :";
            // 
            // numericLength
            // 
            this.numericLength.Location = new System.Drawing.Point(103, 73);
            this.numericLength.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numericLength.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericLength.Name = "numericLength";
            this.numericLength.Size = new System.Drawing.Size(56, 20);
            this.numericLength.TabIndex = 5;
            this.numericLength.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Maximální délka :";
            // 
            // comboAlgo
            // 
            this.comboAlgo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboAlgo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboAlgo.FormattingEnabled = true;
            this.comboAlgo.Items.AddRange(new object[] {
            "MD5",
            "SHA256"});
            this.comboAlgo.Location = new System.Drawing.Point(103, 45);
            this.comboAlgo.Name = "comboAlgo";
            this.comboAlgo.Size = new System.Drawing.Size(121, 21);
            this.comboAlgo.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Druh algoritmu :";
            // 
            // textHash
            // 
            this.textHash.Location = new System.Drawing.Point(51, 17);
            this.textHash.Name = "textHash";
            this.textHash.Size = new System.Drawing.Size(234, 20);
            this.textHash.TabIndex = 1;
            // 
            // labelHash
            // 
            this.labelHash.AutoSize = true;
            this.labelHash.Location = new System.Drawing.Point(6, 20);
            this.labelHash.Name = "labelHash";
            this.labelHash.Size = new System.Drawing.Size(38, 13);
            this.labelHash.TabIndex = 0;
            this.labelHash.Text = "Hash :";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelTimeRemaining);
            this.groupBox2.Controls.Add(this.labelSpeed);
            this.groupBox2.Controls.Add(this.labelRemainingCombinations);
            this.groupBox2.Controls.Add(this.labelTotalCombinations);
            this.groupBox2.Controls.Add(this.progressTotal);
            this.groupBox2.Location = new System.Drawing.Point(2, 145);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(291, 106);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Průběh";
            // 
            // labelTimeRemaining
            // 
            this.labelTimeRemaining.AutoSize = true;
            this.labelTimeRemaining.Location = new System.Drawing.Point(160, 56);
            this.labelTimeRemaining.Name = "labelTimeRemaining";
            this.labelTimeRemaining.Size = new System.Drawing.Size(77, 13);
            this.labelTimeRemaining.TabIndex = 4;
            this.labelTimeRemaining.Text = "Zbývá (minut) :";
            // 
            // labelSpeed
            // 
            this.labelSpeed.AutoSize = true;
            this.labelSpeed.Location = new System.Drawing.Point(6, 57);
            this.labelSpeed.Name = "labelSpeed";
            this.labelSpeed.Size = new System.Drawing.Size(54, 13);
            this.labelSpeed.TabIndex = 3;
            this.labelSpeed.Text = "Rychlost :";
            // 
            // labelRemainingCombinations
            // 
            this.labelRemainingCombinations.AutoSize = true;
            this.labelRemainingCombinations.Location = new System.Drawing.Point(6, 16);
            this.labelRemainingCombinations.Name = "labelRemainingCombinations";
            this.labelRemainingCombinations.Size = new System.Drawing.Size(94, 13);
            this.labelRemainingCombinations.TabIndex = 2;
            this.labelRemainingCombinations.Text = "Zbývá vyzkoušet :";
            // 
            // labelTotalCombinations
            // 
            this.labelTotalCombinations.AutoSize = true;
            this.labelTotalCombinations.Location = new System.Drawing.Point(6, 34);
            this.labelTotalCombinations.Name = "labelTotalCombinations";
            this.labelTotalCombinations.Size = new System.Drawing.Size(134, 13);
            this.labelTotalCombinations.TabIndex = 1;
            this.labelTotalCombinations.Text = "Celkový počet kombinací :";
            // 
            // progressTotal
            // 
            this.progressTotal.Location = new System.Drawing.Point(6, 77);
            this.progressTotal.Name = "progressTotal";
            this.progressTotal.Size = new System.Drawing.Size(279, 23);
            this.progressTotal.TabIndex = 0;
            // 
            // buttonStartStop
            // 
            this.buttonStartStop.Location = new System.Drawing.Point(2, 257);
            this.buttonStartStop.Name = "buttonStartStop";
            this.buttonStartStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStartStop.TabIndex = 2;
            this.buttonStartStop.Text = "Spustit";
            this.buttonStartStop.UseVisualStyleBackColor = true;
            this.buttonStartStop.Click += new System.EventHandler(this.buttonStartStop_Click);
            // 
            // labelResult
            // 
            this.labelResult.AutoSize = true;
            this.labelResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelResult.Location = new System.Drawing.Point(95, 262);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(66, 13);
            this.labelResult.TabIndex = 3;
            this.labelResult.Text = "Výsledek: ";
            this.labelResult.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 283);
            this.Controls.Add(this.labelResult);
            this.Controls.Add(this.buttonStartStop);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Prolamovač hashí";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericLength)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numericLength;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboAlgo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textHash;
        private System.Windows.Forms.Label labelHash;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label labelTimeRemaining;
        private System.Windows.Forms.Label labelSpeed;
        private System.Windows.Forms.Label labelRemainingCombinations;
        private System.Windows.Forms.Label labelTotalCombinations;
        private System.Windows.Forms.ProgressBar progressTotal;
        private System.Windows.Forms.Button buttonStartStop;
        private System.Windows.Forms.TextBox textCharset;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Timer timerUpdater;
        private System.Windows.Forms.Label labelResult;
    }
}

