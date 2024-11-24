using System.ComponentModel.DataAnnotations;

namespace sold{
public class SoldHistoryDTO
{
    [Required]
    public int SoldID{get;set;}

    [Required]
    public int CropID{get;set;}

    [Required]
    public int BidID{get;set;}

    [Required(ErrorMessage = "Crop name is required.")]
    public string CropName { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal MSP { get; set; }

    [Required(ErrorMessage ="Sold price is required.")]
    public decimal SoldPrice { get; set; }

    [Required(ErrorMessage ="Total price is required.")]
    public decimal TotalPrice { get; set; }

    [Required(ErrorMessage ="Quantity is required.")]
    public int Quantity { get; set; }
}
}