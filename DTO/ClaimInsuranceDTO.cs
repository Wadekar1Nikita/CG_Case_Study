using System.ComponentModel.DataAnnotations;

namespace Claimdto
{
    public class ClaimInsuranceDTO
    {
        [Required]
        public int InsuranceID { get; set; }

        [Required(ErrorMessage = "Claim amount is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Claim amount must be a positive number.")]
      
        public decimal ClaimAmount { get; set; }

        [Required(ErrorMessage = "Claim reason is required.")]
        [StringLength(500, ErrorMessage = "Claim reason cannot be longer than 500 characters.")]
     
        public string ClaimReason { get; set; }

        [Required]
        public DateTime DateOfLoss { get; set; }
    }
}
