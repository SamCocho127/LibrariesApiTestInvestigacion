using System.ComponentModel.DataAnnotations;

namespace LibraryService.WebAPI.DTO;

public class FraudForm
{
    [Required(ErrorMessage = "ImpostorDetails es obligatorio.")]
    public string ImpostorDetails { get; set; } = string.Empty;

    [Required(ErrorMessage = "ContactInfo es obligatorio.")]
    public string ContactInfo { get; set; } = string.Empty;

    public string Comments { get; set; } = string.Empty;
}
