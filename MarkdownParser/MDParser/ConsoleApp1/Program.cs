using MDParser.models;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace MDParser

{
    internal class Program
    {
        private static string _SaveTargetLocation = "";

    static void Main()
        {
            var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
            _SaveTargetLocation = config["FileLocations:SaveTargetLocation"];
            string genMonsterFileLocation = config["FileLocations:GenMonsterTargetLocation"];
            string taldorieMonsterTargetLocation = config["FileLocations:TaldorieMonsterTargetLocation"];
            string tombOfBeasts1TargetLocation = config["FileLocations:TombOfBeasts1TargetLocation"];
            string tombOfBeasts2TargetLocation = config["FileLocations:TombOfBeasts2TargetLocation"];
            string tombOfBeasts3TargetLocation = config["FileLocations:TombOfBeasts3TargetLocation"];

            string[] targetLocations = new[]
            {
                genMonsterFileLocation,
                taldorieMonsterTargetLocation,
                tombOfBeasts1TargetLocation,
                tombOfBeasts2TargetLocation,
                tombOfBeasts3TargetLocation
            };

            HashSet<Monster> monsters = new HashSet<Monster>();

            foreach (string targetLocation in targetLocations)
            {
                HashSet<Monster> tempMonsters = DeserializeMonstersFromFile(targetLocation);
                monsters.UnionWith(tempMonsters);
            }

            foreach (Monster monster in monsters)
            {
                monster.Fields.ActionsMarkdownString = GenerateActionsMarkdownString(monster.Fields.ActionsJson, "Actions");
                if (monster.Fields.BonusActionsJson != "null")
                {
                    monster.Fields.BonusActionMarkdownString = GenerateActionsMarkdownString(monster.Fields.BonusActionsJson, "Bonus Actions");
                }
                if (monster.Fields.SpecialAbilitiesJson != "null")
                {
                    monster.Fields.SpecialAbilitiesMarkdownString = GenerateActionsMarkdownString(monster.Fields.SpecialAbilitiesJson, "Abilities");
                }
                if (monster.Fields.LegendaryActionsJson != "null")
                {
                    monster.Fields.LegendaryActionsMarkdownString = GenerateActionsMarkdownString(monster.Fields.LegendaryActionsJson, "Legendary Actions");
                }
                if (monster.Fields.ReactionsJson != "null")
                {
                    monster.Fields.ReactionsMarkdownString = GenerateActionsMarkdownString(monster.Fields.ReactionsJson, "Reactions");
                }

                monster.Fields.MarkdownString = FillMonsterTemplate(monster.Fields);

                // Replace slash with underscore in CR
                string crForDirectoryName = monster.Fields.ChallengeRating.Replace("/", "_");

                // Convert CR to a two-digit string with leading zeros
                string crWithLeadingZeros = crForDirectoryName.PadLeft(2, '0');

                // Create directory name with leading zeros
                string directory = _SaveTargetLocation + $"{monster.Fields.Type}" + "/" + $"{crWithLeadingZeros}";

                // Create filename with CR and leading zeros
                string filename = $"{monster.Fields.Name}.md";
                SaveMarkdownToFile(monster.Fields.MarkdownString, filename, directory);
            }
            
        }

        private static string GenerateActionsMarkdownString(string json, string subject)
        {
            try
            {
                List<TurnAction> actions = JsonConvert.DeserializeObject<List<TurnAction>>(json);
                if (actions == null || actions.Count == 0)
                {
                    return string.Empty; // Return empty string if no actions are defined.
                }

                StringBuilder markdown = new StringBuilder();
                markdown.AppendLine($"### {subject}");
                markdown.AppendLine(" --- ");

                foreach (var action in actions)
                {
                    markdown.AppendLine($"- **{action.Name}**: {action.Description}");
                    markdown.AppendLine("");
                }

                return markdown.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while converting JSON to Markdown: {ex.Message}");
                return string.Empty;
            }

        }

        static HashSet<Monster> DeserializeMonstersFromFile(string fileLocation)
        {
            try
            {
                string json = File.ReadAllText(fileLocation);
                HashSet<Monster> monsterHashSet = JsonConvert.DeserializeObject<HashSet<Monster>>(json);

                return monsterHashSet;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deserializing: {ex.Message}");
                return null;
            }
        }

        public static string FillMonsterTemplate(FieldValues monster)
        {
            string template = $@"## {monster.Name}
*({monster.Size}) ({monster.Type}) *

**Armor Class:** {monster.ArmorClass}
**Hit Points:** {monster.HP} ({monster.HitDice})
**Speed:** {monster.SpeedJson}

**Initiative:** {monster.Dexterity}
**Proficiency Bonus:**
**Challenge:** {monster.ChallengeRating}

**Languages:** {monster.Languages}

{monster.SpecialAbilitiesMarkdownString}

| Stats | Modifier | Stat | Save
| ---- | ---- | ---- | ---- |
| Strength | {monster.Strength} | {CalculateModifier(monster.Strength)} | {monster.StrengthSave?.ToString() ?? "-"} |
| Dexterity | {monster.Dexterity} | {CalculateModifier(monster.Dexterity)} | {monster.DexteritySave?.ToString() ?? "-"} |
| Constitution | {monster.Constitution} | {CalculateModifier(monster.Constitution)} | {monster.ConstitutionSave?.ToString() ?? "-"} |
| Intelligence | {monster.Intelligence} | {CalculateModifier(monster.Intelligence)} | {monster.IntelligenceSave?.ToString() ?? "-"} |
| Wisdom | {monster.Wisdom} | {CalculateModifier(monster.Wisdom)} | {monster.WisdomSave?.ToString() ?? "-"} |
| Charisma | {monster.Charisma} | {CalculateModifier(monster.Charisma)} | {monster.CharismaSave?.ToString() ?? "-"} |

";

            if (!string.IsNullOrEmpty(monster.ActionsMarkdownString))
            {
                template += monster.ActionsMarkdownString;
            }
            if (!string.IsNullOrEmpty(monster.BonusActionMarkdownString))
            {
                template += monster.BonusActionMarkdownString;
            }
            if (!string.IsNullOrEmpty(monster.ReactionsMarkdownString))
            {
                template += monster.ReactionsMarkdownString;
            }
            if (!string.IsNullOrEmpty(monster.LegendaryActionsMarkdownString))
            {
                template += monster.LegendaryActionsMarkdownString;
            }

        return template;
        }

        public static int CalculateModifier(int abilityScore)
        {
            return (abilityScore - 10) / 2;
        }

        public static void SaveMarkdownToFile(string content, string filename, string directory)
        {
            try
            {
                // Create the directory if it doesn't exist
                Directory.CreateDirectory(directory);

                // Combine the directory path and filename to get the full file path
                string filePath = Path.Combine(directory, filename);

                // Write the content to the file
                File.WriteAllText(filePath, content);

                Console.WriteLine($"Markdown saved to {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving the Markdown to a file: {ex.Message}");
            }
        }
    }
}