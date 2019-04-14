#if !SUPPORTS_TYPE_FULL_FEATURES
using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;

namespace QuikGraph.Utils
{
    /// <summary>
    /// Helper to replace type standard utilities when not available in target version.
    /// </summary>
    internal static class TypeUtils
    {
        [NotNull]
        private static readonly Dictionary<Type, TypeCode> TypeCodeMap = new Dictionary<Type, TypeCode>(15)
        {
            { typeof(sbyte), TypeCode.SByte },
            { typeof(byte), TypeCode.Byte },
            { typeof(short), TypeCode.Int16 },
            { typeof(ushort), TypeCode.UInt16 },
            { typeof(int), TypeCode.Int32 },
            { typeof(uint), TypeCode.UInt32 },
            { typeof(long), TypeCode.Int64 },
            { typeof(ulong), TypeCode.UInt64 },
            { typeof(float), TypeCode.Single },
            { typeof(double), TypeCode.Double },
            { typeof(decimal), TypeCode.Decimal},
            { typeof(DateTime), TypeCode.DateTime },
            { typeof(string), TypeCode.String },
            { typeof(bool), TypeCode.Boolean },
            { typeof(char), TypeCode.Char }
        };

        /// <summary>
        /// Get the <see cref="TypeCode"/> of the given <see cref="Type"/>.
        /// </summary>
        /// <remarks>Kind of equivalent to the System function for compatibility, but not really...</remarks>
        /// <param name="type"><see cref="Type"/> to get the <see cref="TypeCode"/>.</param>
        /// <returns>A <see cref="TypeCode"/>.</returns>
        public static TypeCode GetTypeCode([CanBeNull] Type type)
        {
            if (type is null)
                return TypeCode.Empty;

            if (TypeCodeMap.TryGetValue(type, out TypeCode typeCode))
                return typeCode;

            if (type.GetTypeInfo().IsEnum)
                return GetTypeCode(Enum.GetUnderlyingType(type));

            return TypeCode.Object;
        }
    }
}
#endif