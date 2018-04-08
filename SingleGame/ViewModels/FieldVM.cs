using Shared.Components.Fields;
using Shared.Enums;
using SingleGame.ViewModels.Base;
using System.Windows.Media;

namespace SingleGame.ViewModels
{
	interface IFieldVM : IViewModel
	{
		void Update( IField field );
		Brush FieldBorder { get; }
	}
	class FieldVM : ViewModel, IFieldVM
	{
		#region IFieldVM
		public virtual void Update( IField field )
		{
			if( field is ITaskField taskField )
			{
				FieldBorder = Brushes.Black;
			}
			else if( field is IGoalField goalField )
			{
				if( goalField.Team == TeamColour.Blue )
					FieldBorder = Brushes.Blue;
				else
					FieldBorder = Brushes.Red;
			}
		}
		private Brush fieldBorder;
		public virtual Brush FieldBorder
		{
			get => fieldBorder;
			protected set => SetProperty( ref fieldBorder, value );
		}
		#endregion
		#region FieldVM
		public FieldVM() => FieldBorder = Brushes.Transparent;
		#endregion
	}
}
