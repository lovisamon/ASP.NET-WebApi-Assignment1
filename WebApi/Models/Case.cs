using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace WebApi.Models
{
    public partial class Case
    {
        public int Id { get; set; }
        public int HandlerId { get; set; }
        public string Client { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Edited { get; set; }
        public string CurrentStatus { get; set; }

        [JsonIgnore]
        public virtual User Handler { get; set; }
    }
}
