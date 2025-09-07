using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{

    // https://localhost:7161/api/regions
    // https://localhost:7161/scalar/v1
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //get all regions
        // https://localhost:7161/api/regions
        [HttpGet]
        public IActionResult GetAllRegion()
        {
            // Get Data from Database - Domain models
            var regionsDomain = dbContext.Regions.ToList();

            // Map Domain Model to DTOs
            var regionsDTO = new List<RegionDTO>();

            foreach (var regionItem in regionsDomain)
            {
                regionsDTO.Add(new RegionDTO()
                {
                    Id = regionItem.Id,
                    Name = regionItem.Name,
                    Code = regionItem.Code,
                    RegionImageUrl = regionItem.RegionImageUrl,

                });
            }

            // return the DTOs back to the client
            return Ok(regionsDTO);
        }

        // Get single Region(get region by id)
        // https://localhost:7161/api/regions/{id}
        [HttpGet("{id:Guid}")]
        //[Route("{id:Guid}")]
        public IActionResult GetById(Guid id)
        {
            var regionDomain = dbContext.Regions.FirstOrDefault(r => r.Id == id);

            if (regionDomain == null) 
            {
                return NotFound();
            }

            //Map/Convert region domain Model to Region DTO
            var regionsDTO = new RegionDTO
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl,
            };

            // return dto back to client
            return Ok(regionsDTO);
        }
    }
}
