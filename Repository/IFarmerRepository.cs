using System.Collections.Generic;
using System.Threading.Tasks;
using farmer;
using farmerdto;
using registerfarmer;

namespace LatestUpdate.Repository
{
    public interface IFarmerRepository
    {

        
        Task<FarmerDTO> GetFarmerByIdAsync(int farmerId);

        Task<Farmer> FindByEmailAsync(string email);

        Task<string> ResetPasswordAsync(string email, string token, string newPassword);
        //Task<IEnumerable<FarmerDTO>> GetAllFarmersAsync();

        // Task<Farmer> AddFarmerAsync(RegisterFarmerDTO registerFarmerDto);

        Task<string> LoginFarmerAsync(string email, string password);

        Task<List<FarmerDTO>> SearchFarmerAsync(string searchTerm);

        Task<string> GetFarmerEmailAsync(int farmerId);

        Task<bool> PermitFarmerToSellAsync(int farmerId);
        Task<bool> AddFarmerAsync(FarmerDTO farmerDto);
        Task<bool> UpdateFarmerAsync(FarmerDTO farmerDto);
        Task<bool> DeleteFarmerAsync(int farmerId);


    }
}
