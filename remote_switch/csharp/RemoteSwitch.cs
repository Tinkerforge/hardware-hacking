using Tinkerforge;

class Example
{
    private static string HOST = "localhost";
    private static int PORT = 4223;
    private static string UID = "ctG"; // Change to your UID
	private static int VALUE_A_ON  = (1 << 0) | (1 << 2); // Pin 0 and 2 high
	private static int VALUE_A_OFF = (1 << 0) | (1 << 3); // Pin 0 and 3 high
	private static int VALUE_B_ON  = (1 << 1) | (1 << 2); // Pin 1 and 2 high
	private static int VALUE_B_OFF = (1 << 1) | (1 << 3); // Pin 1 and 3 high

    static void Main() 
    {
        IPConnection ipcon = new IPConnection(); // Create IP connection
        BrickletIndustrialQuadRelay iqr = new BrickletIndustrialQuadRelay(UID, ipcon); // Create device object

        ipcon.Connect(HOST, PORT); // Connect to brickd
        // Don't use device before ipcon is connected

		iqr.SetMonoflop(VALUE_A_ON, 255, 1500); // Set pins to high for 1.5 seconds
    }
}
