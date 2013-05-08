using Tinkerforge;

class SmokeDetector
{
	private static string HOST = "localhost";
	private static int PORT = 4223;

	private static IPConnection ipcon = null;
	private static BrickletAnalogIn brickletAnalogIn = null;

	static void VoltageReachedCB(BrickletAnalogIn sender, int voltage)
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
			if(deviceIdentifier == BrickletAnalogIn.DEVICE_IDENTIFIER)
			{
				try
				{
					brickletAnalogIn = new BrickletAnalogIn(UID, ipcon);
					brickletAnalogIn.SetRange(1);
					brickletAnalogIn.SetDebouncePeriod(10000);
					brickletAnalogIn.SetVoltageCallbackThreshold('>', 1200, 0);
					brickletAnalogIn.VoltageReached += VoltageReachedCB;
					System.Console.WriteLine("Analog In initialized");
				}
				catch(TinkerforgeException e)
				{
					System.Console.WriteLine("Analog In init failed: " + e.Message);
					brickletAnalogIn = null;
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

		System.Console.WriteLine("Press key to exit");
		System.Console.ReadKey();
		ipcon.Disconnect();
	}
}
