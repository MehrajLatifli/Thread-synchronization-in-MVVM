using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ATM__thread_synchronization_.Models
{
    public class Card : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnpropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public string _CardCode { get; set; }

        public string CardCode
        {
            get { return _CardCode; }
            set { _CardCode = value; OnpropertyChanged(); }
        }

        public string _UserName { get; set; }

        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; OnpropertyChanged(); }
        }

        public string _Balance { get; set; }

        public string Balance
        {
            get { return _Balance; }
            set { _Balance = value; OnpropertyChanged(); }
        }


        public string _DecliningBalance { get; set; }

        public string DecliningBalance
        {
            get { return _DecliningBalance; }
            set { _DecliningBalance = value; OnpropertyChanged(); }
        }


        public Card()
        {

        }
    }
}
