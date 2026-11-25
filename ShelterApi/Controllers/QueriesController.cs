using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterApi.Models;
using ShelterApi.Models.DTOs;

namespace ShelterApi.Controllers
{
    [Route("queries")]
    [ApiController]
    public class QueriesController : ControllerBase
    {
            private readonly AnimalshelterContext _context;

            public QueriesController(AnimalshelterContext context)
            {
                _context = context;
            }

            [HttpGet("allSheltersWithAnimals")]
            public async Task<IActionResult> AllSheltersWithAnimals()
            {
                try
                {
                    var shelters = await _context.Shelters
                        .Include(s => s.Animals)
                        .ToListAsync();

                    if (shelters is not null)
                    {
                        return StatusCode(200, new { message = "Lekérdezve", result = shelters });
                    }

                    return StatusCode(404, new { message = "Nincs találat", result = shelters });
                }
                catch (Exception exception)
                {
                    return StatusCode(400, new { message = exception.Message });
                }
            }
    }
}
