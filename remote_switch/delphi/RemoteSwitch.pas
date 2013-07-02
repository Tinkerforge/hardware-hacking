program RemoteSwitch;

{$ifdef MSWINDOWS}{$apptype CONSOLE}{$endif}
{$ifdef FPC}{$mode OBJFPC}{$H+}{$endif}

uses
  SysUtils, IPConnection, BrickletIndustrialQuadRelay;

type
  TRemoteSwitch = class
  private
    ipcon: TIPConnection;
    iqr: TBrickletIndustrialQuadRelay;
  public
    procedure Execute;
  end;

const
  HOST = 'localhost';
  PORT = 4223;
  UID = 'ctG'; { Change to your UID }
  VALUE_A_ON  = (1 shl 0) or (1 shl 2); { Pin 0 and 2 high }
  VALUE_A_OFF = (1 shl 0) or (1 shl 3); { Pin 0 and 3 high }
  VALUE_B_ON  = (1 shl 1) or (1 shl 2); { Pin 1 and 2 high }
  VALUE_B_OFF = (1 shl 1) or (1 shl 3); { Pin 1 and 3 high }

var
  rs: TRemoteSwitch;

procedure TRemoteSwitch.Execute;
begin
  { Create IP connection }
  ipcon := TIPConnection.Create;

  { Create device object }
  iqr := TBrickletIndustrialQuadRelay.Create(UID, ipcon);

  { Connect to brickd }
  ipcon.Connect(HOST, PORT);
  { Don't use device before ipcon is connected }

  { Set pins to high for 1.5 seconds }
  iqr.SetMonoflop(VALUE_A_ON, 15, 1500);
end;

begin
  rs := TRemoteSwitch.Create;
  rs.Execute;
  rs.Destroy;
end.
