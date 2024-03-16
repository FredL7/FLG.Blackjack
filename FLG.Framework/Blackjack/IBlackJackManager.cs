using System.Collections.Generic;

using FLG.Cs.IDatamodel;

namespace Blackjack {
    public interface IBlackJackManager : IServiceInstance {
        public List<ICard> PlayerHand { get; }
        public List<ICard> DealerHand { get; }

        // public string PrintHand(List<ICard> hand);
        public int HandTotal(List<ICard> hand);
        public void DrawPlayer();
        public void DealerAIPlay();
        public void Reset();
    }
}
