using System.Collections.Generic;
using Newtonsoft.Json;

namespace HttpOperation
{
    public class RowInfo
    {
        [JsonProperty("lyric")] public string Lyric { get; set; }
        [JsonProperty("originalCharCount")] public int OriginalCharCount { get; set; }
        [JsonProperty("minCharCount")] public int MinCharCount { get; set; }
        [JsonProperty("maxCharCount")] public int MaxCharCount { get; set; }
        [JsonProperty("startNoteIndex")] public int StartNoteIndex { get; set; }
        [JsonProperty("startTime")] public double StartTime { get; set; }
        [JsonProperty("endTime")] public double EndTime { get; set; }
    }

    public class LyricRequestBody
    {
        [JsonProperty("projId")] public string ProjId { get; set; }
        [JsonProperty("rows")] public List<RowInfo> Rows { get; set; }
        [JsonProperty("trackIndex")] public int TrackIndex { get; set; }
        [JsonProperty("singerId")] public string SingerId { get; set; }
    }
}