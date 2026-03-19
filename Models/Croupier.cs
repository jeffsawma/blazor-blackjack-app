namespace TP2_BlackJack.Models
{
    public class Croupier
    {
        public Main Main { get; set; } = new(); // When object is created, the dealer automatically starts with an empty hand

        public void NouvelleMain() // This method resets the dealer's hand so old hand is discarded
        {
            Main = new Main(); // Creates a brand-new Main object (New empty hand)
        }
    }
}

// Model: Croupier
// Actor


