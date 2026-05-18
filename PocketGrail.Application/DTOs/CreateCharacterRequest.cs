namespace PocketGrail.Application.DTOs;

using Microsoft.AspNetCore.Http;

public sealed class CreateCharacterRequest
{
    public string Name { get; set; } = string.Empty;
    public string Race { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public int StartLevel { get; set; } = 1;
    public int? SubclassId { get; set; }
    public int? CampaignId { get; set; }
    public IFormFile? Image { get; set; }

    // Base ability scores chosen by player
    public int StrScore { get; set; } = 10;
    public int DexScore { get; set; } = 10;
    public int ConScore { get; set; } = 10;
    public int IntScore { get; set; } = 10;
    public int WisScore { get; set; } = 10;
    public int ChaScore { get; set; } = 10;

    // Flexible racial bonus distribution (e.g. Half-Elf gets +2 CHA fixed + 2 free points)
    // Each field represents how many free bonus points to add to that ability (0 if none)
    public int FlexStrBonus { get; set; }
    public int FlexDexBonus { get; set; }
    public int FlexConBonus { get; set; }
    public int FlexIntBonus { get; set; }
    public int FlexWisBonus { get; set; }
    public int FlexChaBonus { get; set; }

    // IDs of existing items to add to the character's inventory at creation
    public List<int> StartingItemIds { get; set; } = [];

    // Class skill proficiency choices (player picks N from class list)
    public List<string> SkillChoices { get; set; } = [];

    // Additional proficiency choices
    public List<string> WeaponChoices { get; set; } = [];
    public List<string> ArmorChoices { get; set; } = [];
    public List<string> LanguageChoices { get; set; } = [];
    public List<string> InstrumentChoices { get; set; } = [];
}
