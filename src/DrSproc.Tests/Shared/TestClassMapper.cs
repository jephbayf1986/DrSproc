using DrSproc.EntityMapping;

namespace DrSproc.Tests.Shared
{
    internal class TestClassMapper : CustomMapper<TestClassForMapping>
    {
        public override TestClassForMapping Map()
        {
            return new TestClassForMapping();
        }
    }
}