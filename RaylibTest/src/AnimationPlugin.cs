using RelEcs;

namespace RaylibTest;

public class AnimationPlugin : IPlugin
{
    public void Build(App app)
    {
        app.AddSystem(Stage.PostUpdate, new AnimationSystem());
    }
}

public class Animation
{
    public int[] Frames = Array.Empty<int>();
    public float FrameTime;
    public float Time;
}

public class AnimationSystem : ISystem
{
    public void Run(World world)
    {
        var time = world.GetElement<Time>();
        
        var query = world.Query<Sprite, Animation>().Build();

        foreach (var (sprite, animation) in query)
        {
            animation.Time += time.Delta;

            var index = (int)(animation.Time / animation.FrameTime) % animation.Frames.Length;
            var texture = animation.Frames[index];

            sprite.Texture = texture;
        }
    }
}