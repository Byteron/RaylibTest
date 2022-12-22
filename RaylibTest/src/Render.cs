using Raylib_cs;
using HypEcs;

namespace RaylibTest;

public struct Sprite
{
    public int Texture;
}

public class Textures
{
    public Dictionary<string, int> Indices = new();
    public List<Texture2D> TextureList = new();

    const string Prefix = "../../../assets/";
    
    public int Load(string path)
    {
        if (Indices.TryGetValue(path, out var index)) return index;

        var texture = Raylib.LoadTexture(Prefix + path);
        index = TextureList.Count;
        
        Indices.Add(path, index);
        TextureList.Add(texture);

        return index;
    }
}

public class RenderSystem : ISystem
{
    public void Run(World world)
    {
        var textures = world.GetElement<Textures>();
        var entityCount = world.Info.EntityCount - world.Info.UnusedEntityCount;
        
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.WHITE);
        
        var query = world.Query<Sprite, Position>().Build();

        query.Run((count, sprites, positions) =>
        {
            for (var i = 0; i < count; i++)
            {
                Raylib.DrawTexture(textures.TextureList[sprites[i].Texture], (int)positions[i].X, (int)positions[i].Y,
                    Color.WHITE);
            }
        });   
        
        Raylib.DrawFPS(10, 10);
        Raylib.DrawText($"Entities: {entityCount}", 10, 40, 32, Color.DARKGREEN);
        
        Raylib.EndDrawing();
    }
}