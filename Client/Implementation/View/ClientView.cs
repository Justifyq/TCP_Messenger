using System.Drawing;
using System.Windows.Forms;

namespace Client.Implementation.View
{
    /// <summary>
    /// Вью клиента.
    /// </summary>
    public class ClientView : Form
    {
        /// <summary>
        /// Сообщение в текстбоксе.
        /// </summary>
        public string Message
        {
            get => messageTextBox.Text;
            set => messageTextBox.Text = value;
        }

        /// <summary>
        /// Кнопка отправки сообщения
        /// </summary>
        public readonly Button SendButton;
        /// <summary>
        /// Текстбокс чата
        /// </summary>
        public readonly TextBox ChatTextBox;
        private readonly TextBox messageTextBox;

        public ClientView()
        {
            messageTextBox = new TextBox();
            SendButton = new Button();
            ChatTextBox = new TextBox();

            messageTextBox.Location = new Point(10, 10);
            messageTextBox.Size = new Size(270, 20);

            SendButton.Location = new Point(290, 10);
            SendButton.Size = new Size(75, 20);

            SendButton.Text = "Send";

            ChatTextBox.Location = new Point(10, 40);
            ChatTextBox.Size = new Size(355, 200);

            ChatTextBox.Multiline = true;
            ChatTextBox.ReadOnly = true;

            Controls.Add(messageTextBox);
            Controls.Add(SendButton);
            Controls.Add(ChatTextBox);
        }
    }
}