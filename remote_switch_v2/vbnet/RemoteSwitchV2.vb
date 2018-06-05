Imports Tinkerforge

Module RemoteSwitchV2
	Const HOST As String = "localhost"
	Const PORT As Integer = 4223
	Const UID As String = "2g2gRs" ' Change to your UID

	Sub AOn(ByVal iqr As BrickletIndustrialQuadRelayV2)
		' Close channels 0 and 2 for 1.5 seconds
		iqr.SetMonoflop(0, true, 1500)
		iqr.SetMonoflop(2, true, 1500)
	End Sub

	Sub AOff(ByVal iqr As BrickletIndustrialQuadRelayV2)
		' Close channels 0 and 3 for 1.5 seconds
		iqr.SetMonoflop(0, true, 1500)
		iqr.SetMonoflop(3, true, 1500)
	End Sub

	Sub BOn(ByVal iqr As BrickletIndustrialQuadRelayV2)
		' Close channels 1 and 2 for 1.5 seconds
		iqr.SetMonoflop(1, true, 1500)
		iqr.SetMonoflop(2, true, 1500)
	End Sub

	Sub BOff(ByVal iqr As BrickletIndustrialQuadRelayV2)
		' Close channels 1 and 3 for 1.5 seconds
		iqr.SetMonoflop(1, true, 1500)
		iqr.SetMonoflop(3, true, 1500)
	End Sub

	Sub Main()
		Dim ipcon As New IPConnection() ' Create IP connection
		Dim iqr As New BrickletIndustrialQuadRelayV2(UID, ipcon) ' Create device object

		ipcon.Connect(HOST, PORT) ' Connect to brickd
		' Don't use device before ipcon is connected

		Aon(iqr)

		ipcon.Disconnect()
	End Sub
End Module
