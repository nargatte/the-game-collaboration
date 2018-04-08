using Shared.Components.Fields;
using Shared.Enums;
using SingleGame.ViewModels.Base;
using System.Windows.Media;

namespace SingleGame.ViewModels
{
	interface IFieldVM : IViewModel
	{
		void Update( IField field );
		Brush FieldBackground { get; }
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
				switch( goalField.Team )
				{
				case TeamColour.Blue:
					FieldBorder = Brushes.Blue;
					break;
				case TeamColour.Red:
					FieldBorder = Brushes.Red;
					break;
				}
				switch( goalField.Type )
				{
				case GoalFieldType.Unknown:
					FieldBackground = Brushes.Gray;
					break;
				case GoalFieldType.Goal:
					FieldBackground = Brushes.Yellow;
					break;
				case GoalFieldType.NonGoal:
					FieldBackground = Brushes.White;
					break;
				}
			}
		}
		private Brush fieldBorder;
		public virtual Brush FieldBorder
		{
			get => fieldBorder;
			protected set => SetProperty( ref fieldBorder, value );
		}
		private Brush fieldBackground;
		public virtual Brush FieldBackground
		{
			get => fieldBackground;
			protected set => SetProperty( ref fieldBackground, value );
		}
		#endregion
		#region FieldVM
		public FieldVM()
		{
		}
		#endregion
	}
}
