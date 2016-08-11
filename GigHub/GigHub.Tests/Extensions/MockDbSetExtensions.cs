using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace GigHub.Tests.Extensions
{
    public static class MockDbSetExtensions
    {
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/dn314429.aspx#queryTest
        /// </summary>
        public static void SetSource<T>(this Mock<DbSet<T>> mockSet, IList<T> source) where T : class
        {
            var data = source.AsQueryable();

            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        }

        /// <summary>
        /// https://msdn.microsoft.com/en-us/data/dn314429#async
        /// </summary>
        public static void SetSourceAsync<T>(this Mock<DbSet<T>> mockSet, IList<T> source) where T : class
        {
            var data = source.AsQueryable();

            mockSet.As<IDbAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<T>(data.GetEnumerator()));

            mockSet.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<T>(data.Provider));

            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        }
    }
}
