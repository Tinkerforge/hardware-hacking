#include <stdio.h>

#include "ip_connection.h"
#include "bricklet_industrial_quad_relay.h"

#define HOST "localhost"
#define PORT 4223
#define UID "XYZ" // Change to your UID

#define VALUE_A_ON  ((1 << 0) | (1 << 2)) // Pin 0 and 2 high
#define VALUE_A_OFF ((1 << 0) | (1 << 3)) // Pin 0 and 3 high
#define VALUE_B_ON  ((1 << 1) | (1 << 2)) // Pin 1 and 2 high
#define VALUE_B_OFF ((1 << 1) | (1 << 3)) // Pin 1 and 3 high

int main(void) {
	// Create IP connection
	IPConnection ipcon;
	ipcon_create(&ipcon);

	// Create device object
	IndustrialQuadRelay iqr;
	industrial_quad_relay_create(&iqr, UID, &ipcon);

	// Connect to brickd
	if(ipcon_connect(&ipcon, HOST, PORT) < 0) {
		fprintf(stderr, "Could not connect\n");
		return 1;
	}
	// Don't use device before ipcon is connected

	// Set pins to high for 1.5 seconds
	industrial_quad_relay_set_monoflop(&iqr, VALUE_A_ON, 15, 1500);

	ipcon_destroy(&ipcon); // Calls ipcon_disconnect internally
	return 0;
}
