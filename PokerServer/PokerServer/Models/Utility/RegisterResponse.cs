using PokerServer.Models.DTOs;

namespace PokerServer.Models.Utility
{
    public enum RegistrationErrorType
    {
        None,
        NameInUse,
        EmailInUse
    }

    public class RegisterResponse
    {
        public RegisterResponseDto? Response { get; set; }
        public List<RegistrationErrorType> ErrorTypes { get; set; } = new List<RegistrationErrorType>();
        public bool IsSuccess => ErrorTypes.Count == 0;

        public static RegisterResponse Success(RegisterResponseDto response) =>
            new RegisterResponse { Response = response};

        public static RegisterResponse Failure(List<RegistrationErrorType> errors) =>
            new RegisterResponse { ErrorTypes = errors };

        public static RegisterResponse Failure(RegistrationErrorType error) =>
            new RegisterResponse { ErrorTypes = new List<RegistrationErrorType> { error } };
    }
}
