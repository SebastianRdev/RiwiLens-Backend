using src.RiwiLens.Domain.Enums;

namespace src.RiwiLens.Application.DTOs.Class;

public class ClassResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // e.g. "Clase de Inglés - 15/12/2025"
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int ClassTypeId { get; set; }
    public string ClassTypeName { get; set; } = string.Empty;
    public int DayId { get; set; }
    public string DayName { get; set; } = string.Empty; // Morning / Afternoon
    public int ClanId { get; set; }
    public string ClanName { get; set; } = string.Empty;
    public int TeamLeaderId { get; set; }
    public string TeamLeaderName { get; set; } = string.Empty;
}

public class CreateClassDto
{
    public DateTime Date { get; set; }
    public int ClassTypeId { get; set; }
    public int DayId { get; set; }
    public int ClanId { get; set; }
    public int TeamLeaderId { get; set; }
}

public class UpdateClassDto
{
    public DateTime Date { get; set; }
    public int ClassTypeId { get; set; }
    public int DayId { get; set; }
    public int TeamLeaderId { get; set; }
}
