using App.Metrics;
using App.Metrics.Apdex;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using App.Metrics.ReservoirSampling.Uniform;
using App.Metrics.Timer;

namespace webApi
{
    public static class MyMetricsRegistry
    {
        public static GaugeOptions Errors => new GaugeOptions
        {
            Name = "Errors"
        };

        public static CounterOptions SampleCounter => new CounterOptions
        {
            Name = "Sample Counter",
            MeasurementUnit = Unit.Calls,
        };

        public static HistogramOptions SampleHistogram => new HistogramOptions
        {
            Name = "Sample Histogram",
            Reservoir = () => new DefaultAlgorithmRReservoir(),
            MeasurementUnit = Unit.MegaBytes
        };

        public static MeterOptions SampleMeter => new MeterOptions
        {
            Name = "Sample Meter",
            MeasurementUnit = Unit.Calls
        };

        public static TimerOptions SampleTimer => new TimerOptions
        {
            Name = "Sample Timer",
            MeasurementUnit = Unit.Items,
            DurationUnit = TimeUnit.Milliseconds,
            RateUnit = TimeUnit.Milliseconds,
            Reservoir = () => new DefaultForwardDecayingReservoir(sampleSize: 1028, alpha: 0.015)
        };

        public static ApdexOptions SampleApdex => new ApdexOptions
        {
            Name = "Sample Apdex"
        };
    }
}