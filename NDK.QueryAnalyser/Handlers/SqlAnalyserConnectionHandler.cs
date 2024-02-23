﻿using NDK.Database.Handlers;
using NDK.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.QueryAnalyser.Core.Handlers
{
    public class SqlAnalyserConnectionHandler : NDKDbConnectionFactory
    {
        public SqlAnalyserConnectionHandler(NDKDbConnectionConfiguration configuration) : base(configuration)
        {
        }
    }
}
