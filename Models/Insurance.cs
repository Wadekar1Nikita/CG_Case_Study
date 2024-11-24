using System.ComponentModel.DataAnnotations;
using farmer;


public class Insurance
    {
        [Key]
        public int InsuranceID { get; set; } 
        [Required]
        public int FarmerID { get; set; }

       
        public virtual Farmer Farmer { get; set; }  

        
        [Required(ErrorMessage = "Premium Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Premium Amount must be a positive value.")]
        public decimal PremiumAmount { get; set; }

        [Required(ErrorMessage = "Policy Name is required.")]
        [StringLength(20, ErrorMessage = "Policy Name cannot exceed 20 characters.")]
        public required string PolicyName { get; set; }

      
       [Required(ErrorMessage = "Sum Insured is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Sum Insured must be a positive value.")]
        public decimal SumInsured { get; set; }

        
        [Required(ErrorMessage = "Area is required.")]
        [StringLength(50, ErrorMessage = "Area cannot exceed 50 characters.")]
        public required string Area { get; set; }

        [Required(ErrorMessage = "Season is required.")]
        [StringLength(20, ErrorMessage = "Season cannot exceed 20 characters.")]
        public required string Season { get; set; }

     
        [Required(ErrorMessage = "Year is required.")]
        [Range(1900, int.MaxValue, ErrorMessage = "Please enter a valid year.")]
        public int Year { get; set; }

       
        [Required(ErrorMessage = "Premium Rate For Season is required.")]
        [Range(0, 1, ErrorMessage = "Premium Rate For Season must be between 0 and 1.")]
        public decimal PremiumRateForSeason { get; set; }
    }

