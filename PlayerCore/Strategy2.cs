using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlayerCore.Interfaces;
using Shared.Components.Fields;
using Shared.Interfaces;
using Shared.Interfaces.Proxies;
using System.Threading;
using System.Threading.Tasks;
using Shared.DTO.Communication;

namespace PlayerCore
{
    class Strategy2
    {
        private readonly IServerProxy serverProxy;

        public State State { get; }
        public Strategy2(IServerProxy communicationServerProxy, State state)
        {
            serverProxy = communicationServerProxy;
            State = state;
        }

        private async Task SendMessage<Rt>(Rt gameMessage, CancellationToken cancellationToken)
           where Rt : GameMessage
        {
            cancellationToken.ThrowIfCancellationRequested();
            gameMessage.GameId = State.GameId;
            gameMessage.PlayerGuid = State.Guid;
            await serverProxy.SendAsync(gameMessage, cancellationToken).ConfigureAwait(false);
        }

        public async Task MoveUp(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            MovedBackwards = false;
            if (State.TeamColour == Shared.Enums.TeamColour.Red)
            {
                lastMoveType = Shared.Enums.MoveType.Down;
                await SendMessage(new Move
                {
                    Direction = Shared.Enums.MoveType.Down,
                }, cancellationToken);
            }
            else
            {
                lastMoveType = Shared.Enums.MoveType.Up;
                await SendMessage(new Move
                {
                    Direction = Shared.Enums.MoveType.Up,
                }, cancellationToken);

            }
        }

        public bool MovedBackwards = false;
        public async Task MoveBackwards(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            MovedBackwards = true;

            switch (lastMoveType)
            {
                case Shared.Enums.MoveType.Up:
                    lastMoveType = Shared.Enums.MoveType.Down;
                    await SendMessage(new Move
                    {
                        Direction = Shared.Enums.MoveType.Down,
                    }, cancellationToken).ConfigureAwait(false);
                    return;
                case Shared.Enums.MoveType.Down:
                    lastMoveType = Shared.Enums.MoveType.Up;
                    await SendMessage(new Move
                    {
                        Direction = Shared.Enums.MoveType.Up,
                    }, cancellationToken).ConfigureAwait(false);
                    return;
                case Shared.Enums.MoveType.Left:
                    lastMoveType = Shared.Enums.MoveType.Right;
                    await SendMessage(new Move
                    {
                        Direction = Shared.Enums.MoveType.Right,
                    }, cancellationToken).ConfigureAwait(false);
                    return;
                case Shared.Enums.MoveType.Right:
                    lastMoveType = Shared.Enums.MoveType.Left;
                    await SendMessage(new Move
                    {
                        Direction = Shared.Enums.MoveType.Left,
                    }, cancellationToken).ConfigureAwait(false);
                    return;
            }
        }

        public async Task MoveToTheSameDirection(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            MovedBackwards = false;
            switch (lastMoveType)
            {
                case Shared.Enums.MoveType.Up:

                    if ((State.TeamColour == Shared.Enums.TeamColour.Blue && State.Location.Y == State.Board.Height - State.Board.GoalsHeight - 1) ||
                        (State.TeamColour == Shared.Enums.TeamColour.Red && State.Location.Y == State.Board.Height - State.Board.GoalsHeight - 1))
                    {
                        lastMoveType = Shared.Enums.MoveType.Left;
                        await MoveToTheSameDirection(cancellationToken).ConfigureAwait(false);
                        return;
                    }
                    else
                    {
                        await SendMessage(new Move
                        {
                            Direction = Shared.Enums.MoveType.Up,
                        }, cancellationToken).ConfigureAwait(false);
                        return;
                    }

                case Shared.Enums.MoveType.Down:
                    if ((State.TeamColour == Shared.Enums.TeamColour.Red && State.Location.Y == State.Board.GoalsHeight) ||
                         (State.TeamColour == Shared.Enums.TeamColour.Blue && State.Location.Y == State.Board.GoalsHeight))
                    {
                        lastMoveType = Shared.Enums.MoveType.Left;
                        await MoveToTheSameDirection(cancellationToken).ConfigureAwait(false);
                        return;
                    }
                    else
                    {
                        await SendMessage(new Move
                        {
                            Direction = Shared.Enums.MoveType.Down,
                        }, cancellationToken).ConfigureAwait(false);
                        return;
                    }
                case Shared.Enums.MoveType.Left:
                    if (State.Location.X == 0)
                    {
                        lastMoveType = Shared.Enums.MoveType.Right;
                        await MoveToTheSameDirection(cancellationToken).ConfigureAwait(false);
                        return;
                    }
                    else
                    {
                        await SendMessage(new Move
                        {
                            Direction = Shared.Enums.MoveType.Left,
                        }, cancellationToken).ConfigureAwait(false);
                        return;
                    }
                case Shared.Enums.MoveType.Right:
                    if (State.Location.X == State.Board.Width - 1)
                    {
                        if (State.TeamColour == Shared.Enums.TeamColour.Blue)
                        {
                            lastMoveType = Shared.Enums.MoveType.Down;
                            await MoveToTheSameDirection(cancellationToken).ConfigureAwait(false);
                            return;
                        }
                        else
                        {
                            lastMoveType = Shared.Enums.MoveType.Up;
                            await MoveToTheSameDirection(cancellationToken).ConfigureAwait(false);
                            return;
                        }

                    }
                    else
                    {
                        await SendMessage(new Move
                        {
                            Direction = Shared.Enums.MoveType.Right,
                        }, cancellationToken).ConfigureAwait(false);
                        return;
                    }
            }
        }

        public async Task MoveRight(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (State.Location.X + 1 < State.Board.Width)
            {
                lastMoveType = Shared.Enums.MoveType.Right;
                MovedBackwards = false;
                await SendMessage(new Move
                {
                    Direction = Shared.Enums.MoveType.Right,
                }, cancellationToken).ConfigureAwait(false);
                return;
            }
            else await MoveLeft(cancellationToken).ConfigureAwait(false);
        }

        public async Task MoveLeft(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (State.Location.X - 1 >= 0)
            {
                lastMoveType = Shared.Enums.MoveType.Left;
                MovedBackwards = false;
                await SendMessage(new Move
                {
                    Direction = Shared.Enums.MoveType.Left,
                }, cancellationToken).ConfigureAwait(false);
                return;
            }
            await MoveRight(cancellationToken).ConfigureAwait(false);
        }
        public async Task MoveDown(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            MovedBackwards = false;
            if (State.TeamColour == Shared.Enums.TeamColour.Red)
            {
                lastMoveType = Shared.Enums.MoveType.Up;
                await SendMessage(new Move
                {
                    Direction = Shared.Enums.MoveType.Up,
                }, cancellationToken).ConfigureAwait(false);
                return;
            }
            else
            {
                lastMoveType = Shared.Enums.MoveType.Down;
                await SendMessage(new Move
                {
                    Direction = Shared.Enums.MoveType.Down,
                }, cancellationToken).ConfigureAwait(false);
                return;
            }
        }

        public int LeftBoundaryOfMyGoalFieldSection;
        public int RigthBoundaryOfMyGoalFieldSection;


        public void GetGoalFieldSections()
        {
            int rest;
            int width;
            width = (int)State.Board.Width / State.PlayersMyTeam.Count();
            rest = (int)State.Board.Width % State.PlayersMyTeam.Count();

            int index = -1;
            for (int i = 0; i < State.PlayersMyTeam.Count(); i++)
            {
                if (State.PlayersMyTeam[i].Id == State.Id)
                {
                    index = i;
                    break;
                }
            }

            if (index >= rest)
            {
                LeftBoundaryOfMyGoalFieldSection = rest * (width + 1) + (index - rest) * width;
                RigthBoundaryOfMyGoalFieldSection = rest * (width + 1) + (index - rest) * width + width - 1;
            }
            else
            {
                LeftBoundaryOfMyGoalFieldSection = (width + 1) * (index);
                RigthBoundaryOfMyGoalFieldSection = (width + 1) * (index) + width;
            }

        }

        public int? LastDistanceToPiece = null;

        Shared.Enums.MoveType? lastMoveType = null;

        public bool LastDiscover = false;

        public bool FirstDiscover = true;

        public bool WasLastActionPlace = false;

        Location lastLocation = null;


        public bool IsFieldOnBoard(uint X, uint Y)
        {
            if (State.Board.Width > X && State.Board.Height > Y)
                return true;
            else return false;
        }



        public async Task PerformAction(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var field = State.Field;
            var goalField = field as IGoalField;
            var taskField = field as ITaskField;



            if (LastDiscover == true)
            {
                LastDiscover = false;
            }
            else if (WasLastActionPlace==false && State.HoldingPiece == null && lastLocation != null && lastLocation.X == State.Location.X && lastLocation.Y == State.Location.Y)
            {
                
                if (lastMoveType != null)
                {
                    switch (lastMoveType)
                    {
                        
                        case Shared.Enums.MoveType.Left:
                            if (IsFieldOnBoard(State.Location.X - 1, State.Location.Y) && State.Board.GetField(State.Location.X - 1, State.Location.Y).Player != null)
                            {  
                                State.Board.GetField(State.Location.X - 1, State.Location.Y).Player = null;
                                LastDistanceToPiece = null;
                                lastMoveType = Shared.Enums.MoveType.Up;
                                await MoveToTheSameDirection(cancellationToken).ConfigureAwait(false);
                                return;
                            }
                            break;
                        case Shared.Enums.MoveType.Right:
                            if (IsFieldOnBoard(State.Location.X + 1, State.Location.Y) && State.Board.GetField(State.Location.X + 1, State.Location.Y).Player != null)
                            {
                                State.Board.GetField(State.Location.X + 1, State.Location.Y).Player = null;
                                LastDistanceToPiece = null;
                                lastMoveType = Shared.Enums.MoveType.Down;
                                await MoveToTheSameDirection(cancellationToken).ConfigureAwait(false);
                                return;
                            }
                            break;

                        case Shared.Enums.MoveType.Up:
                            if (IsFieldOnBoard(State.Location.X, State.Location.Y+1) && State.Board.GetField(State.Location.X, State.Location.Y + 1).Player != null)
                            {
                                State.Board.GetField(State.Location.X, State.Location.Y + 1).Player = null;
                                LastDistanceToPiece = null;
                                lastMoveType = Shared.Enums.MoveType.Right;
                                await MoveToTheSameDirection(cancellationToken).ConfigureAwait(false);
                                return;
                            }
                            break;
                        case Shared.Enums.MoveType.Down:
                            if (IsFieldOnBoard(State.Location.X, State.Location.Y-1) && State.Board.GetField(State.Location.X, State.Location.Y - 1).Player != null)
                            {
                                
                                LastDistanceToPiece = null;
                                lastMoveType = Shared.Enums.MoveType.Left;
                                await MoveToTheSameDirection(cancellationToken).ConfigureAwait(false);
                                return;
                            }
                            break;


                    }
                }

                //if (LastMoveType == Shared.Enums.MoveType.Left || LastMoveType == Shared.Enums.MoveType.Right)
                //{
                //    if ((State.TeamColour == Shared.Enums.TeamColour.Blue && State.Location.y == State.Board.GoalsHeight) ||
                //        (State.TeamColour == Shared.Enums.TeamColour.Red && State.Location.y == State.Board.Height - State.Board.GoalsHeight - 1))
                //    {
                //        LastDistanceToPiece = null;
                //    }

                //    return MoveDown();
                //}
                //else
                //{
                //    if (State.Location.x != 0)
                //        return MoveLeft();
                //    else MoveRight();
                //}
            }



			lastLocation = new Location
			{
				X = State.Location.X,
				Y = State.Location.Y
			};

			//fake discover
			if (FirstDiscover == true)
            {
                GetGoalFieldSections();
                FirstDiscover = false;
                await SendMessage(new Discover(), cancellationToken).ConfigureAwait(false);
                return;
            }


            if (State.HoldingPiece == null) // Player does not hold a pece
            {
                if (goalField != null) // player in goalfield
                {
                    WasLastActionPlace = false;
                    await MoveUp(cancellationToken).ConfigureAwait(false);
                    return;
                }
                else
                {
                    if (taskField.DistanceToPiece == 0)
                    {
                        await SendMessage(new PickUpPiece(), cancellationToken).ConfigureAwait(false);
                        return;
                    }
                    else
                    {
                        if (LastDistanceToPiece == null)
                        {
                            LastDistanceToPiece = taskField.DistanceToPiece;
                            //LastMoveType = Shared.Enums.MoveType.Up;
                            await MoveUp(cancellationToken).ConfigureAwait(false);
                            return;

                        }
                        else
                        {
                            if (MovedBackwards)
                            {
                                if ((lastMoveType == Shared.Enums.MoveType.Down && State.TeamColour == Shared.Enums.TeamColour.Blue)
                                    || (lastMoveType == Shared.Enums.MoveType.Up && State.TeamColour == Shared.Enums.TeamColour.Red))
                                {
                                    await MoveLeft(cancellationToken).ConfigureAwait(false); //sprawdzic skraj planszy
                                    return;
                                }
                                else if (lastMoveType == Shared.Enums.MoveType.Right)
                                {
                                    await MoveRight(cancellationToken).ConfigureAwait(false); //sprawdzic skraj planszy
                                    return;
                                }
                                else if (lastMoveType == Shared.Enums.MoveType.Left)
                                {
                                    await MoveDown(cancellationToken).ConfigureAwait(false);
                                    return;
                                }
                                else
                                {
                                    await MoveUp(cancellationToken).ConfigureAwait(false);
                                    return;
                                }
                            }
                            else
                            {
                                if (taskField.DistanceToPiece == LastDistanceToPiece - 1)
                                {
                                    LastDistanceToPiece = taskField.DistanceToPiece;

                                    await MoveToTheSameDirection(cancellationToken).ConfigureAwait(false);
                                    return;
                                }
                                else if (taskField.DistanceToPiece == LastDistanceToPiece + 1)
                                {
                                    await MoveBackwards(cancellationToken).ConfigureAwait(false);
                                    return;
                                }
                                else if (taskField.DistanceToPiece == LastDistanceToPiece)
                                {
                                    LastDistanceToPiece = taskField.DistanceToPiece;
                                    await MoveToTheSameDirection(cancellationToken).ConfigureAwait(false);
                                    return;
                                }
                                else
                                {
                                    LastDiscover = true;
                                    await SendMessage(new Discover(), cancellationToken).ConfigureAwait(false);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            else //HoldingPiece
            {
                //if (State.HoldingPiece.Type == Shared.Enums.PieceType.Sham)
                //{
                //    await SendMessage(new DestroyPiece(), cancellationToken).ConfigureAwait(false);
                //    return;
                //}
                //else
                //{
                    if (State.HoldingPiece.Type == Shared.Enums.PieceType.Unknown && State.TeamColour == Shared.Enums.TeamColour.Blue && State.Board.TasksHeight / 2 + State.Board.GoalsHeight < State.Location.Y)
                    {
                        await SendMessage(new TestPiece(), cancellationToken).ConfigureAwait(false);
                        return;
                    }
                    else if (State.HoldingPiece.Type == Shared.Enums.PieceType.Unknown && State.TeamColour == Shared.Enums.TeamColour.Red && State.Board.TasksHeight / 2 + State.Board.GoalsHeight > State.Location.Y)
                    {
                        await SendMessage(new TestPiece(), cancellationToken).ConfigureAwait(false);
                        return;
                    }
                    else
                    {

                        if (goalField != null) // player in goalfield
                        {
                            uint startY = State.Board.GoalsHeight - 1;
                            uint stopY = 0;
                            if (State.TeamColour == Shared.Enums.TeamColour.Red)
                            {
                                startY = State.Board.Height - 1;
                                stopY = State.Board.Height - State.Board.GoalsHeight + 1;
                            }

                            //IGoalField UnknownGoal;
                            for (int i = (int)startY; i >= (int)stopY; i--)
                            {
                                for (int j = LeftBoundaryOfMyGoalFieldSection; j <= RigthBoundaryOfMyGoalFieldSection; j++)
                                {
                                    var g = State.Board.GetField((uint)j, (uint)i) as IGoalField;
                                    if (g.Type == Shared.Enums.GoalFieldType.Unknown)
                                    {
                                        if (State.Location.Y != i)
                                            await MoveDown(cancellationToken).ConfigureAwait(false);
                                        else if (State.Location.X > j)
                                            await MoveLeft(cancellationToken).ConfigureAwait(false);
                                        else if (State.Location.X < j)
                                            await MoveRight(cancellationToken).ConfigureAwait(false);
                                        else
                                        {
                                            State.HoldingPiece = null;
                                            LastDistanceToPiece = null;
                                            WasLastActionPlace = true;
                                            await SendMessage(new PlacePiece(), cancellationToken).ConfigureAwait(false);
                                        }
                                        return;
                                    }
                                }
                            }

                        }
                        else
                        {
                            if ((uint)LeftBoundaryOfMyGoalFieldSection > State.Location.X)
                            {
                                await MoveRight(cancellationToken).ConfigureAwait(false);
                                return;
                            }
                            else if ((uint)RigthBoundaryOfMyGoalFieldSection < State.Location.X)
                            {
                                await MoveLeft(cancellationToken).ConfigureAwait(false);
                                return;
                            }
                            else
                            {
                                await MoveDown(cancellationToken).ConfigureAwait(false);
                                return;
                            }

                        }
                    }
                //}


            }
        }

    }
}
