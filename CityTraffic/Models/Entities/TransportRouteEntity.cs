﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CityTraffic.Models.Entities
{
    public class TransportRouteEntity : INotifyPropertyChanged
    {
        public string RouteId { get; set; }

        public string RouteNumber { get; set; } = string.Empty;

        public int RouteTypeId { get; set; }

        public string Title { get; set; } = string.Empty;

        public bool IsFavorite { get; set { field = value; OnPropertyChanged(); } }

        public List<StoppointEntity> Stoppoints { get; set; } = [];


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null) => 
            PropertyChanged?.Invoke(this, new(propertyName));


        public override bool Equals(object obj)
        {
            if (obj == null || obj is not TransportRouteEntity other) return false;

            return RouteId == other.RouteId &&
                   RouteNumber == other.RouteNumber &&
                   RouteTypeId == other.RouteTypeId &&
                   Title == other.Title;
        }

        public override int GetHashCode() => RouteId.GetHashCode();
    }
}
