using K009Libs.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHRM.Classes
{
    public class clUser : PropertyChangedBase
    {
        private int? _Id = null;
        public int? Id
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
                OnPropertyChanged();
            }
        }

        private string _Name = null;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                OnPropertyChanged();
            }
        }

        private string _PhoneNumber = null;
        public string PhoneNumber
        {
            get
            {
                return _PhoneNumber;
            }
            set
            {
                _PhoneNumber = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _DateOfBirth = null;
        public DateTime? DateOfBirth
        {
            get
            {
                return _DateOfBirth;
            }
            set
            {
                _DateOfBirth = value;
                OnPropertyChanged();
            }
        }

        private string _Address = null;
        public string Address
        {
            get
            {
                return _Address;
            }
            set
            {
                _Address = value;
                OnPropertyChanged();
            }
        }

    }
}
