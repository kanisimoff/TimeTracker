using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using TimeTracker.Logic;

namespace TimeTracker
{
    /// <summary>
    /// Systray logic
    /// </summary>
    public partial class MainWindow
    {
        private System.Windows.Forms.NotifyIcon m_notifyIcon;
        private WindowState m_storedWindowState = WindowState.Normal;

        private void InitNotifyIcon()
        {
            // initialise code here
            m_notifyIcon = new System.Windows.Forms.NotifyIcon
            {
                BalloonTipText = "Приложение был минимизировано. Чтобы восстановить приложение щелкните по иконке.",
                BalloonTipTitle = Title,
                Text = $"Отработано: {currentWorkTime.Duration.HoursFormat()} \n Осталось: {currentWorkTime.CalculateDayWorkTime.HoursFormat()}"
            };

            var iconStream = Application.GetResourceStream(new Uri("pack://application:,,,/Resources/clock.ico", UriKind.RelativeOrAbsolute))?.Stream;
            if (iconStream != null)
                m_notifyIcon.Icon = new System.Drawing.Icon(iconStream);

            m_notifyIcon.Click += m_notifyIcon_Click;
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            m_notifyIcon.Dispose();
            m_notifyIcon = null;
        }

        private void MainWindow_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            CheckTrayIcon();
        }

        private void MainWindow_OnStateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
                m_notifyIcon?.ShowBalloonTip(2000);
            }
            else
                m_storedWindowState = WindowState;
        }

        void m_notifyIcon_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = m_storedWindowState;
        }
        void CheckTrayIcon()
        {
            ShowTrayIcon(!IsVisible);
        }

        void ShowTrayIcon(bool show)
        {
            if (m_notifyIcon != null)
                m_notifyIcon.Visible = show;
        }
    }
}
