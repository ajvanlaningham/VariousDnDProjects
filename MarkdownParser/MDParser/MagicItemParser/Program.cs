using Microsoft.Extensions.Configuration;
using Models;
using Newtonsoft.Json;

namespace MagicItemParser
{
    internal class Program
    {
        private static string _SaveTargetLocation = "";

        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
            _SaveTargetLocation = config["FileLocations:SaveTargetLocation"];
            string wotcSrdTargetLocation = config["FileLocations:WotcSrdTargetLocation"];
            string a5eTargetLocation = config["FileLocations:A5eTargetLocation"];
            string taldorieMagicTargetLocation = config["FileLocations:TaldorieMagicTargetLocation"];
            string tombOfHeroesTargetLocation = config["FileLocations:TombOfHeroesTargetLocation"];
            string vaultOfMagicTargetLocation = config["FileLocations:VaultOfMagicTargetLocation"];

            string[] targetLocations = new[]
            {
                wotcSrdTargetLocation,
                a5eTargetLocation,
                taldorieMagicTargetLocation,
                tombOfHeroesTargetLocation,
                vaultOfMagicTargetLocation
            };

            HashSet<MagicItem> magicItems = new HashSet<MagicItem>();
            foreach (string targetLocation in targetLocations)
            {
                HashSet<MagicItem> tempItems = DeserializeMonstersFromFile(targetLocation);
                magicItems.UnionWith(tempItems);
            }

            foreach (MagicItem item in magicItems)
            {
                if (item.Fields.RequiresAttunement == "")
                {
                    item.Fields.RequiresAttunement = "Does not require attunement";
                }

                item.Fields.Value = GetItemValue(item.Fields.Rarity);

                item.MarkdownString = FillMagicItemTemplate(item.Fields);

                string folderExtension = GetItemFolderExtension(item.Fields.Type);
                string directory = _SaveTargetLocation + folderExtension;

                string fileName = $"{item.Fields.Name}.md";

                SaveMarkdownToFile(item.MarkdownString, fileName, directory);
            }

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

        private static string FillMagicItemTemplate(MagicItemFields item)
        {
             string template = $@"## {item.Name}
*({item.Type}) {item.Rarity}) *

*{item.RequiresAttunement} 

#### Description:
---
{item.Desc}



* Est. value range: {item.Value}";

            return template;
        }

        static HashSet<MagicItem> DeserializeMonstersFromFile(string fileLocation)
        {
            try
            {
                string json = File.ReadAllText(fileLocation);
                HashSet<MagicItem> itemsHashSet = JsonConvert.DeserializeObject<HashSet<MagicItem>>(json);

                return itemsHashSet;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deserializing: {ex.Message}");
                return null;
            }
        }

        private static string GetItemFolderExtension(string type)
        {
            if (itemTypeToFolderMapping.ContainsKey(type))
            {
                return itemTypeToFolderMapping[type];
            }
            else
            {
                // Default folder for unknown types
                return "Misc";
            }
        }

        static string GetItemValue(string rarity)
        {
            if (rarityToValueMapping.TryGetValue(rarity, out string valueRange))
            {
                return valueRange;
            }
            else
            {
                return "Unknown";
            }
        }

        private static Dictionary<string, string> itemTypeToFolderMapping = new Dictionary<string, string>
        {
            // Armor
            { "Armor (medium or heavy)", "Armor/Medium&Heavy" },
            { "Armor (scale mail)", "Armor/Medium&Heavy" },
            { "Armor (chain shirt)", "Armor/Medium&Heavy" },
            { "Armor (shield)", "Armor/Shield" },
            { "Armor (plate)", "Armor/Medium&Heavy" },
            { "Armor (light)", "Armor/Light" },
            { "Armor (studded leather)", "Armor/Light" },

            // Weapons
            
                //Edged
                { "Weapon (any axe)", "Weapons/Axes" },
                { "Weapon (dagger)", "Weapons/Daggers" },
                { "Dagger", "Weapons/Daggers" },
                { "Weapon (any sword)", "Weapons/Swords" },
                { "Weapon (scimitar)", "Weapons/Swords" },
                { "Weapon (longsword)", "Weapons/Swords" },
                { "Weapon (any sword that deals slashing damage)", "Weapons/Swords" },
                { "Weapon (any axe or sword)", "Weapons/Other" },
            
                { "Weapon (maul)", "Weapons/Blunt" },
                { "Weapon (warhammer)", "Weapons/Blunt" },
                { "Weapon (mace)", "Weapons/Blunt" },

                //Missile Weapons
                { "Weapon (arrow)", "Weapons/Missile/Ammunition" },
                { "Ammunition", "Weapons/Missile/Ammunition" },
                { "Weapon (shortbow or longbow)", "Weapons/Missile" },
                { "Weapon (any ammunition)", "Weapons/Missile/Ammunition" },
                { "Weapon (longbow)", "Weapons/Missile" },
                { "Weapon (javelin)", "Weapons/Thrown" },
                { "Weapon (trident)", "Weapons/Thrown" },

                { "Weapon (any)", "Weapons/Other" },


            // Scrolls
            { "Scroll", "Scrolls" },

            // Jewelry
            { "Ring", "Jewelry/Rings" },

            // Charged Items
            { "Rod", "Charged/Rods" },
            { "Wand", "Charged/Wands" },
            { "Staff", "Charged/Staffs" },

            // Potions
            { "Potion", "Potions" },

            // Miscellaneous
            { "Wondrous item", "Misc" },
            { "Other", "Misc" },
        };

        private static Dictionary<string, string> rarityToValueMapping = new Dictionary<string, string>
        {
            { "common", "50-100 gp" },
            { "Common", "50-100 gp" },

            { "uncommon", "101-500 gp" },
            { "Uncommon", "101-500 gp" },

            { "rare", "501-5,000 gp" },
            { "Rare", "501-5,000 gp" },

            { "very rare", "5,001-50,000 gp" },
            { "Very Rare", "5,001-50,000 gp" },

            { "legendary", "50,001+ gp" },
            { "Legendary", "50,001+ gp" },

            { "artifact", "Priceless" },
            { "Artifact", "Priceless" },

            { "common (+1), uncommon (+2), rare (+3)", "101-500 gp, 101-5,000 gp, 501-5,000 gp" },
            { "uncommon (+1), rare (+2), or very rare (+3)", "101-5,000 gp or 5,001-50,000 gp" },
            { "uncommon, rare, very rare", "101-5,000 gp, 501-5,000 gp, 5,001-50,000 gp" },
            { "uncommon (silver), very rare (gold)", "101-500 gp, 5,001-50,000 gp" },
            { "uncommon (least), rare (lesser), very rare (greater)", "101-500 gp, 501-5,000 gp, 5,001-50,000 gp" },

            { "rare (silver or brass), very rare (bronze) or legendary (iron)", "5,001-50,000 gp or 50,001+ gp" },

            { "very rare or legendary", "5,001-50,000 gp or 50,001+ gp" },

            { "varies", "Varies" },
            { "rarity varies", "Varies" },
            { "rarity by figurine", "Varies" }
        };
    }

}