using Kryptonite.Utils;

namespace Kryptonite.Types.Exceptions;

/// <summary>
///     General exception for Kryptonite classes.
/// </summary>
public class KryptoniteException : Exception
{
    public KryptoniteException(string message)
    {
        Terminal.Log($"An unexpected error has occurred: {message}\nPress any key to continue...", hold: true);
    }
}