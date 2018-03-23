using Shared.Messages.Communication;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface IGameActions
    {
         
        
        
        Task<Data> Move(Move move);

        Task<Data> Discover(Discover discover);

        Task<Data> PickUpPiece(PickUpPiece pickUpPiece);

        Task<Data> TestPiece(TestPiece testPiece);

        Task<Data> PlacePiece(PlacePiece placePiece);

        Task<PlayerMessage> Communicate(AuthorizeKnowledgeExchange authorizeKnowledgeExchange);
    }
}
