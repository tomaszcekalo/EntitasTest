// See https://aka.ms/new-console-template for more information
using Entitas;

Console.WriteLine("Hello, World!");
var Context = new Context<Entity>(3, () => new Entity());
IExecuteSystem speed = new SpeedSystem(Context);
IExecuteSystem accel = new AccelerationSystem(Context);

Entity entity = Context.CreateEntity();
entity.AddComponent(0, new ComponentWrapper<PositionComponent>());
entity.AddComponent(1, new ComponentWrapper<VelocityComponent>()
{
    Component = new VelocityComponent
    {
        x = 0,
        y = 10,
    }
});
entity.AddComponent(2, new ComponentWrapper<AccelerationComponent>()
{
    Component = new AccelerationComponent
    {
        x = 0,
        y = -1
    }
});

for (int i = 0; i < 20; i++)
{
    speed.Execute();
    accel.Execute();
    Console.WriteLine("Y =" + entity.GetComponent(0).ToString());
}

public class SpeedSystem : JobSystem<Entity>
{
    public SpeedSystem(Context<Entity> context)
        : base(context.GetGroup(Matcher<Entity>.AllOf(0, 1)))
    {
    }

    protected override void Execute(Entity entity)
    {
        var a = (ComponentWrapper<PositionComponent>)entity.GetComponent(0);
        var b = (ComponentWrapper<VelocityComponent>)entity.GetComponent(1);
        a.Component.x += b.Component.x;
        a.Component.y += b.Component.y;
    }
}

public class ComponentWrapper<A> : IComponent
{
    public A Component;

    public override string ToString()
    {
        return Component.ToString();
    }
}

public class AccelerationSystem : JobSystem<Entity>
{
    public AccelerationSystem(Context<Entity> context)
        : base(context.GetGroup(Matcher<Entity>.AllOf(1, 2)))
    {
    }

    protected override void Execute(Entity entity)
    {
        var a = (ComponentWrapper<VelocityComponent>)entity.GetComponent(1);
        var b = (ComponentWrapper<AccelerationComponent>)entity.GetComponent(2);
        a.Component.x += b.Component.x;
        a.Component.y += b.Component.y;
    }
}

public struct PositionComponent
{
    public float x;
    public float y;

    public override string ToString()
    {
        return y.ToString();
    }
}

public struct VelocityComponent
{
    public float x;
    public float y;
}

public struct AccelerationComponent
{
    public float x;
    public float y;
}