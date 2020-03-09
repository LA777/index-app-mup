using AutoFixture;

namespace Tests
{
    public abstract class TestBase
    {
        protected static IFixture Fixture { get; }

        static TestBase()
        {
            Fixture = new Fixture();
            Fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
    }
}
