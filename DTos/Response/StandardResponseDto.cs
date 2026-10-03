namespace Hirealdoor.DTos.Response;

public class StandardResponseDto
{
    public bool Success { get; set; } = true;
    public string Message { get; set; } = "";
    public dynamic? Object { get; set; }
}