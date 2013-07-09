program SmokeDetector;

{$ifdef MSWINDOWS}{$apptype CONSOLE}{$endif}
{$ifdef FPC}{$mode OBJFPC}{$H+}{$endif}

uses
  SysUtils, IPConnection, Device, BrickletIndustrialDigitalIn4;

const
  HOST = 'localhost';
  PORT = 4223;

type
  TSmokeDetector = class
  private
    ipcon: TIPConnection;
    brickletIndustrialDigitalIn4: TBrickletIndustrialDigitalIn4;
  public
    constructor Create;
    destructor Destroy; override;
    procedure ConnectedCB(sender: TIPConnection; const connectedReason: byte);
    procedure EnumerateCB(sender: TIPConnection; const uid: string;
                          const connectedUid: string; const position: char;
                          const hardwareVersion: TVersionNumber;
                          const firmwareVersion: TVersionNumber;
                          const deviceIdentifier: word; const enumerationType: byte);
    procedure InterruptCB(sender: TBrickletIndustrialDigitalIn4; const interruptMask: word; const valueMask: word);
    procedure Execute;
  end;

var
  sd: TSmokeDetector;

constructor TSmokeDetector.Create;
begin
  ipcon := nil;
  brickletIndustrialDigitalIn4 := nil;
end;

destructor TSmokeDetector.Destroy;
begin
  if (brickletIndustrialDigitalIn4 <> nil) then brickletIndustrialDigitalIn4.Destroy;
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
    if (deviceIdentifier = BRICKLET_INDUSTRIAL_DIGITAL_IN_4_DEVICE_IDENTIFIER) then begin
      try
        brickletIndustrialDigitalIn4 := TBrickletIndustrialDigitalIn4.Create(uid, ipcon);
        brickletIndustrialDigitalIn4.SetDebouncePeriod(10000);
        brickletIndustrialDigitalIn4.SetInterrupt(15);
        brickletIndustrialDigitalIn4.OnInterrupt := {$ifdef FPC}@{$endif}InterruptCB;
        WriteLn('Industrial Digital In 4 initialized');
      except
        on e: Exception do begin
          WriteLn('Industrial Digital In 4 init failed: ' + e.Message);
          brickletIndustrialDigitalIn4 := nil;
        end;
      end;
    end;
  end;
end;

procedure TSmokeDetector.InterruptCB(sender: TBrickletIndustrialDigitalIn4; const interruptMask: word; const valueMask: word);
begin
  if (valueMask > 0) then begin
    WriteLn('Fire! Fire!');
  end;
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
