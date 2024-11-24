using System.ComponentModel.DataAnnotations;

namespace biddingdto
{
    public class BiddingDTO
    {
        [Required]
        public int BidderID { get; set; }

        [Required]
        public int CropID { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal BidAmount { get; set; }
    }
}
