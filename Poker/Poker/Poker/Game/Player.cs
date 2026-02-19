namespace Poker.Game;

public class Player
{
    public string name { get; set; }
    public List<Card> cards { get; set; }
    public int tableBalance { get; set; }

    public Player() { }

    public Player(string name, int buyIn)
    {
        this.name = name;
        cards = new List<Card>(2);
        tableBalance = buyIn;
    }

    public void SeeCards()
    {
        Console.WriteLine(name);
        Console.WriteLine(cards[0].Value+" "+cards[0].Suit+"\n"+cards[1].Value+" "+cards[1].Suit);
    }
    
}