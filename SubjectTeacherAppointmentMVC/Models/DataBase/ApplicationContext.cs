using Microsoft.EntityFrameworkCore;
using SubjectTeacherAppointmentMVC.Models.DataBase.Tables;
using System.Formats.Asn1;

namespace SubjectTeacherAppointmentMVC.Models.DataBase {
	public class ApplicationContext : DbContext {


		#region Tables

		public DbSet<Subject> Subjects { get; set; } = null!;
		public DbSet<Teacher> Teachers { get; set; } = null!;
		public DbSet<SubjectTeacher> SubjectTeachers { get; set; } = null!;
		public DbSet<User> Users { get; set; } = null!;

		#endregion

		#region Views
		#endregion

		public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { 
		}

	}
}
