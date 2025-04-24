using Examy.Shared;
using Microsoft.JSInterop;

namespace Examy.Web.Services;

public class StorageService : IStorageService
{
    private readonly IJSRuntime _jSRuntime;

    public StorageService(IJSRuntime jSRuntime)
    {
        _jSRuntime = jSRuntime;
    }
    public async ValueTask<string> GetItem(string key)
        => await _jSRuntime.InvokeAsync<string>("localStorage.getItem", key);

    public async ValueTask RemoveItem(string key)

        =>  await _jSRuntime.InvokeVoidAsync("localStorage.removeItem", key);

    public async ValueTask SetItem(string key, string value)
        => await _jSRuntime.InvokeVoidAsync("localStorage.setItem", key, value);

       
}