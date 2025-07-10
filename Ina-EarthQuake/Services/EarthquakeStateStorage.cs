namespace Ina_EarthQuake.Services
{
    using Ina_EarthQuake.Models;
    using Windows.Storage;

    public static class EarthquakeStateStorage
    {
        private const string Key = "LastEarthquakeSignature";

        // Buat signature unik dari tiga properti utama
        public static string GenerateSignature(EarthquakeInfo info)
        {
            return $"{info.DateTime}|{info.Magnitude}|{info.Wilayah}";
        }

        // Simpan signature terakhir ke LocalSettings
        public static void Save(EarthquakeInfo info)
        {
            string signature = GenerateSignature(info);
            ApplicationData.Current.LocalSettings.Values[Key] = signature;
        }

        // Ambil signature terakhir dari LocalSettings
        public static string? GetLastSignature()
        {
            return ApplicationData.Current.LocalSettings.Values[Key] as string;
        }

        // Bandingkan apakah data saat ini berbeda dari yang terakhir disimpan
        public static bool IsNewEarthquake(EarthquakeInfo current)
        {
            string? lastSignature = GetLastSignature();
            string currentSignature = GenerateSignature(current);

            return currentSignature != lastSignature;
        }
    }
}
