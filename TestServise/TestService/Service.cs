using System;
using System.ServiceProcess;

using TestServer;

namespace TestService
{
    public partial class Service : ServiceBase
    {
        private Server server;
        public Service()
        {
            InitializeComponent();
        }
        public void OnDebug()
        {
            OnStart(null);
        }
        protected override void OnStart(string[] args)
        {
            server = new Server(6777);
            server.Start();
        }

        protected override void OnStop()
        {
            server.Stop();
        }
    }
}
