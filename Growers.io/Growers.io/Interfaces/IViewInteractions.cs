using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: May 14, 2024
 * Course: Application Development III
 * Description: 
 */

namespace Growers.io.Interfaces
{
    /// <summary>
    /// Interface for view interactions. This methods should already be implemented in a .NET MAUI page.
    /// Used for passing useful methods/functions to viewmodels.
    /// </summary>
    public interface IViewInteractions
    {
        public Task DisplayAlert(string title, string message, string cancel);
        public INavigation Navigation { get; }
    }
}
