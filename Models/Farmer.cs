using System.ComponentModel.DataAnnotations;

namespace farmer
{
    public class Farmer
    {
         [Key]
        public int FarmerID { get; set; }

        
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Please enter a valid email address.")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters")]
        public required string Email { get; set; }

        
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", 
        ErrorMessage = "Password must be at least 8 characters long, include one uppercase letter, one lowercase letter, one number, and one special character.")]       
         public required string Password { get; set; }

        
        [Required(ErrorMessage = "Aadhar Number is required.")]
        [Range(100000000000L, 999999999999L, ErrorMessage = "Aadhar Number must be a valid 12-digit number.")]
        public required long Adharno { get; set; }

    
        [Required(ErrorMessage = "PAN is required.")]
        [StringLength(20, ErrorMessage = "PAN cannot be longer than 20 characters.")]
        public required string PAN { get; set; }

       
        [StringLength(50, ErrorMessage = "Address cannot be longer than 50 characters.")]
        public required string Address { get; set; }

        [StringLength(10, ErrorMessage = "Phone number cannot be longer than 15 characters.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public required string PhoneNumber { get; set; }

       [Required(ErrorMessage = "Bank Account Number is required")]
        [Range(100000000, long.MaxValue, ErrorMessage = "Bank account number must be a valid number")]
        public long BankAccountNo { get; set; }

        [Required(ErrorMessage = "Certificate is required.")]
        public required string Certificate { get; set; }

        [StringLength(11, ErrorMessage = "IFSC Code cannot be longer than 11 characters.")]
        public required string IFSCCode { get; set; }

     
        [StringLength(50, ErrorMessage = "Area cannot be longer than 50 characters.")]
        public required string Area { get; set; }

        [StringLength(100, ErrorMessage = "Land Address cannot be longer than 100 characters.")]
       
        public required string LandAddress { get; set; }

        
    }

    }
