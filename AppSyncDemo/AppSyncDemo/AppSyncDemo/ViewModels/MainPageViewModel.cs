using AppSyncDemo.Models;
using AppSyncDemo.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppSyncDemo.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public ReactiveProperty<string> Name { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<SampleModel> SampleModel { get; } = new ReactiveProperty<SampleModel>(new SampleModel());
        public AsyncReactiveCommand GetSampleCommand { get; } = new AsyncReactiveCommand();

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";

            GetSampleCommand.Subscribe(async () =>
            {
                this.SampleModel.Value = await AppSyncService.Instance.GetSampleAsync(Name.Value);
            });
        }
    }
}
