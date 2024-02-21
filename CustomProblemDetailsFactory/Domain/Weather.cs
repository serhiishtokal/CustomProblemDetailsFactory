namespace CustomProblemDetailsFactory.Controllers.Domain;

public class Weather
{
    public float Temperature { get; set; }

    public Weather(float temperature)
    {
        this.Temperature = temperature;
    }
}