using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using horus_prueba.Models.Challenges;
using horus_prueba.Views.Templates.Themes;
using Xamarin.Forms;

namespace horus_prueba.Views.Templates
{
    public partial class RetosTemplate : ContentView
    {
        public RetosTemplate()
        {
            InitializeComponent();

            InitRetos();
        }

        public async void InitRetos()
        {
            await Task.Delay(200);

            ResourceDictionary completeTheme = new CompleteTheme();
            ResourceDictionary incompleteTheme = new IncompleteTheme();


            var context = BindingContext as ChallengeModel;
            if(context.IsEqual)
                Resources.Add(completeTheme);
            else
                Resources.Add(incompleteTheme);
        }
    }
}
