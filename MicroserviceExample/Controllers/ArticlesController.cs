using MicroserviceExample.Entities;
using MicroserviceExample.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace MicroserviceExample.Controllers
{
    //[Route("v1/[controller]")]
    public class ArticlesController : ODataController
    {
        private readonly MicroserviceExampleContext database;

        private readonly ILogger<ArticlesController> logger;

        public ArticlesController(MicroserviceExampleContext database, ILogger<ArticlesController> logger)
        {
            this.logger = logger;
            this.database = database;
        }

        [EnableQuery(PageSize = 15)]
        public IQueryable<Article> Get()
        {
            return database.Articles.Include(m => m.Reviews);
        }

        [EnableQuery]
        public SingleResult<Article> Get([FromODataUri] Guid key)
        {
            var result = database.Articles.Where(c => c.Id == key);
            return SingleResult.Create(result);
        }

        [EnableQuery]
        public async Task<IActionResult> Post([FromBody] Article article)
        {
            database.Articles.Add(article);
            await database.SaveChangesAsync();
            return Created(article);
        }

        [EnableQuery]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<Article> article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingarticle = await database.Articles.FindAsync(key);
            if (existingarticle == null)
            {
                return NotFound();
            }

            article.Patch(existingarticle);
            try
            {
                await database.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(existingarticle);
        }

        [EnableQuery]
        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            Article existingarticle = await database.Articles.FindAsync(key);
            if (existingarticle == null)
            {
                return NotFound();
            }

            database.Articles.Remove(existingarticle);
            await database.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }

        private bool ArticleExists(Guid key)
        {
            return database.Articles.Any(p => p.Id == key);
        }
    }
}
