namespace AppointmentManagement.Application.DTOs.Requests.User
{
    public class UpdateUserRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
