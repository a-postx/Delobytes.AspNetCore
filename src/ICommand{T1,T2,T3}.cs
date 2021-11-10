﻿using Microsoft.AspNetCore.Mvc;

namespace Delobytes.AspNetCore;

/// <summary>
/// Executes a single command with three parameters and returns a result.
/// </summary>
/// <typeparam name="T1">The type of the first parameter.</typeparam>
/// <typeparam name="T2">The type of the second parameter.</typeparam>
/// <typeparam name="T3">The type of the third parameter.</typeparam>
public interface ICommand<T1, T2, T3>
{
    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="parameter1">The first parameter.</param>
    /// <param name="parameter2">The second parameter.</param>
    /// <param name="parameter3">The third parameter.</param>
    /// <returns>
    /// The result of the command.
    /// </returns>
    IActionResult Execute(T1 parameter1, T2 parameter2, T3 parameter3);
}
