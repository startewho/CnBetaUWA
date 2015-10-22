﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Store;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace CnBetaUWA.Controls
{

    [TemplatePart(Name = "ContentControl", Type = typeof(ContentPresenter))]
    [TemplatePart(Name = "contentGrid", Type = typeof(Grid))]
    [TemplatePart(Name = "OpenContent", Type = typeof(Storyboard))]
    [TemplatePart(Name = "CloseContent", Type = typeof(Storyboard))]
    public sealed class SwipeableControl : Control
    {
        public SwipeableControl()
        {
            DefaultStyleKey = typeof(SwipeableControl);
            this.Unloaded += SwipeableControl_Unloaded;
        }

        private void SwipeableControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (contentGrid != null)
            {
              
                contentGrid.ManipulationDelta -= CommentGridManipulationDelta;
                contentGrid.ManipulationCompleted -= CommentGridManipulationCompleted;
            }
        }

        private ContentPresenter contentControl;
        private Grid contentGrid;
        private Storyboard OpenStoryboard;
        private Storyboard CloseStoryboard;
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            contentGrid = GetTemplateChild("contentGrid") as Grid;
            contentControl = GetTemplateChild("ContentControl") as ContentPresenter;
            CloseStoryboard = GetTemplateChild("CloseContent") as Storyboard;
            OpenStoryboard = GetTemplateChild("OpenContent") as Storyboard;
            if (contentGrid != null)
            {
                contentGrid.ManipulationMode=ManipulationModes.TranslateX;
                contentGrid.ManipulationDelta += CommentGridManipulationDelta;
                contentGrid.ManipulationCompleted += CommentGridManipulationCompleted;
            }
        }





        public Control Content
        {
            get { return (Control)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(Control), typeof(SwipeableControl), new PropertyMetadata(0));



        private void CloseCommentGrid()
        {
            CloseStoryboard.Begin();
            
            
        }

        private void OpenCommentGrid()
        {
            OpenStoryboard.Begin();
            //RefreshComment(this, EventArgs.Empty);
           
        }
        private void CommentGridManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            var x = e.Velocities.Linear.X;
            if (x <= -0.1)
            {
                OpenCommentGrid();
            }
            else if (x > -0.1 && x < 0.1)
            {
                var transform = contentGrid.RenderTransform as CompositeTransform;
                if (transform != null && Math.Abs(transform.TranslateX) > 150)
                {
                    OpenCommentGrid();
                }
                else
                {
                    CloseCommentGrid();
                }
            }
            else
            {
                CloseCommentGrid();
            }
        }

        private void CommentGridManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var compositeTransform = contentGrid.RenderTransform as CompositeTransform;
            if (compositeTransform != null)
            {
                var x = compositeTransform.TranslateX + e.Delta.Translation.X;
                if (x < -300)
                {
                    x = -300;
                }
                compositeTransform.TranslateX = x;
            }
        }
    }
}