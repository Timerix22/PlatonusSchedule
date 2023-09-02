using System;
using Platonus.API;

var p = new PlatonusClient();
var loginCredentials = new LoginCredentials(
    ReadString("login"),
    ReadString("password"),
    PlatonusLanguage.English);
await p.LoginAsync(loginCredentials);
var schedule = await p.GetScheduleAsync();
return;

string ReadString(string question)
{
    Console.Write($"{question}: ");
    string? answ = Console.ReadLine();
    if (string.IsNullOrEmpty(answ))
        throw new NullReferenceException();
    return answ;
}