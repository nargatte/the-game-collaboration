using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlayerCore.Interfaces;
using Shared.Components.Fields;
using Shared.Interfaces;
using Shared.Messages.Communication;
using Shared.Interfaces.Proxies;
using System.Threading;
using System.Threading.Tasks;

namespace PlayerCore
{
    class Strategy2
    {
        private readonly IServerProxy ServerProxy;

        public State State { get; }
        public Strategy2(IServerProxy communicationServerProxy, State state)
        {
            ServerProxy = communicationServerProxy;
            State = state;
        }

        private async Task SendMessage<Rt>(Rt gameMessage, CancellationToken cancellationToken)
           where Rt : GameMessage
        {
            cancellationToken.ThrowIfCancellationRequested();
            gameMessage.gameId = State.GameId;
            gameMessage.playerGuid = State.Guid;
            await ServerProxy.SendAsync(gameMessage, cancellationToken).ConfigureAwait(false);
        }

        public async Task MoveUp(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            MovedBackwards = false;
            if (State.TeamColour == Shared.Enums.TeamColour.Red)
            {
                LastMoveType = Shared.Enums.MoveType.Down;
                await SendMessage(new Move
                {
                    direction = Shared.Enums.MoveType.Down,
                    directionSpecified = true
                }, cancellationToken);
            }
            else
            {
                LastMoveType = Shared.Enums.MoveType.Up;
                await SendMessage(new Move
                {
                    direction = Shared.Enums.MoveType.Up,
                    directionSpecified = true
                }, cancellationToken);

            }
        }

        public bool MovedBackwards = false;
        public async Task MoveBackwards(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            MovedBackwards = true;

            switch (LastMoveType)
            {
                case Shared.Enums.MoveType.Up:
                    LastMoveType = Shared.Enums.MoveType.Down;
                    await SendMessage(new Move
                    {
                        direction = Shared.Enums.MoveType.Down,
                        directionSpecified = true
                    }, cancellationToken).ConfigureAwait(false);
                    return;
                case Shared.Enums.MoveType.Down:
                    LastMoveType = Shared.Enums.MoveType.Up;
                    await SendMessage(new Move
                    {
                        direction = Shared.Enums.MoveType.Up,
                        directionSpecified = true
                    }, cancellationToken).ConfigureAwait(false);
                    return;
                case Shared.Enums.MoveType.Left:
                    LastMoveType = Shared.Enums.MoveType.Right;
                    await SendMessage(new Move
                    {
                        direction = Shared.Enums.MoveType.Right,
                        directionSpecified = true
                    }, cancellationToken).ConfigureAwait(false);
                    return;
                case Shared.Enums.MoveType.Right:
                    LastMoveType = Shared.Enums.MoveType.Left;
                    await SendMessage(new Move
                    {
                        direction = Shared.Enums.MoveType.Left,
                        directionSpecified = true
                    }, cancellationToken).ConfigureAwait(false);
                    return;
            }
        }

        public async Task MoveToTheSameDirection(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            MovedBackwards = false;
            switch (LastMoveType)
            {
                case Shared.Enums.MoveType.Up:

                    if ((State.TeamColour == Shared.Enums.TeamColour.Blue && State.Location.y == State.Board.Height - State.Board.GoalsHeight - 1) ||
                        (State.TeamColour == Shared.Enums.TeamColour.Red && State.Location.y == State.Board.Height - State.Board.GoalsHeight - 1))
                    {
                        LastMoveType = Shared.Enums.MoveType.Left;
                        await MoveToTheSameDirection(cancellationToken).ConfigureAwait(false);
                        return;
                    }
                    else
                    {
                        await SendMessage(new Move
                        {
                            direction = Shared.Enums.MoveType.Up,
                            directionSpecified = true
                        }, cancellationToken).ConfigureAwait(false);
                        return;
                    }

                case Shared.Enums.MoveType.Down:
                    if ((State.TeamColour == Shared.Enums.TeamColour.Red && State.Location.y == State.Board.GoalsHeight) ||
                         (State.TeamColour == Shared.Enums.TeamColour.Blue && State.Location.y == State.Board.GoalsHeight))
                    {
                        LastMoveType = Shared.Enums.MoveType.Left;
                        await MoveToTheSameDirection(cancellationToken).ConfigureAwait(false);
                        return;
                    }
                    else
                    {
                        await SendMessage(new Move
                        {
                            direction = Shared.Enums.MoveType.Down,
                            directionSpecified = true
                        }, cancellationToken).ConfigureAwait(false);
                        return;
                    }
                case Shared.Enums.MoveType.Left:
                    if (State.Location.x == 0)
                    {
                        LastMoveType = Shared.Enums.MoveType.Right;
                        await MoveToTheSameDirection(cancellationToken).ConfigureAwait(false);
                        return;
                    }
                    else
                    {
                        await SendMessage(new Move
                        {
                            direction = Shared.Enums.MoveType.Left,
                            directionSpecified = true
                        }, cancellationToken).ConfigureAwait(false);
                        return;
                    }
                case Shared.Enums.MoveType.Right:
                    if (State.Location.x == State.Board.Width - 1)
                    {
                        if (State.TeamColour == Shared.Enums.TeamColour.Blue)
                        {
                            LastMoveType = Shared.Enums.MoveType.Down;
                            await MoveToTheSameDirection(cancellationToken).ConfigureAwait(false);
                            return;
                        }
                        else
                        {
                            LastMoveType = Shared.Enums.MoveType.Up;
                            await MoveToTheSameDirection(cancellationToken).ConfigureAwait(false);
                            return;
                        }

                    }
                    else
                    {
                        await SendMessage(new Move
                        {
                            direction = Shared.Enums.MoveType.Right,
                            directionSpecified = true
                        }, cancellationToken).ConfigureAwait(false);
                        return;
                    }
            }
        }

        public async Task MoveRight(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (State.Location.x + 1 < State.Board.Width)
            {
                LastMoveType = Shared.Enums.MoveType.Right;
                MovedBackwards = false;
                await SendMessage(new Move
                {
                    direction = Shared.Enums.MoveType.Right,
                    directionSpecified = true
                }, cancellationToken).ConfigureAwait(false);
                return;
            }
            else await MoveLeft(cancellationToken).ConfigureAwait(false);
        }

        public async Task MoveLeft(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (State.Location.x - 1 >= 0)
            {
                LastMoveType = Shared.Enums.MoveType.Left;
                MovedBackwards = false;
                await SendMessage(new Move
                {
                    direction = Shared.Enums.MoveType.Left,
                    directionSpecified = true
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
                LastMoveType = Shared.Enums.MoveType.Up;
                await SendMessage(new Move
                {
                    direction = Shared.Enums.MoveType.Up,
                    directionSpecified = true
                }, cancellationToken).ConfigureAwait(false);
                return;
            }
            else
            {
                LastMoveType = Shared.Enums.MoveType.Down;
                await SendMessage(new Move
                {
                    direction = Shared.Enums.MoveType.Down,
                    directionSpecified = true
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
                if (State.PlayersMyTeam[i].id == State.Id)
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

        Shared.Enums.MoveType? LastMoveType = null;

        public bool LastDiscover = false;

        public bool FirstDiscover = true;

        public bool WasLastActionPlace = false;

        Location LastLocation = null;






        public async Task PerformAction(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            IField field = State.Field;
            var goalField = field as IGoalField;
            var taskField = field as ITaskField;



            if (LastDiscover == true)
            {
                LastDiscover = false;
            }
            else if (WasLastActionPlace==false && State.HoldingPiece == null && LastLocation != null && LastLocation.x == State.Location.x && LastLocation.y == State.Location.y)
            {
                
                if (LastMoveType != null)
                {
                    switch (LastMoveType)
                    {
                        
                        case Shared.Enums.MoveType.Left:
                            if (State.Board.GetField(State.Location.x - 1, State.Location.y).Player != null)
                            {  
                                State.Board.GetField(State.Location.x - 1, State.Location.y).Player = null;
                                LastDistanceToPiece = null;
                                LastMoveType = Shared.Enums.MoveType.Up;
                                await MoveToTheSameDirection(cancellationToken).ConfigureAwait(false);
                                return;
                            }
                            break;
                        case Shared.Enums.MoveType.Right:
                            if (State.Board.GetField(State.Location.x + 1, State.Location.y).Player != null)
                            {
                                State.Board.GetField(State.Location.x + 1, State.Location.y).Player = null;
                                LastDistanceToPiece = null;
                                LastMoveType = Shared.Enums.MoveType.Down;
                                await MoveToTheSameDirection(cancellationToken).ConfigureAwait(false);
                                return;
                            }
                            break;

                        case Shared.Enums.MoveType.Up:
                            if (State.Board.GetField(State.Location.x, State.Location.y + 1).Player != null)
                            {
                                State.Board.GetField(State.Location.x, State.Location.y + 1).Player = null;
                                LastDistanceToPiece = null;
                                LastMoveType = Shared.Enums.MoveType.Right;
                                await MoveToTheSameDirection(cancellationToken).ConfigureAwait(false);
                                return;
                            }
                            break;
                        case Shared.Enums.MoveType.Down:
                            if (State.Board.GetField(State.Location.x, State.Location.y - 1).Player != null)
                            {
                                
                                LastDistanceToPiece = null;
                                LastMoveType = Shared.Enums.MoveType.Left;
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



            LastLocation = new Location();
            LastLocation.x = State.Location.x;
            LastLocation.y = State.Location.y;

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
                                if ((LastMoveType == Shared.Enums.MoveType.Down && State.TeamColour == Shared.Enums.TeamColour.Blue)
                                    || (LastMoveType == Shared.Enums.MoveType.Up && State.TeamColour == Shared.Enums.TeamColour.Red))
                                {
                                    await MoveLeft(cancellationToken).ConfigureAwait(false); //sprawdzic skraj planszy
                                    return;
                                }
                                else if (LastMoveType == Shared.Enums.MoveType.Right)
                                {
                                    await MoveRight(cancellationToken).ConfigureAwait(false); //sprawdzic skraj planszy
                                    return;
                                }
                                else if (LastMoveType == Shared.Enums.MoveType.Left)
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
                if (State.HoldingPiece.type == Shared.Enums.PieceType.Sham)
                {
                    //return GameMaster.Destroy();
                }
                else
                {
                    if (State.HoldingPiece.type == Shared.Enums.PieceType.Unknown && State.TeamColour == Shared.Enums.TeamColour.Blue && State.Board.TasksHeight / 2 + State.Board.GoalsHeight < State.Location.y)
                    {
                        await SendMessage(new TestPiece(), cancellationToken).ConfigureAwait(false);
                        return;
                    }
                    else if (State.HoldingPiece.type == Shared.Enums.PieceType.Unknown && State.TeamColour == Shared.Enums.TeamColour.Red && State.Board.TasksHeight / 2 + State.Board.GoalsHeight > State.Location.y)
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
                                        if (State.Location.y != i)
                                            await MoveDown(cancellationToken).ConfigureAwait(false);
                                        else if (State.Location.x > j)
                                            await MoveLeft(cancellationToken).ConfigureAwait(false);
                                        else if (State.Location.x < j)
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
                            if ((uint)LeftBoundaryOfMyGoalFieldSection > State.Location.x)
                            {
                                await MoveRight(cancellationToken).ConfigureAwait(false);
                                return;
                            }
                            else if ((uint)RigthBoundaryOfMyGoalFieldSection < State.Location.x)
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
                }


            }
        }

    }
}
