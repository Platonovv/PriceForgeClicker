using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace PriceForgeClicker.Shop
{
    public interface IShopService
    {
        UniTask<IReadOnlyList<ShopOffer>> GetOffersAsync(CancellationToken cancellationToken);
        UniTask<PurchaseResult> PurchaseAsync(ShopOffer offer, CancellationToken cancellationToken);
    }
}
