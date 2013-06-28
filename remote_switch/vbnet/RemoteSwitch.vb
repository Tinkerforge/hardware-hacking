Imports Tinkerforge

Module ExampleSimple
    Const HOST As String = "localhost"
    Const PORT As Integer = 4223
    Const UID As String = "ctG" ' Change to your UID
	Const VALUE_A_ON  As Integer = (1 << 0) Or (1 << 2) ' Pin 0 and 2 high
	Const VALUE_A_OFF As Integer = (1 << 0) Or (1 << 3) ' Pin 0 and 3 high
	Const VALUE_B_ON  As Integer = (1 << 1) Or (1 << 2) ' Pin 1 and 2 high
	Const VALUE_B_OFF As Integer = (1 << 1) Or (1 << 3) ' Pin 1 and 3 high

    Sub Main()
        Dim ipcon As New IPConnection() ' Create IP connection
        Dim iqr As New BrickletIndustrialQuadRelay(UID, ipcon) ' Create device object

        ipcon.Connect(HOST, PORT) ' Connect to brickd
        ' Don't use device before ipcon is connected

		iqr.SetMonoflop(VALUE_A_ON, 255, 1500) ' Set pins to high for 1.5 seconds
    End Sub
End Module
