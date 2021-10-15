using ISOSL_GestContact.Models.Entities;
using ISOSL_GestContact.Models.Repositories;
using ISOSL_GestContact.ViewModels.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Patterns.Mediator;
using Tools.Patterns.Mvvm.Commands;
using Tools.Patterns.Mvvm.ViewModels;

namespace ISOSL_GestContact.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IPersonneRepository _personneRepository;
        private readonly IMessenger<PersonneViewModelMessage> _messenger;
        private readonly IMessenger<OpenWindowMessage<PersonneViewModel>> _openWindowMessenger;

        private ObservableCollection<PersonneViewModel> _items;
        private string _nom;
        private string _prenom;
        private ICommand _addCommand;
        private ICommand _cancelCommand;

        private string _pays;

        public ObservableCollection<PersonneViewModel> Items
        {
            get
            {
                return _items ??= LoadItems();
            }
        }

        private ObservableCollection<PersonneViewModel> LoadItems()
        {
            return new ObservableCollection<PersonneViewModel>(_personneRepository.Get().Select(CreatePersonneViewModel));
        }

        private PersonneViewModel CreatePersonneViewModel(Personne entity)
        {
            PersonneViewModel viewModel = Locator.GetResource<PersonneViewModel>();
            viewModel.Entity = entity;
            return viewModel;
        }

        public string Nom
        {
            get
            {
                return _nom;
            }

            set
            {
                Set(ref _nom, value);
            }
        }

        public string Prenom
        {
            get
            {
                return _prenom;
            }

            set
            {
                Set(ref _prenom, value);
            }
        }

        public ICommand AddCommand
        {
            get
            {
                return _addCommand ??= new DelegateCommand(Add, CanAdd);
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ??= new DelegateCommand(Cancel);
            }
        }

        public string Pays
        {
            get
            {
                return _pays;
            }

            set
            {
                Set(ref _pays, value);
            }
        }

        public MainViewModel(IMessenger<PersonneViewModelMessage> messenger, IPersonneRepository personneRepository, IMessenger<OpenWindowMessage<PersonneViewModel>> openWindowMessenger)
        {
            _openWindowMessenger = openWindowMessenger;
            _messenger = messenger;
            _messenger.Register(OnPersonneViewModelAction);
            _personneRepository = personneRepository;
        }

        private void OnPersonneViewModelAction(PersonneViewModelMessage message)
        {
            switch (message.Action)
            {
                case Actions.Insert:
                    break;
                case Actions.Update:
                    _openWindowMessenger.Send(new OpenWindowMessage<PersonneViewModel>(message.ViewModelInstance));
                    break;
                case Actions.Delete:
                    Items.Remove(message.ViewModelInstance);
                    break;
                default:
                    break;
            }
        }

        public void Add()
        {
            Personne personne = new Personne() { Nom = Nom, Prenom = Prenom, Pays = Pays };
            personne.Id = _personneRepository.Insert(personne);
            Items.Add(CreatePersonneViewModel(personne));
            Reset();
        }

        public bool CanAdd()
        {
            return !string.IsNullOrWhiteSpace(Nom) &&
                !string.IsNullOrWhiteSpace(Prenom) &&
                Pays != null;
        }

        public void Cancel()
        {
            Reset();
        }

        private void Reset()
        {
            Nom = null;
            Prenom = null;
            Pays = null;
        }
    }
}
