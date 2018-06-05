program RemoteSwitchV2;

{$ifdef MSWINDOWS}{$apptype CONSOLE}{$endif}
{$ifdef FPC}{$mode OBJFPC}{$H+}{$endif}

uses
  SysUtils, IPConnection, BrickletIndustrialQuadRelayV2;

type
  TRemoteSwitchV2 = class
  private
    ipcon: TIPConnection;
    iqr: TBrickletIndustrialQuadRelayV2;
  public
    procedure Execute;
    procedure AOn;
    procedure AOff;
    procedure BOn;
    procedure BOff;
  end;

const
  HOST = 'localhost';
  PORT = 4223;
  UID = '2g2gRs'; { Change to your UID }

var
  rs: TRemoteSwitchV2;

procedure TRemoteSwitchV2.AOn;
begin
  { Close channels 0 and 2 for 1.5 seconds }
  iqr.SetMonoflop(0, true, 1500);
  iqr.SetMonoflop(2, true, 1500);
end;

procedure TRemoteSwitchV2.AOff;
begin
  { Close channels 0 and 3 for 1.5 seconds }
  iqr.SetMonoflop(0, true, 1500);
  iqr.SetMonoflop(3, true, 1500);
end;

procedure TRemoteSwitchV2.BOn;
begin
  { Close channels 1 and 2 for 1.5 seconds }
  iqr.SetMonoflop(1, true, 1500);
  iqr.SetMonoflop(2, true, 1500);
end;

procedure TRemoteSwitchV2.BOff;
begin
  { Close channels 1 and 3 for 1.5 seconds }
  iqr.SetMonoflop(1, true, 1500);
  iqr.SetMonoflop(3, true, 1500);
end;

procedure TRemoteSwitchV2.Execute;
begin
  { Create IP connection }
  ipcon := TIPConnection.Create;

  { Create device object }
  iqr := TBrickletIndustrialQuadRelayV2.Create(UID, ipcon);

  { Connect to brickd }
  ipcon.Connect(HOST, PORT);
  { Don't use device before ipcon is connected }

  AOn();
end;

begin
  rs := TRemoteSwitchV2.Create;
  rs.Execute;
  rs.Destroy;
end.
