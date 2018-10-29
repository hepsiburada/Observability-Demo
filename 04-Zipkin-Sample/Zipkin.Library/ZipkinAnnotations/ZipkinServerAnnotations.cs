using zipkin4net;
using zipkin4net.Annotation;

namespace Zipkin.Library
{
    public class ZipkinServerAnnotations : IZipkinAnnotation
    {
        public IAnnotation AnnotationStart(string message)
        {
            return Annotations.ServerRecv();
        }

        public IAnnotation AnnotationStop()
        {
            return Annotations.ServerSend();
        }
    }
}
