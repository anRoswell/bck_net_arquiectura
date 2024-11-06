namespace Core.DTOs.Integration.Authentication
{
	public class AuthenticationResponseDto
	{
        private string _token;
        public string token
        {
            get => _token;
            set
            {
                _token = value;
                if (!string.IsNullOrEmpty(_token) && _token.Length >= 2)
                {
                    role = _token[^2..];
                }
            }
        }
        public string fecha_vence { get; set; }
        public string role { get; private set; }
    }
}