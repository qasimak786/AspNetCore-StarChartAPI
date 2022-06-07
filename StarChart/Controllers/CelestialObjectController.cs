using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route(""), ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var CelestialObject = _context.CelestialObjects.FirstOrDefault(e => e.Id == id);
            if (CelestialObject == null)
            {
                return NotFound();
            }

            CelestialObject.Satellites = _context.CelestialObjects.Where(e => e.Id == CelestialObject.Id).ToList();

            return Ok(CelestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var CelestialObjects = _context.CelestialObjects.Where(e => e.Name == name).ToList();
            if (!CelestialObjects.Any())
            {
                return NotFound();
            }

            foreach (CelestialObject CelestialObject in CelestialObjects)
            {
                CelestialObject.Satellites = _context.CelestialObjects.Where(e => e.Id == CelestialObject.Id).ToList();
            }

            return Ok(CelestialObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<CelestialObject> CelestialObjects = _context.CelestialObjects.ToList();
            foreach (CelestialObject CelestialObject in CelestialObjects)
            {
                CelestialObject.Satellites = _context.CelestialObjects.Where(e => e.Id == CelestialObject.Id).ToList();
            }
            return Ok(CelestialObjects);
        }

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var CelestialObject = _context.CelestialObjects.Find(id);
            if(CelestialObject == null)
            {
                return NotFound();
            }

            CelestialObject.Name = celestialObject.Name;
            CelestialObject.OrbitalPeriod = celestialObject.OrbitalPeriod;
            CelestialObject.OrbitedObjectId = celestialObject.OrbitedObjectId;

            _context.CelestialObjects.Update(CelestialObject);
            _context.SaveChanges();

            return NoContent();

        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var CelestialObject = _context.CelestialObjects.Find(id);
            if(CelestialObject == null)
            {
                return NotFound();
            }

            CelestialObject.Name = name;
            _context.CelestialObjects.Update(CelestialObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            List<CelestialObject> CelestialObjects = _context.CelestialObjects.Where(e => e.Id == id || e.OrbitedObjectId == id).ToList();
            if(!CelestialObjects.Any())
            {
                return NotFound();
            }

            _context.CelestialObjects.RemoveRange(CelestialObjects);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
