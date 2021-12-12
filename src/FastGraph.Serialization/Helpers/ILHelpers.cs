#nullable enable

#if SUPPORTS_GRAPHS_SERIALIZATION
using System.Reflection;
using System.Reflection.Emit;

namespace FastGraph.Serialization
{
    /// <summary>
    /// Helpers to manipulate <see cref="ILGenerator"/> and dynamic stuff.
    /// </summary>
    internal static class ILHelpers
    {
        public static void EmitValue(ILGenerator generator, PropertyInfo property, object value)
        {
            switch (Type.GetTypeCode(property.PropertyType))
            {
                case TypeCode.Int32:
                    generator.Emit(OpCodes.Ldc_I4, (int)value);
                    break;
                case TypeCode.Int64:
                    generator.Emit(OpCodes.Ldc_I8, (long)value);
                    break;
                case TypeCode.Single:
                    generator.Emit(OpCodes.Ldc_R4, (float)value);
                    break;
                case TypeCode.Double:
                    generator.Emit(OpCodes.Ldc_R8, (double)value);
                    break;
                case TypeCode.String:
                    generator.Emit(OpCodes.Ldstr, (string)value);
                    break;
                case TypeCode.Boolean:
                    generator.Emit((bool)value ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
                    break;
                default:
                    throw new NotSupportedException($"Unsupported type {property.PropertyType.FullName}.");
            }
        }

        public static void EmitCall(ILGenerator generator, MethodInfo method)
        {
            generator.EmitCall(
                method.IsVirtual
                    ? OpCodes.Callvirt
                    : OpCodes.Call,
                method,
                default);
        }
    }
}
#endif
