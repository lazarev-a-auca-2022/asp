using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Diagnostics;
using TestApplication.Models;

namespace TestApplication.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly string _connectionString;

    public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult SubmitForm(string name, string surname, int age, string phoneNumber)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            var command = new NpgsqlCommand("INSERT INTO FormData (Name, Surname, Age, PhoneNumber) VALUES (@name, @surname, @age, @phoneNumber)", connection);
            command.Parameters.AddWithValue("name", name);
            command.Parameters.AddWithValue("surname", surname);
            command.Parameters.AddWithValue("age", age);
            command.Parameters.AddWithValue("phoneNumber", phoneNumber);
            command.ExecuteNonQuery();
        }

        return RedirectToAction("Privacy");
    }

    public IActionResult Privacy()
    {
        List<FormData> formDataList = new List<FormData>();

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
            var command = new NpgsqlCommand("SELECT * FROM FormData", connection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    formDataList.Add(new FormData
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Surname = reader.GetString(2),
                        Age = reader.GetInt32(3),
                        PhoneNumber = reader.GetString(4)
                    });
                }
            }
        }

        return View(formDataList);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}