using System;
using System.Threading;
using Client.Implementation.Model;
using Client.Implementation.View;

namespace Client.Implementation.Controller
{
    /// <summary>
    /// Контроллер клиента
    /// </summary>
    public class ClientController
    {
        private readonly ClientModel _model;
        private readonly ClientView _view;

        public ClientController(ClientModel model, ClientView view)
        {
            _model = model;
            _view = view;

            view.SendButton.Click += SendButton_Click;

            new Thread(ReceiveMessages).Start();
        }
        
        private void SendButton_Click(object sender, EventArgs e)
        {
            var message = _view.Message;
            _view.Message = string.Empty;
            _model.SendMessage(message);
            _view.ChatTextBox.AppendText("You: " + message + Environment.NewLine);
        }

        private void ReceiveMessages()
        {
            while (true)
            {
                var message = _model.ReceiveMessage();
                _view.ChatTextBox.Invoke(new Action(() => _view.ChatTextBox.AppendText(message + Environment.NewLine)));
            }
        }
    }
}