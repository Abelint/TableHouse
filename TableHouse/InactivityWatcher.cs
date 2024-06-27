using System;
using System.Windows;
using System.Windows.Threading;

namespace TableHouse
{
    public class InactivityWatcher
    {
        private DispatcherTimer inactivityTimer;
        private TimeSpan inactivityTime;
        private Action onInactivity;

        public InactivityWatcher(TimeSpan inactivityTime, Action onInactivity)
        {
            this.inactivityTime = inactivityTime;
            this.onInactivity = onInactivity;
            inactivityTimer = new DispatcherTimer();
            inactivityTimer.Interval = inactivityTime;
            inactivityTimer.Tick += InactivityTimer_Tick;

            Application.Current.MainWindow.PreviewMouseMove += ResetInactivityTimer;
            Application.Current.MainWindow.PreviewMouseDown += ResetInactivityTimer;
            Application.Current.MainWindow.PreviewKeyDown += ResetInactivityTimer;
        }

        private void InactivityTimer_Tick(object sender, EventArgs e)
        {
            onInactivity?.Invoke();
            inactivityTimer.Stop();
        }

        private void ResetInactivityTimer(object sender, EventArgs e)
        {
            inactivityTimer.Stop();
            inactivityTimer.Start();
        }

        public void Start()
        {
            inactivityTimer.Start();
        }

        public void Stop()
        {
            inactivityTimer.Stop();
        }
    }
}
