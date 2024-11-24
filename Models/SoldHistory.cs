using System.ComponentModel.DataAnnotations;
using bidding;

public class SoldHistory
    {
        [Key]
        public int SoldID { get; set; } 
        [Required]
        public int CropID { get; set; }

        public virtual Crop Crop { get; set; }

        
        [Required]
        public int BidID { get; set; }

        public virtual Bidding Bidding { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal MSP { get; set; }

   
        [Required]
        [Range(0, double.MaxValue)]
        public decimal SoldPrice { get; set; }

        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalPrice { get; set; }
    }

