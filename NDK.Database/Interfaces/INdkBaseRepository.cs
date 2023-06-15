using Microsoft.Extensions.Logging;
using NDK.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Database.Interfaces
{
    public interface INdkBaseRepository
    {
        public void HandleException(Exception exception, NdkResponse response, ILogger logger);
    }
}
