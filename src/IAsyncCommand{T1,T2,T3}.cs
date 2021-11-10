﻿using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Delobytes.AspNetCore;

/// <summary>
/// Executes a single command with three parameters and returns a result.
/// </summary>
/// <typeparam name="T1">The type of the first parameter.</typeparam>
/// <typeparam name="T2">The type of the second parameter.</typeparam>
/// <typeparam name="T3">The type of the third parameter.</typeparam>
public interface IAsyncCommand<T1, T2, T3>
{
    /// <summary>
    /// Executes the command asynchronously.
    /// </summary>
    /// <param name="parameter1">The first parameter.</param>
    /// <param name="parameter2">The second parameter.</param>
    /// <param name="parameter3">The third parameter.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the command.</returns>
    Task<IActionResult> ExecuteAsync(
        T1 parameter1,
        T2 parameter2,
        T3 parameter3,
        CancellationToken cancellationToken = default);
}
