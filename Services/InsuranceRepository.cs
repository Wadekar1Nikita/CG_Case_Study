using ContextFile;
using insurancedto;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;


namespace LatestUpdate.Repositories
{
    public class InsuranceRepository : IInsuranceRepository
    {
        private readonly ApplicationDbContext _context;

        public InsuranceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<InsuranceDTO> AddInsuranceAsync(InsuranceDTO insuranceDto)
        {
            try
            {

                var farmer = await _context.Farmers.FindAsync(insuranceDto.FarmerID);
                if (farmer == null)
                {
                    throw new InvalidOperationException("Farmer not found.");
                }

                 decimal premiumRate = 0.0m;
                if (string.Equals(insuranceDto.CropType, "Kharif", StringComparison.OrdinalIgnoreCase))
                {
                    premiumRate = 0.02m;
                }
                else if (string.Equals(insuranceDto.CropType, "Rabi", StringComparison.OrdinalIgnoreCase))
                {
                    premiumRate = 0.015m;
                }
                else if (string.Equals(insuranceDto.CropType, "Commercial", StringComparison.OrdinalIgnoreCase) || 
                        string.Equals(insuranceDto.CropType, "Horticultural", StringComparison.OrdinalIgnoreCase))
                {
                    premiumRate = 0.05m;
    }
                else
                {
                    throw new InvalidOperationException("Invalid Crop Type.");
                }

           
                var premiumAmount = insuranceDto.SumInsured * premiumRate;

                
                var insurance = new Insurance
                {
                    FarmerID = insuranceDto.FarmerID,
                    PolicyName = insuranceDto.PolicyName,
                    SumInsured = insuranceDto.SumInsured,
                    Area = insuranceDto.Area,
                    Season = insuranceDto.Season,
                    Year = insuranceDto.Year,
                    PremiumRateForSeason = insuranceDto.PremiumRateForSeason,
                    PremiumAmount = premiumAmount
                };

                await _context.Insurances.AddAsync(insurance);
                await _context.SaveChangesAsync();
                return new InsuranceDTO
                {
                    FarmerID = insurance.FarmerID,
                    PremiumAmount = insurance.PremiumAmount,
                    PolicyName = insurance.PolicyName,
                    SumInsured = insurance.SumInsured,
                    Area = insurance.Area,
                    Season = insurance.Season,
                    Year = insurance.Year,
                    PremiumRateForSeason = insurance.PremiumRateForSeason,
                    CropType = insuranceDto.CropType
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while adding insurance: " + ex.Message);
            }
        }

        
    }
}
