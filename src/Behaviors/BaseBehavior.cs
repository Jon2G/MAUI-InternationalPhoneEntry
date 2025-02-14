using System.ComponentModel;
using CommunityToolkit.Maui.Behaviors;

namespace InternationalPhoneEntry.Behaviors
{
    public abstract class BaseBehavior<TView> : Behavior<TView>, IBehavior<TView> where TView : VisualElement
    {
        private protected BaseBehavior()
        {

        }

        /// <summary>
        /// View used by the Behavior
        /// </summary>
        protected TView? View { get; private set; }

        TView? ICommunityToolkitBehavior<TView>.View
        {
            get => View;
            set => View = value;
        }

        /// <summary>
        /// Virtual method that executes when a property on the View has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnViewPropertyChanged(TView sender, PropertyChangedEventArgs e)
        {

        }

        /// <inheritdoc/>
        protected override void OnAttachedTo(TView bindable)
        {
            base.OnAttachedTo(bindable);

            ((IBehavior<TView>)this).InitializeBehavior(bindable);
        }

        /// <inheritdoc/>
        protected override void OnDetachingFrom(TView bindable)
        {
            base.OnDetachingFrom(bindable);

            ((IBehavior<TView>)this).UninitializeBehavior(bindable);
        }

        void ICommunityToolkitBehavior<TView>.OnViewPropertyChanged(TView sender, PropertyChangedEventArgs e) => OnViewPropertyChanged(sender, e);
    }
}
