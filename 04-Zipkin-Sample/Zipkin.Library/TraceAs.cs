using System;
using System.Collections.Generic;
using zipkin4net;

namespace Zipkin.Library
{
    public interface ITraceFactory
    {
        ITraceAs Local(string serviceName, string message, OperationType operationType,
            Dictionary<string, string> tags = null, bool hasInnerTrace = false);
    }
    public enum OperationType
    {
        LocalOperation, Client, Server
    }

    public interface ITraceAs :IDisposable
    {
    }
    
    
    public class TraceAs : ITraceAs
    {

        public static ITraceAs Local(string serviceName, string message, OperationType operationType, Dictionary<string, string> tags = null, bool hasInnerTrace=false)
        {
            return new TraceAs(serviceName, message, operationType, tags, hasInnerTrace);
        }

        private readonly Trace trace;
        private IZipkinAnnotation zipkinAnnotation;
        private readonly bool _hasInnerTrace;
        public TraceAs(string serviceName, string message, OperationType operationType, Dictionary<string, string> tags=null, bool hasInnerTrace=false)
        {
            try
            {
                _hasInnerTrace = hasInnerTrace;
                zipkinAnnotation = ZipkinAnnotationFactory.GetAnnotationObject(operationType);

                trace = Trace.Current == null ? Trace.Create() : Trace.Current.Child();
                
                trace.Record(Annotations.ServiceName(serviceName));
                trace.Record(zipkinAnnotation.AnnotationStart(message));
                trace.Record(Annotations.Rpc(message));
                if (tags != null)
                {
                    foreach (var key in tags.Keys)
                    {
                        trace.Record(Annotations.Tag(key, tags[key]));
                    }
                }

                if (hasInnerTrace)
                {
                    Trace.Current = trace;
                }
            }
            catch (Exception e)
            {
            }
           
        }
        public void Dispose()
        {
            try
            {
                if (_hasInnerTrace && !trace.CurrentSpan.ParentSpanId.HasValue)
                    Trace.Current = null;

                trace.Record(zipkinAnnotation.AnnotationStop());

            }
            catch (Exception e)
            {

            }
        }
    }
}