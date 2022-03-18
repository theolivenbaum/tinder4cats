using System.Security.Cryptography;
using System.Net;
using System.IO;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var count = int.Parse(args[0]);
var path = Path.Combine(Directory.GetCurrentDirectory(), "images");
Directory.CreateDirectory(path);
var client = new HttpClient();

var cts = new CancellationTokenSource();

long success = Directory.EnumerateFiles(path, "*.jpg").Count();

await Parallel.ForEachAsync(Enumerable.Range(0, count * 100), cts.Token, async (i, ct) =>
{
    var response = await client.GetAsync("https://thiscatdoesnotexist.com/");
    var bytes = await response.Content.ReadAsByteArrayAsync();
    var hash = SHA256.HashData(bytes);
    var b64 = Convert.ToBase64String(hash).Replace("+", "_").Replace("/", "-").Replace("=", "");
    var fn = Path.Combine(path, $"{b64}.jpg");
    if (File.Exists(fn)) return;
    try
    {
        await using var file = File.OpenWrite(fn);
        await file.WriteAsync(bytes);
    }
    catch
    {
        return; //ignore
    }
    Console.WriteLine($"Downloaded {i} to {fn}");
    if (Interlocked.Increment(ref success) > count)
    {
        cts.Cancel();
    }
});
