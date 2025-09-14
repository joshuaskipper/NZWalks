using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateWalkRequestDto
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        [MaxLength(1000, ErrorMessage = "Max Character length is 1000")]
        public required string Description { get; set; }

        [Required]
        public required double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }
    }
}
