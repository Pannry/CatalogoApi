namespace ApiCatalogo._6.DTOs
{
    public class UsuarioToken
    {
        public bool Authenticated { get; set; }
        public DateTime Expirated { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
    }
}
