using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirtualPetCareAPI.DBOperations;
using VirtualPetCareAPI.Entities;

namespace VirtualPetCareAPI.Controllers
{
    [Route("api/activities")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        // Dependency Injection ile context kullanma
        private readonly VirtualPetCareDbContext _db;
        public ActivityController(VirtualPetCareDbContext virtualPetCareDbContext)
        {
            _db = virtualPetCareDbContext;
        }


        [HttpPost] // /api/activities
        public async Task<IActionResult> Create(Activity activity)
        {
            _db.Activities.Add(activity);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { activity.PetId }, activity); // olusturulan kaynagin bilgilerini donder 201
        }

        [HttpGet]
        [Route("{PetId}")] // /api/activities/PetId
        public async Task<IActionResult> GetById(int PetId)
        {
            var activity = await _db.Activities.Where(x => x.PetId == PetId).Select(x => new { x.PetId, x.Name }).ToListAsync();

            if (activity is null) return NotFound(); // 404
            return Ok(activity); // 200
        }
    }
}
