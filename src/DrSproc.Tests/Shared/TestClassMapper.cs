using DrSproc.EntityMapping;

namespace DrSproc.Tests.Shared
{
    internal class TestClassMapper : EntityMapper<TestClassForMapping>
    {
        public override TestClassForMapping Map()
        {
            return new TestClassForMapping();
        }
    }
}