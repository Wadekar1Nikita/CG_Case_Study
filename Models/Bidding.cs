using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using bidder;
namespace bidding{
    public class Bidding
    {
        [Key]
        public int BidID { get; set; } 

        [Required]
        [Range(0, double.MaxValue)]
        public decimal BidAmount { get; set; } 
        [Required]
        [StringLength(100)]
        public required string AuctionResult { get; set; } 

        [Required]
        public int CropID { get; set; }

        
        public virtual Crop Crop { get; set; }

        [Required]
        public int BidderID { get; set; }

        public virtual Bidder Bidder { get; set; }

        

        
    }
}

