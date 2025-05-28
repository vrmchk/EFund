namespace EFund.Seeding.Behaviors.Abstractions;

public class DependsOnAttribute : Attribute
{
    public DependsOnAttribute(Type[] behaviors)
    {
        if (behaviors.Any(b => !b.IsAssignableTo(typeof(ISeedingBehavior))))
            throw new ArgumentException("Some types doesn't implement ISeedingBehavior");

        Behaviors = behaviors;
    }

    public Type[] Behaviors { get; }
}