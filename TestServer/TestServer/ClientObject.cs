using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace TestServer
{
    public class ClientObject
    {
        private IClientMsg _clientMsg;
        public TcpClient client;

        public ClientObject(TcpClient tcpClient)
        {
            client = tcpClient;
        }

        public void Process()
        {
            Socket  clientSocket = null;
            try
            {
                clientSocket = client.Client;
                var bytes = new byte[4096];
                var byteRecieve = 0;

                while (true)
                {
                    var msgRecieved = Encoding.Unicode.GetString(bytes, 0, byteRecieve);

                    while (clientSocket.Available > 0)
                    {
                        byteRecieve = clientSocket.Receive(bytes);
                        msgRecieved += Encoding.Unicode.GetString(bytes, 0, byteRecieve);
                    }

                    var msg = msgRecieved.Split(' ');
                    string message;

                    if (msg.Length < 2)
                    {
                        continue;
                    }

                    switch (msg[0])
                    {
                        case ("GetAllProc"):
                            _clientMsg = new GetAllInfoMsg();
                            break;
                        case ("Restart"):
                            _clientMsg = new RestartProcMsg();
                            break;
                        case ("Stop"):
                            _clientMsg = new StopProcMsg();
                            break;
                    }

                    message = _clientMsg.CallBackMsg(msg[1]);

                    UnicodeEncoding encoder = new UnicodeEncoding();
                    byte[] buffer = encoder.GetBytes(message);

                    clientSocket.Send(buffer);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(string.Format("Server {0}", ex.Message));
            }
            finally
            {
                if (clientSocket != null)
                    clientSocket.Close();
                if (client != null)
                    client.Close();
            }
        }
    }
}
