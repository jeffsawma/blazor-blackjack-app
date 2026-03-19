namespace TP2_BlackJack.Models
{
    public class Paquet
    {
        private List<Carte> cartes = new(); // List of cards with a player or dealer at a time

        private static readonly string[] Sortes = { "H", "D", "C", "S" }; // Four types

        private static readonly string[] Rangs = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "V", "D", "R", "A" }; // 2,3,4,5,6,7,8,9,10,J,Q,K,A

        public Paquet() // Brand-new 52-card deck in every game 
        {
            foreach (var sorte in Sortes) // Nested loop
                foreach (var rang in Rangs)
                {
                    cartes.Add(new Carte // Each loop iteration creates one new Carte object and adds it to the deck list
                    {
                        Sorte = sorte,
                        Rang = rang,
                        Valeur = rang switch // Switch expression
                        {
                            "A" => 11, // A = 11
                            "R" or "D" or "V" => 10,  // J, Q, K 
                            _ => int.Parse(rang) // Anything else, convert the string to int because rang values are labels
                        }
                    });
                }
        }

        public void Brasser() // Shuffling the cards
        {
            cartes = cartes.OrderBy(c => Guid.NewGuid()).ToList();
        }

        public Carte Piger() // Drawing a card from the deck
        {
            var carte = cartes.First(); // Drawing the card from the top of the deck
            cartes.RemoveAt(0); // Once a card ia drawn in blacjack, it cannot appear again so it's removed from the deck
            return carte; // ServiceBlackJack receives that card and adds it to a player's hand
        }
    }
}

// Model: Paquet
// Actor
