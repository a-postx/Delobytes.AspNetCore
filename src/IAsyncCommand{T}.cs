using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Delobytes.AspNetCore
{
    /// <summary>
    /// Execute single command with one parameter and return result.
    /// </summary>
    /// <typeparam name="T">Parameter type.</typeparam>
    public interface IAsyncCommand<T>
    {
        /// <summary>
        /// Execute command asynchronously.
        /// </summary>
        /// <param name="parameter">Parameter.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Command result.</returns>
        Task<IActionResult> ExecuteAsync(T parameter, CancellationToken cancellationToken = default);
    }
}
