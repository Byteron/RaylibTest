using System.Diagnostics;
using Raylib_cs;
using HypEcs;

namespace RaylibTest;

public class Main
{
    static readonly World World = new();
    
    static readonly SystemGroup InitSystems = new();
    static readonly SystemGroup UpdateSystems = new();
    static readonly SystemGroup RenderSystems = new();
    
    public static void Run()
    {
        Raylib.InitWindow(1920, 1080, "RaylibTest");
        
        World.AddElement(new Time());
        World.AddElement(new Textures());
        
        UpdateSystems.Add(new SpawnGodotSystem())
            .Add(new MoveSystem())
            .Add(new AnimationSystem());

        RenderSystems.Add(new RenderSystem());
        
        InitSystems.Run(World);
        
        var clock = new Stopwatch();
        var lastElapsed = 0.0;
        clock.Start();
        
        while (!Raylib.WindowShouldClose())
        {
            var elapsed = clock.Elapsed.TotalSeconds;
            var delta = elapsed - lastElapsed;
            lastElapsed = elapsed;

            var time = World.GetElement<Time>();
            time.Delta = (float)delta;
            
            UpdateSystems.Run(World);
            RenderSystems.Run(World);
            
            World.Tick();
            
        }

        Raylib.CloseWindow();
    }
}

public class Time
{
    public float Delta;
}

public struct Position
{
    public float X, Y;
}

public struct Velocity
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
        var query = world.Query<Position, Velocity>().Build();
        
        query.Run((count, positions, velocities) =>
        {
            for (var i = 0; i < count; i++)
            {
                ref var pos = ref positions[i];
                ref var vel = ref velocities[i];
                
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
        });
    }
}



