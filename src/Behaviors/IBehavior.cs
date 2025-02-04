using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Behaviors;

namespace InternationalPhoneEntry.Behaviors
{
    internal interface IBehavior<TView> : ICommunityToolkitBehavior<TView> where TView : Element
    {
        internal void InitializeBehavior(TView bindable)
        {
            View = bindable;
            bindable.PropertyChanged += OnViewPropertyChanged;
        }

        internal void UninitializeBehavior(TView bindable)
        {
            bindable.PropertyChanged -= OnViewPropertyChanged;
            View = null;
        }
    }
}
