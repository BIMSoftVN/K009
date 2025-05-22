﻿using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RevitAddIns.Helper;

namespace RevitAddIns.Classes
{
    public class clFamilyCocInfo : PropertyChangedBase
    {
        private ElementId _Id = null;
        public ElementId Id
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

        private string _Family = null;
        public string Family
        {
            get
            {
                return _Family;
            }
            set
            {
                _Family = value;
                OnPropertyChanged();
            }
        }

        private string _Type = null;
        public string Type
        {
            get
            {
                return _Type;
            }
            set
            {
                _Type = value;
                OnPropertyChanged();
            }
        }

        private double? _Diameter = null;
        public double? Diameter
        {
            get
            {
                return _Diameter;
            }
            set
            {
                _Diameter = value;
                OnPropertyChanged();
            }
        }
    }
}
