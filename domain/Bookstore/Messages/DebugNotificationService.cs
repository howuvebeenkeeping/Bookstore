using System.Diagnostics;

namespace Bookstore.Messages {
    public class DebugNotificationService : INotificationService {
        public void SendConfirmationCode(string cellPhone, int code) {
            Debug.WriteLine($"Cell phone: {cellPhone}, code: {code:0000}.");
        }
    }
}