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
