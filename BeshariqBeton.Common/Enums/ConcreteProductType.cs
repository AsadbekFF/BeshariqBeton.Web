using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeshariqBeton.Common.Enums
{
    public enum ConcreteProductType : byte
    {
        [Display(Name = "Beton 100")]
        Concrete100,
        [Display(Name = "Beton 150")]
        Concrete150,
        [Display(Name = "Beton 200")]
        Concrete200,
        [Display(Name = "Beton 250")]
        Concrete250,
        [Display(Name = "Beton 300")]
        Concrete300,
        [Display(Name = "Beton 350")]
        Concrete350,
        [Display(Name = "Beton 400")]
        Concrete400,
        [Display(Name = "Kolodets")]
        Sump,
        [Display(Name = "Plita")]
        Plate,
        [Display(Name = "Shlakoblok")]
        CinderBlock
    }
}
