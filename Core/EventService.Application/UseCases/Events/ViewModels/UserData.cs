using System.Text.Json.Serialization;

namespace EventService.Application.UseCases.Events.ViewModels
{
    public class UserData
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = default!;

        public string Username { get;  set; } = default!;

        public string Email { get;  set; } = default!;

        [JsonPropertyName("phone")]
        public string PhoneNumber { get;  set; } = default!;

        public string? Website { get; set; }

        public Address? Address { get; set; } = default!;

        public Company? Company { get; set; } = default!;
    }


    public class Address
    {
        public string Street { get;  set; } = default!;
        public string Suite { get; set; } = default!;
        public string City { get; set; } = default!;
        public string ZipCode { get; set; } = default!;
    }


    public class Company
    {
        public string Name { get; set; } = default!;
        public string CatchPhrase { get; set; } = default!;
        public string Bs { get; set; } = default!;
    }
}
