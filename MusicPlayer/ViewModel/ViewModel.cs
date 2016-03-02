using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.ViewModel
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        protected ViewModel()
        {

        }
   
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) { PropertyChanged(this, e); }
        }
        protected virtual void OnPropertyChanged(string property)
        {
            
            OnPropertyChanged(new PropertyChangedEventArgs(property));
        }
    }
}
