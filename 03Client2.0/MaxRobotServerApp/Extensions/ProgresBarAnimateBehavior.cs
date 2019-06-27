using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;

namespace MaxRobotServerApp.Extensions
{
    public class ProgresBarAnimateBehavior : Behavior<System.Windows.Controls.ProgressBar>
    {
        private bool _IsAnimating;

        public bool IsAnimating => _IsAnimating;

        protected override void OnAttached()
        {
            base.OnAttached();
            System.Windows.Controls.ProgressBar progressBar = AssociatedObject;
            progressBar.ValueChanged += ProgressBar_ValueChanged;
        }

        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_IsAnimating)
                return;

            _IsAnimating = true;

            DoubleAnimation doubleAnimation = new DoubleAnimation
                (e.OldValue, e.NewValue, new Duration(TimeSpan.FromSeconds(0.3)), FillBehavior.Stop);
            doubleAnimation.Completed += AnimationCompleted;

            ((System.Windows.Controls.ProgressBar)sender).BeginAnimation(RangeBase.ValueProperty, doubleAnimation);

            e.Handled = true;

            void AnimationCompleted(object _, EventArgs __)
            {
                _IsAnimating = false;
            }
        }

        protected override void OnDetaching()
        {
            AssociatedObject.ValueChanged -= ProgressBar_ValueChanged;
            base.OnDetaching();
        }
    }
}
