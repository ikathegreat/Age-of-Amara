using System;
using System.Timers;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Environment = Android.OS.Environment;

namespace AgeOfAmara
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        //https://docs.microsoft.com/en-us/xamarin/android/get-started/installation/android-emulator/device-manager?tabs=windows&pivots=windows
        private Timer timer;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            //Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            //SetSupportActionBar(toolbar);

            //FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            //fab.Click += FabOnClick;

            CalculateBirthdayInfo();

            timer = new Timer(1000) { AutoReset = true, Enabled = true };
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            RunOnUiThread(CalculateBirthdayInfo);
        }

        protected override void OnResume()
        {
            base.OnResume();
            CalculateBirthdayInfo();
        }
        private void CalculateBirthdayInfo()
        {
            var mainText = FindViewById<TextView>(Resource.Id.main_text);

            var birthday = new DateTime(2017, 12, 17, 23, 57, 00);
            var nextBirthday = new DateTime(DateTime.Now.Year, 12, 17, 23, 57, 00);
            var lastBirthday = new DateTime(DateTime.Now.Year - 1, 12, 17, 23, 57, 00);

            var now = DateTime.Now;

            mainText.Text = "Amara Riley Ikeda" + "\n"
                                                + birthday.ToString("dddd, MMMM")
                                                + birthday.ToString(" dd") + GetDaySuffix(birthday.Day)
                                                + birthday.ToString(", yyyy") + "\n"
                                                + birthday.ToString("hh:mm:ss tt") + "\n" + "\n" +
                                                $"{CalculateYourAge(birthday)}" + "\n" +
                                                "\n" +
                                                "Year of the Rooster" + "\n" +
                                                "Sagittarius: The Archer" + "\n" +
                                                "\n" +
                                                $"Age in Months: {Convert.ToInt32(now.Subtract(birthday).Days / (365.25 / 12)):N0}" +
                                                "\n" +
                                                $"Age in Weeks: {Convert.ToInt32((now - birthday).TotalDays / 7):N0}" +
                                                "\n" +
                                                $"Age in Days: {Convert.ToInt32((now - birthday).TotalDays):N0}" +
                                                "\n" +
                                                $"Age in Hours: {Convert.ToInt32((now - birthday).TotalHours):N0}" +
                                                "\n" +
                                                $"Age in Minutes: {Convert.ToInt32((now - birthday).TotalMinutes):N0}" +
                                                "\n" +
                                                $"Age in Seconds: {Convert.ToInt32((now - birthday).TotalSeconds):N0}" +
                                                "\n" +
                                                "\n" +
                                                $"Days since last birthday: {Convert.ToInt32((now - lastBirthday).TotalDays)}" +
                                                "\n" +
                                                "\n" +
                                                $"Days until next birthday: {Convert.ToInt32((nextBirthday - now).TotalDays)}" + "\n" +
                                                $"Next birthday day of the week:{nextBirthday:dddd}"+ "\n" +
                                                $"Year % Complete: {((now - lastBirthday).TotalDays/365)*100:N8}%";
        }

        private static string GetDaySuffix(int day)
        {
            switch (day)
            {
                case 1:
                case 21:
                case 31:
                    return "st";
                case 2:
                case 22:
                    return "nd";
                case 3:
                case 23:
                    return "rd";
                default:
                    return "th";
            }
        }



        private static string CalculateYourAge(DateTime dob)
        {
            var now = DateTime.Now;
            var years = new DateTime(DateTime.Now.Subtract(dob).Ticks).Year - 1;
            var pastYearDate = dob.AddYears(years);
            var months = 0;
            for (var i = 1; i <= 12; i++)
            {
                if (pastYearDate.AddMonths(i) == now)
                {
                    months = i;
                    break;
                }

                if (pastYearDate.AddMonths(i) < now) continue;
                months = i - 1;
                break;
            }
            var days = now.Subtract(pastYearDate.AddMonths(months)).Days;
            var hours = now.Subtract(pastYearDate).Hours;
            var minutes = now.Subtract(pastYearDate).Minutes;

            var yearPluralMod = years != 1 ? "s" : "";
            var monthPluralMod = months != 1 ? "s" : "";
            var dayPluralMod = days != 1 ? "s" : "";
            var hourPluralMod = hours != 1 ? "s" : "";
            var minutePluralMod = minutes != 1 ? "s" : "";

            return $"{years} year{yearPluralMod}, {months} month{monthPluralMod}, {days} day{dayPluralMod}, {hours} hour{hourPluralMod}, {minutes} minute{minutePluralMod}";
        }

        //public override bool OnCreateOptionsMenu(IMenu menu)
        //{
        //    MenuInflater.Inflate(Resource.Menu.menu_main, menu);
        //    return true;
        //}

        //public override bool OnOptionsItemSelected(IMenuItem item)
        //{
        //    int id = item.ItemId;
        //    if (id == Resource.Id.action_settings)
        //    {
        //        return true;
        //    }

        //    return base.OnOptionsItemSelected(item);
        //}

        //private void FabOnClick(object sender, EventArgs eventArgs)
        //{
        //    View view = (View) sender;
        //    Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
        //        .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        //}
    }
}

