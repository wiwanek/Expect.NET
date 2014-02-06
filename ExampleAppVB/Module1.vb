Imports Expect

Module Module1

    Sub Main()
        Dim spawn As Spawn = New SpawnCommand("cmd.exe")
        Try
            spawn.Expect(">", Sub(s) Console.WriteLine("Prompt --> " + s))
            spawn.SetTimeout(1000)
            spawn.Send("dir c:\" + Environment.NewLine)
            spawn.Expect("Program Files", Sub(s) Console.WriteLine(s))
        Catch ex As System.TimeoutException
            Console.WriteLine("Timeout")
        End Try

        Console.WriteLine("Done")
        Console.ReadKey()
    End Sub

End Module
