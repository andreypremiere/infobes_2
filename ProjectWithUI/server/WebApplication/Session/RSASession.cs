using RSALibrary;

namespace WebApplication.Session
{
    public class RSASession : IRSASession
    {
        public RSAlib rsa { get; set; } = new RSAlib();
    }
}
