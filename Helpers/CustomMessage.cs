namespace PizzaOrder.Helpers
{
    public class CustomMessage
    {
        public const string Added = "Record(s) added successfully";
        public const string Updated = "Record(s) updated successfully";
        public const string Deleted = "Record(s) deleted successfully";
        public const string UserUnAuthorized = "Username or Password does not match. Please try again";
        public const string UserAlreadyExist = "User Name already exist";
        public const string EmailAlreadyExist = "Email already exist";
        public const string NotEligible = "You must be at least 13 years old to use this App";
        public const string PhoneAlreadyExist = "Contact Number already exist"; 
        public const string Credentialsok = "Required fields are filled";
        public const string SqlDuplicateRecord = "Same record already exist in database";
        public const string RecordNotFound = "Record not found";
        public const string CheckInFailed = "Not Allowed To CheckIn OutSide of the Paremeter";
        public const string Invalid = "Invalid Credentials";
        public const string RecordRelationExist = "This {0} record has dependent record. Please remove all dependent details first";
        public const string PasswordNotMatched = "Given password does not match";
        public const string NewPasswordNotGiven = "New password not provided";
        public const string Cloned = "{0} cloned successfully";
        public const string SelectSellerBefore = "Please select seller before order confirmation";
        public const string CouldNotApprovedContract = "Contract basic information (currency, commission etc.) are not available for this contract";
        public const string RecordAlreadyExist = "Record Already Exist";
        public const string RecordUsedInEnquiry = "This record is being used in Enquiry";
        public const string RecordUsedInContract = "This record is being used in Contract";
        public const string ContractNotApproved = "Sale Invoice cannot be added as contract is not approved.";
        public const string ContractCannotProceedToBill = "Contract cannot be proceed for billing {0}";
        public const string UserNotLoggedIn = "Your session is expired, Please login first.";
    }
}
