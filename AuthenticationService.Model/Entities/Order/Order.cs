namespace AuthenticationService.Model.Entities.Order;

public class Order
{
    public string UserId { get; set; }
    public string PhoneNumber { get; set; }
    public string AddressId { get; set; }
    public DateTime ReceiveTime { get; set; }
    public string Sate { get; set; }
    public string PaymentMethod { get; set; }
    public string Mode { get; set; }
    public Double Total { get; set; }
    public Double Discount { get; set; }
}
