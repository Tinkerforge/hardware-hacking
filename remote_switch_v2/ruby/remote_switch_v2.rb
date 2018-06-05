#!/usr/bin/env ruby
# -*- ruby encoding: utf-8 -*-

require 'tinkerforge/ip_connection'
require 'tinkerforge/bricklet_industrial_quad_relay_v2'

include Tinkerforge

HOST = 'localhost'
PORT = 4223
UID = '2g2gRs' # Change to your UID

def a_on(iqr)
    # Close channels 0 and 2 for 1.5 seconds
    iqr.set_monoflop 0, true, 1500
    iqr.set_monoflop 2, true, 1500
end

def a_off(iqr)
    # Close channels 0 and 3 for 1.5 seconds
    iqr.set_monoflop 0, true, 1500
    iqr.set_monoflop 3, true, 1500
end

def b_on(iqr)
    # Close channels 1 and 2 for 1.5 seconds
    iqr.set_monoflop 1, true, 1500
    iqr.set_monoflop 2, true, 1500
end

def b_off(iqr)
    # Close channels 1 and 3 for 1.5 seconds
    iqr.set_monoflop 1, true, 1500
    iqr.set_monoflop 3, true, 1500
end

ipcon = IPConnection.new # Create IP connection
iqr = BrickletIndustrialQuadRelayV2.new UID, ipcon # Create device object

ipcon.connect HOST, PORT # Connect to brickd
# Don't use device before ipcon is connected

a_on iqr

ipcon.disconnect