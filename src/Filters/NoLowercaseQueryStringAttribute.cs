using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Delobytes.AspNetCore.Filters;

/// <summary>
/// Ensures that an HTTP request URL can contain query string parameters with both upper-case and lower-case
/// characters.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public class NoLowercaseQueryStringAttribute : Attribute, IFilterMetadata
{
}
