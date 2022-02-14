namespace Kryptonite.Types.Exceptions;

/// <summary>
///     General exception for Kryptonite classes.
/// </summary>
public class KryptoniteException : Exception
{
    private static string message = "An unexpected error has occurred.";

    public KryptoniteException(string message)
    {
        KryptoniteException.message = message;
    }
}