using System;
using BCrypt.Net;

internal class Program1
{
    private static void Main(string[] args)
    {
        var hash = BCrypt.Net.BCrypt.HashPassword("Admin!123");
        Console.WriteLine(hash);
        Console.WriteLine(BCrypt.Net.BCrypt.Verify("Admin!123", hash));
    }
}