//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace biblioteka
{
    using System;
    using System.Collections.Generic;
    
    public partial class zamowienie
    {
        public int id_zamowienie { get; set; }
        public int id_czytelnik { get; set; }
        public int id_ksiazka { get; set; }
        public Nullable<System.DateTime> data_zwrotu { get; set; }
        public System.DateTimeOffset data_wypozyczenia { get; set; }
        public System.DateTime data_oczekiwana_zwrotu { get; set; }
        public bool oddany { get; set; }
    }
}
