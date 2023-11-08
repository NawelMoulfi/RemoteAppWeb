namespace RemoteApp.Data.Models;

public class Client
{
    public int ClientId { set; get; }
    public string FirstName { set; get; }
    public string LastName { set; get; }
    public string Wilaya  { set; get; }
    public string Adresse { set; get; }
    public string PID { set; get; }
    public string PhoneNumber { set; get; }

    public string FullName => $"{FirstName} {LastName}";
}