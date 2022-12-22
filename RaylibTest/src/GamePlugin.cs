using Raylib_cs;
using RelEcs;

namespace RaylibTest;

public class GamePlugin : IPlugin
{
    public void Build(App app)
    {
        app
            .AddSystem(Stage.Update, new SpawnGodotSystem())
            .AddSystem(Stage.Update, new MoveSystem());
    }
}

public class Position
{
    public float X, Y;
}

public class Velocity
{
    public float X, Y;
}

public class SpawnGodotSystem : ISystem
{
    readonly Random _random = new();
    
    public void Run(World world)
    {
        var textures = world.GetElement<Textures>();
        
        if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE))
        {
            for (var i = 0; i < 100; i++)
            {
                world.Spawn()
                    .Add(new Sprite { Texture = textures.Load("icon.png") })
                    .Add(new Animation
                    {
                        FrameTime = 0.15f,
                        Frames = new[]
                        {
                            textures.Load("icon.png"),
                            textures.Load("icon1.png"),
                            textures.Load("icon2.png"),
                            textures.Load("icon3.png"),
                            textures.Load("icon2.png"),
                            textures.Load("icon1.png"),
                        }
                    })
                    .Add(new Position { X = 50, Y = 50 })
                    .Add(new Velocity { X = _random.Next(-100, 100), Y = _random.Next(-100, 100) });
            }
        }
    }
}

public class MoveSystem : ISystem
{
    public void Run(World world)
    {
        var height = Raylib.GetRenderHeight();
        var width = Raylib.GetRenderWidth();

        var delta = world.GetElement<Time>().Delta;
        
        foreach (var (pos, vel) in world.Query<Position, Velocity>().Build())
        {
            pos.X += vel.X * delta;
            pos.Y += vel.Y * delta;

            if (pos.X < 0 || pos.X > width)
            {
                vel.X = -vel.X;
            }
            
            if (pos.Y < 0 || pos.Y > height)
            {
                vel.Y = -vel.Y;
            }
            
            pos.X = Math.Clamp(pos.X, 0, width);
            pos.Y = Math.Clamp(pos.Y, 0, height);
        }
    }
}