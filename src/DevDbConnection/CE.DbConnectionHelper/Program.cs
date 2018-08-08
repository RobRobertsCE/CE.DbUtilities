using CE.DbConnectionHelper.Properties;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace CE.DbConnectionHelper
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MyCustomApplicationContext());
            // Application.Run(new SecurityGroupSelectionView());
        }

        public class MyCustomApplicationContext : ApplicationContext
        {
            private NotifyIcon trayIcon;
            private frmDatabaseConnections _dbConnectionsForm;
            private frmDatabaseActivities _dbActivitiesForm;

            public MyCustomApplicationContext()
            {
                // Initialize Tray Icon
                trayIcon = new NotifyIcon()
                {
                    Icon = Resources.AppIcon,
                    ContextMenu = new ContextMenu(),
                    Visible = true
                };

                trayIcon.MouseUp += TrayIcon_MouseUp;

                trayIcon.ContextMenu.MenuItems.Add("PFS Conections", ShowDbConnectionsForm);
                trayIcon.ContextMenu.MenuItems.Add("DB Utilities", ShowDbActivitiesForm);
                trayIcon.ContextMenu.MenuItems.Add("-");
                trayIcon.ContextMenu.MenuItems.Add("Exit", Exit);

                Application.ApplicationExit += Application_ApplicationExit;
            }

            private void Application_ApplicationExit(object sender, EventArgs e)
            {
                trayIcon.Dispose();
            }

            private void TrayIcon_MouseUp(object sender, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left)
                {
                    MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                    mi.Invoke(trayIcon, null);
                }
            }

            void Exit(object sender, EventArgs e)
            {
                // Hide tray icon, otherwise it will remain shown until user mouses over it
                trayIcon.MouseUp -= TrayIcon_MouseUp;
                if (_dbConnectionsForm != null)
                    _dbConnectionsForm.Dispose();

                if (_dbActivitiesForm != null)
                    _dbActivitiesForm.Dispose();

                if (trayIcon != null)
                {
                    trayIcon.Visible = false;
                    trayIcon.Dispose();
                }

                Application.Exit();
            }

            void ShowDbConnectionsForm(object sender, EventArgs e)
            {
                if (_dbConnectionsForm == null)
                    _dbConnectionsForm = new frmDatabaseConnections();

                _dbConnectionsForm.Show();
            }

            void ShowDbActivitiesForm(object sender, EventArgs e)
            {
                if (_dbActivitiesForm == null || _dbActivitiesForm.IsDisposed)
                    _dbActivitiesForm = new frmDatabaseActivities();

                _dbActivitiesForm.Show();
            }
        }
    }
}
