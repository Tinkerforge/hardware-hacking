#include <stdio.h>

#include "ip_connection.h"
#include "bricklet_industrial_quad_relay_v2.h"

#define HOST "localhost"
#define PORT 4223
#define UID "2g2gRs" // Change to your UID

void a_on(IndustrialQuadRelayV2 *iqr) {
	// Close channels 0 and 2 for 1.5 seconds
	industrial_quad_relay_v2_set_monoflop(iqr, 0, true, 1500);
	industrial_quad_relay_v2_set_monoflop(iqr, 2, true, 1500);
}

void a_off(IndustrialQuadRelayV2 *iqr) {
	// Close channels 0 and 3 for 1.5 seconds
	industrial_quad_relay_v2_set_monoflop(iqr, 0, true, 1500);
	industrial_quad_relay_v2_set_monoflop(iqr, 3, true, 1500);
}

void b_on(IndustrialQuadRelayV2 *iqr) {
	// Close channels 1 and 2 for 1.5 seconds
	industrial_quad_relay_v2_set_monoflop(iqr, 1, true, 1500);
	industrial_quad_relay_v2_set_monoflop(iqr, 2, true, 1500);
}

void b_off(IndustrialQuadRelayV2 *iqr) {
	// Close channels 1 and 3 for 1.5 seconds
	industrial_quad_relay_v2_set_monoflop(iqr, 1, true, 1500);
	industrial_quad_relay_v2_set_monoflop(iqr, 3, true, 1500);
}

int main(void) {
	// Create IP connection
	IPConnection ipcon;
	ipcon_create(&ipcon);

	// Create device object
	IndustrialQuadRelayV2 iqr;
	industrial_quad_relay_v2_create(&iqr, UID, &ipcon);

	// Connect to brickd
	if(ipcon_connect(&ipcon, HOST, PORT) < 0) {
		fprintf(stderr, "Could not connect\n");
		return 1;
	}
	// Don't use device before ipcon is connected

	a_on(&iqr);

	ipcon_destroy(&ipcon); // Calls ipcon_disconnect internally

	return 0;
}
