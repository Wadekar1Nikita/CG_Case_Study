using Microsoft.EntityFrameworkCore;
using LatestUpdate.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContextFile;
using cropdto;

namespace LatestUpdate.Services
{
    public class CropRepository : ICropRepository
    {
        private readonly ApplicationDbContext _context;

        public CropRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CropDTO>> GetAllCropsAsync()
        {
            
            var crops = await _context.Crops
                                        .Select(c => new CropDTO
                                        {
                                            
                                            CropName = c.CropName,
                                            CropType = c.CropType,
                                            BasePrice = c.BasePrice,  
                                            Quantity = c.Quantity,
                                            FertilizerType = c.FertilizerType,
                                            FarmerID = c.FarmerID 
                                        })
                                        .ToListAsync();

            return crops;
        }
        public async Task<CropDTO> AddCropAsync(CropDTO cropDto)
        {
            try
            {
                // Check if the farmer exists (only if required)
                var farmerExists = await _context.Farmers.AnyAsync(f => f.FarmerID == cropDto.FarmerID);
                if (!farmerExists)
                {
                    throw new InvalidOperationException("Farmer not found.");
                }
                var crop = new Crop
                {
                    CropName = cropDto.CropName,
                    CropType = cropDto.CropType,
                    BasePrice = cropDto.BasePrice,
                    Quantity = cropDto.Quantity,
                    FertilizerType = cropDto.FertilizerType,
                    FarmerID = cropDto.FarmerID  
                };

           
                await _context.Crops.AddAsync(crop);
                await _context.SaveChangesAsync();

              
                return new CropDTO
                {
                    
                    CropName = crop.CropName,
                    CropType = crop.CropType,
                    BasePrice = crop.BasePrice,
                    Quantity = crop.Quantity,
                    FertilizerType = crop.FertilizerType,
                    FarmerID = crop.FarmerID
                };
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Database error occurred while adding crop.", dbEx);
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while adding crop.", ex);
            }
        }

        public async Task<CropDTO> GetCropByIdAsync(int id)
        {
            try
            {
                var crop = await _context.Crops
                    .Where(c => c.CropID == id)
                    .Select(c => new CropDTO
                    {
                        
                        CropName = c.CropName,
                        CropType = c.CropType,
                        BasePrice = c.BasePrice,
                        Quantity = c.Quantity,
                        FertilizerType = c.FertilizerType,
                        FarmerID = c.FarmerID
                    })
                    .FirstOrDefaultAsync();

                if (crop == null)
                {
                    throw new InvalidOperationException($"Crop with ID {id} not found.");
                }

                return crop;
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Database error occurred while fetching crop.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while fetching the crop.", ex);
            }
        }
        public async Task UpdateCropAsync(int cropId, CropDTO cropDto)
        {
            try
            {
                var crop = await _context.Crops.FindAsync(cropId);

                if (crop == null)
                {
                    throw new InvalidOperationException($"Crop with ID {cropId} not found.");
                }

                crop.CropName = cropDto.CropName;
                crop.CropType = cropDto.CropType;
                crop.Quantity = cropDto.Quantity;
                crop.FertilizerType = cropDto.FertilizerType;
                crop.BasePrice = cropDto.BasePrice; 
                crop.FarmerID = cropDto.FarmerID;

                _context.Crops.Update(crop);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the crop.", ex);
            }
        }
        public async Task DeleteCropAsync(int cropId)
        {
            try
            {
                var crop = await _context.Crops.FindAsync(cropId);

                if (crop != null)
                {
                    _context.Crops.Remove(crop);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new InvalidOperationException($"Crop with ID {cropId} not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the crop.", ex);
            }
        }
    }
}