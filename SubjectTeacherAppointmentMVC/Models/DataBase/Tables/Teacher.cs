using System.ComponentModel.DataAnnotations.Schema;

namespace SubjectTeacherAppointmentMVC.Models.DataBase.Tables {
	public class Teacher {
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid? TeacherID { get; set; }
		public string? LastName { get; set; }
		public string? Surname { get; set; }
		public string? Name { get; set; }
		public DateTime? Birthday { get; set; }
		public string? Sex { get; set; }
		public byte[]? Photo { get; set; }
		[NotMapped]
		public IFormFile? PhotoFormFile { get; set; }
		public string? Notes { get; set; }
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime? CreationDate { get; set; }
	}
}
