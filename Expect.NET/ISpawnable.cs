using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpectNet
{
    /// <summary>
    /// Defines how to initialize, read and write from underlying objects.
    /// </summary>
    public interface ISpawnable
    {
        /// <summary>
        /// Initializes underlying objects.
        /// </summary>
        void Init();

        /// <summary>
        /// Writes to underlying objects.
        /// </summary>
        /// <param name="command">text to write to underlying objects</param>
        void Write(string command);
        
        /// <summary>
        /// Reads from underlying objects.
        /// </summary>
        /// <returns>string read from underlying objects</returns>
        string Read();

        /// <summary>
        /// Reads in asynchronous mode from underlying objects.
        /// </summary>
        /// <returns>string read from underlying objects</returns>
        System.Threading.Tasks.Task<string> ReadAsync();
    }
}
