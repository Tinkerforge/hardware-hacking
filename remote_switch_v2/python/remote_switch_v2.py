#!/usr/bin/env python
# -*- coding: utf-8 -*-

import sys

HOST = "localhost"
PORT = 4223
UID = "2g2gRs" # Change to your UID

from tinkerforge.ip_connection import IPConnection
from tinkerforge.bricklet_industrial_quad_relay_v2 import IndustrialQuadRelayV2

def a_on():
    # Close channels 0 and 2 for 1.5 seconds
    iqr.set_monoflop(0, True, 1500)
    iqr.set_monoflop(2, True, 1500)

def a_off():
    # Close channels 0 and 3 for 1.5 seconds
    iqr.set_monoflop(0, True, 1500)
    iqr.set_monoflop(3, True, 1500)

def b_on():
    # Close channels 1 and 2 for 1.5 seconds
    iqr.set_monoflop(1, True, 1500)
    iqr.set_monoflop(2, True, 1500)

def b_off():
    # Close channels 1 and 3 for 1.5 seconds
    iqr.set_monoflop(1, True, 1500)
    iqr.set_monoflop(3, True, 1500)

if __name__ == "__main__":
    ipcon = IPConnection() # Create IP connection
    iqr = IndustrialQuadRelayV2(UID, ipcon) # Create device object

    ipcon.connect(HOST, PORT) # Connect to brickd
    # Don't use device before ipcon is connected

    a_on()

    ipcon.disconnect()
