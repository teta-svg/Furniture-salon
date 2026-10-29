using FurnitureSalon.Core.DTO.Login;

namespace FurnitureSalon.Application.Shared
{
    public static class CurrentUser
    {
        public static LoginResponse? User { get; private set; }

        public static void Set(LoginResponse user)
        {
            User = user;
        }

        public static void Clear()
        {
            User = null;
        }
    }
}