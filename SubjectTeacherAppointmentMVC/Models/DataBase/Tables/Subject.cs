using System.ComponentModel.DataAnnotations.Schema;

namespace SubjectTeacherAppointmentMVC.Models.DataBase.Tables {
	public class Subject {
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid? SubjectID { get; set; }
		public string? Name { get; set; }
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime? CreationDate { get; set; }
		[NotMapped]
		public int? HoursPerWeekTotal { get; set; }
	}
}
