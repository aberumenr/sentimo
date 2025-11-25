using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using sentimo.Models;
using System.Collections.Generic;
using System.Data;

namespace sentimo.Controllers
{
    public class MoodController : Controller
    {
        private readonly IConfiguration _config;

        public MoodController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult Index(string? emotion, string? decade)
        {
            var vm = new MoodFilterViewModel
            {
                Emotion = emotion,
                Decade = decade,
                Results = new List<SongRecommendation>()
            };

            if (!string.IsNullOrEmpty(emotion))
            {
                vm.Results = GetSongsFromDb(emotion, decade);
            }

            return View(vm);
        }

        private List<SongRecommendation> GetSongsFromDb(string emotion, string? decade)
        {
            var results = new List<SongRecommendation>();
            string connString = _config.GetConnectionString("SongsDb");

            using (var conn = new SqlConnection(connString))
            using (var cmd = new SqlCommand("dbo.GetSongsByEmotionDecade", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emotion", emotion);
                cmd.Parameters.AddWithValue("@decade", (object?)decade ?? DBNull.Value);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(new SongRecommendation
                        {
                            SongID = reader.GetInt32(reader.GetOrdinal("SongID")),
                            Song = reader.GetString(reader.GetOrdinal("Song")),
                            Artist = reader.GetString(reader.GetOrdinal("Artist")),
                            Year = reader.GetInt32(reader.GetOrdinal("Year")),
                            Decade = reader.GetString(reader.GetOrdinal("Decade")),
                            DominantEmotion = reader.GetString(reader.GetOrdinal("dominant_emotion")),
                            PolarityMean = reader.GetDouble(reader.GetOrdinal("polarity_mean"))
                        });
                    }
                }
            }

            return results;
        }
    }
}
