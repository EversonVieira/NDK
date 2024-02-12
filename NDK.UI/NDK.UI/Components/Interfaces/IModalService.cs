using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.UI.Components.Interfaces
{
    public interface IModalService
    {
        public Task OpenModal<T>(T component, string? title = null) where T : ComponentBase;
    }
}
