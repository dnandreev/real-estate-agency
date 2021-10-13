using System.Linq;
using System.Windows;
using System.Windows.Controls;
namespace RealEstateAgency{
	public partial class ClientsPage : Page{
		Entities.PersonSet_Client Editable;

		void UpdateItemsSource(){
			ClientsList.ItemsSource = App.Context.PersonSet_Client.ToList();
			Editable = null;
		}

		public ClientsPage(){
			InitializeComponent();
			UpdateItemsSource();
		}

		void View(object sender, SelectionChangedEventArgs e){
			if(ClientsList.SelectedItem != null){
				Editable = ClientsList.SelectedItem as Entities.PersonSet_Client;
				Text.Text = Editable.Text;
				FirstName.Text = Editable.PersonSet.FirstName;
				MiddleName.Text = Editable.PersonSet.MiddleName;
				LastName.Text = Editable.PersonSet.LastName;
				Phone.Text = Editable.Phone;
				Email.Text = Editable.Email;
				SuppliesList.ItemsSource = Editable.SupplySet.ToList();
				DemandsList.ItemsSource = Editable.DemandSet.ToList();
			}
			else{
				Editable = null;
				Text.Text = "Выберите клиента";
				FirstName.Text = MiddleName.Text = LastName.Text = Phone.Text = Email.Text = "";
				SuppliesList.ItemsSource = DemandsList.ItemsSource = null;
			}
		}

		void Create(object sender, RoutedEventArgs e){
			Editable = null;
			Text.Text = "Добавьте клиента";
			FirstName.Text = MiddleName.Text = LastName.Text = Phone.Text = Email.Text = "";
			SuppliesList.ItemsSource = DemandsList.ItemsSource = null;
		}

		void Delete(object sender, RoutedEventArgs e){
			if(Editable == null)
				MessageBox.Show("Нечего удалять.", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
			else if(Editable.SupplySet.Count > 0)
				MessageBox.Show($"Клиент {Editable.PersonSet.Demo} связан с предложениями!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(Editable.DemandSet.Count > 0)
				MessageBox.Show($"Клиент {Editable.PersonSet.Demo} связан с потребностями!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(MessageBox.Show($"Подтвердите удаление клиента {Editable.PersonSet.Demo}.", Title, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes){
				string EditablePersonSetDemo = Editable.PersonSet.Demo;
				App.Context.PersonSet_Client.Remove(Editable);
				App.Context.SaveChanges();
				MessageBox.Show($"Клиент {EditablePersonSetDemo} удалён.", Title, MessageBoxButton.OK, MessageBoxImage.Information);
				Create(null, null);
				UpdateItemsSource();
			}
		}

		void Save(object sender, RoutedEventArgs e){
			if(Phone.Text == "" && Email.Text == "")
				MessageBox.Show("Номер телефона и электронная почта не введены!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(Editable != null){
				Editable.PersonSet.FirstName = FirstName.Text;
				Editable.PersonSet.MiddleName = MiddleName.Text;
				Editable.PersonSet.LastName = LastName.Text;
				Editable.Phone = Phone.Text;
				Editable.Email = Email.Text;
				App.Context.SaveChanges();
				MessageBox.Show($"Клиент {Editable.PersonSet.Demo} изменён.", Title, MessageBoxButton.OK, MessageBoxImage.Information);
				Create(null, null);
				UpdateItemsSource();
			}
			else{
				Entities.PersonSet_Client Createable = new Entities.PersonSet_Client{
					PersonSet = new Entities.PersonSet{
						FirstName = FirstName.Text,
						MiddleName = MiddleName.Text,
						LastName = LastName.Text
					},
					Phone = Phone.Text,
					Email = Email.Text
				};
				App.Context.PersonSet_Client.Add(Createable);
				App.Context.SaveChanges();
				MessageBox.Show($"Клиент {Createable.PersonSet.Demo} добавлен.", Title, MessageBoxButton.OK, MessageBoxImage.Information);
				Create(null, null);
				UpdateItemsSource();
			}
		}
	}
}