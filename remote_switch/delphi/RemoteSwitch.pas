program ExampleSimple;

{$ifdef MSWINDOWS}{$apptype CONSOLE}{$endif}
{$ifdef FPC}{$mode OBJFPC}{$H+}{$endif}

uses
  SysUtils, IPConnection, BrickletIndustrialQuadRelay;

type
  TExample = class
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
  e: TExample;

procedure TExample.Execute;
begin
  { Create IP connection }
  ipcon := TIPConnection.Create;

  { Create device object }
  iqr := TBrickletIndustrialQuadRelay.Create(UID, ipcon);

  { Connect to brickd }
  ipcon.Connect(HOST, PORT);
  { Don't use device before ipcon is connected }

  { Set pins to high for 1.5 seconds }
  iqr.SetMonoflop(VALUE_A_ON, 255, 1500);
end;

begin
  e := TExample.Create;
  e.Execute;
  e.Destroy;
end.
