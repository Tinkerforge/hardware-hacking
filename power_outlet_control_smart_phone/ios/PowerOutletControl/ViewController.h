//
//  ViewController.h
//  PowerOutletControl
//
//  Created by Olaf Lüke on 12.09.13.
//  Copyright (c) 2013 Olaf Lüke. All rights reserved.
//

#import <UIKit/UIKit.h>
#include "ip_connection.h"
#include "bricklet_industrial_quad_relay.h"

@interface ViewController : UIViewController
{
    IBOutlet UITextField *hostTextField;
    IBOutlet UITextField *portTextField;
    IBOutlet UITextField *uidTextField;
    IBOutlet UIButton *connectButton;
    IBOutlet UIButton *aOnButton;
    IBOutlet UIButton *aOffButton;
    IBOutlet UIButton *bOnButton;
    IBOutlet UIButton *bOffButton;
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
@property (nonatomic, retain) UIButton *aOnButton;
@property (nonatomic, retain) UIButton *aOffButton;
@property (nonatomic, retain) UIButton *bOnButton;
@property (nonatomic, retain) UIButton *bOffButton;
@property (nonatomic, retain) UIActivityIndicatorView *indicator;

- (IBAction)connectPressed:(id)sender;
- (IBAction)aOnPressed:(id)sender;
- (IBAction)aOffPressed:(id)sender;
- (IBAction)bOnPressed:(id)sender;
- (IBAction)bOffPressed:(id)sender;

- (void)saveState;
- (void)restoreState;

@end
