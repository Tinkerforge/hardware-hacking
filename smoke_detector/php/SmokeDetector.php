<?php

require_once('Tinkerforge/IPConnection.php');
require_once('Tinkerforge/BrickletIndustrialDigitalIn4.php');

use Tinkerforge\IPConnection;
use Tinkerforge\BrickletIndustrialDigitalIn4;

class SmokeDetector
{
	const HOST = 'localhost';
	const PORT = 4223;

	public function __construct()
    {
		$this->ipcon = new IPConnection();
		$this->brickletIndustrialDigitalIn4 = null;

		while(true) {
			try {
				$this->ipcon->connect(self::HOST, self::PORT);
				break;
			} catch(Exception $e) {
				sleep(1);
			}
		}

		$this->ipcon->registerCallback(IPConnection::CALLBACK_ENUMERATE,
		                               array($this, 'cb_enumerate'));
		$this->ipcon->registerCallback(IPConnection::CALLBACK_CONNECTED,
		                               array($this, 'cb_connected'));

		while(true) {
			try {
				$this->ipcon->enumerate();
				break;
			} catch(Exception $e) {
				sleep(1);
			}
		}
	}

	function cb_interrupt($interruptMask, $valueMask)
	{
		if ($valueMask > 0) {
			echo "Fire! Fire!\n";
		}
	}

	function cb_enumerate($uid, $connectedUid, $position, $hardwareVersion,
	                      $firmwareVersion, $deviceIdentifier, $enumerationType)
	{
		if($enumerationType == IPConnection::ENUMERATION_TYPE_CONNECTED ||
		   $enumerationType == IPConnection::ENUMERATION_TYPE_AVAILABLE) {
			if($deviceIdentifier == BrickletIndustrialDigitalIn4::DEVICE_IDENTIFIER) {
				try {
					$this->brickletIndustrialDigitalIn4 = new BrickletIndustrialDigitalIn4($uid, $this->ipcon);
					$this->brickletIndustrialDigitalIn4->setDebouncePeriod(10000);
					$this->brickletIndustrialDigitalIn4->setInterrupt(15);
					$this->brickletIndustrialDigitalIn4->registerCallback(BrickletIndustrialDigitalIn4::CALLBACK_INTERRUPT,
					                                                      array($this, 'cb_interrupt'));
					echo "Industrial Digital In 4 initialized\n";
				} catch(Exception $e) {
					$this->brickletIndustrialDigitalIn4 = null;
					echo "Industrial Digital In 4 init failed: $e\n";
				}
			}
		}
	}

	function cb_connected($connectedReason)
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

$smokeDetector = new SmokeDetector();
echo "Press ctrl+c to exit\n";
$smokeDetector->ipcon->dispatchCallbacks(-1);

?>
