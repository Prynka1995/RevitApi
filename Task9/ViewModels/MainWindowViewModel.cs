using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Task9.Abstractions;

namespace Task9.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly ISelectionService _selectionService;
        private readonly ISectionService _sectionService;
        private readonly RevitTask _revitTask;
        private string _sectionNamesInput = "Разрез1, Разрез2, Разрез3";
        private double _widthOffsetMm = 100;
        private double _depthOffsetMm = 100;
        private double _heightOffsetMm = 100;

        public MainWindowViewModel(
            ISelectionService selectionService,
            ISectionService sectionService,
            RevitTask revitTask
            )
        {
            CreateSectionCommand = new AsyncRelayCommand(OnCreateSectionCommandExecute);
            _selectionService = selectionService;
            _sectionService = sectionService;
            _revitTask = revitTask;
        }

        public string SectionNames
        {
            get => _sectionNamesInput;
            set => SetProperty(ref _sectionNamesInput, value);
        }

        public double WidthOffsetMm
        {
            get => _widthOffsetMm;
            set => SetProperty(ref _widthOffsetMm, value);
        }

        public double DepthOffsetMm
        {
            get => _depthOffsetMm;
            set => SetProperty(ref _depthOffsetMm, value);
        }

        public double HeightOffsetMm
        {
            get => _heightOffsetMm;
            set => SetProperty(ref _heightOffsetMm, value);
        }

        public AsyncRelayCommand CreateSectionCommand { get; }

        private async Task OnCreateSectionCommandExecute()
        {
            FamilyInstance familyInstance = _selectionService.PickFamilyInstance();
            if (familyInstance == null)
            {
                return;
            }

            // Преобразуем в ObservableCollection
            var sectionNamesList = SectionNames
                .Split(',')
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrEmpty(s))
                .Take(3)
                .ToList();


            var sectionNamesCollection = new ObservableCollection<string>(sectionNamesList);

            bool isCreated = await _revitTask.Run<bool>(app =>
            _sectionService.CreateSection(
                familyInstance,
                WidthOffsetMm,
                DepthOffsetMm,
                HeightOffsetMm,
                sectionNamesCollection
                ));

            if (!isCreated)
            {
                TaskDialog.Show("Ошибка", "Что-то пошло не так");
            }
            else
            {
                TaskDialog.Show("Успех", "Все пошло так");
            }
        }
    }
}
