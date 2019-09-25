using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Windows.Forms;
using Client;
using Contracts;
using Contracts.Enums;
using Contracts.Models;

namespace ConcreteClient
{
    public partial class Form1 : Form
    {
        private readonly ClientSetup _clientSetup;

        private readonly IClientCallbackContract _clientCallbackContract = new ClientCallbackContract();
        
        
        public Form1()
        {
            InitializeComponent();

            _clientSetup = new ClientSetup(_clientCallbackContract);

            _clientCallbackContract.ServiceSimpleMessage += _onSimpleMessageFromService;
            _clientCallbackContract.ServiceActionsQueue += _onServiceActionQueue;
            _clientCallbackContract.ServiceCurrentAction += _onServiceCurrentAction;

            tbClientName.Text = _clientSetup.Id.ToString();
        }

        private void _onSimpleMessageFromService(string text)
        {
            rtbMessages.Text = text;
        }

        private void _onServiceCurrentAction(DelayedOperationModel delayedOperation)
        {
            rtbMessages.Text = $"Currently processed action name: {delayedOperation.Name}";
        }

        private void _onServiceActionQueue(List<DelayedOperationModel> actions)
        {
            lbActionsInQueue.Items.Clear();

            foreach (var action in actions)
            {
                lbActionsInQueue.Items.Add(action.Name);
            }
        }
        


        private void btnUnregister_Click(object sender, EventArgs e)
        {
            _clientSetup.Unregister();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            _clientSetup.Register();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _clientSetup.Unregister();
        }

        private void btnTakeActions_Click(object sender, EventArgs e)
        {
            try
            {
                _clientSetup.ProxyChannel.ActionRequest(new ActionModel
                    {ClientId = _clientSetup.Id, Type = ActionType.SampleOperation, ExecuteImmediately = false});
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(@"Service not available!", @"WCFBasis", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }
}