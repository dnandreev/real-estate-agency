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
    
    public partial class RealEstateSet_House
    {
        public int TotalFloors { get; set; }
        public double TotalArea { get; set; }
        public int Id { get; set; }
    
        public virtual RealEstateSet RealEstateSet { get; set; }
    }
}
