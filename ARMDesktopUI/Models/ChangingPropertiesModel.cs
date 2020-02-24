using System.ComponentModel;
using System.Runtime.CompilerServices;
using ARMDesktopUI.Annotations;

namespace ARMDesktopUI.Models
{
    public abstract class ChangingPropertiesModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void CallPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}