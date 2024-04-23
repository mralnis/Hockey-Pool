﻿using Microsoft.JSInterop;

namespace HockeyPool.Services
{
    public class ClipboardService
    {
        private readonly IJSRuntime _jsInterop;

        public ClipboardService(IJSRuntime jsInterop)
        {
            _jsInterop = jsInterop;
        }

        public async Task CopyToClipboard(string text)
        {
            await _jsInterop.InvokeVoidAsync("navigator.clipboard.writeText", text);
        }
    }
}
