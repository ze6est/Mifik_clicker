using System;

[Serializable]
public class PlayerProgress
{
    public CardProgress[] Cards;

    public long Points;
    public CardProgress CardProgress;

    public PlayerProgress()
    {        
        Cards = new CardProgress[Enum.GetNames(typeof(MifiksName)).Length];

        for (int i = 0; i < Cards.Length; i++)
        {
            CardProgress = new CardProgress();

            Cards[i] = new CardProgress();
        }
    }
}