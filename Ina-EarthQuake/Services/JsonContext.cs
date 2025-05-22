using System.Text.Json.Serialization;

namespace Ina_EarthQuake.Services
{ 
    [JsonSerializable(typeof(UpdateChecker.UpdateInfo))]
    internal partial class UpdateInfoJsonContext : JsonSerializerContext
    {

    }
}
