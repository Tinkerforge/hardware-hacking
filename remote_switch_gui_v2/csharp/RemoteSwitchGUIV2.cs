using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using Tinkerforge;

class RemoteSwitchGUI : Form
{
	private static string HOST = "localhost";
	private static int PORT = 4223;

	private static int A_ON = 0;
	private static int A_OFF = 1;
	private static int B_ON = 2;
	private static int B_OFF = 3;

	private Panel panel = null;
	private Button buttonAOn = null;
	private Button buttonAOff = null;
	private Button buttonBOn = null;
	private Button buttonBOff = null;
	private ListBox listBox = null;

	private IPConnection ipcon = null;
	private BrickletIndustrialQuadRelayV2 brickletIndustrialQuadRelayV2 = null;

	public RemoteSwitchGUI()
	{
		Text = "Remote Switch GUI 1.0.1";
		Size = new Size(300, 500);
		MinimumSize = new Size(260, 200);

		panel = new Panel();
		panel.Parent = this;
		panel.Height = 40;
		panel.Dock = DockStyle.Top;

		listBox = new ListBox();
		listBox.Parent = this;
		listBox.Dock = DockStyle.Fill;
		listBox.BringToFront();

		buttonAOn  = CreateButton("A On",   10, A_ON);
		buttonAOff = CreateButton("A Off",  70, A_OFF);
		buttonBOn  = CreateButton("B On",  130, B_ON);
		buttonBOff = CreateButton("B Off", 190, B_OFF);

		Load += delegate(object sender, System.EventArgs e)
		{
			Thread thread = new Thread(delegate() { Connect(); });
			thread.IsBackground = true;
			thread.Start();
		};
	}

	private Button CreateButton(string name, int x, int selectionMask)
	{
		Button button = new Button();

		button.Text = name;
		button.Parent = panel;
		button.Width = 50;
		button.Location = new Point(x, 10);

		button.Click += delegate(object sender, System.EventArgs e)
		{
			TriggerSwitch(name, selectionMask);
		};

		return button;
	}

	private void Log(string message)
	{
		Invoke((MethodInvoker) delegate() { listBox.Items.Add(message); });
	}

	private void Connect()
	{
		Log("Connecting to " + HOST + ":" + PORT);

		ipcon = new IPConnection();
		while(true)
		{
			try
			{
				ipcon.Connect(HOST, PORT);
				break;
			}
			catch(System.Net.Sockets.SocketException e)
			{
				Log("Connection Error: " + e.Message);
				Thread.Sleep(1000);
			}
		}

		ipcon.EnumerateCallback += EnumerateCB;
		ipcon.Connected += ConnectedCB;

		while(true)
		{
			try
			{
				ipcon.Enumerate();
				break;
			}
			catch(NotConnectedException e)
			{
				Log("Enumeration Error: " + e.Message);
				Thread.Sleep(1000);
			}
		}

		Log("Connected");
	}

	private void TriggerSwitch(string name, int selectionMask)
	{
		if(brickletIndustrialQuadRelayV2 == null) {
			Log("No Industrial Quad Relay V2 Bricklet found");
			return;
		}

		try
		{
			if (selectionMask == A_ON) {
				// Channel 0 and 2 are closed for 500 ms
				brickletIndustrialQuadRelayV2.SetMonoflop(0, true, 500);
				brickletIndustrialQuadRelayV2.SetMonoflop(2, true, 500);
			}
			else if (selectionMask == A_OFF) {
				// Channel 0 and 3 are closed for 500 ms
				brickletIndustrialQuadRelayV2.SetMonoflop(0, true, 500);
				brickletIndustrialQuadRelayV2.SetMonoflop(3, true, 500);
			}
			else if (selectionMask == B_ON) {
				// Channel 1 and 2 are closed for 500 ms
				brickletIndustrialQuadRelayV2.SetMonoflop(1, true, 500);
				brickletIndustrialQuadRelayV2.SetMonoflop(2, true, 500);
			}
			else if (selectionMask == B_OFF) {
				// Channel 1 and 3 are closed for 500 ms
				brickletIndustrialQuadRelayV2.SetMonoflop(1, true, 500);
				brickletIndustrialQuadRelayV2.SetMonoflop(3, true, 500);
			}

			Log("Triggered '" + name + "'");
		}
		catch(TinkerforgeException e)
		{
			Log("Trigger '" + name + "' Error: " + e.Message);
		}
	}

	private void EnumerateCB(IPConnection sender, string UID, string connectedUID, char position,
	                         short[] hardwareVersion, short[] firmwareVersion,
	                         int deviceIdentifier, short enumerationType)
	{
		if(enumerationType == IPConnection.ENUMERATION_TYPE_CONNECTED ||
		   enumerationType == IPConnection.ENUMERATION_TYPE_AVAILABLE)
		{
			if(deviceIdentifier == BrickletIndustrialQuadRelayV2.DEVICE_IDENTIFIER)
			{
				try
				{
					brickletIndustrialQuadRelayV2 = new BrickletIndustrialQuadRelayV2(UID, ipcon);
					Log("Industrial Quad Relay V2 initialized");
				}
				catch(TinkerforgeException e)
				{
					Log("Industrial Quad Relay V2 init failed: " + e.Message);
					brickletIndustrialQuadRelayV2 = null;
				}
			}
		}
	}

	private void ConnectedCB(IPConnection sender, short connectedReason)
	{
		if(connectedReason == IPConnection.CONNECT_REASON_AUTO_RECONNECT)
		{
			Log("Auto Reconnect");

			while(true)
			{
				try
				{
					ipcon.Enumerate();
					break;
				}
				catch(NotConnectedException e)
				{
					Log("Enumeration Error: " + e.Message);
					Thread.Sleep(1000);
				}
			}
		}
	}

	static public void Main()
	{
		Application.Run(new RemoteSwitchGUI());
	}
}
