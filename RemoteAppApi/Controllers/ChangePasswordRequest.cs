namespace RemoteAppApi.Controllers
{
    public class ChangePasswordRequest
    {
        public string NewPassword { get;  set; }
        public int UserId { get;  set; }
    }
}
