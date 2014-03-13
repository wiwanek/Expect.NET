Imports ExpectNET


Module Module1

    Sub Main()
        Dim session As ISession = Expect.Spawn(New ProcessSpawnable("cmd.exe"))
        Try
            session.Expect(">", Sub(s) Console.WriteLine("Prompt --> " + s))
            session.SetTimeout(1000)
            session.Send("dir c:\" + Environment.NewLine)
            session.Expect("Program Files", Sub(s) Console.WriteLine(s))
            session.Expect(">", Sub() session.Send("ping 8.8.8.8" + Environment.NewLine))
        Catch ex As System.TimeoutException
            Console.WriteLine("Timeout")
        End Try
        session.SetTimeout(5000)
        session.Expect("Lost = 0", Sub() session.Send("ping 8.8.8.8" + Environment.NewLine))
        session.Expect("Lost = 0", Sub(s) Console.WriteLine(s))
        Console.WriteLine("Done")
        Console.ReadKey()
    End Sub

End Module
