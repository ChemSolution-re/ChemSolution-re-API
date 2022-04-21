using System.Security.Cryptography;
using System.Text;

namespace ChemSolution_re_API.Services.Pay
{
    public class PayService : IPayService
    {
        private readonly string _publicKey = "sandbox_i78702606286";
        private readonly string _privateKey = "sandbox_s4FSNyEIpXb2eOU076bas7t2q29Z4EWJRJtpor9b";

        public PayButtonModel GetPayButton(PayOptions options)
        {
            var dataJson = $@"{{""public_key"":""{_publicKey}"",""version"":""{options.Version}"",""amount"":""{options.Amount}""," +
            @$"""action"":""pay"",""currency"":""{options.Currency}"",""description"":""{options.Description}"",""order_id"":""{options.OrderId}""}}";

            var dataResult = GetData(dataJson);

            var signString = _privateKey
                             + dataResult
                             + _privateKey;

            var signature = GetSignature(signString);

            return new PayButtonModel
            {
                Data = dataResult,
                Signature = signature
            };
        }

        private string GetSignature(string input)
        {
            byte[] hash;
            var sha1 = SHA1.Create();
            hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hash);

        }
        private string GetData(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(bytes);
        }
    }
}
