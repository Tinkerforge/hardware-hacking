
all: RemoteSwitchGUIV2.exe

clean:
	rm RemoteSwitchGUIV2.exe

RemoteSwitchGUIV2.exe: RemoteSwitchGUIV2.cs IPConnection.cs BrickletIndustrialQuadRelayV2.cs
	mcs /target:winexe /out:RemoteSwitchGUIV2.exe /pkg:dotnet RemoteSwitchGUIV2.cs IPConnection.cs BrickletIndustrialQuadRelayV2.cs
