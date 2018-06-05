using Tinkerforge;

class SmokeDetectorV2
{
	private static string HOST = "localhost";
	private static int PORT = 4223;

	private static IPConnection ipcon = null;
	private static BrickletIndustrialDigitalIn4V2 brickletIndustrialDigitalIn4V2 = null;

	static void InterruptCB(BrickletIndustrialDigitalIn4V2 sender, bool[] changed, bool[] value)
	{
		System.Console.WriteLine("Fire! Fire!");
	}

	static void EnumerateCB(IPConnection sender, string UID, string connectedUID, char position,
	                        short[] hardwareVersion, short[] firmwareVersion,
	                        int deviceIdentifier, short enumerationType)
	{
		if(enumerationType == IPConnection.ENUMERATION_TYPE_CONNECTED ||
		   enumerationType == IPConnection.ENUMERATION_TYPE_AVAILABLE)
		{
			if(deviceIdentifier == BrickletIndustrialDigitalIn4V2.DEVICE_IDENTIFIER)
			{
				try
				{
					brickletIndustrialDigitalIn4V2 = new BrickletIndustrialDigitalIn4V2(UID, ipcon);
					brickletIndustrialDigitalIn4V2.SetAllValueCallbackConfiguration(10000, true);
					brickletIndustrialDigitalIn4V2.AllValueCallback += InterruptCB;
					System.Console.WriteLine("Industrial Digital In 4 V2 initialized");
				}
				catch(TinkerforgeException e)
				{
					System.Console.WriteLine("Industrial Digital In 4 V2 init failed: " + e.Message);
					brickletIndustrialDigitalIn4V2 = null;
				}
			}
		}
	}

	static void ConnectedCB(IPConnection sender, short connectedReason)
	{
		if(connectedReason == IPConnection.CONNECT_REASON_AUTO_RECONNECT)
		{
			System.Console.WriteLine("Auto Reconnect");

			while(true)
			{
				try
				{
					ipcon.Enumerate();
					break;
				}
				catch(NotConnectedException e)
				{
					System.Console.WriteLine("Enumeration Error: " + e.Message);
					System.Threading.Thread.Sleep(1000);
				}
			}
		}
	}

	static void Main()
	{
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
				System.Console.WriteLine("Connection Error: " + e.Message);
				System.Threading.Thread.Sleep(1000);
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
				System.Console.WriteLine("Enumeration Error: " + e.Message);
				System.Threading.Thread.Sleep(1000);
			}
		}

		System.Console.WriteLine("Press enter to exit");
		System.Console.ReadLine();
		ipcon.Disconnect();
	}
}
