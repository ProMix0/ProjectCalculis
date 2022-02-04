using MainLibrary.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClientShell
{
    public abstract class AbstractViewModel : IViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand ExecuteCommand { get;protected init; }
        public RelayCommand LoopCommand { get; protected init; }
        public virtual IReadOnlyCollection<IWorkMetadata> Metadatas { get; protected set; }

        private bool canExecuteWork = true;
        public bool CanExecuteWork
        {
            get
            {
                return canExecuteWork;
            }
            protected set
            {
                canExecuteWork = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}