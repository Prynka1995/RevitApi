using Autodesk.Revit.DB;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Task8.Abstractions;
using Task8.Models;

namespace Task8.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly iSelectionnService _selectionService;
        private readonly IGeometryService _geometryService;
        public MainWindowViewModel(iSelectionnService selectionService, IGeometryService geometryService)
        {
            CaclOpening = new RelayCommand(OnCaclOpeningExecute);
            _selectionService = selectionService;
            _geometryService = geometryService;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private OpeningInfo _openingInfo;
        public OpeningInfo OpeningInfo
        {
            get => _openingInfo;
            set
            {
                _openingInfo = value;
                OnPropertyChanged();
            }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }
        private double _widthLimit = 100;


        public double WidthLimit
        {
            get => _widthLimit;
            set
            {
                _widthLimit = value;
                OnPropertyChanged();
            }
        }
        public ICommand CaclOpening { get; }
        private void OnCaclOpeningExecute(object parameter)
        {
            Wall wall = _selectionService.pickWall();
            if (wall == null)
            {
                return;
            }
            
            OpeningInfo = _geometryService.GetWallInfo(wall, WidthLimit);

        }
    }
}
