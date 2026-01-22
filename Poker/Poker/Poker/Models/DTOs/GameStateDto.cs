namespace Poker.Models.DTOs
{
    public class GameStateDto
    {
        public Game.Table.GameStage Stage { get; set; }
        public List<Game.Card>? TableCards { get; set; }
        public int Pot { get; set; }
        public Game.Player? CurrentPlayer { get; set; }
        public Dictionary<Game.Player, int>? HandWinners { get; set; }
    }
}
