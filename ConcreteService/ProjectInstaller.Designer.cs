﻿namespace ConcreteService
{
    partial class ProjectInstaller
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.WCFServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.WCFServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // WCFServiceProcessInstaller
            // 
            this.WCFServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.WCFServiceProcessInstaller.Password = null;
            this.WCFServiceProcessInstaller.Username = null;
            this.WCFServiceProcessInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.WCFServiceProcessInstaller_AfterInstall);
            // 
            // WCFServiceInstaller
            // 
            this.WCFServiceInstaller.ServiceName = "WCFBasisService";
            this.WCFServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.WCFServiceProcessInstaller,
            this.WCFServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller WCFServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller WCFServiceInstaller;
    }
}