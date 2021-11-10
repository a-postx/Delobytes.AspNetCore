using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Delobytes.AspNetCore;

/// <summary>
/// Execute single command with two parameters and return result.
/// </summary>
/// <typeparam name="T1">First parameter type.</typeparam>
/// <typeparam name="T2">Second parameter type.</typeparam>
public interface IAsyncCommand<T1, T2>
{
    /// <summary>
    /// Execute command asynchronously.
    /// </summary>
    /// <param name="parameter1">First parameter.</param>
    /// <param name="parameter2">Second parameter.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Command result.</returns>
    Task<IActionResult> ExecuteAsync(T1 parameter1, T2 parameter2, CancellationToken cancellationToken = default);
}
