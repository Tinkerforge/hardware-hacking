#include <stdio.h>

#include "ip_connection.h"
#include "bricklet_industrial_digital_in_4.h"

#define HOST "localhost"
#define PORT 4223

typedef struct {
	IPConnection ipcon;
	IndustrialDigitalIn4 idi4;
} SmokeDetector;

void cb_interrupt(uint16_t interrupt_mask, uint16_t value_mask, void *user_data) {
	// avoid unused parameter warning
	(void)interrupt_mask; (void)user_data;

	if(value_mask > 0) {
		printf("Fire! Fire!\n");
	}
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

	// avoid unused parameter warning
	(void)connected_uid; (void)position; (void)hardware_version; (void)firmware_version;

	if(enumeration_type == IPCON_ENUMERATION_TYPE_CONNECTED ||
	   enumeration_type == IPCON_ENUMERATION_TYPE_AVAILABLE) {
		if(device_identifier == INDUSTRIAL_DIGITAL_IN_4_DEVICE_IDENTIFIER) {
			industrial_digital_in_4_create(&sd->idi4, uid, &sd->ipcon);
			industrial_digital_in_4_set_debounce_period(&sd->idi4, 10000);
			industrial_digital_in_4_register_callback(&sd->idi4,
			                                          INDUSTRIAL_DIGITAL_IN_4_CALLBACK_INTERRUPT,
			                                          (void *)cb_interrupt,
			                                          (void *)sd);

			int rc = industrial_digital_in_4_set_interrupt(&sd->idi4, 15);
			if(rc < 0) {
				fprintf(stderr, "Industrial Digital In 4 init failed: %d\n", rc);
			} else {
				printf("Industrial Digital In 4 initialized\n");
			}
		}
	}
}

int main(void) {
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
