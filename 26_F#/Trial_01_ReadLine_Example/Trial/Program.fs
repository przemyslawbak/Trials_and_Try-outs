open System

Console.WriteLine("Type something:");
let line = Console.ReadLine()
Console.WriteLine("You wrote {0}", line)

// Just to make it pause
Console.WriteLine("Press any key");  
Console.ReadKey();
