using farmer;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContextFile;
using farmerdto;
using registerfarmer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using service;

namespace LatestUpdate.Repository
{
    public class FarmerRepository : IFarmerRepository
    {
        private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    private readonly IEmailService _emailService;

    public FarmerRepository(ApplicationDbContext context, IConfiguration configuration,IEmailService emailService)
    {
        _context = context;
        _configuration = configuration;
        _emailService=emailService;
    }

    // public async Task<Farmer> AddFarmerAsync(RegisterFarmerDTO registerFarmerDto)
    // {
    //     var existingFarmer = await _context.Farmers.SingleOrDefaultAsync(f => f.Email == registerFarmerDto.Email);
    //     if (existingFarmer != null)
    //         throw new Exception("Farmer with this email already exists.");

    //     var farmer = new Farmer
    //     {
    //         Name = registerFarmerDto.Name,
    //         Email = registerFarmerDto.Email,
    //         Password = registerFarmerDto.Password, // Storing password as plain text (not recommended in production)
    //         Adharno = registerFarmerDto.Adharno,
    //         PAN = registerFarmerDto.PAN,
    //         Address = registerFarmerDto.Address,
    //         PhoneNumber = registerFarmerDto.PhoneNumber,
    //         BankAccountNo = registerFarmerDto.BankAccountNo,
    //         Certificate = registerFarmerDto.Certificate,
    //         IFSCCode = registerFarmerDto.IFSCCode,
    //         Area = registerFarmerDto.Area,
    //         LandAddress = registerFarmerDto.LandAddress
    //     };

    //     _context.Farmers.Add(farmer);
    //     await _context.SaveChangesAsync();

    //     return farmer;
    // }

    // Farmer Login
    public async Task<string> LoginFarmerAsync(string email, string password)
    {
        var farmer = await _context.Farmers.SingleOrDefaultAsync(f => f.Email == email);
        if (farmer == null || farmer.Password != password) 
            throw new Exception("Invalid credentials.");
        return GenerateJwtToken(farmer.FarmerID.ToString(), "Farmer");
    }


    private string GenerateJwtToken(string userId, string role)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        // var expiration = DateTime.Now.AddHours(1);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials,
            claims: new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"] ?? "default-subject"),
                new Claim(JwtRegisteredClaimNames.Email, "user@example.com"), 
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
        
        public async Task<string> GetFarmerEmailAsync(int farmerId)
    {
        var farmer = await _context.Farmers
            .FirstOrDefaultAsync(f => f.FarmerID == farmerId);

        return farmer?.Email;
    }

        public async Task<FarmerDTO> GetFarmerByIdAsync(int farmerId)
        {
            var farmer = await _context.Farmers.FindAsync(farmerId);
            if (farmer == null) return null;

            return new FarmerDTO
            {
                FarmerID = farmer.FarmerID,
                Name = farmer.Name,
                Email = farmer.Email,
                Adharno = farmer.Adharno,
                PAN = farmer.PAN,
                Address = farmer.Address,
                PhoneNumber = farmer.PhoneNumber,
                BankAccountNo = farmer.BankAccountNo,
                Certificate = farmer.Certificate,
                IFSCCode = farmer.IFSCCode,
                Area = farmer.Area,
                LandAddress = farmer.LandAddress
            };
        }

        public async Task<IEnumerable<FarmerDTO>> GetAllFarmersAsync()
        {
            return await _context.Farmers
                .Select(f => new FarmerDTO
                {
                    FarmerID = f.FarmerID,
                    Name = f.Name,
                    Email = f.Email,
                    Adharno = f.Adharno,
                    PAN = f.PAN,
                    Address = f.Address,
                    PhoneNumber = f.PhoneNumber,
                    BankAccountNo = f.BankAccountNo,
                    Certificate = f.Certificate,
                    IFSCCode = f.IFSCCode,
                    Area = f.Area,
                    LandAddress = f.LandAddress
                })
                .ToListAsync();
        }


        public async Task<List<FarmerDTO>> SearchFarmerAsync(string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            return await _context.Farmers
                                 .Select(f => new FarmerDTO { FarmerID = f.FarmerID, Name = f.Name, Email = f.Email })
                                 .ToListAsync();
        }

        return await _context.Farmers
                             .Where(f => EF.Functions.Like(f.Name, $"{searchTerm}%") || EF.Functions.Like(f.Email, $"{searchTerm}%"))
                             .Select(f => new FarmerDTO { FarmerID = f.FarmerID, Name = f.Name, Email = f.Email,Adharno=f.Adharno,Certificate=f.Certificate,PAN=f.PAN,PhoneNumber=f.PhoneNumber,Address=f.Address})
                             .ToListAsync();
    }

        public async Task<bool> PermitFarmerToSellAsync(int farmerId)
        {
            var farmer = await _context.Farmers.FindAsync(farmerId);
            if (farmer != null)
            {
                return true;  
            }
            return true;
        }

        public async Task<bool> AddFarmerAsync(FarmerDTO farmerDto)
        {
            var farmer = new Farmer
            {
                Name = farmerDto.Name,
                Email = farmerDto.Email,
                Adharno = farmerDto.Adharno,
                PAN = farmerDto.PAN,
                Address = farmerDto.Address,
                PhoneNumber = farmerDto.PhoneNumber,
                BankAccountNo = farmerDto.BankAccountNo,
                Certificate = farmerDto.Certificate,
                IFSCCode = farmerDto.IFSCCode,
                Area = farmerDto.Area,
                LandAddress = farmerDto.LandAddress,
                Password=farmerDto.Password
            };
            _context.Farmers.Add(farmer);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateFarmerAsync(FarmerDTO farmerDto)
        {
            var farmer = await _context.Farmers.FindAsync(farmerDto.FarmerID);
            if (farmer != null)
            {
                farmer.Name = farmerDto.Name;
                farmer.Email = farmerDto.Email;
                farmer.Adharno = farmerDto.Adharno;
                farmer.PAN = farmerDto.PAN;
                farmer.Address = farmerDto.Address;
                farmer.PhoneNumber = farmerDto.PhoneNumber;
                farmer.BankAccountNo = farmerDto.BankAccountNo;
                farmer.Certificate = farmerDto.Certificate;
                farmer.IFSCCode = farmerDto.IFSCCode;
                farmer.Area = farmerDto.Area;
                farmer.LandAddress = farmerDto.LandAddress;

                _context.Farmers.Update(farmer);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<bool> DeleteFarmerAsync(int farmerId)
        {
            var farmer = await _context.Farmers.FindAsync(farmerId);
            if (farmer != null)
            {
                _context.Farmers.Remove(farmer);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<Farmer> FindByEmailAsync(string email)
            {
                return await _context.Farmers.FirstOrDefaultAsync(u => u.Email == email);
            }


            public async Task<string> ResetPasswordAsync(string email, string token, string newPassword)
    {
        var user = await _context.Farmers.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            return "Invalid reset token or user.";
        }
        user.Password = newPassword;  
        await _context.SaveChangesAsync();
        var subject = "Your Password has been Reset Successfully";
        var message = "Dear User,\n\nYour password has been reset successfully. You can now log in with your new password.";
        await _emailService.SendEmailAsync(email, subject, message);

        return "Password reset successful.";
    }
    }
}
