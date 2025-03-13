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
using Newtonsoft.Json;
using Microsoft.Win32;
using System.IO;
using System.Windows;

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
            PerformCmd_OpenJson();
            //try
            //{
            //    //ProjectList.Clear();

            //    //var pList = new List<clProject>();

            //    //pList = await mProject.LoadAdd();

            //    //foreach (clProject project in pList)
            //    //{
            //    //    ProjectList.Add(project);
            //    //}    
            //}
            //catch
            //{

            //}
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

        private ActionCommand cmd_MakeJson;

        public ICommand Cmd_MakeJson
        {
            get
            {
                if (cmd_MakeJson == null)
                {
                    cmd_MakeJson = new ActionCommand(PerformCmd_MakeJson);
                }

                return cmd_MakeJson;
            }
        }

        private void PerformCmd_MakeJson()
        {
            string Json = JsonConvert.SerializeObject(ProjectList);
            //Properties.Settings.Default.ProjectListJson = Json;
            //Properties.Settings.Default.Save();


            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Danh sách dự án (*.txt)|*.txt";
            saveFileDialog.Title = "Lưu danh sách dự án";
            saveFileDialog.FileName = "ProjectList.txt";

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, Json);
                MessageBox.Show($"File đã lưu tại: {saveFileDialog.FileName}");
            }    

        }

        private ActionCommand cmd_OpenJson;

        public ICommand Cmd_OpenJson
        {
            get
            {
                if (cmd_OpenJson == null)
                {
                    cmd_OpenJson = new ActionCommand(PerformCmd_OpenJson);
                }

                return cmd_OpenJson;
            }
        }

        private async void PerformCmd_OpenJson()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt",
                Title = "Chọn file để đọc"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ProjectList.Clear();
                var pList = await mProject.LoadFromTextFile(openFileDialog.FileName);
                foreach (var p in pList)
                {
                    ProjectList.Add(p);
                }
            }    

        }
    }
}
