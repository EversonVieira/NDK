﻿using NDK.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.UI.Components.Interfaces
{
    public interface NDKIDataSourceService<TOutput, TInput> where TInput: NdkRequest
    {
        public IEnumerable<TOutput> FetchData(TInput request);
    }
}