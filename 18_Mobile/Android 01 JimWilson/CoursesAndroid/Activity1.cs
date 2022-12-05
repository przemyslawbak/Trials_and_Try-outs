using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;

namespace CoursesAndroid
{
    [Activity(Label = "Courses", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity
    {
        Button buttonPrev;
        Button buttonNext;
        TextView textTitle;
        ImageView imageCourse;
        TextView textDescription;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            buttonPrev = FindViewById<Button>(Resource.Id.buttonPrev);
            buttonNext = FindViewById<Button>(Resource.Id.buttonNext);
            textTitle = FindViewById<TextView>(Resource.Id.textTitle);
            imageCourse = FindViewById<ImageView>(Resource.Id.imageCourse);
            textDescription = FindViewById<TextView>(Resource.Id.textDescription);

            buttonPrev.Click += buttonPrev_Click;
            buttonNext.Click += buttonNext_Click;

        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            textTitle.Text = "Prev Clicked";
            textDescription.Text = "Something that appers when Prev is clicked";
            imageCourse.SetImageResource(Resource.Drawable.ajax);
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            textTitle.Text = "Next Clicked";
            textDescription.Text = "Something that appers when Next is clicked";
            imageCourse.SetImageResource(Resource.Drawable.bootstrap);
        }
    }
}