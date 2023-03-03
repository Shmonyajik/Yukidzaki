namespace Babadzaki.ViewModel
{
    public class LoginVM//данные предоставленные клиентом
    {
        public string Signer { get; set; } // Учетная запись Ethereum, требующая подписи
        public string Signature { get; set; } // Подпись
        public string Message { get; set; } // Простое сообщение
        public string Hash { get; set; } // Сообщение с префиксом и хешированием sha3
    }
}
