using CE.DbConnectionHelper.Models;
using ReactiveUI;
using ReactiveUI.Winforms;
using System;
using System.Collections.Generic;

namespace CE.DbConnectionHelper.ViewModels
{
    public class SecurityGroupSelectionViewModel : ReactiveObject
    {
        #region properties
        bool _hasSelection = true;
        public bool HasSelection
        {
            get => _hasSelection;
            set => this.RaiseAndSetIfChanged(ref _hasSelection, value);
        }

        SecurityGroupModel _selectedItem;
        public SecurityGroupModel SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }

        ReactiveBindingList<SecurityGroupModel> _securityGroups;
        public ReactiveBindingList<SecurityGroupModel> SecurityGroups
        {
            get => _securityGroups;
            set
            {
                this.RaiseAndSetIfChanged(ref _securityGroups, value);
                _hasSelection = (_securityGroups?.Count > 0);
            }
        }
        #endregion

        #region ctor
        public SecurityGroupSelectionViewModel()
            : this(new List<SecurityGroupModel>() { new SecurityGroupModel() { Id = 0, Name = "default", Description = "No list loaded" } })
        {
        }

        public SecurityGroupSelectionViewModel(IEnumerable<SecurityGroupModel> groups)
        {
            _securityGroups = new ReactiveBindingList<SecurityGroupModel>(groups);
        }
        #endregion
    }
}
