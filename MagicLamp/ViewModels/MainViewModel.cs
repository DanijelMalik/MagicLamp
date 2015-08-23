using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using MagicLamp.Models;

namespace MagicLamp.ViewModels
{
    internal class MainViewModel : ViewModel
    {
        private SolutionModel _selectedTemplate;
        private string _company;
        private string _solutionName;
        private string _searchText;
        private ICollectionView _templatesView;
        private ICommand _createCommand;

        public MainViewModel()
        {
            Templates = new ObservableCollection<SolutionModel>
            {
                SolutionTemplates.SswDataOnion,
                SolutionTemplates.MvcAngular,
                SolutionTemplates.HotTowel,
            };
            TemplatesView = CollectionViewSource.GetDefaultView(Templates);
        }

        public ObservableCollection<SolutionModel> Templates { get; private set; }

        public ICollectionView TemplatesView
        {
            get { return _templatesView; }
            private set
            {
                _templatesView = value;
                OnPropertyChanged();
            }
        }

        public string SolutionName
        {
            get { return _solutionName; }
            set
            {
                _solutionName = value;
                OnPropertyChanged();
            }
        }

        public string Company
        {
            get { return _company; }
            set
            {
                _company = value;
                OnPropertyChanged();
            }
        }

        public SolutionModel SelectedTemplate
        {
            get { return _selectedTemplate; }
            set
            {
                _selectedTemplate = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged();

                if (String.IsNullOrEmpty(_searchText))
                {
                    TemplatesView.Filter = null;
                }
                else
                {
                    TemplatesView.Filter = o =>
                    {
                        var template = (SolutionModel) o;

                        if (template == null)
                        {
                            return false;
                        }

                        var searchText = _searchText.ToLower();
                        if (template.Name.ToLower().Contains(searchText))
                        {
                            return true;
                        }

                        if (template.Description.ToLower().Contains(searchText))
                        {
                            return true;
                        }

                        return template.Tags.Any(x => x.ToLower().Contains(searchText));
                    };
                }
            }
        }

        public ICommand CreateCommand
        {
            get { return _createCommand ?? (_createCommand = new RelayCommand(Create, CanCreate)); }
        }

        private bool CanCreate()
        {
            return !String.IsNullOrEmpty(Company) && !String.IsNullOrEmpty(SolutionName) && SelectedTemplate != null;
        }

        private void Create()
        {
            // Do nothing
        }

        public void LoadFromFile(string fileName)
        {
            var model = SolutionModel.FromFile(fileName);
            if (model != null)
            {
                Templates.Add(model);
            }
        }
    }
}