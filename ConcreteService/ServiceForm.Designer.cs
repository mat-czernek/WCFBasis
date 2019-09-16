namespace ConcreteService
{
    partial class ServiceForm
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
            this.btnSendBroadcast = new System.Windows.Forms.GroupBox();
            this.btnSendToSelected = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.tbBroadcastMessage = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chklbRegisteredClients = new System.Windows.Forms.CheckedListBox();
            this.btnSendBroadcast.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSendBroadcast
            // 
            this.btnSendBroadcast.Controls.Add(this.btnSendToSelected);
            this.btnSendBroadcast.Controls.Add(this.btnSend);
            this.btnSendBroadcast.Controls.Add(this.tbBroadcastMessage);
            this.btnSendBroadcast.Location = new System.Drawing.Point(6, 12);
            this.btnSendBroadcast.Name = "btnSendBroadcast";
            this.btnSendBroadcast.Size = new System.Drawing.Size(412, 97);
            this.btnSendBroadcast.TabIndex = 0;
            this.btnSendBroadcast.TabStop = false;
            this.btnSendBroadcast.Text = "Broadcast message to registered client";
            // 
            // btnSendToSelected
            // 
            this.btnSendToSelected.Location = new System.Drawing.Point(237, 60);
            this.btnSendToSelected.Name = "btnSendToSelected";
            this.btnSendToSelected.Size = new System.Drawing.Size(169, 23);
            this.btnSendToSelected.TabIndex = 2;
            this.btnSendToSelected.Text = "Send to selected";
            this.btnSendToSelected.UseVisualStyleBackColor = true;
            this.btnSendToSelected.Click += new System.EventHandler(this.btnSendToSelected_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(6, 60);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(169, 23);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "Send to all";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // tbBroadcastMessage
            // 
            this.tbBroadcastMessage.Location = new System.Drawing.Point(6, 31);
            this.tbBroadcastMessage.Name = "tbBroadcastMessage";
            this.tbBroadcastMessage.Size = new System.Drawing.Size(399, 23);
            this.tbBroadcastMessage.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chklbRegisteredClients);
            this.groupBox2.Location = new System.Drawing.Point(6, 115);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(412, 212);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Registered clients";
            // 
            // chklbRegisteredClients
            // 
            this.chklbRegisteredClients.FormattingEnabled = true;
            this.chklbRegisteredClients.Location = new System.Drawing.Point(7, 22);
            this.chklbRegisteredClients.Name = "chklbRegisteredClients";
            this.chklbRegisteredClients.Size = new System.Drawing.Size(399, 184);
            this.chklbRegisteredClients.TabIndex = 0;
            // 
            // ServiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 333);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnSendBroadcast);
            this.Name = "ServiceForm";
            this.Text = "WCF Service";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServiceForm_FormClosing);
            this.btnSendBroadcast.ResumeLayout(false);
            this.btnSendBroadcast.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TextBox tbBroadcastMessage;
        private System.Windows.Forms.GroupBox btnSendBroadcast;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSendToSelected;
        private System.Windows.Forms.CheckedListBox chklbRegisteredClients;
    }
}