//// ADD The Validation for the walk controller update and post!!
/// All code is below delete when done.

using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    // https://localhost:7161/api/walks
    // https://localhost:7161/scalar/v1
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        // Create Walk
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto) 
        {
            // Map AddWalkRequestDto to WalkDomainModel
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

            await walkRepository.CreateAsync(walkDomainModel);
            
            //Map domain model to dto

            return Ok(mapper.Map<WalkDto>(walkDomainModel));


        }


        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var walksDomainModel = await walkRepository.GetAllAsync();

            //Map domain model to dto
            return Ok(mapper.Map<List<WalkDto>>(walksDomainModel));
        }

        // GET by id
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id) 
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);
            if (walkRepository is null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        // Update by ID
        [HttpPut("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateById(Guid id, UpdateWalkRequestDto updateWalkRequestDto) 
        {
            //map dto to domain model
            var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);


            walkDomainModel = await walkRepository.UpdateAsync(id,walkDomainModel);

            if (walkDomainModel is null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(walkDomainModel));



        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(Guid id) 
        {
            var deletedWalkDomainModel = await walkRepository.DeleteAsync(id);

            if (deletedWalkDomainModel is null)
            {
                return NotFound();
            }
            // map domina model to dto
            return Ok(mapper.Map<WalkDto>(deletedWalkDomainModel));
        }

    }
}
