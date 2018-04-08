using Shared.Components.Fields;
using SingleGame.ViewModels.Base;

namespace SingleGame.ViewModels
{
	interface IFieldVM : IViewModel
	{
		void Update( IField field );
	}
	class FieldVM : ViewModel, IFieldVM
	{
		#region IFieldVM
		public virtual void Update( IField field )
		{

		}
		#endregion
		#region FieldVM
		public FieldVM()
		{
		}
		#endregion
	}
}
