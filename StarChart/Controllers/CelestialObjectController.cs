using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [ApiController]
    [Route("")]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [HttpGet("{int:id}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestial = _context.CelestialObjects.Find(id);

            if (celestial is null) return NotFound();

            celestial.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == id).ToList();

            return Ok(celestial);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestials = _context.CelestialObjects.Where(c => c.Name == name);

            if (!celestials.Any()) return NotFound();

            foreach (var item in celestials)
            {
                item.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == item.Id).ToList();
            }

            return Ok(celestials.ToList());
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestials = _context.CelestialObjects.ToList();

            if (!celestials.Any()) return NotFound();

            foreach (var item in celestials)
            {
                item.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == item.Id).ToList();
            }

            return Ok(celestials);
        }
    }


}
