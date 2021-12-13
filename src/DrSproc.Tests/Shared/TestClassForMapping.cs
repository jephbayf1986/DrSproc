using System;

namespace DrSproc.Tests.Shared
{
    internal class TestClassForMapping
    {
        public int Id { get; set; } = RandomHelpers.IntBetween(100, 200);

        public string FirstName { get; set; } = RandomHelpers.RandomString();

        public string LastName { get; set; } = RandomHelpers.RandomString();

        public string Description { get; set; } = RandomHelpers.RandomString();

        public TestSubClass SubClass { get; set; } = new TestSubClass();

        public decimal Height { get; set; } = RandomHelpers.IntBetween(150, 220);

        public decimal Width { get; set; } = RandomHelpers.IntBetween(150, 220);

        public DateTime DateOfBirth { get; set; } = RandomHelpers.DateInPast(RandomHelpers.IntBetween(4000, 30000));

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }

    public class TestSubClass
    {
        public int Id { get; set; } = RandomHelpers.IntBetween(10, 30);

        public string Name { get; set; } = RandomHelpers.RandomString();

        public string Description { get; set; } = RandomHelpers.RandomString();
    }
}