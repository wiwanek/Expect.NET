using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect
{
    /// <summary>
    /// Executes code when expected string is found by Expect function
    /// </summary>
    public delegate void ExpectedHandler();

    /// <summary>
    /// Executes code when expected string is found by Expect function.
    /// Receives session output to handle.
    /// </summary>
    /// <param name="output">session output with expected pattern</param>
    public delegate void ExpectedHandlerWithOutput(string output);

    public interface ISession
    {
        /// <summary>
        /// Sends characters to the session.
        /// </summary>
        /// <remarks>
        /// To send enter you have to add '\n' at the end.
        /// </remarks>
        /// <example>
        /// Send("cmd.exe\n");
        /// </example>
        /// <param name="command">String to be sent to session</param>
        void Send(string command);

        /// <summary>
        /// Waits until query is printed on session output and 
        /// executes handler
        /// </summary>
        /// <param name="query">expected output</param>
        /// <param name="handler">action to be performed</param>
        /// <exception cref="System.TimeoutException">Thrown when query is not find for given
        /// amount of time</exception>
        void Expect(string query, ExpectedHandler handler);

        /// <summary>
        /// Waits until query is printed on session output and 
        /// executes handler. The output including expected query is
        /// passed to handler.
        /// </summary>
        /// <param name="query">expected output</param>
        /// <param name="handler">action to be performed, it accepts session output as ana argument</param>
        /// <exception cref="System.TimeoutException">Thrown when query is not find for given
        /// amount of time</exception>
        void Expect(string query, ExpectedHandlerWithOutput handler);

        /// <summary>
        /// Waits until query is printed on session output and 
        /// executes handler
        /// </summary>
        /// <param name="query">expected output</param>
        /// <param name="handler">action to be performed</param>
        /// <exception cref="System.TimeoutException">Thrown when query is not find for given
        /// amount of time</exception>
        Task ExpectAsync(string query, ExpectedHandler handler);

        /// <summary>
        /// Waits until query is printed on session output and 
        /// executes handler. The output including expected query is
        /// passed to handler.
        /// </summary>
        /// <param name="query">expected output</param>
        /// <param name="handler">action to be performed, it accepts session output as ana argument</param>
        /// <exception cref="System.TimeoutException">Thrown when query is not find for given
        /// amount of time</exception>
        Task ExpectAsync(string query, ExpectedHandlerWithOutput handler);

        /// <summary>
        /// Returns configured timeout value for Expect function
        /// </summary>
        /// <returns>timeout value in miliseconds for Expect function</returns>
        int GetTimeout();

        /// <summary>
        /// Sets timeout value for Expect function
        /// </summary>
        /// <param name="timeout">timeout value in miliseconds for Expect function</param>
        void SetTimeout(int timeout);


    }
}
