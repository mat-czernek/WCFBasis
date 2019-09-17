using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client;
using Contracts;
using Contracts.Models;

namespace ConcreteClient
{
    public partial class Form1 : Form
    {
        private readonly ClientSetup _clientSetup;

        private readonly ICallbacksApi _callbacksApi = new CallbacksApi();
        
        
        public Form1()
        {
            InitializeComponent();

            _clientSetup = new ClientSetup(_callbacksApi);

            _callbacksApi.ServiceSimpleMessage += _onSimpleMessageFromService;
            _callbacksApi.ServiceActionsQueue += _onServiceActionQueue;
            _callbacksApi.ServiceCurrentAction += _onServiceCurrentAction;

            tbClientName.Text = _clientSetup.Id.ToString();
        }

        private void _onSimpleMessageFromService(string text)
        {
            rtbMessages.Text = text;
        }

        private void _onServiceCurrentAction(ActionModel action)
        {
            rtbMessages.Text = $"Currently processed action name: {action.Name}";
        }

        private void _onServiceActionQueue(List<ActionModel> actions)
        {
            lbActionsInQueue.Items.Clear();

            foreach (var action in actions)
            {
                lbActionsInQueue.Items.Add(action.Name);
            }
        }
        
        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                var result = _clientSetup.ProxyChannel.RegisterClient(_clientSetup.Id);
                rtbMessages.Text = result.ToString();
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(@"Service not available!", @"WCFBasis", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void btnUnregister_Click(object sender, EventArgs e)
        {
            try
            {
                var result = _clientSetup.ProxyChannel.UnregisterClient(_clientSetup.Id);
                rtbMessages.Text = result.ToString();
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(@"Service not available!", @"WCFBasis", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _clientSetup.ProxyChannel.UnregisterClient(_clientSetup.Id);
            }
            catch (EndpointNotFoundException){}
            
        }

        private void btnTakeActions_Click(object sender, EventArgs e)
        {
            _clientSetup.ProxyChannel.TakeActions();
        }
        
    }
}