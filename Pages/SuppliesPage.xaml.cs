using System.Linq;
using System.Windows;
using System.Windows.Controls;
namespace RealEstateAgency{
	public partial class SuppliesPage : Page{
		Entities.SupplySet Editable;

		void UpdateItemsSource(){
			SuppliesList.ItemsSource = App.Context.SupplySet.ToList();
			RealEstate.ItemsSource = App.Context.RealEstateSet.ToList();
			Client.ItemsSource = App.Context.PersonSet_Client.ToList();
			Agent.ItemsSource = App.Context.PersonSet_Agent.ToList();
			Editable = null;
		}

		public SuppliesPage(){
			InitializeComponent();
			UpdateItemsSource();
		}

		void View(object sender, SelectionChangedEventArgs e){
			if(SuppliesList.SelectedItem != null){
				Editable = SuppliesList.SelectedItem as Entities.SupplySet;
				Text.Text = Editable.Text;
				RealEstate.SelectedItem = Editable.RealEstateSet;
				Price.Text = Editable.Price.ToString();
				Client.SelectedItem = Editable.PersonSet_Client;
				Agent.SelectedItem = Editable.PersonSet_Agent;
				DemandsList.ItemsSource = Editable.DemandsList;
			}
			else{
				Editable = null;
				Text.Text = "Выберите предложение";
				RealEstate.SelectedIndex = Client.SelectedIndex = Agent.SelectedIndex = -1;
				Price.Text = "";
				DemandsList.ItemsSource = null;
			}
		}

		void Create(object sender, RoutedEventArgs e){
			Editable = null;
			Text.Text = "Добавьте предложение";
			RealEstate.SelectedIndex = Client.SelectedIndex = Agent.SelectedIndex = -1;
			Price.Text = "";
			DemandsList.ItemsSource = null;
		}

		void Delete(object sender, RoutedEventArgs e){
			if(Editable == null)
				MessageBox.Show("Нечего удалять.", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
			else if(Editable.DealSet.Count > 0)
				MessageBox.Show($"Предложение {Editable.Demo} связано со сделками!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(MessageBox.Show($"Подтвердите удаление предложения {Editable.Demo}.", Title, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes){
				string EditableDemo = Editable.Demo;
				App.Context.SupplySet.Remove(Editable);
				App.Context.SaveChanges();
				MessageBox.Show($"Предложение {EditableDemo} удалено.", Title, MessageBoxButton.OK, MessageBoxImage.Information);
				Create(null, null);
				UpdateItemsSource();
			}
		}

		void Save(object sender, RoutedEventArgs e){
			long PriceInput;
			if(RealEstate.SelectedIndex == -1)
				MessageBox.Show("Объект недвижимости не выбран!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(long.TryParse(Price.Text, out PriceInput) == false || PriceInput < 0)
				MessageBox.Show("Цена не является положительным целым числом!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(Client.SelectedIndex == -1)
				MessageBox.Show("Клиент не выбран!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(Agent.SelectedIndex == -1)
				MessageBox.Show("Агент не выбран!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(Editable != null){
				Editable.RealEstateSet = RealEstate.SelectedItem as Entities.RealEstateSet;
				Editable.Price = PriceInput;
				Editable.PersonSet_Client = Client.SelectedItem as Entities.PersonSet_Client;
				Editable.PersonSet_Agent = Agent.SelectedItem as Entities.PersonSet_Agent;
				App.Context.SaveChanges();
				MessageBox.Show($"Предложение {Editable.Demo} изменено.", Title, MessageBoxButton.OK, MessageBoxImage.Information);
				Create(null, null);
				UpdateItemsSource();
			}
			else{
				Entities.SupplySet Createable = new Entities.SupplySet{
					RealEstateSet = RealEstate.SelectedItem as Entities.RealEstateSet,
					Price = PriceInput,
					PersonSet_Client = Client.SelectedItem as Entities.PersonSet_Client,
					PersonSet_Agent = Agent.SelectedItem as Entities.PersonSet_Agent
				};
				App.Context.SupplySet.Add(Createable);
				App.Context.SaveChanges();
				MessageBox.Show($"Предложение {Createable.Demo} добавлено.", Title, MessageBoxButton.OK, MessageBoxImage.Information);
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