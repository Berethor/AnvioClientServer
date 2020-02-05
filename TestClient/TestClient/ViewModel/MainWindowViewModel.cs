using System;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.ComponentModel;

using TestClient.Data;
using TestClient.Model;
using TestClient.Helpers;

namespace TestClient.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        readonly MainWindowModel model = new MainWindowModel();
        private string _log;

        #region Properties
        public ObservableCollection<ProcessInfo> ProcessInfos => model.ProccessInfos;
        public string Log
        { 
            get { return _log; }
            set
            {
                _log = value;
                OnPropertyChanged("Log");
            }
        }
        private RelayCommand _stopCommand;
        public RelayCommand StopCommand
        {
            get
            {
                return _stopCommand ??
                    (_stopCommand = new RelayCommand(a =>
                    {
                        model.StopProcByPid(((ProcessInfo)a).Pid);
                        Log = model.Log;
                        model.GetAllProcInfo();
                    }));
            }
        }
        private RelayCommand _restartCommand;
        public RelayCommand RestartCommand
        {
            get
            {
                return _restartCommand ??
                    (_restartCommand = new RelayCommand(a =>
                    {
                        model.RestartProcByPid(((ProcessInfo)a).Pid);
                        Log = model.Log;
                        model.GetAllProcInfo();
                    }));
            }
        }
        private RelayCommand _getAllProcCommand;
        public RelayCommand GetAllProcCommand
        {
            get
            {
                return _getAllProcCommand ??
                    (_getAllProcCommand = new RelayCommand(_ =>
                    {
                        model.GetAllProcInfo();
                        Log = model.Log;
                    }));
            }
        }
        #endregion

        #region Public Methods
        public MainWindowViewModel()
        {
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        #endregion
    }
}
