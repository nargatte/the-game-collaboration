﻿using Shared.Components.Fields;
using Shared.Interfaces;
using Shared.Messages.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerCore
{
    class Strategy
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
            gameMessage.gameId = State.GameId;
            gameMessage.playerGuid = State.Guid;
            return gameMessage;
        }

        const int DiscoveryCountMax = 3;
        private int DiscoveryCount = 0;

        private Shared.Enums.MoveType DirectionToPiece()
        {
            var fieldsSortedByTime = State.Board.Fields.OrderByDescending(f => f.Timestamp).OfType<ITaskField>();
            //foreach (var taskField in fieldsSortedByTime)
            //{
            //    if (taskField.Timestamp == DateTime.MinValue)
            //        break;
            //    Console.WriteLine(taskField.Timestamp.Ticks + "  " + taskField.Timestamp.ToString());
            //}
            var lastChcekFields = fieldsSortedByTime.Where(f => f.Timestamp.Ticks >= fieldsSortedByTime.FirstOrDefault().Timestamp.Ticks - 100000000);
            if (lastChcekFields.Count() < 4)
                throw new Exception("To little descoverd fields");

            uint max_x = lastChcekFields.Max(f => f.X);
            uint max_y = lastChcekFields.Max(f => f.Y);
            uint min_x = lastChcekFields.Min(f => f.X);
            uint min_y = lastChcekFields.Min(f => f.Y);

            IField field = lastChcekFields.OrderBy(f => f.DistanceToPiece).FirstOrDefault();

            if (max_x == field.X)
                return Shared.Enums.MoveType.Right;
            if (max_y == field.Y)
                return Shared.Enums.MoveType.Up;
            if (min_x == field.X)
                return Shared.Enums.MoveType.Left;
            if (min_y == field.Y)
                return Shared.Enums.MoveType.Down;

            throw new Exception("Cannot make movement dccidion");
        }

        private Shared.Enums.MoveType DirectionToUnknownGoal()
        {
            uint startY = 0;
            uint stopY = State.Board.GoalsHeight;
            if (State.TeamColour == Shared.Enums.TeamColour.Blue)
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

            throw new Exception("Cannot localizate unknown goal");
        }

        public Data PerformAction()
        {
            IField field = State.Field;
            var goalField = field as IGoalField;
            var taskField = field as ITaskField;

            if (State.HoldingPiece == null) // Player does not hold pece
            {
                if (goalField != null) // Player is standing in goal field
                {
                    if (State.TeamColour == Shared.Enums.TeamColour.Red) // Must go down
                    {
                        return GameMaster.PerformMove(SetCommunicationData(new Move
                        {
                            direction = Shared.Enums.MoveType.Down,
                            directionSpecified = true
                        }));
                    }
                    else // Must go up
                    {
                        var data = GameMaster.PerformMove(SetCommunicationData(new Move
                        {
                            direction = Shared.Enums.MoveType.Up,
                            directionSpecified = true
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
                            direction = DirectionToPiece(),
                            directionSpecified = true
                        }));
                    }
                }

            }
            else // Player hold piece
            {
                if (State.HoldingPiece.type == Shared.Enums.PieceType.Unknown) // Piece state is unknown
                {
                    return GameMaster.PerformTestPiece(SetCommunicationData(new TestPiece()));
                }
                else if (State.HoldingPiece.type == Shared.Enums.PieceType.Sham) // Bad piece, put it back
                {
                    return GameMaster.PerformPlace(SetCommunicationData(new PlacePiece()));
                }
                else // Piece is ok
                {
                    if(goalField != null && goalField.Type == Shared.Enums.GoalFieldType.Unknown) // Player is standing in goal field
                    {
                        return GameMaster.PerformPlace(SetCommunicationData(new PlacePiece()));
                    }
                    else if (taskField != null) // Player is standing in task field
                    {
                        if (State.TeamColour == Shared.Enums.TeamColour.Red) // Must go up
                        {
                            return GameMaster.PerformMove(SetCommunicationData(new Move
                            {
                                direction = Shared.Enums.MoveType.Up,
                                directionSpecified = true
                            }));
                        }
                        else // Must go down
                        {
                            return GameMaster.PerformMove(SetCommunicationData(new Move
                            {
                                direction = Shared.Enums.MoveType.Down,
                                directionSpecified = true
                            }));
                        }
                    }
                    else // Must looking for unnknown goal field
                    {
                        return GameMaster.PerformMove(SetCommunicationData(new Move
                        {
                            direction = DirectionToUnknownGoal(),
                            directionSpecified = true
                        }));
                    }

                }
            }
        }
    }
}

