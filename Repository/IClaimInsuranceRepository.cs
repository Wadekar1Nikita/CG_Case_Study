using claim;
using Claimdto;
using System;
using System.Threading.Tasks;

namespace LatestUpdate.Repository
{
    public interface IClaimInsuranceRepository
    {
        Task<List<ClaimInsuranceDTO>> SearchInsuranceClaimsAsync(string searchTerm);
        Task<ClaimInsurance> SubmitClaimAsync(ClaimInsuranceDTO claimDTO);
    }
}