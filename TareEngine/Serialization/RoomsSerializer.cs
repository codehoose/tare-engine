﻿using Newtonsoft.Json;

namespace TareEngine.Serialization
{
    internal static class RoomsSerializer
    {
        private static readonly string Content = nameof(Content);

        public static SerializedRoomCollection? ReadRooms(string jsonFile)
        {
            var path = Path.Combine(Content, jsonFile);
            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<SerializedRoomCollection>(json);
        }
    }
}
