using ISOSL_GestContact.Models.Entities;
using ISOSL_GestContact.Models.Repositories;
using ISOSL_GestContact.ViewModels.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Patterns.Mediator;
using Tools.Patterns.Mvvm.Commands;
using Tools.Patterns.Mvvm.ViewModels;

namespace ISOSL_GestContact.ViewModels
{
    public class PersonneViewModel : ViewModelBase
    {
        private readonly IPersonneRepository _personneRepository;
        private readonly IMessenger<PersonneViewModelMessage> _messenger;
        private Personne _entity;
        private ICommand _deleteCommand;
        private ICommand _updateCommand;
        private ICommand _saveCommand;
        private ICommand _cancelCommand;
        private string _nom;
        private string _prenom;
        private string _pays;

        public PersonneViewModel(IPersonneRepository personneRepository, IMessenger<PersonneViewModelMessage> messenger)
        {
            _personneRepository = personneRepository;
            _messenger = messenger;            
        }

        public int Id { get { return _entity.Id; } }

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

        public ICommand DeleteCommand
        {
            get
            {
                return _deleteCommand ??= new DelegateCommand(Delete);
            }
        }

        public ICommand UpdateCommand
        {
            get
            {
                return _updateCommand ??= new DelegateCommand(() => _messenger.Send(new PersonneViewModelMessage(this, Actions.Update)));
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ??= new DelegateCommand(Save, CanSave);
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ??= new DelegateCommand(Cancel);
            }
        }

        public Personne Entity
        {
            set
            {
                if (_entity is not null)
                    throw new InvalidOperationException();

                _entity = value;
                Nom = value.Nom;
                Prenom = value.Prenom;
                Pays = value.Pays;
            }
        }

        private void Cancel()
        {
            Nom = _entity.Nom;
            Prenom = _entity.Prenom;
            Pays = _entity.Pays;
        }

        private void Save()
        {
            _entity.Nom = Nom;
            _entity.Prenom = Prenom;
            _entity.Pays = Pays;
            _personneRepository.Update(_entity);
            RaisePropertyChanged("");            
        }

        private bool CanSave()
        {
            return _entity.Nom != Nom ||
            _entity.Prenom != Prenom ||
            _entity.Pays != Pays;
        }

        private void Delete()
        {
            _personneRepository.Delete(Id);
            _entity = null;
            _messenger.Send(new PersonneViewModelMessage(this, Actions.Delete));
        }
    }
}
