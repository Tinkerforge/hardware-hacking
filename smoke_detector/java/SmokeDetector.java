import com.tinkerforge.IPConnection;
import com.tinkerforge.BrickletIndustrialDigitalIn4;

class SmokeListener implements IPConnection.EnumerateListener,
                               IPConnection.ConnectedListener,
                               BrickletIndustrialDigitalIn4.InterruptListener {
	private IPConnection ipcon = null;
	private BrickletIndustrialDigitalIn4 brickletIndustrialDigitalIn4 = null;

	public SmokeListener(IPConnection ipcon) {
		this.ipcon = ipcon;
	}

	public void interrupt(int interruptMask, int valueMask) {
		if(valueMask > 0) {
			System.out.println("Fire! Fire!");
		}
	}

	public void enumerate(String uid, String connectedUid, char position,
	                      short[] hardwareVersion, short[] firmwareVersion,
	                      int deviceIdentifier, short enumerationType) {
		if(enumerationType == IPConnection.ENUMERATION_TYPE_CONNECTED ||
		   enumerationType == IPConnection.ENUMERATION_TYPE_AVAILABLE) {
			if(deviceIdentifier == BrickletIndustrialDigitalIn4.DEVICE_IDENTIFIER) {
				try {
					brickletIndustrialDigitalIn4 = new BrickletIndustrialDigitalIn4(uid, ipcon);
					brickletIndustrialDigitalIn4.setDebouncePeriod(10000);
					brickletIndustrialDigitalIn4.setInterrupt(15);
					brickletIndustrialDigitalIn4.addInterruptListener(this);
					System.out.println("Industrial Digital In 4 initialized");
				} catch(com.tinkerforge.TinkerforgeException e) {
					brickletIndustrialDigitalIn4 = null;
					System.out.println("Industrial Digital In 4 init failed: " + e);
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
	private static final String HOST = "localhost";
	private static final int PORT = 4223;
	private static IPConnection ipcon = null;
	private static SmokeListener smokeListener = null;

	public static void main(String args[]) {
		ipcon = new IPConnection();

		while(true) {
			try {
				ipcon.connect(HOST, PORT);
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

		try {
			System.out.println("Press key to exit"); System.in.read();
		} catch(java.io.IOException e) {
		}

		try {
			ipcon.disconnect();
		} catch(com.tinkerforge.NotConnectedException e) {
		}
	}
}
