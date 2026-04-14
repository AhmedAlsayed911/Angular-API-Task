using Madrasty.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Madrasty_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicController(Madrasty.Entites.ApplicationDbContext dbContext) : ControllerBase
    {
        [HttpGet]
        [Route("GetAllTopics")]
        public async Task<IActionResult> GetAllTopics()
        {
            var topics = await dbContext.Topics
                .Select(topic => new
                {
                    topic.Id,
                    topic.Name
                })
                .ToListAsync();

            return Ok(topics);
        }
    }
}