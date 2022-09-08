using BattleshipGame.ConsoleRunner;

try
{
    var output = new ConsoleOutput();
    var runner = new ConsoleGameRunner(output, 10, 10);
    runner.Run();
}
catch
{
    Console.WriteLine("Unfortunately, something went wrong");
    // add some logging here
}

Console.ReadKey();