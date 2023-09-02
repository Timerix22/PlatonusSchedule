using System;
using System.Linq;
using System.Text;
using DTLib.Ben.Demystifier;
using Platonus.API;
using Platonus.API.DataModels;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;
try
{
    var p = new PlatonusClient();
    var loginCredentials = new LoginCredentials(
        ReadString("student id") + "@iitu.edu.kz",
        ReadString("password", true),
        PlatonusLanguage.Parse(ReadString("language (en/ru/kz)"))
    );
    await p.LoginAsync(loginCredentials);
    var schedule = await p.GetScheduleAsync();
    PrintSchedule(schedule);
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToStringDemystified());
}
Console.Write("press [ENTER] to exit");
Console.ReadLine();


string ReadString(string question, bool hideInput = false)
{
    Console.Write($"{question}: ");
    string? answ = Console.ReadLine();
    if (string.IsNullOrEmpty(answ))
        throw new NullReferenceException();
    if (hideInput)
    {
        Console.CursorTop--;
        Console.CursorLeft = question.Length + 2;
        for (int i = 0; i < answ.Length; i++) 
            Console.Write('*');
        Console.WriteLine();
    }
    return answ;
}

void PrintHeader(char sep, string title)
{
    int sepCount = Console.BufferWidth - 4 - title.Length;
    for (int i = 0; i < sepCount / 2; i++)
        Console.Write(sep);
    Console.Write('[');
    Console.Write(title);
    Console.Write(']');
    for (int i = 0; i < sepCount / 2 + sepCount % 2; i++)
        Console.Write(sep);
    Console.WriteLine();
}

void PrintSchedule(Schedule s)
{
    PrintHeader('=', "SCHEDULE");
    Console.WriteLine($"selected year: {s.SelectedYear}");
    Console.WriteLine($"selected term: {s.SelectedTermNumber}");
    Console.WriteLine($"selected week: {s.SelectedWeekNumber} of {s.TermWeekNumbers.Last()}");
    Console.WriteLine($"current week: {s.CurrentWeekNumber} of {s.TermWeekNumbers.Last()}");
    
    foreach (var day in s.Days)
    {
        PrintHeader('-', $"{day.DayOfWeek:G}");
        foreach (var lessonHour in day.LessonHours)
        {
            if (lessonHour.Lesson is not null)
            {
                Console.Write($"{lessonHour.Time.Start:hh\\:mm}-{lessonHour.Time.Finish:hh\\:mm}");
                Schedule.Day.Lesson l = lessonHour.Lesson;
                Console.WriteLine($" | {l.SubjectName} ({l.LessonTypeName}) at {l.Auditory}");
                Console.WriteLine($"            | tutor: {l.TutorName}");
            }
        }
    }
}
