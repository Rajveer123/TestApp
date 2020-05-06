using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XFTest.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Calendar : ContentPage
    {
        public Calendar()
        {
            InitializeComponent();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            var state = width < 280 ? "Small" : width == 320 ? "Small" : width < 380 ? "Medium" : "Large";
            VisualStateManager.GoToState(dayLbl01, state);
            VisualStateManager.GoToState(dayLbl02, state);
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
            VisualStateManager.GoToState(dayLbl14, state);
            /*VisualStateManager.GoToState(dayLbl15, state);
            VisualStateManager.GoToState(dayLbl16, state);
            VisualStateManager.GoToState(dayLbl17, state);
            VisualStateManager.GoToState(dayLbl18, state);
            VisualStateManager.GoToState(dayLbl19, state);
            VisualStateManager.GoToState(dayLbl20, state);
            VisualStateManager.GoToState(dayLbl21, state);
            VisualStateManager.GoToState(dayLbl22, state);
            VisualStateManager.GoToState(dayLbl23, state);
            VisualStateManager.GoToState(dayLbl24, state);
            VisualStateManager.GoToState(dayLbl25, state);
            VisualStateManager.GoToState(dayLbl26, state);
            VisualStateManager.GoToState(dayLbl27, state);
            VisualStateManager.GoToState(dayLbl28, state);
            VisualStateManager.GoToState(dayLbl29, state);
            VisualStateManager.GoToState(dayLbl30, state);
            VisualStateManager.GoToState(dayLbl31, state);*/

            VisualStateManager.GoToState(dateLbl01, state);
            VisualStateManager.GoToState(dateLbl02, state);
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
            VisualStateManager.GoToState(dateLbl14, state);
            /*VisualStateManager.GoToState(dateLbl15, state);
            VisualStateManager.GoToState(dateLbl16, state);
            VisualStateManager.GoToState(dateLbl17, state);
            VisualStateManager.GoToState(dateLbl18, state);
            VisualStateManager.GoToState(dateLbl19, state);
            VisualStateManager.GoToState(dateLbl20, state);
            VisualStateManager.GoToState(dateLbl21, state);
            VisualStateManager.GoToState(dateLbl22, state);
            VisualStateManager.GoToState(dateLbl23, state);
            VisualStateManager.GoToState(dateLbl24, state);
            VisualStateManager.GoToState(dateLbl25, state);
            VisualStateManager.GoToState(dateLbl26, state);
            VisualStateManager.GoToState(dateLbl27, state);
            VisualStateManager.GoToState(dateLbl28, state);
            VisualStateManager.GoToState(dateLbl29, state);
            VisualStateManager.GoToState(dateLbl30, state);
            VisualStateManager.GoToState(dateLbl31, state);*/

            VisualStateManager.GoToState(calPad01, state);
            VisualStateManager.GoToState(calPad02, state);
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
            VisualStateManager.GoToState(calPad14, state);
            /*VisualStateManager.GoToState(calPad15, state);
            VisualStateManager.GoToState(calPad16, state);
            VisualStateManager.GoToState(calPad17, state);
            VisualStateManager.GoToState(calPad18, state);
            VisualStateManager.GoToState(calPad19, state);
            VisualStateManager.GoToState(calPad20, state);
            VisualStateManager.GoToState(calPad21, state);
            VisualStateManager.GoToState(calPad22, state);
            VisualStateManager.GoToState(calPad23, state);
            VisualStateManager.GoToState(calPad24, state);
            VisualStateManager.GoToState(calPad25, state);
            VisualStateManager.GoToState(calPad26, state);
            VisualStateManager.GoToState(calPad27, state);
            VisualStateManager.GoToState(calPad28, state);
            VisualStateManager.GoToState(calPad29, state);
            VisualStateManager.GoToState(calPad30, state);
            VisualStateManager.GoToState(calPad31, state);*/

            /*VisualStateManager.GoToState(calPadInr01, state);
            VisualStateManager.GoToState(calPadInr02, state);
            VisualStateManager.GoToState(calPadInr03, state);
            VisualStateManager.GoToState(calPadInr04, state);
            VisualStateManager.GoToState(calPadInr05, state);
            VisualStateManager.GoToState(calPadInr06, state);
            VisualStateManager.GoToState(calPadInr07, state);
            VisualStateManager.GoToState(calPadInr08, state);
            VisualStateManager.GoToState(calPadInr09, state);
            VisualStateManager.GoToState(calPadInr10, state);
            VisualStateManager.GoToState(calPadInr11, state);
            VisualStateManager.GoToState(calPadInr12, state);
            VisualStateManager.GoToState(calPadInr13, state);
            VisualStateManager.GoToState(calPadInr14, state);
            VisualStateManager.GoToState(calPadInr15, state);
            VisualStateManager.GoToState(calPadInr16, state);
            VisualStateManager.GoToState(calPadInr17, state);
            VisualStateManager.GoToState(calPadInr18, state);
            VisualStateManager.GoToState(calPadInr19, state);
            VisualStateManager.GoToState(calPadInr20, state);
            VisualStateManager.GoToState(calPadInr21, state);
            VisualStateManager.GoToState(calPadInr22, state);
            VisualStateManager.GoToState(calPadInr23, state);
            VisualStateManager.GoToState(calPadInr24, state);
            VisualStateManager.GoToState(calPadInr25, state);
            VisualStateManager.GoToState(calPadInr26, state);
            VisualStateManager.GoToState(calPadInr27, state);
            VisualStateManager.GoToState(calPadInr28, state);
            VisualStateManager.GoToState(calPadInr29, state);
            VisualStateManager.GoToState(calPadInr30, state);
            VisualStateManager.GoToState(calPadInr31, state);*/

        }

    }
}