using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMessenge.Models;
public enum AccessControlModel
{
    [Display(Name = "Мандатна")]
    MandatoryAccessControl,
    [Display(Name = "Дискреційна")]
    DiscretionaryAccessControl,
}




