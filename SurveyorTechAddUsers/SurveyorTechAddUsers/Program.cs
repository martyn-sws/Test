// See https://aka.ms/new-console-template for more information
using SurveyorTechAddUsers;

Console.WriteLine("Hello, World!");

var users = new List<UserModel>();

AddUsers(users);

foreach (var user in users)
{
    using (HttpClient client = new HttpClient())
    {

    }
}

void AddUsers(List<UserModel> users)
{
    
}