using System.Linq;
using System.Windows;
using System.Windows.Controls;
namespace RealEstateAgency{
	public partial class DealsPage : Page{
		Entities.DealSet Editable;

		void UpdateItemsSource(){
			DealsList.ItemsSource = App.Context.DealSet.ToList();
			Supply.ItemsSource = App.Context.SupplySet.ToList();
			Demand.ItemsSource = App.Context.DemandSet.ToList();
			Editable = null;
		}

		public DealsPage(){
			InitializeComponent();
			UpdateItemsSource();
		}

		void View(object sender, SelectionChangedEventArgs e){
			if(DealsList.SelectedItem != null){
				Editable = DealsList.SelectedItem as Entities.DealSet;
				Text.Text = Editable.Demo;
				Supply.SelectedItem = Editable.SupplySet;
				Demand.SelectedItem = Editable.DemandSet;
			}
			else{
				Editable = null;
				Text.Text = "Выберите сделку";
				Supply.SelectedIndex = Demand.SelectedIndex = -1;
			}
		}

		void Create(object sender, RoutedEventArgs e){
			Editable = null;
			Text.Text = "Добавьте сделку";
			Supply.SelectedIndex = Demand.SelectedIndex = -1;
		}

		void Delete(object sender, RoutedEventArgs e){
			if(Editable == null)
				MessageBox.Show("Нечего удалять.", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
			else if(MessageBox.Show($"Подтвердите удаление сделки {Editable.Demo}.", Title, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes){
				string EditableDemo = Editable.Demo;
				App.Context.DealSet.Remove(Editable);
				App.Context.SaveChanges();
				MessageBox.Show($"Сделка {EditableDemo} удалена.", Title, MessageBoxButton.OK, MessageBoxImage.Information);
				Create(null, null);
				UpdateItemsSource();
			}
		}

		void Save(object sender, RoutedEventArgs e){
			if(Supply.SelectedIndex == -1)
				MessageBox.Show("Предложение не выбрано!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(Demand.SelectedIndex == -1)
				MessageBox.Show("Потребность не выбрана!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(Editable != null){
				var SupplySet = Supply.SelectedItem as Entities.SupplySet;
				var DemandSet = Demand.SelectedItem as Entities.DemandSet;
				if(SupplySet != Editable.SupplySet && SupplySet.DealSet.Count > 0)
					MessageBox.Show("Предложение участвует в другой сделке!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
				else if(DemandSet != Editable.DemandSet && DemandSet.DealSet.Count > 0)
					MessageBox.Show("Потребность участвует в другой сделке!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
				else{
					Editable.SupplySet = SupplySet;
					Editable.DemandSet = DemandSet;
					App.Context.SaveChanges();
					MessageBox.Show($"Сделка {Editable.Demo} изменена.", Title, MessageBoxButton.OK, MessageBoxImage.Information);
					Create(null, null);
					UpdateItemsSource();
				}
			}
			else if((Supply.SelectedItem as Entities.SupplySet).DealSet.Count > 0)
				MessageBox.Show("Предложение участвует в другой сделке!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if((Demand.SelectedItem as Entities.DemandSet).DealSet.Count > 0)
					MessageBox.Show("Потребность участвует в другой сделке!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else{
				Entities.DealSet Createable = new Entities.DealSet{
					SupplySet = Supply.SelectedItem as Entities.SupplySet,
					DemandSet = Demand.SelectedItem as Entities.DemandSet
				};
				App.Context.DealSet.Add(Createable);
				App.Context.SaveChanges();
				MessageBox.Show($"Сделка {Createable.Demo} добавлена.", Title, MessageBoxButton.OK, MessageBoxImage.Information);
				Create(null, null);
				UpdateItemsSource();
			}
		}
		void Calculate(object sender, RoutedEventArgs e){
			if(DealsList.SelectedItem == null)
				MessageBox.Show("Сделка не выбрана!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else
				MessageBox.Show($"Комиссии для продавца и покупателя составляют {Editable.SupplyCommission} и {Editable.DemandCommission} соответственно, из которых их риелторы получают {Editable.SupplyDealShare} и {Editable.DemandDealShare} соответственно, а агентство получает {Editable.AgencyGains}.", Title, MessageBoxButton.OK, MessageBoxImage.Information);
		}
	}
}