using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using System.Collections.Generic;
using XamarinLogoQuizApp.Adapter;
using System.Linq;

namespace XamarinLogoQuizApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        public List<string> suggestSource = new List<string>();
        public GridViewAnswerAdapter answerAdapter;
        public GridViewSuggestAdapter suggestAdapter;

        public Button btnSubmit;
        public GridView gVAnswer, gVSuggest;

        public ImageView imgLogo;


        int[] imageList =
        {
            Resource.Drawable.facebook,
            Resource.Drawable.groupme,
            Resource.Drawable.instagram,
            Resource.Drawable.itunes,
            Resource.Drawable.kik,
            Resource.Drawable.linkedin,
            Resource.Drawable.messenger,
            Resource.Drawable.periscope,
            Resource.Drawable.reddit,
            Resource.Drawable.skype,
            Resource.Drawable.snapchat,
            Resource.Drawable.tumblr,
            Resource.Drawable.twitter,
            Resource.Drawable.whatsapp,
            Resource.Drawable.youtube

        };

        public char[] answer;
        string correctAnswer;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            InitViews();
        
        }

        private void InitViews()
        {
            gVAnswer = FindViewById<GridView>(Resource.Id.gVAnswers);
            gVSuggest = FindViewById<GridView>(Resource.Id.gVSuggest);

            imgLogo = FindViewById<ImageView>(Resource.Id.imgLogo);
            SetupList();
            btnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);
            
            btnSubmit.Click += delegate {
                string result = new string(Common.Common.m_UserSubmitAnswer);

                if (result.Equals(correctAnswer))
                {
                    Toast.MakeText(ApplicationContext, "Finish! This is " + result.ToUpper(), ToastLength.Short).Show();

                    //Reset
                    Common.Common.m_UserSubmitAnswer = new char[correctAnswer.Length];

                    //Update
                    GridViewAnswerAdapter answerAdapter = new GridViewAnswerAdapter(SetupNullList(), this);
                    gVAnswer.Adapter = answerAdapter;
                    answerAdapter.NotifyDataSetChanged();

                    GridViewSuggestAdapter suggestAdapter = new GridViewSuggestAdapter(suggestSource, this, this);
                    gVSuggest.Adapter = suggestAdapter;
                    suggestAdapter.NotifyDataSetChanged();

                    SetupList();

                }
                else
                    Toast.MakeText(this, "Incorrect answer..", ToastLength.Short).Show();

            };

        }

        private char[] SetupNullList()
        {
            char[] result = new char[answer.Length];
            return result;
        }

        private void SetupList()
        {
            Random random = new Random();
            int imageSelected = imageList[random.Next(imageList.Length)];
            imgLogo.SetImageResource(imageSelected);

            correctAnswer = Resources.GetResourceName(imageSelected);
            correctAnswer = correctAnswer.Substring(correctAnswer.LastIndexOf("/") + 1);

            answer = correctAnswer.ToCharArray();

            Common.Common.m_UserSubmitAnswer = new char[answer.Length];

            //add answer chars to list

            suggestSource.Clear();
            foreach (char item in answer)
                suggestSource.Add(Convert.ToString(item));

            //random chars from alphabet list and add to list

            for (int i = answer.Length; i < answer.Length*2; i++)
                suggestSource.Add(Common.Common.m_AlphabetCharacters[random.Next(Common.Common.m_AlphabetCharacters.Length)]);


            //sort list

            suggestSource = suggestSource.OrderBy(Xamarin => Guid.NewGuid()).ToList();

            //set adapter for grid view
            answerAdapter = new GridViewAnswerAdapter(SetupNullList(), this);
            suggestAdapter = new GridViewSuggestAdapter(suggestSource, this, this);

            answerAdapter.NotifyDataSetInvalidated();
            suggestAdapter.NotifyDataSetChanged();

            gVAnswer.Adapter = answerAdapter;
            gVSuggest.Adapter = suggestAdapter;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}