﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Delobytes.AspNetCore;

/// <summary>
/// Reads or writes cursors which are opaque strings representing a position in a list of items.
/// </summary>
public static class Cursor
{
    /// <summary>
    /// Gets the strongly typed <typeparamref name="T"/> from the specified <paramref name="cursor"/>.
    /// </summary>
    /// <typeparam name="T">The type of the cursor value.</typeparam>
    /// <param name="cursor">The cursor.</param>
    /// <returns>The cursor value.</returns>
    public static T? FromCursor<T>(string cursor)
    {
        if (string.IsNullOrEmpty(cursor))
        {
            return default;
        }

        string decodedValue;
        try
        {
            decodedValue = Base64Decode(cursor);
        }
        catch (FormatException)
        {
            return default;
        }

        Type type = typeof(T);
        type = Nullable.GetUnderlyingType(type) ?? type;

        if (type == typeof(DateTime))
        {
            return (T)(object)DateTime.ParseExact(decodedValue, "o", CultureInfo.InvariantCulture);
        }

        if (type == typeof(DateTimeOffset))
        {
            return (T)(object)DateTimeOffset.ParseExact(decodedValue, "o", CultureInfo.InvariantCulture);
        }

        if (type == typeof(DateOnly))
        {
            return (T)(object)DateOnly.ParseExact(decodedValue, "o", CultureInfo.InvariantCulture);
        }

        return (T)Convert.ChangeType(decodedValue, type, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Gets the cursor for the first and last item in the collection.
    /// </summary>
    /// <typeparam name="TItem">The type of the items in the collection.</typeparam>
    /// <typeparam name="TCursor">The type of the cursor value.</typeparam>
    /// <param name="enumerable">The enumerable.</param>
    /// <param name="getCursorProperty">The get cursor property.</param>
    /// <returns>The first and last cursor in the collection.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> or 
    /// <paramref name="getCursorProperty"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException"><paramref name="enumerable"/> is empty.</exception>
    public static (string? firstCursor, string? lastCursor) GetFirstAndLastCursor<TItem, TCursor>(
        IEnumerable<TItem> enumerable,
        Func<TItem, TCursor> getCursorProperty)
    {
        ArgumentNullException.ThrowIfNull(enumerable);
        ArgumentNullException.ThrowIfNull(getCursorProperty);

        if (!enumerable.Any())
        {
            return (null, null);
        }

        string firstCursor = ToCursor(getCursorProperty(enumerable.First()));
        string lastCursor = ToCursor(getCursorProperty(enumerable.Last()));

        return (firstCursor, lastCursor);
    }

    /// <summary>
    /// Gets the cursor from the strongly typed <paramref name="value"/>.
    /// </summary>
    /// <typeparam name="T">The type of the cursor.</typeparam>
    /// <param name="value">The value.</param>
    /// <returns>The cursor.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException"><paramref name="value"/> is not converible to string.</exception>
    public static string ToCursor<T>(T value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value is DateTime dateTime)
        {
            return Base64Encode(dateTime.ToString("o", CultureInfo.InvariantCulture));
        }

        if (value is DateTimeOffset dateTimeOffset)
        {
            return Base64Encode(dateTimeOffset.ToString("o", CultureInfo.InvariantCulture));
        }

        if (value is DateOnly dateOnly)
        {
            return Base64Encode(dateOnly.ToString("o", CultureInfo.InvariantCulture));
        }

        string? rawValue = value.ToString();

        if (rawValue is not null)
        {
            return Base64Encode(rawValue);
        }
        else
        {
            throw new InvalidOperationException("Value is not convertable to string");
        }
    }

    private static string Base64Decode(string value) => Encoding.UTF8.GetString(Convert.FromBase64String(value));

    private static string Base64Encode(string value) => Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
}
