using System;
using System.Security.Claims;

namespace Delobytes.AspNetCore;

/// <summary>
/// <see cref="ClaimsPrincipal"/> extension methods.
/// </summary>
public static class ClaimPrincipalExtensions
{
    /// <summary>
    /// Gets the strongly typed <typeparamref name="T"/> claim value from the specified <paramref name="claimName"/> or returns
    /// <c>default</c> if the claim was not found.
    /// </summary>
    /// <typeparam name="T">The type of the claim value.</typeparam>
    /// <param name="principal">Principal to search the claim from.</param>
    /// <param name="claimName">Name of the claim to search for.</param>
    /// <returns>The T value or <c>default</c> if the key was not found.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="principal"/> or <paramref name="claimName"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">T is not supported.</exception>
    public static T? GetClaimValue<T>(this ClaimsPrincipal principal, string claimName)
    {
        ArgumentNullException.ThrowIfNull(principal);
        ArgumentNullException.ThrowIfNull(claimName);

        string? claimValue = principal.FindFirst(claimName)?.Value;

        if (claimValue is null)
        {
            return default;
        }

        if (typeof(T) == typeof(string))
        {
            return (T)Convert.ChangeType(claimValue, typeof(T));
        }
        else if (typeof(T) == typeof(int) || typeof(T) == typeof(long))
        {
            return (T)Convert.ChangeType(claimValue, typeof(T));
        }
        else if (typeof(T) == typeof(Guid))
        {
            bool parsed = Guid.TryParse(claimValue, out Guid guidClaimValue);
            return parsed ? (T)Convert.ChangeType(guidClaimValue, typeof(T)) : default;
        }
        else
        {
            throw new InvalidOperationException("Not supported type provided.");
        }
    }
}
