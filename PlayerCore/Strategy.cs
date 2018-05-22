using Shared.Components.Fields;
using Shared.DTOs.Communication;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerCore
{
    class Strategy: Shared.Interfaces.IStrategy
    {
        public IGameMaster GameMaster { get; set; }

        public State State { get; }

        public Strategy(IGameMaster gameMaster, State state)
        {
            GameMaster = gameMaster;
            State = state;
        }

        private Rt SetCommunicationData<Rt>(Rt gameMessage)
            where Rt: GameMessage
        {
            gameMessage.GameId = State.GameId;
            gameMessage.PlayerGuid = State.Guid;
            return gameMessage;
        }

        const int DiscoveryCountMax = 1;
        private int DiscoveryCount = 0;
        bool LatMoveUp = false;

        private Shared.Enums.MoveType DirectionToPiece()
        {
            var fieldsSortedByTime = State.Board.Fields.OrderByDescending(f => f.Timestamp).OfType<ITaskField>();
            var lastChcekFields = fieldsSortedByTime.Take(State.LastDiscoveryCount);
            if (lastChcekFields.Count() < 4)
                throw new Exception("Not enough fields discovered ");

            uint max_x = lastChcekFields.Max(f => f.X);
            uint max_y = lastChcekFields.Max(f => f.Y);
            uint min_x = lastChcekFields.Min(f => f.X);
            uint min_y = lastChcekFields.Min(f => f.Y);

            IField field = lastChcekFields.OrderBy(f => f.DistanceToPiece).FirstOrDefault();

            if (max_x == field.X && State.Location.x != State.Board.Width-1)
                return Shared.Enums.MoveType.Right;
            if (max_y == field.Y && State.Location.y != State.Board.Height-1)
                return Shared.Enums.MoveType.Up;
            if (min_x == field.X)
                return Shared.Enums.MoveType.Left;
            if (min_y == field.Y)
                return Shared.Enums.MoveType.Down;

            throw new Exception("Cannot decide on where to move");
        }

        private Shared.Enums.MoveType DirectionToUnknownGoal()
        {
            uint startY = 0;
            uint stopY = State.Board.GoalsHeight;
            if (State.TeamColour == Shared.Enums.TeamColour.Red)
            {
                startY = State.Board.Height - State.Board.GoalsHeight;
                stopY = State.Board.Height;
            }

            uint distance = uint.MaxValue;
            IGoalField bestGoal = null;

            for(uint x = 0; x < State.Board.Width; x++)
            {
                for(uint y = startY; y < stopY; y++)
                {
                    var goalField = State.Board.GetField(x, y) as IGoalField;
                    if (goalField.Type != Shared.Enums.GoalFieldType.Unknown)
                        continue;

                    uint localDistance = (uint)Math.Abs(x - State.Location.x) + (uint)Math.Abs(y - State.Location.y);
                    if(localDistance < distance)
                    {
                        distance = localDistance;
                        bestGoal = goalField;
                    }

                }
            }

            if (bestGoal.X < State.Location.x)
                return Shared.Enums.MoveType.Left;
            if (bestGoal.Y < State.Location.y)
                return Shared.Enums.MoveType.Down;
            if (bestGoal.X > State.Location.x)
                return Shared.Enums.MoveType.Right;
            if (bestGoal.Y > State.Location.y)
                return Shared.Enums.MoveType.Up;

            throw new Exception("Cannot localize the unknown goal");
        }

        public Data PerformAction()
        {
            IField field = State.Field;
            var goalField = field as IGoalField;
            var taskField = field as ITaskField;

            if (State.HoldingPiece == null) // Player does not hold a pece
            {
                if (goalField != null) // Player is standing in goal field
                {
                    if (State.TeamColour == Shared.Enums.TeamColour.Red) // Must go down
                    {
                        return GameMaster.PerformMove(SetCommunicationData(new Move
                        {
                            Direction = Shared.Enums.MoveType.Down,
                            DirectionSpecified = true
                        }));
                    }
                    else // Must go up
                    {
                        var data = GameMaster.PerformMove(SetCommunicationData(new Move
                        {
                            Direction = Shared.Enums.MoveType.Up,
                            DirectionSpecified = true
                        }));
                        DiscoveryCount = 0;
                        return data;
                    }
                }
                else // Player is standing in task field
                {
                    if (taskField.Piece != null) // Player is standing on piece
                    {
                        return GameMaster.PerformPickUp(SetCommunicationData(new PickUpPiece())); // Pick up piece
                    }
                    else
                    {
                        if(DiscoveryCount == 0) // Discover every 3 movements
                        {
                            var data = GameMaster.PerformDiscover(SetCommunicationData(new Discover()));
                            DiscoveryCount = DiscoveryCountMax;
                            return data;
                        }
                        DiscoveryCount--;

                        //Make movement towards piece

                        return GameMaster.PerformMove(SetCommunicationData(new Move
                        {
                            Direction = DirectionToPiece(),
                            DirectionSpecified = true
                        }));
                    }
                }

            }
            else // Player indeed holds a piece
            {
                if (State.HoldingPiece.type == Shared.Enums.PieceType.Unknown) // Piece state is unknown
                {
                    return GameMaster.PerformTestPiece(SetCommunicationData(new TestPiece()));
                }
                else if (State.HoldingPiece.type == Shared.Enums.PieceType.Sham) // Bad piece, put it back
                {
                    State.HoldingPiece = null;
                    return GameMaster.PerformPlace(SetCommunicationData(new PlacePiece()));
                }
                else // Piece is ok
                {
                    if(goalField != null && goalField.Type != Shared.Enums.GoalFieldType.NonGoal) // Player is standing in goal field
                    {
                        State.HoldingPiece = null;
                        if (goalField.Type != Shared.Enums.GoalFieldType.NonGoal)
                        {
                            return GameMaster.PerformPlace(SetCommunicationData(new PlacePiece()));
                        }
                        else return GameMaster.PerformMove(SetCommunicationData(new Move
                        {
                            Direction = DirectionToPiece(),
                            DirectionSpecified = true
                        }));
                    }
                    else if(goalField!=null && (goalField.Type == Shared.Enums.GoalFieldType.NonGoal))
                    {
                        return GameMaster.PerformMove(SetCommunicationData(new Move
                        {
                            Direction = DirectionToUnknownGoal(),
                            DirectionSpecified = true
                        }));
                    }
                    else if (taskField != null) // Player is standing in task field
                    {
                        if (State.TeamColour == Shared.Enums.TeamColour.Red) // Must go up
                        {
                            if (State.LastLocalization != State.Location)
                            {
                                return GameMaster.PerformMove(SetCommunicationData(new Move
                                {
                                    Direction = Shared.Enums.MoveType.Up,
                                    DirectionSpecified = true
                                }));
                                
                            }
                            else
                            {
                                return GameMaster.PerformMove(SetCommunicationData(new Move
                                {
                                    Direction = DirectionToPiece(),
                                    DirectionSpecified = true
                                }));
                            }
                        }
                        else // Must go down
                        {
                            if (State.LastLocalization != State.Location)
                            {
                                return GameMaster.PerformMove(SetCommunicationData(new Move
                                {
                                    Direction = Shared.Enums.MoveType.Down,
                                    DirectionSpecified = true
                                }));
                            }
                            else
                            {
                                return GameMaster.PerformMove(SetCommunicationData(new Move
                                {
                                    Direction = DirectionToPiece(),
                                    DirectionSpecified = true
                                }));
                            }
                        }
                    }
                    else // Must looking for unnknown goal field
                    {
                        return GameMaster.PerformMove(SetCommunicationData(new Move
                        {
                            Direction = DirectionToUnknownGoal(),
                            DirectionSpecified = true
                        }));
                    }

                }
            }
        }
    }
}

