using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using static LoafThePenguin.Helpers.Internal.ExceptionMessages;

namespace LoafThePenguin.Helpers;

public static class ThrowHelper
{
    private const string VALUE_ARGUMENT_NAME = "value";
    private const string SEQUENCE_ARGUMENT_NAME = "sequence";

    /// <summary>
    /// Throws exception.
    /// </summary>
    /// <typeparam name="T">
    /// Exception's type.
    /// </typeparam>
    /// <param name="args">
    /// Arguments' array.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Throws when <paramref name="args"/> are empty. Or when exception is obscure.
    /// </exception>
    /// <remarks>
    /// <paramref name="args"/> should include at least one argument as message.
    /// </remarks>
    [DoesNotReturn]
    public static void Throw<T>(params object?[] args)
        where T : Exception
    {
        ThrowIfArgumentNull(args);

        if (args.Length < 1)
        {
            Throw<InvalidOperationException>(NEED_AT_LEAST_A_MESSAGE);
        }

        throw (T)(Activator.CreateInstance(typeof(T), args)
            ?? throw new InvalidOperationException(OBSCURE_EXCEPTION_THROWN));
    }

    /// <summary>
    /// Throws <see cref="ObjectDisposedException"/>.
    /// </summary>
    /// <typeparam name="T">Type of disposed object.</typeparam>
    /// <param name="value">Disposed object.</param>
    /// <param name="argumentName">Name of disposed object.</param>
    /// <exception cref="ObjectDisposedException"/>
    [DoesNotReturn]
    [SuppressMessage("Style", "IDE0060:Удалите неиспользуемый параметр", Justification = "<Ожидание>")]
    public static bool ThrowDisposed<T>(
        [NotNullWhen(false)] T? value,
        [CallerArgumentExpression(VALUE_ARGUMENT_NAME)] string? argumentName = null)
    {
        Throw<ObjectDisposedException>(string.Format(
                OBJECT_DISPOSED,
                argumentName,
                typeof(T)
            ));

        return false;
    }

    /// <summary>
    /// Throws <see cref="ArgumentNullException"/> when <paramref name="value"/> is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T"><paramref name="value"/>'s type.</typeparam>
    /// <param name="value">Value.</param>
    /// <param name="argumentName">Argument name.</param>
    /// <returns>
    /// <see langword="false"/> if <paramref name="value"/> is not <see langword="null"/>,
    /// otherwise throws an <see cref="ArgumentNullException"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Throw when <paramref name="value"/> is <see langword="null"/>.</exception>
    public static bool ThrowIfArgumentNull<T>(
        [NotNullWhen(false)] T? value,
        [CallerArgumentExpression(VALUE_ARGUMENT_NAME)] string? argumentName = null)
    {
        if (value is null)
        {
            Throw<ArgumentNullException>(argumentName);
        }

        return false;
    }

    /// <summary>
    /// Throws <see cref="NullReferenceException"/> when <paramref name="value"/> is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T"><paramref name="value"/>'s type.</typeparam>
    /// <param name="value">Value.</param>
    /// <param name="argumentName">Argument name.</param>
    /// <returns>
    /// <see langword="false"/> if <paramref name="value"/> is not <see langword="null"/>,
    /// otherwise throws an <see cref="NullReferenceException"/>.
    /// </returns>
    /// <exception cref="NullReferenceException">Throw when <paramref name="value"/> is <see langword="null"/>.</exception>
    public static bool ThrowIfNull<T>(
        [NotNullWhen(false)] T? value,
        [CallerArgumentExpression(VALUE_ARGUMENT_NAME)] string? argumentName = null)
    {
        if (value is null)
        {
            Throw<NullReferenceException>(
                string.Format(NULL_REFERENCE,
                argumentName,
                typeof(T).Name));
        }

        return false;
    }

    /// <summary>
    /// Throws <see cref="ArgumentOutOfRangeException"/> when <paramref name="value"/>
    /// is out of bounds.
    /// </summary>
    /// <param name="value">
    /// <see cref="int"/> value.
    /// </param>
    /// <param name="permissibleMinimum">
    /// The minimum that a <paramref name="value"/> can be.
    /// </param>
    /// <param name="permissibleMaximum">
    /// The maximum that a <paramref name="value"/> can be.
    /// </param>
    /// <param name="argumentName">Argument name.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Throws when <paramref name="value"/> is out of bounds.
    /// </exception>
    public static void ThrowIfIntArgumentOutOfRange(
        int value,
        int permissibleMinimum = 0,
        int permissibleMaximum = int.MaxValue,
        [CallerArgumentExpression(VALUE_ARGUMENT_NAME)] string? argumentName = null)
    {
        if (value < permissibleMinimum)
        {
            Throw<ArgumentOutOfRangeException>(
                argumentName,
                string.Format(ARGUMENT_OUT_OF_RANGE_CANT_BE_LOWER,
                              argumentName,
                              permissibleMinimum));
        }

        if (value > permissibleMaximum)
        {
            Throw<ArgumentOutOfRangeException>(
                argumentName,
                string.Format(ARGUMENT_OUT_OF_RANGE_CANT_BE_HIGHER,
                              argumentName,
                              permissibleMaximum));
        }
    }

    /// <summary>
    /// Throws <see cref="IndexOutOfRangeException"/> when <paramref name="value"/>
    /// is lower than zero.
    /// </summary>
    /// <param name="value">
    /// <see cref="int"/> index.
    /// </param>
    /// <param name="argumentName">Argument name.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Throws when <paramref name="value"/> is out of bounds.
    /// </exception>
    public static void ThrowIfIndexLowerZero(int index)
    {
        if (index < 0)
        {
            Throw<IndexOutOfRangeException>(INDEX_CANT_BE_LOWER_TAHN_ZERO);
        }
    }

    /// <summary>
    /// Invokes <paramref name="action"/> when <paramref name="value"/> is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T"><paramref name="value"/>'s type.</typeparam>
    /// <param name="value">Value.</param>
    /// <param name="action">Action who invokes when <paramref name="value"/> is <see langword="null"/>..</param>
    /// <exception cref="ArgumentNullException">Throw when <paramref name="action"/> is <see langword="null"/>.</exception>
    public static void DoIfNull<T>(T? value, Action action)
    {
        ThrowIfArgumentNull(action);
        if (value is null)
        {
            action();
        }
    }

    /// <summary>
    /// Throws <see cref="NullReferenceException"/>.
    /// if any item in sequence <paramref name="sequence"/> is <see langword="null"/>
    /// </summary>
    /// <param name="sequence">Sequence.</param>
    /// <param name="argumentName">Argument's name.</param>
    /// <returns>
    /// <see langword="false"/> if all items are not <see langword="null"/>,
    /// otherwise throws <see cref="NullReferenceException"/>.
    /// </returns>
    /// <exception cref="NullReferenceException"/>
    public static bool ThrowIfAnyItemIsNull(
        IEnumerable sequence,
        [CallerArgumentExpression(SEQUENCE_ARGUMENT_NAME)] string? argumentName = null)
    {
        foreach (object enumeable in sequence)
        {
            if (enumeable is null)
            {
                Throw<NullReferenceException>(string.Format(SEQUENCE_HAS_NULL_REFERENCE, argumentName));

                return true;
            }
        }

        return false;
    }
}
