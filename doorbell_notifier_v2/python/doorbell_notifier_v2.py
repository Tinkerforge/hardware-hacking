#!/usr/bin/env python
# -*- coding: utf-8 -*-

import socket
import sys
import time
import math
import logging as log
log.basicConfig(level=log.INFO)

from tinkerforge.ip_connection import IPConnection
from tinkerforge.ip_connection import Error
from tinkerforge.bricklet_industrial_digital_in_4_v2 import IndustrialDigitalIn4V2

class DoorbellNotifier:
    HOST = 'localhost'
    PORT = 4223
    ipcon = None
    idi4 = None

    def __init__(self):
        self.ipcon = IPConnection()
        while True:
            try:
                self.ipcon.connect(DoorbellNotifier.HOST, DoorbellNotifier.PORT)
                break
            except Error as e:
                log.error('Connection Error: ' + str(e.description))
                time.sleep(1)
            except socket.error as e:
                log.error('Socket error: ' + str(e))
                time.sleep(1)

        self.ipcon.register_callback(IPConnection.CALLBACK_ENUMERATE,
                                     self.cb_enumerate)
        self.ipcon.register_callback(IPConnection.CALLBACK_CONNECTED,
                                     self.cb_connected)

        while True:
            try:
                self.ipcon.enumerate()
                break
            except Error as e:
                log.error('Enumerate Error: ' + str(e.description))
                time.sleep(1)

    def cb_interrupt(self, changed, value):
        log.warn('Ring Ring Ring!')

    def cb_enumerate(self, uid, connected_uid, position, hardware_version,
                     firmware_version, device_identifier, enumeration_type):
        if enumeration_type == IPConnection.ENUMERATION_TYPE_CONNECTED or \
           enumeration_type == IPConnection.ENUMERATION_TYPE_AVAILABLE:
                if device_identifier == IndustrialDigitalIn4V2.DEVICE_IDENTIFIER:
                    try:
                        self.idi4 = IndustrialDigitalIn4V2(uid, self.ipcon)

                        self.idi4.set_all_value_callback_configuration(10000, True)
                        self.idi4.register_callback(IndustrialDigitalIn4V2.CALLBACK_ALL_VALUE,
                                                    self.cb_interrupt)

                        log.info('Industrial Digital In 4 V2 initialized')
                    except Error as e:
                        log.error('Industrial Digital In 4 V2 init failed: ' + str(e.description))

                        self.idi4 = None

    def cb_connected(self, connected_reason):
        if connected_reason == IPConnection.CONNECT_REASON_AUTO_RECONNECT:
            log.info('Auto Reconnect')

            while True:
                try:
                    self.ipcon.enumerate()
                    break
                except Error as e:
                    log.error('Enumerate Error: ' + str(e.description))
                    time.sleep(1)

if __name__ == "__main__":
    log.info('Doorbell Notifier: Start')

    doorbell_notifier = DoorbellNotifier()

    if sys.version_info < (3, 0):
        input = raw_input # Compatibility for Python 2.x
    input('Press key to exit\n')

    if doorbell_notifier.ipcon != None:
        doorbell_notifier.ipcon.disconnect()

    log.info('Doorbell Notifier: End')
