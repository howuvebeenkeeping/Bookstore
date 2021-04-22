namespace Bookstore.Messages {
    public interface INotificationService {
        void SendConfirmationCode(string cellPhone, int code);
    }
}