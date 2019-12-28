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

        /*
         * -Age on other planets
         * -Age in dog years
         * -Time spent ____ (based on averages)
         */
        private const double daysInAYear = 365.25;

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

            const int birthYear = 2017;
            const int birthMonth = 12;
            const int birthDayOfTheMonth = 17;
            const int birthHour = 23; //24 hour time
            const int birthMinute = 57;
            const int birthSecond = 00;

            const string name = "Amara Riley Ikeda";

            mainText.Text = GetDisplayText(name,
                new DateTime(birthYear, birthMonth, birthDayOfTheMonth, birthHour, birthMinute, birthSecond));
        }

        private static string GetDisplayText(string name, DateTime birthday)
        {
            var now = DateTime.Now;
            var nextBirthday = new DateTime(DateTime.Now.Year, birthday.Month, birthday.Day, birthday.Hour,
                birthday.Minute, birthday.Second);
            var lastBirthday = new DateTime(DateTime.Now.Year - 1, birthday.Month, birthday.Day, birthday.Hour,
                birthday.Minute, birthday.Second);

            var halfBirthday = lastBirthday.AddDays(daysInAYear / 2);

            if (DateTime.Now > nextBirthday)
            {
                //Birthday has passed for current year
                halfBirthday = nextBirthday.AddDays(daysInAYear / 2);
                lastBirthday = lastBirthday.AddYears(1);
                nextBirthday = nextBirthday.AddYears(1);
            }


            return name
                   + "\n"
                   + birthday.ToString("dddd, MMMM")
                   + birthday.ToString(" dd") + GetDaySuffix(birthday.Day)
                   + birthday.ToString(", yyyy") + "\n"
                   + birthday.ToString("hh:mm:ss tt") + "\n" + "\n" +
                   $"{CalculateYourAge(birthday)}" + "\n" +
                   "\n" +
                   $"Year of the {GetChineseZodiacAnimal(birthday.Year)}" + "\n" +
                   $"{GetZodiacSign(birthday)}" + "\n" +
                   "\n" +
                   "Age:" +
                   "\n" +
                   $"Years: {Convert.ToDouble((now - birthday).TotalDays / daysInAYear):N3}" +
                   "\n" +
                   $"Months: {Convert.ToDouble(now.Subtract(birthday).Days / (daysInAYear / 12)):N1}" +
                   "\n" +
                   $"Weeks: {Convert.ToDouble((now - birthday).TotalDays / 7):N1}" +
                   "\n" +
                   $"Days: {Convert.ToDouble((now - birthday).TotalDays):N1}" +
                   "\n" +
                   $"Hours: {Convert.ToDouble((now - birthday).TotalHours):N1}" +
                   "\n" +
                   $"Minutes: {Convert.ToDouble((now - birthday).TotalMinutes):N1}" +
                   "\n" +
                   $"Seconds: {Convert.ToDouble((now - birthday).TotalSeconds):N1}" +
                   "\n" +
                   "\n" +
                   $"Days since last birthday: {Convert.ToDouble((now - lastBirthday).TotalDays):N1}" +
                   "\n" +
                   $"Half birthday: {halfBirthday}" +
                   "\n" +
                   "\n" +
                   $"Days until next birthday: {Convert.ToDouble((nextBirthday - now).TotalDays):N1}" + "\n" +
                   $"Next birthday day of the week: {nextBirthday:dddd}" + "\n" +
                   $"% of year: {(now - lastBirthday).TotalDays / daysInAYear * 100:N8}%";
        }

        private static string GetDaySuffix(int day)
        {
            return day switch
            {
                1 => "st",
                21 => "st",
                31 => "st",
                2 => "nd",
                22 => "nd",
                3 => "rd",
                23 => "rd",
                _ => "th"
            };
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

            return
                $"{years} year{IsPluralString(years)}, {months} month{IsPluralString(months)}, {days} day{IsPluralString(days)}, {hours} hour{IsPluralString(hours)}, {minutes} minute{IsPluralString(minutes)}";
        }

        private static string IsPluralString(int value)
        {
            return value != 1 ? "s" : "";
        }

        private static string GetChineseZodiacAnimal(int birthYear)
        {
            if ((birthYear - 1924) % 12 == 0)
                return "Rat";
            if ((birthYear - 1925) % 12 == 0)
                return "Ox";
            if ((birthYear - 1926) % 12 == 0)
                return "Tiger";
            if ((birthYear - 1927) % 12 == 0)
                return "Rabbit";
            if ((birthYear - 1928) % 12 == 0)
                return "Dragon";
            if ((birthYear - 1929) % 12 == 0)
                return "Snake";
            if ((birthYear - 1930) % 12 == 0)
                return "Horse";
            if ((birthYear - 1931) % 12 == 0)
                return "Goat";
            if ((birthYear - 1932) % 12 == 0)
                return "Monkey";
            if ((birthYear - 1933) % 12 == 0)
                return "Rooster";
            if ((birthYear - 1934) % 12 == 0)
                return "Dog";
            return (birthYear - 1945) % 12 == 0 ? "Pig" : "?";
        }

        private static string GetZodiacSign(DateTime birthday)
        {
            var signDateStart = new DateTime(birthday.Year, 1, 20);
            var signDateEnd = new DateTime(birthday.Year, 2, 18);

            if (signDateStart <= birthday && birthday <= signDateEnd)
                return "Aquarius";

            signDateStart = new DateTime(birthday.Year, 2, 19);
            signDateEnd = new DateTime(birthday.Year, 3, 20);

            if (signDateStart <= birthday && birthday <= signDateEnd)
                return "Pisces";

            signDateStart = new DateTime(birthday.Year, 3, 21);
            signDateEnd = new DateTime(birthday.Year, 4, 19);

            if (signDateStart <= birthday && birthday <= signDateEnd)
                return "Aries";

            signDateStart = new DateTime(birthday.Year, 4, 20);
            signDateEnd = new DateTime(birthday.Year, 5, 20);

            if (signDateStart <= birthday && birthday <= signDateEnd)
                return "Taurus";

            signDateStart = new DateTime(birthday.Year, 5, 21);
            signDateEnd = new DateTime(birthday.Year, 6, 20);

            if (signDateStart <= birthday && birthday <= signDateEnd)
                return "Gemini";

            signDateStart = new DateTime(birthday.Year, 6, 21);
            signDateEnd = new DateTime(birthday.Year, 7, 22);

            if (signDateStart <= birthday && birthday <= signDateEnd)
                return "Cancer";

            signDateStart = new DateTime(birthday.Year, 7, 23);
            signDateEnd = new DateTime(birthday.Year, 8, 22);

            if (signDateStart <= birthday && birthday <= signDateEnd)
                return "Leo";

            signDateStart = new DateTime(birthday.Year, 8, 23);
            signDateEnd = new DateTime(birthday.Year, 9, 22);

            if (signDateStart <= birthday && birthday <= signDateEnd)
                return "Virgo";

            signDateStart = new DateTime(birthday.Year, 9, 23);
            signDateEnd = new DateTime(birthday.Year, 10, 22);

            if (signDateStart <= birthday && birthday <= signDateEnd)
                return "Libra";

            signDateStart = new DateTime(birthday.Year, 10, 23);
            signDateEnd = new DateTime(birthday.Year, 11, 21);

            if (signDateStart <= birthday && birthday <= signDateEnd)
                return "Scorpio";

            signDateStart = new DateTime(birthday.Year, 11, 22);
            signDateEnd = new DateTime(birthday.Year, 12, 21);

            if (signDateStart <= birthday && birthday <= signDateEnd)
                return "Sagittarius";

            //signDateStart = new DateTime(birthday.Year, 11, 22);
            //signDateEnd = new DateTime(birthday.Year, 12, 21);

            //if (signDateStart <= birthday && birthday <= signDateEnd)
            return "Capricorn";

            //return "?";
        }
    }
}

