﻿using Microsoft.AspNetCore.Mvc;

namespace Delobytes.AspNetCore
{
    /// <summary>
    /// Executes a single command and returns a result.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <returns>The result of the command.</returns>
        IActionResult Execute();
    }
}
