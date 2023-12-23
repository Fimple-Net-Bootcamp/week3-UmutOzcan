using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirtualPetCareAPI.DBOperations;
using VirtualPetCareAPI.Entities;

namespace VirtualPetCareAPI.Controllers
{
    [Route("api/nutrients")]
    [ApiController]
    public class NutrientController : ControllerBase
    {
        // Dependency Injection ile context kullanma
        private readonly VirtualPetCareDbContext _db;
        public NutrientController(VirtualPetCareDbContext virtualPetCareDbContext)
        {
            _db = virtualPetCareDbContext;
        }


        [HttpPost] // /api/nutrients
        public async Task<IActionResult> Create(Nutrient nutrient)
        {
            _db.Nutrients.Add(nutrient);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { nutrient.PetId }, nutrient); // olusturulan kaynagin bilgilerini donder 201
        }

        [HttpGet]
        [Route("{PetId}")] // /api/nutrients/PetId
        public async Task<IActionResult> GetById(int PetId)
        {
            var nutrient = await _db.Nutrients.Where(x => x.PetId == PetId).Select(x => new { x.PetId, x.Name }).ToListAsync();

            if (nutrient is null) return NotFound(); // 404
            return Ok(nutrient); // 200
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var nutrients = await _db.Nutrients.Select(x => new { x.PetId, x.Name }).ToListAsync(); // First'de garanti deger olmali, FirstOrDefault ile yoksa null doner

            if (!nutrients.Any()) return NotFound(); // 404
            return Ok(nutrients); // 200
        }
    }
}
