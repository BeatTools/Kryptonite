using System.Security;

namespace Kryptonite.Types;

public class User
{
    public User(string? name, SecureString? password)
    {
        this.name = name;
        this.password = password;
    }

    public string name { get; }
    public SecureString password { get; }
}