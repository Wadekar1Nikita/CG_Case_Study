using cropdto;

namespace LatestUpdate.Repository
{
    public interface ICropRepository
    {
      
         Task<IEnumerable<CropDTO>> GetAllCropsAsync();
        Task<CropDTO> AddCropAsync(CropDTO cropDto);
        Task<CropDTO> GetCropByIdAsync(int id);

        Task UpdateCropAsync(int cropId, CropDTO cropDto); 
        Task DeleteCropAsync(int cropId);
    }
}
