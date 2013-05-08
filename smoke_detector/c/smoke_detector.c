#include <stdio.h>

#include "ip_connection.h"
#include "bricklet_analog_in.h"

#define HOST "localhost"
#define PORT 4223

typedef struct {
	IPConnection ipcon;
	AnalogIn analog_in;
} SmokeDetector;

void cb_voltage_reached(uint16_t voltage, void *user_data) {
	printf("Fire! Fire!\n");
}

void cb_connected(uint8_t connected_reason, void *user_data) {
	SmokeDetector *sd = (SmokeDetector *)user_data;

	if(connected_reason == IPCON_CONNECT_REASON_AUTO_RECONNECT) {
		printf("Auto Reconnect\n");

		while(true) {
			int rc = ipcon_enumerate(&sd->ipcon);
			if(rc < 0) {
				fprintf(stderr, "Could not enumerate: %d\n", rc);
				// TODO: sleep 1s
				continue;
			}
			break;
		}
	}
}

void cb_enumerate(const char *uid, const char *connected_uid,
                  char position, uint8_t hardware_version[3],
                  uint8_t firmware_version[3], uint16_t device_identifier,
                  uint8_t enumeration_type, void *user_data) {
	SmokeDetector *sd = (SmokeDetector *)user_data;

	if(enumeration_type == IPCON_ENUMERATION_TYPE_CONNECTED ||
	   enumeration_type == IPCON_ENUMERATION_TYPE_AVAILABLE) {
		if(device_identifier == ANALOG_IN_DEVICE_IDENTIFIER) {
			analog_in_create(&sd->analog_in, uid, &sd->ipcon);
			analog_in_set_range(&sd->analog_in, 1);
			analog_in_set_debounce_period(&sd->analog_in, 10000);
			analog_in_register_callback(&sd->analog_in,
			                            ANALOG_IN_CALLBACK_VOLTAGE_REACHED,
			                            (void *)cb_voltage_reached,
			                            (void *)sd);

			int rc = analog_in_set_voltage_callback_threshold(&sd->analog_in, '>', 1200, 0);
			if(rc < 0) {
				fprintf(stderr, "Analog In init failed: %d\n", rc);
			} else {
				printf("Analog In initialized\n");
			}
		}
	}
}

int main() {
	SmokeDetector sd;

	ipcon_create(&sd.ipcon);

	while(true) {
		int rc = ipcon_connect(&sd.ipcon, HOST, PORT);
		if(rc < 0) {
			fprintf(stderr, "Could not connect to brickd: %d\n", rc);
			// TODO: sleep 1s
			continue;
		}
		break;
	}

	ipcon_register_callback(&sd.ipcon,
	                        IPCON_CALLBACK_ENUMERATE,
	                        (void *)cb_enumerate,
	                        (void *)&sd);

	ipcon_register_callback(&sd.ipcon,
	                        IPCON_CALLBACK_CONNECTED,
	                        (void *)cb_connected,
	                        (void *)&sd);

	while(true) {
		int rc = ipcon_enumerate(&sd.ipcon);
		if(rc < 0) {
			fprintf(stderr, "Could not enumerate: %d\n", rc);
			// TODO: sleep 1s
			continue;
		}
		break;
	}

	printf("Press key to exit\n");
	getchar();
	ipcon_destroy(&sd.ipcon);
	return 0;
}
