using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XFTest.ViewModels;

namespace XFTest.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Calendar : ContentView
    {
        public Calendar()
        {
            InitializeComponent();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (this.BindingContext != null)
            {
                CleaningListViewModel cleaningListViewModel = this.BindingContext as CleaningListViewModel;
                cleaningListViewModel.State = width < 280 ? "Small" : width == 320 ? "Small" : width < 380 ? "Medium" : "Large";
            }


            //VisualStateManager.GoToState(dayLbl01, state);
            /*VisualStateManager.GoToState(dayLbl02, state);
            VisualStateManager.GoToState(dayLbl03, state);
            VisualStateManager.GoToState(dayLbl04, state);
            VisualStateManager.GoToState(dayLbl05, state);
            VisualStateManager.GoToState(dayLbl06, state);
            VisualStateManager.GoToState(dayLbl07, state);
            VisualStateManager.GoToState(dayLbl08, state);
            VisualStateManager.GoToState(dayLbl09, state);
            VisualStateManager.GoToState(dayLbl10, state);
            VisualStateManager.GoToState(dayLbl11, state);
            VisualStateManager.GoToState(dayLbl12, state);
            VisualStateManager.GoToState(dayLbl13, state);
            VisualStateManager.GoToState(dayLbl14, state);*/


            //VisualStateManager.GoToState(dateLbl01, state);
            /*VisualStateManager.GoToState(dateLbl02, state);
            VisualStateManager.GoToState(dateLbl03, state);
            VisualStateManager.GoToState(dateLbl04, state);
            VisualStateManager.GoToState(dateLbl05, state);
            VisualStateManager.GoToState(dateLbl06, state);
            VisualStateManager.GoToState(dateLbl07, state);
            VisualStateManager.GoToState(dateLbl08, state);
            VisualStateManager.GoToState(dateLbl09, state);
            VisualStateManager.GoToState(dateLbl10, state);
            VisualStateManager.GoToState(dateLbl11, state);
            VisualStateManager.GoToState(dateLbl12, state);
            VisualStateManager.GoToState(dateLbl13, state);
            VisualStateManager.GoToState(dateLbl14, state);*/


            //VisualStateManager.GoToState(calPad01, state);
            /*VisualStateManager.GoToState(calPad02, state);
            VisualStateManager.GoToState(calPad03, state);
            VisualStateManager.GoToState(calPad04, state);
            VisualStateManager.GoToState(calPad05, state);
            VisualStateManager.GoToState(calPad06, state);
            VisualStateManager.GoToState(calPad07, state);
            VisualStateManager.GoToState(calPad08, state);
            VisualStateManager.GoToState(calPad09, state);
            VisualStateManager.GoToState(calPad10, state);
            VisualStateManager.GoToState(calPad11, state);
            VisualStateManager.GoToState(calPad12, state);
            VisualStateManager.GoToState(calPad13, state);
            VisualStateManager.GoToState(calPad14, state);*/
        }

    }
}