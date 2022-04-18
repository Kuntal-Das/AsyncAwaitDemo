using AsyncAwaitPrc.Commands;
using AsyncAwaitPrc.Helpers;
using AsyncAwaitPrc.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AsyncAwaitPrc.ViewModel
{
    class MainWindowViewModel : BaseNotifyVM
    {
        private static List<string> websites = new List<string>()
        {
            "https://www.yahoo.com",
            "https://www.google.com",
            "https://www.microsoft.com",
            "https://www.cnn.com",
            "https://www.codeproject.com",
            "https://www.stackoverflow.com",
        };

        private string _strStatus;
        public string StrStatus
        {
            get { return _strStatus; }
            set { _strStatus = value; NotifyPropertyChanged(); }
        }
        private bool isRunning;
        Stopwatch watch;

        private ICommand _downloadSyncCommand;
        public ICommand DownloadSyncCommand
        {
            get
            {
                if (_downloadSyncCommand == null)
                    _downloadSyncCommand = new RelayCommand(RunDownLoadSync, canExecte);
                return _downloadSyncCommand;
            }
        }

        private ICommand _downloadAsyncCommand;
        public ICommand DownloadAsyncCommand
        {
            get
            {
                if (_downloadAsyncCommand == null)
                    _downloadAsyncCommand = new AsyncRelayCommand(RunDownLoadAsync, canExecte);
                return _downloadAsyncCommand;
            }
        }

        public MainWindowViewModel()
        {
            isRunning = false;
            watch = new Stopwatch();
            _strStatus = string.Empty;
        }

        private bool canExecte(object arg)
        {
            return !isRunning;
        }

        private void RunDownLoadSync(object parameter)
        {
            GeneralCommandStart();
            foreach (var site in websites)
            {
                StrStatus += DownloadWebSiteAsString.DownloadWebsite(site);
            }
            GeneralCommandEnd();
        }

        private async Task RunDownLoadAsync(object obj)
        {
            GeneralCommandStart();
            List<Task<string>> tasks = new();
            foreach (var site in websites)
            {
                tasks.Add(DownloadWebSiteAsString.DownloadWebsiteAsync(site));
            }
            StrStatus = string.Join("", await Task.WhenAll(tasks));
            GeneralCommandEnd();
        }

        private void GeneralCommandStart()
        {
            watch.Reset();
            StrStatus = string.Empty;
            isRunning = true;
            watch.Start();
        }
        private void GeneralCommandEnd()
        {
            watch.Stop();
            isRunning = false;
            StrStatus += $"Total time elapsed: {watch.Elapsed}";
        }
    }
}
