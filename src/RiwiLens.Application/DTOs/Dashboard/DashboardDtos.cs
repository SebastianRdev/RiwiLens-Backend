namespace src.RiwiLens.Application.DTOs.Dashboard;

public class DashboardStatsDto
{
    public int TotalCoders { get; set; }
    public int TotalTeamLeaders { get; set; }
    public int ActiveClans { get; set; }
    public List<TopTechnologyDto> TopTechnologies { get; set; } = new();
}

public class TopTechnologyDto
{
    public string Name { get; set; } = string.Empty;
    public int Count { get; set; }
}
