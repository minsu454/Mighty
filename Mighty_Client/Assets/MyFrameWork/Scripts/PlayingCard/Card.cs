using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Playing_Card
{
    public class Card
    {
        private Action<Card> returnEvent; 
        public Suit Suit;
        public Rank Rank;

        public Card(Action<Card> returnEvent, Suit suit, Rank rank)
        {
            this.returnEvent = returnEvent;
            Suit = suit;
            Rank = rank;
        }

        public void Return()
        {
            returnEvent.Invoke(this);
        }
    }
}