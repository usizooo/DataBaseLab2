//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataBaseLab2
{
    using System;
    using System.Collections.Generic;
    
    public partial class cats
    {
        public int ID_cats { get; set; }
        public string nickname { get; set; }
        public int age { get; set; }
        public string gender { get; set; }
        public string takehomestatus { get; set; }
        public int ID_catsitter { get; set; }
    
        public virtual catsitter catsitter { private get; set; }
    }
}