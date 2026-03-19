using Microsoft.AspNetCore.SignalR;

namespace TP2_BlackJack.Models
{
    public class Joueur
    {
        public Main Main { get; private set; } = new();
        public int Argent { get; set; } = 1000; // 1000 as default
        public int Mise { get; private set; }

        public void NouvelleMain(int mise)
        {
            Mise = mise;
            Main = new Main();
        }
    }
}

// Model: Joueur
// Actor
