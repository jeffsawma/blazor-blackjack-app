using TP2_BlackJack.Models;
using System.Text.Json;

namespace TP2_BlackJack.Services
{
    public class ServiceBlackJack
    {
        // Core Game Entities
        public Joueur Joueur { get; private set; } = new();
        public Croupier Croupier { get; private set; } = new();
        public Paquet Paquet { get; private set; } = new();

        // Historique
        public List<Historique> Historiques { get; private set; } = new(); // New history list every single game not round

        // Game State
        public bool MancheActive { get; private set; } // Tells UI if a round is currently running
        public bool MancheTerminee { get; private set; } // Tells UI if a round is finished and the results have been calculated for both player and dealer

        public int NumeroRonde { get; private set; } // Needed for Historique export // Starts with the index 1 at the start of every round in a game

        // Constructor
        public ServiceBlackJack()
        {
            NumeroRonde = 1;
        }

        // Game Actions
        public void NouvelleManche(int mise) // Button 'Nouvelle manche'
        {
            if (MancheActive) // If round is active, stop
                return;

            if (Joueur.Argent < mise) // If player's solde is lower than 'mise', stop
                return;

            Joueur.NouvelleMain(mise); // reseting player with 'mise' at the beginning of every round in a new game
            Croupier.NouvelleMain(); // reseting dealer at the beginning of every game

            Paquet = new Paquet(); // New 52-card deck to start with at every game
            Paquet.Brasser(); // Shuffle the deck of cards at the start of the round

            // Deal 2 cards for each players // Shuffle the cards-deck before dealing // Simulating a real blackjack game
            Joueur.Main.AjouterCarte(Paquet.Piger()); // One card to the player
            Croupier.Main.AjouterCarte(Paquet.Piger()); // Another to the dealer
            Joueur.Main.AjouterCarte(Paquet.Piger()); 
            Croupier.Main.AjouterCarte(Paquet.Piger());

            // Activating the round
            MancheActive = true;
            MancheTerminee = false;
        }

        public void Tirer() // Button 'Carte'
        {

            if (!MancheActive || MancheTerminee) // If round is not active or round is finished, stop
                return;

            var carte = Paquet.Piger(); // Drawing a card from the cards-deck

            Joueur.Main.AjouterCarte(carte); // Adding a new card to the player's list of cards during a round

            if (Joueur.Main.Score > 21) // Checking after every card draw if the score of the player is currently higher than 21
            {
                MancheTerminee = true; // If yes, round is finished
                MancheActive = false; // If false, round is not active
                Joueur.Argent -= Joueur.Mise; // We deduct the 'mise' from the 'money' or 'solde' if player lose
                Historiques.Add(new Historique // We add to the list we created in this file aka 'Historiques' // Historique is an instance of the class 'Historique'
                {
                    NumeroRonde = NumeroRonde,
                    JouerA = DateTime.Now,
                    Resultat = "Trop - Défaite",
                    Mise = Joueur.Mise,
                    ScoreJoueur = Joueur.Main.Score,
                    ScoreCroupier = Croupier.Main.Score,
                    ArgentApres = Joueur.Argent // 'Argent' is deducted and updated here
                });

                NumeroRonde++; // We increment the index of rounds

            }
        }

        public void Rester() // Button 'Rester'
        {
            if (!MancheActive || MancheTerminee) // If round is not active or round is finished, stop
                return;

            while (Croupier.Main.Score < 17)
            {
                var carte = Paquet.Piger(); // Drawing a new card from the cards-deck if score < 17 
                Croupier.Main.AjouterCarte(carte); // Adding a new card to the Croupier's list of cards if score < 17
            }

            string resultat;

            if (Croupier.Main.Score > 21) // If dealer busts
            {
                resultat = "Victoire"; // Player wins
                Joueur.Argent += Joueur.Mise; // We add the amount of mise to the 'solde' of player
            }
            else if (Joueur.Main.Score > Croupier.Main.Score)
            {
                resultat = "Victoire"; // Player wins 
                Joueur.Argent += Joueur.Mise;
            }
            else if (Joueur.Main.Score < Croupier.Main.Score)
            {
                resultat = "Défaite"; // Player lose
                Joueur.Argent -= Joueur.Mise;
            }
            else
            {
                resultat = "Egalité"; // Equality
            }

            Historiques.Add(new Historique
            {
                NumeroRonde = NumeroRonde,
                JouerA = DateTime.Now,
                Resultat = resultat,
                Mise = Joueur.Mise,
                ScoreJoueur = Joueur.Main.Score,
                ScoreCroupier = Croupier.Main.Score,
                ArgentApres = Joueur.Argent
            });

            NumeroRonde++; // We increment the index of rounds

            MancheTerminee = true; // Round is finished
            MancheActive = false; // Round is no longer active
        }

        public void Quitter() // Button 'Quitter la partie' 
        {
            Joueur = new Joueur(); // Reset the player
            Croupier = new Croupier(); // Reset the dealer
            Paquet = new Paquet(); // Reset the 52-card deck when the player leaves
            Historiques.Clear(); // Reset the list 'Historiques'
            NumeroRonde = 1; // The number of rounds will obviously reset to 1

            MancheActive = false;
            MancheTerminee = false;
        }

        // Exporting
        public byte[] ExporterHistoriqueJSON() // Button 'Exporter l'historique (JSON)' // The browser expects binary data even tho JSON is text
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true // Without indentation we will get: [{"NumeroRonde":1, "JouerA":"2026-01-01T00:00:00", etc.}]
            };

            string json = JsonSerializer.Serialize(Historiques, options); // Converting List<Historique> into JSON text

            return System.Text.Encoding.UTF8.GetBytes(json); // We now have json as string but browers download files as binary, so string -> byte[] // UTF8 is the standard encoding for JSON
        }

        // Importing
        public void ImporterHistoriqueJSON(string jsonContent)
        {
            var historiquesImportes = JsonSerializer.Deserialize<List<Historique>>(jsonContent); // Converting back JSON text to List<Historique> to be able to display the content in a table

            if (historiquesImportes != null) // If there is content in the file
            {
                Historiques = historiquesImportes; // Assign the content variable to the list 'Historiques'

                NumeroRonde = Historiques.Count + 1; // Incrementation the rounds by 1 depending on how many rounds are saved in the content file

                MancheActive = false;
                MancheTerminee = false; // 'Manche' should always be true immediately after a real round finishes so in this case 'false'
            }
        }
    }
}

// This class must define the public methods it must expose:
//
// NouvelleManche(int mise)
// Tirer()
// Rester()
// Quitter()
// ExporterHistoriqueJSON()
// ImporterHistoriqueJSON()
