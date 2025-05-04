using SubjectTeacherAppointmentMVC.Models.DataBase.Tables;
using System.Globalization;

namespace SubjectTeacherAppointmentMVC.Utils {
	public static class Formatting {
		private static NumberFormatInfo NFI { get { return _NFI == null ? _NFI = new NumberFormatInfo() : _NFI; } }
		private static NumberFormatInfo _NFI;

		public static Func<double?, string, string> toFixed = (v, p) => v == null || v == 0 ? "" : v.Value.ToString(p);
		public static Func<double?, string, string> toFixedPercent = (v, p) => v == null || v == 0 ? "" : $"{v.Value.ToString(p)} %";
		public static Func<double?, string> toDefaultCulture = (v) => v == null || v == 0 ? "" : v.Value.ToString(NFI);
		public static string getTeacherDisplayString(Teacher teacher) => $"{teacher.LastName} {teacher.Name}{(string.IsNullOrEmpty(teacher.Surname) ? "" : $" {teacher.Surname}")}";
	}
}
