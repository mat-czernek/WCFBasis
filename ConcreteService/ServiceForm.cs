using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Contracts;
using Service;

namespace ConcreteService
{
    public partial class ServiceForm : Form
    {
        private readonly Host _wcfHost;
        
        public ServiceForm()
        {
            InitializeComponent();
            
            _wcfHost = new Host();

            _wcfHost.Open();
        }



        private void _onClientRegistration(string clientId)
        {
            chklbRegisteredClients.Items.Add(clientId);
        }

        private void _onClientUnregistration(string clientId)
        {
            var indexToRemove = chklbRegisteredClients.Items.IndexOf(clientId);
            
            if(indexToRemove < 0) return;
            
            chklbRegisteredClients.Items.RemoveAt(indexToRemove);
        }
        
        private void ServiceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _wcfHost.Close();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
   
        }

        private void btnSendToSelected_Click(object sender, EventArgs e)
        {

        }
    }
}