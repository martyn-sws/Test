// See https://aka.ms/new-console-template for more information
using SurveyorTechAddUsers;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;

Console.WriteLine("Welcome to the SurveyorTech user adder program thingy");

var users = new List<UserModel>();

AddUsers(users);
Console.WriteLine("Adding {0} users", users.Count);
var ServerAddress = "";

if (!string.IsNullOrEmpty(ServerAddress))
{
    var URL = ServerAddress + "/api/Account/Register";

    foreach (var user in users)
    {
        Console.WriteLine("Adding user: {0}", user.Email);
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var json = JsonSerializer.Serialize(user);
            var payload = new StringContent(json);
            var response = client.PostAsync(URL, payload).Result;
            Console.WriteLine("API returned response: {0}", response.StatusCode.ToString());
        }
    }
} else { 
    Console.WriteLine("Server Address not defined");
}

void AddUsers(List<UserModel> users)
{
    
}