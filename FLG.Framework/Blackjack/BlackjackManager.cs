using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Godot;

using FLG.Cs.Cards;
using FLG.Cs.IDatamodel;
using FLG.Cs.ServiceLocator;

namespace Blackjack {
    public class BlackjackManager : IBlackJackManager {
        public IDeck _deck = new StandardDeck52(2);

        public List<ICard> PlayerHand { get => _deck.GetHand(0); }
        public List<ICard> DealerHand { get => _deck.GetHand(1); }

        #region IServiceInstance
        public bool IsProxy() => false;
        public void OnServiceRegisteredFail() { Locator.Instance.Get<ILogManager>().Error("UI Manager Failed to register"); }
        public void OnServiceRegistered()
        {
            Locator.Instance.Get<ILogManager>().Debug("Blackjack Manager Registered");
            Init();
        }
        #endregion IServiceInstance

        private void Init()
        {
            _deck = new StandardDeck52(2);
            _deck.Shuffle();
            SetInitialHand();
        }

        public void DealerAIPlay()
        {
            var dealerHand = _deck.GetHand(1);
            while (HandTotal(dealerHand) < 17)
            {
                _deck.DrawTop(1);
            }
        }

        private void SetInitialHand()
        {
            _deck.DrawMultiple(2, 0);
            _deck.DrawMultiple(2, 1);
        }

        public void DrawPlayer()
        {
            _deck.DrawTop(0);
        }

        public void Reset()
        {
            _deck.Reset();
            _deck.Shuffle();
            SetInitialHand();
        }

        public int HandTotal(List<ICard> hand)
        {
            int total = 0;
            int aces = 0;
            foreach (var card in hand)
            {
                if (card.Number == 1)
                {
                    ++aces;
                }
                else if (card.Number > 10)
                {
                    total += 10;
                }
                else
                {
                    total += card.Number;
                }
            }

            for (int i = 0; i < aces; ++i)
            {
                if (total + 11 + (aces - 1) <= 21)
                {
                    total += 11;
                }
                else
                {
                    total += 1;
                }
            }

            return total;
        }
    }
}
