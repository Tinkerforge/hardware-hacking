#!/usr/bin/env ruby
# -*- ruby encoding: utf-8 -*-

require 'tinkerforge/ip_connection'
require 'tinkerforge/bricklet_industrial_quad_relay'

include Tinkerforge

HOST = 'localhost'
PORT = 4223
UID = 'ctG' # Change to your UID
VALUE_A_ON  = (1 << 0) | (1 << 2) # Pin 0 and 2 high
VALUE_A_OFF = (1 << 0) | (1 << 3) # Pin 0 and 3 high
VALUE_B_ON  = (1 << 1) | (1 << 2) # Pin 1 and 2 high
VALUE_B_OFF = (1 << 1) | (1 << 3) # Pin 1 and 3 high

ipcon = IPConnection.new # Create IP connection
iqr = BrickletIndustrialQuadRelay.new UID, ipcon # Create device object

ipcon.connect HOST, PORT # Connect to brickd
# Don't use device before ipcon is connected

iqr.set_monoflop VALUE_A_ON, 15, 1500

ipcon.disconnect
