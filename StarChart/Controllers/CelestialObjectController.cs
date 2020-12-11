using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

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

        [HttpGet("{id:int}", Name = "GetById")]
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

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestial)
        {
            if (celestial is null) return NotFound();

            _context.Add(celestial);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = celestial.Id }, celestial);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestial)
        {
            var existingCelestial = _context.CelestialObjects.Find(id);

            if (existingCelestial is null) return NotFound();

            existingCelestial.Name = celestial.Name;
            existingCelestial.OrbitalPeriod = celestial.OrbitalPeriod;
            existingCelestial.OrbitedObjectId = celestial.OrbitedObjectId;

            _context.Update(existingCelestial);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var existingCelestial = _context.CelestialObjects.Find(id);

            if (existingCelestial is null) return NotFound();

            existingCelestial.Name = name;
            _context.Update(existingCelestial);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingCelestial = _context.CelestialObjects.Where(c => c.Id == id || c.OrbitedObjectId == id).ToList();

            if (!existingCelestial.Any()) return NotFound();

            _context.RemoveRange(existingCelestial);
            _context.SaveChanges();

            return NoContent();
        }
    }


}
