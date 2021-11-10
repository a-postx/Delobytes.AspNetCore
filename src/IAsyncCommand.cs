using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Delobytes.AspNetCore;

/// <summary>
/// Execute single command and return result.
/// </summary>
public interface IAsyncCommand
{
    /// <summary>
    /// Execute command asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Command result.</returns>
    Task<IActionResult> ExecuteAsync(CancellationToken cancellationToken = default);
}
