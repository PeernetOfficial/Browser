using System.ComponentModel;

namespace Peernet.Browser.Application.ViewModels
{
    public class GenericViewModelBase<TParameter> : ViewModelBase
        where TParameter : class
    {
        private TParameter parameter;

        public TParameter Parameter
        {
            get => parameter;
            set
            {
                parameter = value;
                OnPropertyChanged(nameof(Parameter));
            }
        }
    }
}
