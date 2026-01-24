using CSharpFunctionalExtensions;
using FamalyPlacment.Models;

namespace FamalyPlacment.Abstractions
{
    public interface IPlacementService
    {
        Result Place(FurnitureType selectedFurnitureType, int count);
    }
}
