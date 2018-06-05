import com.tinkerforge.IPConnection;
import com.tinkerforge.BrickletIndustrialQuadRelayV2;
import com.tinkerforge.TimeoutException;
import com.tinkerforge.NotConnectedException;

public class RemoteSwitchV2 {
	private static final String HOST = "localhost";
	private static final int PORT = 4223;
	private static final String UID = "2g2gRs"; // Change to your UID

	private static void a_on(BrickletIndustrialQuadRelayV2 iqr) throws TimeoutException, NotConnectedException {
		// Close channels 0 and 2 for 1.5 seconds
		iqr.setMonoflop(0, true, 1500);
		iqr.setMonoflop(2, true, 1500);
	}

	private static void a_off(BrickletIndustrialQuadRelayV2 iqr) throws TimeoutException, NotConnectedException  {
		// Close channels 0 and 3 for 1.5 seconds
		iqr.setMonoflop(0, true, 1500);
		iqr.setMonoflop(3, true, 1500);
	}

	private static void b_on(BrickletIndustrialQuadRelayV2 iqr) throws TimeoutException, NotConnectedException  {
		// Close channels 1 and 2 for 1.5 seconds
		iqr.setMonoflop(1, true, 1500);
		iqr.setMonoflop(2, true, 1500);
	}

	private static void b_off(BrickletIndustrialQuadRelayV2 iqr) throws TimeoutException, NotConnectedException  {
		// Close channels 1 and 3 for 1.5 seconds
		iqr.setMonoflop(1, true, 1500);
		iqr.setMonoflop(3, true, 1500);
	}

	// Note: To make the example code cleaner we do not handle exceptions. Exceptions you
	//       might normally want to catch are described in the documentation
	public static void main(String args[]) throws Exception {
		IPConnection ipcon = new IPConnection(); // Create IP connection
		BrickletIndustrialQuadRelayV2 iqr = new BrickletIndustrialQuadRelayV2(UID, ipcon); // Create device object

		ipcon.connect(HOST, PORT); // Connect to brickd
		// Don't use device before ipcon is connected

		a_on(iqr);

		ipcon.disconnect();
	}
}
