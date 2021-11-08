﻿using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Models.Presentation.Home;

namespace Peernet.Browser.Application.ViewModels
{
    public class FiltersViewModel : MvxViewModel<FiltersModel>, IModal
    {
        private readonly IApplicationManager applicationManager;
        private FiltersModel filters;

        public FiltersViewModel(IApplicationManager applicationManager)
        {
            this.applicationManager = applicationManager;
        }

        public FiltersModel Filters
        {
            get => filters;
            set => SetProperty(ref filters, value);
        }

        public override void Prepare(FiltersModel p)
        {
            Filters = p;
            p.CloseAction += Hide;
        }

        private void Hide(bool withApply)
        {
            Filters.CloseAction -= Hide;
            GlobalContext.IsMainWindowActive = true;
            applicationManager.CloseModal();
        }
    }
}