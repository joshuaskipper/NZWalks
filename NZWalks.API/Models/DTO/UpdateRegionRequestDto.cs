using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code has to be a minium of 3 characters")]
        [MaxLength(3, ErrorMessage = "Code can't be more than 3 characters")]
        public required string Code { get; set; }

        [Required]
        public required string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
