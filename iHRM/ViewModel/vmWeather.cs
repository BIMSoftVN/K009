using iHRM.Classes;
using K009Libs.MVVM;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using iHRM.Model;

namespace iHRM.ViewModel
{
    public class vmWeather : PropertyChangedBase
    {
        private ObservableCollection<clProject> _ProjectList = new ObservableCollection<clProject>();
        public ObservableCollection<clProject> ProjectList
        {
            get
            {
                return _ProjectList;
            }
            set
            {
                _ProjectList = value;
                OnPropertyChanged();
            }
        }

        private clProject _SelectedProject = new clProject();
        public clProject SelectedProject
        {
            get
            {
                return _SelectedProject;
            }
            set
            {
                _SelectedProject = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<clWeatherInfo> _WeatherList = new ObservableCollection<clWeatherInfo>();
        public ObservableCollection<clWeatherInfo> WeatherList
        {
            get
            {
                return _WeatherList;
            }
            set
            {
                _WeatherList = value;
                OnPropertyChanged();
            }
        }



        private ActionCommand cmd_LoadAll;
        public ICommand Cmd_LoadAll
        {
            get
            {
                if (cmd_LoadAll == null)
                {
                    cmd_LoadAll = new ActionCommand(PerformCmd_LoadAll);
                }

                return cmd_LoadAll;
            }
        }

        private async void PerformCmd_LoadAll()
        {
            try
            {
                ProjectList.Clear();

                var pList = new List<clProject>();

                pList = await mProject.LoadAdd();

                foreach (clProject project in pList)
                {
                    ProjectList.Add(project);
                }    
            }
            catch
            {

            }
        }

        private ActionCommand cmd_Run;

        public ICommand Cmd_Run
        {
            get
            {
                if (cmd_Run == null)
                {
                    cmd_Run = new ActionCommand(PerformCmd_Run);
                }

                return cmd_Run;
            }
        }

        private async void PerformCmd_Run()
        {
            try
            {
                WeatherList.Clear();

                var wList = await mWeather.LoadInfoByLocation(SelectedProject.ViDo.Value, SelectedProject.KinhDo.Value);

                foreach (clWeatherInfo weather in wList)
                {
                    WeatherList.Add(weather);
                }
            }
            catch
            {

            }
        }
    }
}
