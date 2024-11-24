using System.Runtime.CompilerServices;

namespace claiminsurance
{
    public class InsuranceClaimValidationDTO
{
    public bool IsValid { get; set; }
    public string Message { get; set; }

    public String FarmerEmail{get;set;}
}

}