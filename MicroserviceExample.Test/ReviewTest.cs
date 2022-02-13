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
    public class ReviewTest
    {
        [Fact]
        public async void GetReviewTest()
        {
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>{});
            var client = application.CreateClient();

            var okResult = await client.GetAsync("v1/reviews");

            okResult.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async void PostReview_Article_NotFound_Test()
        {
            var application = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
            var client = application.CreateClient();

            Review rewview = new()
            {
                Id = new Guid(),
                ArticleId = new Guid(),
                ReviewContent = "Test Content",
                Reviewer = "Samet Çoruhlu"
            };
            var content = new StringContent(JsonConvert.SerializeObject(rewview), Encoding.UTF8, "application/json");

            var result = await client.PostAsync("v1/reviews", content);

            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
        }

        [Fact]
        public async void DeleteReview_NotFound_Test()
        {
            var application = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
            var client = application.CreateClient();

            var result = await client.DeleteAsync("v1/reviews(5dd28e3d-91fd-475a-8f35-0f675d925fb7)");

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
    }
}
