Imports Tinkerforge

Module SmokeDetector
    Const HOST As String = "localhost"
    Const PORT As Integer = 4223

    Private ipcon As IPConnection = Nothing
    Private brickletIndustrialDigitalIn4 As BrickletIndustrialDigitalIn4 = Nothing

    Sub InterruptCB(ByVal sender As BrickletIndustrialDigitalIn4, _
                    ByVal interruptMask As Integer, ByVal valueMask As Integer)
        If valueMask > 0 Then
            System.Console.WriteLine("Fire! Fire!")
        End If
    End Sub

    Sub EnumerateCB(ByVal sender As IPConnection, ByVal uid As String, _
                    ByVal connectedUid As String, ByVal position As Char, _
                    ByVal hardwareVersion() As Short, ByVal firmwareVersion() As Short, _
                    ByVal deviceIdentifier As Integer, ByVal enumerationType As Short)
        If enumerationType = IPConnection.ENUMERATION_TYPE_CONNECTED Or _
           enumerationType = IPConnection.ENUMERATION_TYPE_AVAILABLE Then
            If deviceIdentifier = BrickletIndustrialDigitalIn4.DEVICE_IDENTIFIER Then
                Try
                    brickletIndustrialDigitalIn4 = New BrickletIndustrialDigitalIn4(UID, ipcon)
                    brickletIndustrialDigitalIn4.SetDebouncePeriod(10000)
                    brickletIndustrialDigitalIn4.SetInterrupt(15)
                    AddHandler brickletIndustrialDigitalIn4.Interrupt, AddressOf InterruptCB
                    System.Console.WriteLine("Industrial Digital In 4 initialized")
                Catch e As TinkerforgeException
                    System.Console.WriteLine("Industrial Digital In 4 init failed: " + e.Message)
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
        System.Console.ReadKey()
        ipcon.Disconnect()
    End Sub
End Module
