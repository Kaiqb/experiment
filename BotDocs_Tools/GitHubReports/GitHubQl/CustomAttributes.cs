using System;
using System.Collections.Generic;
using System.Text;

namespace GitHubQl
{
    /// <summary>Signifies that this type correspondes to a QL object.</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class QlObjectAttribute : Attribute
    {
    }

    /// <summary>Signifies that this type correspondes to a QL enumeration.</summary>
    [AttributeUsage(AttributeTargets.Enum, AllowMultiple = false)]
    public class QlEnumAttribute : Attribute
    {
    }

    /// <summary>Signifies that this type correspondes to a QL connection.</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class QlConnectionAttribute : Attribute
    {
    }

    /// <summary>Signifies that this type correspondes to a QL edge.</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class QlEdgeAttribute : Attribute
    {
    }

    /// <summary>Signifies that this property correspondes to an array value.</summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class QlArrayAttribute : Attribute
    {
    }
}
