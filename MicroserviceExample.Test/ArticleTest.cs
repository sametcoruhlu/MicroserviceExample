using System;
using System.Net;
using System.Net.Http;
using System.Text;
using MicroserviceExample.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace MicroserviceExample.Test
{
    public class ArticleTest
    {
        [Fact]
        public async void GetArticle_Success_Test()
        {
            var application = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
            var client = application.CreateClient();

            var result = await client.GetAsync("v1/articles");

            result.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async void PostArticle_Success_Test()
        {
            var application = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
            var client = application.CreateClient();

            Article article = new()
            {
                Title = "Unit Test",
                Id = new Guid(),
                ArticleContent = "Test Content",
                Author = "Samet Çoruhlu",
                PublishDate = DateTime.Now,
                StarCount = 5
            };
            var content = new StringContent(JsonConvert.SerializeObject(article), Encoding.UTF8, "application/json");

            var result = await client.PostAsync("v1/articles", content);

            result.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
        }

        [Fact]
        public async void DeleteArticle_NotFound_Test()
        {
            var application = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
            var client = application.CreateClient();

            var result = await client.DeleteAsync("v1/articles(5dd28e3d-91fd-475a-8f35-0f675d925fb7)");

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
    }
}
