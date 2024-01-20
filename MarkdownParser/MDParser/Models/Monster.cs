using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace MDParser.models
{
    public class FieldValues
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "desc")]
        public string Desc { get; set; }

        [JsonProperty(PropertyName = "document")]
        public int Document { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty(PropertyName = "page_no")]
        public int? PageNo { get; set; }

        [JsonProperty(PropertyName = "size")]
        public string Size { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "subtype")]
        public string SubType { get; set; }

        [JsonProperty(PropertyName = "group")]
        public object Group { get; set; }

        [JsonProperty(PropertyName = "alignment")]
        public string Alignment { get; set; }

        [JsonProperty(PropertyName = "armor_class")]
        public int ArmorClass { get; set; }

        [JsonProperty(PropertyName = "armor_desc")]
        public string ArmorDesc { get; set; }

        [JsonProperty(PropertyName = "hit_points")]
        public int HP { get; set; }

        [JsonProperty(PropertyName = "hit_dice")]
        public string HitDice { get; set; }

        [JsonProperty(PropertyName = "speed_json")]
        public string SpeedJson { get; set; }

        [JsonProperty(PropertyName = "environments_json")]
        public string EnvironmentsJson { get; set; }

        [JsonProperty(PropertyName = "strength")]
        public int Strength { get; set; }

        [JsonProperty(PropertyName = "dexterity")]
        public int Dexterity { get; set; }

        [JsonProperty(PropertyName = "constitution")]
        public int Constitution { get; set; }

        [JsonProperty(PropertyName = "intelligence")]
        public int Intelligence { get; set; }

        [JsonProperty(PropertyName = "wisdom")]
        public int Wisdom { get; set; }

        [JsonProperty(PropertyName = "charisma")]
        public int Charisma { get; set; }

        [JsonProperty(PropertyName = "strength_save")]
        public object StrengthSave { get; set; }

        [JsonProperty(PropertyName = "dexterity_save")]
        public int? DexteritySave { get; set; }

        [JsonProperty(PropertyName = "constitution_save")]
        public int? ConstitutionSave { get; set; }

        [JsonProperty(PropertyName = "intelligence_save")]
        public int? IntelligenceSave { get; set; }

        [JsonProperty(PropertyName = "wisdom_save")]
        public int? WisdomSave { get; set; }

        [JsonProperty(PropertyName = "charisma_save")]
        public object CharismaSave { get; set; }

        [JsonProperty(PropertyName = "perception")]
        public object Perception { get; set; }

        [JsonProperty(PropertyName = "skills_json")]
        public string SkillsJson { get; set; }

        [JsonProperty(PropertyName = "damage_vulnerabilities")]
        public string DamageVulnerabilities { get; set; }

        [JsonProperty(PropertyName = "damage_resistances")]
        public string DamageResistances { get; set; }

        [JsonProperty(PropertyName = "damage_immunities")]
        public string DamageImmunities { get; set; }

        [JsonProperty(PropertyName = "condition_immunities")]
        public string ConditionImmunities { get; set; }

        [JsonProperty(PropertyName = "senses")]
        public string Senses { get; set; }

        [JsonProperty(PropertyName = "languages")]
        public string Languages { get; set; }

        [JsonProperty(PropertyName = "challenge_rating")]
        public string ChallengeRating { get; set; }

        [JsonProperty(PropertyName = "cr")]
        public double CR { get; set; }

        [JsonProperty(PropertyName = "actions_json")]
        public string ActionsJson { get; set; }

        [JsonProperty(PropertyName = "bonus_actions_json")]
        public string BonusActionsJson { get; set; }

        [JsonProperty(PropertyName = "special_abilities_json")]
        public string SpecialAbilitiesJson { get; set; }

        [JsonProperty(PropertyName = "reactions_json")]
        public string ReactionsJson { get; set; }

        [JsonProperty(PropertyName = "legendary_desc")]
        public string LegendaryDesc { get; set; }

        [JsonProperty(PropertyName = "legendary_actions_json")]
        public string LegendaryActionsJson { get; set; }

        [JsonProperty(PropertyName = "spells_json")]
        public string SpellsJson { get; set; }

        [JsonProperty(PropertyName = "route")]
        public string Route { get; set; }

        [JsonProperty(PropertyName = "img_main")]
        public object ImgMain { get; set; }

        public string ActionsMarkdownString { get; set; }
        public string BonusActionMarkdownString { get; set; }
        public string ReactionsMarkdownString { get; set; }
        public string SpecialAbilitiesMarkdownString { get; set; }
        public string LegendaryActionsMarkdownString { get; set; }

        public string MarkdownString { get; set; }

    }

    public class Monster
    {
        public string model { get; set; }
        public string pk { get; set; }
        [JsonProperty(PropertyName = "fields")]
        public FieldValues Fields { get; set; }
    }

    public class TurnAction
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "desc")]
        public string Description { get; set; }
    }
}
