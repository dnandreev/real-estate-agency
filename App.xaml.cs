using System.Windows;

namespace RealEstateAgency{
	public partial class App : Application{
		public static Entities.RealEstateAgencyEntities Context = new Entities.RealEstateAgencyEntities();
	}
}