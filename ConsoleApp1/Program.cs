using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Student
{
	public string Name { get; set; }
	public int Semester { get; set; }
	public string CourseName { get; set; }

	public Student(string name, int semester, string courseName)
	{
		Name = name;
		Semester = semester;
		CourseName = courseName;
	}

	public override string ToString()
	{
		return $"{Name,-20} | Semester: {Semester,-3} | Course: {CourseName}";
	}
}

class StudentManager
{
	private List<Student> students = new List<Student>();
	private readonly string filePath = "students.txt";
	private readonly string[] validCourses = { "Java", ".Net", "C/C++" };

	// Đọc dữ liệu từ file
	public void LoadFromFile()
	{
		if (!File.Exists(filePath)) return;

		foreach (var line in File.ReadAllLines(filePath))
		{
			var parts = line.Split('|');
			if (parts.Length == 3)
			{
				string name = parts[0].Trim();
				int semester = int.Parse(parts[1].Trim());
				string course = parts[2].Trim();
				students.Add(new Student(name, semester, course));
			}
		}
	}

	// Ghi dữ liệu ra file
	public void SaveToFile()
	{
		var lines = students.Select(s => $"{s.Name}|{s.Semester}|{s.CourseName}");
		File.WriteAllLines(filePath, lines);
	}

	// Thêm sinh viên
	public void AddStudent()
	{
		Console.Write("Nhập tên sinh viên: ");
		string name = Console.ReadLine();

		Console.Write("Nhập học kỳ: ");
		int semester = int.Parse(Console.ReadLine());

		Console.Write("Nhập môn học (Java, .Net, C/C++): ");
		string course = Console.ReadLine();

		if (!validCourses.Contains(course))
		{
			Console.WriteLine("❌ Môn học không hợp lệ!");
			return;
		}

		students.Add(new Student(name, semester, course));
		Console.WriteLine("✅ Đã thêm sinh viên.");
	}

	// Tìm kiếm sinh viên theo tên
	public void SearchByName()
	{
		Console.Write("Nhập tên cần tìm: ");
		string keyword = Console.ReadLine();

		var result = students.Where(s => s.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();

		if (result.Count == 0)
		{
			Console.WriteLine("❌ Không tìm thấy sinh viên.");
		}
		else
		{
			Console.WriteLine("Kết quả tìm kiếm:");
			result.ForEach(s => Console.WriteLine(s));
		}
	}

	// Sửa thông tin sinh viên
	public void EditStudent()
	{
		Console.Write("Nhập tên sinh viên cần sửa: ");
		string name = Console.ReadLine();

		var student = students.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
		if (student == null)
		{
			Console.WriteLine("❌ Không tìm thấy sinh viên.");
			return;
		}

		Console.Write("Tên mới (Enter để bỏ qua): ");
		string newName = Console.ReadLine();
		if (!string.IsNullOrWhiteSpace(newName)) student.Name = newName;

		Console.Write("Học kỳ mới (Enter để bỏ qua): ");
		string sem = Console.ReadLine();
		if (!string.IsNullOrWhiteSpace(sem)) student.Semester = int.Parse(sem);

		Console.Write("Môn học mới (Java, .Net, C/C++ - Enter để bỏ qua): ");
		string course = Console.ReadLine();
		if (!string.IsNullOrWhiteSpace(course))
		{
			if (validCourses.Contains(course)) student.CourseName = course;
			else Console.WriteLine("❌ Môn học không hợp lệ. Giữ nguyên.");
		}

		Console.WriteLine("✅ Đã cập nhật thông tin sinh viên.");
	}

	// Xóa sinh viên
	public void DeleteStudent()
	{
		Console.Write("Nhập tên sinh viên cần xóa: ");
		string name = Console.ReadLine();

		var student = students.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
		if (student == null)
		{
			Console.WriteLine("❌ Không tìm thấy sinh viên.");
			return;
		}

		students.Remove(student);
		Console.WriteLine("✅ Đã xóa sinh viên.");
	}

	// Thống kê số lần đăng ký
	public void Report()
	{
		var report = students
			.GroupBy(s => new { s.Name, s.CourseName })
			.Select(g => new { g.Key.Name, g.Key.CourseName, Count = g.Count() });

		Console.WriteLine("\nStudent Name       | Course   | Total of Course");
		Console.WriteLine("------------------------------------------------");
		foreach (var item in report)
		{
			Console.WriteLine($"{item.Name,-18} | {item.CourseName,-7} | {item.Count}");
		}
	}

	// Hiển thị tất cả sinh viên
	public void DisplayAll()
	{
		if (students.Count == 0)
		{
			Console.WriteLine("❌ Danh sách trống.");
			return;
		}
		Console.WriteLine("Danh sách sinh viên:");
		students.ForEach(s => Console.WriteLine(s));
	}
}

class Program
{
	static void Main()
	{
		Console.OutputEncoding = System.Text.Encoding.UTF8;
		StudentManager manager = new StudentManager();
		manager.LoadFromFile();

		while (true)
		{
			Console.WriteLine("\n=== MENU ===");
			Console.WriteLine("1. Thêm sinh viên");
			Console.WriteLine("2. Hiển thị tất cả sinh viên");
			Console.WriteLine("3. Tìm kiếm theo tên");
			Console.WriteLine("4. Sửa thông tin sinh viên");
			Console.WriteLine("5. Xóa sinh viên");
			Console.WriteLine("6. Thống kê số lần đăng ký");
			Console.WriteLine("7. Lưu và thoát");
			Console.Write("Chọn chức năng: ");

			string choice = Console.ReadLine();
			switch (choice)
			{
				case "1": manager.AddStudent(); break;
				case "2": manager.DisplayAll(); break;
				case "3": manager.SearchByName(); break;
				case "4": manager.EditStudent(); break;
				case "5": manager.DeleteStudent(); break;
				case "6": manager.Report(); break;
				case "7":
					manager.SaveToFile();
					Console.WriteLine("💾 Đã lưu dữ liệu. Thoát chương trình.");
					return;
				default: Console.WriteLine("❌ Lựa chọn không hợp lệ."); break;
			}
		}
	}
}
