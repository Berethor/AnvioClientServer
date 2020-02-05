using System;

namespace TestClient.Data
{
    public class ProcessInfo
    {
        public int Pid { get; }
        public string Path { get; }
        public ProcessInfo(string[] info)
        {
            if (int.TryParse(info[0], out int pid))
            {
                Pid = pid;
            }
            for (int i = 1; i < info.Length; i++)
            {
                Path += $"{info[i]} ";
            }
        }
    }
}
