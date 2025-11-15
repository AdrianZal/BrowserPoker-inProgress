using PokerServer.Models.DTOs;

namespace PokerServer.Models.Utility
{
    public enum RegistrationErrorType
    {
        None, // Use this for checks, but ideally don't put it in the ErrorTypes list
        NameInUse,
        EmailInUse
    }

    public class RegisterResponse
    {
        public RegisterResponseDto? Response { get; set; }
        public List<RegistrationErrorType> ErrorTypes { get; set; } = new List<RegistrationErrorType>();
        public bool IsSuccess => ErrorTypes.Count == 0; // Check if the list is empty for success

        // 1. Corrected Success Helper
        public static RegisterResponse Success(RegisterResponseDto response) =>
            new RegisterResponse { Response = response /* ErrorTypes defaults to empty list */ };

        // 2. Failure Helper (Accepts list of errors)
        public static RegisterResponse Failure(List<RegistrationErrorType> errors) =>
            new RegisterResponse { ErrorTypes = errors };

        // 3. Failure Helper (Accepts single error, creates list)
        public static RegisterResponse Failure(RegistrationErrorType error) =>
            new RegisterResponse { ErrorTypes = new List<RegistrationErrorType> { error } };
    }
}
