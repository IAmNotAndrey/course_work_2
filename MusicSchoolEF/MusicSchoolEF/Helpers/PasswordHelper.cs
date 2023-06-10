using System.Security.Cryptography;
using System.Text;

namespace MusicSchoolEF.Helpers
{
	public static class PasswordHelper
	{
		public static string GetHashPassword(string password)
		{
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

            return hash;
        }

		public static bool IsPasswordValid(string password, string hashedPassword)
		{
			string hashedInputPassword = GetHashPassword(password);

			return hashedInputPassword == hashedPassword;
		}
	}
}
