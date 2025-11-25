using sentimo.Models;
using System.Collections.Generic;

namespace sentimo.Models
{
    public class MoodFilterViewModel
    {
        public string? Emotion { get; set; }
        public string? Decade { get; set; }
        public int? Year { get; set; }
        public List<SongRecommendation> Results { get; set; } = new();
    }
}
