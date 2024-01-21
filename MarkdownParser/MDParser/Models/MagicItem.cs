using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class MagicItem
    {
        [JsonProperty(PropertyName = "model")]
        public string Model { get; set; }
        [JsonProperty(PropertyName = "pk")]
        public string PK { get; set; }
        [JsonProperty(PropertyName = "fields")]
        public MagicItemFields Fields { get; set; }

        public string MarkdownString { get; set; }
    }

    public class MagicItemFields
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
        public object PageNum { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "rarity")]
        public string Rarity { get; set; }

        [JsonProperty(PropertyName = "requires_attunement")]
        public string RequiresAttunement { get; set; }

        [JsonProperty(PropertyName = "route")]
        public string Route { get; set; }

        public string Value { get; set; }
    }
}
