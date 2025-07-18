using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Playing_Card
{
    public class Card
    {
        public Suit suit;
        public Rank rank;

        public Card(Suit suit, Rank rank)
        {
            this.suit = suit;
            this.rank = rank;
        }
    }
}