<?php

require_once('Tinkerforge/IPConnection.php');
require_once('Tinkerforge/BrickletAnalogIn.php');

use Tinkerforge\IPConnection;
use Tinkerforge\BrickletAnalogIn;

class SmokeDetector
{
	const HOST = 'localhost';
	const PORT = 4223;

	public function __construct()
    {
		$this->ipcon = new IPConnection();
		$this->brickletAnalogIn = null;

		while(true) {
			try {
				$this->ipcon->connect(self::HOST, self::PORT);
				break;
			} catch(Exception $e) {
				sleep(1);
			}
		}

		$this->ipcon->registerCallback(IPConnection::CALLBACK_ENUMERATE,
		                               array($this, 'enumerateCB'));
		$this->ipcon->registerCallback(IPConnection::CALLBACK_CONNECTED,
		                               array($this, 'connectedCB'));

		while(true) {
			try {
				$this->ipcon->enumerate();
				break;
			} catch(Exception $e) {
				sleep(1);
			}
		}
	}

	function voltageReachedCB($voltage)
	{
		echo "Fire! Fire!\n";
	}

	function enumerateCB($uid, $connectedUid, $position, $hardwareVersion,
	                     $firmwareVersion, $deviceIdentifier, $enumerationType)
	{
		if($enumerationType == IPConnection::ENUMERATION_TYPE_CONNECTED ||
		   $enumerationType == IPConnection::ENUMERATION_TYPE_AVAILABLE) {
			if($deviceIdentifier == BrickletAnalogIn::DEVICE_IDENTIFIER) {
				try {
					$this->brickletAnalogIn = new BrickletAnalogIn($uid, $this->ipcon);
					$this->brickletAnalogIn->setRange(1);
					$this->brickletAnalogIn->setDebouncePeriod(10000);
					$this->brickletAnalogIn->setVoltageCallbackThreshold('>', 1200, 0);
					$this->brickletAnalogIn->registerCallback(BrickletAnalogIn::CALLBACK_VOLTAGE_REACHED,
					                                          array($this, 'voltageReachedCB'));
					echo "Analog In initialized\n";
				} catch(Exception $e) {
					$this->brickletAnalogIn = null;
					echo "Analog In init failed: $e\n";
				}
			}
		}
	}

	function connectedCB($connectedReason)
	{
		if($connectedReason == IPConnection::CONNECT_REASON_AUTO_RECONNECT) {
			echo "Auto Reconnect\n";

			while(true) {
				try {
					$this->ipcon->enumerate();
					break;
				} catch(Exception $e) {
					sleep(1);
				}
			}
		}
	}
}

$weatherStation = new SmokeDetector();
echo "Press ctrl+c to exit\n";
$weatherStation->ipcon->dispatchCallbacks(-1);

?>
