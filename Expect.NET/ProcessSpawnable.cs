//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Expect
//{
//    /// <summary>
//    /// Represents shell command to be spawned
//    /// </summary>
//    public class SpawnCommand : Spawn
//    {
//        /// <summary>
//        /// Initializes new SpawnCommand instance to handle shell command process 
//        /// </summary>
//        /// <param name="command">shell command to be spawned</param>
//        public SpawnCommand(string command)
//            : base(new CommandBackendFactory(command))
//        {
//        }

//        /// <summary>
//        /// Initializes new SpawnCommand instance to handle shell command process 
//        /// </summary>
//        /// <param name="command">shell command to be spawned</param>
//        /// <param name="arguments">commandline arguments to be passed to the spawned command</param>
//        public SpawnCommand(string command, string arguments)
//            : base(new CommandBackendFactory(command, arguments))
//        {
//        }

//    }
//}
