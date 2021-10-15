using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Patterns.Mvvm.ViewModels
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ModeAttribute : Attribute
    {
        public Mode Mode { get; init; }
        public ModeAttribute(Mode mode)
        {
            Mode = mode;
        }
    }
}
