//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RealEstateAgency.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class RealEstateSet
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RealEstateSet()
        {
            this.SupplySet = new HashSet<SupplySet>();
        }
    
        public int Id { get; set; }
        public string Address_City { get; set; }
        public string Address_Street { get; set; }
        public string Address_House { get; set; }
        public string Address_Number { get; set; }
        public double Coordinate_latitude { get; set; }
        public double Coordinate_longitude { get; set; }
    
        public virtual RealEstateSet_Apartment RealEstateSet_Apartment { get; set; }
        public virtual RealEstateSet_House RealEstateSet_House { get; set; }
        public virtual RealEstateSet_Land RealEstateSet_Land { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupplySet> SupplySet { get; set; }
    }
}
