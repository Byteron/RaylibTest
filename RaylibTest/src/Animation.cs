using HypEcs;

namespace RaylibTest;

public struct Animation
{
    public int[] Frames;
    public float FrameTime;
    public float Time;
}

public class AnimationSystem : ISystem
{
    public void Run(World world)
    {
        var time = world.GetElement<Time>();
        
        var query = world.Query<Sprite, Animation>().Build();
        
        query.Run((count, sprites, animations) =>
        {
            for (var i = 0; i < count; i++)
            {
                ref var animation = ref animations[i];
                ref var sprite = ref sprites[i];
                
                animation.Time += time.Delta;

                var index = (int)(animation.Time / animation.FrameTime) % animation.Frames.Length;
                var texture = animation.Frames[index];

                sprite.Texture = texture;
            }
        });
    }
}