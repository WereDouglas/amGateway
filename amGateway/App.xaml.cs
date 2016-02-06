using amLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace amGateway
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Start am { get; private set; }
        public App()
        {
            am = new Start();
            ShutdownMode = ShutdownMode.OnLastWindowClose;

        }
    }
}
