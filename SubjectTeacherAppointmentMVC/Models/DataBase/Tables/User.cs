using System.ComponentModel.DataAnnotations.Schema;

namespace SubjectTeacherAppointmentMVC.Models.DataBase.Tables {
	public class User {
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid? UserID { get; set; }
		public string? Login { get; set; }
		[NotMapped]
		public string? Password { get; set; }
		public byte[]? PasswordHashed { get; set; }
		public bool IsAdmin { get; set; }
	}
}
