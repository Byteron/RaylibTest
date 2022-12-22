using Raylib_cs;
using RelEcs;

namespace RaylibTest;

public class RenderPlugin : IPlugin
{
    public void Build(App app)
    {
        app
            .AddElement(new Textures())
            .AddSystem(Stage.Render, new RenderSystem());
    }
}

public class Sprite
{
    public int Texture;
}

public class Textures
{
    public Dictionary<string, int> Indices = new();
    public List<Texture2D> TextureList = new();

    public int Load(string path)
    {
        if (Indices.TryGetValue(path, out var index)) return index;

        var texture = Raylib.LoadTexture(path);
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
     
        foreach (var (sprite, pos) in query)
        {
            Raylib.DrawTexture(textures.TextureList[sprite.Texture], (int)pos.X, (int)pos.Y, Color.WHITE);
        }
        
        Raylib.DrawFPS(10, 10);
        Raylib.DrawText($"Entities: {entityCount}", 10, 40, 32, Color.DARKGREEN);
        
        Raylib.EndDrawing();
    }
}