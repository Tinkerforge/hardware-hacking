#include <stdio.h>

#include "ip_connection.h"
#include "bricklet_industrial_digital_in_4_v2.h"

#define HOST "localhost"
#define PORT 4223

typedef struct {
	IPConnection ipcon;
	IndustrialDigitalIn4V2 idi4;
} SmokeDetectorV2;

void cb_interrupt(uint8_t *changed, uint8_t *value, void *user_data) {
	// avoid unused parameter warning
	(void)user_data;

	printf("Fire! Fire!\n");
}

void cb_connected(uint8_t connected_reason, void *user_data) {
	SmokeDetectorV2 *sd = (SmokeDetectorV2 *)user_data;

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
	SmokeDetectorV2 *sd = (SmokeDetectorV2 *)user_data;

	// avoid unused parameter warning
	(void)connected_uid; (void)position; (void)hardware_version; (void)firmware_version;

	if(enumeration_type == IPCON_ENUMERATION_TYPE_CONNECTED ||
	   enumeration_type == IPCON_ENUMERATION_TYPE_AVAILABLE) {
		if(device_identifier == INDUSTRIAL_DIGITAL_IN_4_V2_DEVICE_IDENTIFIER) {
			industrial_digital_in_4_v2_create(&sd->idi4, uid, &sd->ipcon);
			industrial_digital_in_4_v2_register_callback(&sd->idi4,
			                                             INDUSTRIAL_DIGITAL_IN_4_V2_CALLBACK_ALL_VALUE,
			                                             (void *)cb_interrupt,
			                                             (void *)sd);
			int rc = industrial_digital_in_4_v2_set_all_value_callback_configuration(&sd->idi4, 10000, true);

			if(rc < 0) {
				fprintf(stderr, "Industrial Digital In 4 V2 init failed: %d\n", rc);
			} else {
				printf("Industrial Digital In 4 V2 initialized\n");
			}
		}
	}
}

int main(void) {
	SmokeDetectorV2 sd;

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
