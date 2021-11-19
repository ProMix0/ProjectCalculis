﻿using MainLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client;
using System.Windows.Input;

namespace ClientShell
{
    public class ViewModel : IViewModel
    {

        public ViewModel(IClientModel model)
        {
            this.model = model;
            ExecuteCommand = new(model);
        }

        private IClientModel model;

        public IReadOnlyCollection<IWorkMetadata> Metadatas => model.WorksMetas;

        public ExecuteCommandClass ExecuteCommand { get; }

        public class ExecuteCommandClass : ICommand
        {
            private IClientModel model;

            public ExecuteCommandClass(IClientModel model)
            {
                this.model = model;
            }

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public async void Execute(object parameter)
            {
                await model.ExecuteAsync(parameter as IWorkMetadata);
            }
        }
    }
}
