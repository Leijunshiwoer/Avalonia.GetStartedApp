using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.ViewModels.UserControls
{
    public class PaginationViewModel : BindableBase
    {
        private int _currentPage = 1;
        private int _totalPages = 10;
        public int CurrentPage
        {
            get => _currentPage;
            set => SetProperty(ref _currentPage, value, () => RaisePropertyChanged(nameof(CurrentPageDisplay)));
        }
        public int TotalPages
        {
            get => _totalPages;
            set => SetProperty(ref _totalPages, value, () => RaisePropertyChanged(nameof(CurrentPageDisplay)));
        }
        public string CurrentPageDisplay => $"第 {CurrentPage} / {TotalPages} 页";
        public DelegateCommand PreviousPageCommand { get; }
        public DelegateCommand NextPageCommand { get; }
        public bool CanGoPrevious => CurrentPage > 1;
        public bool CanGoNext => CurrentPage < TotalPages;

        public PaginationViewModel()
        {
            PreviousPageCommand = new DelegateCommand(() => { if (CanGoPrevious) CurrentPage--; }, () => CanGoPrevious)
                .ObservesProperty(() => CurrentPage);
            NextPageCommand = new DelegateCommand(() => { if (CanGoNext) CurrentPage++; }, () => CanGoNext)
                .ObservesProperty(() => CurrentPage);
        }
    }
}
