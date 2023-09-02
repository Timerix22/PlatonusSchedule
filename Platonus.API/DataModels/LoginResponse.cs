namespace Platonus.API.DataModels;

class LoginResponse
{
    /// session_id
    public string? sid { get; set; }
    public string? auth_token { get; set; }
    /// should be "success"
    public string? login_status { get; set; }
}