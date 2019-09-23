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

        private void _onServiceCurrentAction(OperationModel operation)
        {
            rtbMessages.Text = $"Currently processed action name: {operation.Name}";
        }

        private void _onServiceActionQueue(List<OperationModel> actions)
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