using System.Security.Cryptography;
using System.Text;

namespace SubjectTeacherAppointmentMVC.Utils {
	public class UserInfo {

		public static byte[] EncodePassword(string password, Guid? userID) {
			var textArr = Encoding.UTF8.GetBytes(password + userID);
			var sha256 = SHA256.Create();
			var hashArr = sha256.ComputeHash(textArr);
			return hashArr;
		}

	}
}
