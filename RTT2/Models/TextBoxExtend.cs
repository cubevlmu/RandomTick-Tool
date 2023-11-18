using System;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;

namespace RandomTick.Models;

public static class TextBoxExtend
{
    public static void AddFadeAnimation(this TextBlock box)
    {
        var fadeInAnimation = new DoubleTransition
        {
            Duration = TimeSpan.FromMilliseconds(500),
            Easing = Easing.Parse("QuadraticEaseOut")
        };

        var fadeOutAnimation = new DoubleTransition
        {
            Duration = TimeSpan.FromMilliseconds(500),
            Easing = Easing.Parse("QuadraticEaseIn")
        };

        box.Transitions = new Transitions
        {
            fadeInAnimation,
            fadeOutAnimation
        };
    }
    
    public static void UpdateText(this TextBlock box, string newText)
    {
        box.Opacity = 0;

        box.Text = newText;

        box.Opacity = 1;
    }
}