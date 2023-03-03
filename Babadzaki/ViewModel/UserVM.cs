namespace Babadzaki.ViewModel
{
    public class UserVM//пользователь на стороне сервера
    {
        public string Account { get; set; } // Уникальное имя учетной записи(учетная запись Ethereum)
        public string Name { get; set; } // Имя пользователя
        public string Email { get; set; } // Email
    }
}
