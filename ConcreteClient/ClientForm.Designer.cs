namespace ConcreteClient
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
            this.components = new System.ComponentModel.Container();
            this.tbClientName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnUnregister = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rtbMessages = new System.Windows.Forms.RichTextBox();
            this.tmrChannerlUpdate = new System.Windows.Forms.Timer(this.components);
            this.btnTakeActions = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lbActionsInQueue = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbClientName
            // 
            this.tbClientName.Location = new System.Drawing.Point(6, 22);
            this.tbClientName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tbClientName.Name = "tbClientName";
            this.tbClientName.ReadOnly = true;
            this.tbClientName.Size = new System.Drawing.Size(361, 23);
            this.tbClientName.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbClientName);
            this.groupBox1.Location = new System.Drawing.Point(9, 14);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Size = new System.Drawing.Size(373, 55);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Client name:";
            // 
            // btnRegister
            // 
            this.btnRegister.Location = new System.Drawing.Point(9, 75);
            this.btnRegister.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(88, 37);
            this.btnRegister.TabIndex = 2;
            this.btnRegister.Text = "Register";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // btnUnregister
            // 
            this.btnUnregister.Location = new System.Drawing.Point(289, 75);
            this.btnUnregister.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnUnregister.Name = "btnUnregister";
            this.btnUnregister.Size = new System.Drawing.Size(88, 37);
            this.btnUnregister.TabIndex = 3;
            this.btnUnregister.Text = "Un-register";
            this.btnUnregister.UseVisualStyleBackColor = true;
            this.btnUnregister.Click += new System.EventHandler(this.btnUnregister_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rtbMessages);
            this.groupBox2.Location = new System.Drawing.Point(9, 267);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Size = new System.Drawing.Size(373, 128);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Messages from service";
            // 
            // rtbMessages
            // 
            this.rtbMessages.EnableAutoDragDrop = true;
            this.rtbMessages.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.rtbMessages.Location = new System.Drawing.Point(6, 22);
            this.rtbMessages.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rtbMessages.Name = "rtbMessages";
            this.rtbMessages.Size = new System.Drawing.Size(361, 93);
            this.rtbMessages.TabIndex = 0;
            this.rtbMessages.Text = "";
            // 
            // tmrChannerlUpdate
            // 
            this.tmrChannerlUpdate.Enabled = true;
            this.tmrChannerlUpdate.Interval = 40000;
            // 
            // btnTakeActions
            // 
            this.btnTakeActions.Location = new System.Drawing.Point(150, 75);
            this.btnTakeActions.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnTakeActions.Name = "btnTakeActions";
            this.btnTakeActions.Size = new System.Drawing.Size(94, 37);
            this.btnTakeActions.TabIndex = 5;
            this.btnTakeActions.Text = "Take actions";
            this.btnTakeActions.UseVisualStyleBackColor = true;
            this.btnTakeActions.Click += new System.EventHandler(this.btnTakeActions_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lbActionsInQueue);
            this.groupBox3.Location = new System.Drawing.Point(9, 128);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(367, 133);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Actions in queue";
            // 
            // lbActionsInQueue
            // 
            this.lbActionsInQueue.FormattingEnabled = true;
            this.lbActionsInQueue.ItemHeight = 15;
            this.lbActionsInQueue.Location = new System.Drawing.Point(6, 22);
            this.lbActionsInQueue.Name = "lbActionsInQueue";
            this.lbActionsInQueue.Size = new System.Drawing.Size(355, 94);
            this.lbActionsInQueue.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 408);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnTakeActions);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnUnregister);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Form1";
            this.Text = "WCF Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbClientName;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnUnregister;
        private System.Windows.Forms.RichTextBox rtbMessages;
        private System.Windows.Forms.Timer tmrChannerlUpdate;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox lbActionsInQueue;
        private System.Windows.Forms.Button btnTakeActions;
    }
}