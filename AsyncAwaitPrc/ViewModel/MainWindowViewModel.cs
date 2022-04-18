﻿using AsyncAwaitPrc.Commands;
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

        private Progress<ProgressReportModel> _progress = new Progress<ProgressReportModel>();

        Stopwatch watch;
        private bool _isRunning;

        private int _percentageComplete;
        public int PercentageComplete
        {
            get { return _percentageComplete; }
            set
            {
                _percentageComplete = value;
                NotifyPropertyChanged();
            }
        }

        private string _strStatus;
        public string StrStatus
        {
            get { return _strStatus.ToString(); }
            set
            {
                _strStatus = value;
                NotifyPropertyChanged();
            }
        }

        private ICommand _downloadSyncCommand;
        public ICommand DownloadSyncCommand
        {
            get
            {
                if (_downloadSyncCommand == null)
                    _downloadSyncCommand = new RelayCommand(RunDownLoadSync, canDownload);
                return _downloadSyncCommand;
            }
        }

        private ICommand _downloadAsyncCommand;
        public ICommand DownloadAsyncCommand
        {
            get
            {
                if (_downloadAsyncCommand == null)
                    _downloadAsyncCommand = new AsyncRelayCommand(RunDownLoadAsync, canDownload);
                return _downloadAsyncCommand;
            }
        }

        private ICommand _downloadParalleAsyncCommand;
        public ICommand DownloadParalleAsyncCommand
        {
            get
            {
                if (_downloadParalleAsyncCommand == null)
                    _downloadParalleAsyncCommand = new AsyncRelayCommand(RunDownLoadParalleAsync, canDownload);
                return _downloadParalleAsyncCommand;
            }
        }

        private ICommand _cancelDownloadCommand;
        public ICommand CancelDownloadCommand
        {
            get
            {
                if (_cancelDownloadCommand == null)
                    _cancelDownloadCommand = new RelayCommand(CancelDownload, canCancelDownload);
                return _cancelDownloadCommand;
            }
        }

        private void CancelDownload(object obj)
        {
            throw new NotImplementedException();
        }

        public MainWindowViewModel()
        {
            _isRunning = false;
            watch = new Stopwatch();
            StrStatus = "";
            _progress.ProgressChanged += ReportProgress;
        }

        private void ReportProgress(object sender, ProgressReportModel e)
        {
            PercentageComplete = e.PercentageComplete;
            StrStatus = string.Join(Environment.NewLine, e.ProgressStatus);
        }

        private bool canDownload(object arg) => !_isRunning;

        private bool canCancelDownload(object arg) => _isRunning;

        private void RunDownLoadSync(object parameter)
        {
            IProgress<ProgressReportModel> progress = _progress;
            ProgressReportModel progressReport = new();
            GeneralCommandStart();
            foreach (var site in websites)
            {
                var result = DownloadWebSiteAsString.DownloadWebsite(site);

                progressReport.ProgressStatus.Add(result);
                progressReport.PercentageComplete = (progressReport.ProgressStatus.Count * 100) / websites.Count;

                progress.Report(progressReport);
            }
            GeneralCommandEnd(progressReport);
        }

        private async Task RunDownLoadAsync(object parameter)
        {
            IProgress<ProgressReportModel> progress = _progress;
            ProgressReportModel progressReport = new();
            GeneralCommandStart();
            foreach (var site in websites)
            {
                var result = await DownloadWebSiteAsString.DownloadWebsiteAsync(site);

                progressReport.ProgressStatus.Add(result);
                progressReport.PercentageComplete = (progressReport.ProgressStatus.Count * 100) / websites.Count;

                progress.Report(progressReport);
            }
            GeneralCommandEnd(progressReport);
        }

        private async Task RunDownLoadParalleAsync(object parameter)
        {
            IProgress<ProgressReportModel> progress = _progress;
            ProgressReportModel progressReport = new();
            GeneralCommandStart();
            List<Task<string>> tasks = new();
            foreach (var site in websites)
            {
                tasks.Add(DownloadWebSiteAsString.DownloadWebsiteAsync(site));
            }
            progressReport.ProgressStatus = (await Task.WhenAll(tasks)).ToList();
            progressReport.PercentageComplete = (progressReport.ProgressStatus.Count * 100) / websites.Count;

            GeneralCommandEnd(progressReport);
        }

        private void GeneralCommandStart()
        {
            watch.Reset();
            StrStatus = "";
            _isRunning = true;
            watch.Start();
        }
        private void GeneralCommandEnd(ProgressReportModel progressReport)
        {
            watch.Stop();
            _isRunning = false;
            progressReport.ProgressStatus.Add($"Total time elapsed: {watch.Elapsed}");
            (_progress as IProgress<ProgressReportModel>).Report(progressReport);
        }
    }
}
