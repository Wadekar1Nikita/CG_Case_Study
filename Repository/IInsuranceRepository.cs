using System.Collections.Generic;
using System.Threading.Tasks;
using insurancedto;


namespace LatestUpdate.Repositories
{
    public interface IInsuranceRepository
    {
        Task<InsuranceDTO> AddInsuranceAsync(InsuranceDTO insuranceDto);

       

        
    }
}
