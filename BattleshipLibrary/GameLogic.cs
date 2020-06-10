using BattleshipLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipLibrary
{
    public static class GameLogic
    {
        public static void InitializeGrid(PlayerModel model)
        {
            List<string> letters = new List<string>()
            {

                "A",
                "B",
                "C",
                "D",
                "E"

            };

            List<int> numbers = new List<int>()
            {

                1,
                2,
                3,
                4,  
                5

            };

            foreach ( string letter in letters)
            {
                foreach (int  number in numbers)
                {
                    AddGridSpot(model, letter, number);
                }
            }
        }

        public static bool PlayerStillActive(PlayerModel player)
        {
            bool isActivePlayer = false;

            foreach (var ship in player.ShipLocations)
            {
                if (ship.Status != GridSpotStatus.Sunk)
                {
                    isActivePlayer = true;
                }
            }
            return isActivePlayer;
        }

        private static void AddGridSpot(PlayerModel model, string letter, int number)
        {
            GridSpotModel spot = new GridSpotModel
            {
                SpotLetter = letter,
                SpotNumber = number,
                Status = GridSpotStatus.Empty
            };
            model.ShotGrid.Add(spot);
        }

        public static bool PlaceShip(PlayerModel model, string location)
        {
            bool output = false;

            (string row, int column) = SplitShotIntoRowAndColumn(location);

            bool isValidLocation = ValidateGridLocation(model, row, column);
            bool isSpotOpen = ValidateShipLocation(model, row, column);

            if (isValidLocation && isSpotOpen)
            {
                model.ShipLocations.Add(new GridSpotModel
                {
                    SpotLetter = row.ToUpper(),
                    SpotNumber = column,
                    Status = GridSpotStatus.Ship
                });
                output = true;
            }

            return output;
        }

        public static bool ValidateShot(PlayerModel model, string row, int column)
        {
            bool isValidShot = false;
            foreach (var spotGrid in model.ShotGrid)
            {
                if ( spotGrid.SpotLetter== row.ToUpper() && spotGrid.SpotNumber == column )
                {
                    if (spotGrid.Status == GridSpotStatus.Empty)
                    {
                        isValidShot = true;
                    }
                }
            }
            return isValidShot;
        }

        private static bool ValidateShipLocation(PlayerModel model, string row, int column)
        {
            bool isValidLocation = true;
            foreach (var ship in model.ShipLocations)
            {
                if (ship.SpotLetter == row.ToUpper() && ship.SpotNumber == column)
                {
                    isValidLocation = false;
                }
            }
            return isValidLocation;
        }

        private static bool ValidateGridLocation(PlayerModel model, string row, int column)
        {
            bool isValidSpot = false;
            foreach (var spot in model.ShotGrid)
            {
                if (spot.SpotLetter == row.ToUpper() && spot.SpotNumber == column)
                {
                    isValidSpot = true;
                }
            }
            return isValidSpot;
        }

        public static int GetShotCount(PlayerModel player)
        {
            int shotNumber = 0;
            foreach (var spot in player.ShotGrid)
            {
                if (spot.Status != GridSpotStatus.Empty)
                {
                    ++shotNumber;
                }
            }
            return shotNumber;
        }

        public static (string row, int column) SplitShotIntoRowAndColumn(string shot)
        {
            string row = "";
            int column = 0;
            if (shot.Length != 2)
            {
                throw new ArgumentException("that was not a valid location", "location");
            }
            char[] shotArray = shot.ToArray();
            row = shotArray[0].ToString();
            column = int.Parse(shotArray[1].ToString());

            return (row, column);
        }

        public static bool IdentifyShotResult(PlayerModel player, string row, int column)
        {
            bool isAHit = false;
            foreach (var ship in player.ShipLocations)
            {
                if (ship.SpotLetter == row.ToUpper() && ship.SpotNumber == column)
                {
                    isAHit = true;
                    ship.Status = GridSpotStatus.Sunk;
                }
            }
            return isAHit;
        }

        public static void MarkShotResult(PlayerModel player, string row, int column, bool isAHit)
        {
            foreach (var gridSpot in player.ShotGrid)
            {
                if (gridSpot.SpotLetter == row.ToUpper() && gridSpot.SpotNumber == column)
                {
                    if (isAHit)
                    {
                        gridSpot.Status = GridSpotStatus.Hit;
                    }
                    else
                    {
                        gridSpot.Status = GridSpotStatus.Miss;
                    }
                }
            }
        }
    }
}
