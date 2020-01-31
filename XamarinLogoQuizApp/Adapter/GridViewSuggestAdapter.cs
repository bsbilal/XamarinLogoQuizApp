using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace XamarinLogoQuizApp.Adapter
{
    public class GridViewSuggestAdapter : BaseAdapter
    {
        private List<string> m_LSuggestSource;
        private Context context;
        private MainActivity mainActivity;

        public GridViewSuggestAdapter(List <string> suggestSource,Context context,MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;
            this.context = context;
            this.m_LSuggestSource = suggestSource;


        }
        public override int Count { get { return m_LSuggestSource.Count; } }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Button button;
            if (convertView == null)

            {
                if (m_LSuggestSource[position].Equals("null"))
                {
                    button = new Button(context);
                    button.LayoutParameters = new GridView.LayoutParams(100, 100);
                    button.SetPadding(12,12, 12, 12);
                    button.SetBackgroundColor(Color.DarkGray);
                }
                else
                {
                    button = new Button(context);
                    button.LayoutParameters = new GridView.LayoutParams(100, 100);
                    button.SetPadding(12,12,12, 12);
                    button.SetBackgroundColor(Color.DarkGray);
                    button.SetTextColor(Color.Yellow);
                    button.Text = m_LSuggestSource[position];
                    button.Click += delegate {

                        string answer = new string(mainActivity.answer);
                        if(answer.Contains(m_LSuggestSource[position]))
                        {
                            char compare = m_LSuggestSource[position][0];

                            for (int i = 0; i < answer.Length; i++)
                            {
                                if (compare == answer[i])
                                    Common.Common.m_UserSubmitAnswer[i] = compare;
                            }
                            //update ui

                            GridViewAnswerAdapter answerAdapter = new GridViewAnswerAdapter(Common.Common.m_UserSubmitAnswer, context);
                            mainActivity.gVAnswer.Adapter = answerAdapter;
                            answerAdapter.NotifyDataSetChanged();

                            //remove chars from suggest source
                            mainActivity.suggestSource[position] = "null";
                            mainActivity.suggestAdapter = new GridViewSuggestAdapter(mainActivity.suggestSource, context, mainActivity);
                            mainActivity.gVSuggest.Adapter = mainActivity.suggestAdapter;
                            mainActivity.suggestAdapter.NotifyDataSetChanged();

                        }
                        else
                        {
                            //remove chars from suggest source
                            mainActivity.suggestSource[position] = "null";
                            mainActivity.suggestAdapter = new GridViewSuggestAdapter(mainActivity.suggestSource, context, mainActivity);
                            mainActivity.gVSuggest.Adapter = mainActivity.suggestAdapter;
                            mainActivity.suggestAdapter.NotifyDataSetChanged();
                        }
                    
                    };

                }
            }
            else
                button = (Button)convertView;
            return button;
            }
    }
}