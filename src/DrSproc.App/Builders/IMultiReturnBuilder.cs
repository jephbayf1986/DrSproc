using DrSproc.EntityMapping;
using System.Collections.Generic;

namespace DrSproc.Builders
{
    /// <summary>
    /// <b>IMultiReturnBuilder</b> <br />
    /// Contains options for completing the execution of a Stored Procedure call returning an IEnumerable of a user specified type
    /// </summary>
    public interface IMultiReturnBuilder<TReturn>
    {
        /// <summary>
        /// <b>Use Custom Mapping</b> <br />
        /// Specifies a custom mapping to use for building up the return type, instead of using reflection.
        /// </summary>
        /// <typeparam name="TMapping">An extension of CustomMapper</typeparam>
        IMultiReturnBuilder<TReturn> UseCustomMapping<TMapping>() where TMapping : CustomMapper<TReturn>, new();

        /// <summary>
        /// <b>Go</b> <bt />
        /// Executes the Stored Procedure call and returns an IEnumerable of the specified type
        /// </summary>
        IEnumerable<TReturn> Go();
    }
}