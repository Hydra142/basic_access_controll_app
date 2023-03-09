using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMessenge.Contracts;

public interface IActivationService
{
    Task ActivateAsync();
}

