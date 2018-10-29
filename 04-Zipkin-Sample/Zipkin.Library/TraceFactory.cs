using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using zipkin4net;
using zipkin4net.Propagation;


namespace Zipkin.Library
{
    public class TraceFactory : ITraceFactory
    {
        public ITraceAs Local(string serviceName, string message, OperationType operationType, Dictionary<string, string> tags = null, bool hasInnerTrace=false)
        {
            return new TraceAs(serviceName, message, operationType, tags, hasInnerTrace);
        }
    }
}