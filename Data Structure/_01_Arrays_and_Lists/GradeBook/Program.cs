namespace GradeBook;

public class Program
{
    // Scenario: A class has a fixed number of students enrolled.
    // We use an Array — size is known upfront and never changes.
    // Arrays are perfect for fixed, structured data like grade books.

    static void Main(string[] args)
    {
        // ✅ Fixed size — 5 students enrolled this semester
        string[] students = { "Anjali", "Binoy", "Chinmay", "Debargha", "Eshita" };
        double[] grades   = { 88.5,    72.0,  95.5,      61.0,    79.5  };

        Console.WriteLine("📚 Student Grade Book");
        Console.WriteLine(new string('=', 45));

        PrintGradeBook(students, grades);      // O(n) — print all
        Console.WriteLine();
        FindHighestGrade(students, grades);    // O(n) — scan for max
        Console.WriteLine();
        FindLowestGrade(students, grades);     // O(n) — scan for min
        Console.WriteLine();
        ComputeClassAverage(grades);           // O(n) — sum all grades
        Console.WriteLine();
        GetStudentGrade(students, grades, "Charlie"); // O(n) — search by name
    }

    // O(n) — prints every student and their grade
    static void PrintGradeBook(string[] students, double[] grades)
    {
        Console.WriteLine("📋 Grade Book:");
        Console.WriteLine(new string('-', 35));

        // Direct index access → O(1) per element
        for (int i = 0; i < students.Length; i++)
        {
            string grade = GetLetterGrade(grades[i]);
            Console.WriteLine($"   {i + 1}. {students[i],-10} {grades[i]:F1}%  [{grade}]");
        }
    }

    // O(n) — scans every grade to find the highest
    static void FindHighestGrade(string[] students, double[] grades)
    {
        int    topIndex = 0;
        double topGrade = grades[0];   // O(1) — fixed variable

        for (int i = 1; i < grades.Length; i++)
        {
            if (grades[i] > topGrade)
            {
                topGrade = grades[i];
                topIndex = i;
            }
        }

        Console.WriteLine($"🏆 Highest Grade : {students[topIndex]} → {topGrade:F1}% [{GetLetterGrade(topGrade)}]");
    }

    // O(n) — scans every grade to find the lowest
    static void FindLowestGrade(string[] students, double[] grades)
    {
        int    lowIndex = 0;
        double lowGrade = grades[0];   // O(1) — fixed variable

        for (int i = 1; i < grades.Length; i++)
        {
            if (grades[i] < lowGrade)
            {
                lowGrade = grades[i];
                lowIndex = i;
            }
        }

        Console.WriteLine($"📉 Lowest Grade  : {students[lowIndex]} → {lowGrade:F1}% [{GetLetterGrade(lowGrade)}]");
    }

    // O(n) — sums all grades then divides
    static void ComputeClassAverage(double[] grades)
    {
        double total = 0;   // O(1) — fixed variable

        for (int i = 0; i < grades.Length; i++)
            total += grades[i];

        double average = total / grades.Length;
        Console.WriteLine($"📊 Class Average : {average:F1}% [{GetLetterGrade(average)}]");
    }

    // O(n) — searches for a student by name
    static void GetStudentGrade(string[] students, double[] grades, string name)
    {
        for (int i = 0; i < students.Length; i++)
        {
            if (students[i] == name)
            {
                Console.WriteLine($"🔍 Search Result : {name} → {grades[i]:F1}% [{GetLetterGrade(grades[i])}]");
                return;
            }
        }

        Console.WriteLine($"⚠️  Student '{name}' not found.");
    }

    // O(1) — simple conditional check, no loops
    static string GetLetterGrade(double grade) => grade switch
    {
        >= 90 => "A",
        >= 80 => "B",
        >= 70 => "C",
        >= 60 => "D",
        _     => "F"
    };
}