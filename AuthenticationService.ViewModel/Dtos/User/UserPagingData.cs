namespace AuthenticationService.ViewModel.Dtos.User;

public class UserPagingData
{
    public int Total { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }

    public List<UserCardDto> Users { get; set; }
}
