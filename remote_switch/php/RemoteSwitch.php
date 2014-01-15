<?php

require_once('Tinkerforge/IPConnection.php');
require_once('Tinkerforge/BrickletIndustrialQuadRelay.php');

use Tinkerforge\IPConnection;
use Tinkerforge\BrickletIndustrialQuadRelay;

const HOST = 'localhost';
const PORT = 4223;
const UID = 'ctG'; // Change to your UID
$VALUE_A_ON  = (1 << 0) | (1 << 2); // Pin 0 and 2 high
$VALUE_A_OFF = (1 << 0) | (1 << 3); // Pin 0 and 3 high
$VALUE_B_ON  = (1 << 1) | (1 << 2); // Pin 1 and 2 high
$VALUE_B_OFF = (1 << 1) | (1 << 3); // Pin 1 and 3 high

$ipcon = new IPConnection(); // Create IP connection
$iqr = new BrickletIndustrialQuadRelay(UID, $ipcon); // Create device object

$ipcon->connect(HOST, PORT); // Connect to brickd
// Don't use device before ipcon is connected

$iqr->setMonoflop($VALUE_A_ON, 15, 1500);

$ipcon->disconnect();

?>
