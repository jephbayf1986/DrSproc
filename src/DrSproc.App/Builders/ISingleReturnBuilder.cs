using DrSproc.EntityMapping;

namespace DrSproc.Builders
{
    /// <summary>
    /// <b>ISingleReturnBuilder</b> <br />
    /// Contains options for completing the execution of a Stored Procedure call returning a single user specified type
    /// </summary>
    public interface ISingleReturnBuilder<TReturn>
    {
        /// <summary>
        /// <b>Use Custom Mapping</b> <br />
        /// Specifies a custom mapping to use for building up the return type, instead of using reflection.
        /// </summary>
        /// <typeparam name="TMapping">An extension of CustomMapper</typeparam>
        ISingleReturnBuilder<TReturn> UseCustomMapping<TMapping>() 
            where TMapping : CustomMapper<TReturn>, new();

        /// <summary>
        /// <b>Go</b> <bt />
        /// Executes the Stored Procedure call and returns the specified type
        /// </summary>
        TReturn Go();
    }
}