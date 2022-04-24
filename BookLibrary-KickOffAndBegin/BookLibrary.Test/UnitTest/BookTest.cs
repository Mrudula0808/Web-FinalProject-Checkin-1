using System;
using System.Collections.Generic;
using BookLibrary.Data.DbContexts;
using BookLibrary.Data.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BookLibrary.Test.UnitTest
{
    public class BookTest
    {
        [Fact]
        public void Get_All_Books()
        {
            var serviceProvider = BuildServiceProvider();
            var bookService = serviceProvider.GetRequiredService<BookService>();

            var result = bookService.GetBooks();

            Assert.True(result.Success);
            Assert.Empty(result.Data);
        }

        private static IServiceProvider BuildServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddScoped<BookService>();
            services.AddDbContext<AppDbContext>(b => b.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            return services.BuildServiceProvider();
        }
    }
}
