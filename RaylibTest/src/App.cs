using Raylib_cs;
using RelEcs;

namespace RaylibTest;

public enum Stage
{
    Startup,
    First,
    PreUpdate,
    Update,
    PostUpdate,
    Render,
    Last,
}

public interface IPlugin
{
    void Build(App app);
}

public class App
{
    readonly World _world = new();
    readonly List<IPlugin> _plugins = new();
    readonly Dictionary<Stage, SystemGroup> _systems = new()
    {
        { Stage.Startup , new SystemGroup() },
        { Stage.First , new SystemGroup() },
        { Stage.PreUpdate , new SystemGroup() },
        { Stage.Update , new SystemGroup() },
        { Stage.PostUpdate , new SystemGroup() },
        { Stage.Render , new SystemGroup() },
        { Stage.Last , new SystemGroup() },
    };

    public App AddPlugin(IPlugin plugin)
    {
        _plugins.Add(plugin);
        return this;
    }

    public App AddSystem(Stage stage, ISystem system)
    {
        _systems[stage].Add(system);
        return this;
    }
    
    public App AddElement<T>(T element) where T : class
    {
        _world.AddElement(element);
        return this;
    }
    
    public void Run()
    {
        Raylib.InitWindow(800, 480, "RaylibTest");

        foreach (var plugin in _plugins)
        {
            plugin.Build(this);
        }
        
        var startupSystems = _systems[Stage.Startup];
        
        var firstSystems = _systems[Stage.First];
        var preUpdateSystems = _systems[Stage.PreUpdate];
        var updateSystems = _systems[Stage.Update];
        var postUpdateSystems = _systems[Stage.PostUpdate];
        var renderSystems = _systems[Stage.Render];
        var lastSystems = _systems[Stage.Last];

        startupSystems.Run(_world);

        while (!Raylib.WindowShouldClose())
        {
            
            firstSystems.Run(_world);
            preUpdateSystems.Run(_world);
            updateSystems.Run(_world);
            postUpdateSystems.Run(_world);
            renderSystems.Run(_world);
            lastSystems.Run(_world);
            
            _world.Tick();
        }

        Raylib.CloseWindow();
    }
}


