using Microsoft.AspNetCore.Mvc;
using RSALibrary;
using WebApplication.Session;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ControllerRSA : ControllerBase
    {
        private readonly IRSASession _rsaSession;

        public ControllerRSA(IRSASession rsaSession)
        {
            _rsaSession = rsaSession;
        }

        [HttpPost("encrypt")]
        public async Task<ActionResult<string>> GetEncryptMessage([FromBody] RSAresult result)
        {
            string encryptMessage = _rsaSession.rsa.EncryptString(result.Message);

            return Ok(new { encryptedMessage = encryptMessage });
        }

        [HttpPost("decrypt")]
        public async Task<ActionResult<string>> GetDecryptMessage([FromBody] RSAresult result)
        {
            Console.WriteLine($"Строка для шифрования: {result.Message}") ;
            string decryptMessage = _rsaSession.rsa.DecryptString(result.Message);

            return Ok(new { decryptedMessage = decryptMessage });
        }

        [HttpGet("changeState")]
        public async Task<ActionResult> ChangeState()
        {
            _rsaSession.rsa = new RSAlib();
            return Ok();
        }

        [HttpGet("getStates")]
        public async Task<ActionResult<Dictionary<string, string>>> GetStates()
        {
            Dictionary<string, string> states = new Dictionary<string, string>();

            string n = _rsaSession.rsa.GetN().ToString();
            string s = _rsaSession.rsa.Gets().ToString();
            string e = _rsaSession.rsa.Gete().ToString();

            states["N"] = n;
            states["S"] = s;
            states["E"] = e;
            states["P"] = _rsaSession.rsa._p.ToString();
            states["Q"] = _rsaSession.rsa._q.ToString();

            return Ok(states);
        }
    }
}
