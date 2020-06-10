using BattleshipLibrary;
using BattleshipLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Battleship
{
    class Program
    {
        static void Main(string[] args)
        {
            WelcomeMesage();

            PlayerModel activePlayer = CreatePlayer("Player 1");
            PlayerModel opponent = CreatePlayer("Player 2");
            PlayerModel winner = null;

            do
            {
               
                DisplayShotGrid(activePlayer);
               
                RecordPlayerShot(activePlayer, opponent);
               
                bool doesGamecontinue = GameLogic.PlayerStillActive(opponent);

                if (doesGamecontinue == true)
                {
                    //swap players
                    (activePlayer, opponent) = (opponent, activePlayer);
                }
                else
                {
                    winner = activePlayer;
                }
            } while (winner == null);

            IdentifyWinner(winner);

            Console.ReadLine();
        }

        private static void IdentifyWinner(PlayerModel winner)
        {
            Console.WriteLine($"Congradulations to {winner.PlayersName} for winning this game");
            Console.WriteLine($"Winner took {GameLogic.GetShotCount(winner)} shots to win this game");
        }

        private static void RecordPlayerShot(PlayerModel activePlayer, PlayerModel opponent)
        {
            bool isValidShot = false;
            string row = "";
            int column = 0;
            do
            {
                string shot = AskForShot(activePlayer);
                try
                {
                    (row, column) = GameLogic.SplitShotIntoRowAndColumn(shot);
                    isValidShot = GameLogic.ValidateShot(activePlayer, row, column);
                }
                catch ( Exception )
                {

                   // Console.WriteLine("This was not a valid Shot.Please try again");
                    isValidShot = false;
                }
                if (isValidShot == false)
                {
                    Console.WriteLine("Invalid shot. Please try again");
                }
            } while (isValidShot == false);

            bool isAHit = GameLogic.IdentifyShotResult(opponent, row, column);
            
            GameLogic.MarkShotResult(activePlayer, row, column, isAHit);
            DysplayShotResut(row, column, isAHit);
        }

        private static void DysplayShotResut(string row, int column, bool isAHit)
        {
            if (isAHit)
            {
                Console.WriteLine($"{row}{column} is a Hit!");

            }
            else
            {
                Console.WriteLine($"{row}{column} is a miss");
            }
            Console.WriteLine();
        }

        private static string AskForShot(PlayerModel player)
        {

            Console.Write($"{player.PlayersName} Please enter your shot selection :");
            string output = Console.ReadLine();
            return output;
        }

        private static void DisplayShotGrid(PlayerModel activePlayer)
        {
            string currentRow = activePlayer.ShotGrid[0].SpotLetter;

            foreach (var GridSpot  in activePlayer.ShotGrid)
            {
                if (GridSpot.SpotLetter != currentRow )
                {
                    Console.WriteLine();
                    currentRow = GridSpot.SpotLetter;   
                }
                if (GridSpot.Status == GridSpotStatus.Empty)
                {
                    Console.Write($"{GridSpot.SpotLetter}{GridSpot.SpotNumber} ");
                }
                else if (GridSpot.Status == GridSpotStatus.Hit)
                {
                    Console.Write("X  ");
                }
                else if (GridSpot.Status == GridSpotStatus.Miss)
                {
                    Console.Write("O  ");
                }
                else
                {
                    Console.Write("?  ");
                }
            }
            Console.WriteLine();
        }

        private static void WelcomeMesage()
        {
            Console.WriteLine("Welcome to the BottleShipLite!");
            Console.WriteLine("Created by Diana Boariu");
            Console.WriteLine();
        }

        private static PlayerModel CreatePlayer(string playtitle)
        { 
          
            PlayerModel output = new PlayerModel();

            Console.WriteLine($"Player information for {playtitle}:");

            //Ask players'name
            output.PlayersName = AskUsersName();

            //Load up the shotgrid
            GameLogic.InitializeGrid(output);

            //Ask for 5 ship placement

            PlaceShips(output);

            //Clear
            Console.Clear();

            return output;
        }

        private static void PlaceShips(PlayerModel model)
        {
            //Ask player to name a location (B2 model  not B and 2 )
            //split into letter and number
            // check if positin is valid
            // add position to ship location

            do
            {
                Console.Write($"Where do you want to place the ship number {model.ShipLocations.Count +1}: ");
                string location = Console.ReadLine();

                bool isVAlidLocation = false;
                try
                {
                    isVAlidLocation = GameLogic.PlaceShip(model, location);
                }
                catch ( Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                if (isVAlidLocation == false)
                {
                    Console.WriteLine("That was not a valid location. Please try again." );
                }
            } while (model.ShipLocations.Count < 5);
           
        }

        private static string AskUsersName()
        {
            Console.Write($"What is your name: ");
            string output = Console.ReadLine();
            return output;
        }
    }
}
