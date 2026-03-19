namespace TP2_BlackJack.Models
{
    public class Carte
    {
        public string Sorte { get; set; } = ""; // Card type // C:clubs, D:diamonds, H:hearts, S:spades
        public string Rang { get; set; } = ""; // Card position (label, not a numeric value) // '2', '3', etc.
        public int Valeur { get; set; } // Card value at a specific position // '2' = 2, etc. '10' = 10, 'V' = 10, 'D' = 10, 'R' = 10, 'A' = 11 or 1 (depends on the logic of the game)
        public string CheminImage => $"/images/cartes/{Rang}_{Sorte}.png"; // Card image path // For ex: A_C.png => Ace of clubs
    }
}

// Model: Carte
// It represents details about the cards
