using System.IO;
using UnityEngine;

public class StreamingAssetsLoader
{
    public static Texture2D GetTexture2D(string path)
    {
        string combined = Path.Combine(Application.streamingAssetsPath, path);
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Sprite file({path}) not found.");
        }
        byte[] raw = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(raw);
        return texture;
    }
}
