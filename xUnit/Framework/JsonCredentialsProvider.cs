using Core;
using Shared;
using System.IO;
using System.Text.Json;

namespace xUnitTests;



internal class JsonCredentialsProvider : ICredentialsProvider
{
    public JsonCredentialsProvider(string fileName) =>
        Credentials = JsonSerializer.Deserialize<Credentials>(File.ReadAllText(fileName));
    public Credentials Credentials { get; private set; }
}

