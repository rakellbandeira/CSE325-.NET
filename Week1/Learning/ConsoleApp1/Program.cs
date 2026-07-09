using System;

DateTime today = DateTime.Today;
int year = today.Year;
DateTime christmas = new DateTime(year, 12, 25);
int daysUntil = (christmas - today).Days;

Console.WriteLine($"There are {daysUntil} days until Christmas!");