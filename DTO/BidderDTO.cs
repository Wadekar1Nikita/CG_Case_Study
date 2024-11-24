using System.ComponentModel.DataAnnotations;

namespace bidderdto
{
    public class BidderDTO
    {
        public int BidderID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Please enter a valid email address.")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters")]

        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", 
        ErrorMessage = "Password must be at least 8 characters long, include one uppercase letter, one lowercase letter, one number, and one special character.")]

        public string Password { get; set; }
        
        [Required(ErrorMessage = "Adhar number is required")]
        [Range(100000000000, 999999999999, ErrorMessage = "Adhar number must be a 12-digit number")]

        public long Adharno { get; set; }

         [Required(ErrorMessage = "PAN is required")]
        [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}$", ErrorMessage = "Invalid PAN format")]

        public string PAN { get; set; }
        
         [Required(ErrorMessage = "Address is required")]
        [StringLength(255, ErrorMessage = "Address cannot be longer than 255 characters")]

        public string Address { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits")]

        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Bank Account Number is required")]
        [Range(100000000, long.MaxValue, ErrorMessage = "Bank account number must be a valid number")]

        public long BankAccountNo { get; set; }

        [Required(ErrorMessage = "IFSC code is required")]
        [RegularExpression(@"^[A-Za-z]{4}[0-9]{7}$", ErrorMessage = "Invalid IFSC code format")]

        public string IFSCCode { get; set; }

        [Required(ErrorMessage = "Trader Licence is required")]
        [StringLength(100, ErrorMessage = "Trader Licence cannot be longer than 100 characters")]

        public string TraderLicence { get; set; }
        public bool IsApproved { get; set; } = true;
}
}