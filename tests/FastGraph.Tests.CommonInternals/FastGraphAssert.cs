#nullable enable

using System.Diagnostics.CodeAnalysis;
using System.Text;
using JetBrains.Annotations;

namespace FastGraph.Tests
{
    /// <summary>
    /// Additional assertion helpers.
    /// </summary>
    internal static class FastGraphAssert
    {
        /// <summary>
        /// Contains the exception from executing code, if an exception was thrown.
        /// </summary>
        private readonly record struct CatchResult(Exception? Exception)
        {
            /// <summary>
            /// Contains the exception object, if any.
            /// </summary>
            public Exception? Exception { get; } = Exception;

            /// <summary>
            /// Indicates whether the structure contains an exception.
            /// </summary>
            public bool HasException => Exception != default;

            /// <summary>
            /// Contains the exception type.
            /// </summary>
            /// <remarks>If the structure does not contain an exception object, this property is null.</remarks>
            public Type? ExceptionType => Exception?.GetType();

            private bool PrintMembers(StringBuilder builder)
            {

                if (HasException)
                {
                    builder.Append("raised '");
                    builder.Append(ExceptionType);
                    builder.Append("' ");
                }
                else
                {
                    builder.Append("returned ");
                }

                return true;
            }
        }

        /// <summary>
        /// Executes a specified delegate and returns the result.
        /// </summary>
        /// <param name="action">An <see cref="Action"/> delegate that performs a user-defined action.</param>
        /// <returns>Returns a <see cref="CatchResult"/> structure that contains an exception object,
        /// if an exception was thrown.</returns>
        private static CatchResult Catch([InstantHandle] Action action)
        {
            try
            {
                action();
                return new CatchResult();
            }
            catch (Exception ex)
            {
                return new CatchResult(ex);
            }
        }

        /// <summary>
        /// Asserts that the observable behavior of two delegates is the same.
        /// </summary>
        /// <overloads>Asserts that the observable behavior of two delegates is the same.</overloads>
        /// <param name="left">An <see cref="Action"/> delegate that performs a user-defined action.</param>
        /// <param name="right">An <see cref="Action"/> delegate that performs a user-defined action.</param>
        /// <remarks>"Same behavior" is defined as both delegates throwing the same exception or neither delegate throwing an exception.</remarks>
        [CustomAssertion]
        public static void AreBehaviorsEqual(
            [InstantHandle] Action left,
            [InstantHandle] Action right)
        {
            CatchResult catchResult1 = Catch(left);
            CatchResult catchResult2 = Catch(right);

            catchResult1.Should().Be(catchResult2);
        }

        /// <summary>
        /// Contains either a value returned by executing code or the exception that was thrown.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        public readonly record struct CatchResult<T>
        {
            /// <summary>
            /// Contains the return value, if any.
            /// </summary>
            public T? Value { get; }

            /// <summary>
            /// Contains the exception, if any.
            /// </summary>
            public Exception? Exception { get; }

            /// <summary>
            /// Initializes a new instance of <see cref="CatchResult{T}" />.
            /// </summary>
            /// <param name="value">A return value.</param>
            public CatchResult(T? value)
            {
                Value = value;
                Exception = default;
            }

            /// <summary>
            /// Initializes a new instance of <see cref="CatchResult{T}" />.
            /// </summary>
            /// <param name="exception">An exception object.</param>
            public CatchResult(Exception exception)
            {
                Value = default;
                Exception = exception;
            }

            /// <summary>Tries to get the return value.</summary>
            /// <param name="value">Receives the return value. If the structure does not contain a return value,
            /// this parameter is set to <c>default(T)</c>.</param>
            /// <returns>Returns <c>true</c> if the structure contains a return value and<c>false</c> otherwise.</returns>
            [Pure]
            public bool TryGetValue([NotNullWhen(true)] out T? value)
            {
                if (HasValue)
                {
                    value = Value!;
                    return true;
                }
                value = default;
                return false;
            }

            /// <summary>Tries to get the exception object.</summary>
            /// <param name="exception">Receives the exception object.If the structure does not contain an exception object,
            /// this parameter is set to <c>default</c>.</param>
            /// <returns>Returns <c>true</c> if the structure contains an exception object and<c>false</c> otherwise.</returns>
            [Pure]
            public bool TryGetException([NotNullWhen(true)] out Exception? exception)
            {
                exception = Exception;
                return exception != default;
            }

            /// <summary>
            /// Indicates whether the structure contains a return value.
            /// </summary>
            [MemberNotNullWhen(true, nameof(Value))]
            public bool HasValue => Exception == default;

            /// <summary>
            /// Indicates whether the structure contains an exception object.
            /// </summary>
            public bool HasException => Exception != default;

            /// <summary>
            /// Contains the exception type, if the structure contains an exception object.
            /// </summary>
            /// <remarks>If the structure does not contain an exception object, this property is null.</remarks>
            public Type? ExceptionType => Exception?.GetType();

            private bool PrintMembers(StringBuilder builder)
            {

                if (HasValue)
                {
                    builder.Append("result");
                } else if (HasException)
                {
                    builder.Append("raised");
                }

                builder.Append(" '");

                if (HasValue)
                {
                    builder.Append(Value);
                } else if (HasException)
                {
                    builder.Append(ExceptionType);
                }

                builder.Append("' ");

                return true;
            }
        }

        /// <summary>
        /// Executes a specified delegate and returns the result.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="function">An <see cref="Func{T}"/> delegate that performs a user-defined function
        /// and returns a value of type <typeparamref name="T"/>.</param>
        /// <returns>Returns a <see cref="CatchResult"/> structure that contains a return value or,
        /// if an exception was thrown, an exception object.</returns>
        private static CatchResult<T> Catch<T>([InstantHandle] Func<T> function)
        {
            try
            {
                return new CatchResult<T>(function());
            }
            catch (Exception ex)
            {
                return new CatchResult<T>(ex);
            }
        }

        /// <summary>
        /// Asserts that the observable behavior of two delegates is the same.
        /// </summary>
        /// <typeparam name="T">The delegates return type.</typeparam>
        /// <param name="left">A <see cref="Func{TResult}"/> delegate that performs a user-defined action and returns a value of type <typeparamref name="T"/>.</param>
        /// <param name="right">A <see cref="Func{TResult}"/> delegate that performs a user-defined action and returns a value of type <typeparamref name="T"/>.</param>
        /// <remarks>"Same behavior" is defined as both delegates returning the same value or both delegates throwing the same exception.</remarks>
        [CustomAssertion]
        public static void AreBehaviorsEqual<T>(
            [InstantHandle] Func<T> left,
            [InstantHandle] Func<T> right)
        {
            CatchResult<T> catchResult1 = Catch(left);
            CatchResult<T> catchResult2 = Catch(right);

            catchResult1.Should().Be(catchResult2);
        }
    }
}
