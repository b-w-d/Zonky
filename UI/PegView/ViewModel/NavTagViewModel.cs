using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;


namespace PegView.ViewModel
{
    public class NavTagViewModel : ViewModelBase
    {


        public ObservableCollection<string> Tags
        {
            get;
            set;
        }
    }
}
