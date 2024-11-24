using System.ComponentModel.DataAnnotations;


    public class Admin
    {
        [Key]
        public int AdminID { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Please enter a valid email address.")]
        [StringLength(100)]
        public required string AEmail { get; set; } 

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", 
        ErrorMessage = "Password must be at least 8 characters long, include one uppercase letter, one lowercase letter, one number, and one special character.")]
        public required string APassword { get; set; } 

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100)]
        public required string AName { get; set; } 
    }

