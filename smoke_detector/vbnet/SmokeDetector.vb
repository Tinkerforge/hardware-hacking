Imports Tinkerforge

Module SmokeDetector
    Const HOST As String = "localhost"
    Const PORT As Integer = 4223

    Private ipcon As IPConnection = Nothing
    Private brickletAnalogIn As BrickletAnalogIn = Nothing

    Sub VoltageReachedCB(ByVal sender As BrickletAnalogIn, ByVal voltage As Integer)
        System.Console.WriteLine("Fire! Fire!")
    End Sub

    Sub EnumerateCB(ByVal sender As IPConnection, ByVal uid As String, _
                    ByVal connectedUid As String, ByVal position As Char, _
                    ByVal hardwareVersion() As Short, ByVal firmwareVersion() As Short, _
                    ByVal deviceIdentifier As Integer, ByVal enumerationType As Short)
        If enumerationType = IPConnection.ENUMERATION_TYPE_CONNECTED Or _
           enumerationType = IPConnection.ENUMERATION_TYPE_AVAILABLE Then
            If deviceIdentifier = BrickletAnalogIn.DEVICE_IDENTIFIER Then
                Try
                    brickletAnalogIn = New BrickletAnalogIn(UID, ipcon)
                    brickletAnalogIn.SetRange(1)
                    brickletAnalogIn.SetDebouncePeriod(10000)
                    brickletAnalogIn.SetVoltageCallbackThreshold(">"C, 1200, 0)
                    AddHandler brickletAnalogIn.VoltageReached, AddressOf VoltageReachedCB
                    System.Console.WriteLine("Analog In initialized")
                Catch e As TinkerforgeException
                    System.Console.WriteLine("Analog In init failed: " + e.Message)
                    brickletAnalogIn = Nothing
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
