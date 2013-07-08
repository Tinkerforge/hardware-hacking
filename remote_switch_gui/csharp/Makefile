
all: RemoteSwitchGUI.exe

clean:
	rm RemoteSwitchGUI.exe

RemoteSwitchGUI.exe: RemoteSwitchGUI.cs IPConnection.cs BrickletIndustrialQuadRelay.cs
	gmcs /target:winexe /out:RemoteSwitchGUI.exe /pkg:dotnet RemoteSwitchGUI.cs IPConnection.cs BrickletIndustrialQuadRelay.cs
