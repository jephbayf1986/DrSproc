using DrSproc.EntityMapping;

namespace DrSproc.Tests.Shared
{
    internal class TestClassMapper : CustomEntityMapping<TestClassForMapping>
    {
        public override TestClassForMapping Map()
        {
            return new TestClassForMapping();
        }
    }
}