using System.Linq;
using System.Windows;
namespace RealEstateAgency{
	public partial class MainWindow : Window{
		public MainWindow(){
			InitializeComponent();
			foreach(var Apartment in App.Context.RealEstateSet_Apartment)
				Apartment.RealEstateSet.Type = Entities.RealEstateSet.RealEstateSet_Type.Apartment;
			foreach(var House in App.Context.RealEstateSet_House)
				House.RealEstateSet.Type = Entities.RealEstateSet.RealEstateSet_Type.House;
			foreach(var Land in App.Context.RealEstateSet_Land)
				Land.RealEstateSet.Type = Entities.RealEstateSet.RealEstateSet_Type.Land;
			foreach(var ApartmentFilter in App.Context.RealEstateFilterSet_ApartmentFilter){
				var DemandSet = ApartmentFilter.RealEstateFilterSet.DemandSet.FirstOrDefault();
				if(DemandSet != default(Entities.DemandSet))
					DemandSet.Type = Entities.RealEstateSet.RealEstateSet_Type.Apartment;
			}
			foreach(var HouseFilter in App.Context.RealEstateFilterSet_HouseFilter){
				var DemandSet = HouseFilter.RealEstateFilterSet.DemandSet.FirstOrDefault();
				if(DemandSet != default(Entities.DemandSet))
					DemandSet.Type = Entities.RealEstateSet.RealEstateSet_Type.House;
			}
			foreach(var LandFilter in App.Context.RealEstateFilterSet_LandFilter){
				var DemandSet = LandFilter.RealEstateFilterSet.DemandSet.FirstOrDefault();
				if(DemandSet != default(Entities.DemandSet))
					DemandSet.Type = Entities.RealEstateSet.RealEstateSet_Type.Land;
			}
			App.Context.SaveChanges();
			MainFrame.Navigate(new MenuPage());
		}

		void Return(object sender, RoutedEventArgs e){
			if(MainFrame.CanGoBack)
				MainFrame.GoBack();
		}
	}
}