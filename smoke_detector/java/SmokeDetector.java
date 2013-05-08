import com.tinkerforge.IPConnection;
import com.tinkerforge.BrickletAnalogIn;

class SmokeListener implements IPConnection.EnumerateListener,
                               IPConnection.ConnectedListener,
                               BrickletAnalogIn.VoltageReachedListener {
	private IPConnection ipcon = null;
	private BrickletAnalogIn brickletAnalogIn = null;

	public SmokeListener(IPConnection ipcon) {
		this.ipcon = ipcon;
	}

	public void voltageReached(int illuminance) {
		System.out.println("Fire! Fire!");
	}

	public void enumerate(String uid, String connectedUid, char position,
	                      short[] hardwareVersion, short[] firmwareVersion,
	                      int deviceIdentifier, short enumerationType) {
		if(enumerationType == IPConnection.ENUMERATION_TYPE_CONNECTED ||
		   enumerationType == IPConnection.ENUMERATION_TYPE_AVAILABLE) {
			if(deviceIdentifier == BrickletAnalogIn.DEVICE_IDENTIFIER) {
				try {
					brickletAnalogIn = new BrickletAnalogIn(uid, ipcon);
					brickletAnalogIn.setRange((short)1);
					brickletAnalogIn.setDebouncePeriod(10000);
					brickletAnalogIn.setVoltageCallbackThreshold('>', (short)1200, (short)0);
					brickletAnalogIn.addVoltageReachedListener(this);
					System.out.println("Analog In initialized");
				} catch(com.tinkerforge.TinkerforgeException e) {
					brickletAnalogIn = null;
					System.out.println("Analog In init failed: " + e);
				}
			}
		}
	}

	public void connected(short connectedReason) {
		if(connectedReason == IPConnection.CONNECT_REASON_AUTO_RECONNECT) {
			System.out.println("Auto Reconnect");

			while(true) {
				try {
					ipcon.enumerate();
					break;
				} catch(com.tinkerforge.NotConnectedException e) {
				}

				try {
					Thread.sleep(1000);
				} catch(InterruptedException ei) {
				}
			}
		}
	}
}

public class SmokeDetector {
	private static final String host = "localhost";
	private static final int port = 4223;
	private static IPConnection ipcon = null;
	private static SmokeListener smokeListener = null;

	public static void main(String args[]) {
		ipcon = new IPConnection();

		while(true) {
			try {
				ipcon.connect(host, port);
				break;
			} catch(java.net.UnknownHostException e) {
			} catch(java.io.IOException e) {
			} catch(com.tinkerforge.AlreadyConnectedException e) {
			}

			try {
				Thread.sleep(1000);
			} catch(InterruptedException ei) {
			}
		}

		smokeListener = new SmokeListener(ipcon);
		ipcon.addEnumerateListener(smokeListener);
		ipcon.addConnectedListener(smokeListener);

		while(true) {
			try {
				ipcon.enumerate();
				break;
			} catch(com.tinkerforge.NotConnectedException e) {
			}

			try {
				Thread.sleep(1000);
			} catch(InterruptedException ei) {
			}
		}

		System.console().readLine("Press key to exit\n");
		try {
			ipcon.disconnect();
		} catch(com.tinkerforge.NotConnectedException e) {
		}
	}
}
