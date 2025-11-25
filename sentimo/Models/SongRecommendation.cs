namespace sentimo.Models
{
    public class SongRecommendation
    {
        public int SongID { get; set; }
        public string Song { get; set; } = "";
        public string Artist { get; set; } = "";
        public int Year { get; set; }
        public string Decade { get; set; } = "";
        public string DominantEmotion { get; set; } = "";
        public double PolarityMean { get; set; }
    }
}
