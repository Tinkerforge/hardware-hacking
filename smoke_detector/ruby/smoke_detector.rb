#!/usr/bin/env ruby
# -*- ruby encoding: utf-8 -*-

require 'tinkerforge/ip_connection'
require 'tinkerforge/bricklet_industrial_digital_in_4'

include Tinkerforge

HOST = 'localhost'
PORT = 4223

idi4 = nil

ipcon = IPConnection.new
while true
  begin
    ipcon.connect HOST, PORT
    break
  rescue Exception => e
    puts 'Connection Error: ' + e
    sleep 1
  end
end

ipcon.register_callback(IPConnection::CALLBACK_ENUMERATE) do |uid, connected_uid, position,
                                                              hardware_version, firmware_version,
                                                              device_identifier, enumeration_type|
  if enumeration_type == IPConnection::ENUMERATION_TYPE_CONNECTED or
     enumeration_type == IPConnection::ENUMERATION_TYPE_AVAILABLE
    if device_identifier == BrickletIndustrialDigitalIn4::DEVICE_IDENTIFIER
      begin
        idi4 = BrickletIndustrialDigitalIn4.new uid, ipcon
        idi4.set_debounce_period 10000
        idi4.set_interrupt 15
        idi4.register_callback(BrickletIndustrialDigitalIn4::CALLBACK_INTERRUPT) do |interrupt_mask, value_mask|
          if value_mask > 0
            puts 'Fire! Fire!'
          end
        end
        puts 'Industrial Digital In 4 initialized'
      rescue Exception => e
        idi4 = nil
        puts 'Industrial Digital In 4 init failed: ' + e
      end
    end
  end
end

ipcon.register_callback(IPConnection::CALLBACK_CONNECTED) do |connected_reason|
  if connected_reason == IPConnection::CONNECT_REASON_AUTO_RECONNECT
    puts 'Auto Reconnect'
    while true
      begin
        ipcon.enumerate
        break
      rescue Exception => e
        puts 'Enumerate Error: ' + e
        sleep 1
      end
    end
  end
end

while true
  begin
    ipcon.enumerate
    break
  rescue Exception => e
    puts 'Enumerate Error: ' + e
    sleep 1
  end
end

puts 'Press key to exit'
$stdin.gets
ipcon.disconnect
