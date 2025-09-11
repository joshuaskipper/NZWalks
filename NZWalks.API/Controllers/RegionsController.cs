using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> GetAllRegion()
        {
            // Get Data from Database - Domain models
            var regionsDomain = await dbContext.Regions.ToListAsync();

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
        public async Task<IActionResult> GetById( Guid id)
        {
            var regionDomain = await dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);

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

        //create/post
        // 
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Map or convert DTO to DOmain Model
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl,
            };

            //use domain model to crete region
            await dbContext.Regions.AddAsync(regionDomainModel);
           await dbContext.SaveChangesAsync();

            //Map domain model back to DTO
            var regionDto = new RegionDTO
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);


        }


        //Update
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto) 
        {
            var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);

            if (regionDomainModel is null)
            {
                return NotFound();
            }
            //
            //Map DTO to Domain Model
            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            await dbContext.SaveChangesAsync();


            // convert domain model to dto
            var RegionDto = new RegionDTO 
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };

            return Ok(RegionDto);
        }



        //Delete
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id) 
        {
            var regionDomain = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (regionDomain is null) 
            {
                return NotFound();
            }


            // Delete Region
            dbContext.Regions.Remove(regionDomain);
            await dbContext.SaveChangesAsync();

            //return deleted region back
            var regionDto = new RegionDTO
            {
                Id= regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl,
            };


            return Ok(regionDto);

        }
    } 
}
