using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Components.Fields;
using Shared.Interfaces;
using Shared.Messages.Communication;

namespace PlayerCore
{
    class Strategy2 : Shared.Interfaces.IStrategy
    {
        public IGameMaster GameMaster { get; set; }

        public State State { get; }
        public Strategy2(IGameMaster gameMaster, State state)
        {
            GameMaster = gameMaster;
            State = state;
        }

        private Rt SetCommunicationData<Rt>(Rt gameMessage)
           where Rt : GameMessage
        {
            gameMessage.gameId = State.GameId;
            gameMessage.playerGuid = State.Guid;
            return gameMessage;
        }

        public Data MoveUp()
        {
            MovedBackwards = false;
            if (State.TeamColour == Shared.Enums.TeamColour.Red)
            {
                LastMoveType = Shared.Enums.MoveType.Down;
                return GameMaster.PerformMove(SetCommunicationData(new Move
                {
                    direction = Shared.Enums.MoveType.Down,
                    directionSpecified = true
                }));
            }
            else
            {
                LastMoveType = Shared.Enums.MoveType.Up;
                return GameMaster.PerformMove(SetCommunicationData(new Move
                {
                    direction = Shared.Enums.MoveType.Up,
                    directionSpecified = true
                }));

            }
        }

        public bool MovedBackwards = false;
        public Data MoveBackwards()
        {
            MovedBackwards = true;

            switch (LastMoveType)
            {
                case Shared.Enums.MoveType.Up:
                    LastMoveType = Shared.Enums.MoveType.Down;
                    return GameMaster.PerformMove(SetCommunicationData(new Move
                    {
                        direction = Shared.Enums.MoveType.Down,
                        directionSpecified = true
                    }));
                case Shared.Enums.MoveType.Down:
                    LastMoveType = Shared.Enums.MoveType.Up;
                    return GameMaster.PerformMove(SetCommunicationData(new Move
                    {
                        direction = Shared.Enums.MoveType.Up,
                        directionSpecified = true
                    }));
                case Shared.Enums.MoveType.Left:
                    LastMoveType = Shared.Enums.MoveType.Right;
                    return GameMaster.PerformMove(SetCommunicationData(new Move
                    {
                        direction = Shared.Enums.MoveType.Right,
                        directionSpecified = true
                    }));
                case Shared.Enums.MoveType.Right:
                    LastMoveType = Shared.Enums.MoveType.Left;
                    return GameMaster.PerformMove(SetCommunicationData(new Move
                    {
                        direction = Shared.Enums.MoveType.Left,
                        directionSpecified = true
                    }));
                default: return null;
            }
        }

        public Data MoveToTheSameDirection()
        {
            MovedBackwards = false;
            switch (LastMoveType)
            {
                case Shared.Enums.MoveType.Up:
                    //LastMoveType = Shared.Enums.MoveType.Up;
                    return GameMaster.PerformMove(SetCommunicationData(new Move
                    {
                        direction = Shared.Enums.MoveType.Up,
                        directionSpecified = true
                    }));
                case Shared.Enums.MoveType.Down:
                    //LastMoveType = Shared.Enums.MoveType.Down;
                    return GameMaster.PerformMove(SetCommunicationData(new Move
                    {
                        direction = Shared.Enums.MoveType.Down,
                        directionSpecified = true
                    }));
                case Shared.Enums.MoveType.Left:
                    return GameMaster.PerformMove(SetCommunicationData(new Move
                    {
                        direction = Shared.Enums.MoveType.Left,
                        directionSpecified = true
                    }));
                case Shared.Enums.MoveType.Right:
                    return GameMaster.PerformMove(SetCommunicationData(new Move
                    {
                        direction = Shared.Enums.MoveType.Right,
                        directionSpecified = true
                    }));
                default: return null;
            }
        }

        public Data MoveRight()
        {
            LastMoveType = Shared.Enums.MoveType.Right;
            MovedBackwards = false;
            return GameMaster.PerformMove(SetCommunicationData(new Move
            {
                direction = Shared.Enums.MoveType.Right,
                directionSpecified = true
            }));
        }

        public Data MoveLeft()
        {
            LastMoveType = Shared.Enums.MoveType.Left;
            MovedBackwards = false;
            return GameMaster.PerformMove(SetCommunicationData(new Move
            {
                direction = Shared.Enums.MoveType.Left,
                directionSpecified = true
            }));
        }
        public Data MoveDown()
        {
            MovedBackwards = false;
            if (State.TeamColour == Shared.Enums.TeamColour.Red)
            {
                LastMoveType = Shared.Enums.MoveType.Up;
                return GameMaster.PerformMove(SetCommunicationData(new Move
                {
                    direction = Shared.Enums.MoveType.Up,
                    directionSpecified = true
                }));
            }
            else
            {
                LastMoveType = Shared.Enums.MoveType.Down;
                return GameMaster.PerformMove(SetCommunicationData(new Move
                {
                    direction = Shared.Enums.MoveType.Down,
                    directionSpecified = true
                }));

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
                if(State.PlayersMyTeam[i].id==State.Id)
                {
                    index = i;
                    break;
                }
            }

            if(index>=rest)
            {
                LeftBoundaryOfMyGoalFieldSection = rest * (width + 1) + (index - rest) * width;
                RigthBoundaryOfMyGoalFieldSection = rest * (width + 1) + (index - rest) * width + width -1;
            }
            else
            {
                LeftBoundaryOfMyGoalFieldSection = (width+1) * (index);
                RigthBoundaryOfMyGoalFieldSection = (width + 1) * (index) + width;
            }

        }

        public int? LastDistanceToPiece = null;
        Shared.Enums.MoveType? LastMoveType = null;


        public bool LastDiscover = false;

        public bool FirstDiscover = true;



        public Data PerformAction()
        {
            IField field = State.Field;
            var goalField = field as IGoalField;
            var taskField = field as ITaskField;

            //fake discover
            if(FirstDiscover==true)
            {
                GetGoalFieldSections();
                FirstDiscover = false;
                return GameMaster.PerformDiscover(SetCommunicationData(new Discover()));
            }


            if (State.HoldingPiece == null) // Player does not hold a pece
            {
                if (goalField != null) // player in goalfield
                {
                    return MoveUp();
                }
                else
                {
                    if (taskField.DistanceToPiece == 0)
                    {
                        return GameMaster.PerformPickUp(SetCommunicationData(new PickUpPiece()));
                    }
                    else
                    {
                        if (LastDistanceToPiece == null)
                        {
                            LastDistanceToPiece = taskField.DistanceToPiece;
                            LastMoveType = Shared.Enums.MoveType.Up;
                            return MoveUp();

                        }
                        else
                        {
                            if (MovedBackwards)
                            {
                                if ((LastMoveType == Shared.Enums.MoveType.Down && State.TeamColour == Shared.Enums.TeamColour.Blue)
                                    || (LastMoveType == Shared.Enums.MoveType.Up && State.TeamColour == Shared.Enums.TeamColour.Red))
                                {
                                    return MoveLeft(); //sprawdzic skraj planszy
                                }
                                else if (LastMoveType == Shared.Enums.MoveType.Right)
                                {
                                    return MoveRight(); //sprawdzic skraj planszy
                                }
                                else if (LastMoveType == Shared.Enums.MoveType.Left)
                                {
                                    return MoveDown();
                                }
                                else
                                {
                                    return MoveUp();
                                }
                            }
                            else
                            {
                                if (taskField.DistanceToPiece == LastDistanceToPiece - 1)
                                {
                                    LastDistanceToPiece = taskField.DistanceToPiece;

                                    return MoveToTheSameDirection();
                                }
                                else if (taskField.DistanceToPiece == LastDistanceToPiece + 1)
                                {
                                    return MoveBackwards();
                                }
                                else if (taskField.DistanceToPiece == LastDistanceToPiece)
                                {
                                    LastDistanceToPiece = taskField.DistanceToPiece;
                                    return MoveToTheSameDirection();
                                }
                                else
                                {
                                    LastDiscover = true;
                                    return GameMaster.PerformDiscover(SetCommunicationData(new Discover()));
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
                    if (State.HoldingPiece.type== Shared.Enums.PieceType.Unknown && State.TeamColour == Shared.Enums.TeamColour.Blue && State.Board.TasksHeight / 2 + State.Board.GoalsHeight < State.Location.y)
                    {
                        return GameMaster.PerformTestPiece(SetCommunicationData(new TestPiece()));
                    }
                    else if (State.HoldingPiece.type == Shared.Enums.PieceType.Unknown && State.TeamColour == Shared.Enums.TeamColour.Red && State.Board.TasksHeight / 2 + State.Board.GoalsHeight > State.Location.y)
                    {
                        return GameMaster.PerformTestPiece(SetCommunicationData(new TestPiece()));
                    }
                    else
                    {

                        if (goalField != null) // player in goalfield
                        {
                            uint startY = State.Board.GoalsHeight - 1;
                            uint stopY = 0;
                            if (State.TeamColour == Shared.Enums.TeamColour.Red)
                            {
                                startY = State.Board.Height;
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
                                            return MoveDown();
                                        else if (State.Location.x > j)
                                            return MoveLeft();
                                        else if (State.Location.x < j)
                                            return MoveRight();
                                        else
                                        {
                                            State.HoldingPiece = null;
                                            LastDistanceToPiece = null;
                                            return GameMaster.PerformPlace(SetCommunicationData(new PlacePiece()));
                                        }

                                    }
                                }
                            }
                            
                        }
                        else
                        {
                            if ((uint)LeftBoundaryOfMyGoalFieldSection > State.Location.x)
                            {
                                return MoveRight();
                            }
                            else if ((uint)RigthBoundaryOfMyGoalFieldSection < State.Location.x)
                            {
                                return MoveLeft();
                            }
                            else
                            {
                                return MoveDown();
                            }

                        }
                    }
                }
                

            }
            return null;
        }
    }
}
