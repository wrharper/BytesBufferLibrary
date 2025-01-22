using System.Text.Json;

namespace BytesBufferLibrary
{
    public struct Vector3(float x, float y, float z)
    {
        public float X { get; set; } = x;
        public float Y { get; set; } = y;
        public float Z { get; set; } = z;

        public override readonly string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }
    }

    public static class BytesBuffer
    {
        private static readonly Dictionary<int, MemoryStream> streams = [];

        public static void InitializeStream(int clientId)
        {
            if (!streams.ContainsKey(clientId))
            {
                streams[clientId] = new MemoryStream();
            }
        }

        public static bool IsInitialized(int clientId)
        {
            return streams.ContainsKey(clientId);
        }

        public static void WriteToStream<T>(int clientId, T data)
        {
            if (streams.TryGetValue(clientId, out MemoryStream? value))
            {
                lock (value) // Ensure thread safety
                {
                    string jsonData = JsonSerializer.Serialize(new Wrapper<T> { Data = data });
                    using var writer = new StreamWriter(value, System.Text.Encoding.UTF8, 1024, true);
                    writer.Write(jsonData);
                    writer.Flush(); // Make sure all data is written to the stream
                }
            }
        }

        public static byte[] GetBuffer(int clientId)
        {
            if (streams.TryGetValue(clientId, out MemoryStream? value))
            {
                lock (value) // Ensure thread safety
                {
                    byte[] buffer = value.ToArray();
                    value.SetLength(0); // Clear the stream after reading
                    return buffer;
                }
            }
            return [0];
        }

        public static void ClearBuffer(int clientId)
        {
            if (streams.TryGetValue(clientId, out MemoryStream? value))
            {
                lock (value) // Ensure thread safety
                {
                    value.SetLength(0); // Clear the stream
                }
            }
        }

        public static T? Deserialize<T>(MemoryStream ms)
        {
            ms.Position = 0;
            using var reader = new StreamReader(ms, System.Text.Encoding.UTF8, true);
            string jsonData = reader.ReadToEnd();

            var wrapper = JsonSerializer.Deserialize<Wrapper<T>>(jsonData);

            if (wrapper == null || wrapper.Data == null)
            {
                return default; // Return an empty value of the type
            }

            return wrapper.Data;
        }

        private class Wrapper<T>
        {
            public T? Data { get; set; }
        }
    }
}