using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Example.IRemoteFitness
{
	[DataContract]
    public class City
    {
        public City(string name, double latitude, double longitude)
        {
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
        }
		[DataMember]
        public string Name { set; get; }
		[DataMember]
		public double Latitude { get; set; }
		[DataMember]
		public double Longitude { get; set; }

        /// <summary> 
        /// Returns the distance in Km between this City and the specified location. 
        ///<summary> 
        public double GetDistanceFromPosition(double latitude, double longitude)
        {
            var R = 6371;
            // radius of the earth in km 
            var dLat = DegreesToRadians(latitude - Latitude);
            var dLon = DegreesToRadians(longitude - Longitude);
            var a = System.Math.Sin(dLat/2)*System.Math.Sin(dLat/2) +
                    System.Math.Cos(DegreesToRadians(Latitude))*System.Math.Cos(DegreesToRadians(latitude))*
                    System.Math.Sin(dLon/2)*System.Math.Sin(dLon/2);
            var c = 2*System.Math.Atan2(System.Math.Sqrt(a), System.Math.Sqrt(1 - a));
            var d = R*c;
            // distance in km 
            return d;
        }

        private static double DegreesToRadians(double deg)
        {
            return deg*(System.Math.PI/180);
        }

		public override string ToString ()
		{
			return Name;
		}
    }
}
