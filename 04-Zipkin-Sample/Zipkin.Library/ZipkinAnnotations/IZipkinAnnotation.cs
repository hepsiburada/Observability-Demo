using zipkin4net.Annotation;

namespace Zipkin.Library
{
    public interface IZipkinAnnotation
    {
        IAnnotation AnnotationStart(string message);
        IAnnotation AnnotationStop();
    }
}
