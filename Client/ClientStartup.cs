using System;
using System.Windows.Forms;
using Client.Implementation.Controller;
using Client.Implementation.Model;
using Client.Implementation.View;

namespace Client
{
    /// <summary>
    /// Инициализация клиента.
    /// </summary>
    public static class ClientStartup
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            var model = new ClientModel("127.0.0.1", 666);
            var view = new ClientView();
            var controller = new ClientController(model, view);
            Application.Run(view);
        }
    }
}