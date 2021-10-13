using System.Linq;
namespace RealEstateAgency{
	public class Utils{
		public static int LevenshteinDistance(string s1, string s2){
			int l1 = s1.Length;
			int l2 = s2.Length;
			int[,] a = new int[l1 + 1, l2 + 1];
			if(l1 == 0)
				return l2;
			if(l2 == 0)
				return l1;
			int n1 = 0;
			while (n1 <= l1)
				a[n1, 0] = n1++;
			int n2 = 0;
			while (n2 <= l2)
				a[0, n2] = n2++;
			for(int i = 1; i <= l1; i++)
				for(int j = 1; j <= l2; j++){
					int n3 = (s2[j - 1] != s1[i - 1]) ? 1 : 0;
					a[i, j] = System.Math.Min(System.Math.Min(a[i - 1, j] + 1, a[i, j - 1] + 1), a[i - 1, j - 1] + n3);
				}
			return a[l1, l2];
		}
	}
}
namespace RealEstateAgency.Entities{
	public partial class PersonSet{
		public string Text => $"{FirstName} {MiddleName} {LastName}";
		public string Demo => $"{MiddleName[0]}. {LastName[0]}. {FirstName}";
	}

	public partial class PersonSet_Agent{
		public string Text => $"{PersonSet.Text} ({DealShare} %)";
	}

	public partial class PersonSet_Client{
		public string Text => $"{PersonSet.Text} ({Phone}, {Email})";
	}

	public partial class RealEstateSet{
		public enum RealEstateSet_Type{
			Apartment, House, Land
		}
		public RealEstateSet_Type Type;
		public string SharedText => $"{Address_City}, {Address_Street}";
		public string Text => Type == RealEstateSet_Type.Apartment ? RealEstateSet_Apartment.Text : Type == RealEstateSet_Type.House ? RealEstateSet_House.Text : Type == RealEstateSet_Type.Land ? RealEstateSet_Land.Text : SharedText;
		public string Demo => Type == RealEstateSet_Type.Apartment ? RealEstateSet_Apartment.Demo : Type == RealEstateSet_Type.House ? RealEstateSet_House.Demo : Type == RealEstateSet_Type.Land ? RealEstateSet_Land.Demo : SharedText;
	}

	public partial class RealEstateSet_Apartment{
		public string Text => $"{Demo}, {RealEstateSet.Coordinate_latitude}°, {RealEstateSet.Coordinate_longitude}° ({TotalArea} м², эт. {Floor}, {Rooms} к.)";
		public string Demo => $"Квартира, {RealEstateSet.SharedText}, д. {RealEstateSet.Address_House}, кв. {RealEstateSet.Address_Number}";
	}

	public partial class RealEstateSet_House{
		public string Text => $"{Demo}, {RealEstateSet.Coordinate_latitude}°, {RealEstateSet.Coordinate_longitude}° ({TotalArea} м², {TotalFloors} эт.)";
		public string Demo => $"Дом, {RealEstateSet.SharedText}, д. {RealEstateSet.Address_House}";
	}

	public partial class RealEstateSet_Land{
		public string Text => $"{Demo}, {RealEstateSet.Coordinate_latitude}°, {RealEstateSet.Coordinate_longitude}° ({TotalArea} м²)";
		public string Demo => $"Земля, {RealEstateSet.SharedText}";
	}

	public partial class SupplySet{
		public string Text => $"{Demo}, к. {PersonSet_Client.PersonSet.Demo}, р. {PersonSet_Client.PersonSet.Demo}";
		public string Demo => $"{RealEstateSet.Demo} за {Price}";
		public bool Demanded(DemandSet Demand) =>
			RealEstateSet.Type == Demand.Type &&
			(string.IsNullOrEmpty(Demand.Address_City) ? true : Utils.LevenshteinDistance(RealEstateSet.Address_City, Demand.Address_City) <= 3) &&
			(string.IsNullOrEmpty(Demand.Address_Street) ? true : Utils.LevenshteinDistance(RealEstateSet.Address_Street, Demand.Address_Street) <= 3) &&
			(string.IsNullOrEmpty(Demand.Address_House) ? true : Utils.LevenshteinDistance(RealEstateSet.Address_House, Demand.Address_House) <= 1) &&
			(string.IsNullOrEmpty(Demand.Address_Number) ? true : Utils.LevenshteinDistance(RealEstateSet.Address_Number, Demand.Address_Number) <= 1) &&
			(Demand.MinPrice == null ? true : Demand.MinPrice <= Price) &&
			(Demand.MaxPrice == null ? true : Price <= Demand.MaxPrice) &&
			(RealEstateSet.Type != RealEstateSet.RealEstateSet_Type.Apartment ? true :
				(Demand.RealEstateFilterSet.RealEstateFilterSet_ApartmentFilter.MinArea == null ? true : Demand.RealEstateFilterSet.RealEstateFilterSet_ApartmentFilter.MinArea <= RealEstateSet.RealEstateSet_Apartment.TotalArea) &&
				(Demand.RealEstateFilterSet.RealEstateFilterSet_ApartmentFilter.MaxArea == null ? true : RealEstateSet.RealEstateSet_Apartment.TotalArea <= Demand.RealEstateFilterSet.RealEstateFilterSet_ApartmentFilter.MaxArea) &&
				(Demand.RealEstateFilterSet.RealEstateFilterSet_ApartmentFilter.MinFloor == null ? true : Demand.RealEstateFilterSet.RealEstateFilterSet_ApartmentFilter.MinFloor <= RealEstateSet.RealEstateSet_Apartment.Floor) &&
				(Demand.RealEstateFilterSet.RealEstateFilterSet_ApartmentFilter.MaxFloor == null ? true : RealEstateSet.RealEstateSet_Apartment.Floor <= Demand.RealEstateFilterSet.RealEstateFilterSet_ApartmentFilter.MaxFloor) &&
				(Demand.RealEstateFilterSet.RealEstateFilterSet_ApartmentFilter.MinRooms == null ? true : Demand.RealEstateFilterSet.RealEstateFilterSet_ApartmentFilter.MinRooms <= RealEstateSet.RealEstateSet_Apartment.Rooms) &&
				(Demand.RealEstateFilterSet.RealEstateFilterSet_ApartmentFilter.MaxRooms == null ? true : RealEstateSet.RealEstateSet_Apartment.Rooms <= Demand.RealEstateFilterSet.RealEstateFilterSet_ApartmentFilter.MaxRooms)
			) &&
			(RealEstateSet.Type != RealEstateSet.RealEstateSet_Type.House ? true :
				(Demand.RealEstateFilterSet.RealEstateFilterSet_HouseFilter.MinArea == null ? true : Demand.RealEstateFilterSet.RealEstateFilterSet_HouseFilter.MinArea <= RealEstateSet.RealEstateSet_House.TotalArea) &&
				(Demand.RealEstateFilterSet.RealEstateFilterSet_HouseFilter.MaxArea == null ? true : RealEstateSet.RealEstateSet_House.TotalArea <= Demand.RealEstateFilterSet.RealEstateFilterSet_HouseFilter.MaxArea) &&
				(Demand.RealEstateFilterSet.RealEstateFilterSet_HouseFilter.MinFloors == null ? true : Demand.RealEstateFilterSet.RealEstateFilterSet_HouseFilter.MinFloors <= RealEstateSet.RealEstateSet_House.TotalFloors) &&
				(Demand.RealEstateFilterSet.RealEstateFilterSet_HouseFilter.MaxFloors == null ? true : RealEstateSet.RealEstateSet_House.TotalFloors <= Demand.RealEstateFilterSet.RealEstateFilterSet_HouseFilter.MaxFloors)
			) &&
			(RealEstateSet.Type != RealEstateSet.RealEstateSet_Type.Land ? true :
				(Demand.RealEstateFilterSet.RealEstateFilterSet_LandFilter.MinArea == null ? true : Demand.RealEstateFilterSet.RealEstateFilterSet_LandFilter.MinArea <= RealEstateSet.RealEstateSet_Land.TotalArea) &&
				(Demand.RealEstateFilterSet.RealEstateFilterSet_LandFilter.MaxArea == null ? true : RealEstateSet.RealEstateSet_Land.TotalArea <= Demand.RealEstateFilterSet.RealEstateFilterSet_LandFilter.MaxArea)
			);
		public System.Collections.Generic.List<DemandSet> DemandsList{
			get{
				System.Collections.Generic.List<DemandSet> DemandsList = new System.Collections.Generic.List<DemandSet>();
				foreach(var Demand in App.Context.DemandSet)
					if(Demanded(Demand))
						DemandsList.Add(Demand);
				return DemandsList;
			}
		}
	}

	public partial class DemandSet{
		public RealEstateSet.RealEstateSet_Type Type;
		public string SharedText => $"{Address_City}, {Address_Street}";
		public string Text => Type == RealEstateSet.RealEstateSet_Type.Apartment ? RealEstateFilterSet.RealEstateFilterSet_ApartmentFilter.Text : Type == RealEstateSet.RealEstateSet_Type.House ? RealEstateFilterSet.RealEstateFilterSet_HouseFilter.Text : Type == RealEstateSet.RealEstateSet_Type.Land ? RealEstateFilterSet.RealEstateFilterSet_LandFilter.Text : SharedText;
		public string Demo => Type == RealEstateSet.RealEstateSet_Type.Apartment ? RealEstateFilterSet.RealEstateFilterSet_ApartmentFilter.Demo : Type == RealEstateSet.RealEstateSet_Type.House ? RealEstateFilterSet.RealEstateFilterSet_HouseFilter.Demo : Type == RealEstateSet.RealEstateSet_Type.Land ? RealEstateFilterSet.RealEstateFilterSet_LandFilter.Demo : SharedText;
		public System.Collections.Generic.List<SupplySet> SuppliesList{
			get{
				System.Collections.Generic.List<SupplySet> SuppliesList = new System.Collections.Generic.List<SupplySet>();
				foreach(var Supply in App.Context.SupplySet)
					if(Supply.Demanded(this))
						SuppliesList.Add(Supply);
				return SuppliesList;
			}
		}
	}

	public partial class RealEstateFilterSet_ApartmentFilter{
		public string Text => $"{Demo}, д. {RealEstateFilterSet.DemandSet.First().Address_House}, кв. {RealEstateFilterSet.DemandSet.First().Address_Number}, к. {RealEstateFilterSet.DemandSet.First().PersonSet_Client.PersonSet.Demo}, р. {RealEstateFilterSet.DemandSet.First().PersonSet_Agent.PersonSet.Demo} ({MinArea} - {MaxArea} м², эт. {MinFloor} - {MaxFloor}, {MinRooms} - {MaxRooms} к.)";
		public string Demo => $"Квартира за {RealEstateFilterSet.DemandSet.First().MinPrice} - {RealEstateFilterSet.DemandSet.First().MaxPrice}, {RealEstateFilterSet.DemandSet.First().SharedText}";
	}

	public partial class RealEstateFilterSet_HouseFilter{
		public string Text => $"{Demo}, д. {RealEstateFilterSet.DemandSet.First().Address_House}, кв. {RealEstateFilterSet.DemandSet.First().Address_Number}, к. {RealEstateFilterSet.DemandSet.First().PersonSet_Client.PersonSet.Demo}, р. {RealEstateFilterSet.DemandSet.First().PersonSet_Agent.PersonSet.Demo} ({MinArea} - {MaxArea} м², {MinFloors} - {MaxFloors} эт.)";
		public string Demo => $"Дом за {RealEstateFilterSet.DemandSet.First().MinPrice} - {RealEstateFilterSet.DemandSet.First().MaxPrice}, {RealEstateFilterSet.DemandSet.First().SharedText}";
	}

	public partial class RealEstateFilterSet_LandFilter{
		public string Text => $"{Demo}, д. {RealEstateFilterSet.DemandSet.First().Address_House}, кв. {RealEstateFilterSet.DemandSet.First().Address_Number}, к. {RealEstateFilterSet.DemandSet.First().PersonSet_Client.PersonSet.Demo}, р. {RealEstateFilterSet.DemandSet.First().PersonSet_Agent.PersonSet.Demo} ({MinArea} - {MaxArea} м²)";
		public string Demo => $"Земля за {RealEstateFilterSet.DemandSet.First().MinPrice} - {RealEstateFilterSet.DemandSet.First().MaxPrice}, {RealEstateFilterSet.DemandSet.First().SharedText}";
	}
	
	public partial class DealSet{
		public long SupplyCommission => SupplySet.RealEstateSet.Type == RealEstateSet.RealEstateSet_Type.Apartment ? 36000 + SupplySet.Price / 100 : SupplySet.RealEstateSet.Type == RealEstateSet.RealEstateSet_Type.House ? 30000 + SupplySet.Price / 100 : SupplySet.RealEstateSet.Type == RealEstateSet.RealEstateSet_Type.Land ? 30000 + 2 * SupplySet.Price / 100 : 0;
		public long DemandCommission => 3 * SupplySet.Price / 100;
		public long SupplyDealShare => SupplyCommission / 100 * SupplySet.PersonSet_Agent.DealShare;
		public long DemandDealShare => DemandCommission / 100 * DemandSet.PersonSet_Agent.DealShare;
		public long AgencyGains => SupplyCommission - SupplyDealShare + DemandCommission - DemandDealShare;
		public string Demo => $"{SupplySet.Demo} для {DemandSet.Demo}";
	}
}