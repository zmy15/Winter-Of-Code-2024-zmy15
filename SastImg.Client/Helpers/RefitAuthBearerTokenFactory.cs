using System;
using System.Threading;
using System.Threading.Tasks;

namespace SastImg.Client.Helpers;
public class RefitAuthBearerTokenFactory
{
    private static Func<CancellationToken, Task<string>>? _getBearerTokenAsyncFunc;

    public static void SetBearerTokenGetterFunc ( ) => _getBearerTokenAsyncFunc = _ => Task.FromResult(App.AuthService.Token ?? "");

    public static Task<string> GetBearerTokenAsync (CancellationToken cancellationToken)
    {
        if ( _getBearerTokenAsyncFunc is null )
            throw new InvalidOperationException("先设置getBearerTokenAsyncFunc的方法");
        return _getBearerTokenAsyncFunc!(cancellationToken);
    }
}
