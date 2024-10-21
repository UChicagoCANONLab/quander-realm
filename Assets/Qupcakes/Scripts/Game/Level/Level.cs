using System;
using System.Linq;
using System.Collections.Generic;
using CT = Qupcakery.GameCakeType;

/*
 * Level information : goal, time constraint, puzzle setup, solution
 * 
 * Puzzle specification index meaning:
 * 0 - |0>
 * 1 - |1>
 * 2 - |+>
 * 3 - |->
 * 4 - Same Entangled bit 0 
 * 5 - Same Entangled bit 1
 * 6 - Opposite Entangled bit 0
 * 7 - Opposite Entangled bit 1
 */

/*
 * Level 1 - NOT
 * Level 3 - SWAP
 * Level 8 - CNOT
 * Level 13 - H
 * Level 14 - H unwrap
 * Level 16 - Z
 * Level 23 - same entanglement
 * Level 24 - opposite entanglement
 */
namespace Qupcakery
{
    public class Level
    {
        public int LevelInd { get; private set; }
        public int Goal { get; private set; }
        public int TimeLimit { get; private set; }
        public int TotalBeltCnt { get; private set; }

        public int TotalPuzzleCnt { get; private set; }
        public Puzzle[] Puzzles { get; private set; }
        public int[] AvailableGates { get; private set; }

        public Tuple<int[], bool>[] Solutions { get; private set; }

        // Constructor
        public Level(int maxPuzzleCnt, int maxGateCnt)
        {
            LevelInd = 0;
            TotalPuzzleCnt = 0;
            Puzzles = new Puzzle[maxPuzzleCnt];
            for (int i = 0; i < maxPuzzleCnt; i++)
            {
                Puzzles[i] = new Puzzle(3);
            }
            AvailableGates = new int[maxGateCnt];

            Solutions = new Tuple<int[], bool>[] { new Tuple<int[], bool> (new int[] { 0 }, false) };
        }

        public void Update(int levelInd)
        {
            Array.Clear(AvailableGates, 0, AvailableGates.Length);
            LevelInd = levelInd;

            switch (levelInd)
            {
                case 1:
                    SetLevel1();
                    break;
                case 2:
                    SetLevel2();
                    break;
                case 3:
                    SetLevel3();
                    break;
                case 4:
                    SetLevel4();
                    break;
                case 5:
                    SetLevel5();
                    break;
                case 6:
                    SetLevel6();
                    break;
                case 7:
                    SetLevel7();
                    break;
                case 8:
                    SetLevel8();
                    break;
                case 9:
                    SetLevel9();
                    break;
                case 10:
                    SetLevel10();
                    break;
                case 11:
                    SetLevel11();
                    break;
                case 12:
                    SetLevel12();
                    break;
                case 13:
                    SetLevel13();
                    break;
                case 14:
                    SetLevel14();
                    break;
                case 15:
                    SetLevel15();
                    break;
                case 16:
                    SetLevel16();
                    break;
                case 17:
                    SetLevel17();
                    break;
                case 18:
                    SetLevel18();
                    break;
                case 19:
                    SetLevel19();
                    break;
                case 20:
                    SetLevel20();
                    break;
                case 21:
                    SetLevel21();
                    break;
                case 22:
                    SetLevel22();
                    break;
                case 23:
                    SetLevel23();
                    break;
                case 24:
                    SetLevel24();
                    break;
                case 25:
                    SetLevel25();
                    break;
                case 26:
                    SetLevel26();
                    break;
                case 27:
                    SetLevel27();
                    break;
                default:
                    throw new ArgumentException("Invalid level index: " + LevelInd);
            }
        }

        // Update level spec
        private void UpdateLevelSpec(int levelGoal, int levelTimeLimit, int levelTotalBeltCnt)
        {
            Goal = levelGoal;
            TimeLimit = levelTimeLimit;
            TotalBeltCnt = levelTotalBeltCnt;
        }

        private void SetLevel1()
        {
            UpdateLevelSpec(levelGoal: 60, levelTimeLimit: 50, levelTotalBeltCnt: 1);
            AvailableGates[(int)GateType.NOT] = 1;

            Puzzles[0].UpdatePuzzle(0, 0);
            Puzzles[1].UpdatePuzzle(0, 1);
            Puzzles[2].UpdatePuzzle(1, 0);
            Puzzles[3].UpdatePuzzle(1, 1);

            Puzzles[4].UpdatePuzzle(0, 1);
            Puzzles[5].UpdatePuzzle(0, 0);
            Puzzles[6].UpdatePuzzle(1, 0);
            Puzzles[7].UpdatePuzzle(1, 1);

            Solutions = new Tuple<int[], bool>[] {
                new Tuple<int[], bool>(new int[] { 0 }, false),
                new Tuple<int[], bool>(new int[] { 1 }, false),
                new Tuple<int[], bool>(new int[] { 1 }, false),
                new Tuple<int[], bool>(new int[] { 0 }, false),
                new Tuple<int[], bool>(new int[] { 1 }, false),
                new Tuple<int[], bool>(new int[] { 0 }, false),
                new Tuple<int[], bool>(new int[] { 1 }, false),
                new Tuple<int[], bool>(new int[] { 0 }, false)
            };

            TotalPuzzleCnt = 8;
        }

        private void SetLevel2()
        {
            UpdateLevelSpec(levelGoal: 100, levelTimeLimit: 60, levelTotalBeltCnt: 2);
            AvailableGates[(int)GateType.NOT] = 2;

            Puzzles[0].UpdatePuzzle(0, 0, 1, 0);
            Puzzles[1].UpdatePuzzle(1, 0, 0, 1);
            Puzzles[2].UpdatePuzzle(0, 1, 0, 0);
            Puzzles[3].UpdatePuzzle(1, 0, 1, 0);

            Puzzles[4].UpdatePuzzle(0, 1, 1, 1);
            Puzzles[5].UpdatePuzzle(0, 1, 0, 1);

            Solutions = new Tuple<int[], bool>[] {
                new Tuple<int[], bool>(new int[] { 1, 0 }, false),
                new Tuple<int[], bool>(new int[] { 1, 1 }, false),
                new Tuple<int[], bool>(new int[] { 0, 1 }, false),
                new Tuple<int[], bool>(new int[] { 1, 1 }, false),
                new Tuple<int[], bool>(new int[] { 0, 1 }, false),
                new Tuple<int[], bool>(new int[] { 1, 1 }, false)
            };

            TotalPuzzleCnt = 6;
        }

        private void SetLevel3()
        {
            UpdateLevelSpec(levelGoal: 100, levelTimeLimit: 60, levelTotalBeltCnt: 2);
            AvailableGates[(int)GateType.NOT] = 2;
            AvailableGates[(int)GateType.SWAP] = 1;

            Puzzles[0].UpdatePuzzle(0, 1, 1, 0);
            Puzzles[1].UpdatePuzzle(0, 1, 0, 1);
            Puzzles[2].UpdatePuzzle(1, 0, 1, 0);
            Puzzles[3].UpdatePuzzle(1, 0, 0, 1);

            Puzzles[4].UpdatePuzzle(1, 0, 1, 0);
            Puzzles[5].UpdatePuzzle(0, 1, 0, 1);

            Solutions = new Tuple<int[], bool>[] {
                new Tuple<int[], bool>(new int[] {11}, false),
                new Tuple<int[], bool>(new int[] {1, 1}, false),
                new Tuple<int[], bool>(new int[] {1, 1}, false),
                new Tuple<int[], bool>(new int[] {11}, false),
                new Tuple<int[], bool>(new int[] {1, 1}, false),
                new Tuple<int[], bool>(new int[] {1, 1}, false)
            };
            TotalPuzzleCnt = 6;
        }

        private void SetLevel4()
        {
            UpdateLevelSpec(levelGoal: 120, levelTimeLimit: 60, levelTotalBeltCnt: 3);
            AvailableGates[(int)GateType.NOT] = 1;
            AvailableGates[(int)GateType.SWAP] = 1;

            Puzzles[0].UpdatePuzzle(0, 1, 1, 0, 0, 1);
            Puzzles[1].UpdatePuzzle(1, 0, 1, 0, 0, 1);
            Puzzles[2].UpdatePuzzle(0, 1, 0, 1, 1, 0);
            Puzzles[3].UpdatePuzzle(0, 1, 1, 0, 1, 0);

            Puzzles[4].UpdatePuzzle(1, 0, 1, 0, 0, 1);

            Solutions = new Tuple<int[], bool>[] {
                new Tuple<int[], bool>(new int[] {1, 11}, false),
                new Tuple<int[], bool>(new int[] {11, 1}, false),
                new Tuple<int[], bool>(new int[] {11, 1}, false),
                new Tuple<int[], bool>(new int[] {1, 11}, false),
                new Tuple<int[], bool>(new int[] {11, 1}, false)
            };

            TotalPuzzleCnt = 5;

        }

        private void SetLevel5()
        {
            UpdateLevelSpec(levelGoal: 120, levelTimeLimit: 60, levelTotalBeltCnt: 3);
            AvailableGates[(int)GateType.NOT] = 2;
            AvailableGates[(int)GateType.SWAP] = 1;

            Puzzles[0].UpdatePuzzle(0, 1, 1, 1, 0, 1);
            Puzzles[1].UpdatePuzzle(0, 1, 1, 0, 0, 1);
            Puzzles[2].UpdatePuzzle(0, 0, 1, 0, 0, 1);
            Puzzles[3].UpdatePuzzle(1, 0, 0, 1, 1, 0);

            Puzzles[4].UpdatePuzzle(0, 1, 0, 1, 1, 0);

            Solutions = new Tuple<int[], bool>[] {
                new Tuple<int[], bool>(new int[] {1, 0, 1}, false),
                new Tuple<int[], bool>(new int[] {1, 11}, false),
                new Tuple<int[], bool>(new int[] {11, 0}, false),
                new Tuple<int[], bool>(new int[] {1, 11}, false),
                new Tuple<int[], bool>(new int[] {11, 1}, false)
            };

            TotalPuzzleCnt = 5;
        }

        private void SetLevel6()
        {
            UpdateLevelSpec(levelGoal: 150, levelTimeLimit: 60, levelTotalBeltCnt: 3);
            AvailableGates[(int)GateType.NOT] = 1;
            AvailableGates[(int)GateType.SWAP] = 1;

            Puzzles[0].UpdatePuzzle(0, 1, 1, 0, 0, 1);
            Puzzles[1].UpdatePuzzle(1, 0, 1, 0, 0, 1);
            Puzzles[2].UpdatePuzzle(0, 0, 1, 0, 0, 1);
            Puzzles[3].UpdatePuzzle(1, 0, 0, 1, 1, 0);

            Puzzles[4].UpdatePuzzle(0, 1, 1, 0, 1, 1);
            Puzzles[5].UpdatePuzzle(1, 1, 0, 1, 1, 0);

            Solutions = new Tuple<int[], bool>[] {
                new Tuple<int[], bool>(new int[] {1, 11}, false),
                new Tuple<int[], bool>(new int[] {11, 1}, false),
                new Tuple<int[], bool>(new int[] {11, 0}, false),
                new Tuple<int[], bool>(new int[] {1, 11}, false),
                new Tuple<int[], bool>(new int[] {0, 11}, false),
                new Tuple<int[], bool>(new int[] {11, 0}, false)
            };

            TotalPuzzleCnt = 6;
        }

        private void SetLevel7()
        {
            UpdateLevelSpec(levelGoal: 150, levelTimeLimit: 70, levelTotalBeltCnt: 3);
            AvailableGates[(int)GateType.SWAP] = 2;

            Puzzles[0].UpdatePuzzle(0, 1, 1, 1, 1, 0);
            Puzzles[1].UpdatePuzzle(1, 0, 1, 1, 0, 1);
            Puzzles[2].UpdatePuzzle(1, 0, 0, 1, 1, 1);
            Puzzles[3].UpdatePuzzle(1, 0, 1, 1, 0, 1);

            Puzzles[4].UpdatePuzzle(1, 0, 0, 0, 0, 1);

            Solutions = new Tuple<int[], bool>[] {
                new Tuple<int[], bool>(new int[] {101}, true),
                new Tuple<int[], bool>(new int[] {101}, false),
                new Tuple<int[], bool>(new int[] {0, 11}, false),
                new Tuple<int[], bool>(new int[] {101}, false),
                new Tuple<int[], bool>(new int[] {101}, true)
            };

            TotalPuzzleCnt = 5;
        }

        private void SetLevel8()
        {
            UpdateLevelSpec(levelGoal: 80, levelTimeLimit: 60, levelTotalBeltCnt: 2);
            AvailableGates[(int)GateType.CNOT] = 1;

            Puzzles[0].UpdatePuzzle(0, 1, 1, 1);
            Puzzles[1].UpdatePuzzle(1, 0, 1, 1);
            Puzzles[2].UpdatePuzzle(0, 1, 1, 1);
            Puzzles[3].UpdatePuzzle(1, 0, 1, 1);

            Solutions = new Tuple<int[], bool>[] {
                new Tuple<int[], bool>(new int[] {12}, false),
                new Tuple<int[], bool>(new int[] {12}, false),
                new Tuple<int[], bool>(new int[] {12}, false),
                new Tuple<int[], bool>(new int[] {12}, false)
            };

            TotalPuzzleCnt = 4;
        }

        // Allow clicking to swap channels 
        private void SetLevel9()
        {
            UpdateLevelSpec(levelGoal: 100, levelTimeLimit: 80, levelTotalBeltCnt: 2);
            AvailableGates[(int)GateType.CNOT] = 1;

            Puzzles[0].UpdatePuzzle(1, 1, 1, 0);
            Puzzles[1].UpdatePuzzle(1, 0, 1, 1);
            Puzzles[2].UpdatePuzzle(1, 1, 0, 1);
            Puzzles[3].UpdatePuzzle(1, 1, 1, 0);

            Puzzles[4].UpdatePuzzle(1, 0, 1, 1);
            Puzzles[5].UpdatePuzzle(1, 1, 0, 1);

            Solutions = new Tuple<int[], bool>[] {
                new Tuple<int[], bool>(new int[] {12}, true),
                new Tuple<int[], bool>(new int[] {12}, false),
                new Tuple<int[], bool>(new int[] {12}, true),
                new Tuple<int[], bool>(new int[] {12}, true),
                new Tuple<int[], bool>(new int[] {12}, false),
                new Tuple<int[], bool>(new int[] {12}, true)
            };


            TotalPuzzleCnt = 6;
        }

        private void SetLevel10()
        {
            UpdateLevelSpec(levelGoal: 120, levelTimeLimit: 100, levelTotalBeltCnt: 2);
            AvailableGates[(int)GateType.SWAP] = 1;
            AvailableGates[(int)GateType.CNOT] = 1;

            Puzzles[0].UpdatePuzzle(0, 1, 1, 1);
            Puzzles[1].UpdatePuzzle(1, 0, 1, 1);
            Puzzles[2].UpdatePuzzle(0, 1, 1, 0);
            Puzzles[3].UpdatePuzzle(1, 1, 0, 1);

            Puzzles[4].UpdatePuzzle(1, 0, 0, 1);
            Puzzles[5].UpdatePuzzle(1, 0, 1, 1);
            Puzzles[6].UpdatePuzzle(0, 1, 1, 1);

            Solutions = new Tuple<int[], bool>[] {
                new Tuple<int[], bool>(new int[] {12}, false),
                new Tuple<int[], bool>(new int[] {12}, false),
                new Tuple<int[], bool>(new int[] {11}, false),
                new Tuple<int[], bool>(new int[] {12}, true),
                new Tuple<int[], bool>(new int[] {11}, false),
                new Tuple<int[], bool>(new int[] {12}, false),
                new Tuple<int[], bool>(new int[] {12}, false)
            };

            TotalPuzzleCnt = 7;
        }

        private void SetLevel11()
        {
            UpdateLevelSpec(levelGoal: 150, levelTimeLimit: 100, levelTotalBeltCnt: 3);
            AvailableGates[(int)GateType.NOT] = 1;
            AvailableGates[(int)GateType.SWAP] = 2;
            AvailableGates[(int)GateType.CNOT] = 1;

            Puzzles[0].UpdatePuzzle(0, 1, 1, 0, 0, 1);
            Puzzles[1].UpdatePuzzle(1, 1, 0, 1, 1, 1);
            Puzzles[2].UpdatePuzzle(0, 1, 0, 1, 1, 0);
            Puzzles[3].UpdatePuzzle(1, 1, 1, 0, 1, 0);

            Puzzles[4].UpdatePuzzle(1, 0, 0, 1, 1, 0);

            Solutions = new Tuple<int[], bool>[] {
                new Tuple<int[], bool>(new int[] {1, 11}, false),
                new Tuple<int[], bool>(new int[] {0, 1, 0}, false),
                new Tuple<int[], bool>(new int[] {11, 1}, false),
                new Tuple<int[], bool>(new int[] {12, 0}, false),
                new Tuple<int[], bool>(new int[] {1, 11}, false)
            };

            TotalPuzzleCnt = 5;
        }

        private void SetLevel12()
        {
            UpdateLevelSpec(levelGoal: 150, levelTimeLimit: 100, levelTotalBeltCnt: 3);
            AvailableGates[(int)GateType.SWAP] = 1;
            AvailableGates[(int)GateType.CNOT] = 2;

            Puzzles[0].UpdatePuzzle(1, 1, 0, 1, 1, 1);
            Puzzles[1].UpdatePuzzle(0, 1, 1, 0, 0, 1);
            Puzzles[2].UpdatePuzzle(1, 0, 0, 1, 1, 0);
            Puzzles[3].UpdatePuzzle(0, 1, 0, 1, 1, 0);

            Puzzles[4].UpdatePuzzle(1, 1, 1, 0, 1, 0);

            Solutions = new Tuple<int[], bool>[] {
                new Tuple<int[], bool>(new int[] {12, 0}, false),
                new Tuple<int[], bool>(new int[] {102}, true),
                new Tuple<int[], bool>(new int[] {103}, true),
                new Tuple<int[], bool>(new int[] {103}, true),
                new Tuple<int[], bool>(new int[] {104}, true)
            };

            TotalPuzzleCnt = 5;
        }

        private void SetLevel13()
        {
            UpdateLevelSpec(levelGoal: 60, levelTimeLimit: 40, levelTotalBeltCnt: 1);
            AvailableGates[(int)GateType.H] = 1;

            Puzzles[0].UpdatePuzzle(1, 3);
            Puzzles[1].UpdatePuzzle(0, 2);
            Puzzles[2].UpdatePuzzle(1, 2);
            Puzzles[3].UpdatePuzzle(0, 3);

            Solutions = new Tuple<int[], bool>[] {
                new Tuple<int[], bool>(new int[] {2}, false),
                new Tuple<int[], bool>(new int[] {2}, false),
                new Tuple<int[], bool>(new int[] {2}, false),
                new Tuple<int[], bool>(new int[] {2}, false)
            };

            TotalPuzzleCnt = 4;
        }

        // Use H to unwrap
        private void SetLevel14()
        {
            UpdateLevelSpec(levelGoal: 60, levelTimeLimit: 60, levelTotalBeltCnt: 1);
            // UpdateLevelSpec(levelGoal: 80, levelTimeLimit: 60, levelTotalBeltCnt: 1);
            AvailableGates[(int)GateType.NOT] = 1;
            AvailableGates[(int)GateType.H] = 1;

            Puzzles[0].UpdatePuzzle(3, 1);
            Puzzles[1].UpdatePuzzle(3, 0);
            Puzzles[2].UpdatePuzzle(2, 1);
            Puzzles[3].UpdatePuzzle(2, 0);

            Solutions = new Tuple<int[], bool>[] {
                new Tuple<int[], bool>(new int[] {2}, false),
                new Tuple<int[], bool>(new int[] {4}, false),
                new Tuple<int[], bool>(new int[] {4}, false),
                new Tuple<int[], bool>(new int[] {2}, false)
            };

            TotalPuzzleCnt = 4;
        }

        private void SetLevel15()
        {
            UpdateLevelSpec(levelGoal: 80, levelTimeLimit: 80, levelTotalBeltCnt: 2);
            AvailableGates[(int)GateType.SWAP] = 1;
            AvailableGates[(int)GateType.H] = 2;

            Puzzles[0].UpdatePuzzle(3, 1, 0, 0);
            Puzzles[1].UpdatePuzzle(2, 0, 3, 1);
            Puzzles[2].UpdatePuzzle(0, 2, 2, 0);
            Puzzles[3].UpdatePuzzle(3, 0, 2, 1);

            Puzzles[4].UpdatePuzzle(2, 0, 1, 3);
            Puzzles[5].UpdatePuzzle(1, 3, 3, 1);
            Puzzles[6].UpdatePuzzle(2, 0, 1, 3);
            Puzzles[7].UpdatePuzzle(2, 1, 3, 0);

            Solutions = new Tuple<int[], bool>[] {
                new Tuple<int[], bool>(new int[] {0, 2}, false),
                new Tuple<int[], bool>(new int[] {2, 2}, false),
                new Tuple<int[], bool>(new int[] {11}, false),
                new Tuple<int[], bool>(new int[] {14}, false),

                new Tuple<int[], bool>(new int[] {2, 2}, false),
                new Tuple<int[], bool>(new int[] {11}, false),
                new Tuple<int[], bool>(new int[] {2, 2}, false),
                new Tuple<int[], bool>(new int[] {14}, false)
            };

            TotalPuzzleCnt = 8;
        }

        // Introduce Z gate
        private void SetLevel16()
        {
            UpdateLevelSpec(levelGoal: 50, levelTimeLimit: 60, levelTotalBeltCnt: 1);
            AvailableGates[(int)GateType.H] = 1;
            AvailableGates[(int)GateType.Z] = 1;

            Puzzles[0].UpdatePuzzle(3, 0);
            Puzzles[1].UpdatePuzzle(2, 0);
            Puzzles[2].UpdatePuzzle(2, 1);
            Puzzles[3].UpdatePuzzle(2, 0);

            Puzzles[4].UpdatePuzzle(0, 2);
            Puzzles[5].UpdatePuzzle(0, 3);
            Puzzles[6].UpdatePuzzle(3, 0);
            Puzzles[7].UpdatePuzzle(2, 0);

            Solutions = new Tuple<int[], bool>[] {
                new Tuple<int[], bool>(new int[] {5}, false),
                new Tuple<int[], bool>(new int[] {2}, false),
                new Tuple<int[], bool>(new int[] {5}, false),
                new Tuple<int[], bool>(new int[] {2}, false),

                new Tuple<int[], bool>(new int[] {2}, false),
                new Tuple<int[], bool>(new int[] {2}, false),
                new Tuple<int[], bool>(new int[] {5}, false),
                new Tuple<int[], bool>(new int[] {2}, false)
            };

            TotalPuzzleCnt = 8;
        }

        private void SetLevel17()
        {
            UpdateLevelSpec(levelGoal: 100, levelTimeLimit: 70, levelTotalBeltCnt: 2);
            AvailableGates[(int)GateType.NOT] = 1;
            AvailableGates[(int)GateType.H] = 2;
            AvailableGates[(int)GateType.Z] = 2;

            Puzzles[0].UpdatePuzzle(3, 0, 0, 1);
            Puzzles[1].UpdatePuzzle(2, 0, 3, 0);
            Puzzles[2].UpdatePuzzle(0, 3, 2, 0);
            Puzzles[3].UpdatePuzzle(3, 0, 2, 1);

            Puzzles[4].UpdatePuzzle(1, 0, 1, 0);
            Puzzles[5].UpdatePuzzle(0, 1, 2, 0);
            Puzzles[6].UpdatePuzzle(2, 1, 3, 0);
            Puzzles[7].UpdatePuzzle(0, 3, 1, 2);

            Solutions = new Tuple<int[], bool>[] {
                new Tuple<int[], bool>(new int[] {1, 5}, false),
                new Tuple<int[], bool>(new int[] {5, 2}, false),
                new Tuple<int[], bool>(new int[] {2, 2}, false),
                new Tuple<int[], bool>(new int[] {5, 5}, false),

                new Tuple<int[], bool>(new int[] {1, 6}, false),
                new Tuple<int[], bool>(new int[] {2, 1}, false),
                new Tuple<int[], bool>(new int[] {5, 5}, false),
                new Tuple<int[], bool>(new int[] {2, 2}, false)
            };

            TotalPuzzleCnt = 8;
        }

        private void SetLevel18()
        {
            UpdateLevelSpec(levelGoal: 150, levelTimeLimit: 90, levelTotalBeltCnt: 3);
            AvailableGates[(int)GateType.SWAP] = 1;
            AvailableGates[(int)GateType.H] = 3;
            AvailableGates[(int)GateType.Z] = 2;

            Puzzles[0].UpdatePuzzle(1, 0, 0, 1, 0, 2);
            Puzzles[1].UpdatePuzzle(1, 2, 3, 1, 1, 1);
            Puzzles[2].UpdatePuzzle(0, 3, 2, 3, 2, 1);
            Puzzles[3].UpdatePuzzle(1, 2, 0, 3, 2, 1);

            Puzzles[4].UpdatePuzzle(0, 3, 0, 2, 1, 0);

            Solutions = new Tuple<int[], bool>[] {
                new Tuple<int[], bool>(new int[] {2, 11}, false),
                new Tuple<int[], bool>(new int[] {0, 11}, false),
                new Tuple<int[], bool>(new int[] {5, 0, 2}, false),
                new Tuple<int[], bool>(new int[] {5, 2, 2}, false),
                new Tuple<int[], bool>(new int[] {16, 2}, false)
            };

            TotalPuzzleCnt = 5;
        }

        private void SetLevel19()
        {
            UpdateLevelSpec(levelGoal: 100, levelTimeLimit: 80, levelTotalBeltCnt: 2);
            AvailableGates[(int)GateType.SWAP] = 1;
            AvailableGates[(int)GateType.H] = 2;
            AvailableGates[(int)GateType.Z] = 2;

            Puzzles[0].UpdatePuzzle(3, 2, 0, 3);
            Puzzles[1].UpdatePuzzle(2, 1, 3, 1);
            Puzzles[2].UpdatePuzzle(0, 1, 3, 0);
            Puzzles[3].UpdatePuzzle(0, 3, 2, 3);

            Puzzles[4].UpdatePuzzle(1, 2, 0, 3);
            Puzzles[5].UpdatePuzzle(0, 3, 0, 2);
            Puzzles[6].UpdatePuzzle(3, 0, 2, 3);

            Solutions = new Tuple<int[], bool>[] {
                new Tuple<int[], bool>(new int[] {2, 0}, false),
                new Tuple<int[], bool>(new int[] {2, 5}, false),
                new Tuple<int[], bool>(new int[] {16}, false),
                new Tuple<int[], bool>(new int[] {0, 2}, false),

                new Tuple<int[], bool>(new int[] {2, 2}, false),
                new Tuple<int[], bool>(new int[] {2, 2}, false),
                new Tuple<int[], bool>(new int[] {0, 5}, false)
            };

            TotalPuzzleCnt = 7;
        }
       

        private void SetLevel20()
        {
            UpdateLevelSpec(levelGoal: 80, levelTimeLimit: 70, levelTotalBeltCnt: 2);
            AvailableGates[(int)GateType.H] = 3;
            AvailableGates[(int)GateType.Z] = 2;

            Puzzles[0].UpdatePuzzle(3, 0, 2, 0);
            Puzzles[1].UpdatePuzzle(0, 1, 1, 2);
            Puzzles[2].UpdatePuzzle(0, 2, 1, 0);
            Puzzles[3].UpdatePuzzle(1, 3, 2, 3);

            Puzzles[4].UpdatePuzzle(3, 1, 1, 0);
            Puzzles[5].UpdatePuzzle(0, 3, 0, 2);
            Puzzles[6].UpdatePuzzle(0, 1, 3, 0);

            Solutions = new Tuple<int[], bool>[] {
                new Tuple<int[], bool>(new int[] {2, 5}, false),
                new Tuple<int[], bool>(new int[] {2, 6}, false),
                new Tuple<int[], bool>(new int[] {6, 2}, false),
                new Tuple<int[], bool>(new int[] {0, 2}, false),

                new Tuple<int[], bool>(new int[] {6, 2}, false),
                new Tuple<int[], bool>(new int[] {2, 2}, false),
                new Tuple<int[], bool>(new int[] {5, 6}, false)
            };

            TotalPuzzleCnt = 7;
        }

        private void SetLevel21()
        {
            UpdateLevelSpec(levelGoal: 100, levelTimeLimit: 80, levelTotalBeltCnt: 3);
            AvailableGates[(int)GateType.NOT] = 2;
            AvailableGates[(int)GateType.H] = 3;
            AvailableGates[(int)GateType.Z] = 2;

            Puzzles[0].UpdatePuzzle(1, 0, 0, 1, 0, 1);
            Puzzles[1].UpdatePuzzle(1, 0, 3, 0, 1, 0);
            Puzzles[2].UpdatePuzzle(2, 1, 3, 1, 2, 0);
            Puzzles[3].UpdatePuzzle(0, 1, 0, 1, 0, 1);

            Puzzles[4].UpdatePuzzle(1, 2, 0, 3, 2, 1);
            Puzzles[5].UpdatePuzzle(0, 3, 0, 2, 1, 0);

            Solutions = new Tuple<int[], bool>[] {
                new Tuple<int[], bool>(new int[] {1, 1, 6}, false),
                new Tuple<int[], bool>(new int[] {1, 5, 1}, false),
                new Tuple<int[], bool>(new int[] {2, 2, 5}, false),
                new Tuple<int[], bool>(new int[] {1, 1, 6}, false),

                new Tuple<int[], bool>(new int[] {5, 2, 2}, false),
                new Tuple<int[], bool>(new int[] {1, 2, 2}, false)
            };

            TotalPuzzleCnt = 6;
        }

        private void SetLevel22()
        {
            UpdateLevelSpec(levelGoal: 120, levelTimeLimit: 80, levelTotalBeltCnt: 3);
            AvailableGates[(int)GateType.SWAP] = 1;
            AvailableGates[(int)GateType.H] = 2;
            AvailableGates[(int)GateType.Z] = 1;

            Puzzles[0].UpdatePuzzle(0, 1, 1, 0, 1, 0);
            Puzzles[1].UpdatePuzzle(1, 3, 3, 1, 1, 0);
            Puzzles[2].UpdatePuzzle(2, 1, 1, 0, 0, 1);
            Puzzles[3].UpdatePuzzle(0, 2, 0, 0, 3, 0);

            Puzzles[4].UpdatePuzzle(0, 1, 1, 0, 0, 1);
            Puzzles[5].UpdatePuzzle(0, 3, 2, 2, 2, 1);

            Solutions = new Tuple<int[], bool>[] {
                new Tuple<int[], bool>(new int[] {6, 11}, false),
                new Tuple<int[], bool>(new int[] {6, 11}, false),
                new Tuple<int[], bool>(new int[] {11, 5}, false),
                new Tuple<int[], bool>(new int[] {5, 0, 2}, false),

                new Tuple<int[], bool>(new int[] {6, 11}, false),
                new Tuple<int[], bool>(new int[] {5, 0, 2}, false)
            };

            TotalPuzzleCnt = 6;
        }

        // Introduce same entanglement
        private void SetLevel23()
        {
            UpdateLevelSpec(levelGoal: 100, levelTimeLimit: 60, levelTotalBeltCnt: 2);
            AvailableGates[(int)GateType.NOT] = 1;
            AvailableGates[(int)GateType.CNOT] = 1;
            AvailableGates[(int)GateType.H] = 1;

            Puzzles[0].UpdatePuzzle(0, 4, 0, 5);
            Puzzles[1].UpdatePuzzle(0, 4, 1, 5);
            Puzzles[2].UpdatePuzzle(1, 3, 0, 1);
            Puzzles[3].UpdatePuzzle(1, 4, 0, 5);

            Puzzles[4].UpdatePuzzle(0, 4, 0, 5);
            Puzzles[5].UpdatePuzzle(1, 4, 0, 5);
            Puzzles[6].UpdatePuzzle(1, 4, 1, 5);

            Solutions = new Tuple<int[], bool>[] {
                new Tuple<int[], bool>(new int[] {17}, false),
                new Tuple<int[], bool>(new int[] {17}, false),
                new Tuple<int[], bool>(new int[] {1, 2}, false),
                new Tuple<int[], bool>(new int[] {17}, true),

                new Tuple<int[], bool>(new int[] {17}, false),
                new Tuple<int[], bool>(new int[] {17}, true),
                new Tuple<int[], bool>(new int[] {18}, true)
            };

            TotalPuzzleCnt = 7;
        }

        // Introduce opposite entanglement
        private void SetLevel24()
        {
            UpdateLevelSpec(levelGoal: 100, levelTimeLimit: 60, levelTotalBeltCnt: 2);
            AvailableGates[(int)GateType.NOT] = 1;
            AvailableGates[(int)GateType.CNOT] = 1;
            AvailableGates[(int)GateType.H] = 1;

            Puzzles[0].UpdatePuzzle(1, 6, 0, 7);
            Puzzles[1].UpdatePuzzle(0, 6, 1, 7);
            Puzzles[2].UpdatePuzzle(1, 3, 0, 1);
            Puzzles[3].UpdatePuzzle(1, 6, 1, 7);

            Puzzles[4].UpdatePuzzle(1, 6, 0, 7);
            Puzzles[5].UpdatePuzzle(0, 6, 1, 7);

            Solutions = new Tuple<int[], bool>[] {
                new Tuple<int[], bool>(new int[] {17}, false),
                new Tuple<int[], bool>(new int[] {17}, true),
                new Tuple<int[], bool>(new int[] {1, 2}, false),
                new Tuple<int[], bool>(new int[] {17}, false),

                new Tuple<int[], bool>(new int[] {17}, false),
                new Tuple<int[], bool>(new int[] {17}, true)
            };

            TotalPuzzleCnt = 6;
        }

        // Not can be placed after entanglement
        private void SetLevel25()
        {
            UpdateLevelSpec(levelGoal: 100, levelTimeLimit: 100, levelTotalBeltCnt: 2);
            AvailableGates[(int)GateType.NOT] = 1;
            AvailableGates[(int)GateType.CNOT] = 1;
            AvailableGates[(int)GateType.H] = 1;

            Puzzles[0].UpdatePuzzle(1, 4, 0, 5);
            Puzzles[1].UpdatePuzzle(0, 6, 1, 7);
            Puzzles[2].UpdatePuzzle(1, 0, 1, 0);
            Puzzles[3].UpdatePuzzle(1, 6, 0, 7);

            Puzzles[4].UpdatePuzzle(0, 4, 0, 5);
            Puzzles[5].UpdatePuzzle(1, 6, 1, 7);
            Puzzles[6].UpdatePuzzle(0, 1, 0, 1);

            Solutions = new Tuple<int[], bool>[] {
                new Tuple<int[], bool>(new int[] {17}, true),
                new Tuple<int[], bool>(new int[] {17}, true),
                new Tuple<int[], bool>(new int[] {19}, false),
                new Tuple<int[], bool>(new int[] {17}, false),

                new Tuple<int[], bool>(new int[] {17}, false),
                new Tuple<int[], bool>(new int[] {17}, false),
                new Tuple<int[], bool>(new int[] {20}, false)
            };

            TotalPuzzleCnt = 7;
        }

        private void SetLevel26()
        {
            UpdateLevelSpec(levelGoal: 120, levelTimeLimit: 100, levelTotalBeltCnt: 3);
            AvailableGates[(int)GateType.NOT] = 2;
            AvailableGates[(int)GateType.CNOT] = 1;
            AvailableGates[(int)GateType.SWAP] = 1;
            AvailableGates[(int)GateType.H] = 1;

            Puzzles[0].UpdatePuzzle(0, 6, 0, 1, 1, 7);
            Puzzles[1].UpdatePuzzle(1, 0, 0, 4, 1, 5);
            Puzzles[2].UpdatePuzzle(1, 4, 0, 5, 0, 1);
            Puzzles[3].UpdatePuzzle(1, 6, 1, 7, 0, 1);

            Puzzles[4].UpdatePuzzle(1, 6, 1, 0, 0, 7);
            Puzzles[5].UpdatePuzzle(0, 4, 1, 5, 1, 0);

            Solutions = new Tuple<int[], bool>[] {
                new Tuple<int[], bool>(new int[] {105}, true),
                new Tuple<int[], bool>(new int[] {17, 1}, false),
                new Tuple<int[], bool>(new int[] {1, 17}, true),
                new Tuple<int[], bool>(new int[] {1, 17}, true),

                new Tuple<int[], bool>(new int[] {107}, true),
                new Tuple<int[], bool>(new int[] {1, 17}, false)
            };

            TotalPuzzleCnt = 6;
        }

        private void SetLevel27()
        {
            UpdateLevelSpec(levelGoal: 120, levelTimeLimit: 100, levelTotalBeltCnt: 3);
            AvailableGates[(int)GateType.NOT] = 1;
            AvailableGates[(int)GateType.CNOT] = 1;
            AvailableGates[(int)GateType.H] = 1;
            AvailableGates[(int)GateType.SWAP] = 1;
            AvailableGates[(int)GateType.Z] = 1;

            Puzzles[0].UpdatePuzzle(1, 6, 0, 1, 1, 7);
            Puzzles[1].UpdatePuzzle(1, 0, 0, 1, 1, 3);
            Puzzles[2].UpdatePuzzle(1, 1, 0, 1, 0, 1);
            Puzzles[3].UpdatePuzzle(1, 4, 1, 5, 0, 1);

            Puzzles[4].UpdatePuzzle(1, 6, 0, 1, 0, 7);
            Puzzles[5].UpdatePuzzle(0, 1, 1, 1, 1, 0);

            Solutions = new Tuple<int[], bool>[] {
                new Tuple<int[], bool>(new int[] {108}, false),
                new Tuple<int[], bool>(new int[] {2, 11}, false),
                new Tuple<int[], bool>(new int[] {20, 0}, false),
                new Tuple<int[], bool>(new int[] {109}, true),

                new Tuple<int[], bool>(new int[] {110}, false),
                new Tuple<int[], bool>(new int[] {1, 12}, false)
            };

            TotalPuzzleCnt = 6;
        }
    }
}

