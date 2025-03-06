using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using K009Libs.MVVM;

namespace iHRM.Classes
{
    public class clChatInfo : PropertyChangedBase
    {
        private string _chatUser = null;
        public string ChatUser 
        { 
            get 
            { 
                return _chatUser; 
            } 
            set 
            { 
                _chatUser = value;
                OnPropertyChanged();
            } 
        }

        private string _Message = null;
        public string Message
        {
            get
            {
                return _Message;
            }
            set
            {
                _Message = value;
                OnPropertyChanged();
            }
        }
    }
}
