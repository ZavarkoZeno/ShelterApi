using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using ShelterApi.Models;
using ShelterApi.Models.DTOs;

namespace ShelterApi.Controllers
{
    public class AnimalsController
    {
        [Route("animals")]
        [ApiController]

        public class AnimalsController : ControllerBase
        {
            private readonly AnimalshelterContext _context;

            public AnimalsController(AnimalshelterContext context)
            {
                _context = context;
            }

            [HttpPost("newAnimal")]
            public async Task<IActionResult> NewAnimal([FromBody] NewAnimalDTO newAnimalDTO)
            {
                try
                {
                    var animal = new Animal
                    {
                        Id = Guid.NewGuid(),
                        Name = newAnimalDTO.Name,
                    };

                    if (animal is not null)
                    {
                        await _context.Animals.AddAsync(animal);
                        await _context.SaveChangesAsync();

                        return StatusCode(201, new { message = "Felvéve!", Result = animal });
                    }
                }
                catch (Exception exception)
                {
                    return BadRequest(exception.Message);
                }
            }

            [HttpGet("allAnimal")]
            public async Task<IActionResult> allAnimal()
            {
                try
                {

                }
            }

        }
    }
}
