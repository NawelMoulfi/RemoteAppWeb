using System.ComponentModel.DataAnnotations;

namespace RemoteApp.Data.Models;

public class Client
{
    public int ClientId { set; get; }
    [Required]
    public string FirstName { set; get; }
    [Required]
    public string LastName { set; get; }
    [Required]

    public string Wilaya  { set; get; }
    [Required]
    public string Adresse { set; get; }
    [Required]

    public string PID { set; get; }
    [Required]
    public string PhoneNumber { set; get; }

    public string FullName => $"{FirstName} {LastName}";
}