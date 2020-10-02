using System;
using UIKit;
using Xamarin.Forms;
using Volunteerio.iOS.Renderers;

[assembly: ExportRenderer(typeof(DatePicker), typeof(DatePickerRenderer))]
namespace Volunteerio.iOS.Renderers
{
	class DatePickerRenderer : Xamarin.Forms.Platform.iOS.DatePickerRenderer
	{
		protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<DatePicker> e)
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