// See https://aka.ms/new-console-template for more information
using JSON_01_Trial;
using System.Net.Http.Json;

Console.WriteLine("Hello, World!");

SampleObject sample = new SampleObject() { user_name = "xxx", UserSurname = "yyy" };

var content = JsonContent.Create(sample);

string zzz = content.ToString();

var aa = 1;