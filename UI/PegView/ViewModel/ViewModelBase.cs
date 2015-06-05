using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PegView.ViewModel
{
    using System.ComponentModel;

    /// <summary>
    /// Base for all ViewModel classes. 
    /// This handles the INotifyPropertyChanged interface, allowing updates to be passed to the view.
    /// 
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChangedEvent(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
