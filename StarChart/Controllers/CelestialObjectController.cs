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
            if(CelestialObject == null)
            {
                return NotFound();
            }

            CelestialObject.Satellites = _context.CelestialObjects.Where(e => e.Id == CelestialObject.Id).ToList();

            return Ok(CelestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var CelestialObject = _context.CelestialObjects.FirstOrDefault(e => e.Name.Contains(name));
            if(CelestialObject == null)
            {
                return NotFound();
            }

            CelestialObject.Satellites = _context.CelestialObjects.Where(e => e.Id == CelestialObject.Id).ToList();

            Console.WriteLine(CelestialObject);

            return Ok(CelestialObject);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<CelestialObject> CelestialObjects = _context.CelestialObjects.ToList();
            foreach(CelestialObject CelestialObject in CelestialObjects)
            {
                CelestialObject.Satellites = _context.CelestialObjects.Where(e => e.Id == CelestialObject.Id).ToList();
            }
            return Ok(CelestialObjects);
        }
    }
}
