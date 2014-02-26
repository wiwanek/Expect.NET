//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Expect
//{
//    class CommandBackendFactory : IBackendFactory
//    {
//        private string _command;
//        private string _arguments;

//        internal CommandBackendFactory(string command, string arguments)
//        {
//            _command = command;
//            _arguments = arguments;
//        }

//        internal CommandBackendFactory(string command) : this(command, "")
//        {
//        }

//        public IBackend CreateBackend()
//        {
//            Process process = new Process();

//            process.StartInfo.FileName = _command;
//            process.StartInfo.Arguments = _arguments;
//            process.StartInfo.UseShellExecute = false;
//            process.StartInfo.RedirectStandardInput = true;
//            process.StartInfo.RedirectStandardError = true;
//            process.StartInfo.RedirectStandardOutput = true;

//            process.Start();

//            return new CommandBackend(process);
//        }
//    }
//}
