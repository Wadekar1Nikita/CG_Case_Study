
using claim;
using Claimdto; 
using ContextFile;
using LatestUpdate.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using service;
using System.Threading.Tasks;

namespace LatestUpdate.Services
{
    public class ClaimInsuranceRepository : IClaimInsuranceRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public ClaimInsuranceRepository(ApplicationDbContext context,IEmailService emailService)
        {
            _context = context;
            _emailService=emailService;
        }

        public async Task<List<ClaimInsuranceDTO>> SearchInsuranceClaimsAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
            
                return await _context.ClaimInsurances
                                    .Select(c => new ClaimInsuranceDTO {  DateOfLoss= c.DateOfLoss, ClaimReason = c.ClaimReason, ClaimAmount = c.ClaimAmount })
                                    .ToListAsync();
            }
            return await _context.ClaimInsurances
                                .Where(c => EF.Functions.Like(c.Insurance.Farmer.Name, $"{searchTerm}%") || EF.Functions.Like(c.ClaimAmount.ToString(), $"{searchTerm}%"))
                                .Select(c => new ClaimInsuranceDTO {  DateOfLoss= c.DateOfLoss, ClaimReason = c.ClaimReason, ClaimAmount = c.ClaimAmount })
                                .ToListAsync();
        }

        
        public async Task<ClaimInsurance> SubmitClaimAsync(ClaimInsuranceDTO claimDTO)
        { 
            var insurance = await _context.Insurances
        .Include(i => i.Farmer)  
        .FirstOrDefaultAsync(i => i.InsuranceID == claimDTO.InsuranceID);

            // var insurance = await _context.ClaimInsurances
            // .Include(c => c.Insurance)         
            // .ThenInclude(i => i.Farmer)
            // .Where(i => i.InsuranceID == claimDTO.InsuranceID)
            // .FirstOrDefaultAsync();
            if (insurance == null)
            {
                throw new Exception("Insurance policy not found.");
            }
            
            // var insurances = await _context.Insurances
            // .FirstOrDefaultAsync(i => i.InsuranceID == claimDTO.InsuranceID);

            // if (claimDTO.ClaimAmount > insurances.SumInsured)
            // {
            //     throw new Exception($"Claim amount exceeds the sum insured. Maximum claim amount allowed is {insurances.SumInsured}.");
            // }
            if (claimDTO.ClaimAmount > insurance.SumInsured)
            {
                throw new Exception($"Claim amount exceeds the sum insured. Maximum claim amount allowed is {insurance.SumInsured}.");
            }
            var claim = new ClaimInsurance
            {
                InsuranceID = claimDTO.InsuranceID,
                ClaimAmount = claimDTO.ClaimAmount,
                ClaimReason = claimDTO.ClaimReason,
                DateOfLoss = claimDTO.DateOfLoss
            };            
            _context.ClaimInsurances.Add(claim);
            await _context.SaveChangesAsync();


            var farmerEmail = insurance.Farmer?.Email;

            if (string.IsNullOrEmpty(farmerEmail))
            {
                throw new Exception("Farmer's email not found.");
            }

            var subject = "Your Insurance Claim Has Been Submitted!";
            var message = $"Dear {insurance.Farmer.Name},<br/>" +
                        $"Your claim for the amount of {claimDTO.ClaimAmount} has been successfully submitted.<br/>" +
                        "Our team will review it and notify you soon.";
            await _emailService.SendEmailAsync(farmerEmail, subject, message);
            return claim;
        }
    }
}
