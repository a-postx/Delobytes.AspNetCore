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
    /// <c>default</c> if the key was not found.
    /// </summary>
    /// <typeparam name="T">The type of the claim value.</typeparam>
    /// <param name="claimName">Name of the claim to search for.</param>
    /// <returns>The <typeparam name="T"/> value or <c>default</c> if the key was not found.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="principal"/> or <paramref name="claimName"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException"><typeparam name="T"/> is not supported.</exception>
    public static T GetClaimValue<T>(this ClaimsPrincipal principal, string claimName)
    {
        if (principal == null)
        {
            throw new ArgumentNullException(nameof(principal));
        }
        if (claimName == null)
        {
            throw new ArgumentNullException(nameof(claimName));
        }

        string claimValue = principal.FindFirst(claimName)?.Value;

        if (claimValue != null)
        {
            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(claimValue, typeof(T));
            }
            else if (typeof(T) == typeof(int) || typeof(T) == typeof(long))
            {
                return claimValue != null ? (T)Convert.ChangeType(claimValue, typeof(T)) : (T)Convert.ChangeType(0, typeof(T));
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
        else
        {
            return default;
        }
    }
}
