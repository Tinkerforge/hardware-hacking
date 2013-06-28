import com.tinkerforge.BrickletIndustrialQuadRelay;
import com.tinkerforge.IPConnection;

public class RemoteSwitch {
    private static final String host = "localhost";
    private static final int port = 4223;
    private static final String UID = "ctG"; // Change to your UID
	private static final int VALUE_A_ON  = (1 << 0) | (1 << 2); // Pin 0 and 2 high
	private static final int VALUE_A_OFF = (1 << 0) | (1 << 3); // Pin 0 and 3 high
	private static final int VALUE_B_ON  = (1 << 1) | (1 << 2); // Pin 1 and 2 high
	private static final int VALUE_B_OFF = (1 << 1) | (1 << 3); // Pin 1 and 3 high
    
    // Note: To make the example code cleaner we do not handle exceptions. Exceptions you
    //       might normally want to catch are described in the documentation
    public static void main(String args[]) throws Exception {
        IPConnection ipcon = new IPConnection(); // Create IP connection
        BrickletIndustrialQuadRelay iqr = new BrickletIndustrialQuadRelay(UID, ipcon); // Create device object

        ipcon.connect(host, port); // Connect to brickd
        // Don't use device before ipcon is connected

		iqr.setMonoflop(VALUE_A_ON, 255, 1500); // Set pins to high for 1.5 seconds
    }
}
