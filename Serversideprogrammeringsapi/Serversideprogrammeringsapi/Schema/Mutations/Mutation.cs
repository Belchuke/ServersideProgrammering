namespace Serversideprogrammeringsapi.Schema.Mutations
{
    public class Mutation
    {
        public TestObject RequiredMutation(string name)
        {
            return new TestObject() { Name = name, Child = new TestObjectChild() { ChildName = " hello" } };
        }
    }

    public class TestObject
    {
        public string Name { get; set; }

        public TestObjectChild Child { get; set; }
    }

    public class TestObjectChild
    {
        public string ChildName { get; set; }
    }
}
