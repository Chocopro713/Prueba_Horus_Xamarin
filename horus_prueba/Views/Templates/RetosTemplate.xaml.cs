using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            // Resources.Clear();
            // Debug.WriteLine("Ingresa!!!");
            // InitRetos();
        }

        /*
        public async void InitRetos()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                // Add delay so I'm sure the binding context changed event has been already fired
                await Task.Delay(150);

                ResourceDictionary completeTheme = new CompleteTheme();
                ResourceDictionary incompleteTheme = new IncompleteTheme();

                var context = BindingContext as ChallengeModel;
                if (context != null && context.IsEqual)
                    Resources.Add(completeTheme);
                else
                    Resources.Add(incompleteTheme);

                Debug.WriteLine(context.IsEqual);
                Debug.WriteLine(context.CurrentPoints.ToString() + " - " + context.TotalPoints.ToString());
            });            
        }
        */
    }
}
