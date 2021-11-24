
namespace InactiveEVCUser
{
    partial class Form1
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
            this.btnSendMail = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.btnProcess = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnGetFile = new System.Windows.Forms.Button();
            this.btnGetEVC = new System.Windows.Forms.Button();
            this.EVCUserReconcileWorker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // btnSendMail
            // 
            this.btnSendMail.Location = new System.Drawing.Point(420, 18);
            this.btnSendMail.Name = "btnSendMail";
            this.btnSendMail.Size = new System.Drawing.Size(104, 34);
            this.btnSendMail.TabIndex = 18;
            this.btnSendMail.Text = "Send Mail";
            this.btnSendMail.UseVisualStyleBackColor = true;
            this.btnSendMail.Click += new System.EventHandler(this.btnSendMail_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(34, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 18);
            this.label1.TabIndex = 17;
            this.label1.Text = "Status";
            // 
            // cboStatus
            // 
            this.cboStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.ItemHeight = 16;
            this.cboStatus.Location = new System.Drawing.Point(117, 23);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(175, 24);
            this.cboStatus.TabIndex = 16;
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(907, 420);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(132, 51);
            this.btnProcess.TabIndex = 15;
            this.btnProcess.Text = "Process";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Visible = false;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(20, 66);
            this.textBox2.Margin = new System.Windows.Forms.Padding(4);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2.Size = new System.Drawing.Size(966, 347);
            this.textBox2.TabIndex = 14;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(885, 267);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(154, 48);
            this.textBox1.TabIndex = 13;
            this.textBox1.Visible = false;
            // 
            // btnGetFile
            // 
            this.btnGetFile.Location = new System.Drawing.Point(911, 323);
            this.btnGetFile.Margin = new System.Windows.Forms.Padding(4);
            this.btnGetFile.Name = "btnGetFile";
            this.btnGetFile.Size = new System.Drawing.Size(128, 32);
            this.btnGetFile.TabIndex = 12;
            this.btnGetFile.Text = "Select File";
            this.btnGetFile.UseVisualStyleBackColor = true;
            this.btnGetFile.Visible = false;
            // 
            // btnGetEVC
            // 
            this.btnGetEVC.Location = new System.Drawing.Point(309, 17);
            this.btnGetEVC.Name = "btnGetEVC";
            this.btnGetEVC.Size = new System.Drawing.Size(105, 35);
            this.btnGetEVC.TabIndex = 11;
            this.btnGetEVC.Text = "Get EVC";
            this.btnGetEVC.UseVisualStyleBackColor = true;
            this.btnGetEVC.Click += new System.EventHandler(this.btnTest_Click_1);
            // 
            // EVCUserReconcileWorker
            // 
            this.EVCUserReconcileWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.EVCUserReconcileWorker_DoWork);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(999, 441);
            this.Controls.Add(this.btnSendMail);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboStatus);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnGetFile);
            this.Controls.Add(this.btnGetEVC);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSendMail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnGetFile;
        private System.Windows.Forms.Button btnGetEVC;
        private System.ComponentModel.BackgroundWorker EVCUserReconcileWorker;
    }
}

