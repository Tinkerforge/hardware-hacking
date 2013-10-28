#import <UIKit/UIKit.h>

#include "ip_connection.h"
#include "bricklet_industrial_quad_relay.h"

@interface ViewController : UIViewController
{
    IBOutlet UITextField *hostTextField;
    IBOutlet UITextField *portTextField;
    IBOutlet UITextField *uidTextField;
    IBOutlet UIButton *connectButton;
    IBOutlet UIButton *triggerButton;
    IBOutlet UIActivityIndicatorView *indicator;

    dispatch_queue_t queue;
    IPConnection ipcon;
    IndustrialQuadRelay relay;
    BOOL connected;
}

@property (nonatomic, retain) UITextField *hostTextField;
@property (nonatomic, retain) UITextField *portTextField;
@property (nonatomic, retain) UITextField *uidTextField;
@property (nonatomic, retain) UIButton *connectButton;
@property (nonatomic, retain) UIButton *triggerButton;
@property (nonatomic, retain) UIActivityIndicatorView *indicator;

- (IBAction)connectPressed:(id)sender;
- (IBAction)triggerPressed:(id)sender;

- (void)saveState;
- (void)restoreState;

@end
