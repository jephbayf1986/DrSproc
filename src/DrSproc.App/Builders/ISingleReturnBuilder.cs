using DrSproc.EntityMapping;

namespace DrSproc.Builders
{
    public interface ISingleReturnBuilder<TReturn>
    {
        ISingleReturnBuilder<TReturn> UseCustomMapping<TMapping>() 
            where TMapping : CustomMapper<TReturn>, new();

        TReturn Go();
    }
}