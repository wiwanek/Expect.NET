using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpectNet
{
    /// <summary>
    /// Top library class. 
    /// </summary>
    public class Expect
    {
        /// <summary>
        /// Creates spawned session to control input/output from underlying objects.
        /// </summary>
        /// <param name="spawnable">Definition how to create session</param>
        /// <returns>Spawned session</returns>
        public static Session Spawn(ISpawnable spawnable) 
        {
            spawnable.Init();
            return new Session(spawnable);
        }
    }
}
