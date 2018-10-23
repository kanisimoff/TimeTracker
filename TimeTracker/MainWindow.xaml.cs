using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Newtonsoft.Json;
using TimeTracker.Hooks;
using TimeTracker.Logic;
using TimeTracker.Logic.Models;

namespace TimeTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private KeyboardInput keyboard;
        private MouseInput mouse;

        private DispatcherTimer saveTimer;
        private DispatcherTimer uiTimer;

        private WorkTimeNote currentWorkTime;
        private LogService _logService;

        private bool isUserActiveInOs = true;

        public MainWindow()
        {
            InitializeComponent();
            InitNotifyIcon();
                        
            keyboard = new KeyboardInput();
            keyboard.KeyBoardKeyPressed += keyboard_KeyBoardKeyPressed;

            mouse = new MouseInput();
            mouse.MouseMoved += mouse_MouseMoved;

            //lastInput = new AllInputSources();
            InitSaveTimer();
            InitUiTimer();
            _logService = new LogService();

            SystemEvents.SessionSwitch += SystemEvents_SessionSwitch1;


            currentWorkTime = _logService.CurrentDay;
        }

        private void SystemEvents_SessionSwitch1(object sender, SessionSwitchEventArgs e)
        {
            switch (e.Reason)
            {
                case SessionSwitchReason.RemoteConnect:
                    isUserActiveInOs = true;
                    break;
                case SessionSwitchReason.RemoteDisconnect:
                    isUserActiveInOs = false;
                    break;
                case SessionSwitchReason.SessionLogon:
                    isUserActiveInOs = true;
                    break;
                case SessionSwitchReason.SessionLogoff:
                    isUserActiveInOs = false;
                    break;
                case SessionSwitchReason.SessionLock:
                    isUserActiveInOs = false;
                    break;
                case SessionSwitchReason.SessionUnlock:
                    isUserActiveInOs = true;
                    break;
            }
        }

        void keyboard_KeyBoardKeyPressed(object sender, EventArgs e)
        {
            if (isUserActiveInOs)
                currentWorkTime.EndDateTime = DateTime.Now;
        }

        void mouse_MouseMoved(object sender, EventArgs e)
        {
            if (isUserActiveInOs)
                currentWorkTime.EndDateTime = DateTime.Now;
        }

        #region Таймеры
        private void InitSaveTimer()
        {
            saveTimer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1, 0, 0)
            };
            saveTimer.Tick += saveTimer_Tick;
            saveTimer.Start();
        }

        void saveTimer_Tick(object sender, EventArgs e)
        {
            var logWorkTime = _logService.LoadLog();
            var date = currentWorkTime.BeginDateTime.Date;
            if (logWorkTime.LogNotes.ContainsKey(date))
            {
                logWorkTime.LogNotes[date] = currentWorkTime;
            }
            else
            {
                logWorkTime.LogNotes.Add(date, currentWorkTime);
            }
            _logService.SaveLog(logWorkTime);
        }

        private void InitUiTimer()
        {
            uiTimer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 1, 0)
            };
            uiTimer.Tick += UiTimer_Tick;
            uiTimer.Start();
        }

        void UiTimer_Tick(object sender, EventArgs e)
        {
            currentDay.Content = currentWorkTime.BeginDateTime.ToString("dd-MM-yyyy", CultureInfo.CurrentUICulture);

            beginWorkTimeLabel.Content = currentWorkTime.BeginDateTime.ToLocalTime()
                .ToString("HH:mm:ss", CultureInfo.CurrentUICulture);

            durationWorkTime.Content = currentWorkTime.Duration.HoursFormat();
            leftDayLabel.Content = currentWorkTime.CalculateDayWorkTime < TimeSpan.Zero ? "Переработал:" : "Осталось:";
            leftWorkTime.Content = currentWorkTime.CalculateDayWorkTime.HoursFormat();

            durationWeekWorkTime.Content = _logService.Log.DurationCurrentWeek.HoursFormat();
            leftWeekLabel.Content = _logService.Log.LeftCurrentWeek < TimeSpan.Zero ? "Переработал:" : "Осталось:";
            leftWeekWorkTime.Content = _logService.Log.LeftCurrentWeek.HoursFormat();

            durationMonthWorkTime.Content =  _logService.Log.DurationCurrentMonth.HoursFormat();
            leftMonthLabel.Content = _logService.Log.LeftCurrentMonth < TimeSpan.Zero ? "Переработал:" : "Осталось:";
            leftMonthWorkTime.Content = _logService.Log.LeftCurrentMonth.HoursFormat();


            m_notifyIcon.Text =
                $"Отработано: {currentWorkTime.Duration.HoursFormat()} \n Осталось: {currentWorkTime.CalculateDayWorkTime.HoursFormat()}";
        }

        #endregion

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;
        }
    }
}
