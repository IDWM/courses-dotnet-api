using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using courses_dotnet_api.Src.DTOs.Account;
using courses_dotnet_api.Src.Models;

namespace courses_dotnet_api.Src.Data;

public class Seeder()
{
    private static readonly JsonSerializerOptions _options =
        new() { PropertyNameCaseInsensitive = true };

    public static void Seed(DataContext dataContext)
    {
        SeedRoles(dataContext);
        SeedTeacher(dataContext);
        SeedStudents(dataContext);
    }

    public static void SeedRoles(DataContext dataContext)
    {
        var rolesData = File.ReadAllText("Src/Data/Seed/roles.json");
        
        var roles =
            JsonSerializer.Deserialize<List<Role>>(rolesData, _options)
            ?? throw new Exception("Roles not found");

        if (dataContext.Roles.Any()) return;
        
        dataContext.Roles.AddRange(roles);
        
        dataContext.SaveChanges();
            
    }

    public static void SeedTeacher(DataContext dataContext)
    {
        if (dataContext.Users.Any(user => user.Role.Name == "teacher"))
        {
            return;
        }

        Role adminRole = dataContext.Roles.First(role => role.Name == "teacher");
        string password = "p4ssw0rd";
        using var hmac = new HMACSHA512();

        User admin =
            new()
            {
                Rut = "204166994",
                Name = "jorge rivera",
                Email = "jorge.rivera01@ce.ucn.cl",
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmac.Key,
                RoleId = adminRole.Id,
                Role = adminRole
            };

        dataContext.Users.Add(admin);
        dataContext.SaveChanges();
    }

    public static void SeedStudents(DataContext dataContext)
    {
        var studentData = File.ReadAllText("Src/Data/Seed/students.json");
        var students =
            JsonSerializer.Deserialize<List<RegisterDto>>(studentData, _options)
            ?? throw new Exception("Students not found");

        Role studentRole = dataContext.Roles.First(role => role.Name == "student");

        foreach (RegisterDto student in students)
        {
            if (dataContext.Users.Any(x => x.Rut == student.Rut || x.Email == student.Email))
            {
                continue;
            }

            using var hmac = new HMACSHA512();

            User user =
                new()
                {
                    Rut = student.Rut,
                    Name = student.Name,
                    Email = student.Email,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(student.Password)),
                    PasswordSalt = hmac.Key,
                    RoleId = studentRole.Id,
                    Role = studentRole
                };

            dataContext.Users.Add(user);
        }

        dataContext.SaveChanges();
    }
}
