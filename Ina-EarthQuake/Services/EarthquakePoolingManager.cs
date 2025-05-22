namespace Ina_EarthQuake.Services
{
    public class EarthquakePollingManager
    {
        private static EarthquakePollingManager? _instance;
        public static EarthquakePollingManager Instance => _instance ??= new EarthquakePollingManager();

        private readonly EarthquakePollingService _service = new();

        public void Start() => _service.Start();
        public void Stop() => _service.Stop();
    }
}
