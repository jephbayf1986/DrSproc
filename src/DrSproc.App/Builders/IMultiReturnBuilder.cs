using DrSproc.EntityMapping;
using System.Collections.Generic;

namespace DrSproc.Builders
{
    public interface IMultiReturnBuilder<TReturn>
    {
        IMultiReturnBuilder<TReturn> UseCustomMapping<TMapping>() where TMapping : CustomMapper<TReturn>, new();

        IEnumerable<TReturn> Go();
    }
}