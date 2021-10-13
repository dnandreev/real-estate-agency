using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace RealEstateAgency{
	public partial class MenuPage : Page{
		public MenuPage() => InitializeComponent();

		void Agents(object sender, RoutedEventArgs e) => NavigationService.Navigate(new AgentsPage());

		void Clients(object sender, RoutedEventArgs e) => NavigationService.Navigate(new ClientsPage());

		void RealEstates(object sender, RoutedEventArgs e) => NavigationService.Navigate(new RealEstatesPage());

		void Supplies(object sender, RoutedEventArgs e) => NavigationService.Navigate(new SuppliesPage());

		void Demands(object sender, RoutedEventArgs e) => NavigationService.Navigate(new DemandsPage());

		void Deals(object sender, RoutedEventArgs e) => NavigationService.Navigate(new DealsPage());
	}
}