using CSharpFunctionalExtensions;
using Task10.Models;

namespace Task10.Abstractions
{
    public interface IPlacementService
    {
        Result Place(TreeType selectedTreeType, int count);
    }
}
