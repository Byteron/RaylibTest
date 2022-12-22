using System.Diagnostics;
using RelEcs;

namespace RaylibTest;

public class Time
{
    public float Delta;
}

public class Clock
{
    public readonly Stopwatch Watch = new();
    public double Elapsed;
}

public class DefaultPlugin : IPlugin
{
    public void Build(App app)
    {
        app
            .AddElement(new Clock())
            .AddElement(new Time())
            .AddSystem(Stage.Startup, new StartClockSystem())
            .AddSystem(Stage.First, new UpdateTimeSystem());
    }
}

public class StartClockSystem : ISystem
{
    public void Run(World world)
    {
        var clock = world.GetElement<Clock>();
        clock.Watch.Start();
    }
}

public class UpdateTimeSystem : ISystem
{
    public void Run(World world)
    {
        var clock = world.GetElement<Clock>();
        var time = world.GetElement<Time>();
        
        var elapsed = clock.Watch.Elapsed.TotalSeconds;
        var delta = elapsed - clock.Elapsed;
        clock.Elapsed = elapsed;

        time.Delta = (float)delta;
    }
}