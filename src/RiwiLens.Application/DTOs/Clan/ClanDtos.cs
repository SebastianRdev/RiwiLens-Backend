namespace src.RiwiLens.Application.DTOs.Clan;

public class ClanResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ActiveCodersCount { get; set; }
    public IEnumerable<string> TeamLeaders { get; set; } = new List<string>();
}

public class CreateClanDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class UpdateClanDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class AssignCoderDto
{
    public int CoderId { get; set; }
}

public class AssignTeamLeaderDto
{
    public int TeamLeaderId { get; set; }
    public int RoleId { get; set; }
}
