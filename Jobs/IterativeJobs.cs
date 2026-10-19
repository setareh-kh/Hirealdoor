namespace Hirealdoor.Jobs;
public class IterativeJobs
{
    public void WriteHello()
    {
        using StreamWriter writer = new("SayHello.txt", append: true);
        writer.WriteLine("Hello");
        return;
    }
    
}