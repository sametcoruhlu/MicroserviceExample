using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using MicroserviceExample.Entities;
using MicroserviceExample.EntityFramework;

namespace MicroserviceExample.Controllers
{
    //[Route("v1/[controller]")]
    public class ReviewsController : ODataController
    {
        private readonly MicroserviceExampleContext database;

        private readonly ILogger<ReviewsController> logger;

        public ReviewsController(MicroserviceExampleContext database, ILogger<ReviewsController> logger)
        {
            this.logger = logger;
            this.database = database;
        }

        [EnableQuery(PageSize = 15)]
        public IQueryable<Review> Get()
        {
            return database.Reviews.Include(m => m.Article);
        }

        [EnableQuery]
        public SingleResult<Review> Get([FromODataUri] Guid key)
        {
            var result = database.Reviews.Where(c => c.Id == key);
            return SingleResult.Create(result);
        }

        [EnableQuery]
        public async Task<IActionResult> Post([FromBody] Review review)
        {
            database.Reviews.Add(review);
            await database.SaveChangesAsync();
            return Created(review);
        }

        [EnableQuery]
        public async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<Review> review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingreview = await database.Reviews.FindAsync(key);
            if (existingreview == null)
            {
                return NotFound();
            }

            review.Patch(existingreview);
            try
            {
                await database.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(existingreview);
        }

        [EnableQuery]
        public async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            Review existingreview = await database.Reviews.FindAsync(key);
            if (existingreview == null)
            {
                return NotFound();
            }

            database.Reviews.Remove(existingreview);
            await database.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }

        private bool ReviewExists(Guid key)
        {
            return database.Reviews.Any(p => p.Id == key);
        }
    }
}
