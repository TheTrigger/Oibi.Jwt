namespace Oibi.Jwt.Demo.Models.Dto
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Token { get; set; }
    }

    /// <summary>
    /// Login response. <see cref="TData"/> could be your user dto or your error type
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class LoginResponse<TData> : LoginResponse
    {
        public TData Data { get; set; }
    }
}