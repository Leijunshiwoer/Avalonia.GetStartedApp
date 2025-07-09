using Avalonia.Threading;
using GetStartedApp.Utils;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ursa.Controls;
using Velopack;
using System.IO;
using Velopack.Logging;

namespace GetStartedApp.ViewModels.ProgramPack
{
    public class ProgramPackViewModel : ViewModelBase
    {
        private UpdateManager _um;
        private UpdateInfo _update;
        public ProgramPackViewModel()
        {

            var updateUrl = Path.Combine( AppDomain.CurrentDomain.BaseDirectory,"win-x64");
            if (!Directory.Exists(updateUrl))
            {
                Directory.CreateDirectory(updateUrl);
            }
            // replace with your update path/url
            _um = new UpdateManager(updateUrl);

            TextLog = Program.Log.ToString();

            Program.Log.LogUpdated += LogUpdated;
            UpdateStatus();
        }

        #region 属性

        private string _TextStatus;
        public string TextStatus
        {
            get { return _TextStatus; }
            set { SetProperty(ref _TextStatus, value); }
        }

        private string _TextLog;
        public string TextLog
        {
            get { return _TextLog; }
            set { SetProperty(ref _TextLog, value); }
        }


        private bool _IsEnabled01;
        public bool IsEnabled01
        {
            get { return _IsEnabled01; }
            set { SetProperty(ref _IsEnabled01, value); }
        }

        private bool _IsEnabled02;
        public bool IsEnabled02
        {
            get { return _IsEnabled02; }
            set { SetProperty(ref _IsEnabled02, value); }
        }

        private bool _IsEnabled03;
        public bool IsEnabled03
        {
            get { return _IsEnabled03; }
            set { SetProperty(ref _IsEnabled03, value); }
        }

        private bool _isScrollToEnd;
        public bool IsScrollToEnd
        {
            get => _isScrollToEnd;
            set => SetProperty(ref _isScrollToEnd, value);
        }
        #endregion
        #region  事件
        private DelegateCommand _CheckUpdateCmd;
        public DelegateCommand CheckUpdateCmd =>
            _CheckUpdateCmd ?? (_CheckUpdateCmd = new DelegateCommand(ExecuteCheckUpdateCmd));

       async void ExecuteCheckUpdateCmd()
        {
            Working();
            try
            {
                // ConfigureAwait(true) so that UpdateStatus() is called on the UI thread
                _update = await _um.CheckForUpdatesAsync().ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                Program.Log.LogError(ex, "检查更新时发生错误");
            }

            UpdateStatus();
        }


        private DelegateCommand _DownloadUpdateCmd;
        public DelegateCommand DownloadUpdateCmd =>
            _DownloadUpdateCmd ?? (_DownloadUpdateCmd = new DelegateCommand(ExecuteDownloadUpdateCmd));

        async void ExecuteDownloadUpdateCmd()
        {
            Working();
            try
            {
                // ConfigureAwait(true) so that UpdateStatus() is called on the UI thread
                await _um.DownloadUpdatesAsync(_update, Progress).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                Program.Log.LogError(ex, "下载更新时发生错误");
            }

            UpdateStatus();
        }

        private DelegateCommand _RestartApplyCmd;
        public DelegateCommand RestartApplyCmd =>
            _RestartApplyCmd ?? (_RestartApplyCmd = new DelegateCommand(ExecuteRestartApplyCmd));

        void ExecuteRestartApplyCmd()
        {
            _um.ApplyUpdatesAndRestart(_update);
        }
        #endregion


        #region 方法

        private void LogUpdated(object sender, LogUpdatedEventArgs e)
        {
            // logs can be sent from other threads
            Dispatcher.UIThread.InvokeAsync(
                () => {
                    TextLog = e.Text;
                    IsScrollToEnd = true;

                });
        }

        private void Progress(int percent)
        {
            // progress can be sent from other threads
            Dispatcher.UIThread.InvokeAsync(
                () =>
                {
                    TextStatus = $"正在下载 ({percent}%)...";
                });
        }

        private void Working()
        {
            Program.Log.LogInformation("");
            IsEnabled01 = false;
            IsEnabled02 = false;
            IsEnabled03 = false;
            TextStatus = "正在处理...";
        }

        private void UpdateStatus()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Velopack 版本: {VelopackRuntimeInfo.VelopackNugetVersion}");
            sb.AppendLine($"当前应用版本: {(_um.IsInstalled ? _um.CurrentVersion : "(未安装)")}");

            if (_update != null)
            {
                sb.AppendLine($"发现新版本: {_update.TargetFullRelease.Version}");
                IsEnabled02 = true;
            }
            else
            {
                IsEnabled02 = false;
            }

            if (_um.UpdatePendingRestart != null)
            {
                sb.AppendLine("更新已就绪，重启后生效");
                IsEnabled03= true;
            }
            else
            {
                IsEnabled03 = false;
            }


            TextStatus = sb.ToString();
            IsEnabled01 = true;
        }
        #endregion
    }
}
