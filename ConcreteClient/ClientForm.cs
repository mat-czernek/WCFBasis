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
using Contracts.Models;

namespace ConcreteClient
{
    public partial class Form1 : Form
    {
        private readonly ClientSetup _clientSetup;
        
        
        public Form1()
        {
            InitializeComponent();
            
            _clientSetup = new ClientSetup();

            _clientSetup.CallbackMethods.ServiceSimpleMessage += _onSimpleMessageFromService;
            _clientSetup.CallbackMethods.ServiceActionsQueue += _onServiceActionQueue;
            _clientSetup.CallbackMethods.ServiceCurrentAction += _onServiceCurrentAction;

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
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("Actions in queue: ");
            stringBuilder.Append(Environment.NewLine);
            
            foreach (var action in actions)
            {
                stringBuilder.Append($"Action name: {action.Name}");
                stringBuilder.Append(Environment.NewLine);
            }

            rtbMessages.Text = stringBuilder.ToString();
        }
        
        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                _clientSetup.ProxyChannel.RegisterClient(_clientSetup.Id);
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
                _clientSetup.ProxyChannel.UnregisterClient(_clientSetup.Id);
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

        private void btnGetActions_Click(object sender, EventArgs e)
        {
            _clientSetup.ProxyChannel.GetActions();
        }
    }
}