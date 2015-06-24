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
    public class ViewModelBase : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChangedEvent(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Error
        {
            get { return string.Empty; }
        }

        public string this[string propertyName]
        {
            get
            {
                return OnValidate(propertyName);
            }
        }

        
        protected virtual string OnValidate(string propertyName)
        {
            return null;
        }
    }
}
