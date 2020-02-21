using System.Threading.Tasks;
using ARMDesktopUI.Library.Models;

namespace ARMDesktopUI.Library.Api
{
    public interface ISaleEndpoint
    {
        Task PostSale(SaleModel sale);
    }
}