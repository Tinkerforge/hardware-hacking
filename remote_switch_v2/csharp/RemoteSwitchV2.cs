using Tinkerforge;

class RemoteSwitchV2
{
	private static string HOST = "localhost";
	private static int PORT = 4223;
	private static string UID = "2g2gRs"; // Change to your UID

	static void AOn(BrickletIndustrialQuadRelayV2 iqr) {
		// Close channels 0 and 2 for 1.5 seconds
		iqr.SetMonoflop(0, true, 1500);
		iqr.SetMonoflop(2, true, 1500);
	}

	static void AOff(BrickletIndustrialQuadRelayV2 iqr) {
		// Close channels 0 and 3 for 1.5 seconds
		iqr.SetMonoflop(0, true, 1500);
		iqr.SetMonoflop(3, true, 1500);
	}

	static void BOn(BrickletIndustrialQuadRelayV2 iqr) {
		// Close channels 1 and 2 for 1.5 seconds
		iqr.SetMonoflop(1, true, 1500);
		iqr.SetMonoflop(2, true, 1500);
	}

	static void BOff(BrickletIndustrialQuadRelayV2 iqr) {
		// Close channels 1 and 3 for 1.5 seconds
		iqr.SetMonoflop(1, true, 1500);
		iqr.SetMonoflop(3, true, 1500);
	}

	static void Main()
	{
		IPConnection ipcon = new IPConnection(); // Create IP connection
		BrickletIndustrialQuadRelayV2 iqr = new BrickletIndustrialQuadRelayV2(UID, ipcon); // Create device object

		ipcon.Connect(HOST, PORT); // Connect to brickd
		// Don't use device before ipcon is connected

		AOn(iqr);

		ipcon.Disconnect();
	}
}
