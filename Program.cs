using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

public class Person
{
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string MiddleInitial { get; set; }
    public string DateOfBirth { get; set; }
    public string ContactNumber { get; set; }
    public string Address { get; set; }

    public Person(string lastName, string firstName, string middleInitial, string dateOfBirth, string contactNumber, string address)
    {
        LastName = lastName;
        FirstName = firstName;
        MiddleInitial = middleInitial;
        DateOfBirth = dateOfBirth;
        ContactNumber = contactNumber;
        Address = address;
    }
}

public class Student : Person
{
    public string ID { get; private set; }
    public string Teacher { get; set; }
    public string Schedule { get; set; }
    public List<string> Courses { get; set; }

    public Student(string lastName, string firstName, string middleInitial, string dateOfBirth, string contactNumber, string address, string teacher, string schedule, List<string> courses)
        : base(lastName, firstName, middleInitial, dateOfBirth, contactNumber, address)
    {
        ID = GenerateStudentID();
        Teacher = teacher;
        Schedule = schedule;
        Courses = courses;
    }

    private string GenerateStudentID()
    {
        Random random = new Random();
        return $"{DateTime.Now.Year}-{random.Next(1000, 9999)}";
    }
}

public class StudentManager
{
    private List<Student> students = new List<Student>();

    public void AddStudent(Student student)
    {
        try
        {
            students.Add(student);
            SaveStudentsToFile();
            Console.WriteLine("Student added successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while adding the student: " + ex.Message);
        }
    }

    public void UpdateStudent(string studentID, string newLastName, string newFirstName, string newMiddleInitial, string newTeacher, string newSchedule, List<string> newCourses)
    {
        try
        {
            var student = students.FirstOrDefault(s => s.ID == studentID);
            if (student != null)
            {
                student.LastName = newLastName;
                student.FirstName = newFirstName;
                student.MiddleInitial = newMiddleInitial;
                student.Teacher = newTeacher;
                student.Schedule = newSchedule;
                student.Courses = newCourses;
                SaveStudentsToFile();
                Console.WriteLine("Student updated successfully.");
            }
            else
            {
                Console.WriteLine("Student not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while updating the student: " + ex.Message);
        }
    }

    public void DeleteStudent(string studentID)
    {
        try
        {
            var student = students.FirstOrDefault(s => s.ID == studentID);
            if (student != null)
            {
                students.Remove(student);
                SaveStudentsToFile();
                Console.WriteLine("Student deleted successfully.");
            }
            else
            {
                Console.WriteLine("Student not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while deleting the student: " + ex.Message);
        }
    }

    public void SearchStudent(string studentID)
    {
        var student = students.FirstOrDefault(s => s.ID == studentID);
        if (student != null)
        {
            DisplayStudentDetails(student);
        }
        else
        {
            Console.WriteLine("Student not found.");
        }
    }

    public void ViewStudentsByTeacher(string teacherName)
    {
        var assignedStudents = students.Where(s => s.Teacher == teacherName).ToList();
        if (assignedStudents.Count > 0)
        {
            foreach (var student in assignedStudents)
            {
                DisplayStudentDetails(student);
            }
        }
        else
        {
            Console.WriteLine("No students found for this teacher.");
        }
    }

    public void DisplayAllStudents()
    {
        if (students.Count == 0)
        {
            Console.WriteLine("No students to display.");
        }
        else
        {
            foreach (var student in students)
            {
                DisplayStudentDetails(student);
            }
        }
    }

    private void DisplayStudentDetails(Student student)
    {
        
        // Print student data in tabular format
        Console.WriteLine("{0,-15} {1,-30} {2,-15} {3,-20} {4,-30} {5,-25} {6,-30} {7,-30}",
            student.ID,
            $"{student.LastName}, {student.FirstName} {student.MiddleInitial}.",
            student.DateOfBirth,
            student.ContactNumber,
            student.Address,
            student.Teacher,
            student.Schedule,
            string.Join(", ", student.Courses));

        // Print a line to separate the rows
        Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
    }

    public void SaveStudentsToFile()
    {
        try
        {
            using (StreamWriter writer = new StreamWriter("students.txt"))
            {
                foreach (var student in students)
                {
                    writer.WriteLine($"{student.ID},{student.LastName},{student.FirstName},{student.MiddleInitial},{student.DateOfBirth},{student.ContactNumber},{student.Address},{student.Teacher},{student.Schedule},{string.Join(";", student.Courses)}");
                }
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine("Error saving to file: " + ex.Message);
        }
    }

    public void LoadStudentsFromFile()
    {
        try
        {
            if (File.Exists("students.txt"))
            {
                using (StreamReader reader = new StreamReader("students.txt"))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var parts = line.Split(',');
                        string lastName = parts[1];
                        string firstName = parts[2];
                        string middleInitial = parts[3];
                        string dob = parts[4];
                        string contact = parts[5];
                        string address = parts[6];
                        string teacher = parts[7];
                        string schedule = parts[8];
                        List<string> courses = parts[9].Split(';').ToList();

                        var student = new Student(lastName, firstName, middleInitial, dob, contact, address, teacher, schedule, courses);
                        students.Add(student);
                    }
                }
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine("Error loading from file: " + ex.Message);
        }
    }
}

class Program
{
    static void Main()
    {
        StudentManager manager = new StudentManager();
        manager.LoadStudentsFromFile();
        MenuCommand:
        Console.WriteLine("Are you:");
        Console.WriteLine("1. Registrar");
        Console.WriteLine("2. Teacher");
        Console.Write("Enter your role: ");
        string role = Console.ReadLine();

        while (true)
        {

            if (role == "1")
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("\nRegistrar Menu");
                    Console.WriteLine("1. Add Student");
                    Console.WriteLine("2. Update Student");
                    Console.WriteLine("3. Delete Student");
                    Console.WriteLine("4. Search Student");
                    Console.WriteLine("5. View All Students");
                    Console.WriteLine("6. Exit");
                    Console.Write("Enter your choice: ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            Console.Write("Enter Last Name: ");
                            string lastName = Console.ReadLine();
                            Console.Write("Enter First Name: ");
                            string firstName = Console.ReadLine();
                            Console.Write("Enter Middle Initial: ");
                            string middleInitial = Console.ReadLine();
                            Console.Write("Enter Date of Birth (yyyy-mm-dd): ");
                            string dob = Console.ReadLine();
                            Console.Write("Enter Contact Number: ");
                            string contact = Console.ReadLine();
                            Console.Write("Enter Address: ");
                            string address = Console.ReadLine();
                            Console.Write("Enter Teacher: ");
                            string teacher = Console.ReadLine();
                            Console.Write("Enter Schedule: ");
                            string schedule = Console.ReadLine();
                            Console.Write("Enter Courses (comma-separated): ");
                            List<string> courses = Console.ReadLine().Split(',').Select(c => c.Trim()).ToList();

                            manager.AddStudent(new Student(lastName, firstName, middleInitial, dob, contact, address, teacher, schedule, courses));
                            break;
                        case "2":
                            Console.Write("Enter Student ID to update: ");
                            string updateID = Console.ReadLine();
                            Console.Write("Enter New Last Name: ");
                            string newLastName = Console.ReadLine();
                            Console.Write("Enter New First Name: ");
                            string newFirstName = Console.ReadLine();
                            Console.Write("Enter New Middle Initial: ");
                            string newMiddleInitial = Console.ReadLine();
                            Console.Write("Enter New Teacher: ");
                            string newTeacher = Console.ReadLine();
                            Console.Write("Enter New Schedule: ");
                            string newSchedule = Console.ReadLine();
                            Console.Write("Enter New Courses (comma-separated): ");
                            List<string> newCourses = Console.ReadLine().Split(',').Select(c => c.Trim()).ToList();

                            manager.UpdateStudent(updateID, newLastName, newFirstName, newMiddleInitial, newTeacher, newSchedule, newCourses);
                            break;
                        case "3":
                            Console.Write("Enter Student ID to delete: ");
                            string deleteID = Console.ReadLine();
                            manager.DeleteStudent(deleteID);
                            break;
                        case "4":
                            Console.Write("Enter Student ID to search: ");
                            string searchID = Console.ReadLine();
                            manager.SearchStudent(searchID);
                            break;
                        case "5":
                            // Print the table header
                            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                            Console.WriteLine("{0,-15} {1,-30} {2,-15} {3,-20} {4,-30} {5,-25} {6,-30} {7,-30}",
                                "ID", "Name", "Date of Birth", "Contact", "Address", "Teacher", "Schedule", "Courses");
                            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------"); 
                            manager.DisplayAllStudents();
                            break;
                        case "6":
                            goto MenuCommand;
                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }

                    Console.WriteLine("\nPress Enter to continue...");
                    Console.ReadLine();
                }
            }
            else if (role == "2")
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("\nTeacher Menu");
                    Console.WriteLine("1. View All Students");
                    Console.WriteLine("2. View Students by Teacher");
                    Console.WriteLine("3. Exit");
                    Console.Write("Enter your choice: ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            manager.DisplayAllStudents();
                            break;
                        case "2":
                            Console.Write("Enter Teacher's Name: ");
                            string teacherName = Console.ReadLine();
                            // Print the table header
                            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                            Console.WriteLine("{0,-15} {1,-30} {2,-15} {3,-20} {4,-30} {5,-25} {6,-30} {7,-30}",
                                "ID", "Name", "Date of Birth", "Contact", "Address", "Teacher", "Schedule", "Courses");
                            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                            manager.ViewStudentsByTeacher(teacherName);
                            break;
                        case "3":
                            goto MenuCommand;
                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }

                    Console.WriteLine("\nPress Enter to continue...");
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("Invalid role.");
            }
        }
    }
}
