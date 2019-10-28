using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Location.Models;
using System.Net;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using Location.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Location.Filters;

namespace Location.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[ServiceFilter(typeof(RequestFilter))]
    public class LocationsController : Controller
    {
        private LocationDbContext _context;

        public LocationsController(LocationDbContext context)
        {
            _context = context;
        }

        // GET: api/Locations
        [HttpGet]
        public IEnumerable<Locations> GetLocations()
        {
            return _context.Locations;
        }

        // GET: api/Locations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocations([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var locations = await _context.Locations.FindAsync(id);

            if (locations == null)
            {
                return NotFound();
            }

            return Ok(locations);
        }
        //dohvati sirinu i visinu za seris ispis
        [HttpGet("{latitude}")]
        public async Task<IActionResult> GetLocations([FromRoute] string latitude)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Locations newLocations = new Locations
            {
                Latitude = latitude,

            };


            var locations = await _context.Locations.FindAsync(newLocations);

            if (locations == null)
            {
                return NotFound();
            }

            return Ok(locations);
        }

        // PUT: api/Locations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocations([FromRoute] int id, [FromBody] Locations locations)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != locations.LocationsID)
            {
                return BadRequest();
            }

            _context.Entry(locations).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //spremi podatke u bazu
        // POST: api/Locations
        [HttpPost]
        public async Task<IActionResult> PostLocations(Locations locations)
        {
            Locations newLocations = new Locations();
            newLocations.Latitude = locations.Latitude;
            newLocations.Longitude = locations.Longitude;
            string latitude, longitude;
            latitude = locations.Latitude;
            longitude = locations.Longitude;

            string url = "https://api.foursquare.com/v2/venues/search" +
                "?client_id=OPVXQPHD0AMGMQDGVDAKIWMB21K414HNAXE0KHSCPSHAA2N1" +
                "&client_secret=DZJ3U51QJ0QJ0YRAK3YSZUKK1AO1TPXA4AKD1LJILDZZ4WQI" +
                "&v=20180323&limit=10";

            string noviUrl = url + "&ll=" + latitude + "," + longitude;

            HttpClient httpClient = new HttpClient();
            string json = await httpClient.GetStringAsync(noviUrl);

            var jsonDes = JsonConvert.DeserializeObject(json);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _context.Locations.AddAsync(newLocations);
            _context.SaveChanges();


            return Ok(jsonDes);
        }
        //spremi podatke u bazu
        //POST: api/Locations/{querry}
        [HttpPost("{query}")]
        public async Task<IActionResult> PostLocationsQuerry([FromBody] Locations locations, [FromRoute] string query)
        {
            Locations newLocations = new Locations();
            newLocations.Latitude = locations.Latitude;
            newLocations.Longitude = locations.Longitude;
            string latitude, longitude;
            latitude = locations.Latitude;
            longitude = locations.Longitude;

            string url = "https://api.foursquare.com/v2/venues/search" +
                "?client_id=OPVXQPHD0AMGMQDGVDAKIWMB21K414HNAXE0KHSCPSHAA2N1" +
                "&client_secret=DZJ3U51QJ0QJ0YRAK3YSZUKK1AO1TPXA4AKD1LJILDZZ4WQI" +
                "&v=20180323&limit=10";

            string noviUrl = url + "&ll=" + latitude + "," + longitude + "&query=" + query;

            HttpClient httpClient = new HttpClient();
            string json = await httpClient.GetStringAsync(noviUrl);

            var jsonDes = JsonConvert.DeserializeObject(json);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _context.Locations.AddAsync(newLocations);
            _context.SaveChanges();


            return Ok(jsonDes);
        }

        // DELETE: api/Locations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocations([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var locations = await _context.Locations.FindAsync(id);
            if (locations == null)
            {
                return NotFound();
            }

            _context.Locations.Remove(locations);
            await _context.SaveChangesAsync();

            return Ok(locations);
        }

        private bool LocationsExists(int id)
        {
            return _context.Locations.Any(e => e.LocationsID == id);
        }
    }
}