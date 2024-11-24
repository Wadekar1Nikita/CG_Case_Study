
using bidder;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContextFile;
using bidderdto;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using registerbidderdto;
using service;

namespace LatestUpdate.Repository
{
    public class BidderRepository : IBidderRepository
    {
        

        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        private readonly IEmailService _emailService;

        public BidderRepository(ApplicationDbContext context, IConfiguration configuration,IEmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService=emailService;
        }

    // public async Task<Bidder> AddBidderAsync(RegisterBidderDTO registerBidderDto)
    // {
    //     var existingBidder = await _context.Bidders.SingleOrDefaultAsync(b => b.Email == registerBidderDto.Email);
    //     if (existingBidder != null)
    //         throw new Exception("Bidder with this email already exists.");

    //     var bidder = new Bidder
    //     {
    //         Name = registerBidderDto.Name,
    //         Email = registerBidderDto.Email,
    //         Password = registerBidderDto.Password,  // Plain text password
    //         Adharno = registerBidderDto.Adharno,
    //         PAN = registerBidderDto.PAN,
    //         Address = registerBidderDto.Address,
    //         PhoneNumber = registerBidderDto.PhoneNumber,
    //         BankAccountNo = registerBidderDto.BankAccountNo,
    //         IFSCCode = registerBidderDto.IFSCCode,
    //         TraderLicence = registerBidderDto.TraderLicence
    //     };

    //     _context.Bidders.Add(bidder);
    //     await _context.SaveChangesAsync();

    //     return bidder;
    // }

    public async Task<string> LoginBidderAsync(string email, string password)
    {
        var bidder = await _context.Bidders.SingleOrDefaultAsync(b => b.Email == email);
        if (bidder == null || bidder.Password != password) 
            throw new Exception("Invalid credentials.");

        return GenerateJwtToken(bidder.BidderID.ToString(), "Bidder");
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
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"] ?? "default-subject"),
                new Claim(JwtRegisteredClaimNames.Email, "user@example.com"), 
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


        public async Task<string> GetBidderEmailAsync(int bidderId)
    {
        var bidder = await _context.Bidders
            .FirstOrDefaultAsync(b => b.BidderID == bidderId);

        return bidder?.Email; 
    }

        public async Task<IEnumerable<BidderDTO>> GetAllBiddersAsync()
        {
            return await _context.Bidders
                .Select(b => new BidderDTO
                {
                    BidderID = b.BidderID,
                    Name = b.Name,
                    Email = b.Email,
                    PAN = b.PAN,
                    Address = b.Address,
                    PhoneNumber = b.PhoneNumber,
                    TraderLicence = b.TraderLicence,
                    
                })
                .ToListAsync();
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
                                    .Select(b => new BidderDTO { BidderID = b.BidderID, Name = b.Name, Email = b.Email , TraderLicence=b.TraderLicence,Adharno=b.Adharno,Address=b.Address,PAN=b.PAN,PhoneNumber=b.PhoneNumber})
                                    .ToListAsync();
            }

        public async Task<BidderDTO> ApproveBidderAsync(int bidderId)
        {
            var bidder = await _context.Bidders.FindAsync(bidderId);
            if (bidder != null)
            {
                var bidderDto = new BidderDTO
                {
                    BidderID = bidder.BidderID,
                    Name = bidder.Name,
                    Email = bidder.Email,
                    IsApproved = true 
                };
                _context.Bidders.Update(bidder);
                await _context.SaveChangesAsync();

                return new BidderDTO
                {
                    BidderID = bidder.BidderID,
                    Name = bidder.Name,
                    Email = bidder.Email,
                    PAN = bidder.PAN,
                    Address = bidder.Address,
                    PhoneNumber = bidder.PhoneNumber,
                    TraderLicence = bidder.TraderLicence,
                };
            }
            return null;
        }

        
        public async Task<bool> AddBidderAsync(BidderDTO bidderDto)
        {
            var bidder = new Bidder
            {
                Name = bidderDto.Name,
                Email = bidderDto.Email,
                PAN = bidderDto.PAN,
                Address = bidderDto.Address,
                PhoneNumber = bidderDto.PhoneNumber,
                TraderLicence = bidderDto.TraderLicence,
                Password=bidderDto.Password,
                IFSCCode=bidderDto.IFSCCode
            };
            _context.Bidders.Add(bidder);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateBidderAsync(BidderDTO bidderDto)
        {
            var bidder = await _context.Bidders.FindAsync(bidderDto.BidderID);
            if (bidder != null)
            {
                bidder.Name = bidderDto.Name;
                bidder.Email = bidderDto.Email;
                bidder.PAN = bidderDto.PAN;
                bidder.Address = bidderDto.Address;
                bidder.PhoneNumber = bidderDto.PhoneNumber;
                bidder.TraderLicence = bidderDto.TraderLicence;

                _context.Bidders.Update(bidder);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<bool> DeleteBidderAsync(int bidderId)
        {
            var bidder = await _context.Bidders.FindAsync(bidderId);
            if (bidder != null)
            {
                _context.Bidders.Remove(bidder);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<Bidder> FindByEmailAsync(string email)
            {
                return await _context.Bidders.FirstOrDefaultAsync(u => u.Email == email);
            }


            public async Task<string> ResetPasswordAsync(string email, string token, string newPassword)
            {
                var user = await _context.Bidders.FirstOrDefaultAsync(u => u.Email == email);

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


            


    //     public async Task<string> GeneratePasswordResetTokenAsync(string email)
    // {
    //     var user = await _context.Bidders.FirstOrDefaultAsync(u => u.Email == email);
    //     if (user == null) return null;

    //     var token = Guid.NewGuid().ToString();

    //     var passwordResetToken = new PasswordResetToken
    //     {
    //         Token = token,
    //         Expiration = DateTime.UtcNow.AddHours(1), // Token expires in 1 hour
    //         UserId = user.BidderID
    //     };

    //     _context.PasswordResetTokens.Add(passwordResetToken);
    //     await _context.SaveChangesAsync();

    //     return token;
    // }

    // public async Task<bool> ValidatePasswordResetTokenAsync(string token)
    // {
    //     var resetToken = await _context.PasswordResetTokens
    //         .FirstOrDefaultAsync(t => t.Token == token);

    //     return resetToken != null && resetToken.Expiration > DateTime.UtcNow;
    // }
    }
}
