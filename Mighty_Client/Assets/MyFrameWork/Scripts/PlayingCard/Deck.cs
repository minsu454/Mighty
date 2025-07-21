using Common.ListEx;
using System;
using System.Collections.Generic;

namespace Playing_Card
{
    public class Deck
    {
        private readonly List<Card> _useCardList;           //사용 중인 덱 리스트
        private readonly List<Card> _throwCardList;         //사용한 덱 리스트

        public Deck(Deck_Setting setting)
        {
            _throwCardList = new List<Card>();

            for (int suit = 1; suit < Enum.GetValues(typeof(Suit)).Length; suit++)
            {
                for (int rank = 1; rank < Enum.GetValues(typeof(Rank)).Length; rank++)
                {
                    _throwCardList.Add(new Card(OnReturnCard, (Suit)suit, (Rank)rank));
                }
            }

            for (int i = 0; i < setting.jokerCount; i++)
            {
                _throwCardList.Add(new Card(OnReturnCard, Suit.None, Rank.Joker));
            }
        }

        /// <summary>
        /// 덱 셔플 함수
        /// </summary>
        public void Shuffle()
        {
            _throwCardList.Shuffle();

            while (_throwCardList.Count != 0)
            {
                _useCardList.Add(_throwCardList.TryRemoveAt(0));
            }
        }

        /// <summary>
        /// 카드 받는 함수
        /// </summary>
        public Card GetCard()
        {
            return _useCardList.TryRemoveAt(0);
        }

        /// <summary>
        /// 카드 덱으로 돌릴때 사용 이벤트 함수
        /// </summary>
        private void OnReturnCard(Card card)
        {
            _throwCardList.Add(card);
        }
    }
}

