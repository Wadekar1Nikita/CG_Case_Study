using System;
using System.ComponentModel.DataAnnotations;

namespace claim{
public class ClaimInsurance
    {
        [Key]
        public int ClaimID { get; set; } 
        [Required(ErrorMessage = "Insurance ID is required.")]
        public int InsuranceID { get; set; }

      
        public virtual Insurance Insurance { get; set; }

       
        [Required(ErrorMessage = "Claim amount is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Claim amount must be a positive number.")]
        public decimal ClaimAmount { get; set; }

        [Required(ErrorMessage = "Claim reason is required.")]
        [StringLength(500, ErrorMessage = "Claim reason cannot be longer than 500 characters.")]
        public required string ClaimReason { get; set;}

        
       [Required(ErrorMessage = "Date of loss is required.")]
        public DateTime DateOfLoss { get; set; }
    }

}

