using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using SastImg.Client.Service.API;

namespace SastImg.Client.Services;
public class SastImgAPI
{
    public IAccountApi Account { get; private set; }
    public IImageApi Image { get; private set; }
    public IAlbumApi Album { get; private set; }
    public ICategoryApi Category { get; private set; }
    public ITagApi Tag { get; private set; }
    public IUserApi User { get; private set; }

    public SastImgAPI (string endpointUrl)
    {
        Account = RestService.For<IAccountApi>("http://sastwoc2024.shirasagi.space:5265/", new() { AuthorizationHeaderValueGetter = GetToken() });
        Image = RestService.For<IImageApi>("http://sastwoc2024.shirasagi.space:5265/", new() { AuthorizationHeaderValueGetter = GetToken() });
        Album = RestService.For<IAlbumApi>("http://sastwoc2024.shirasagi.space:5265/", new() { AuthorizationHeaderValueGetter = GetToken() });
        Category = RestService.For<ICategoryApi>("http://sastwoc2024.shirasagi.space:5265/", new() { AuthorizationHeaderValueGetter = GetToken() });
        Tag = RestService.For<ITagApi>("http://sastwoc2024.shirasagi.space:5265/", new() { AuthorizationHeaderValueGetter = GetToken() });
        User = RestService.For<IUserApi>("http://sastwoc2024.shirasagi.space:5265/", new() { AuthorizationHeaderValueGetter = GetToken() });
    }

    private static Func<HttpRequestMessage, CancellationToken, Task<string>> GetToken ( )
    {
        return (_, _) => Task.FromResult(App.AuthService.Token ?? "");
    }
}
