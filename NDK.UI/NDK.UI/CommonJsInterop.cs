using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.UI
{
    public sealed class CommonJsInterop : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public CommonJsInterop(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/NDK.UI/common.js").AsTask());
        }

        public async Task SetInputValue(ElementReference element,string value)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("setInputValue",element, value);
        }

        public async Task<string?> GetInputValue(ElementReference element)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<string>("getInputValue", element);
        }

        public async Task SetFocus(ElementReference element)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("setFocus", element);
        }
        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
