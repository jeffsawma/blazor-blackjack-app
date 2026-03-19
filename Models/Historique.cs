using TP2_BlackJack.Models;

namespace TP2_BlackJack.Models
{
    public class Historique
    {
        public int NumeroRonde { get; set; }
        public DateTime JouerA { get; set; }
        public int ScoreJoueur { get; set; }
        public int ScoreCroupier { get; set; }
        public int Mise { get; set; }
        public string Resultat { get; set; } = "";
        public int ArgentApres { get; set; }
    }
}

// Model: Historique
// Storing all rounds in memory




