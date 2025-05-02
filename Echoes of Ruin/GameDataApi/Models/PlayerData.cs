using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace GameDataApi.Models {
    [BsonIgnoreExtraElements]
    public class PlayerData {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string PlayerId {get; set; } = null!;
        public int Currency { get; set; }
        public List<string> OwnedSkins { get; set; } = new List<string>();
        public string CurrentSkin { get; set; } = "Default";
        public int BallCount { get; set; }
        public int BiscuitCount { get; set; }
        public int BrushCount { get; set; }
        [BsonElement("lastupdated")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime LastUpdated { get; set; }
    }
}