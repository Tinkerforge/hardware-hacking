program SmokeDetectorV2;

{$ifdef MSWINDOWS}{$apptype CONSOLE}{$endif}
{$ifdef FPC}{$mode OBJFPC}{$H+}{$endif}

uses
  SysUtils, IPConnection, Device, BrickletIndustrialDigitalIn4V2;

const
  HOST = 'localhost';
  PORT = 4223;

type
  TSmokeDetector = class
  private
    ipcon: TIPConnection;
    brickletIndustrialDigitalIn4: TBrickletIndustrialDigitalIn4V2;
  public
    constructor Create;
    destructor Destroy; override;
    procedure ConnectedCB(sender: TIPConnection; const connectedReason: byte);
    procedure EnumerateCB(sender: TIPConnection; const uid: string;
                          const connectedUid: string; const position: char;
                          const hardwareVersion: TVersionNumber;
                          const firmwareVersion: TVersionNumber;
                          const deviceIdentifier: word; const enumerationType: byte);
    procedure InterruptCB(sender: TBrickletIndustrialDigitalIn4V2; const changed: TArray0To3OfBoolean; const value: TArray0To3OfBoolean);
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
    if (deviceIdentifier = BRICKLET_INDUSTRIAL_DIGITAL_IN_4_V2_DEVICE_IDENTIFIER) then begin
      try
        brickletIndustrialDigitalIn4 := TBrickletIndustrialDigitalIn4V2.Create(uid, ipcon);
        brickletIndustrialDigitalIn4.SetAllValueCallbackConfiguration(10000, true);
        brickletIndustrialDigitalIn4.OnAllValue := {$ifdef FPC}@{$endif}InterruptCB;
        WriteLn('Industrial Digital In 4 V2 initialized');
      except
        on e: Exception do begin
          WriteLn('Industrial Digital In 4 V2 init failed: ' + e.Message);
          brickletIndustrialDigitalIn4 := nil;
        end;
      end;
    end;
  end;
end;

procedure TSmokeDetector.InterruptCB(sender: TBrickletIndustrialDigitalIn4V2; const changed: TArray0To3OfBoolean; const value: TArray0To3OfBoolean);
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
