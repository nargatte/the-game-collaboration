﻿using Shared.Components.Fields;
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
		int? FieldDistance { get; }
		Brush PieceBackground { get; }
		int? PieceId { get; }
	}
	class FieldVM : ViewModel, IFieldVM
	{
		#region IFieldVM
		public virtual void Update( IField field )
		{
			if( field is ITaskField taskField )
			{
				FieldBorder = Brushes.Black;
				FieldBackground = Brushes.White;
				FieldDistance = taskField.DistanceToPiece;
				if( taskField.Piece != null )
				{
					switch( taskField.Piece.Type )
					{
					case PieceType.Unknown:
						PieceBackground = Brushes.Gray;
						break;
					case PieceType.Sham:
						PieceBackground = Brushes.Green;
						break;
					case PieceType.Normal:
						PieceBackground = Brushes.Orange;
						break;
					}
					PieceId = ( int )taskField.Piece.Id;
				}
				else
				{
					PieceBackground = Brushes.Transparent;
					PieceId = null;
				}
			}
			else if( field is IGoalField goalField )
			{
				FieldDistance = null;
				PieceBackground = Brushes.Transparent;
				PieceId = null;
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
		private int? fieldDistance;
		public virtual int? FieldDistance
		{
			get => fieldDistance;
			protected set => SetProperty( ref fieldDistance, value );
		}
		private Brush pieceBackground;
		public virtual Brush PieceBackground
		{
			get => pieceBackground;
			protected set => SetProperty( ref pieceBackground, value );
		}
		private int? pieceId;
		public virtual int? PieceId
		{
			get => pieceId;
			protected set => SetProperty( ref pieceId, value );
		}
		#endregion
		#region FieldVM
		public FieldVM()
		{
		}
		#endregion
	}
}
