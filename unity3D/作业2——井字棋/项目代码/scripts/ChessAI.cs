using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessAI 
{
    // the position AI will drop chess
    private int[,] AI_chess = new int[3, 3];
    public ChessAI()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                AI_chess[i, j] = 0;
            }
        }
    }

    // 取得当前局面下AI的最好走步方式
    public int[] ChessAIMakePosition(Chessboard chessboard, Player chessai, Player player, int order)
    {
        int[,] board = chessboard.GetBoard();
        // 存储AI走棋的下标
        int[] AI_position = new int[2];
        // 如果AI为首先走棋的并且为首局，则AI随便选择一个位置下子
        if (chessai.GetFirstDrop() && order == 0)
        {
            AI_position = this.GetRandomPosition();
        }
        else
        {
            int AIchesstype = chessai.GetChessType();
            int playerchesstype = player.GetChessType();
            AI_position = ChessAIMakePosition2(board, AIchesstype, playerchesstype);
        }
        return AI_position;
    }

    public int[] ChessAIMakePosition2(int[,] chessboard, int AIchesstype, int playerchesstype)
    {
        // 存放不同位置的分数
        int[,] score = new int[3, 3];
        // 存放不同情况的分数
        int[] lineweight = new int[8];
        // 分数分为3档
        const int score0 = -50;
        const int score1 = 0;
        const int score2 = 60;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                // 如果棋盘当前位置为AI的棋子,当前位置分数设为最高
                if (chessboard[i, j] == AIchesstype)
                {
                    score[i, j] = score2;
                }
                // 如果为玩家棋子，则分数设为最低
                else if (chessboard[i, j] == playerchesstype)
                {
                    score[i, j] = score0;
                }
                // 如果没有棋子，则分数设为0
                else
                {
                    score[i, j] = score1;
                }
            }
        }

        /*
              0          1          2
            X X X      . . .      . . .
            . . .  OR  X X X  OR  . . .
            . . .      . . .      X X X
        */
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                lineweight[i] += score[i, j];
            }
        }
        /*
              3          4          5
            X . .      . X .      . . X
            X . .  OR  . X .  OR  . . X
            X . .      . X .      . . X
        */
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                lineweight[i + 3] += score[j, i];
            }
        }
        /*
              6
            X . .   
            . X .  
            . . X 
        */
        lineweight[6] = score[0, 0] + score[1, 1] + score[2, 2];
        /*
              7
            . . X   
            . X .  
            X . . 
        */
        lineweight[7] = score[0, 2] + score[1, 1] + score[2, 0];

        // 在不同位置上的棋子可以胜利的情况最大值，例如在[0,0]位置的棋子可以走情况0，3，6
        AI_chess[0, 0] = System.Math.Max(System.Math.Max(System.Math.Abs(lineweight[0]), System.Math.Abs(lineweight[3])), System.Math.Abs(lineweight[6]));
        AI_chess[0, 1] = System.Math.Max(System.Math.Abs(lineweight[0]), System.Math.Abs(lineweight[4]));
        AI_chess[0, 2] = System.Math.Max(System.Math.Max(System.Math.Abs(lineweight[0]), System.Math.Abs(lineweight[5])), System.Math.Abs(lineweight[7]));

        AI_chess[1, 0] = System.Math.Max(System.Math.Abs(lineweight[1]), System.Math.Abs(lineweight[3]));
        AI_chess[1, 1] = System.Math.Max(System.Math.Max(System.Math.Max(System.Math.Abs(lineweight[1]), System.Math.Abs(lineweight[4])), System.Math.Abs(lineweight[6])), System.Math.Abs(lineweight[7]));
        AI_chess[1, 2] = System.Math.Max(System.Math.Abs(lineweight[1]), System.Math.Abs(lineweight[5]));

        AI_chess[2, 0] = System.Math.Max(System.Math.Max(System.Math.Abs(lineweight[2]), System.Math.Abs(lineweight[3])), System.Math.Abs(lineweight[7]));
        AI_chess[2, 1] = System.Math.Max(System.Math.Abs(lineweight[2]), System.Math.Abs(lineweight[4]));
        AI_chess[2, 2] = System.Math.Max(System.Math.Max(System.Math.Abs(lineweight[2]), System.Math.Abs(lineweight[3])), System.Math.Abs(lineweight[6]));

        // 如果当前位置已经有棋子，则分数设置为0
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (chessboard[i, j] == AIchesstype)
                {
                    AI_chess[i, j] = 0;
                }
                else if (chessboard[i, j] == playerchesstype)
                {
                    AI_chess[i, j] = 0;
                }
            }
        }
        // 找到分数最大值的位置
        int max = 0, x = 0, y = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int flag = Random.Range(0, 2);
                if (flag == 0)
                {
                    if (AI_chess[i, j] > max)
                    {
                        max = AI_chess[i, j];
                        x = i;
                        y = j;
                    }
                }
                else
                {
                    if (AI_chess[i, j] >= max)
                    {
                        max = AI_chess[i, j];
                        x = i;
                        y = j;
                    }
                }
            }
        }
        int[] XY = new int[2];
        XY[0] = x;
        XY[1] = y;
        return XY;
    }

    // 随机得到位置
    private int[] GetRandomPosition()
    {
        int[] Position = new int[2];
        // 随机生成数字
        Position[0] = Random.Range(0, 3);
        Position[1] = Random.Range(0, 3);
        return Position;
    }

    // 打印当前棋盘
    public string PrintAIChess()
    {
        string info = "The AIChess\n";
        for (int i = 0; i< 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                info += AI_chess[i, j].ToString() + " ";
            }
            info += "\n";
        }
        return info;
    }
}