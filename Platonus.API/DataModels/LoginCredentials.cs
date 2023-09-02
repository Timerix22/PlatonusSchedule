namespace Platonus.API;

public struct LoginCredentials
{
    /// <summary>
    /// student_id@iitu.edu.kz
    /// </summary>
    public readonly string Login;

    public readonly string Password;

    public readonly PlatonusLanguage Language;

    public LoginCredentials(string login, string password, PlatonusLanguage language)
    {
        Login = login;
        Password = password;
        Language = language;
    }
}