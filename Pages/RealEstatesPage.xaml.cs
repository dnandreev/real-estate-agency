using System.Linq;
using System.Windows;
using System.Windows.Controls;
namespace RealEstateAgency{
	public partial class RealEstatesPage : Page{
		Entities.RealEstateSet Editable;

		void UpdateItemsSource(){
			RealEstatesList.ItemsSource = App.Context.RealEstateSet.ToList();
			Editable = null;
		}

		public RealEstatesPage(){
			InitializeComponent();
			UpdateItemsSource();
		}

		void View(object sender, SelectionChangedEventArgs e){
			if(RealEstatesList.SelectedItem != null){
				Editable = RealEstatesList.SelectedItem as Entities.RealEstateSet;
				Text.Text = Editable.Text;
				Type.SelectedIndex = (int)Editable.Type;
				Address_City.Text = Editable.Address_City;
				Address_Street.Text = Editable.Address_Street;
				Address_House.Text = Editable.Address_House;
				Address_Number.Text = Editable.Address_Number;
				Coordinate_latitude.Text = Editable.Coordinate_latitude.ToString();
				Coordinate_longitude.Text = Editable.Coordinate_longitude.ToString();
				TotalArea.Text = Editable.Type == Entities.RealEstateSet.RealEstateSet_Type.Apartment ? Editable.RealEstateSet_Apartment.TotalArea.ToString() : Editable.Type == Entities.RealEstateSet.RealEstateSet_Type.House ? Editable.RealEstateSet_House.TotalArea.ToString() : Editable.Type == Entities.RealEstateSet.RealEstateSet_Type.Land ? Editable.RealEstateSet_Land.TotalArea.ToString() : "";
				FloorOrTotalFloors.Text = Editable.Type == Entities.RealEstateSet.RealEstateSet_Type.Apartment ? Editable.RealEstateSet_Apartment.Floor.ToString() : Editable.Type == Entities.RealEstateSet.RealEstateSet_Type.House ? Editable.RealEstateSet_House.TotalFloors.ToString() : "";
				Rooms.Text = Editable.Type == Entities.RealEstateSet.RealEstateSet_Type.Apartment ? Editable.RealEstateSet_Apartment.Rooms.ToString() : "";
			}
			else{
				Editable = null;
				Text.Text = "Выберите объект недвижимости";
				Type.SelectedIndex = 3;
				Address_City.Text = Address_Street.Text = Address_House.Text = Address_Number.Text = Coordinate_latitude.Text = Coordinate_longitude.Text = TotalArea.Text = FloorOrTotalFloors.Text = Rooms.Text = "";
			}
		}

		void Create(object sender, RoutedEventArgs e){
			Editable = null;
			Text.Text = "Добавьте объект недвижимости";
			Type.SelectedIndex = 3;
			Address_City.Text = Address_Street.Text = Address_House.Text = Address_Number.Text = Coordinate_latitude.Text = Coordinate_longitude.Text = TotalArea.Text = FloorOrTotalFloors.Text = Rooms.Text = "";
		}

		void Delete(object sender, RoutedEventArgs e){
			if(Editable == null)
				MessageBox.Show("Нечего удалять.", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
			else if(Editable.SupplySet.Count > 0)
				MessageBox.Show($"Объект недвижимости {Editable.Demo} связан с предложениями!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(MessageBox.Show($"Подтвердите удаление объекта недвижимости {Editable.Demo}.", Title, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes){
				string EditableDemo = Editable.Demo;
				App.Context.RealEstateSet.Remove(Editable);
				App.Context.SaveChanges();
				MessageBox.Show($"Объект недвижимости {EditableDemo} удалён.", Title, MessageBoxButton.OK, MessageBoxImage.Information);
				Create(null, null);
				UpdateItemsSource();
			}
		}

		void Save(object sender, RoutedEventArgs e){
			double Coordinate_latitudeInput = 0, Coordinate_longitudeInput = 0, TotalAreaInput = 0;
			int FloorOrTotalFloorsInput = 1, RoomsInput = 1;
			if(Type.SelectedIndex == 3)
				MessageBox.Show("Тип не выбран!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(Coordinate_latitude.Text != "" && (double.TryParse(Coordinate_latitude.Text, out Coordinate_latitudeInput) == false || Coordinate_latitudeInput < -90 || Coordinate_latitudeInput > 90))
				MessageBox.Show("Широта не является рациональным числом от -90 до 90!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(Coordinate_longitude.Text != "" && (double.TryParse(Coordinate_longitude.Text, out Coordinate_longitudeInput) == false || Coordinate_longitudeInput < -90 || Coordinate_longitudeInput > 90))
				MessageBox.Show("Долгота не является рациональным числом от -180 до 180!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(TotalArea.Text != "" && (double.TryParse(TotalArea.Text, out TotalAreaInput) == false || TotalAreaInput < 0))
				MessageBox.Show("Площадь не является положительным рациональным числом!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(FloorOrTotalFloors.Text != "" && (int.TryParse(FloorOrTotalFloors.Text, out FloorOrTotalFloorsInput) == false || FloorOrTotalFloorsInput < 1))
				MessageBox.Show("Этаж(ей) не является натуральным числом!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(Rooms.Text != "" && (int.TryParse(Rooms.Text, out RoomsInput) == false || RoomsInput < 1))
				MessageBox.Show("Комнат не является натуральным числом!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(Editable != null){
				Editable.Address_City = Address_City.Text;
				Editable.Address_Street = Address_Street.Text;
				Editable.Address_House = Address_House.Text;
				Editable.Address_Number = Address_Number.Text;
				Editable.Coordinate_latitude = Coordinate_latitudeInput;
				Editable.Coordinate_longitude = Coordinate_longitudeInput;
				Editable.Type = (Entities.RealEstateSet.RealEstateSet_Type)Type.SelectedIndex;
				if(Editable.Type == Entities.RealEstateSet.RealEstateSet_Type.Apartment){
					Editable.RealEstateSet_Apartment = new Entities.RealEstateSet_Apartment{
						TotalArea = TotalAreaInput,
						Floor = FloorOrTotalFloorsInput,
						Rooms = RoomsInput
					};
					if(Editable.RealEstateSet_House != null)
						App.Context.RealEstateSet_House.Remove(Editable.RealEstateSet_House);
					if(Editable.RealEstateSet_Land != null)
						App.Context.RealEstateSet_Land.Remove(Editable.RealEstateSet_Land);

				}
				else if(Editable.Type == Entities.RealEstateSet.RealEstateSet_Type.House){
					Editable.RealEstateSet_House = new Entities.RealEstateSet_House{
						TotalArea = TotalAreaInput,
						TotalFloors = FloorOrTotalFloorsInput
					};
					if(Editable.RealEstateSet_Apartment != null)
						App.Context.RealEstateSet_Apartment.Remove(Editable.RealEstateSet_Apartment);
					if(Editable.RealEstateSet_Land != null)
						App.Context.RealEstateSet_Land.Remove(Editable.RealEstateSet_Land);
				}
				else if(Editable.Type == Entities.RealEstateSet.RealEstateSet_Type.Land){
					Editable.RealEstateSet_Land = new Entities.RealEstateSet_Land{
						TotalArea = TotalAreaInput
					};
					if(Editable.RealEstateSet_Apartment != null)
						App.Context.RealEstateSet_Apartment.Remove(Editable.RealEstateSet_Apartment);
					if(Editable.RealEstateSet_House != null)
						App.Context.RealEstateSet_House.Remove(Editable.RealEstateSet_House);
				}
				App.Context.SaveChanges();
				MessageBox.Show($"Объект недвижимости {Editable.Demo} изменён.", Title, MessageBoxButton.OK, MessageBoxImage.Information);
				Create(null, null);
				UpdateItemsSource();
			}
			else{
				Entities.RealEstateSet Createable = new Entities.RealEstateSet{
					Address_City = Address_City.Text,
					Address_Street = Address_Street.Text,
					Address_House = Address_House.Text,
					Address_Number = Address_Number.Text,
					Coordinate_latitude = Coordinate_latitudeInput,
					Coordinate_longitude = Coordinate_longitudeInput,
					Type = (Entities.RealEstateSet.RealEstateSet_Type)Type.SelectedIndex
				};
				if(Createable.Type == Entities.RealEstateSet.RealEstateSet_Type.Apartment)
					Createable.RealEstateSet_Apartment = new Entities.RealEstateSet_Apartment{
						TotalArea = TotalAreaInput,
						Floor = FloorOrTotalFloorsInput,
						Rooms = RoomsInput
					};
				else if(Createable.Type == Entities.RealEstateSet.RealEstateSet_Type.House)
					Createable.RealEstateSet_House = new Entities.RealEstateSet_House{
						TotalArea = TotalAreaInput,
						TotalFloors = FloorOrTotalFloorsInput
					};
				else if(Createable.Type == Entities.RealEstateSet.RealEstateSet_Type.Land)
					Createable.RealEstateSet_Land = new Entities.RealEstateSet_Land{
						TotalArea = TotalAreaInput
					};
				App.Context.RealEstateSet.Add(Createable);
				App.Context.SaveChanges();
				MessageBox.Show($"Объект недвижимости {Createable.Demo} добавлен.", Title, MessageBoxButton.OK, MessageBoxImage.Information);
				Create(null, null);
				UpdateItemsSource();
			}
		}
	}
}