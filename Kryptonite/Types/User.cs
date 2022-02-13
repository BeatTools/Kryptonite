using System.Security;

namespace Kryptonite.Types;

public class User
{
    public User(string name, SecureString password)
    {
        Name = name;
        Password = password;
    }

    public string Name { get; }
    public SecureString Password { get; }
}