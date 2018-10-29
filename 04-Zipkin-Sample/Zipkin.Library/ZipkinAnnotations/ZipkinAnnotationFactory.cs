namespace Zipkin.Library
{
    public class ZipkinAnnotationFactory
    {
        public static IZipkinAnnotation GetAnnotationObject(OperationType operationType)
        {
            IZipkinAnnotation zipkinAnnotation = null;
            switch (operationType)
            {
                case OperationType.LocalOperation:
                    zipkinAnnotation = new ZipkinLocalOperationAnnotation();
                    break;
                case OperationType.Client:
                    zipkinAnnotation = new ZipkinClientAnnotations();
                    break;
                case OperationType.Server:
                    zipkinAnnotation = new ZipkinServerAnnotations();
                    break;
            }

            return zipkinAnnotation;
        }
    }


}
