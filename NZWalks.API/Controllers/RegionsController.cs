using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{

    // https://localhost:7161/api/regions
    // https://localhost:7161/scalar/v1
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        //get all regions
        // https://localhost:7161/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAllRegion()
        {
            // Get Data from Database - Domain models
            var regionsDomain = await regionRepository.GetAllAsync();

            // Map Domain Model to DTOs
            var regionsDTO = mapper.Map<List<Region>>(regionsDomain);

            // return the DTOs back to the client
            return Ok(regionsDTO);
        }


        [HttpGet("{id:Guid}")]
        //[Route("{id:Guid}")]
        public async Task<IActionResult> GetById( Guid id)
        {
            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            //Map/Convert region domain Model to Region DTO

            // return dto back to client
            return Ok(mapper.Map<RegionDTO>(regionDomain));
        }

        //create/post
        // 
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Map or convert DTO to DOmain Model
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

            //use domain model to crete region
            await regionRepository.CreateAsync(regionDomainModel);

            //Map domain model back to DTO
            var regionDto = mapper.Map<RegionDTO>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }


        //Update
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto) 
        {
            //map dto to domain model
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);


            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel is null)
            {
                return NotFound();
            }
            // convert domain model to dto
            return Ok(mapper.Map<RegionDTO>(regionDomainModel));
        }


        //Delete
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id) 
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if (regionDomainModel is null) 
            {
                return NotFound();
            }
            return Ok(mapper.Map<RegionDTO>(regionDomainModel));

        }
    } 
}
