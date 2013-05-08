program SmokeDetector;

{$ifdef MSWINDOWS}{$apptype CONSOLE}{$endif}
{$ifdef FPC}{$mode OBJFPC}{$H+}{$endif}

uses
  SysUtils, IPConnection, Device, BrickletAnalogIn;

const
  HOST = 'localhost';
  PORT = 4223;

type
  TSmokeDetector = class
  private
    ipcon: TIPConnection;
    brickletAnalogIn: TBrickletAnalogIn;
  public
    constructor Create;
    destructor Destroy; override;
    procedure ConnectedCB(sender: TIPConnection; const connectedReason: byte);
    procedure EnumerateCB(sender: TIPConnection; const uid: string;
                          const connectedUid: string; const position: char;
                          const hardwareVersion: TVersionNumber;
                          const firmwareVersion: TVersionNumber;
                          const deviceIdentifier: word; const enumerationType: byte);
    procedure VoltageReachedCB(sender: TBrickletAnalogIn; const voltage: word);
    procedure Execute;
  end;

var
  sd: TSmokeDetector;

constructor TSmokeDetector.Create;
begin
  ipcon := nil;
  brickletAnalogIn := nil;
end;

destructor TSmokeDetector.Destroy;
begin
  if (brickletAnalogIn <> nil) then brickletAnalogIn.Destroy;
  if (ipcon <> nil) then ipcon.Destroy;
  inherited Destroy;
end;

procedure TSmokeDetector.ConnectedCB(sender: TIPConnection; const connectedReason: byte);
begin
  if (connectedReason = IPCON_CONNECT_REASON_AUTO_RECONNECT) then begin
    WriteLn('Auto Reconnect');
    while (true) do begin
      try
        ipcon.Enumerate;
        break;
      except
        on e: Exception do begin
          WriteLn('Enumeration Error: ' + e.Message);
          Sleep(1000);
        end;
      end;
    end;
  end;
end;

procedure TSmokeDetector.EnumerateCB(sender: TIPConnection; const uid: string;
                                     const connectedUid: string; const position: char;
                                     const hardwareVersion: TVersionNumber;
                                     const firmwareVersion: TVersionNumber;
                                     const deviceIdentifier: word; const enumerationType: byte);
begin
  if ((enumerationType = IPCON_ENUMERATION_TYPE_CONNECTED) or
      (enumerationType = IPCON_ENUMERATION_TYPE_AVAILABLE)) then begin
    if (deviceIdentifier = BRICKLET_ANALOG_IN_DEVICE_IDENTIFIER) then begin
      try
        brickletAnalogIn := TBrickletAnalogIn.Create(uid, ipcon);
        brickletAnalogIn.SetRange(1);
        brickletAnalogIn.SetDebouncePeriod(10000);
        brickletAnalogIn.SetVoltageCallbackThreshold('>', 1200, 0);
        brickletAnalogIn.OnVoltageReached := {$ifdef FPC}@{$endif}VoltageReachedCB;
        WriteLn('Analog In initialized');
      except
        on e: Exception do begin
          WriteLn('Analog In init failed: ' + e.Message);
          brickletAnalogIn := nil;
        end;
      end;
    end;
  end;
end;

procedure TSmokeDetector.VoltageReachedCB(sender: TBrickletAnalogIn; const voltage: word);
begin
  WriteLn('Fire! Fire!');
end;

procedure TSmokeDetector.Execute;
begin
  ipcon := TIPConnection.Create;
  while (true) do begin
    try
      ipcon.Connect(HOST, PORT);
      break;
    except
      on e: Exception do begin
        WriteLn('Connection Error: ' + e.Message);
        Sleep(1000);
      end;
    end;
  end;
  ipcon.OnEnumerate := {$ifdef FPC}@{$endif}EnumerateCB;
  ipcon.OnConnected := {$ifdef FPC}@{$endif}ConnectedCB;
  while (true) do begin
    try
      ipcon.Enumerate;
      break;
    except
      on e: Exception do begin
        WriteLn('Enumeration Error: ' + e.Message);
        Sleep(1000);
      end;
    end;
  end;
  WriteLn('Press key to exit');
  ReadLn;
end;

begin
  sd := TSmokeDetector.Create;
  sd.Execute;
  sd.Destroy;
end.
