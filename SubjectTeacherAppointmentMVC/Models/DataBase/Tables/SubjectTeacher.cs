using System.ComponentModel.DataAnnotations.Schema;

namespace SubjectTeacherAppointmentMVC.Models.DataBase.Tables {
	public class SubjectTeacher {
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid? SubjectTeacherID { get; set; }
		public Guid? SubjectID { get; set; }
		public Guid? TeacherID { get; set; }
		public int? HoursPerWeek { get; set; }
		[NotMapped]
		public Subject? Subject { get; set; }
		[NotMapped]
		public Teacher? Teacher { get; set; }
	}
}
