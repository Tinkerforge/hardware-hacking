#!/usr/bin/env python
# -*- coding: utf-8 -*-

HOST = "localhost"
PORT = 4223
UID = "ctG" # Change to your UID

VALUE_A_ON  = (1 << 0) | (1 << 2) # Pin 0 and 2 high
VALUE_A_OFF = (1 << 0) | (1 << 3) # Pin 0 and 3 high
VALUE_B_ON  = (1 << 1) | (1 << 2) # Pin 1 and 2 high
VALUE_B_OFF = (1 << 1) | (1 << 3) # Pin 1 and 3 high

from tinkerforge.ip_connection import IPConnection
from tinkerforge.bricklet_industrial_quad_relay import IndustrialQuadRelay

if __name__ == "__main__":
    ipcon = IPConnection() # Create IP connection
    iqr = IndustrialQuadRelay(UID, ipcon) # Create device object

    ipcon.connect(HOST, PORT) # Connect to brickd
    # Don't use device before ipcon is connected

    iqr.set_monoflop(VALUE_A_ON, 15, 1500); # Set pins to high for 1.5 seconds

    ipcon.disconnect()
