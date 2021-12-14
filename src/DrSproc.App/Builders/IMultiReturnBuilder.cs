using DrSproc.EntityMapping;
using System.Collections.Generic;

namespace DrSproc.Builders
{
    public interface IMultiReturnBuilder<TReturn>
    {
        ISingleReturnBuilder<TReturn> UseCustomMapping<TMapping>() where TMapping : CustomMapper<TReturn>;

        IEnumerable<TReturn> Go();
    }
}