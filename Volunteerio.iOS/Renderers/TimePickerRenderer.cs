using System;
using UIKit;
using Xamarin.Forms;
using Volunteerio.iOS.Renderers;

[assembly: ExportRenderer(typeof(TimePicker), typeof(TimePickerRenderer))]
namespace Volunteerio.iOS.Renderers
{
    class TimePickerRenderer : Xamarin.Forms.Platform.iOS.TimePickerRenderer
    {
		protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<TimePicker> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				UITextField entry = Control;
				UIDatePicker picker = (UIDatePicker)entry.InputView;
				picker.PreferredDatePickerStyle = UIDatePickerStyle.Wheels;
			}
		}
	}
}