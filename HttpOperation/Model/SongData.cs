using System.Collections.Generic;
using Newtonsoft.Json;

namespace HttpOperation.Model
{
    public class SongData
    {
        [JsonProperty("SongId")] public string SongId { get; set; }
        [JsonProperty("Rows")] public List<RowInfo> RowInfos { get; set; }
        [JsonProperty("SingerId")] public string SingerId { get; set; }
        [JsonProperty("TrackIndex")] public int TrackIndex { get; set; }
    }
}