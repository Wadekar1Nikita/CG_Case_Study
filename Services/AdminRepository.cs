using System.Threading.Tasks;
using ContextFile;  
using bidding;
using adminrepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Text;
using sold;
using claiminsurance;
using bidderdto;
using biddingdto;
using service;

namespace LatestUpdate.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    private readonly IEmailService _emailService;

    public AdminRepository(ApplicationDbContext context, IConfiguration configuration,IEmailService emailService)
    {
        _context = context;
        _configuration = configuration;

     _emailService=emailService;
    }

    public async Task<string> LoginAdminAsync(string email, string password)
    {
        var admin = await _context.Admins.SingleOrDefaultAsync(a => a.AEmail == email);
        if (admin == null || admin.APassword != password) 
            throw new Exception("Invalid credentials.");

        return GenerateJwtToken(admin.AdminID.ToString(), "Admin");
    }

    private string GenerateJwtToken(string userId, string role)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        //var expiration = DateTime.Now.AddHours(1);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials,
            claims: new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Role, role)
            }
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

        public async Task<List<BidderDTO>> SearchBidderAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return await _context.Bidders
                                    .Select(b => new BidderDTO { BidderID = b.BidderID, Name = b.Name, Email = b.Email })
                                    .ToListAsync();
            }

            return await _context.Bidders
                                .Where(b => EF.Functions.Like(b.Name, $"{searchTerm}%") || EF.Functions.Like(b.Email, $"{searchTerm}%"))
                                .Select(b => new BidderDTO { BidderID = b.BidderID, Name = b.Name, Email = b.Email, TraderLicence=b.TraderLicence,Adharno=b.Adharno,Address=b.Address,PAN=b.PAN,PhoneNumber=b.PhoneNumber })
                                .ToListAsync();
        }


        public async Task<List<BiddingDTO>> SearchBidsAsync(string searchTerm)
        {
            return await _context.Biddings
                .Where(b => EF.Functions.Like(b.Bidder.Name, $"{searchTerm}%") || 
                            EF.Functions.Like(b.Crop.Farmer.Name, $"{searchTerm}%") )
                .Select(b => new BiddingDTO
                {
                    BidderID=b.BidderID,
                    BidAmount=b.BidAmount,
                    CropID=b.CropID
                })
                .ToListAsync();
        }
        public async Task<string> GetBidderEmailAsync(int bidId)
        {
            var bidder = await _context.Biddings
                .Include(b => b.Bidder)
                .FirstOrDefaultAsync(b => b.BidID == bidId);

            return bidder?.Bidder.Email; 
        }

        public async Task<Bidding> ApproveBidAsync(int bidId)
        {
            var bid = await _context.Biddings.FindAsync(bidId);
            if (bid == null)
            {
                throw new InvalidOperationException("Bid not found.");
            }
            bid.AuctionResult = "Approved";

            try
            {
                _context.Biddings.Update(bid);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error occurred while approving bid: " + ex.Message);
            }

            return bid; 
        }

        public async Task CreateSoldHistoryAsync(int bidId)
    {
        try
        {
            var bid = await _context.Biddings
                .Include(b => b.Crop)     
                .FirstOrDefaultAsync(b => b.BidID == bidId);

            if (bid == null)
            {
                throw new InvalidOperationException("Bid not found.");
            }

            var crop = bid.Crop;
            if (crop == null)
            {
                throw new InvalidOperationException("Crop not found.");
            }
            decimal totalPrice = bid.BidAmount * crop.Quantity;
            var soldHistory = new SoldHistory
            {
                CropID = crop.CropID,
                BidID = bid.BidID,
                MSP = 5000,
                SoldPrice = bid.BidAmount,
                TotalPrice = totalPrice,
                
            };
            await _context.SoldHistories.AddAsync(soldHistory);
            bid.AuctionResult = "Approved"; 
            _context.Biddings.Update(bid);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error occurred while creating SoldHistory: " + ex.Message);
        }
    }


//         public async Task<SoldHistoryDTO> SaveToSoldHistoryAsync(int bidId)
// {
//     // Retrieve the bid and bidder details from the Bidding and Bidder tables
//     var bid = await _context.Biddings.FindAsync(bidId);
//     var bidder = await _context.Bidders.FindAsync(bid.BidderID);
//     var crop = await _context.Crops.FindAsync(bid.CropID);

//     if (bid == null || bidder == null || crop == null)
//     {
//         throw new Exception("Invalid bid, bidder, or crop.");
//     }

//     // Assume MSP (Minimum Support Price) is the crop's MSP in the Crop table, if available.
//     decimal totalPrice = bid.BidAmount * crop.Quantity;  // Total price = Bid amount * Quantity of the crop

//     // Create a SoldHistory entry
//     var soldHistory = new SoldHistory
//     {
//         BidID = bid.BidID,
//         CropID = bid.CropID,
//         SoldPrice = bid.BidAmount,
//         TotalPrice = totalPrice,
        
//     };

//     // Add the SoldHistory to the database
//     _context.SoldHistories.Add(soldHistory);
//     await _context.SaveChangesAsync();

//     // Create a SoldHistoryDTO object to return
//     var soldHistoryDTO = new SoldHistoryDTO
//     {
//         SoldID = soldHistory.SoldID,
//         CropID = soldHistory.CropID,
//         BidID = soldHistory.BidID,
//         CropName = soldHistory.Crop.CropName,
//         MSP = soldHistory.MSP,
//         SoldPrice = soldHistory.SoldPrice,
//         TotalPrice = soldHistory.TotalPrice,
//         Quantity = soldHistory.Crop.Quantity
//     };

//     return soldHistoryDTO;
// }
        // Admin rejects the bid
        public async Task<Bidding> RejectBidAsync(int bidId)
        {
            var bid = await _context.Biddings.FindAsync(bidId);
            if (bid == null)
            {
                throw new InvalidOperationException("Bid not found.");
            }
            bid.AuctionResult = "Rejected";

            try
            {
                _context.Biddings.Update(bid);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error occurred while rejecting bid: " + ex.Message);
            }

            return bid;  
        }

        public async Task<InsuranceClaimValidationDTO> ValidateInsuranceClaimAsync(int claimId)
    {
        try
        {
            var claim = await _context.ClaimInsurances
                .Include(c => c.Insurance) 
                .ThenInclude(i => i.Farmer)
                .FirstOrDefaultAsync(c => c.ClaimID == claimId);

            if (claim == null)
            {
                return new InsuranceClaimValidationDTO
                {
                    IsValid = false,
                    Message = "Claim not found."
                };
            }

            // Validation logic, for example: check if the claim amount is less than or equal to the sum insured
            // if (claim.ClaimAmount <= claim.Insurance.SumInsured)
            // {
                // return new InsuranceClaimValidationDTO
                // {
                //     IsValid = true,
                //     Message = "Claim is valid and can be processed.",
                //     FarmerEmail = claim.Insurance.Farmer.Email
                // };
                 var farmerEmail = claim.Insurance.Farmer?.Email;
            if (string.IsNullOrEmpty(farmerEmail))
            {
                return new InsuranceClaimValidationDTO
                {
                    IsValid = true,
                    Message = "Claim is valid and can be processed, but farmer email not found.",
                    FarmerEmail = null
                };
            }

            var result= new InsuranceClaimValidationDTO
            {
                IsValid = true,
                Message = "Claim is valid and can be processed.",
                FarmerEmail = farmerEmail
            };

            string subject = "Your Insurance Claim Has Been Approved!";
                string message = $"Dear {claim.Insurance.Farmer.Name},<br/><br/>Your insurance claim has been successfully approved.";
                await _emailService.SendEmailAsync(farmerEmail, subject, message);
          return result;

        }
        catch (Exception ex)
        {
            return new InsuranceClaimValidationDTO
            {
                IsValid = false,
                Message = "Error occurred during validation: " + ex.Message
            };
        }
    }

     public async Task<InsuranceClaimValidationDTO> RejectInsuranceClaimAsync(int claimId)
    {
        try
        {
            var claim = await _context.ClaimInsurances
                .Include(c => c.Insurance)
                .ThenInclude(i=>i.Farmer) 
                .FirstOrDefaultAsync(c => c.ClaimID == claimId);

            if (claim == null)
            {
                return new InsuranceClaimValidationDTO
                {
                    IsValid = false,
                    Message = "Claim not found."
                };
            }

            // _context.ClaimInsurances.Update(claim);
            await _context.SaveChangesAsync();
            var farmerEmail = claim.Insurance.Farmer?.Email;
            if (string.IsNullOrEmpty(farmerEmail))
            {
                
                     var subject = "Your Insurance Claim Has Been Rejected";
                    var message = $"Dear {claim.Insurance.Farmer.Name},<br/>" +
                                "Unfortunately, your insurance claim has been rejected. Please contact us for further details.";
                    await _emailService.SendEmailAsync(farmerEmail, subject, message);
                
                
            }

            return new InsuranceClaimValidationDTO
            {
                IsValid = true,
                Message = "Claim has been rejected successfully.",
                 FarmerEmail = farmerEmail
            };

            
        }
        catch (Exception ex)
        {
            return new InsuranceClaimValidationDTO
            {
                IsValid = false,
                Message = "Error occurred while rejecting the claim: " + ex.Message
            };
        }
    
        }
    }
}
 