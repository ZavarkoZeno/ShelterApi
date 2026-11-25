using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using ShelterApi.Models;
using ShelterApi.Models.DTOs;

namespace ShelterApi.Controllers
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
            public async Task<IActionResult> NewAnimal([FromBody] NewAnimalDTO newAnimalDto)
            {
                try
                {
                    var shelter = await _context.Shelters.FindAsync(newAnimalDto.ShelterId);

                    if (shelter is null)
                    {
                        return StatusCode(404, new { message = "Nincs ilyen" });
                    }

                    var animal = new Animal
                    {
                        Id = Guid.NewGuid(),
                        Name = newAnimalDto.Name,
                        Species = newAnimalDto.Species,
                        Age = newAnimalDto.Age,
                        Weight = newAnimalDto.Weight,
                        ShelterId = newAnimalDto.ShelterId,
                        RegTime = DateTime.Now
                    };

                    if (animal is not null)
                    {
                        await _context.Animals.AddAsync(animal);
                        await _context.SaveChangesAsync();

                        return StatusCode(201, new { message = "Felvéve", result = animal });
                    }

                    return StatusCode(400, new { message = "Hiba", result = animal });
                }
                catch (Exception exception)
                {
                    return StatusCode(400, new { message = exception.Message });
                }
            }

            [HttpGet("allAnimal")]
            public async Task<IActionResult> AllAnimal()
            {
                try
                {
                    var animals = await _context.Animals.ToListAsync();

                    if (animals is not null && animals.Count > 0)
                    {
                        return StatusCode(200, new { message = "Lekérdezve", result = animals });
                    }

                    return StatusCode(404, new { message = "Nincs találat", result = animals });
                }
                catch (Exception exception)
                {
                    return StatusCode(400, new { message = exception.Message });
                }
            }

            [HttpGet("byId")]
            public async Task<IActionResult> AnimalById([FromQuery] Guid id)
            {
                try
                {
                    var animal = await _context.Animals.SingleOrDefaultAsync(a => a.Id == id);

                    if (animal is not null)
                    {
                        return StatusCode(200, new { message = "Lekérdezve", result = animal });
                    }

                    return StatusCode(404, new { message = "Nincs találat", result = animal });
                }
                catch (Exception exception)
                {
                    return StatusCode(400, new { message = exception.Message });
                }
            }

            [HttpPut("updateAnimal")]
            public async Task<IActionResult> UpdateAnimal([FromQuery] Guid id, [FromBody] UpdateAnimalDTO updateAnimalDto)
            {
                try
                {
                    var animal = await _context.Animals.FirstOrDefaultAsync(a => a.Id == id);

                    if (animal is not null)
                    {
                        var shelter = await _context.Shelters.FindAsync(updateAnimalDto.ShelterId);

                        if (shelter is null)
                        {
                            return StatusCode(404, new { message = "Nincs ilyen" });
                        }

                        animal.Name = updateAnimalDto.Name;
                        animal.Species = updateAnimalDto.Species;
                        animal.Age = updateAnimalDto.Age;
                        animal.Weight = updateAnimalDto.Weight;
                        animal.ShelterId = updateAnimalDto.ShelterId;

                        _context.Animals.Update(animal);
                        await _context.SaveChangesAsync();

                        return StatusCode(200, new { message = "Frissítve", result = animal });
                    }

                    return StatusCode(404, new { message = "Nincs találat", result = animal });
                }
                catch (Exception exception)
                {
                    return StatusCode(400, new { message = exception.Message });
                }
            }

            [HttpDelete("deleteAnimal")]
            public async Task<IActionResult> DeleteAnimal([FromQuery] Guid id)
            {
                try
                {
                    var animal = await _context.Animals.FindAsync(id);

                    if (animal is not null)
                    {
                        _context.Animals.Remove(animal);
                        await _context.SaveChangesAsync();

                        return StatusCode(200, new { message = "Törölve", result = animal.Name });
                    }

                    return StatusCode(404, new { message = "Nincs találat", result = animal });
                }
                catch (Exception exception)
                {
                    return StatusCode(400, new { message = exception.Message });
                }
            }
    }
}
