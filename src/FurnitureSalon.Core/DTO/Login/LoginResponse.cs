namespace FurnitureSalon.Core.DTO.Login
{
    public class LoginResponse
    {
        public string Surname { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Patronymic { get; set; }
        public string EmployeeRole { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}