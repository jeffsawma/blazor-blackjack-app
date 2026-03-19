namespace TP2_BlackJack.Models
{
    public class Main
    {
        public List<Carte> Cartes { get; set; } = new(); // A hand contains multiple cards // When drawing a card, we add it here

        public int Score // It calculates the score every time we access it and does not permanently store it
        {
            get // Getting the right score
            {
                int score = Cartes.Sum(c => c.Valeur);
                int las = Cartes.Count(c => c.Rang == "A");

                while (score > 21 && las > 0) // If cards score higher than 21 and number of Aces is higher than 0
                {
                    score -= 10;
                    las--; // Number of Ace // Or las = las - 1 
                }
                return score; // Returning the score each time
            }
        }

        public void AjouterCarte(Carte carte) => Cartes.Add(carte); // Method to add a card to the list of cards
    }
}

// Model: Main
// It represents a hand of cards + it calculates score correctly

