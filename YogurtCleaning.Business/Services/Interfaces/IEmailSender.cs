namespace YogurtCleaning.Business.Services;

public interface IEmailSender
{
    void SendEmail(int orderId);
}