using System.ComponentModel;

namespace Peernet.Browser.Application.ViewModels.Parameters
{
    public class TerminalInstanceParameter : INotifyPropertyChanged
    {
        private string commandLineInput;
        private string commandLineOutput;

        public event PropertyChangedEventHandler PropertyChanged;

        public string CommandLineOutput
        {
            get => commandLineOutput;
            set
            {
                commandLineOutput = value;
                PropertyChanged?.Invoke(this, new(nameof(CommandLineOutput)));
            }
        }

        public string CommandLineInput
        {
            get => commandLineInput;
            set
            {
                commandLineInput = value;
                PropertyChanged?.Invoke(this, new(nameof(CommandLineInput)));
            }
        }
    }
}
