using System.Data;

namespace DrSproc.Main.EntityMapping
{
    internal interface IEntityCreator
    {
        T ReadEntityUsingMapper<T>(IDataReader reader, IEnityMapper<T> mapper);

        T ReadEntityUsingReflection<T>(IDataReader reader);
    }
}