using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace TestServer
{
    public class Server
    {
        private int _port;
        private Thread _listenerThread;
        private TcpListener _listener;

        public Server() : this(7890)
        {
        }

        public Server(int port)
        {
            _port = port;
        }

        public void Start()
        {
            _listenerThread = new Thread(ListenerThread)
            {
                IsBackground = true,
                Name = "Listener"
            };

            _listenerThread.Start();
        }
        protected void ListenerThread()
        {
            try
            {
                var ipAddress = IPAddress.Parse("127.0.0.1");
                _listener = new TcpListener(ipAddress, _port);
                _listener.Start();

                var bytes = new byte[4096];

                while (true)
                {
                    TcpClient client = _listener.AcceptTcpClient();
                    ClientObject clientObject = new ClientObject(client);

                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (SocketException ex)
            {
                Trace.TraceError(string.Format("Server {0}", ex.Message));
            }
        }

        public void Stop()
        {
            _listener.Stop();
        }

        public void Suspend()
        {
            _listener.Stop();
        }

        public void Resume()
        {
            _listener.Start();
        }

    }
}
