using BytesBufferLibrary;

public class Program
{
    public static void Main()
    {
        int clientId1 = 1;
        int clientId2 = 2;

        // Initialize streams for two clients
        BytesBuffer.InitializeStream(clientId1);
        BytesBuffer.InitializeStream(clientId2);

        // Check if streams are initialized
        Console.WriteLine("Client 1 initialized: " + BytesBuffer.IsInitialized(clientId1)); // Output: True
        Console.WriteLine("Client 2 initialized: " + BytesBuffer.IsInitialized(clientId2)); // Output: True

        // Test with string data
        string data1 = "Hello, Client 1!";
        string data2 = "Hello, Client 2!";

        BytesBuffer.WriteToStream(clientId1, data1);
        BytesBuffer.WriteToStream(clientId2, data2);

        // Get buffers and print the data
        byte[]? buffer1 = BytesBuffer.GetBuffer(clientId1);
        buffer1 ??= [];
        byte[]? buffer2 = BytesBuffer.GetBuffer(clientId2);
        buffer2 ??= [];

        string client1Data = System.Text.Encoding.UTF8.GetString(buffer1);
        string client2Data = System.Text.Encoding.UTF8.GetString(buffer2);

        Console.WriteLine("Client 1 Data: " + client1Data); // Output: {"Data":"Hello, Client 1!"}
        Console.WriteLine("Client 2 Data: " + client2Data); // Output: {"Data":"Hello, Client 2!"}

        // Deserialize data
        using var ms1 = new MemoryStream(buffer1);
        using var ms2 = new MemoryStream(buffer2);

        string? deserializedClient1Data = BytesBuffer.Deserialize<string>(ms1);
        string? deserializedClient2Data = BytesBuffer.Deserialize<string>(ms2);

        Console.WriteLine("Deserialized Client 1 Data: " + deserializedClient1Data); // Output: Hello, Client 1!
        Console.WriteLine("Deserialized Client 2 Data: " + deserializedClient2Data); // Output: Hello, Client 2!

        // Clear buffers
        BytesBuffer.ClearBuffer(clientId1);
        BytesBuffer.ClearBuffer(clientId2);

        // Check if buffers are cleared
        buffer1 = BytesBuffer.GetBuffer(clientId1);
        buffer2 = BytesBuffer.GetBuffer(clientId2);

        Console.WriteLine("Client 1 Buffer Cleared: " + (buffer1.Length == 0)); // Output: True
        Console.WriteLine("Client 2 Buffer Cleared: " + (buffer2.Length == 0)); // Output: True

        // Test with Vector3 data
        Vector3 vector1 = new(1.0f, 2.0f, 3.0f);
        Vector3 vector2 = new(4.0f, 5.0f, 6.0f);

        BytesBuffer.WriteToStream(clientId1, vector1);
        BytesBuffer.WriteToStream(clientId2, vector2);

        // Get buffers and print the data
        buffer1 = BytesBuffer.GetBuffer(clientId1);
        buffer2 = BytesBuffer.GetBuffer(clientId2);

        client1Data = System.Text.Encoding.UTF8.GetString(buffer1);
        client2Data = System.Text.Encoding.UTF8.GetString(buffer2);

        Console.WriteLine("Client 1 Vector Data: " + client1Data); // Output: {"Data":{"X":1,"Y":2,"Z":3}}
        Console.WriteLine("Client 2 Vector Data: " + client2Data); // Output: {"Data":{"X":4,"Y":5,"Z":6}}

        // Deserialize data
        using var ms3 = new MemoryStream(buffer1);
        using var ms4 = new MemoryStream(buffer2);

        Vector3 deserializedClient1Vector = BytesBuffer.Deserialize<Vector3>(ms3);
        Vector3 deserializedClient2Vector = BytesBuffer.Deserialize<Vector3>(ms4);

        Console.WriteLine("Deserialized Client 1 Vector: " + deserializedClient1Vector); // Output: (1, 2, 3)
        Console.WriteLine("Deserialized Client 2 Vector: " + deserializedClient2Vector); // Output: (4, 5, 6)

        // Clear buffers
        BytesBuffer.ClearBuffer(clientId1);
        BytesBuffer.ClearBuffer(clientId2);

        // Check if buffers are cleared
        buffer1 = BytesBuffer.GetBuffer(clientId1);
        buffer2 = BytesBuffer.GetBuffer(clientId2);

        Console.WriteLine("Client 1 Buffer Cleared: " + (buffer1.Length == 0)); // Output: True
        Console.WriteLine("Client 2 Buffer Cleared: " + (buffer2.Length == 0)); // Output: True
    }
}
