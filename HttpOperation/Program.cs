using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HttpOperation.Model;
using Newtonsoft.Json;

namespace HttpOperation
{
    internal class Program
    {
        private const string BaseUrl = "";
        private const string LocalDirectory = @"C:\Users\xiangxingxing\Desktop\XiaoIce\Lyric\result";
        private static readonly LocalClient Client = new LocalClient(BaseUrl);

        public static async Task Main(string[] args)
        {
            var files = Directory.GetFiles(LocalDirectory).Where(f => f.EndsWith("json"));
            foreach (var file in files)
            {
                await DoRequestSingleFile(file);
                Console.WriteLine($"{Path.GetFileName(file)} done...");
            }
            // while (tasks.Count > 0)
            // {
            //     var completed = await Task.WhenAny(tasks);
            //     tasks.Remove(completed);
            // }

            Console.WriteLine("End.");
        }

        private static async Task DoRequestSingleFile(string filePath)
        {
            var content = File.ReadAllText(filePath);
            var songDataList = JsonConvert.DeserializeObject<List<SongData>>(content);
            if (songDataList == null)
            {
                return;
            }

            //await DoRequestOnceAsync(songDataList[0]);
            foreach (var songData in songDataList)
            {
                await DoRequestOnceAsync(songData);
            }
            
            // var taskList = songDataList.Select(DoRequestOnceAsync).ToList();
            // while (taskList.Count > 0)
            // {
            //     var completed = await Task.WhenAny(taskList);
            //     taskList.Remove(completed);
            // }
        }

        private static async Task DoRequestOnceAsync(SongData songData)
        {
            var requestBody = new LyricRequestBody
            {
                ProjId = songData.SongId,
                TrackIndex = songData.TrackIndex,
                SingerId = songData.SingerId,
                // Rows = new List<RowInfo>()
                Rows = new List<RowInfo>(songData.RowInfos)
            };
            
            //requestBody.Rows.Add(songData.RowInfos[0]);
            try
            {
                using (var ctx = new CancellationTokenSource())
                {
                    await Client.ExecuteRequestAsync(requestBody, ctx.Token);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed with \"{e.Message}\": {songData.SongId} with singer = {songData.SingerId} on track{songData.TrackIndex}");
            }
        }
    }
}