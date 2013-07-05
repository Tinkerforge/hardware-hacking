using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Tinkerforge;

namespace GarageControl
{
    public partial class MainPage : PhoneApplicationPage
    {
        private IPConnection ipcon = null;
        private BrickletIndustrialQuadRelay relay = null;
        private BackgroundWorker connectWorker = null;
		private BackgroundWorker disconnectWorker = null;
		private BackgroundWorker triggerWorker = null;
        private IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        enum ConnectResult
        {
            SUCCESS,
            NO_CONNECTION,
            NO_DEVICE
        }

        public MainPage()
        {
            InitializeComponent();

            progress.Visibility = Visibility.Collapsed;
            progress.IsIndeterminate = true;

            trigger.IsEnabled = false;

            connectWorker = new BackgroundWorker();
            connectWorker.DoWork += ConnectWorker_DoWork;
            connectWorker.RunWorkerCompleted += ConnectWorker_RunWorkerCompleted;

            disconnectWorker = new BackgroundWorker();
            disconnectWorker.DoWork += DisconnectWorker_DoWork;
			disconnectWorker.RunWorkerCompleted += DisconnectWorker_RunWorkerCompleted;

			triggerWorker = new BackgroundWorker();
			triggerWorker.DoWork += TriggerWorker_DoWork;
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
			bool connected = false;

            try
            {
                host.Text = settings["host"] as string;
                port.Text = settings["port"] as string;
				uid.Text = settings["uid"] as string;
				connected = settings["connected"].Equals(true);
            }
            catch (KeyNotFoundException)
            {
                settings["host"] = host.Text;
                settings["port"] = port.Text;
				settings["uid"] = uid.Text;
				settings["connected"] = connected;
                settings.Save();
            }

			if (connected && (ipcon == null || ipcon.GetConnectionState() == IPConnection.CONNECTION_STATE_DISCONNECTED))
			{
				Connect();
			}
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            settings["host"] = host.Text;
            settings["port"] = port.Text;
            settings["uid"] = uid.Text;

			if (ipcon != null && ipcon.GetConnectionState() == IPConnection.CONNECTION_STATE_CONNECTED)
			{
				settings["connected"] = true;
			}
			else
			{
				settings["connected"] = false;
			}

			settings.Save();
        }

        private void ConnectWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] argument = e.Argument as string[];

            ipcon = new IPConnection();
            relay = new BrickletIndustrialQuadRelay(argument[2], ipcon);

            try
            {
                ipcon.Connect(argument[0], Convert.ToInt32(argument[1]));
            }
            catch (System.IO.IOException)
            {
                e.Result = ConnectResult.NO_CONNECTION;
                return;
            }

            try
            {
                string uid;
                string connectedUid;
                char position;
                byte[] hardwareVersion;
                byte[] firmwareVersion;
                int deviceIdentifier;

                relay.GetIdentity(out uid, out connectedUid, out position, out hardwareVersion, out firmwareVersion, out deviceIdentifier);

                if (deviceIdentifier != BrickletIndustrialQuadRelay.DEVICE_IDENTIFIER)
                {
                    ipcon.Disconnect();
                    e.Result = ConnectResult.NO_DEVICE;
                    return;
                }
            }
            catch (TinkerforgeException)
            {
                try
                {
                    ipcon.Disconnect();
                }
                catch (NotConnectedException)
                {
                }

                e.Result = ConnectResult.NO_DEVICE;
                return;
            }

            e.Result = ConnectResult.SUCCESS;
        }

        private void ConnectWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ConnectResult result = (ConnectResult)e.Result;

            progress.Visibility = Visibility.Collapsed;

            if (result == ConnectResult.SUCCESS)
            {
                connect.Content = "Disconnect";
                connect.IsEnabled = true;
                trigger.IsEnabled = true;
            }
            else
            {
                MessageBoxResult retry;

                if (result == ConnectResult.NO_CONNECTION) {
                    retry = MessageBox.Show("Could not connect to " + host.Text + ":" + port.Text + ". Retry?", "Error", MessageBoxButton.OKCancel);

                } else { // ConnectResult.NO_DEVICE
                    retry = MessageBox.Show("Could not find Industrial Quad Relay Bricklet [" + uid.Text + "]. Retry?", "Error", MessageBoxButton.OKCancel);
                }

                if (retry == MessageBoxResult.OK) {
                    Connect();
                } else {
                    host.IsEnabled = true;
                    port.IsEnabled = true;
                    uid.IsEnabled = true;
                    connect.Content = "Connect";
                    connect.IsEnabled = true;
                }
            }
        }

        private void DisconnectWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                ipcon.Disconnect();
                e.Result = true;
            }
            catch (NotConnectedException)
            {
                e.Result = false;
            }
        }

        private void DisconnectWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((bool)e.Result)
            {
                host.IsEnabled = true;
                port.IsEnabled = true;
                uid.IsEnabled = true;
                connect.Content = "Connect";
            }

            connect.IsEnabled = true;
        }

		private void TriggerWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				relay.SetMonoflop(1 << 0, 1 << 0, 1500);
			}
			catch (TinkerforgeException)
			{
			}
		}

        private void Connect()
        {
            if (host.Text.Length == 0 || port.Text.Length == 0 || uid.Text.Length == 0)
            {
                MessageBox.Show("Host/Port/UID cannot be empty", "Error", MessageBoxButton.OK);
                return;
            }

            host.IsEnabled = false;
            port.IsEnabled = false;
            uid.IsEnabled = false;
            connect.IsEnabled = false;
            trigger.IsEnabled = false;

            progress.Visibility = Visibility.Visible;

            string[] argument = new string[3];

            argument[0] = host.Text;
            argument[1] = port.Text;
            argument[2] = uid.Text;

            connectWorker.RunWorkerAsync(argument);
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            if (ipcon == null || ipcon.GetConnectionState() == IPConnection.CONNECTION_STATE_DISCONNECTED)
            {
                Connect();
            }
            else
            {
                connect.IsEnabled = false;
                trigger.IsEnabled = false;

                disconnectWorker.RunWorkerAsync();
            }
        }

        private void Trigger_Click(object sender, RoutedEventArgs e)
		{
			triggerWorker.RunWorkerAsync();
        }
    }
}
