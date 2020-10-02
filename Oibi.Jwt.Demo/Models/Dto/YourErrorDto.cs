namespace Oibi.Jwt.Demo.Models.Dto
{
    public class YourErrorDto
    {
        public YourErrorDto(string message)
        {
            Message = message;
        }

        public string Code { get; set; }
        public string Message { get; set; }
    }
}