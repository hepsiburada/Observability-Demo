using zipkin4net;
using zipkin4net.Annotation;

namespace Zipkin.Library
{
    public class ZipkinLocalOperationAnnotation : IZipkinAnnotation
    {
        public IAnnotation AnnotationStart(string message)
        {
            return Annotations.LocalOperationStart(message);
        }

        public IAnnotation AnnotationStop()
        {
            return Annotations.LocalOperationStop();
        }
    }
}
