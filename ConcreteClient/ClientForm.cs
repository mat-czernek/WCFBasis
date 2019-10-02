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

        private readonly ICallbackContract _callbackContract = new CallbackContract();
        
        
        public Form1()
        {
            InitializeComponent();

            _clientSetup = new ClientSetup(_callbackContract);

            _callbackContract.GeneralStatusChanged += _onGeneralStatusChanged;
            _callbackContract.OperationsQueueChanged += _onOperationQueueChanged;
            _callbackContract.CurrentOperationChanged += _onCurrentActionChanged;

            tbClientName.Text = _clientSetup.ClientId.ToString();
        }

        private void _onGeneralStatusChanged(string text)
        {
            rtbMessages.Text = text;
        }

        private void _onCurrentActionChanged(SampleOperationModel sampleOperation)
        {
            rtbMessages.Text = $@"Currently processed action name: {sampleOperation.Name}";
        }

        private void _onOperationQueueChanged(List<SampleOperationModel> actions)
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
            //_clientSetup.Unregister();
        }

        private void btnTakeActions_Click(object sender, EventArgs e)
        {
            try
            {
                _clientSetup.ServiceCommunicationChannel.ActionRequest(new ActionModel
                    {ClientId = _clientSetup.ClientId, Type = ActionType.SampleOperation, ExecuteImmediately = false});
            }
            catch (EndpointNotFoundException)
            {
                MessageBox.Show(@"Service not available!", @"WCFBasis", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }
}