using System;

namespace Zipkin.Library
{
    public class ZipkinConfiguration
    {
        public const string DefaultUrl = "localhost:9411";
        public const string DefaultContentType = "application/json";
        public const float DefaultSamplingRate = 20 % 100;
        
        public string Url { get; set; }
        public string ContentType { get; set; }
        public float SamplingRate { get; set; }

        public ZipkinConfiguration():this(DefaultUrl)
        {
            
        }
        public ZipkinConfiguration(string url)
        {
            Url = url=="" ? throw new ArgumentNullException("Url"):url;
        }
    }
}