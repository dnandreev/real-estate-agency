using System.Linq;
using System.Windows;
using System.Windows.Controls;
namespace RealEstateAgency{
	public partial class DemandsPage : Page{
		Entities.DemandSet Editable;

		void UpdateItemsSource(){
			DemandsList.ItemsSource = App.Context.DemandSet.ToList();
			Client.ItemsSource = App.Context.PersonSet_Client.ToList();
			Agent.ItemsSource = App.Context.PersonSet_Agent.ToList();
			Editable = null;
		}

		public DemandsPage(){
			InitializeComponent();
			UpdateItemsSource();
		}

		void View(object sender, SelectionChangedEventArgs e){
			if(DemandsList.SelectedItem != null){
				Editable = DemandsList.SelectedItem as Entities.DemandSet;
				Text.Text = Editable.Text;
				Client.SelectedItem = Editable.PersonSet_Client;
				Agent.SelectedItem = Editable.PersonSet_Agent;
				Type.SelectedIndex = (int)Editable.Type;
				PriceMin.Text = Editable.MinPrice.ToString();
				PriceMax.Text = Editable.MaxPrice.ToString();
				Address_City.Text = Editable.Address_City;
				Address_Street.Text = Editable.Address_Street;
				Address_House.Text = Editable.Address_House;
				Address_Number.Text = Editable.Address_Number;
				TotalAreaMin.Text = Editable.Type == Entities.RealEstateSet.RealEstateSet_Type.Apartment ? Editable.RealEstateFilterSet.RealEstateFilterSet_ApartmentFilter.MinArea.ToString() : Editable.Type == Entities.RealEstateSet.RealEstateSet_Type.House ? Editable.RealEstateFilterSet.RealEstateFilterSet_HouseFilter.MinArea.ToString() : Editable.Type == Entities.RealEstateSet.RealEstateSet_Type.Land ? Editable.RealEstateFilterSet.RealEstateFilterSet_LandFilter.MinArea.ToString() : "";
				TotalAreaMax.Text = Editable.Type == Entities.RealEstateSet.RealEstateSet_Type.Apartment ? Editable.RealEstateFilterSet.RealEstateFilterSet_ApartmentFilter.MaxArea.ToString() : Editable.Type == Entities.RealEstateSet.RealEstateSet_Type.House ? Editable.RealEstateFilterSet.RealEstateFilterSet_HouseFilter.MaxArea.ToString() : Editable.Type == Entities.RealEstateSet.RealEstateSet_Type.Land ? Editable.RealEstateFilterSet.RealEstateFilterSet_LandFilter.MaxArea.ToString() : "";
				FloorOrTotalFloorsMin.Text = Editable.Type == Entities.RealEstateSet.RealEstateSet_Type.Apartment ? Editable.RealEstateFilterSet.RealEstateFilterSet_ApartmentFilter.MinFloor.ToString() : Editable.Type == Entities.RealEstateSet.RealEstateSet_Type.House ? Editable.RealEstateFilterSet.RealEstateFilterSet_HouseFilter.MinFloors.ToString() : "";
				FloorOrTotalFloorsMax.Text = Editable.Type == Entities.RealEstateSet.RealEstateSet_Type.Apartment ? Editable.RealEstateFilterSet.RealEstateFilterSet_ApartmentFilter.MaxFloor.ToString() : Editable.Type == Entities.RealEstateSet.RealEstateSet_Type.House ? Editable.RealEstateFilterSet.RealEstateFilterSet_HouseFilter.MaxFloors.ToString() : "";
				RoomsMin.Text = Editable.Type == Entities.RealEstateSet.RealEstateSet_Type.Apartment ? Editable.RealEstateFilterSet.RealEstateFilterSet_ApartmentFilter.MinRooms.ToString() : "";
				RoomsMax.Text = Editable.Type == Entities.RealEstateSet.RealEstateSet_Type.Apartment ? Editable.RealEstateFilterSet.RealEstateFilterSet_ApartmentFilter.MaxRooms.ToString() : "";
				SuppliesList.ItemsSource = Editable.SuppliesList;
			}
			else{
				Editable = null;
				Text.Text = "Выберите потребность";
				Client.SelectedIndex = Agent.SelectedIndex = -1;
				Type.SelectedIndex = 3;
				PriceMin.Text = PriceMax.Text = Address_City.Text = Address_Street.Text = Address_House.Text = Address_Number.Text = TotalAreaMin.Text = TotalAreaMax.Text = FloorOrTotalFloorsMin.Text = FloorOrTotalFloorsMax.Text = RoomsMin.Text = RoomsMax.Text = "";
				SuppliesList.ItemsSource = null;
			}
		}

		void Create(object sender, RoutedEventArgs e){
			Editable = null;
			Text.Text = "Добавьте потребность";
			Client.SelectedIndex = Agent.SelectedIndex = -1;
			Type.SelectedIndex = 3;
			PriceMin.Text = PriceMax.Text = Address_City.Text = Address_Street.Text = Address_House.Text = Address_Number.Text = TotalAreaMin.Text = TotalAreaMax.Text = FloorOrTotalFloorsMin.Text = FloorOrTotalFloorsMax.Text = RoomsMin.Text = RoomsMax.Text = "";
			SuppliesList.ItemsSource = null;
		}

		void Delete(object sender, RoutedEventArgs e){
			if(Editable == null)
				MessageBox.Show("Нечего удалять.", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
			else if(Editable.DealSet.Count > 0)
				MessageBox.Show($"Потребность {Editable.Demo} связана со сделками!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(MessageBox.Show($"Подтвердите удаление потребности {Editable.Demo}.", Title, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes){
				string EditableDemo = Editable.Demo;
				App.Context.DemandSet.Remove(Editable);
				App.Context.SaveChanges();
				MessageBox.Show($"Потребность {EditableDemo} удалена.", Title, MessageBoxButton.OK, MessageBoxImage.Information);
				Create(null, null);
				UpdateItemsSource();
			}
		}

		void Save(object sender, RoutedEventArgs e){
			double TotalAreaMinInput = 0, TotalAreaMaxInput = 0;
			int FloorOrTotalFloorsMinInput = 1, FloorOrTotalFloorsMaxInput = 1, RoomsMinInput = 1, RoomsMaxInput = 1;
			long MinPriceInput = 0, MaxPriceInput = 0;
			if(Client.SelectedIndex == -1)
				MessageBox.Show("Клиент не выбран!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(Agent.SelectedIndex == -1)
				MessageBox.Show("Агент не выбран!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(Type.SelectedIndex == 3)
				MessageBox.Show("Тип не выбран!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(PriceMin.Text != "" && (long.TryParse(PriceMin.Text, out MinPriceInput) == false || MinPriceInput < 0))
					MessageBox.Show("Мин. цена не является положительным целым числом!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(PriceMax.Text != "" && (long.TryParse(PriceMax.Text, out MaxPriceInput) == false || MaxPriceInput < 0))
				MessageBox.Show("Макс. цена не является положительным целым числом!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(TotalAreaMin.Text != "" && (double.TryParse(TotalAreaMin.Text, out TotalAreaMinInput) == false || TotalAreaMinInput < 0))
				MessageBox.Show("Мин. площадь не является положительным рациональным числом!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(TotalAreaMax.Text != "" && (double.TryParse(TotalAreaMax.Text, out TotalAreaMaxInput) == false || TotalAreaMaxInput < 0))
				MessageBox.Show("Макс. площадь не является положительным рациональным числом!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(FloorOrTotalFloorsMin.Text != "" && (int.TryParse(FloorOrTotalFloorsMin.Text, out FloorOrTotalFloorsMinInput) == false || FloorOrTotalFloorsMinInput < 0))
					MessageBox.Show("Мин. этаж(ей) не является натуральным числом!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(FloorOrTotalFloorsMax.Text != "" && (int.TryParse(FloorOrTotalFloorsMax.Text, out FloorOrTotalFloorsMaxInput) == false || FloorOrTotalFloorsMaxInput < 0))
					MessageBox.Show("Макс. этаж(ей) не является натуральным числом!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(RoomsMin.Text != "" && (int.TryParse(RoomsMin.Text, out RoomsMinInput) == false || RoomsMinInput < 0))
				MessageBox.Show("Мин. комнат не является натуральным числом!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(RoomsMax.Text != "" && (int.TryParse(RoomsMax.Text, out RoomsMaxInput) == false || RoomsMaxInput < 0))
				MessageBox.Show("Макс. комнат не является натуральным числом!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(Editable != null){
				Editable.PersonSet_Client = Client.SelectedItem as Entities.PersonSet_Client;
				Editable.PersonSet_Agent = Agent.SelectedItem as Entities.PersonSet_Agent;
				Editable.Type = (Entities.RealEstateSet.RealEstateSet_Type)Type.SelectedIndex;
				Editable.MinPrice = PriceMin.Text == "" ? null : (long?)MinPriceInput;
				Editable.MaxPrice = PriceMax.Text == "" ? null : (long?)MaxPriceInput;
				Editable.Address_City = Address_City.Text;
				Editable.Address_Street = Address_Street.Text;
				Editable.Address_House = Address_House.Text;
				Editable.Address_Number = Address_Number.Text;
				if(Editable.Type == Entities.RealEstateSet.RealEstateSet_Type.Apartment){
					Editable.RealEstateFilterSet.RealEstateFilterSet_ApartmentFilter = new Entities.RealEstateFilterSet_ApartmentFilter{
						MinArea = TotalAreaMin.Text == "" ? null : (double?)TotalAreaMinInput,
						MaxArea = TotalAreaMax.Text == "" ? null : (double?)TotalAreaMaxInput,
						MinFloor = FloorOrTotalFloorsMin.Text == "" ? null : (int?)FloorOrTotalFloorsMinInput,
						MaxFloor = FloorOrTotalFloorsMax.Text == "" ? null : (int?)FloorOrTotalFloorsMaxInput,
						MinRooms = RoomsMin.Text == "" ? null : (int?)RoomsMinInput,
						MaxRooms = RoomsMax.Text == "" ? null : (int?)RoomsMaxInput
					};
					if(Editable.RealEstateFilterSet.RealEstateFilterSet_HouseFilter != null)
						App.Context.RealEstateFilterSet_HouseFilter.Remove(Editable.RealEstateFilterSet.RealEstateFilterSet_HouseFilter);
					if(Editable.RealEstateFilterSet.RealEstateFilterSet_LandFilter != null)
						App.Context.RealEstateFilterSet_LandFilter.Remove(Editable.RealEstateFilterSet.RealEstateFilterSet_LandFilter);

				}
				else if(Editable.Type == Entities.RealEstateSet.RealEstateSet_Type.House){
					Editable.RealEstateFilterSet.RealEstateFilterSet_HouseFilter = new Entities.RealEstateFilterSet_HouseFilter{
						MinArea = TotalAreaMin.Text == "" ? null : (double?)TotalAreaMinInput,
						MaxArea = TotalAreaMax.Text == "" ? null : (double?)TotalAreaMaxInput,
						MinFloors = FloorOrTotalFloorsMin.Text == "" ? null : (int?)FloorOrTotalFloorsMinInput,
						MaxFloors = FloorOrTotalFloorsMax.Text == "" ? null : (int?)FloorOrTotalFloorsMaxInput
					};
					if(Editable.RealEstateFilterSet.RealEstateFilterSet_ApartmentFilter != null)
						App.Context.RealEstateFilterSet_ApartmentFilter.Remove(Editable.RealEstateFilterSet.RealEstateFilterSet_ApartmentFilter);
					if(Editable.RealEstateFilterSet.RealEstateFilterSet_LandFilter != null)
						App.Context.RealEstateFilterSet_LandFilter.Remove(Editable.RealEstateFilterSet.RealEstateFilterSet_LandFilter);
				}
				else if(Editable.Type == Entities.RealEstateSet.RealEstateSet_Type.Land){
					Editable.RealEstateFilterSet.RealEstateFilterSet_LandFilter = new Entities.RealEstateFilterSet_LandFilter{
						MinArea = TotalAreaMin.Text == "" ? null : (double?)TotalAreaMinInput,
						MaxArea = TotalAreaMax.Text == "" ? null : (double?)TotalAreaMaxInput
					};
					if(Editable.RealEstateFilterSet.RealEstateFilterSet_ApartmentFilter != null)
						App.Context.RealEstateFilterSet_ApartmentFilter.Remove(Editable.RealEstateFilterSet.RealEstateFilterSet_ApartmentFilter);
					if(Editable.RealEstateFilterSet.RealEstateFilterSet_HouseFilter != null)
						App.Context.RealEstateFilterSet_HouseFilter.Remove(Editable.RealEstateFilterSet.RealEstateFilterSet_HouseFilter);
				}
				App.Context.SaveChanges();
				MessageBox.Show($"Потребность {Editable.Demo} изменена.", Title, MessageBoxButton.OK, MessageBoxImage.Information);
				Create(null, null);
				UpdateItemsSource();
			}
			else{
				Entities.DemandSet Createable = new Entities.DemandSet{
					PersonSet_Client = Client.SelectedItem as Entities.PersonSet_Client,
					PersonSet_Agent = Agent.SelectedItem as Entities.PersonSet_Agent,
					Type = (Entities.RealEstateSet.RealEstateSet_Type)Type.SelectedIndex,
					MinPrice = PriceMin.Text == "" ? null : (long?)MinPriceInput,
					MaxPrice = PriceMax.Text == "" ? null : (long?)MaxPriceInput,
					Address_City = Address_City.Text,
					Address_Street = Address_Street.Text,
					Address_House = Address_House.Text,
					Address_Number = Address_Number.Text,
					RealEstateFilterSet = new Entities.RealEstateFilterSet()
				};
				if(Createable.Type == Entities.RealEstateSet.RealEstateSet_Type.Apartment)
					Createable.RealEstateFilterSet.RealEstateFilterSet_ApartmentFilter = new Entities.RealEstateFilterSet_ApartmentFilter{
						MinArea = TotalAreaMin.Text == "" ? null : (double?)TotalAreaMinInput,
						MaxArea = TotalAreaMax.Text == "" ? null : (double?)TotalAreaMaxInput,
						MinFloor = FloorOrTotalFloorsMin.Text == "" ? null : (int?)FloorOrTotalFloorsMinInput,
						MaxFloor = FloorOrTotalFloorsMax.Text == "" ? null : (int?)FloorOrTotalFloorsMaxInput,
						MinRooms = RoomsMin.Text == "" ? null : (int?)RoomsMinInput,
						MaxRooms = RoomsMax.Text == "" ? null : (int?)RoomsMaxInput
					};
				else if(Createable.Type == Entities.RealEstateSet.RealEstateSet_Type.House)
					Createable.RealEstateFilterSet.RealEstateFilterSet_HouseFilter = new Entities.RealEstateFilterSet_HouseFilter{
						MinArea = TotalAreaMin.Text == "" ? null : (double?)TotalAreaMinInput,
						MaxArea = TotalAreaMax.Text == "" ? null : (double?)TotalAreaMaxInput,
						MinFloors = FloorOrTotalFloorsMin.Text == "" ? null : (int?)FloorOrTotalFloorsMinInput,
						MaxFloors = FloorOrTotalFloorsMax.Text == "" ? null : (int?)FloorOrTotalFloorsMaxInput
					};
				else if(Createable.Type == Entities.RealEstateSet.RealEstateSet_Type.Land)
					Createable.RealEstateFilterSet.RealEstateFilterSet_LandFilter = new Entities.RealEstateFilterSet_LandFilter{
						MinArea = TotalAreaMin.Text == "" ? null : (double?)TotalAreaMinInput,
						MaxArea = TotalAreaMax.Text == "" ? null : (double?)TotalAreaMaxInput
					};
				App.Context.DemandSet.Add(Createable);
				App.Context.SaveChanges();
				MessageBox.Show($"Потребность {Createable.Demo} добавлена.", Title, MessageBoxButton.OK, MessageBoxImage.Information);
				Create(null, null);
				UpdateItemsSource();
			}
		}
		void Do(object sender, RoutedEventArgs e){
			if(SuppliesList.SelectedItem == null)
				MessageBox.Show("Предложение не выбрано!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(DemandsList.SelectedItem == null)
				MessageBox.Show("Потребность не выбрана!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if((SuppliesList.SelectedItem as Entities.SupplySet).DealSet.Count > 0)
				MessageBox.Show("Предложение участвует в другой сделке!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if((DemandsList.SelectedItem as Entities.DemandSet).DealSet.Count > 0)
					MessageBox.Show("Потребность участвует в другой сделке!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else{
				Entities.DealSet Createable = new Entities.DealSet{
					SupplySet = SuppliesList.SelectedItem as Entities.SupplySet,
					DemandSet = DemandsList.SelectedItem as Entities.DemandSet
				};
				App.Context.DealSet.Add(Createable);
				App.Context.SaveChanges();
				MessageBox.Show($"Сделка {Createable.Demo} добавлена.", Title, MessageBoxButton.OK, MessageBoxImage.Information);
				Create(null, null);
				UpdateItemsSource();
			}
		}
	}
}