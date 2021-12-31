using DrSproc.Exceptions;
using DrSproc.Main.Builders;
using DrSproc.Main.DbExecutor;
using DrSproc.Main.EntityMapping;
using DrSproc.Main.Shared;
using DrSproc.Tests.Shared;
using Moq;
using Shouldly;
using System;
using System.Data.SqlClient;
using Xunit;

namespace DrSproc.Tests.SprocBuilderTests
{
    public class WithParamTests
    {
        [Fact]
        public void GivenNullParamName_OnWithParams_ThrowInformativeError() 
        {
            // Arrange
            var connection = new SqlConnection(RandomHelpers.RandomConnectionString());
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var sprocName = RandomHelpers.RandomString();

            var sproc = new StoredProc(sprocName);

            SprocBuilder<ContosoDb> sut = new(dbExecutor.Object, entityMapper.Object, connection, sproc);

            // Act
            Func<object?> action = () => sut.WithParam(null, RandomHelpers.RandomString());

            // Assert
            Should.Throw<DrSprocParameterException>(action)
                .Message.ShouldSatisfyAllConditions(x => x.ToLower().ShouldContain("parameter"),
                                                    x => x.ToLower().ShouldContain("null"),
                                                    x => x.ToLower().ShouldContain(sprocName.ToLower()));
        }

        [Fact]
        public void GivenBlankParamName_OnWithParams_ThrowInformativeError()
        {
            // Arrange
            var connection = new SqlConnection(RandomHelpers.RandomConnectionString());
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var sprocName = RandomHelpers.RandomString();

            var sproc = new StoredProc(sprocName);

            SprocBuilder<ContosoDb> sut = new(dbExecutor.Object, entityMapper.Object, connection, sproc);

            // Act
            Func<object?> action = () => sut.WithParam(string.Empty, RandomHelpers.RandomString());

            // Assert
            Should.Throw<DrSprocParameterException>(action)
                .Message.ShouldSatisfyAllConditions(x => x.ToLower().ShouldContain("parameter"),
                                                    x => x.ToLower().ShouldContain("blank"),
                                                    x => x.ToLower().ShouldContain(sprocName.ToLower()));
        }

        [Theory]
        [InlineData("Invalid Name")]
        [InlineData("Invalid    Tab")]
        [InlineData(@"Invalid
                        Carriag Return")]
        [InlineData("   InvalidStart")]
        public void GivenWhiteSpaceInMiddleOfParamName_OnWithParams_ThrowInformativeError(string paramName)
        {
            // Arrange
            var connection = new SqlConnection(RandomHelpers.RandomConnectionString());
            Mock<IDbExecutor> dbExecutor = new();
            Mock<IEntityMapper> entityMapper = new();

            var sprocName = RandomHelpers.RandomString();

            var sproc = new StoredProc(sprocName);

            SprocBuilder<ContosoDb> sut = new(dbExecutor.Object, entityMapper.Object, connection, sproc);

            // Act
            Func<object?> action = () => sut.WithParam(paramName, RandomHelpers.IntBetween(1, 10));

            // Assert
            Should.Throw<DrSprocParameterException>(action)
                .Message.ShouldSatisfyAllConditions(x => x.ToLower().ShouldContain("parameter"),
                                                    x => x.ToLower().ShouldContain("white space"),
                                                    x => x.ToLower().ShouldContain(sprocName.ToLower()));
        }
    }
}