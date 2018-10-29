using zipkin4net;
using zipkin4net.Annotation;

namespace Zipkin.Library
{
    public class ZipkinClientAnnotations: IZipkinAnnotation
    {
        public IAnnotation AnnotationStart(string message)
        {
            return Annotations.ClientSend();
        }

        public IAnnotation AnnotationStop()
        {
            return Annotations.ClientRecv();
        }
    }
}
