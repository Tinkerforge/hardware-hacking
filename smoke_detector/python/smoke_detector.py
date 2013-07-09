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
from tinkerforge.bricklet_industrial_digital_in_4 import IndustrialDigitalIn4


class SmokeDetector:
    HOST = 'localhost'
    PORT = 4223

    ipcon = None
    idi4 = None

    def __init__(self):
        self.ipcon = IPConnection()
        while True:
            try:
                self.ipcon.connect(SmokeDetector.HOST, SmokeDetector.PORT)
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

    def cb_interrupt(self, interrupt_mask, value_mask):
        if value_mask > 0:
            log.warn('Fire! Fire!')

    def cb_enumerate(self, uid, connected_uid, position, hardware_version,
                     firmware_version, device_identifier, enumeration_type):
        if enumeration_type == IPConnection.ENUMERATION_TYPE_CONNECTED or \
           enumeration_type == IPConnection.ENUMERATION_TYPE_AVAILABLE:
            if device_identifier == IndustrialDigitalIn4.DEVICE_IDENTIFIER:
                try:
                    self.idi4 = IndustrialDigitalIn4(uid, self.ipcon)
                    self.idi4.set_debounce_period(10000)
                    self.idi4.set_interrupt(15)
                    self.idi4.register_callback(IndustrialDigitalIn4.CALLBACK_INTERRUPT,
                                                self.cb_interrupt)
                    log.info('Industrial Digital In 4 initialized')
                except Error as e:
                    log.error('Industrial Digital In 4 init failed: ' + str(e.description))
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
    log.info('Smoke Detector: Start')

    smoke_detector = SmokeDetector()

    if sys.version_info < (3, 0):
        input = raw_input # Compatibility for Python 2.x
    input('Press key to exit\n')

    if smoke_detector.ipcon != None:
        smoke_detector.ipcon.disconnect()

    log.info('Smoke Detector: End')
