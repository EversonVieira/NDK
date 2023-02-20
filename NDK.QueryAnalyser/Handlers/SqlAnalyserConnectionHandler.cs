using NDK.Database.Handlers;
using NDK.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.QueryAnalyser.Handlers
{
    public class SqlAnalyserConnectionHandler : NdkDbConnectionHandler
    {
        public SqlAnalyserConnectionHandler(NdkDbConnectionConfiguration configuration) : base(configuration)
        {
        }
    }
}
