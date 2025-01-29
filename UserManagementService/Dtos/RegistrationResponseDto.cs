namespace UserManagementService.Dtos
{
    public class RegistrationResponseDto
    {
        public bool IsSuccesful { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
