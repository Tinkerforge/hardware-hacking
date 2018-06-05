<?php

require_once('Tinkerforge/IPConnection.php');
require_once('Tinkerforge/BrickletIndustrialQuadRelayV2.php');

use Tinkerforge\IPConnection;
use Tinkerforge\BrickletIndustrialQuadRelayV2;

const HOST = 'localhost';
const PORT = 4223;
const UID = '2g2gRs'; // Change to your UID

function a_on($iqr) {
    # Close channels 0 and 2 for 1.5 seconds
    $iqr->setMonoflop(0, True, 1500);
    $iqr->setMonoflop(2, True, 1500);
}

function a_off($iqr) {
    # Close channels 0 and 3 for 1.5 seconds
    $iqr->setMonoflop(0, True, 1500);
    $iqr->setMonoflop(3, True, 1500);
}

function b_on($iqr) {
    # Close channels 1 and 2 for 1.5 seconds
    $iqr->setMonoflop(1, True, 1500);
    $iqr->setMonoflop(2, True, 1500);
}

function b_off($iqr) {
    # Close channels 1 and 3 for 1.5 seconds
    $iqr->setMonoflop(1, True, 1500);
    $iqr->setMonoflop(3, True, 1500);
}

$ipcon = new IPConnection(); // Create IP connection
$iqr = new BrickletIndustrialQuadRelayV2(UID, $ipcon); // Create device object

$ipcon->connect(HOST, PORT); // Connect to brickd
// Don't use device before ipcon is connected

a_on($iqr);

$ipcon->disconnect();

?>
