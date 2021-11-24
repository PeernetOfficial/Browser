using System.Runtime.CompilerServices;

namespace Peernet.Browser.Models.Presentation.Home
{
    public class FileFormatFilterModel : CustomFilterModel<FileFormat>
    {
        public FileFormatFilterModel() : base("File format")
        {
        }

        public override void UnselectAll()
        {
            base.UnselectAll();
            Set(FileFormat.None);
            SelectedItemIndex = 0;
        }
    }
}