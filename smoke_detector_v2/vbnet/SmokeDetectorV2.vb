Imports Tinkerforge

Module SmokeDetector
    Const HOST As String = "localhost"
    Const PORT As Integer = 4223

    Private ipcon As IPConnection = Nothing
    Private brickletIndustrialDigitalIn4 As BrickletIndustrialDigitalIn4V2 = Nothing

    Sub InterruptCB(ByVal sender As BrickletIndustrialDigitalIn4V2, _
                    ByVal changed() As Boolean, ByVal value() As Boolean)
        System.Console.WriteLine("Fire! Fire!")
    End Sub

    Sub EnumerateCB(ByVal sender As IPConnection, ByVal uid As String, _
                    ByVal connectedUid As String, ByVal position As Char, _
                    ByVal hardwareVersion() As Short, ByVal firmwareVersion() As Short, _
                    ByVal deviceIdentifier As Integer, ByVal enumerationType As Short)
        If enumerationType = IPConnection.ENUMERATION_TYPE_CONNECTED Or _
           enumerationType = IPConnection.ENUMERATION_TYPE_AVAILABLE Then
            If deviceIdentifier = BrickletIndustrialDigitalIn4V2.DEVICE_IDENTIFIER Then
                Try
                    brickletIndustrialDigitalIn4 = New BrickletIndustrialDigitalIn4V2(UID, ipcon)
                    brickletIndustrialDigitalIn4.SetAllValueCallbackConfiguration(10000, true)
                    AddHandler brickletIndustrialDigitalIn4.AllValueCallback, AddressOf InterruptCB
                    System.Console.WriteLine("Industrial Digital In 4 V2 initialized")
                Catch e As TinkerforgeException
                    System.Console.WriteLine("Industrial Digital In 4 V2 init failed: " + e.Message)
                    brickletIndustrialDigitalIn4 = Nothing
                End Try
            End If
        End If
    End Sub

    Sub ConnectedCB(ByVal sender As IPConnection, ByVal connectedReason as Short)
        If connectedReason = IPConnection.CONNECT_REASON_AUTO_RECONNECT Then
            System.Console.WriteLine("Auto Reconnect")
            while True
                Try
                    ipcon.Enumerate()
                    Exit While
                Catch e As NotConnectedException
                    System.Console.WriteLine("Enumeration Error: " + e.Message)
                    System.Threading.Thread.Sleep(1000)
                End Try
            End While
        End If
    End Sub

    Sub Main()
        ipcon = New IPConnection()
        while True
            Try
                ipcon.Connect(HOST, PORT)
                Exit While
            Catch e As System.Net.Sockets.SocketException
                System.Console.WriteLine("Connection Error: " + e.Message)
                System.Threading.Thread.Sleep(1000)
            End Try
        End While

        AddHandler ipcon.EnumerateCallback, AddressOf EnumerateCB
        AddHandler ipcon.Connected, AddressOf ConnectedCB

        while True
            try
                ipcon.Enumerate()
                Exit While
            Catch e As NotConnectedException
                System.Console.WriteLine("Enumeration Error: " + e.Message)
                System.Threading.Thread.Sleep(1000)
            End Try
        End While

        System.Console.WriteLine("Press key to exit")
        System.Console.ReadLine()
        ipcon.Disconnect()
    End Sub
End Module
