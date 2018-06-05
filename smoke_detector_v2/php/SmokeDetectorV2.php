<?php

require_once('Tinkerforge/IPConnection.php');
require_once('Tinkerforge/BrickletIndustrialDigitalIn4V2.php');

use Tinkerforge\IPConnection;
use Tinkerforge\BrickletIndustrialDigitalIn4V2;

class SmokeDetector
{
	const HOST = 'localhost';
	const PORT = 4223;

	public function __construct()
    {
		$this->ipcon = new IPConnection();
		$this->brickletIndustrialDigitalIn4V2 = null;

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

	function cb_interrupt($changed, $value)
	{
		echo "Fire! Fire!\n";
	}

	function cb_enumerate($uid, $connectedUid, $position, $hardwareVersion,
	                      $firmwareVersion, $deviceIdentifier, $enumerationType)
	{
		if($enumerationType == IPConnection::ENUMERATION_TYPE_CONNECTED ||
		   $enumerationType == IPConnection::ENUMERATION_TYPE_AVAILABLE) {
			if($deviceIdentifier == BrickletIndustrialDigitalIn4V2::DEVICE_IDENTIFIER) {
				try {
					$this->brickletIndustrialDigitalIn4V2 = new BrickletIndustrialDigitalIn4V2($uid, $this->ipcon);
					$this->brickletIndustrialDigitalIn4V2->setAllValueCallbackConfiguration(10000, True);
					$this->brickletIndustrialDigitalIn4V2->registerCallback(BrickletIndustrialDigitalIn4V2::CALLBACK_ALL_VALUE,
					                                                        array($this, 'cb_interrupt'));
					echo "Industrial Digital In 4 V2 initialized\n";
				} catch(Exception $e) {
					$this->BrickletIndustrialDigitalIn4V2 = null;
					echo "Industrial Digital In 4 V2 init failed: $e\n";
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
