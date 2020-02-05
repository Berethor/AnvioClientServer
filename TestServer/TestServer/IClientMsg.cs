using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Management;

namespace TestServer
{
    public interface IClientMsg
    {
        string CallBackMsg(string msg);
    }

    public class GetAllInfoMsg : IClientMsg
    {
        public string CallBackMsg(string msg)
        {
            StringBuilder resultMsg = new StringBuilder();

            using (var mCollection = new ManagementClass("Win32_Process").GetInstances())
            {
                foreach (ManagementObject processes in mCollection)
                {
                    resultMsg.AppendLine($"{(string)processes["Handle"]} " +
                        $"{(string)processes["ExecutablePath"]}");
                }
            }
            return resultMsg.ToString();
        }
    }
    public class StopProcMsg : IClientMsg
    {
        public string CallBackMsg(string msg)
        {
            string status;

            try
            {
                if (int.TryParse(msg, out int pid))
                {
                    var proc = Process.GetProcessById(pid);
                    proc.Kill();
                }
                else
                {
                    var name = msg.Split('\\').LastOrDefault();

                    if (name != null)
                    {
                        var proc = Process.GetProcessesByName(name)
                                          .Where(a => a.StartInfo.FileName == msg)
                                          .FirstOrDefault();

                        proc.Kill();
                    }
                }

                status = "Success";
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }

            return status;
        }
    }
    public class RestartProcMsg : IClientMsg
    {
        public string CallBackMsg(string msg)
        {
            string status;

            try
            {
                if (int.TryParse(msg, out int pid))
                {
                    var proc = Process.GetProcessById(pid);

                    var path = proc.StartInfo.FileName;
                    var arguments = proc.StartInfo.Arguments;

                    proc.Kill();

                    Process.Start(path, arguments);
                }
                else
                {
                    var name = msg.Split('\\').LastOrDefault();

                    if (name != null)
                    {
                        var proc = Process.GetProcessesByName(name)
                                          .Where(a => a.StartInfo.FileName == msg)
                                          .FirstOrDefault();
                        var arguments = proc.StartInfo.Arguments;

                        proc.Kill();
                        Process.Start(msg, arguments);
                    }
                }

                status = "Success";
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }

            return status;
        }
    }
}
