
using Core;

namespace Shared
{
    public interface ICredentialsProvider
    {
        Credentials Credentials { get; }
    }
}
