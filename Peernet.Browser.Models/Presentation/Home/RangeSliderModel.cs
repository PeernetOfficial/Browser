﻿using MvvmCross.ViewModels;

namespace Peernet.Browser.Models.Presentation.Home
{
    public class RangeSliderModel : MvxNotifyPropertyChanged
    {
        private int currentMax;
        private int currentMin;
        private int max;
        private int min;

        public int CurrentMax
        {
            get => currentMax;
            set => SetProperty(ref currentMax, value);
        }

        public int CurrentMin
        {
            get => currentMin;
            set => SetProperty(ref currentMin, value);
        }

        public int Max
        {
            get => max;
            set => SetProperty(ref max, value);
        }

        public int Min
        {
            get => min;
            set => SetProperty(ref min, value);
        }
    }
}