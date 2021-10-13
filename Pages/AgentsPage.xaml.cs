using System.Linq;
using System.Windows;
using System.Windows.Controls;
namespace RealEstateAgency{
	public partial class AgentsPage : Page{
		Entities.PersonSet_Agent Editable;

		void UpdateItemsSource(){
			AgentsList.ItemsSource = App.Context.PersonSet_Agent.ToList();
			Editable = null;
		}

		public AgentsPage(){
			InitializeComponent();
			UpdateItemsSource();
		}

		void View(object sender, SelectionChangedEventArgs e){
			if(AgentsList.SelectedItem != null){
				Editable = AgentsList.SelectedItem as Entities.PersonSet_Agent;
				Text.Text = Editable.Text;
				FirstName.Text = Editable.PersonSet.FirstName;
				MiddleName.Text = Editable.PersonSet.MiddleName;
				LastName.Text = Editable.PersonSet.LastName;
				DealShare.Text = Editable.DealShare.ToString();
				SuppliesList.ItemsSource = Editable.SupplySet.ToList();
				DemandsList.ItemsSource = Editable.DemandSet.ToList();
			}
			else{
				Editable = null;
				Text.Text = "Выберите риелтора";
				FirstName.Text = MiddleName.Text = LastName.Text = DealShare.Text = "";
				SuppliesList.ItemsSource = DemandsList.ItemsSource = null;
			}
		}

		void Create(object sender, RoutedEventArgs e){
			Editable = null;
			Text.Text = "Добавьте риелтора";
			FirstName.Text = MiddleName.Text = LastName.Text = DealShare.Text = "";
			SuppliesList.ItemsSource = DemandsList.ItemsSource = null;
		}

		void Delete(object sender, RoutedEventArgs e){
			if(Editable == null)
				MessageBox.Show("Нечего удалять.", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
			else if(Editable.SupplySet.Count > 0)
				MessageBox.Show($"Риелтор {Editable.PersonSet.Demo} связан с предложениями!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(Editable.DemandSet.Count > 0)
				MessageBox.Show($"Риелтор {Editable.PersonSet.Demo} связан с потребностями!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(MessageBox.Show($"Подтвердите удаление риелтора {Editable.PersonSet.Demo}.", Title, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes){
				string EditablePersonSetDemo = Editable.PersonSet.Demo;
				App.Context.PersonSet_Agent.Remove(Editable);
				App.Context.SaveChanges();
				MessageBox.Show($"Риелтор {EditablePersonSetDemo} удалён.", Title, MessageBoxButton.OK, MessageBoxImage.Information);
				Create(null, null);
				UpdateItemsSource();
			}
		}

		void Save(object sender, RoutedEventArgs e){
			int DealShareInput = 0;
			if(FirstName.Text == "" || MiddleName.Text == "" || LastName.Text == "")
				MessageBox.Show("Фамилия, имя или отчество не введены!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(DealShare.Text != "" && (int.TryParse(DealShare.Text, out DealShareInput) == false || DealShareInput < 0 || DealShareInput > 100))
				MessageBox.Show("Доля от комиссии не является целым числом от 0 до 100!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
			else if(Editable != null){
				Editable.PersonSet.FirstName = FirstName.Text;
				Editable.PersonSet.MiddleName = MiddleName.Text;
				Editable.PersonSet.LastName = LastName.Text;
				Editable.DealShare = DealShareInput;
				App.Context.SaveChanges();
				MessageBox.Show($"Риелтор {Editable.PersonSet.Demo} изменён.", Title, MessageBoxButton.OK, MessageBoxImage.Information);
				Create(null, null);
				UpdateItemsSource();
			}
			else{
				Entities.PersonSet_Agent Createable = new Entities.PersonSet_Agent{
					PersonSet = new Entities.PersonSet{
						FirstName = FirstName.Text,
						MiddleName = MiddleName.Text,
						LastName = LastName.Text
					},
					DealShare = DealShareInput
				};
				App.Context.PersonSet_Agent.Add(Createable);
				App.Context.SaveChanges();
				MessageBox.Show($"Риелтор {Createable.PersonSet.Demo} добавлен.", Title, MessageBoxButton.OK, MessageBoxImage.Information);
				Create(null, null);
				UpdateItemsSource();
			}
		}
	}
}