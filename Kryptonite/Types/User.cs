using System.Security;

namespace Kryptonite.Types;

public class User
{
    public User(string name, SecureString password)
    {
        Name = name;
        Password = password;
    }

    /// <summary>
    ///     The name of the user
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     The password of the user
    /// </summary>
    /// <remarks>
    ///     This is a <see cref="SecureString" />, so it is not stored in plain text, and gets wiped from memory when it is no
    ///     longer needed.
    /// </remarks>
    public SecureString Password { get; }
}