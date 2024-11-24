using System.ComponentModel.DataAnnotations;
using farmer;

namespace cropdto
{
    public class CropDTO{

        [Required(ErrorMessage = "Crop Name is required.")]
        [StringLength(100, ErrorMessage = "Crop Name cannot be longer than 100 characters.")]

        public required string CropName { get; set; } 

        
        [Required(ErrorMessage = "Crop Type is required.")]
        [StringLength(50, ErrorMessage = "Crop Type cannot be longer than 50 characters.")]

        public required string CropType { get; set; } 

        [Required(ErrorMessage = "Quantity is required.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Fertilizer Type is required.")]
        [StringLength(50, ErrorMessage = "Fertilizer Type cannot be longer than 50 characters.")]
        public required string FertilizerType { get; set; } 


        [Required(ErrorMessage = "Farmer ID is required.")]
       public int FarmerID { get; set; }

         [Required(ErrorMessage = "Base Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Base Price must be a positive value.")]
       
        public decimal BasePrice{get;set;}

    }
}