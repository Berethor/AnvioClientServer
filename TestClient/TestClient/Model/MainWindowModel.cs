using System;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

using TestClient.Data;

namespace TestClient.Model
{
    public class MainWindowModel
    {
        private const int bufferSize = 1024;

        private readonly int port = Properties.Settings.Default.PortNumber;
        private readonly string serverName = Properties.Settings.Default.ServerName;
        private static TcpClient client;
        private Socket socket = null;
        private StringBuilder procInfo = new StringBuilder();

        public ObservableCollection<ProcessInfo> ProccessInfos { get; } = new ObservableCollection<ProcessInfo>();
        public string Log { get; private set; } = string.Empty;

        public void StopProcByPath(string path)
        {
            var msg = $"Stop {path}";
            SendRequest(msg);

            Log += $"{msg}:" + procInfo.ToString() + Environment.NewLine;
        }
        public void StopProcByPid(int pid)
        {
            var msg = $"Stop {pid.ToString()}";
            SendRequest(msg);

            Log += $"{msg}:" + procInfo.ToString() + Environment.NewLine;
        }
        public void RestartProcByPath(string path)
        {
            var msg = $"Restart {path}";
            SendRequest(msg);

            Log += $"{msg}:" + procInfo.ToString() + Environment.NewLine;
        }
        public void RestartProcByPid(int pid)
        {
            var msg = $"Restart {pid.ToString()}";
            SendRequest(msg);

            Log += $"{msg}:" + procInfo.ToString() + Environment.NewLine;
        }
        public void GetAllProcInfo()
        {
            Log += "Отправка запроса на получение информации" + Environment.NewLine;
            SendRequest("GetAllProc 0");
            ProccessInfos.Clear();

            var array =  procInfo.ToString().Split(new[]
                                { "\r\n", "\r", "\n" },
                                StringSplitOptions.None).Distinct().ToList();

            foreach (var info in array)
            {
                var proccInfo = info.Split(' ');

                if (proccInfo.Length < 2 ||
                    proccInfo[1] == string.Empty)
                {
                    continue;
                }
                ProccessInfos.Add(new ProcessInfo(proccInfo));
            }

            Log += "Обновлена информация о процессах" + Environment.NewLine;
        }
        public MainWindowModel()
        {
        }
        private void SendRequest(string msg)
        {
            client = new TcpClient(serverName, port);
            socket = client.Client;

            try
            {
                byte[] requestMsg = Encoding.Unicode.GetBytes(msg);

                socket.Send(requestMsg);
                var buffer = new byte[bufferSize];
                StringBuilder callBackInfo = new StringBuilder();
                
                do
                {
                    int received = socket.Receive(buffer);
                    callBackInfo.Append(Encoding.Unicode.GetString(buffer));
                }
                while (socket.Available > 0);

                procInfo = callBackInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (socket != null)
                {
                    socket.Close();
                }

                if (client.Connected)
                {
                    client.Close();
                }
            }
        }
    }
}
