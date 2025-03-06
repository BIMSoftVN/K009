using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using K009Libs.MVVM;
using System.Windows.Input;
using System.Collections.ObjectModel;
using iHRM.Classes;
using iHRM.Model;

namespace iHRM.ViewModel
{
    public class vmGoogleDich : PropertyChangedBase
    {
        private string _InputText = null;
        public string InputText
        {
            get
            {
                return _InputText;
            }
            set 
            { 
                _InputText = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<clChatInfo> _ChatList = new ObservableCollection<clChatInfo>();
        public ObservableCollection<clChatInfo> ChatList
        {
            get
            {
                return _ChatList;
            }
            set
            {
                _ChatList = value;
                OnPropertyChanged();
            }
        }


        private ActionCommand cmd_Send;

        public ICommand Cmd_Send
        {
            get
            {
                if (cmd_Send == null)
                {
                    cmd_Send = new ActionCommand(PerformCmd_Send);
                }

                return cmd_Send;
            }
        }

        private async void PerformCmd_Send()
        {
            var chat = new clChatInfo
            {
                ChatUser = "User",
                Message = InputText
            };
            ChatList.Add(chat);

            var chat2 = new clChatInfo
            {
                ChatUser = "Google",
                Message = await mGoogle.DichAsync(InputText)
            };
            ChatList.Add(chat2);

            InputText = string.Empty;
        }

    }
}
