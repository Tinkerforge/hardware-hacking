#import "ViewController.h"

@interface ViewController ()

@end

@implementation ViewController

@synthesize hostTextField;
@synthesize portTextField;
@synthesize uidTextField;
@synthesize connectButton;
@synthesize aOnButton;
@synthesize aOffButton;
@synthesize bOnButton;
@synthesize bOffButton;
@synthesize indicator;

- (void)viewDidLoad
{
    [super viewDidLoad];
    // Do any additional setup after loading the view, typically from a nib.

    queue = dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_DEFAULT, 0);
    connected = NO;

    aOnButton.enabled = NO;
    aOffButton.enabled = NO;
    bOnButton.enabled = NO;
    bOffButton.enabled = NO;
    [indicator setHidden:YES];
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

- (void)connect
{
    NSString *host = hostTextField.text;
    NSString *port = portTextField.text;
    NSString *uid = uidTextField.text;

    if ([host length] == 0 || [port length] == 0 || [uid length] == 0) {
        UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Error" message:@"Host/Port/UID cannot be empty" delegate:nil cancelButtonTitle:nil otherButtonTitles:@"Okay", nil];
        [alert show];
        return;
    }

    int portNumber = [port intValue];
    NSString *reformatedPort = [NSString stringWithFormat:@"%d", portNumber];

    if (portNumber < 1 || portNumber > 65535 || ![port isEqualToString:reformatedPort]) {
        UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Error" message:@"Port number is invalid" delegate:nil cancelButtonTitle:nil otherButtonTitles:@"Okay", nil];
        [alert show];
        return;
    }

    hostTextField.enabled = NO;
    portTextField.enabled = NO;
    uidTextField.enabled = NO;
    connectButton.enabled = NO;
    aOnButton.enabled = NO;
    aOffButton.enabled = NO;
    bOnButton.enabled = NO;
    bOffButton.enabled = NO;
    [indicator setHidden:NO];
    [indicator startAnimating];

    dispatch_async(queue, ^{
        ipcon_create(&ipcon);
        industrial_quad_relay_create(&relay, [uid UTF8String], &ipcon);

        if (ipcon_connect(&ipcon, [host UTF8String], portNumber) < 0) {
            industrial_quad_relay_destroy(&relay);
            ipcon_destroy(&ipcon);

            dispatch_async(dispatch_get_main_queue(), ^{
                [indicator setHidden:YES];

                UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Error" message:[NSString stringWithFormat:@"Could not connect to %@:%d", host, portNumber] delegate:self cancelButtonTitle:@"Cancel" otherButtonTitles:@"Retry", nil];
                [alert show];
            });

            return;
        }

        char uid_[8];
        char connected_uid[8];
        char position;
        uint8_t hardware_version[3];
        uint8_t firmware_version[3];
        uint16_t device_identifier;

        if (industrial_quad_relay_get_identity(&relay, uid_, connected_uid, &position,
                                               hardware_version, firmware_version, &device_identifier) < 0 ||
            device_identifier != INDUSTRIAL_QUAD_RELAY_DEVICE_IDENTIFIER) {
            dispatch_async(dispatch_get_main_queue(), ^{
                [indicator setHidden:YES];

                UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Error" message:[NSString stringWithFormat:@"Could not find Industrial Quad Relay Bricklet [%@]", uid] delegate:self cancelButtonTitle:@"Cancel" otherButtonTitles:@"Retry", nil];
                [alert show];
            });

            return;
        }

        dispatch_async(dispatch_get_main_queue(), ^{
            [indicator setHidden:YES];

            [connectButton setTitle:@"Disconnect" forState: UIControlStateNormal];

            connectButton.enabled = YES;
            aOnButton.enabled = YES;
            aOffButton.enabled = YES;
            bOnButton.enabled = YES;
            bOffButton.enabled = YES;

            connected = YES;
        });
    });
}

- (void)disconnect
{
    connectButton.enabled = NO;
    aOnButton.enabled = NO;
    aOffButton.enabled = NO;
    bOnButton.enabled = NO;
    bOffButton.enabled = NO;

    dispatch_async(queue, ^{
        if (ipcon_disconnect(&ipcon) < 0) {
            dispatch_async(dispatch_get_main_queue(), ^{
                connectButton.enabled = YES;
            });

            return;
        }

        industrial_quad_relay_destroy(&relay);
        ipcon_destroy(&ipcon);

        dispatch_async(dispatch_get_main_queue(), ^{
            [connectButton setTitle:@"Connect" forState: UIControlStateNormal];

            connectButton.enabled = YES;
            hostTextField.enabled = YES;
            portTextField.enabled = YES;
            uidTextField.enabled = YES;

            connected = NO;
        });
    });
}

- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex
{
    if (buttonIndex == 1) {
        [self connect];
    } else {
        [connectButton setTitle:@"Connect" forState: UIControlStateNormal];

        connectButton.enabled = YES;
        hostTextField.enabled = YES;
        portTextField.enabled = YES;
        uidTextField.enabled = YES;
    }
}

- (IBAction)connectPressed:(id)sender
{
    if (!connected) {
        [self connect];
    } else {
        [self disconnect];
    }
}

- (IBAction)aOnPressed:(id)sender
{
    industrial_quad_relay_set_monoflop(&relay, (1 << 0) | (1 << 2), 15, 500);
}

- (IBAction)aOffPressed:(id)sender
{
    industrial_quad_relay_set_monoflop(&relay, (1 << 0) | (1 << 3), 15, 500);
}

- (IBAction)bOnPressed:(id)sender
{
    industrial_quad_relay_set_monoflop(&relay, (1 << 1) | (1 << 2), 15, 500);
}

- (IBAction)bOffPressed:(id)sender
{
    industrial_quad_relay_set_monoflop(&relay, (1 << 1) | (1 << 3), 15, 500);
}

- (void)saveState
{
    [[NSUserDefaults standardUserDefaults] setObject:hostTextField.text forKey:@"host"];
    [[NSUserDefaults standardUserDefaults] setObject:portTextField.text forKey:@"port"];
    [[NSUserDefaults standardUserDefaults] setObject:uidTextField.text forKey:@"uid"];
    [[NSUserDefaults standardUserDefaults] setBool:connected forKey:@"connected"];
}

- (void)restoreState
{
    NSString *host = [[NSUserDefaults standardUserDefaults] stringForKey:@"host"];
    NSString *port = [[NSUserDefaults standardUserDefaults] stringForKey:@"port"];
    NSString *uid = [[NSUserDefaults standardUserDefaults] stringForKey:@"uid"];

    if (host != nil) {
        hostTextField.text = host;
    }

    if (port != nil) {
        portTextField.text = port;
    }

    if (uid != nil) {
        uidTextField.text = uid;
    }

    if ([[NSUserDefaults standardUserDefaults] boolForKey:@"connected"] && !connected) {
        [self connect];
    }
}

@end
